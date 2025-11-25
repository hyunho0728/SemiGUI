using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        // --- 시스템 상태 변수 ---
        private bool isLoggedIn = false;
        private bool isAutoRun = false; // 자동 운전 모드 여부

        // --- 시뮬레이션용 타이머 ---
        private Timer clockTimer; // 시계용
        private Timer sysTimer;   // 장비 제어 스케줄러 (100ms)

        // --- 데이터 모델 ---
        private int foupACount = 25; // FOUP A 웨이퍼 수
        private int foupBCount = 0;  // FOUP B 웨이퍼 수

        // 모듈 상태 (0: Idle/Empty, 1: Processing, 2: Complete/Wait_Out)
        private int statusPmA = 0;
        private int statusPmB = 0;
        private int statusPmC = 0;

        // 공정 진행률 (0~100)
        private double progressA = 0;
        private double progressB = 0;
        private double progressC = 0;

        // 레시피 설정값 (초 단위 Time) - 기본값
        private int timePmA = 5;
        private int timePmB = 5;
        private int timePmC = 5;

        // 로봇 애니메이션 변수
        private float robotAngle = 0; // 현재 각도
        private float targetAngle = 0; // 목표 각도
        private bool isRobotMoving = false;
        private bool robotHasWafer = false; // 로봇이 웨이퍼를 들고 있는지
        private string robotDestination = ""; // 로봇의 현재 목적지 (PMA, PMB...)
        private string robotSource = "";      // 어디서 가져오는지

        // 모듈별 각도 정의 (GDI+ 좌표계 기준: 0=우측, 90=하단, 180=좌측, 270=상단)
        private const float ANG_PMC = 0;
        private const float ANG_FOUP_B = 45;
        private const float ANG_FOUP_A = 135;
        private const float ANG_PMA = 180;
        private const float ANG_PMB = 270;

        public Form1()
        {
            InitializeComponent();

            // 1920x1080 고정
            this.Size = new Size(1920, 1080);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            SetupLogic();

            // 이벤트 연결
            this.btnMain.Click += BtnMain_Click;
            this.btnRecipe.Click += BtnRecipe_Click;
            this.btnLog.Click += BtnLog_Click;
            this.btnLogin.Click += BtnLogin_Click;

            // CONNECT 버튼을 'AUTO RUN' 시작 버튼으로 활용
            this.btnConnect.Click += (s, e) => ToggleAutoRun();

            // FOUP Load/Unload (수동 조작)
            this.btnLoadA.Click += (s, e) => { foupACount = 25; UpdateWaferUI(); };
            this.btnUnloadA.Click += (s, e) => { foupACount = 0; UpdateWaferUI(); };
            this.btnLoadB.Click += (s, e) => { foupBCount = 25; UpdateWaferUI(); };
            this.btnUnloadB.Click += (s, e) => { foupBCount = 0; UpdateWaferUI(); };

            this.pnlCenter.SizeChanged += (s, e) => UpdateLayout();

            SetLoginState(false);
            UpdateWaferUI();
        }

        private void SetupLogic()
        {
            this.DoubleBuffered = true;

            // 1. 시계 타이머
            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += (s, e) => {
                if (lblTime != null) lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                if (lblDate != null) lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            };
            clockTimer.Start();

            // 2. 시스템 제어 스케줄러 (핵심 로직)
            sysTimer = new Timer();
            sysTimer.Interval = 50; // 0.05초마다 갱신 (부드러운 애니메이션 + 로직)
            sysTimer.Tick += SysTimer_Tick;
            // Auto Run 시작 시 Start 함
        }

        // =============================================================
        // [핵심 로직] 스케줄러 & 상태 머신
        // =============================================================
        private void SysTimer_Tick(object sender, EventArgs e)
        {
            // 1. 로봇이 움직이는 중이면 애니메이션만 처리하고 리턴
            if (isRobotMoving)
            {
                AnimateRobot();
                pnlCenter.Invalidate(); // 다시 그리기
                return;
            }

            // 2. 공정 진행 시뮬레이션 (로봇이 안 움직일 때 처리)
            SimulateProcess();

            // 3. 스케줄링 (다음 동작 결정) - 우선순위에 따라 결정
            // 우선순위: Unload C -> Move BtoC -> Move AtoB -> Load A

            // [CASE 1] PM C 완료 -> FOUP B로 배출
            if (statusPmC == 2 && !robotHasWafer)
            {
                StartRobotMove("PMC", "FOUP_B");
            }
            // [CASE 2] PM B 완료 & PM C 비어있음 -> B에서 C로 이동
            else if (statusPmB == 2 && statusPmC == 0 && !robotHasWafer)
            {
                StartRobotMove("PMB", "PMC");
            }
            // [CASE 3] PM A 완료 & PM B 비어있음 -> A에서 B로 이동
            else if (statusPmA == 2 && statusPmB == 0 && !robotHasWafer)
            {
                StartRobotMove("PMA", "PMB");
            }
            // [CASE 4] FOUP A에 웨이퍼 있음 & PM A 비어있음 -> 투입 (파이프라인)
            else if (foupACount > 0 && statusPmA == 0 && !robotHasWafer)
            {
                StartRobotMove("FOUP_A", "PMA");
            }

            // UI 업데이트 (프로그레스바 등)
            UpdateProcessUI();
            pnlCenter.Invalidate();
        }

        // 로봇 이동 명령 시작
        private void StartRobotMove(string src, string dest)
        {
            robotSource = src;
            robotDestination = dest;
            isRobotMoving = true;
            robotHasWafer = false; // 이동 시작할 땐 일단 빈손 (가서 잡음) or 잡고 이동

            // 소스 위치에 따른 목표 각도 설정 (가질러 가기)
            targetAngle = GetAngle(src);
        }

        // 로봇 애니메이션 로직
        private void AnimateRobot()
        {
            // 각도 부드럽게 변경 (단순화: 5도씩 회전)
            float speed = 10.0f;

            if (Math.Abs(robotAngle - targetAngle) > speed)
            {
                if (robotAngle < targetAngle) robotAngle += speed;
                else robotAngle -= speed;
            }
            else
            {
                robotAngle = targetAngle; // 도달

                // [1단계] 소스 도착 -> 웨이퍼 집기
                if (!robotHasWafer)
                {
                    robotHasWafer = true;
                    // 소스에서 웨이퍼 제거 로직
                    if (robotSource == "FOUP_A") foupACount--;
                    else if (robotSource == "PMA") statusPmA = 0; // 비움
                    else if (robotSource == "PMB") statusPmB = 0;
                    else if (robotSource == "PMC") statusPmC = 0;

                    UpdateWaferUI(); // 카세트 UI 갱신

                    // 이제 목적지로 이동 설정
                    targetAngle = GetAngle(robotDestination);
                }
                // [2단계] 목적지 도착 -> 웨이퍼 내려놓기
                else
                {
                    robotHasWafer = false;
                    isRobotMoving = false; // 이동 완료

                    // 목적지에 웨이퍼 투입 로직
                    if (robotDestination == "PMA") { statusPmA = 1; progressA = 0; } // 공정 시작
                    else if (robotDestination == "PMB") { statusPmB = 1; progressB = 0; }
                    else if (robotDestination == "PMC") { statusPmC = 1; progressC = 0; }
                    else if (robotDestination == "FOUP_B") { foupBCount++; UpdateWaferUI(); }
                }
            }
        }

        // 공정 진행 시뮬레이션
        private void SimulateProcess()
        {
            // PM A
            if (statusPmA == 1) // Processing
            {
                progressA += (100.0 / (timePmA * 20)); // 50ms 호출 기준, timePmA(초) 동안 100% 도달
                if (progressA >= 100) { progressA = 100; statusPmA = 2; } // 완료
            }
            // PM B
            if (statusPmB == 1)
            {
                progressB += (100.0 / (timePmB * 20));
                if (progressB >= 100) { progressB = 100; statusPmB = 2; }
            }
            // PM C
            if (statusPmC == 1)
            {
                progressC += (100.0 / (timePmC * 20));
                if (progressC >= 100) { progressC = 100; statusPmC = 2; }
            }
        }

        private float GetAngle(string moduleName)
        {
            switch (moduleName)
            {
                case "PMA": return ANG_PMA;
                case "PMB": return ANG_PMB;
                case "PMC": return ANG_PMC;
                case "FOUP_A": return ANG_FOUP_A;
                case "FOUP_B": return ANG_FOUP_B;
                default: return 0;
            }
        }

        // =============================================================
        // UI 업데이트 Helper
        // =============================================================
        private void ToggleAutoRun()
        {
            isAutoRun = !isAutoRun;
            if (isAutoRun)
            {
                btnConnect.Text = "STOP";
                btnConnect.BackColor = Color.LightCoral;
                sysTimer.Start();
            }
            else
            {
                btnConnect.Text = "CONNECT"; // Start/AutoRun 의미
                btnConnect.BackColor = Color.Khaki;
                sysTimer.Stop();
            }
        }

        private void UpdateProcessUI()
        {
            // PM A
            progA.Value = (int)Math.Min(progressA, 100);
            pnlChamberA.BackColor = GetStateColor(statusPmA);

            // PM B
            progB.Value = (int)Math.Min(progressB, 100);
            pnlChamberB.BackColor = GetStateColor(statusPmB);

            // PM C
            progC.Value = (int)Math.Min(progressC, 100);
            pnlChamberC.BackColor = GetStateColor(statusPmC);
        }

        private Color GetStateColor(int state)
        {
            if (state == 0) return Color.FromArgb(220, 220, 220); // Idle (Gray)
            if (state == 1) return Color.LimeGreen; // Processing (Green)
            if (state == 2) return Color.Yellow; // Complete/Wait (Yellow)
            return Color.Gray;
        }

        private void UpdateWaferUI()
        {
            txtCarrierA.Text = $"FOUP_LOT01 ({foupACount})";
            txtCarrierB.Text = $"FOUP_LOT02 ({foupBCount})";

            // FOUP A 그래픽 (단순화: 5개 슬롯만 표현, 개수에 따라 색칠)
            pnlWaferL1.BackColor = foupACount >= 21 ? Color.Blue : Color.Black;
            pnlWaferL2.BackColor = foupACount >= 16 ? Color.Blue : Color.Black;
            pnlWaferL3.BackColor = foupACount >= 11 ? Color.Blue : Color.Black;
            pnlWaferL4.BackColor = foupACount >= 6 ? Color.Blue : Color.Black;
            pnlWaferL5.BackColor = foupACount >= 1 ? Color.Blue : Color.Black;

            // FOUP B 그래픽
            pnlWaferR1.BackColor = foupBCount >= 21 ? Color.Blue : Color.Black;
            pnlWaferR2.BackColor = foupBCount >= 16 ? Color.Blue : Color.Black;
            pnlWaferR3.BackColor = foupBCount >= 11 ? Color.Blue : Color.Black;
            pnlWaferR4.BackColor = foupBCount >= 6 ? Color.Blue : Color.Black;
            pnlWaferR5.BackColor = foupBCount >= 1 ? Color.Blue : Color.Black;
        }

        // [중요] RecipeControl에서 넘어온 데이터 적용
        private void ApplyRecipeData(RecipeControl.RecipeModel data)
        {
            if (data.PmA_Params != null)
            {
                valTargetA.Text = data.PmA_Params[0];
                valGasA.Text = data.PmA_Params[1];
                valTimeA.Text = data.PmA_Params[2];
                // [핵심] 실제 로직 변수에 반영 (int.TryParse 등으로 안전하게 변환 필요)
                int.TryParse(data.PmA_Params[2], out timePmA);
            }
            if (data.PmB_Params != null)
            {
                valAlignB.Text = data.PmB_Params[0];
                valRpmB.Text = data.PmB_Params[1];
                valTimeB.Text = data.PmB_Params[2];
                int.TryParse(data.PmB_Params[2], out timePmB);
            }
            if (data.PmC_Params != null)
            {
                valPressC.Text = data.PmC_Params[0];
                valGasC.Text = data.PmC_Params[1];
                valSpinTimeC.Text = data.PmC_Params[2];
                int.TryParse(data.PmC_Params[2], out timePmC);
            }
        }

        // ... (이하 기존 레이아웃 및 로그인 관련 코드 유지) ...
        private void UpdateLayout()
        {
            if (pnlCenter.Width == 0 || pnlCenter.Height == 0) return;
            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            pnlChamberB.Location = new Point(cx - 40, cy - 250);
            pnlChamberA.Location = new Point(cx - 250, cy - 50);
            pnlChamberC.Location = new Point(cx + 170, cy - 50);
            pnlFoupA.Location = new Point(cx - 200, cy + 180);
            pnlFoupB.Location = new Point(cx + 120, cy + 180);
            pnlCassetteL.Location = new Point(cx - 100, cy + 330);
            pnlCassetteR.Location = new Point(cx + 20, cy + 330);
        }

        private void SetLoginState(bool login)
        {
            isLoggedIn = login;
            pnlLeft.Visible = login;
            pnlRight.Visible = login;
            pnlCenter.Visible = login;
            pnlBottom.Visible = login;
            txtId.Enabled = !login;
            txtPw.Enabled = !login;
            btnLogin.Text = login ? "LOGOUT" : "LOGIN";
            if (!login) { txtId.Text = ""; txtPw.Text = ""; txtId.Focus(); }
            if (login) UpdateLayout();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (isLoggedIn)
            {
                if (MessageBox.Show("로그아웃 하시겠습니까?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    SetLoginState(false);
                return;
            }
            if (txtId.Text.Trim() == "admin" && txtPw.Text.Trim() == "1234")
            {
                MessageBox.Show("로그인 되었습니다.", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetLoginState(true);
            }
            else
            {
                MessageBox.Show("ID/PW 불일치", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            if (!isLoggedIn) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            // 로봇 본체
            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);

            // 로봇 팔 (회전 적용)
            g.TranslateTransform(cx, cy);
            g.RotateTransform(robotAngle); // 애니메이션 각도 적용

            // 팔 링크
            g.FillRectangle(Brushes.Gray, 0, -15, 120, 30); // 길이가 좀 더 긴 팔 (중앙에서 뻗어나감)

            // 웨이퍼 (로봇이 들고 있을 때만 그림)
            if (robotHasWafer)
            {
                g.FillEllipse(Brushes.CornflowerBlue, 90, -20, 40, 40); // 팔 끝에 웨이퍼 표시
            }

            g.ResetTransform();

            // 신호등
            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(isAutoRun ? Brushes.Maroon : Brushes.Red, lightX, lightY, 30, 30); // Stop 상태일 때 Red 밝게
            g.FillRectangle(Brushes.Olive, lightX, lightY + 30, 30, 30);
            g.FillRectangle(isAutoRun ? Brushes.Lime : Brushes.Green, lightX, lightY + 60, 30, 30); // Run 상태일 때 Green 밝게
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);
        }

        private void BtnMain_Click(object sender, EventArgs e) { pnlCenter.Invalidate(); UpdateLayout(); }
        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            Form rPop = new Form() { Text = "Recipe", Size = new Size(1290, 760), StartPosition = FormStartPosition.CenterScreen };
            RecipeControl rc = new RecipeControl() { Dock = DockStyle.Fill };
            rc.ApplyToMainRequested += (s2, data) => ApplyRecipeData(data);
            rc.btnCancel.Click += (s2, e2) => rPop.Close();
            rPop.Controls.Add(rc);
            rPop.ShowDialog();
        }
        private void BtnLog_Click(object sender, EventArgs e)
        {
            Form lPop = new Form() { Text = "Log", Size = new Size(1280, 760), StartPosition = FormStartPosition.CenterScreen };
            LogControl lc = new LogControl() { Dock = DockStyle.Fill };
            lPop.Controls.Add(lc);
            lPop.Show();
        }

        // 웨이퍼 상태 변경 (Load/Unload 버튼용 - 기존 메서드 유지)
        private void SetFoupState(string type, bool loaded)
        {
            if (type == "A") foupACount = loaded ? 25 : 0;
            else foupBCount = loaded ? 25 : 0;
            UpdateWaferUI();
        }
    }
}