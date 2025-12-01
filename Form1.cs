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

        // [수정] 로봇 동작 상태 상수 추가 (WAIT)
        private const int ROBOT_STATE_ROTATE = 0;
        private const int ROBOT_STATE_EXTEND = 1;
        private const int ROBOT_STATE_WAIT = 2;   // [NEW] 대기 상태
        private const int ROBOT_STATE_RETRACT = 3; // 번호 밀림
        private int currentRobotState = ROBOT_STATE_ROTATE;

        // [추가] 대기 시간 카운터 변수
        private int robotWaitCounter = 0;
        private const int ROBOT_WAIT_TICKS = 10; // 10 * 50ms = 0.5초 대기

        private float robotSpeed = 10.0f;

        private const float ANG_PMC = 0;
        // [수정] 화면 좌표에 맞춰 각도 보정 (기존 45/135 -> 54/126)
        private const float ANG_FOUP_B = 54;
        private const float ANG_FOUP_A = 126;
        private const float ANG_PMA = 180;
        private const float ANG_PMB = 270;

        private Button btnAutoRun;
        private int currentAlarmLevel = 0;

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
                AddLog("Info", "FOUP A", "Carrier Loaded manually");
            };
            this.btnUnloadA.Click += (s, e) => { foupACount = 0; UpdateWaferUI(); };
            this.btnLoadB.Click += (s, e) => { foupBCount = 5; UpdateWaferUI(); };
            this.btnUnloadB.Click += (s, e) => { foupBCount = 0; UpdateWaferUI(); };

            this.btnResetChambers.Click += BtnResetChambers_Click;

            this.pnlCenter.SizeChanged += (s, e) => UpdateLayout();
            this.pnlCenter.Paint += pnlCenter_Paint;
            this.pnlAlarm.Paint += pnlAlarm_Paint;

            SetLoginState(false);
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
            statusPmA = 0; progressA = 0;
            statusPmB = 0; progressB = 0;
            statusPmC = 0; progressC = 0;

            UpdateProcessUI();
            pnlCenter.Invalidate();

            AddLog("Info", "System", "Chambers reset by user");
            MessageBox.Show("모든 챔버 상태가 초기화되었습니다.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            btnAutoRun.Click += (s, e) => ToggleAutoRun();
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

            if (isRobotMoving)
            {
                AnimateRobot();
                pnlCenter.Invalidate();
                return;
            }

            SimulateProcess();

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

            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);

            g.TranslateTransform(cx, cy);
            g.RotateTransform(robotAngle);

            g.FillRectangle(Brushes.DimGray, -20, -20, 100, 40);

            float armX = 0 + robotExtension;
            g.FillRectangle(Brushes.Gray, armX, -15, 100, 30);

            if (robotHasWafer)
            {
                g.FillEllipse(Brushes.CornflowerBlue, armX + 70, -20, 40, 40);
            }

            g.ResetTransform();

            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(isAutoRun ? Brushes.Maroon : Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Olive, lightX, lightY + 30, 30, 30);
            g.FillRectangle(isAutoRun ? Brushes.Lime : Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);
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
                        currentRobotState = ROBOT_STATE_EXTEND;
                    }
                    break;

                case ROBOT_STATE_EXTEND:
                    robotExtension += EXTENSION_SPEED;
                    if (robotExtension >= MAX_EXTENSION)
                    {
                        robotExtension = MAX_EXTENSION;
                        // [수정] Extend 완료 후 바로 접지 않고 WAIT 상태로 전환
                        currentRobotState = ROBOT_STATE_WAIT;
                        robotWaitCounter = ROBOT_WAIT_TICKS; // 대기 시간 설정
                    }
                    break;

                case ROBOT_STATE_WAIT:
                    // [추가] 대기 로직 수행
                    if (robotWaitCounter > 0)
                    {
                        robotWaitCounter--;
                    }
                    else
                    {
                        // 대기 끝, 데이터 처리(Pick/Place) 수행
                        PerformRobotAction();
                        currentRobotState = ROBOT_STATE_RETRACT; // 다음 상태: 팔 접기
                    }
                    break;

                case ROBOT_STATE_RETRACT:
                    robotExtension -= EXTENSION_SPEED;
                    if (robotExtension <= 0)
                    {
                        robotExtension = 0;
                        currentRobotState = ROBOT_STATE_ROTATE;

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

        // [추가] 로봇이 완전히 뻗은 후(WAIT 끝난 후) 실행할 데이터 처리 로직 분리
        private void PerformRobotAction()
        {
            if (!robotHasWafer) // 빈손이면 집는다 (Pick)
            {
                robotHasWafer = true;
                if (robotSource == "FOUP_A") foupACount--;
                else if (robotSource == "PMA") statusPmA = 0;
                else if (robotSource == "PMB") statusPmB = 0;
                else if (robotSource == "PMC") statusPmC = 0;

                UpdateWaferUI();
            }
            else // 웨이퍼 있으면 놓는다 (Place)
            {
                robotHasWafer = false;
                if (robotDestination == "PMA") { statusPmA = 1; progressA = 0; }
                else if (robotDestination == "PMB") { statusPmB = 1; progressB = 0; }
                else if (robotDestination == "PMC") { statusPmC = 1; progressC = 0; }
                else if (robotDestination == "FOUP_B") { foupBCount++; UpdateWaferUI(); }
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

            pnlWaferL1.BackColor = foupACount >= 5 ? Color.Blue : Color.Black;
            pnlWaferL2.BackColor = foupACount >= 4 ? Color.Blue : Color.Black;
            pnlWaferL3.BackColor = foupACount >= 3 ? Color.Blue : Color.Black;
            pnlWaferL4.BackColor = foupACount >= 2 ? Color.Blue : Color.Black;
            pnlWaferL5.BackColor = foupACount >= 1 ? Color.Blue : Color.Black;

            pnlWaferR1.BackColor = foupBCount >= 5 ? Color.Blue : Color.Black;
            pnlWaferR2.BackColor = foupBCount >= 4 ? Color.Blue : Color.Black;
            pnlWaferR3.BackColor = foupBCount >= 3 ? Color.Blue : Color.Black;
            pnlWaferR4.BackColor = foupBCount >= 2 ? Color.Blue : Color.Black;
            pnlWaferR5.BackColor = foupBCount >= 1 ? Color.Blue : Color.Black;
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

            pnlChamberB.Location = new Point(cx - 40, cy - 250);
            pnlChamberA.Location = new Point(cx - 250, cy - 50);
            pnlChamberC.Location = new Point(cx + 170, cy - 50);
            pnlFoupA.Location = new Point(cx - 200, cy + 180);
            pnlFoupB.Location = new Point(cx + 120, cy + 180);
            pnlCassetteL.Location = new Point(cx - 100, cy + 330);
            pnlCassetteR.Location = new Point(cx + 20, cy + 330);

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