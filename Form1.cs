using IEG3268_Dll;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        // EtherCAT 통신 라이브러리 인스턴스 (모터, IO 제어용)
        IEG3268 EtherCAT_M = new IEG3268();

        // EtherCAT 하드웨어 연결 상태 플래그 (true: 연결됨, false: 연결 안됨)
        bool isEtherConnected = false;

        // [추가] 시뮬레이션 모드 플래그 (True면 AnimateRobot의 자동 상태 전환을 막음)
        private bool isSimulation = false;

        // -------------------------------------------------------------------------
        // [1] 좌/우 서보 (Axis 2) 위치 값 상수 (단위: Pulse)
        // 각 모듈(FOUP, PM)의 좌우 기준 좌표
        // -------------------------------------------------------------------------
        private const long FOUP_A_HPOS = 13500;   // FOUP A의 좌우 위치
        private const long FOUP_B_HPOS = -394700; // FOUP B의 좌우 위치
        private const long PM_A_HPOS = -59064;     // PM A 챔버의 좌우 위치
        private const long PM_B_HPOS = -190823;    // PM B 챔버의 좌우 위치
        private const long PM_C_HPOS = -322000;    // PM C 챔버의 좌우 위치

        // -------------------------------------------------------------------------
        // [2] 상/하 서보 (Axis 1) 위치 값 상수 (단위: Pulse)
        // -------------------------------------------------------------------------
        // 챔버 공통 높이
        private const long CHAMBER_VPOS = 1150000; // 챔버 진입 전 안전하게 상승한 높이 (이동 시 충돌 방지)
        private const long CHAMBER_PLACE_VPOS = 806931; // 챔버 내부에 웨이퍼를 내려놓거나 집는 높이 (안착 위치)

        // FOUP 슬롯별 높이 (Slot 1 ~ 5)
        // FOUP은 여러 층으로 되어 있으므로 각 슬롯의 높이를 배열로 관리
        private readonly long[] FOUP_SLOTS_POS = new long[]
        {
            290000,   // Slot 1 (가장 아래 칸) 높이
            982378,   // Slot 2 높이
            1627604,  // Slot 3 높이
            2332102,  // Slot 4 높이
            3018457   // Slot 5 (가장 위 칸) 높이
        };
        private readonly long[] FOUP_SLOTS_IN_POS = new long[]
        {
            40000, // Slot 1 진입 높이
            782378, // Slot 2 진입 높이
            1450000, // Slot 3 진입 높이
            2119399, // Slot 4 진입 높이
            2818463, // Slot 5 진입 높이
        };

        // -------------------------------------------------------------------------
        // [3] 로봇 실린더 IO 번호 (디지털 출력 포트 번호)
        // -------------------------------------------------------------------------
        private const int ROBOT_FORWARD_PORT = 12;  // 로봇 팔 전진(Extend) 신호 출력 포트
        private const int ROBOT_BACKWARD_PORT = 13; // 로봇 팔 후진(Retract) 신호 출력 포트
        private const int ROBOT_VACUUM_ON_PORT = 14;     // 진공 흡착(Vacuum On) 신호 출력 포트 (웨이퍼 잡기)
        private const int ROBOT_VACUUM_OFF_PORT = 15;    // 진공 해제(Vacuum Off) 신호 출력 포트 (웨이퍼 놓기 - 필요 시)

        // [DB 연결 문자열] MySQL 데이터베이스 접속 정보
        private string connectionString = "Server=localhost;Port=3306;Database=SemiGuiData;Uid=root;Pwd=1234;Charset=utf8;";

        // ... (기존 변수 유지) ...

        // 사용자 로그인 상태 (true: 로그인 됨)
        private bool isLoggedIn = false;

        // 자동 운전(Auto Run) 모드 활성화 여부 (true: 자동 운전 중)
        private bool isAutoRun = false;

        // 시계 표시용 타이머 (1초 간격)
        private Timer clockTimer;

        // 시스템 로직 처리용 메인 타이머 (50ms 간격, 로봇 이동 및 상태 체크)
        private Timer sysTimer;

        // 각 FOUP에 현재 남아있는 웨이퍼 개수
        private int foupACount = 5; // FOUP A 초기 웨이퍼 개수
        private int foupBCount = 0; // FOUP B 초기 웨이퍼 개수

        // 각 챔버(Process Module)의 공정 상태
        // 0: Idle(대기/빔), 1: Processing(공정 진행 중), 2: Complete(공정 완료/웨이퍼 있음)
        private int statusPmA = 0;
        private int statusPmB = 0;
        private int statusPmC = 0;

        // 각 챔버의 공정 진행률 (0.0 ~ 100.0 %)
        private double progressA = 0;
        private double progressB = 0;
        private double progressC = 0;

        // [추가] 각 챔버별 공정 비동기 작업(Task)이 실행 중인지 확인하는 플래그
        // 중복 실행 방지 용도
        private bool isRunningPmA = false;
        private bool isRunningPmB = false;
        private bool isRunningPmC = false;

        // 각 챔버의 공정 소요 시간 설정값 (초 단위, 레시피에서 로드됨)
        private int timePmA = 5;
        private int timePmB = 5;
        private int timePmC = 5;

        // 각 챔버별 도어 상태
        private bool isDoorOpenPmA = false;
        private bool isDoorOpenPmB = false;
        private bool isDoorOpenPmC = false;

        // 로봇 애니메이션용 현재 각도 (UI 표시용)
        private float robotAngle = 180;

        // 로봇이 회전해야 할 목표 각도
        private float targetAngle = 0;

        // 로봇이 현재 이동(회전 또는 팔 동작) 중인지 여부
        private bool isRobotMoving = false;

        // 로봇이 현재 웨이퍼를 가지고 있는지 여부 (true: 있음, false: 없음)
        private bool robotHasWafer = false;

        // 로봇의 현재 이동 목적지 이름 ("PMA", "FOUP_A" 등)
        private string robotDestination = "";

        // 로봇이 웨이퍼를 가져온 출발지 이름
        private string robotSource = "";

        // 로봇 팔의 현재 연장 길이 (애니메이션용, 0 ~ MAX_EXTENSION)
        private float robotExtension = 0;

        // 로봇 팔의 최대 연장 길이 상수
        private const float MAX_EXTENSION = 60.0f;

        // 로봇 팔이 연장/수축되는 속도 (틱당 길이 변화량)
        private const float EXTENSION_SPEED = 5.0f;

        // 로봇 동작 상태 상수 정의
        private const int ROBOT_STATE_ROTATE = 0;  // 회전 중
        private const int ROBOT_STATE_FORWARD = 1;  // 팔 뻗는 중
        private const int ROBOT_STATE_WAIT = 2;    // 동작 대기 중 (집거나 놓는 시간)
        private const int ROBOT_STATE_BACKWARD = 3; // 팔 접는 중

        // 로봇 동작 가능 여부
        private bool ROBOT_HORIZONTAL_MOVE = true; // 좌우(수평) 이동 허용
        private bool ROBOT_VERTICAL_MOVE = true;   // 상하(수직) 이동 허용
        private bool ROBOT_FORWARD_MOVE = true;    // 팔 전진 이동 허용
        private bool ROBOT_BACKWARD_MOVE = true;    // 팔 전진 이동 허용

        // 현재 로봇의 동작 상태 (위 상수 중 하나)
        private int currentRobotState = ROBOT_STATE_ROTATE;

        // 로봇 대기 시간 카운터 (ROBOT_STATE_WAIT 상태에서 사용)
        private int robotWaitCounter = 0;

        // 로봇 대기 시간 틱 수 (이 값만큼 대기 후 다음 동작)
        private const int ROBOT_WAIT_TICKS = 10;

        // 로봇 회전 속도 (틱당 회전 각도, Config에서 설정 가능)
        private float robotSpeed = 10.0f;

        // 각 모듈의 UI 상 표시 각도 (FOUP, 챔버의 배치 각도)
        private const float ANG_PMC = 0;
        private const float ANG_FOUP_B = 45;
        private const float ANG_FOUP_A = 135;
        private const float ANG_PMA = 180;
        private const float ANG_PMB = 270;

        // 자동 운전 버튼 컨트롤 객체
        private Button btnAutoRun;

        // 현재 시스템 알람 레벨 (0: 정상, 1: 경고, 2: 에러/위험)
        private int currentAlarmLevel = 0;

        // 시스템 리셋(초기화) 진행 중 여부 플래그
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
            this.btnRobot.Click += BtnRobot_Click;
            this.btnTestSimul.Click += btnTestSimul_Click;

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

        private void BtnRobot_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
                currentRobotState = ROBOT_STATE_BACKWARD;
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

            // [수정] 시뮬레이션 중이 아닐 때만 자동 공정 로직(RunProcessAsync)을 실행
            if (!isSimulation)
            {
                // 1. 공정 시뮬레이션 시작 트리거 (자동/수동 모드용)
                // 시뮬레이션 모드일 때는 이 부분이 실행되지 않아 충돌이 사라짐
                if (statusPmA == 1 && !isRunningPmA) RunProcessAsync("PMA", timePmA);
                if (statusPmB == 1 && !isRunningPmB) RunProcessAsync("PMB", timePmB);
                if (statusPmC == 1 && !isRunningPmC) RunProcessAsync("PMC", timePmC);
            }

            // 2. 로봇 애니메이션 (시뮬레이션 중에도 로봇은 움직여야 하므로 유지)
            if (isRobotMoving)
            {
                AnimateRobot();
                pnlCenter.Invalidate();
            }
            // 3. 리셋 처리
            else if (isResetting)
            {
                isResetting = false;
                if (!isAutoRun) sysTimer.Stop();
            }
            // 4. 새로운 명령 확인 (시뮬레이션 중에는 자동 스케줄링 금지)
            else if (!isSimulation)
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

            // UI 업데이트 (항상 실행)
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

                    // 1. 각도 정규화 (-180 ~ 180)
                    while (diff <= -180) diff += 360;
                    while (diff > 180) diff -= 360;

                    // ----------------------------------------------------------------------
                    // [Dead Zone 회피 로직 - Zone 기반]
                    // 90도 선(FOUP A와 B 사이)을 절대 넘지 않도록 방향을 강제함
                    // ----------------------------------------------------------------------

                    // 목표가 왼쪽 구역(90~270도)에 있는지 확인 (PM A, FOUP A)
                    bool isTargetLeft = (targetAngle > 90 && targetAngle < 270);

                    // 현재 로봇이 왼쪽 구역(90~270도)에 있는지 확인
                    bool isCurrentLeft = (robotAngle > 90 && robotAngle < 270);

                    // 서로 다른 구역으로 이동해야 한다면? (반드시 90도 벽을 우회해야 함)
                    if (isTargetLeft != isCurrentLeft)
                    {
                        if (isTargetLeft)
                        {
                            // 목표가 왼쪽(135, 180 등)인데 현재 오른쪽(0, 45 등)에 있다면
                            // 무조건 시계반대방향(CCW, 음수)으로 돌아서 270도 쪽으로 가야 함
                            if (diff > 0) diff -= 360;
                        }
                        else
                        {
                            // 목표가 오른쪽(0, 45 등)인데 현재 왼쪽(135, 180 등)에 있다면
                            // 무조건 시계방향(CW, 양수)으로 돌아서 270도 쪽으로 가야 함
                            if (diff < 0) diff += 360;
                        }
                    }
                    // ----------------------------------------------------------------------

                    if (Math.Abs(diff) > speed)
                    {
                        if (diff > 0) robotAngle += speed;
                        else robotAngle -= speed;

                        // 0~360 보정
                        if (robotAngle >= 360) robotAngle -= 360;
                        if (robotAngle < 0) robotAngle += 360;
                    }
                    else
                    {
                        robotAngle = targetAngle;

                        if (isSimulation) return; // 시뮬레이션 모드면 여기서 대기

                        if (isResetting && robotAngle == 180)
                        {
                            isRobotMoving = false;
                            return;
                        }

                        currentRobotState = ROBOT_STATE_FORWARD;

                        if (isEtherConnected)
                        {
                            EtherCAT_M.Digital_Output(ROBOT_BACKWARD_PORT, false);
                            EtherCAT_M.Digital_Output(ROBOT_FORWARD_PORT, true);
                        }
                    }
                    break;

                // ... (나머지 Case는 기존 코드 유지) ...
                case ROBOT_STATE_FORWARD:
                    robotExtension += EXTENSION_SPEED;
                    if (robotExtension >= MAX_EXTENSION)
                    {
                        robotExtension = MAX_EXTENSION;
                        if (isSimulation) return;
                        currentRobotState = ROBOT_STATE_WAIT;
                        robotWaitCounter = ROBOT_WAIT_TICKS;
                        if (isEtherConnected)
                        {
                            if (!robotHasWafer) EtherCAT_M.Digital_Output(ROBOT_VACUUM_ON_PORT, true);
                            else EtherCAT_M.Digital_Output(ROBOT_VACUUM_ON_PORT, false);
                        }
                    }
                    break;

                case ROBOT_STATE_WAIT:
                    if (robotWaitCounter > 0) robotWaitCounter--;
                    else
                    {
                        if (isSimulation) return;
                        if (!isResetting) PerformRobotAction();
                        currentRobotState = ROBOT_STATE_BACKWARD;
                        if (isEtherConnected)
                        {
                            EtherCAT_M.Digital_Output(ROBOT_FORWARD_PORT, false);
                            EtherCAT_M.Digital_Output(ROBOT_BACKWARD_PORT, true);
                        }
                    }
                    break;

                case ROBOT_STATE_BACKWARD:
                    robotExtension -= EXTENSION_SPEED;
                    if (robotExtension <= 0)
                    {
                        robotExtension = 0;
                        if (isSimulation) return;
                        currentRobotState = ROBOT_STATE_ROTATE;

                        if (isResetting) { targetAngle = 180; return; }

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

        // 로봇 좌우 이동
        private async Task WaitAxis1(long targetPos)
        {
            int timeout = 0;
            while (timeout < 100) // 약 10초 대기
            {
                try
                {
                    string sPos = EtherCAT_M.Axis1_is_PosData();
                    long curPos = long.Parse(sPos);
                    // 오차 100 Pulse 이내면 도착으로 간주
                    if (Math.Abs(curPos - targetPos) < 100) return;
                }
                catch { }
                await Task.Delay(100);
                timeout++;
            }
            throw new Exception("Axis 1 Move Timeout");
        }

        // [Helper] Axis 2(좌우) 이동 및 대기 (기존에 없어서 유지)
        private async Task MoveAxis2_Wait(long targetPos)
        {
            EtherCAT_M.Axis2_LR_POS_Update(targetPos);
            EtherCAT_M.Axis2_LR_Move_Send();

            int timeout = 0;
            while (timeout < 100)
            {
                try
                {
                    string sPos = EtherCAT_M.Axis2_is_PosData();
                    long curPos = long.Parse(sPos);
                    if (Math.Abs(curPos - targetPos) < 100) return;
                }
                catch { }
                await Task.Delay(100);
                timeout++;
            }
            throw new Exception("Axis 2 Move Timeout");
        }

        private async void btnTestSimul_Click(object sender, EventArgs e)
        {
            // 시뮬레이션을 위해 타이머가 돌아가고 있어야 애니메이션이 그려짐
            if (!sysTimer.Enabled) sysTimer.Start();

            // 자동 운전과 충돌 방지
            isAutoRun = false;
            btnAutoRun.Text = "AUTO RUN";
            btnAutoRun.BackColor = Color.LightGray;

            isSimulation = true;

            await RunSimulationTesting();

            isSimulation = false;
        }

        private async Task RunSimulationTesting()
        {
            try
            {
                btnRobot.Enabled = false;
                AddLog("Simul", "Scheduler", "--- Parallel Simulation Start ---");

                // 종료 조건: FOUP A가 비어있고, 모든 챔버가 비워질(Idle) 때까지
                // (즉, 공장 내 모든 웨이퍼가 FOUP B로 나갈 때까지)
                while (foupACount > 0 || statusPmA != 0 || statusPmB != 0 || statusPmC != 0)
                {
                    bool actionTaken = false;

                    // -----------------------------------------------------------------
                    // [우선순위 1] 배출: PM C (완료) -> FOUP B
                    // -----------------------------------------------------------------
                    if (statusPmC == 2 && !robotHasWafer)
                    {
                        AddLog("Simul", "Job", "Unloading: PM C -> FOUP B");
                        await Transfer_Chamber_to_FoupB("PMC", ANG_PMC);
                        actionTaken = true;
                    }

                    // -----------------------------------------------------------------
                    // [우선순위 2] 이송: PM B (완료) -> PM C (비어있음)
                    // -----------------------------------------------------------------
                    else if (statusPmB == 2 && statusPmC == 0 && !robotHasWafer)
                    {
                        AddLog("Simul", "Job", "Transfer: PM B -> PM C");
                        await Transfer_Chamber_to_Chamber("PMB", ANG_PMB, "PMC", ANG_PMC);

                        // 옮겨놓고 로봇은 바로 빠지고, PM C 공정은 백그라운드에서 시작
                        _ = RunProcessBackground("PMC");
                        actionTaken = true;
                    }

                    // -----------------------------------------------------------------
                    // [우선순위 3] 이송: PM A (완료) -> PM B (비어있음)
                    // -----------------------------------------------------------------
                    else if (statusPmA == 2 && statusPmB == 0 && !robotHasWafer)
                    {
                        AddLog("Simul", "Job", "Transfer: PM A -> PM B");
                        await Transfer_Chamber_to_Chamber("PMA", ANG_PMA, "PMB", ANG_PMB);

                        _ = RunProcessBackground("PMB");
                        actionTaken = true;
                    }

                    // -----------------------------------------------------------------
                    // [우선순위 4] 투입: FOUP A (있음) -> PM A (비어있음)
                    // -----------------------------------------------------------------
                    else if (foupACount > 0 && statusPmA == 0 && !robotHasWafer)
                    {
                        AddLog("Simul", "Job", "Loading: FOUP A -> PM A");
                        await Transfer_FoupA_to_Chamber("PMA", ANG_PMA);

                        _ = RunProcessBackground("PMA");
                        actionTaken = true;
                    }

                    // 할 일이 없으면 잠시 대기 (CPU 과부하 방지 및 공정 완료 대기)
                    if (!actionTaken)
                    {
                        await Task.Delay(100);
                    }
                }

                AddLog("Simul", "Scheduler", "--- All Batches Completed ---");
                MessageBox.Show("모든 공정이 완료되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                btnRobot.Enabled = true;
                ResetSimulationState();
            }
        }

        // =========================================================================
        // [백그라운드 공정 시뮬레이션] - 로봇을 잡지 않고 혼자 돌아감
        // =========================================================================
        private async Task RunProcessBackground(string pmName)
        {
            // 1. 해당 챔버의 레시피 시간 가져오기
            int targetDurationSec = 5; // 기본값
            if (pmName == "PMA") targetDurationSec = timePmA;
            else if (pmName == "PMB") targetDurationSec = timePmB;
            else if (pmName == "PMC") targetDurationSec = timePmC;

            // 0초 이하 방지 (최소 1초)
            if (targetDurationSec < 1) targetDurationSec = 1;

            // 상태를 1(Processing)로 변경
            if (pmName == "PMA") statusPmA = 1;
            else if (pmName == "PMB") statusPmB = 1;
            else if (pmName == "PMC") statusPmC = 1;

            UpdateProcessUI();
            AddLog("Simul", "Process", $"{pmName} Started. (Recipe Time: {targetDurationSec}s)");

            // 2. 딜레이 계산
            // 루프는 0~100까지 5단위로 증가하므로 총 20단계 (100 / 5 = 20)
            // 각 단계별 딜레이(ms) = (목표시간(초) * 1000) / 20
            int totalSteps = 20;
            int stepDelay = (targetDurationSec * 1000) / totalSteps;

            // 진행률 애니메이션
            for (int i = 0; i <= 100; i += 5)
            {
                if (pmName == "PMA") progressA = i;
                else if (pmName == "PMB") progressB = i;
                else if (pmName == "PMC") progressC = i;

                UpdateProcessUI();

                // 계산된 시간만큼 대기 (레시피 시간에 따라 속도가 달라짐)
                await Task.Delay(stepDelay);
            }

            // 상태를 2(Done)로 변경
            if (pmName == "PMA") statusPmA = 2;
            else if (pmName == "PMB") statusPmB = 2;
            else if (pmName == "PMC") statusPmC = 2;

            UpdateProcessUI();
            AddLog("Simul", "Process", $"{pmName} Finished.");
            pnlCenter.Invalidate();
        }

        // =========================================================================
        // [이동 동작 모듈화]
        // =========================================================================

        // 1. 투입: FOUP A -> Chamber (수정됨: 진입 전 안전 확인)
        private async Task Transfer_FoupA_to_Chamber(string targetPm, float targetAng)
        {
            int slotIdx = foupACount - 1;

            // Go to FOUP A
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            await SimulateServoMove(ANG_FOUP_A);

            // ... (이하 로직 기존과 동일) ...
            await SimulateZMove(FOUP_SLOTS_IN_POS[slotIdx], "IN_POS");
            await SimulateCylinder("Forward");
            SetVacuum(true);
            foupACount--;
            UpdateWaferUI();
            await SimulateZMove(FOUP_SLOTS_POS[slotIdx], "PICK_POS");
            await SimulateCylinder("Backward");

            // Move to Chamber
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            await SimulateServoMove(targetAng);

            // Place
            SetDoorState(targetPm, true);
            await Task.Delay(300);
            await SimulateZMove(CHAMBER_PLACE_VPOS, "Place");
            await SimulateCylinder("Forward");
            SetVacuum(false);
            await SimulateCylinder("Backward");
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            SetDoorState(targetPm, false);
        }

        // 2. 이송: Chamber -> Chamber
        private async Task Transfer_Chamber_to_Chamber(string srcPm, float srcAng, string destPm, float destAng)
        {
            // 1. Source Pick
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            await SimulateServoMove(srcAng);
            SetDoorState(srcPm, true);
            await Task.Delay(300);

            // Pick 동작 (이미 완료된 웨이퍼 집기)
            await SimulateZMove(CHAMBER_PLACE_VPOS, "Pick Height");
            await SimulateCylinder("Forward");
            SetVacuum(true);

            // 집어오면 Source는 비게 됨 (상태 0)
            if (srcPm == "PMA") statusPmA = 0;
            else if (srcPm == "PMB") statusPmB = 0;
            UpdateProcessUI(); // 비워진 상태 즉시 반영

            await SimulateCylinder("Backward");
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            SetDoorState(srcPm, false);

            // 2. Dest Place
            await SimulateServoMove(destAng);
            SetDoorState(destPm, true);
            await Task.Delay(300);

            await SimulateZMove(CHAMBER_PLACE_VPOS, "Place Height");
            await SimulateCylinder("Forward");
            SetVacuum(false);
            await SimulateCylinder("Backward");
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            SetDoorState(destPm, false);
        }

        // 3. 배출: Chamber -> FOUP B
        // 3. 배출: Chamber -> FOUP B (수정됨: 작업 후 안전 위치 복귀)
        private async Task Transfer_Chamber_to_FoupB(string srcPm, float srcAng)
        {
            int slotIdx = foupBCount;

            // ... (Pick 로직 기존과 동일) ...
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            await SimulateServoMove(srcAng);
            SetDoorState(srcPm, true);
            await Task.Delay(300);

            await SimulateZMove(CHAMBER_PLACE_VPOS, "Pick");
            await SimulateCylinder("Forward");
            SetVacuum(true);

            if (srcPm == "PMC") statusPmC = 0;
            UpdateProcessUI();

            await SimulateCylinder("Backward");
            await SimulateZMove(CHAMBER_VPOS, "Safe");
            SetDoorState(srcPm, false);

            // ... (Place 로직 기존과 동일) ...
            await SimulateServoMove(ANG_FOUP_B);

            await SimulateZMove(FOUP_SLOTS_POS[slotIdx], "DROP_POS");
            await SimulateCylinder("Forward");
            SetVacuum(false);
            foupBCount++;
            UpdateWaferUI();

            await SimulateZMove(FOUP_SLOTS_IN_POS[slotIdx], "CLEARANCE");
            await SimulateCylinder("Backward");
            await SimulateZMove(CHAMBER_VPOS, "Safe");
        }

        // [헬퍼] Z축 이동 시뮬레이션 (로그 및 딜레이)
        private async Task SimulateZMove(long pos, string desc)
        {
            //AddLog("Simul", "Axis1", $"Moving Z to {desc} ({pos})");
            // 실제 이동처럼 보이게 약간의 딜레이
            await Task.Delay(600);
        }

        // [헬퍼] 진공 상태 설정 및 UI 갱신
        private void SetVacuum(bool on)
        {
            robotHasWafer = on;
            AddLog("Simul", "IO", on ? "Vacuum ON" : "Vacuum OFF");
            pnlCenter.Invalidate(); // 로봇 웨이퍼 그림 갱신
        }

        // [헬퍼] 챔버 상태 설정 및 UI 갱신
        private void SetChamberStatus(string pmName, int status)
        {
            if (pmName == "PMA") statusPmA = status;
            else if (pmName == "PMB") statusPmB = status;
            else if (pmName == "PMC") statusPmC = status;
            UpdateProcessUI();
            pnlCenter.Invalidate();
        }

        // [헬퍼] 도어 상태 설정
        private void SetDoorState(string pmName, bool isOpen)
        {
            if (pmName == "PMA") isDoorOpenPmA = isOpen;
            else if (pmName == "PMB") isDoorOpenPmB = isOpen;
            else if (pmName == "PMC") isDoorOpenPmC = isOpen;
            //AddLog("Simul", "Door", $"{pmName} Door {(isOpen ? "Open" : "Close")}");
            pnlCenter.Invalidate();
        }

        // [헬퍼] 시뮬레이션 종료 후 정리
        private void ResetSimulationState()
        {
            robotSource = "";
            robotDestination = "";
            isRobotMoving = false;
        }

        // -------------------------------------------------------------------------
        // [시뮬레이션 헬퍼 함수들]
        // -------------------------------------------------------------------------

        // 1. 서보 회전 시뮬레이션 (목표 각도까지 기다림)
        private async Task SimulateServoMove(float targetAng)
        {
            targetAngle = targetAng;
            currentRobotState = ROBOT_STATE_ROTATE;
            isRobotMoving = true; // SysTimer가 AnimateRobot을 호출하게 함

            // 각도가 거의 일치할 때까지 대기
            while (Math.Abs(robotAngle - targetAng) > 1.0f)
            {
                await Task.Delay(50);
            }
            isRobotMoving = false; // 도착하면 애니메이션 중단 플래그 (필요시)
        }

        // 2. 실린더 동작 시뮬레이션 (MAX 또는 0이 될 때까지 기다림)
        private async Task SimulateCylinder(string cmd)
        {
            isRobotMoving = true;

            if (cmd == "Forward")
            {
                currentRobotState = ROBOT_STATE_FORWARD;
                // SysTimer가 ROBOT_STATE_FORWARD 상태를 보고 robotExtension을 증가시킴

                // 완전히 뻗을 때까지 대기
                while (robotExtension < MAX_EXTENSION - 1.0f)
                {
                    await Task.Delay(50);
                }
            }
            else // Backward
            {
                currentRobotState = ROBOT_STATE_BACKWARD;

                // 완전히 접힐 때까지 대기
                while (robotExtension > 1.0f)
                {
                    await Task.Delay(50);
                }
            }
        }


        private async void testing()
        {
            if (!isEtherConnected)
            {
                AddLog("Error", "Test", "EtherCAT Disconnected.");
                MessageBox.Show("장비 연결 확인 필요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnRobot.Enabled = false;
                AddLog("Info", "Test", "--- Pick Sequence Test (Scoop Motion) ---");

                // -----------------------------------------------------------------
                // [STEP 1] 초기 이동 (FOUP A 앞, 진입 높이로 이동)
                // -----------------------------------------------------------------

                // 1. Z축 안전 높이 이동
                CylinderVerticalMove((int)CHAMBER_VPOS);
                await WaitAxis1(CHAMBER_VPOS);

                // 2. X축 FOUP A 위치 이동
                await MoveAxis2_Wait(FOUP_A_HPOS);

                // 3. Z축 진입 높이(IN_POS)로 하강 (웨이퍼 밑으로 들어갈 높이)
                CylinderVerticalMove((int)FOUP_SLOTS_IN_POS[0]);
                await WaitAxis1(FOUP_SLOTS_IN_POS[0]);

                // -----------------------------------------------------------------
                // [STEP 2] Pick 동작 (진입 -> 진공 -> 상승 -> 후진)
                // -----------------------------------------------------------------

                // 1. 실린더 전진 (웨이퍼 하단으로 진입)
                if (!CheckRobotSafety("Forward"))
                    throw new Exception("Safety Check Failed: Cannot Enter FOUP A");

                CylinderMove("Forward");
                await Task.Delay(2000);

                // 2. 진공 ON
                EtherCAT_M.Digital_Output(ROBOT_VACUUM_ON_PORT, true);
                AddLog("Info", "Test", "Vacuum ON");
                await Task.Delay(500);

                // 3. Z축 상승 (웨이퍼를 들어 올림: IN_POS -> POS)
                // 중요: 팔이 전진된 상태에서 상하 이동을 하므로, 미세한 이동이나 안전 범위 내여야 함
                // 하드웨어적으로 전진 상태에서 Z축 이동이 가능한지 확인 필요 (일반적으로 Scoop 동작엔 필수)
                CylinderVerticalMove((int)FOUP_SLOTS_POS[0]);
                await WaitAxis1(FOUP_SLOTS_POS[0]);

                AddLog("Info", "Test", "Lift Up Complete");

                // 4. 실린더 후진 (웨이퍼를 가지고 나옴)
                // 이제 높이는 POS[0] 상태임
                if (!CheckRobotSafety("Backward"))
                    throw new Exception("Safety Check Failed: Cannot Retract from FOUP A");

                CylinderMove("Backward");
                await Task.Delay(2000);

                // -----------------------------------------------------------------
                // [STEP 3] 이후 동작 (PM A 이동 등...)
                // -----------------------------------------------------------------
                // (테스트 편의를 위해 여기서 안전 높이로 복귀하고 종료)
                CylinderVerticalMove((int)CHAMBER_VPOS);
                await WaitAxis1(CHAMBER_VPOS);

                AddLog("Info", "Test", "--- Pick Test Complete ---");
                MessageBox.Show("Pick 동작(Scoop) 완료");
            }
            catch (Exception ex)
            {
                AddLog("Error", "Test", $"Test Stopped: {ex.Message}");
                MessageBox.Show($"오류 발생: {ex.Message}");
                // 비상 시 후진 시도
                CylinderMove("Backward");
            }
            finally
            {
                btnRobot.Enabled = true;
            }
        }

        private bool IsRange(long current, long target, int tolerance = 100)
        {
            return (current >= target - tolerance && current <= target + tolerance);
        }

        // [보조 함수] 로봇 안전 점검 로직
        private bool CheckRobotSafety(string requst = "None")
        {
            /*
             안전을 위한 규칙 정의   

            1. 로봇 실린더가 전진 상태(확장)면 좌우 이동 금지
            2. 챔버에 로봇팔을 전진하기 전에 도어 상태 확인 후 0.5초 여유두고 로봇팔 전진
            3. 챔버에서 로봇팔 후진 후 0.5초 여유두고 도어 닫힘
            4. 로봇 팔이 챔버 A,B,C 좌우PP값 ±500 이라면 상하PP값이 각 챔버 상하PP값 ±500 이여야함
            5. FOUP A,B 각 좌우PP값 ±500 이라면 0.5초 여유 두고 로봇팔 전진
            6. 동작은 한번에 하나씩 ex) 좌우 이동 중에는 상하 이동 불가, 전진, 후진 중에는 이동 불가


            * 비상 정지 버튼 필수

             */

            long currentVPos = 0;
            long currentHPos = 0;

            // 좌표값 파싱 (안전하게 처리)
            try
            {
                currentVPos = long.Parse(EtherCAT_M.Axis1_is_PosData());
                currentHPos = long.Parse(EtherCAT_M.Axis2_is_PosData());
            }
            catch
            {
                return false; // 좌표를 못 읽으면 무조건 위험
            }

            string dest = "UNKNOWN";

            // 목적지 판별 로직 (기존 유지)
            if (IsRange(currentHPos, PM_A_HPOS)) dest = "PM A";
            else if (IsRange(currentHPos, PM_B_HPOS)) dest = "PM B";
            else if (IsRange(currentHPos, PM_C_HPOS)) dest = "PM C";
            else if (IsRange(currentHPos, FOUP_A_HPOS)) dest = "FOUP A";
            else if (IsRange(currentHPos, FOUP_B_HPOS)) dest = "FOUP B";

            if (dest == "FOUP A" || dest == "FOUP B")
            {
                // [수정] 허용 높이: 안착 위치(POS) 또는 진입 위치(IN_POS) 근처라면 허용
                // 웨이퍼를 집으러 들어갈 때는 IN_POS, 들고 나올 때는 POS 높이임
                bool isAtPickPos = IsRange(currentVPos, FOUP_SLOTS_POS[0]);    // Slot 1 기준
                bool isAtEntryPos = IsRange(currentVPos, FOUP_SLOTS_IN_POS[0]); // Slot 1 기준

                if (requst == "Forward")
                {
                    // 전진은 보통 빈 손으로 진입(IN_POS)하거나, 놓으러 갈 때(POS) - 여기선 Pick이므로 IN_POS 체크
                    // 유연성을 위해 둘 다 허용하되, 시나리오에 맞게 타이트하게 잡을 수도 있음
                    if (isAtEntryPos || isAtPickPos) return true;
                    else return false;
                }
                else if (requst == "Backward")
                {
                    // 후진은 집고 나서(POS) 또는 놓고 나서(IN_POS) 나옴
                    if (isAtPickPos || isAtEntryPos) return true;
                    else return false;
                }
            }

            if (requst == "HMove")
            {
                if (currentRobotState == ROBOT_STATE_FORWARD)
                    return false;
            }

            // A. 실린더 이동 요청에 따른 도어 상태 확인 및 이동 명령
            if (requst == "Forward")
            {
                ROBOT_BACKWARD_MOVE = true;
                ROBOT_HORIZONTAL_MOVE = false;

                if (dest == "PM A")
                {
                    if (isDoorOpenPmA)
                    {
                        //CylinderMove("Forward");
                        return true;
                    }
                    else
                    {
                        //ChamberA_SetDoor(true);
                        // 여유시간 대기
                        //CylinderMove("Forward");
                        return false;
                    }
                }
                else if (dest == "PM B")
                {
                    if (isDoorOpenPmB)
                    {
                        //CylinderMove("Forward");
                        return true;
                    }
                    else
                    {
                        //ChamberB_SetDoor(true);
                        // 여유시간 대기
                        //CylinderMove("Forward");
                        return false;
                    }
                }
                else if (dest == "PM C")
                {
                    if (isDoorOpenPmC)
                    {
                        //CylinderMove("Forward");
                        return true;
                    }
                    else
                    {
                        //ChamberC_SetDoor(true);
                        // 여유시간 대기
                        //CylinderMove("Forward");
                        return false;
                    }
                }
            }
            else if (requst == "Backward")
            {
                ROBOT_BACKWARD_MOVE = true;
                ROBOT_HORIZONTAL_MOVE = false;

                if (dest == "PM A")
                {
                    if (isDoorOpenPmA)
                    {
                        //CylinderMove("Backward");
                        // 여유시간 대기
                        //ChamberA_SetDoor(false);
                        return true;
                    }
                    else
                    {
                        //ChamberA_SetDoor(true);
                        // 여유시간 대기
                        //CylinderMove("Backward");
                        // 여유시간 대기
                        //ChamberA_SetDoor(false);
                        return false;
                    }
                }
                else if (dest == "PM B")
                {
                    if (isDoorOpenPmB)
                    {
                        //CylinderMove("Backward");
                        // 여유시간 대기
                        //ChamberB_SetDoor(false);
                        return true;
                    }
                    else
                    {
                        //ChamberB_SetDoor(true);
                        // 여유시간 대기
                        //CylinderMove("Backward");
                        // 여유시간 대기
                        //ChamberB_SetDoor(false);
                        return false;
                    }
                }
                else if (dest == "PM C")
                {
                    if (isDoorOpenPmC)
                    {
                        //CylinderMove("Backward");
                        // 여유시간 대기
                        //ChamberC_SetDoor(false);
                        return true;
                    }
                    else
                    {
                        //ChamberC_SetDoor(true);
                        // 여유시간 대기
                        //CylinderMove("Backward");
                        // 여유시간 대기
                        //ChamberC_SetDoor(false);
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnRobot_Click(object sender, EventArgs e)
        {
            Form robotForm = new Form() { Text = "Robot", Size = new Size(1263, 759), StartPosition = FormStartPosition.CenterScreen };
            RobotControl rc = new RobotControl() { Dock = DockStyle.Fill };
            robotForm.Controls.Add(rc);
            robotForm.ShowDialog();
        }
        private void ChamberA_SetDoor(bool isOpen)
        {
            if (isOpen)
            {
                EtherCAT_M.Digital_Output(5, true);    // P105 (하강)
                EtherCAT_M.Digital_Output(4, false);   // P104 (상승)
                isDoorOpenPmA = true;
            }
            else
            {
                EtherCAT_M.Digital_Output(5, false);   // P105 (하강)
                EtherCAT_M.Digital_Output(4, true);    // P104 (상승)
                isDoorOpenPmA = false;
            }
        }

        private void ChamberB_SetDoor(bool isOpen)
        {
            if (isOpen)
            {
                EtherCAT_M.Digital_Output(8, true);    // P108 (하강)
                EtherCAT_M.Digital_Output(7, false);   // P107 (상승)
                isDoorOpenPmB = true;
            }
            else
            {
                EtherCAT_M.Digital_Output(8, false);   // P108 (하강)
                EtherCAT_M.Digital_Output(7, true);    // P107 (상승)
                isDoorOpenPmB = false;
            }
        }

        private void ChamberC_SetDoor(bool isOpen)
        {
            if (isOpen)
            {
                EtherCAT_M.Digital_Output(11, true);    // P111 (하강)
                EtherCAT_M.Digital_Output(10, false);   // P110 (상승)
                isDoorOpenPmC = true;
            }
            else
            {
                EtherCAT_M.Digital_Output(11, false);   // P111 (하강)
                EtherCAT_M.Digital_Output(10, true);    // P110 (상승)
                isDoorOpenPmC = false;
            }
        }

        private void CylinderMove(string command)
        {
            if (command == "Forward")
            {
                EtherCAT_M.Digital_Output(ROBOT_FORWARD_PORT, true);
                EtherCAT_M.Digital_Output(ROBOT_BACKWARD_PORT, false);
                currentRobotState = ROBOT_STATE_FORWARD;

            }
            else if (command == "Backward")
            {
                EtherCAT_M.Digital_Output(ROBOT_FORWARD_PORT, false);
                EtherCAT_M.Digital_Output(ROBOT_BACKWARD_PORT, true);
                currentRobotState = ROBOT_STATE_BACKWARD;
            }
        }

        private void CylinderVerticalMove(int targetVpos)
        {
            if (targetVpos <= 0)
            {
                MessageBox.Show("0 이하 위치로는 이동 불가합니다");
                return;
            }

            EtherCAT_M.Axis1_UD_POS_Update(targetVpos);
            EtherCAT_M.Axis1_UD_Move_Send();
        }
    }
}