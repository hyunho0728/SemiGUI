using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using IEG3268_Dll;
using MySql.Data.MySqlClient;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        IEG3268 EtherCAT_M = new IEG3268();
        bool isEtherConnected = false;

        // Form1.cs 클래스 내부 상단 (변수 선언부)

        // [1] 좌/우 서보 (Axis 2) 위치 값 (단위: Pulse)
        private const long POS_LR_FOUP_A = 13500;
        private const long POS_LR_FOUP_B = -394700;
        private const long POS_LR_PMA = -59064;
        private const long POS_LR_PMB = -190823;
        private const long POS_LR_PMC = -322000;

        // [2] 상/하 서보 (Axis 1) 위치 값 (단위: Pulse)
        // 챔버 공통 높이
        private const long POS_UD_CHAMBER_SAFE = 1150000; // 상승 위치
        private const long POS_UD_CHAMBER_PLACE = 806931; // 안착 위치

        // FOUP 슬롯별 높이 (Slot 1 ~ 5)
        private readonly long[] POS_UD_FOUP_SLOTS = new long[]
        {
            290000,   // Slot 1 (Bottom)
            982378,   // Slot 2
            1627604,  // Slot 3
            2332102,  // Slot 4
            3018457   // Slot 5 (Top)
        };

        // [3] 로봇 실린더 IO 번호
        private const int DO_ROBOT_EXTEND = 12;  // 전진 (이미지/참조코드 기준)
        private const int DO_ROBOT_RETRACT = 13; // 후진
        private const int DO_VACUUM_ON = 14;     // 흡기 (Wafer Grip)
        private const int DO_VACUUM_OFF = 15;    // 배기 (Wafer Release) - 필요 시

        // [DB 연결 문자열] 환경에 맞게 수정 필요
        private string connectionString = "Server=localhost;Port=3306;Database=SemiGuiData;Uid=root;Pwd=1234;Charset=utf8;";

        // ... (기존 변수 유지) ...
        private bool isLoggedIn = false;
        private bool isAutoRun = false;

        private Timer clockTimer;
        private Timer sysTimer;

        private int foupACount = 5;
        private int foupBCount = 0;

        private int statusPmA = 0;
        private int statusPmB = 0;
        private int statusPmC = 0;

        private double progressA = 0;
        private double progressB = 0;
        private double progressC = 0;

        // [추가] 각 챔버별 공정 비동기 작업 실행 여부 확인용 플래그
        private bool isRunningPmA = false;
        private bool isRunningPmB = false;
        private bool isRunningPmC = false;

        private int timePmA = 5;
        private int timePmB = 5;
        private int timePmC = 5;

        private float robotAngle = 180;
        private float targetAngle = 0;
        private bool isRobotMoving = false;
        private bool robotHasWafer = false;
        private string robotDestination = "";
        private string robotSource = "";

        private float robotExtension = 0;
        private const float MAX_EXTENSION = 60.0f;
        private const float EXTENSION_SPEED = 5.0f;

        // 로봇 동작 상태 상수
        private const int ROBOT_STATE_ROTATE = 0;
        private const int ROBOT_STATE_EXTEND = 1;
        private const int ROBOT_STATE_WAIT = 2;
        private const int ROBOT_STATE_RETRACT = 3;
        private int currentRobotState = ROBOT_STATE_ROTATE;

        // 대기 시간 카운터 변수
        private int robotWaitCounter = 0;
        private const int ROBOT_WAIT_TICKS = 10;

        private float robotSpeed = 10.0f;

        private const float ANG_PMC = 0;
        // 로봇 진입 각도 표준화 (135도 / 45도)
        private const float ANG_FOUP_B = 45;
        private const float ANG_FOUP_A = 135;
        private const float ANG_PMA = 180;
        private const float ANG_PMB = 270;

        private Button btnAutoRun;
        private int currentAlarmLevel = 0;

        // 리셋 중임을 나타내는 플래그
        private bool isResetting = false;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();

            this.Size = new Size(1920, 1080);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            SetupLogic();
            LoadSystemConfig();
            CreateAutoRunButton();

            this.btnMain.Click += BtnMain_Click;
            this.btnRecipe.Click += BtnRecipe_Click;
            this.btnLog.Click += BtnLog_Click;
            this.btnConfig.Click += BtnConfig_Click;
            this.btnLogin.Click += BtnLogin_Click;

            this.btnLoadA.Click += (s, e) =>
            {
                foupACount = 5;
                UpdateWaferUI();
                pnlCenter.Invalidate();
                AddLog("Info", "FOUP A", "Carrier Loaded manually");
            };
            this.btnUnloadA.Click += (s, e) => { foupACount = 0; UpdateWaferUI(); pnlCenter.Invalidate(); };
            this.btnLoadB.Click += (s, e) => { foupBCount = 5; UpdateWaferUI(); pnlCenter.Invalidate(); };
            this.btnUnloadB.Click += (s, e) => { foupBCount = 0; UpdateWaferUI(); pnlCenter.Invalidate(); };

            this.btnResetChambers.Click += BtnResetChambers_Click;

            this.pnlCenter.SizeChanged += (s, e) => UpdateLayout();
            this.pnlCenter.Paint += pnlCenter_Paint;
            this.pnlAlarm.Paint += pnlAlarm_Paint;

            SetLoginState(false);

            pnlFoupA.Visible = false;
            pnlFoupB.Visible = false;
            pnlCassetteL.Visible = false;
            pnlCassetteR.Visible = false;

            UpdateWaferUI();
            UpdateProcessUI();
        }

        private void LoadSystemConfig()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string initSql = "INSERT IGNORE INTO sys_config (cfg_key, cfg_value) VALUES ('RobotSpeed', '10'), ('TimerInterval', '50')";
                    using (MySqlCommand cmd = new MySqlCommand(initSql, conn)) cmd.ExecuteNonQuery();

                    string sql = "SELECT cfg_key, cfg_value FROM sys_config";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string key = reader.GetString("cfg_key");
                            string val = reader.GetString("cfg_value");

                            if (key == "RobotSpeed") float.TryParse(val, out robotSpeed);
                            if (key == "TimerInterval")
                            {
                                int interval;
                                if (int.TryParse(val, out interval))
                                {
                                    if (sysTimer != null) sysTimer.Interval = interval;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            Form cPop = new Form() { Text = "Configuration", Size = new Size(820, 640), StartPosition = FormStartPosition.CenterScreen };
            ConfigControl cc = new ConfigControl() { Dock = DockStyle.Fill };

            cc.ConfigSaved += (s2, e2) =>
            {
                LoadSystemConfig();
                AddLog("Event", "System", "Configuration Updated");
            };

            cc.btnClose.Click += (s2, e2) => cPop.Close();
            cPop.Controls.Add(cc);
            cPop.ShowDialog();
        }

        private void AddLog(string type, string equipment, string message)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO logs (timestamp, type, equipment, message) VALUES (@ts, @type, @eqp, @msg)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ts", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@eqp", equipment);
                        cmd.Parameters.AddWithValue("@msg", message);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception) { }
        }

        // [수정] 리셋 로직에 비동기 플래그 초기화 추가
        private void BtnResetChambers_Click(object sender, EventArgs e)
        {
            if (isAutoRun) ToggleAutoRun();

            statusPmA = 0; progressA = 0; isRunningPmA = false;
            statusPmB = 0; progressB = 0; isRunningPmB = false;
            statusPmC = 0; progressC = 0; isRunningPmC = false;

            UpdateProcessUI();
            ResetRobotState();
            pnlCenter.Invalidate();

            AddLog("Info", "System", "System reset by user");
            MessageBox.Show("시스템 상태가 초기화되었습니다. 로봇이 초기 위치로 복귀합니다.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnResetRobot_Click(object sender, EventArgs e)
        {
            ResetRobotState();

            // [추가] 하드웨어 연결 시 로봇 상하/좌우 원점 복귀(Homing) 실행
            if (isEtherConnected)
            {
                try
                {
                    // 라이브러리 메서드를 통해 각 축 원점 복귀 명령 전송
                    // (라이브러리 메서드명: Homming)
                    EtherCAT_M.Axis1_UD_Homming(); // 상하 축 원점 복귀
                    EtherCAT_M.Axis2_LR_Homming(); // 좌우 축 원점 복귀

                    AddLog("Info", "Robot", "Hardware Origin Return (Homing) Started");
                }
                catch (Exception ex)
                {
                    AddLog("Error", "Robot", $"Homing Failed: {ex.Message}");
                    MessageBox.Show($"원점 복귀 명령 실패: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetRobotState()
        {
            isResetting = true;

            isRobotMoving = true;
            robotHasWafer = false;
            targetAngle = 180;
            robotDestination = "";
            robotSource = "";

            if (robotExtension > 0)
            {
                currentRobotState = ROBOT_STATE_RETRACT;
            }
            else
            {
                currentRobotState = ROBOT_STATE_ROTATE;
            }

            robotWaitCounter = 0;

            if (!sysTimer.Enabled)
            {
                sysTimer.Start();
            }
        }

        private void InitializeDatabase()
        {
            string initConnStr = "Server=localhost;Port=3306;Uid=root;Pwd=1234;Charset=utf8;";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(initConnStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS SemiGuiData", conn)) cmd.ExecuteNonQuery();
                    conn.ChangeDatabase("SemiGuiData");
                    using (MySqlCommand cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS logs (id INT AUTO_INCREMENT PRIMARY KEY, timestamp DATETIME NOT NULL, type VARCHAR(50), equipment VARCHAR(100), message TEXT)", conn)) cmd.ExecuteNonQuery();
                    using (MySqlCommand cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS recipes (name VARCHAR(100) PRIMARY KEY, pmA_target VARCHAR(50), pmA_gas VARCHAR(50), pmA_time VARCHAR(50), pmB_align VARCHAR(50), pmB_rpm VARCHAR(50), pmB_time VARCHAR(50), pmC_press VARCHAR(50), pmC_gas VARCHAR(50), pmC_time VARCHAR(50))", conn)) cmd.ExecuteNonQuery();
                    using (MySqlCommand cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS sys_config (cfg_key VARCHAR(50) PRIMARY KEY, cfg_value VARCHAR(100))", conn)) cmd.ExecuteNonQuery();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM recipes", conn))
                    {
                        if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                        {
                            string insertSql = "INSERT INTO recipes VALUES ('Standard_Process', '1000.0', '500', '60', '0.001', '3000', '45', '15', '100', '120'), ('High_Temp_Fast', '1200.0', '800', '30', '0.002', '4000', '30', '20', '200', '90')";
                            using (MySqlCommand insertCmd = new MySqlCommand(insertSql, conn)) insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"DB 초기화 실패 (MySQL 서버 확인 필요):\n{ex.Message}", "DB Init Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void CreateAutoRunButton()
        {
            btnAutoRun = new Button();
            btnAutoRun.Text = "AUTO RUN";
            btnAutoRun.Size = new Size(100, 60);
            btnAutoRun.Location = new Point(750, 10);
            btnAutoRun.BackColor = Color.LightGray;
            btnAutoRun.Font = new Font("Arial", 10, FontStyle.Bold);
            btnAutoRun.Click += (s, e) =>
            {
                if (isLoggedIn)
                {
                    ToggleAutoRun();
                }
                else
                {
                    MessageBox.Show("로그인이 필요합니다.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            this.pnlTop.Controls.Add(btnAutoRun);
        }

        private void SetupLogic()
        {
            this.DoubleBuffered = true;

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, pnlCenter, new object[] { true });

            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += (s, e) =>
            {
                if (lblTime != null) lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                if (lblDate != null) lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            };
            clockTimer.Start();
            sysTimer = new Timer();
            sysTimer.Interval = 50;
            sysTimer.Tick += SysTimer_Tick;
        }

        // [수정] SysTimer_Tick: 로봇 제어 및 공정 시작 트리거 역할만 수행
        private void SysTimer_Tick(object sender, EventArgs e)
        {
            CheckAlarms();

            //SyncHardwareIO();

            // 1. 공정 시뮬레이션 시작 트리거 (비동기 함수 호출)
            // 상태가 1(Running)이고 아직 비동기 작업이 시작되지 않았다면 시작
            if (statusPmA == 1 && !isRunningPmA) RunProcessAsync("PMA", timePmA);
            if (statusPmB == 1 && !isRunningPmB) RunProcessAsync("PMB", timePmB);
            if (statusPmC == 1 && !isRunningPmC) RunProcessAsync("PMC", timePmC);

            // 2. 로봇 애니메이션 (기존 로직 유지)
            if (isRobotMoving)
            {
                AnimateRobot();
                pnlCenter.Invalidate(); // 로봇 움직임 갱신
            }
            // 3. 리셋 처리
            else if (isResetting)
            {
                isResetting = false;
                if (!isAutoRun) sysTimer.Stop();
            }
            // 4. 새로운 명령 확인 (로봇이 IDLE 상태이고 리셋 중이 아닐 때만)
            else
            {
                // 로봇 스케줄링 로직 (기존과 동일)
                if (statusPmC == 2 && !robotHasWafer)
                {
                    if (foupBCount < 5) StartRobotMove("PMC", "FOUP_B");
                }
                else if (statusPmB == 2 && statusPmC == 0 && !robotHasWafer)
                {
                    StartRobotMove("PMB", "PMC");
                }
                else if (statusPmA == 2 && statusPmB == 0 && !robotHasWafer)
                {
                    StartRobotMove("PMA", "PMB");
                }
                else if (foupACount > 0 && statusPmA == 0 && !robotHasWafer)
                {
                    StartRobotMove("FOUP_A", "PMA");
                }
            }

            // UI 업데이트 (프로그레스바 등)
            // 비동기 함수 내에서도 업데이트하지만, 로봇 이동 등 다른 요소 반영을 위해 유지
            UpdateProcessUI();
        }

        // [신규] 비동기 공정 시뮬레이션 메서드
        private async void RunProcessAsync(string pmName, int durationSec)
        {
            // 중복 실행 방지
            if (pmName == "PMA") isRunningPmA = true;
            else if (pmName == "PMB") isRunningPmB = true;
            else if (pmName == "PMC") isRunningPmC = true;

            DateTime startTime = DateTime.Now;
            double targetDuration = durationSec; // 초 단위

            while (true)
            {
                // 경과 시간 계산
                double elapsed = (DateTime.Now - startTime).TotalSeconds;
                double progress = (elapsed / targetDuration) * 100.0;

                // 진행률 업데이트 (해당 PM만)
                if (pmName == "PMA") progressA = progress;
                else if (pmName == "PMB") progressB = progress;
                else if (pmName == "PMC") progressC = progress;

                // UI 갱신 (부드러운 진행을 위해)
                UpdateProcessUI();

                // 종료 조건: 100% 도달 시
                if (progress >= 100.0)
                {
                    // 100%로 값 고정
                    if (pmName == "PMA") progressA = 100.0;
                    else if (pmName == "PMB") progressB = 100.0;
                    else if (pmName == "PMC") progressC = 100.0;

                    UpdateProcessUI(); // 100% 상태 그리기
                    break; // 루프 종료
                }

                // 20ms 정도 대기 (약 50fps) - UI 스레드 양보
                await System.Threading.Tasks.Task.Delay(20);

                // [안전장치] 만약 공정 중 강제 리셋되거나 상태가 바뀌면 중단
                bool shouldStop = false;
                if (pmName == "PMA" && statusPmA != 1) shouldStop = true;
                if (pmName == "PMB" && statusPmB != 1) shouldStop = true;
                if (pmName == "PMC" && statusPmC != 1) shouldStop = true;

                if (shouldStop)
                {
                    // 실행 플래그 해제 후 종료
                    if (pmName == "PMA") isRunningPmA = false;
                    else if (pmName == "PMB") isRunningPmB = false;
                    else if (pmName == "PMC") isRunningPmC = false;
                    return;
                }
            }

            // 공정 완료 처리
            if (pmName == "PMA") { statusPmA = 2; isRunningPmA = false; }
            else if (pmName == "PMB") { statusPmB = 2; isRunningPmB = false; }
            else if (pmName == "PMC") { statusPmC = 2; isRunningPmC = false; }

            AddLog("Process", GetPmFullName(pmName), "Process Complete");
            UpdateProcessUI(); // 완료 상태(색상) 반영
        }

        private string GetPmFullName(string pmName)
        {
            if (pmName == "PMA") return "PM A";
            if (pmName == "PMB") return "PM B";
            if (pmName == "PMC") return "PM C";
            return pmName;
        }

        private void CheckAlarms()
        {
            int prevLevel = currentAlarmLevel;
            currentAlarmLevel = 0;
            string msg = "";

            if (foupBCount >= 5 && statusPmC == 2)
            {
                currentAlarmLevel = 2;
                msg = "DANGER: FOUP B Full!";
            }
            else if (foupACount == 0)
            {
                currentAlarmLevel = 1;
                msg = "WARNING: FOUP A Empty";
            }
            else
            {
                currentAlarmLevel = 0;
                msg = "System Normal";
            }

            if (prevLevel != currentAlarmLevel)
            {
                if (currentAlarmLevel > 0) lblAlarmMsg.Text = msg;
                else lblAlarmMsg.Text = "";

                pnlAlarm.Invalidate();

                string logType = "Info";
                if (currentAlarmLevel == 1) logType = "Warning";
                else if (currentAlarmLevel == 2) logType = "Alarm";

                if (currentAlarmLevel > 0) AddLog(logType, "System", msg);
                else if (prevLevel > 0 && currentAlarmLevel == 0) AddLog("Info", "System", "Alarm Cleared (Normal)");
            }
        }

        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            if (!isLoggedIn) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            // [중요] 그리기 전 좌표계 초기화
            g.ResetTransform();

            // 1. 배경 그리기
            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);

            // 2. FOUP 그리기 (회전된 형태)
            DrawRotatedFoup(g, cx, cy, ANG_FOUP_A, "FOUP A", foupACount);
            DrawRotatedFoup(g, cx, cy, ANG_FOUP_B, "FOUP B", foupBCount);

            // [중요] 로봇 그리기 전 좌표계 초기화
            g.ResetTransform();

            // 3. 로봇 그리기 (회전하는 형태)
            g.TranslateTransform(cx, cy);
            g.RotateTransform(robotAngle);

            // 로봇 몸통
            g.FillRectangle(Brushes.DimGray, -20, -20, 100, 40);

            // 로봇 팔 (Extension)
            float armX = 0 + robotExtension;
            g.FillRectangle(Brushes.Gray, armX, -15, 100, 30);

            // 로봇 웨이퍼
            if (robotHasWafer)
            {
                g.FillEllipse(Brushes.CornflowerBlue, armX + 70, -20, 40, 40);
            }

            // [중요] UI 그리기 전 좌표계 초기화
            g.ResetTransform();

            // 4. 신호등 그리기 (우측 상단 고정)
            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(isAutoRun ? Brushes.Maroon : Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Olive, lightX, lightY + 30, 30, 30);
            g.FillRectangle(isAutoRun ? Brushes.Lime : Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);
        }

        // [수정] FOUP 디자인 개선 및 5개 슬롯 항상 표시 (유무에 따라 색상 변경)
        private void DrawRotatedFoup(Graphics g, int cx, int cy, float angle, string label, int waferCount)
        {
            var state = g.Save();

            g.TranslateTransform(cx, cy);
            g.RotateTransform(angle);
            g.TranslateTransform(160, 0);
            g.RotateTransform(180); // 입구가 로봇을 향하게

            // [1] FOUP 배경
            Rectangle foupRect = new Rectangle(-40, -40, 80, 80);
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(240, 240, 240)))
            {
                g.FillRectangle(bgBrush, foupRect);
            }

            // [2] 웨이퍼 그리기 (항상 5개 표시, 유무에 따라 색상 변경)
            int maxWafers = 5;
            for (int i = 0; i < maxWafers; i++)
            {
                // 있음: 파랑, 없음: 검정
                Color waferColor = (i < waferCount) ? Color.Blue : Color.Black;

                using (SolidBrush waferBrush = new SolidBrush(waferColor))
                {
                    float waferWidth = 10;
                    float waferLength = 70;

                    float xPos = -35 + (i * 14);
                    float yPos = -35;

                    g.FillRectangle(waferBrush, xPos, yPos, waferWidth, waferLength);
                }
            }

            // [3] 벽면 그리기 (ㄷ자 모양)
            using (Pen wallPen = new Pen(Color.DimGray, 4))
            {
                g.DrawLine(wallPen, -40, -40, 40, -40); // 위
                g.DrawLine(wallPen, -40, 40, 40, 40);   // 아래
                g.DrawLine(wallPen, -40, -42, -40, 42); // 뒤
            }

            // [4] 라벨 그리기
            using (Font f = new Font("Arial", 10, FontStyle.Bold))
            {
                var labelState = g.Save();

                g.TranslateTransform(-50, 0);
                //g.RotateTransform(-180);
                g.RotateTransform(90);

                SizeF size = g.MeasureString(label, f);
                g.DrawString(label, f, Brushes.Black, -size.Width / 2, -size.Height / 2);

                g.Restore(labelState);
            }

            g.Restore(state);
        }

        private void pnlAlarm_Paint(object sender, PaintEventArgs e)
        {
            if (currentAlarmLevel == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Point[] trianglePoints = {
                new Point(30, 5),
                new Point(5, 55),
                new Point(55, 55)
            };

            if (currentAlarmLevel == 2)
            {
                g.FillPolygon(Brushes.Red, trianglePoints);
                using (Font f = new Font("Arial", 24, FontStyle.Bold))
                {
                    SizeF size = g.MeasureString("!", f);
                    g.DrawString("!", f, Brushes.White, 30 - size.Width / 2, 35 - size.Height / 2);
                }
            }
            else if (currentAlarmLevel == 1)
            {
                g.FillPolygon(Brushes.Gold, trianglePoints);
                using (Font f = new Font("Arial", 24, FontStyle.Bold))
                {
                    SizeF size = g.MeasureString("!", f);
                    g.DrawString("!", f, Brushes.White, 30 - size.Width / 2, 35 - size.Height / 2);
                }
            }
        }

        private void StartRobotMove(string src, string dest)
        {
            robotSource = src;
            robotDestination = dest;
            isRobotMoving = true;
            robotHasWafer = false;
            targetAngle = GetAngle(src);

            currentRobotState = ROBOT_STATE_ROTATE;
            robotExtension = 0;

            // [추가] 하드웨어 로봇 이동 명령 전송
            // Pick하러 가는 것이므로 Source 위치로 이동해야 함
            // FOUP인 경우 현재 카운트(슬롯 위치)를 계산해서 전달
            int slotIdx = 0;
            if (src == "FOUP_A") slotIdx = foupACount - 1;
            else if (src == "FOUP_B") slotIdx = foupBCount - 1;

            // 1차 이동: 물건을 가지러(Source) 감
            //MoveRobotHardware(src, slotIdx);
        }

        private void AnimateRobot()
        {
            float speed = this.robotSpeed;

            switch (currentRobotState)
            {
                case ROBOT_STATE_ROTATE:
                    float diff = targetAngle - robotAngle;

                    while (diff <= -180) diff += 360;
                    while (diff > 180) diff -= 360;

                    if (Math.Abs(diff) > speed)
                    {
                        if (diff > 0) robotAngle += speed;
                        else robotAngle -= speed;

                        if (robotAngle >= 360) robotAngle -= 360;
                        if (robotAngle < 0) robotAngle += 360;
                    }
                    else
                    {
                        robotAngle = targetAngle;

                        if (isResetting && robotAngle == 180)
                        {
                            isRobotMoving = false;
                            return;
                        }

                        currentRobotState = ROBOT_STATE_EXTEND;

                        // [추가] 회전 완료 -> 실린더 전진 (DO 12 ON, 13 OFF)
                        if (isEtherConnected)
                        {
                            EtherCAT_M.Digital_Output(DO_ROBOT_RETRACT, false);
                            EtherCAT_M.Digital_Output(DO_ROBOT_EXTEND, true);
                        }
                    }
                    break;

                case ROBOT_STATE_EXTEND:
                    robotExtension += EXTENSION_SPEED;
                    if (robotExtension >= MAX_EXTENSION)
                    {
                        robotExtension = MAX_EXTENSION;
                        currentRobotState = ROBOT_STATE_WAIT;
                        robotWaitCounter = ROBOT_WAIT_TICKS;

                        // [추가] 완전히 뻗었을 때 -> 진공 흡입/해제
                        if (isEtherConnected)
                        {
                            if (!robotHasWafer) // Pick 동작 중이면 흡착
                                EtherCAT_M.Digital_Output(DO_VACUUM_ON, true);
                            else // Place 동작 중이면 해제
                                EtherCAT_M.Digital_Output(DO_VACUUM_ON, false);
                        }
                    }
                    break;

                case ROBOT_STATE_WAIT:
                    if (robotWaitCounter > 0)
                    {
                        robotWaitCounter--;
                    }
                    else
                    {
                        if (!isResetting) PerformRobotAction();
                        currentRobotState = ROBOT_STATE_RETRACT;

                        // [추가] 대기 끝 -> 실린더 후진 (DO 12 OFF, 13 ON)
                        if (isEtherConnected)
                        {
                            EtherCAT_M.Digital_Output(DO_ROBOT_EXTEND, false);
                            EtherCAT_M.Digital_Output(DO_ROBOT_RETRACT, true);
                        }
                    }
                    break;

                case ROBOT_STATE_RETRACT:
                    robotExtension -= EXTENSION_SPEED;
                    if (robotExtension <= 0)
                    {
                        robotExtension = 0;
                        currentRobotState = ROBOT_STATE_ROTATE;

                        // [추가] 후진 완료
                        if (isResetting)
                        {
                            targetAngle = 180;
                            return;
                        }

                        if (robotHasWafer)
                        {
                            // Pick 완료 -> Place 위치(Destination)로 이동
                            targetAngle = GetAngle(robotDestination);

                            // [추가] 하드웨어: 목적지로 서보 이동
                            int slotIdx = 0;
                            if (robotDestination == "FOUP_A") slotIdx = foupACount; // 놓을 빈 공간
                            else if (robotDestination == "FOUP_B") slotIdx = foupBCount;

                            //MoveRobotHardware(robotDestination, slotIdx);
                        }
                        else
                        {
                            isRobotMoving = false;
                            AddLog("Transfer", "Robot", $"Wafer moved: {robotSource} > {robotDestination}");
                        }
                    }
                    break;
            }
        }

        private void PerformRobotAction()
        {
            if (!robotHasWafer) // Pick
            {
                robotHasWafer = true;
                if (robotSource == "FOUP_A") foupACount--;
                else if (robotSource == "PMA") { statusPmA = 0; progressA = 0; } // 꺼낼 때 초기화
                else if (robotSource == "PMB") { statusPmB = 0; progressB = 0; }
                else if (robotSource == "PMC") { statusPmC = 0; progressC = 0; }

                UpdateWaferUI();
                pnlCenter.Invalidate();
            }
            else // Place
            {
                robotHasWafer = false;
                // 상태를 1로 바꾸면 SysTimer_Tick에서 감지하여 RunProcessAsync를 시작함
                if (robotDestination == "PMA") { statusPmA = 1; progressA = 0; }
                else if (robotDestination == "PMB") { statusPmB = 1; progressB = 0; }
                else if (robotDestination == "PMC") { statusPmC = 1; progressC = 0; }
                else if (robotDestination == "FOUP_B") { foupBCount++; UpdateWaferUI(); pnlCenter.Invalidate(); }
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

        private void ToggleAutoRun()
        {
            isAutoRun = !isAutoRun;
            if (isAutoRun)
            {
                btnAutoRun.Text = "STOP";
                btnAutoRun.BackColor = Color.LightCoral;
                sysTimer.Start();
                AddLog("Event", "System", "Auto Run Started");
            }
            else
            {
                btnAutoRun.Text = "AUTO RUN";
                btnAutoRun.BackColor = Color.LightGray;
                sysTimer.Stop();
                AddLog("Event", "System", "Auto Run Stopped");
            }
        }

        private void UpdateProcessUI()
        {
            // [변경] 기존 단순 대입(progA.Value = ...) 대신 즉시 갱신 메서드 사용
            UpdateProgressBar(progA, (int)Math.Min(progressA, 100));
            pnlChamberA.BackColor = GetStateColor(statusPmA);

            UpdateProgressBar(progB, (int)Math.Min(progressB, 100));
            pnlChamberB.BackColor = GetStateColor(statusPmB);

            UpdateProgressBar(progC, (int)Math.Min(progressC, 100));
            pnlChamberC.BackColor = GetStateColor(statusPmC);
        }

        // [추가] ProgressBar 애니메이션 딜레이 제거를 위한 헬퍼 메서드
        private void UpdateProgressBar(ProgressBar pb, int value)
        {
            if (value == pb.Value) return;

            // 값이 100(Maximum)일 때 애니메이션 없이 즉시 꽉 차게 설정하는 트릭
            if (value == pb.Maximum)
            {
                pb.Maximum = value + 1;     // Maximum을 잠시 늘림
                pb.Value = value + 1;       // 값을 늘린 Maximum으로 설정 (즉시 이동)
                pb.Maximum = value;         // Maximum을 원래대로 복구
            }
            else
            {
                // 목표 값보다 1 크게 설정했다가 줄이면 애니메이션이 생략됨
                pb.Value = value + 1;
                pb.Value = value;
            }
        }

        private Color GetStateColor(int state)
        {
            if (state == 0) return Color.FromArgb(220, 220, 220);
            if (state == 1) return Color.LimeGreen;
            if (state == 2) return Color.Yellow;
            return Color.Gray;
        }

        private void UpdateWaferUI()
        {
            txtCarrierA.Text = $"FOUP_LOT01 ({foupACount})";
            txtCarrierB.Text = $"FOUP_LOT02 ({foupBCount})";
        }

        private void ApplyRecipeData(RecipeControl.RecipeModel data)
        {
            if (data.PmA_Params != null)
            {
                valTargetA.Text = data.PmA_Params[0]; valGasA.Text = data.PmA_Params[1]; valTimeA.Text = data.PmA_Params[2]; valCurrA.Text = data.PmA_Params[0];
                int.TryParse(data.PmA_Params[2], out timePmA);
            }
            if (data.PmB_Params != null)
            {
                valAlignB.Text = data.PmB_Params[0]; valRpmB.Text = data.PmB_Params[1]; valTimeB.Text = data.PmB_Params[2];
                int.TryParse(data.PmB_Params[2], out timePmB);
            }
            if (data.PmC_Params != null)
            {
                valPressC.Text = data.PmC_Params[0]; valGasC.Text = data.PmC_Params[1]; valSpinTimeC.Text = data.PmC_Params[2];
                int.TryParse(data.PmC_Params[2], out timePmC);
            }
            AddLog("Event", "Recipe", $"Recipe Applied: {data.Name}");
        }

        private void UpdateLayout()
        {
            if (pnlCenter.Width == 0 || pnlCenter.Height == 0) return;
            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            pnlChamberB.Location = new Point(cx - 40, cy - 220);
            pnlChamberA.Location = new Point(cx - 210, cy - 50);
            pnlChamberC.Location = new Point(cx + 120, cy - 50);

            pnlFoupA.Location = new Point(cx - 150, cy + 70);
            pnlFoupB.Location = new Point(cx + 70, cy + 70);

            pnlCassetteL.Location = new Point(cx - 100, cy + 160);
            pnlCassetteR.Location = new Point(cx + 20, cy + 160);

            if (lblNameA != null) lblNameA.Location = new Point(pnlChamberA.Left + (pnlChamberA.Width - lblNameA.Width) / 2, pnlChamberA.Top - 25);
            if (lblNameB != null) lblNameB.Location = new Point(pnlChamberB.Left + (pnlChamberB.Width - lblNameB.Width) / 2, pnlChamberB.Top - 25);
            if (lblNameC != null) lblNameC.Location = new Point(pnlChamberC.Left + (pnlChamberC.Width - lblNameC.Width) / 2, pnlChamberC.Top - 25);
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
            else { AddLog("Event", "System", "User Logged In"); }
            if (login) UpdateLayout();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (isLoggedIn)
            {
                if (MessageBox.Show("로그아웃 하시겠습니까?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AddLog("Event", "System", "User Logged Out");
                    SetLoginState(false);
                }
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
                AddLog("Warning", "Security", "Login Failed: Invalid Credentials");
            }
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

        // [추가] 시뮬레이션 상태를 실제 하드웨어 IO로 출력
        // [신규] 시뮬레이션 상태를 실제 IO와 동기화
        // Form1.cs 내부의 SyncHardwareIO 메서드 수정

        // [수정] 하드웨어 IO 동기화 (로봇 동작 단계별 도어 제어 강화)
        // [수정] 하드웨어 IO 동기화 (공정 중 도어 닫힘 유지)

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isEtherConnected)
            {
                // 연결 시도
                if (EtherCAT_M.CIFX_50RE_Connect() == true)
                {
                    isEtherConnected = true;
                    btnConnect.Text = "DISCONNECT";
                    btnConnect.BackColor = Color.LimeGreen; // 연결 성공 시 녹색
                    lblHostState.Text = "ONLINE"; // HOST 상태 표시 연동 (선택사항)

                    EtherCAT_M.ReadData_Send_Start(300); // Timer interval Set
                    EtherCAT_M.ReadData_Timer_Start();   // Timer Start

                    // [추가] Servo ON (필수)
                    EtherCAT_M.Axis1_ON(); // Up/Down
                    EtherCAT_M.Axis2_ON(); // Left/Right

                    //Axis Parameter Update
                    EtherCAT_M.Axis1_UD_Config_Update(1000000, 1000000, 1000000, 1000000);
                    EtherCAT_M.Axis2_LR_Config_Update(1000000, 1000000, 1000000, 1000000);

                    AddLog("Event", "System", "EtherCAT Connected Success");
                }
                else
                {
                    MessageBox.Show("EtherCAT 연결 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddLog("Error", "System", "EtherCAT Connection Failed");
                }
            }
            else
            {
                // [추가] Servo OFF
                EtherCAT_M.Axis1_OFF();
                EtherCAT_M.Axis2_OFF();

                // 연결 해제
                EtherCAT_M.CIFX_50RE_Disconnect();
                isEtherConnected = false;

                btnConnect.Text = "CONNECT";
                btnConnect.BackColor = Color.Khaki; // 원래 색상 복구
                lblHostState.Text = "OFFLINE";

                AddLog("Event", "System", "EtherCAT Disconnected");
            }
        }

        private void testing()
        {

        }

        private void btnRobot_Click(object sender, EventArgs e)
        {
            Form robotForm = new Form() { Text = "Robot", Size = new Size(1263, 759), StartPosition = FormStartPosition.CenterScreen };
            RobotControl rc = new RobotControl() { Dock = DockStyle.Fill };
            rc.ApplyToMainRequested += (s2, data) => ApplyRecipeData(data);
            rc.btnCancel.Click += (s2, e2) => rPop.Close();
            rPop.Controls.Add(rc);
            rPop.ShowDialog();
        }

        /*private void SyncHardwareIO()
        {
            if (!isEtherConnected) return;

            // 1. 타워 램프
            EtherCAT_M.Digital_Output(0, currentAlarmLevel == 2); // Red
            EtherCAT_M.Digital_Output(1, currentAlarmLevel == 1); // Yellow
            EtherCAT_M.Digital_Output(2, currentAlarmLevel == 0); // Green

            // [로봇 상태 공통 변수]
            bool isRetracting = (currentRobotState == ROBOT_STATE_RETRACT);

            // =========================================================
            // 2. Chamber A 제어
            // =========================================================
            bool shouldOpenA = false; // 기본값: 닫힘

            if (isRobotMoving)
            {
                // 로봇의 목표가 정확히 'PM A'일 때만 조건 검사
                if (robotSource == "PMA") // A에서 꺼낼 때 (Pick)
                {
                    // (1) 도착해서 집기 전(!HasWafer) OR (2) 집고 나오는 중(Retracting)
                    if (!robotHasWafer || isRetracting) shouldOpenA = true;
                }
                else if (robotDestination == "PMA") // A에 넣을 때 (Place)
                {
                    // (1) 도착해서 놓기 전(HasWafer) OR (2) 놓고 나오는 중(Retracting)
                    if (robotHasWafer || isRetracting) shouldOpenA = true;
                }
            }

            EtherCAT_M.Digital_Output(3, statusPmA == 1); // 램프
            EtherCAT_M.Digital_Output(4, !shouldOpenA);   // P104 (닫힘)
            EtherCAT_M.Digital_Output(5, shouldOpenA);    // P105 (열림)


            // =========================================================
            // 3. Chamber B 제어
            // =========================================================
            bool shouldOpenB = false;

            if (isRobotMoving)
            {
                // 로봇의 목표가 정확히 'PM B'일 때만 조건 검사
                if (robotSource == "PMB")
                {
                    if (!robotHasWafer || isRetracting) shouldOpenB = true;
                }
                else if (robotDestination == "PMB")
                {
                    if (robotHasWafer || isRetracting) shouldOpenB = true;
                }
            }

            EtherCAT_M.Digital_Output(6, statusPmB == 1);
            EtherCAT_M.Digital_Output(7, !shouldOpenB);   // P107
            EtherCAT_M.Digital_Output(8, shouldOpenB);    // P108


            // =========================================================
            // 4. Chamber C 제어
            // =========================================================
            bool shouldOpenC = false;

            if (isRobotMoving)
            {
                // 로봇의 목표가 정확히 'PM C'일 때만 조건 검사
                if (robotSource == "PMC")
                {
                    if (!robotHasWafer || isRetracting) shouldOpenC = true;
                }
                else if (robotDestination == "PMC")
                {
                    if (robotHasWafer || isRetracting) shouldOpenC = true;
                }
            }

            EtherCAT_M.Digital_Output(9, statusPmC == 1);
            EtherCAT_M.Digital_Output(10, !shouldOpenC);  // P110
            EtherCAT_M.Digital_Output(11, shouldOpenC);   // P111
        }

        // [추가] EtherCAT 연결/해제 버튼 핸들러
        
        private void MoveRobotHardware(string destination, int slotIndex = 0)
        {
            if (!isEtherConnected) return;

            long targetPosLR = 0;
            long targetPosUD = POS_UD_CHAMBER_PLACE; // 기본값

            // 1. 목적지에 따른 좌/우(Axis 2) 좌표 설정
            switch (destination)
            {
                case "FOUP_A": targetPosLR = POS_LR_FOUP_A; break;
                case "FOUP_B": targetPosLR = POS_LR_FOUP_B; break;
                case "PMA": targetPosLR = POS_LR_PMA; break;
                case "PMB": targetPosLR = POS_LR_PMB; break;
                case "PMC": targetPosLR = POS_LR_PMC; break;
            }

            // 2. 목적지에 따른 상/하(Axis 1) 좌표 설정
            if (destination.Contains("FOUP"))
            {
                // 슬롯 인덱스 보호 (0~4)
                int idx = Math.Max(0, Math.Min(slotIndex, 4));
                targetPosUD = POS_UD_FOUP_SLOTS[idx];
            }
            else
            {
                // 챔버는 안착 위치로 이동
                targetPosUD = POS_UD_CHAMBER_PLACE;
            }

            // 3. 실제 명령 전송 (참조 파일: Form1.cs - button1_Click 등 참고)
            // Axis 2 (좌/우) 이동
            EtherCAT_M.Axis2_LR_POS_Update(targetPosLR);
            EtherCAT_M.Axis2_LR_Move_Send();

            // Axis 1 (상/하) 이동
            EtherCAT_M.Axis1_UD_POS_Update(targetPosUD);
            EtherCAT_M.Axis1_UD_Move_Send();
        }

        // [신규] 도어 강제 개방 및 로봇 안전 이동 함수
        private void MoveRobotSafely(string destination, int slotIndex = 0)
        {
            if (!isEtherConnected) return;

            // 1. [중요] 기존 자동 제어 루프(SyncHardwareIO) 간섭 방지
            // 타이머가 돌고 있으면 도어를 닫으려고 할 수 있으므로 잠시 멈춥니다.
            if (sysTimer.Enabled)
            {
                sysTimer.Stop();
                // 필요 시 UI에 "Manual Mode" 표시 등 추가 가능
            }

            // 2. 모든 챔버 도어 강제 하강(Open)
            // Chamber A
            EtherCAT_M.Digital_Output(4, false); // 상승 OFF
            EtherCAT_M.Digital_Output(5, true);  // 하강 ON
                                                 // Chamber B
            EtherCAT_M.Digital_Output(7, false); // 상승 OFF
            EtherCAT_M.Digital_Output(8, true);  // 하강 ON
                                                 // Chamber C
            EtherCAT_M.Digital_Output(10, false); // 상승 OFF
            EtherCAT_M.Digital_Output(11, true);  // 하강 ON

            // 3. 로봇 하드웨어 이동 명령
            // 기존에 만들어둔 하드웨어 이동 함수 재사용
            MoveRobotHardware(destination, slotIndex);

            AddLog("Info", "Safety", $"Safety Move: Doors Opened & Robot Moving to {destination}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MoveRobotSafely("PMA", 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EtherCAT_M.Axis2_LR_POS_Update(13500);
            EtherCAT_M.Axis2_LR_Move_Send();
        }
    }*/
    }
}