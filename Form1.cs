using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        // [추가] DB 연결 문자열
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

            this.btnConnect.Click += (s, e) => {
                MessageBox.Show("EtherCAT 연결 기능은 추후 구현 예정입니다.", "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            this.btnLoadA.Click += (s, e) => {
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

            cc.ConfigSaved += (s2, e2) => {
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

        private void BtnResetChambers_Click(object sender, EventArgs e)
        {
            if (isAutoRun) ToggleAutoRun();

            statusPmA = 0; progressA = 0;
            statusPmB = 0; progressB = 0;
            statusPmC = 0; progressC = 0;

            UpdateProcessUI();
            ResetRobotState();
            pnlCenter.Invalidate();

            AddLog("Info", "System", "System reset by user");
            MessageBox.Show("시스템 상태가 초기화되었습니다. 로봇이 초기 위치로 복귀합니다.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnResetRobot_Click(object sender, EventArgs e)
        {
            ResetRobotState();
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
            btnAutoRun.Click += (s, e) => {
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
            clockTimer.Tick += (s, e) => {
                if (lblTime != null) lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                if (lblDate != null) lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            };
            clockTimer.Start();
            sysTimer = new Timer();
            sysTimer.Interval = 50;
            sysTimer.Tick += SysTimer_Tick;
        }

        private void SysTimer_Tick(object sender, EventArgs e)
        {
            CheckAlarms();

            // 1. 공정 시뮬레이션: 로봇의 상태와 무관하게 항상 실행 (병렬 처리 효과)
            SimulateProcess();

            // 2. 로봇 애니메이션
            if (isRobotMoving)
            {
                AnimateRobot();
            }
            // 3. 리셋 처리 (로봇 이동이 끝났고 리셋 모드일 때)
            else if (isResetting)
            {
                isResetting = false;
                if (!isAutoRun) sysTimer.Stop();
            }
            // 4. 새로운 명령 확인 (로봇이 IDLE 상태이고 리셋 중이 아닐 때만)
            else
            {
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

            // 5. UI 업데이트 및 화면 다시 그리기
            UpdateProcessUI();
            pnlCenter.Invalidate();
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

            // 2. FOUP 그리기 (회전된 형태) - 고정됨
            // [수정] 원래 위치 각도(135, 45)를 그대로 사용하여 제자리에 배치
            // 내부는 DrawRotatedFoup에서 뒤집어서 그림
            DrawRotatedFoup(g, cx, cy, ANG_FOUP_A, "FOUP A", foupACount);
            DrawRotatedFoup(g, cx, cy, ANG_FOUP_B, "FOUP B", foupBCount);

            // [중요] 로봇 그리기 전 좌표계 초기화 (FOUP 회전이 로봇에 영향 주지 않게)
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

            // [중요] UI 그리기 전 좌표계 초기화 (로봇 회전이 신호등에 영향 주지 않게)
            g.ResetTransform();

            // 4. 신호등 그리기 (우측 상단 고정)
            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(isAutoRun ? Brushes.Maroon : Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Olive, lightX, lightY + 30, 30, 30);
            g.FillRectangle(isAutoRun ? Brushes.Lime : Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);
        }

        private void DrawRotatedFoup(Graphics g, int cx, int cy, float angle, string label, int waferCount)
        {
            var state = g.Save(); // 좌표계 저장

            // 1. 화면 중심으로 이동
            g.TranslateTransform(cx, cy);

            // 2. 해당 FOUP의 위치 각도(135도, 45도)만큼 회전
            g.RotateTransform(angle);

            // 3. 중심에서 160 거리만큼 바깥으로 이동
            g.TranslateTransform(160, 0);

            // 4. [핵심] 180도 회전 -> 로컬 좌표계의 +X축(입구)이 로봇(중심)을 향하게 됨
            g.RotateTransform(180);

            // [1] FOUP 바닥면 (배경)
            Rectangle foupRect = new Rectangle(-40, -40, 80, 80);
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(240, 240, 240))) // 아주 밝은 회색
            {
                g.FillRectangle(bgBrush, foupRect);
            }

            // [2] 웨이퍼 그리기
            int maxWafers = 5;
            for (int i = 0; i < maxWafers; i++)
            {
                Color color = (i < waferCount) ? Color.Blue : Color.Black;

                using (SolidBrush waferBrush = new SolidBrush(color))
                {
                    float waferWidth = 10;  // 웨이퍼 두께 (X축 방향)
                    float waferLength = 70; // 웨이퍼 길이 (Y축 방향, 꽉 차게)

                    float xPos = -35 + (i * 14);
                    float yPos = -35; // 위쪽(-Y)에서 시작

                    g.FillRectangle(waferBrush, xPos, yPos, waferWidth, waferLength);
                }
            }

            // [3] 벽면 그리기 (ㄷ자 모양 - 뒤, 위, 아래 막힘)
            // Pen 두께를 두껍게 해서 벽 느낌을 냄
            using (Pen wallPen = new Pen(Color.DimGray, 4))
            {
                // 1. 위쪽 벽 (Side 1)
                g.DrawLine(wallPen, -40, -40, 40, -40);

                // 2. 아래쪽 벽 (Side 2)
                g.DrawLine(wallPen, -40, 40, 40, 40);

                // 3. 뒷면 벽 (Back - 로봇 반대편)
                // 모서리가 깔끔하게 만나도록 조정
                g.DrawLine(wallPen, -40, -42, -40, 42);
            }

            // [4] 라벨 그리기 (FOUP 바깥쪽)
            // 텍스트 위치: 박스 아래쪽이나 뒤쪽
            using (Font f = new Font("Arial", 10, FontStyle.Bold))
            {
                var labelState = g.Save();

                // 박스 '아래쪽(화면상)'에 글씨를 쓰기 위해 이동
                // 로컬 좌표계에서 Y=40이 한쪽 벽면임. 
                // 텍스트가 항상 화면 정방향으로 보이게 하려면 역회전 필요

                g.TranslateTransform(-50, 0); // 박스 뒤쪽으로 이동
                g.RotateTransform(90);

                SizeF size = g.MeasureString(label, f);
                g.DrawString(label, f, Brushes.Black, -size.Width / 2, -size.Height / 2);

                g.Restore(labelState);
            }

            g.Restore(state); // 좌표계 복구
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
                    }
                    break;

                case ROBOT_STATE_EXTEND:
                    robotExtension += EXTENSION_SPEED;
                    if (robotExtension >= MAX_EXTENSION)
                    {
                        robotExtension = MAX_EXTENSION;
                        currentRobotState = ROBOT_STATE_WAIT;
                        robotWaitCounter = ROBOT_WAIT_TICKS;
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
                    }
                    break;

                case ROBOT_STATE_RETRACT:
                    robotExtension -= EXTENSION_SPEED;
                    if (robotExtension <= 0)
                    {
                        robotExtension = 0;
                        currentRobotState = ROBOT_STATE_ROTATE;

                        if (isResetting)
                        {
                            targetAngle = 180;
                            return;
                        }

                        if (robotHasWafer)
                        {
                            targetAngle = GetAngle(robotDestination);
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
                else if (robotSource == "PMA") statusPmA = 0;
                else if (robotSource == "PMB") statusPmB = 0;
                else if (robotSource == "PMC") statusPmC = 0;

                UpdateWaferUI();
                pnlCenter.Invalidate(); // FOUP 그리기 갱신
            }
            else // Place
            {
                robotHasWafer = false;
                if (robotDestination == "PMA") { statusPmA = 1; progressA = 0; }
                else if (robotDestination == "PMB") { statusPmB = 1; progressB = 0; }
                else if (robotDestination == "PMC") { statusPmC = 1; progressC = 0; }
                else if (robotDestination == "FOUP_B") { foupBCount++; UpdateWaferUI(); pnlCenter.Invalidate(); }
            }
        }

        private void SimulateProcess()
        {
            if (statusPmA == 1)
            {
                progressA += (100.0 / (timePmA * 20));
                if (progressA >= 100) { progressA = 100; statusPmA = 2; AddLog("Process", "PM A", "Process Complete"); }
            }
            if (statusPmB == 1)
            {
                progressB += (100.0 / (timePmB * 20));
                if (progressB >= 100) { progressB = 100; statusPmB = 2; AddLog("Process", "PM B", "Process Complete"); }
            }
            if (statusPmC == 1)
            {
                progressC += (100.0 / (timePmC * 20));
                if (progressC >= 100) { progressC = 100; statusPmC = 2; AddLog("Process", "PM C", "Process Complete"); }
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
            progA.Value = (int)Math.Min(progressA, 100);
            pnlChamberA.BackColor = GetStateColor(statusPmA);
            progB.Value = (int)Math.Min(progressB, 100);
            pnlChamberB.BackColor = GetStateColor(statusPmB);
            progC.Value = (int)Math.Min(progressC, 100);
            pnlChamberC.BackColor = GetStateColor(statusPmC);
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

            // 기존 패널 색상 변경 코드는 이제 무의미하지만(패널 숨김), 
            // 나중에 참조용으로 남겨두거나 삭제해도 무방함.
            // pnlWaferL1.BackColor ... (화면에 안보임)
        }

        private void ApplyRecipeData(RecipeControl.RecipeModel data)
        {
            if (data.PmA_Params != null)
            {
                valTargetA.Text = data.PmA_Params[0]; valGasA.Text = data.PmA_Params[1]; valTimeA.Text = data.PmA_Params[2];
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

            // [수정] 챔버 위치 조정 (바깥으로 조금 더 이동)
            pnlChamberB.Location = new Point(cx - 40, cy - 220);
            pnlChamberA.Location = new Point(cx - 210, cy - 50);
            pnlChamberC.Location = new Point(cx + 120, cy - 50);

            // [수정] FOUP 위치 조정 (45도/135도 각도에 맞춰 X, Y 오프셋을 1:1로 설정)
            // 중심에서 약 155px 거리 (X:110, Y:110 오프셋)
            // FOUP A (135도) -> 왼쪽(-110), 아래(+110)
            pnlFoupA.Location = new Point(cx - 150, cy + 70);
            // FOUP B (45도) -> 오른쪽(+110), 아래(+110)
            pnlFoupB.Location = new Point(cx + 70, cy + 70);

            // [수정] 카세트(웨이퍼 슬롯) 위치 조정 (FOUP 바로 아래 정렬)
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
    }
}