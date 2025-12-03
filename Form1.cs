using IEG3268_Dll;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;       // CancellationToken
using System.Threading.Tasks; // Task
using System.Windows.Forms;

// [중요] Timer 모호성 해결
using Timer = System.Windows.Forms.Timer;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        // =========================================================================
        // [1] 멤버 변수 및 객체 선언
        // =========================================================================

        // EtherCAT 라이브러리
        IEG3268 EtherCAT_M = new IEG3268();
        bool isEtherConnected = false;

        // 시뮬레이션 모드 플래그
        private bool isSimulation = false;

        // 작업 취소 토큰 (비상 정지용)
        private CancellationTokenSource _cts;

        // DB 연결 문자열
        private string connectionString = "Server=localhost;Port=3306;Database=SemiGuiData;Uid=root;Pwd=1234;Charset=utf8;";

        // -------------------------------------------------------------------------
        // 좌표 상수 (Pulse)
        // -------------------------------------------------------------------------
        private const long FOUP_A_HPOS = 13200;    // 135도
        private const long FOUP_B_HPOS = -394700;  // 45도
        private const long PM_A_HPOS = -59064;     // 180도
        private const long PM_B_HPOS = -190823;    // 270도
        private const long PM_C_HPOS = -322000;    // 0도

        Dictionary<string, long> Hpositions = new Dictionary<string, long>()
        {
            {"PM A", PM_A_HPOS},
            {"PM B", PM_B_HPOS},
            {"PM C", PM_C_HPOS},
            {"FOUP A", FOUP_A_HPOS},
            {"FOUP B", FOUP_B_HPOS}
        };
        private const long CHAMBER_VPOS = 1150000;      // 이동 안전 높이
        private const long CHAMBER_PLACE_VPOS = 806931; // 챔버 안착 높이

        // FOUP 슬롯별 안착(Lift) 높이
        private readonly long[] FOUP_SLOTS_POS = new long[]
        {
            290000, 982378, 1627604, 2332102, 3018457
        };
        // FOUP 슬롯별 진입(Entry) 높이
        private readonly long[] FOUP_SLOTS_IN_POS = new long[]
        {
            40000, 782378, 1450000, 2119399, 2818463
        };

        // -------------------------------------------------------------------------
        // IO 포트
        // -------------------------------------------------------------------------
        private const int ROBOT_FORWARD_PORT = 12;
        private const int ROBOT_BACKWARD_PORT = 13;
        private const int ROBOT_VACUUM_ON_PORT = 14;
        private const int ROBOT_VACUUM_OFF_PORT = 15;

        // -------------------------------------------------------------------------
        // 상태 변수
        // -------------------------------------------------------------------------
        private bool isLoggedIn = false;
        private bool isAutoRun = false;

        private Timer clockTimer;
        private Timer sysTimer;

        // 웨이퍼 수량
        private int foupACount = 5;
        private int foupBCount = 0;

        // 챔버 상태 (0:Idle, 1:Processing, 2:Complete)
        private int statusPmA = 0;
        private int statusPmB = 0;
        private int statusPmC = 0;

        // 진행률
        private double progressA = 0;
        private double progressB = 0;
        private double progressC = 0;

        // 실행 플래그
        private bool isRunningPmA = false;
        private bool isRunningPmB = false;
        private bool isRunningPmC = false;

        // 레시피 시간
        private int timePmA = 5;
        private int timePmB = 5;
        private int timePmC = 5;

        // 도어 상태
        private bool isDoorOpenPmA = false;
        private bool isDoorOpenPmB = false;
        private bool isDoorOpenPmC = false;

        // 로봇 애니메이션
        private float robotAngle = 180;
        private float targetAngle = 180;
        private bool isRobotMoving = false;
        private bool robotHasWafer = false;
        private string robotDestination = "";
        private string robotSource = "";

        private float robotExtension = 0;
        private const float MAX_EXTENSION = 60.0f;
        private const float EXTENSION_SPEED = 5.0f;

        private const int ROBOT_STATE_ROTATE = 0;
        private const int ROBOT_STATE_FORWARD = 1;
        private const int ROBOT_STATE_WAIT = 2;
        private const int ROBOT_STATE_BACKWARD = 3;

        private int currentRobotState = ROBOT_STATE_ROTATE;
        private int robotWaitCounter = 0;
        private const int ROBOT_WAIT_TICKS = 10;
        private float robotSpeed = 10.0f;

        // UI 각도
        private const float ANG_PMC = 0;
        private const float ANG_FOUP_B = 45;
        private const float ANG_FOUP_A = 135;
        private const float ANG_PMA = 180;
        private const float ANG_PMB = 270;

        // 컨트롤
        private Button btnAutoRun;
        private int currentAlarmLevel = 0;
        private bool isResetting = false;

        // =========================================================================
        // [2] 생성자 및 초기화
        // =========================================================================
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

            // 이벤트 연결
            this.btnMain.Click += BtnMain_Click;
            this.btnRecipe.Click += BtnRecipe_Click;
            this.btnLog.Click += BtnLog_Click;
            this.btnConfig.Click += BtnConfig_Click;
            this.btnLogin.Click += BtnLogin_Click;

            this.btnLoadA.Click += (s, e) => { foupACount = 5; UpdateWaferUI(); pnlCenter.Invalidate(); AddLog("Info", "FOUP A", "Manual Load"); };
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
                if (isLoggedIn) ToggleAutoRun();
                else MessageBox.Show("로그인이 필요합니다.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
            this.pnlTop.Controls.Add(btnAutoRun);
        }

        // =========================================================================
        // [3] 메인 로직 (Timer & Event)
        // =========================================================================
        private void SysTimer_Tick(object sender, EventArgs e)
        {
            CheckAlarms();

            if (!isSimulation && !isAutoRun)
            {
                if (statusPmA == 1 && !isRunningPmA) RunProcessAsync("PMA", timePmA);
                if (statusPmB == 1 && !isRunningPmB) RunProcessAsync("PMB", timePmB);
                if (statusPmC == 1 && !isRunningPmC) RunProcessAsync("PMC", timePmC);
            }

            // [수정된 로직]
            if (isRobotMoving)
            {
                AnimateRobot();
                pnlCenter.Invalidate();
            }
            else
            {
                // 로봇이 멈췄는데 리셋 중이었다면? -> 리셋 완료 처리
                if (isResetting)
                {
                    isResetting = false; // 리셋 모드 해제

                    // AutoRun 상태가 아니면 타이머 정지 (CPU 절약)
                    if (!isAutoRun) sysTimer.Stop();

                    AddLog("System", "Robot", "Robot Reset Complete");
                }
            }

            UpdateProcessUI();
        }

        // =========================================================================
        // [4] Auto Run & Scheduler (통합 제어)
        // =========================================================================
        private async void ToggleAutoRun()
        {
            if (isAutoRun)
            {
                // [수정] STOP 로직 강화
                if (_cts != null) _cts.Cancel(); // 1. 작업 취소 토큰 발동

                isAutoRun = false;

                // 2. 로봇 애니메이션 즉시 정지
                isRobotMoving = false;

                // 3. 실행 중이던 공정 플래그 즉시 해제 (RunProcessAsync가 종료되도록 유도)
                isRunningPmA = false;
                isRunningPmB = false;
                isRunningPmC = false;

                btnAutoRun.Text = "AUTO RUN";
                btnAutoRun.BackColor = Color.LightGray;
                AddLog("System", "System", "Stop Command Received...");

                // 4. UI 강제 갱신 (멈춘 모습 바로 반영)
                pnlCenter.Invalidate();
                return;
            }

            isAutoRun = true;
            btnAutoRun.Text = "STOP";
            btnAutoRun.BackColor = Color.LightCoral;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            if (!sysTimer.Enabled) sysTimer.Start();

            try
            {
                if (isEtherConnected)
                {
                    AddLog("System", "Mode", "Starting HARDWARE Auto Run");
                    isSimulation = false;
                    await RunHardwareAutoRun(token);
                }
                else
                {
                    AddLog("System", "Mode", "Starting SIMULATION Auto Run");
                    isSimulation = true;
                    await RunSimulationTesting(token);
                    isSimulation = false;
                }
            }
            catch (OperationCanceledException)
            {
                AddLog("System", "System", "Stopped Immediately by User.");
            }
            catch (Exception ex)
            {
                AddLog("Error", "AutoRun", ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // 종료 후 정리 로직
                isAutoRun = false;
                isSimulation = false;
                btnAutoRun.Text = "AUTO RUN";
                btnAutoRun.BackColor = Color.LightGray;
                btnRobot.Enabled = true;
                if (_cts != null) { _cts.Dispose(); _cts = null; }

                isRunningPmA = false; isRunningPmB = false; isRunningPmC = false;
                isRobotMoving = false; // 확실하게 한번 더 정지
            }
        }

        // [시뮬레이션 스케줄러]
        private async Task RunSimulationTesting(CancellationToken token)
        {
            btnRobot.Enabled = false;
            AddLog("Simul", "Scheduler", "--- Simulation Start ---");

            while (!token.IsCancellationRequested && (foupACount > 0 || statusPmA != 0 || statusPmB != 0 || statusPmC != 0))
            {
                bool actionTaken = false;

                // 1. 배출: PM C -> FOUP B
                if (statusPmC == 2 && !robotHasWafer)
                {
                    AddLog("Simul", "Job", "Unloading: PM C -> FOUP B");
                    await Transfer_Chamber_to_FoupB("PMC", ANG_PMC, token, isHardware: false);
                    actionTaken = true;
                }
                // 2. 이송: PM B -> PM C
                else if (statusPmB == 2 && statusPmC == 0 && !robotHasWafer)
                {
                    AddLog("Simul", "Job", "Transfer: PM B -> PM C");
                    await Transfer_Chamber_to_Chamber("PMB", ANG_PMB, "PMC", ANG_PMC, token, isHardware: false);
                    _ = RunProcessBackground("PMC", token);
                    actionTaken = true;
                }
                // 3. 이송: PM A -> PM B
                else if (statusPmA == 2 && statusPmB == 0 && !robotHasWafer)
                {
                    AddLog("Simul", "Job", "Transfer: PM A -> PM B");
                    await Transfer_Chamber_to_Chamber("PMA", ANG_PMA, "PMB", ANG_PMB, token, isHardware: false);
                    _ = RunProcessBackground("PMB", token);
                    actionTaken = true;
                }
                // 4. 투입: FOUP A -> PM A
                else if (foupACount > 0 && statusPmA == 0 && !robotHasWafer)
                {
                    AddLog("Simul", "Job", "Loading: FOUP A -> PM A");
                    await Transfer_FoupA_to_Chamber("PMA", ANG_PMA, token, isHardware: false);
                    _ = RunProcessBackground("PMA", token);
                    actionTaken = true;
                }

                if (!actionTaken) await Task.Delay(100, token);
            }
            if (!token.IsCancellationRequested) MessageBox.Show("시뮬레이션 완료");
        }

        // [하드웨어 스케줄러]
        private async Task RunHardwareAutoRun(CancellationToken token)
        {
            btnRobot.Enabled = false;
            AddLog("Hardware", "Scheduler", "--- Hardware Run Start ---");

            while (!token.IsCancellationRequested && (foupACount > 0 || statusPmA != 0 || statusPmB != 0 || statusPmC != 0))
            {
                bool actionTaken = false;

                if (foupACount > 0 && statusPmA == 0 && !robotHasWafer)
                {
                    AddLog("Hard", "Job", "Loading: FOUP A -> PM A");
                    await Transfer_FoupA_to_Chamber("PMA", ANG_PMA, token, isHardware: true);
                    _ = RunProcessBackground("PMA", token);
                    actionTaken = true;
                }

                else if (statusPmC == 2 && !robotHasWafer)
                {
                    AddLog("Hard", "Job", "Unloading: PM C -> FOUP B");
                    await Transfer_Chamber_to_FoupB("PMC", ANG_PMC, token, isHardware: true);
                    actionTaken = true;
                }
                else if (statusPmB == 2 && statusPmC == 0 && !robotHasWafer)
                {
                    AddLog("Hard", "Job", "Transfer: PM B -> PM C");
                    await Transfer_Chamber_to_Chamber("PMB", ANG_PMB, "PMC", ANG_PMC, token, isHardware: true);
                    _ = RunProcessBackground("PMC", token);
                    actionTaken = true;
                }
                else if (statusPmA == 2 && statusPmB == 0 && !robotHasWafer)
                {
                    AddLog("Hard", "Job", "Transfer: PM A -> PM B");
                    await Transfer_Chamber_to_Chamber("PMA", ANG_PMA, "PMB", ANG_PMB, token, isHardware: true);
                    _ = RunProcessBackground("PMB", token);
                    actionTaken = true;
                }

                if (!actionTaken) await Task.Delay(100, token);
            }
            if (!token.IsCancellationRequested) MessageBox.Show("하드웨어 공정 완료");
        }

        // =========================================================================
        // [5] 이송 패턴 로직 (FOUP A -> PM, PM -> PM, PM -> FOUP B)
        // =========================================================================

        // 1. FOUP A -> PM
        private async Task Transfer_FoupA_to_Chamber(string targetPm, float targetAng, CancellationToken token, bool isHardware)
        {
            int slotIdx = -(foupACount - 5);

            if (CheckNeedHomeReturn()) await Common_ServoMove(ANG_PMA, token, isHardware);

            //await Common_ZMove(CHAMBER_VPOS, token, isHardware);
            await Common_ServoMove(ANG_FOUP_A, token, isHardware);

            // Scoop Pick
            await Common_ZMove(FOUP_SLOTS_IN_POS[slotIdx], token, isHardware);
            await Common_Cylinder("Forward", token, isHardware);
            SetVacuum(true, isHardware);
            await Common_ZMove(FOUP_SLOTS_POS[slotIdx], token, isHardware);
            await Common_Cylinder("Backward", token, isHardware);
            foupACount--; UpdateWaferUI();

            await Common_ZMove(CHAMBER_VPOS, token, isHardware);
            await Common_ServoMove(targetAng, token, isHardware);

            SetDoorState(targetPm, true); await Task.Delay(500, token);
            await Common_Cylinder("Forward", token, isHardware);
            await Common_ZMove(CHAMBER_PLACE_VPOS, token, isHardware); 
            SetVacuum(false, isHardware);
            await Common_Cylinder("Backward", token, isHardware);
            await Common_ZMove(CHAMBER_VPOS, token, isHardware);
            SetDoorState(targetPm, false);
        }

        // 2. PM -> PM
        private async Task Transfer_Chamber_to_Chamber(string srcPm, float srcAng, string destPm, float destAng, CancellationToken token, bool isHardware)
        {
            await Common_ZMove(CHAMBER_PLACE_VPOS, token, isHardware);
            await Common_ServoMove(srcAng, token, isHardware);
            SetDoorState(srcPm, true); await Task.Delay(500, token);

            await Common_Cylinder("Forward", token, isHardware);
            SetVacuum(true, isHardware);
            await Common_ZMove(CHAMBER_VPOS, token, isHardware);

            if (srcPm == "PMA") statusPmA = 0;
            else if (srcPm == "PMB") statusPmB = 0;
            UpdateProcessUI();

            await Common_Cylinder("Backward", token, isHardware);
            await Common_ZMove(CHAMBER_VPOS, token, isHardware);
            SetDoorState(srcPm, false);

            await Common_ServoMove(destAng, token, isHardware);
            SetDoorState(destPm, true); await Task.Delay(500, token);

            await Common_Cylinder("Forward", token, isHardware);
            await Common_ZMove(CHAMBER_PLACE_VPOS, token, isHardware);
            SetVacuum(false, isHardware);
            await Task.Delay(500);
            await Common_Cylinder("Backward", token, isHardware);
            await Common_ZMove(CHAMBER_VPOS, token, isHardware);
            SetDoorState(destPm, false);
        }

        // 3. PM -> FOUP B
        private async Task Transfer_Chamber_to_FoupB(string srcPm, float srcAng, CancellationToken token, bool isHardware)
        {
            int slotIdx = -(foupBCount - 5);

            await Common_ZMove(CHAMBER_PLACE_VPOS, token, isHardware);
            await Common_ServoMove(srcAng, token, isHardware);
            SetDoorState(srcPm, true); await Task.Delay(500, token);

            await Common_Cylinder("Forward", token, isHardware);
            SetVacuum(true, isHardware);
            await Common_ZMove(CHAMBER_VPOS, token, isHardware);

            if (srcPm == "PMC") statusPmC = 0;
            UpdateProcessUI();

            await Common_Cylinder("Backward", token, isHardware);
            await Common_ZMove(CHAMBER_VPOS, token, isHardware);
            SetDoorState(srcPm, false);

            await Common_ServoMove(ANG_FOUP_B, token, isHardware);

            // Drop (Reverse Scoop)
            await Common_ZMove(FOUP_SLOTS_POS[slotIdx], token, isHardware);
            await Common_Cylinder("Forward", token, isHardware);
            SetVacuum(false, isHardware);
            foupBCount++; UpdateWaferUI();

            await Common_ZMove(FOUP_SLOTS_IN_POS[slotIdx], token, isHardware);
            await Common_Cylinder("Backward", token, isHardware);
            await Common_ZMove(CHAMBER_VPOS, token, isHardware);

            await Common_ServoMove(ANG_PMA, token, isHardware); // Home Return
        }

        // =========================================================================
        // [6] 공통 제어 Wrapper (Hardware vs Simulation)
        // =========================================================================
        private async Task Common_ZMove(long pos, CancellationToken token, bool isHardware)
        {
            if (isHardware && isEtherConnected)
            {
                EtherCAT_M.Axis1_UD_POS_Update(pos);
                EtherCAT_M.Axis1_UD_Move_Send();
                await WaitAxis1(pos, token);
            }
            else
            {
                await Task.Delay(600, token);
            }
        }

        private async Task Common_ServoMove(float angle, CancellationToken token, bool isHardware)
        {
            targetAngle = angle;
            isRobotMoving = true;

            string desk = "None";

            switch (angle)
            {
                case ANG_FOUP_A:
                    desk = "FOUP A";
                    break;
                case ANG_FOUP_B:
                    desk = "FOUP B";
                    break;
                case ANG_PMA:
                    desk = "PM A";
                    break;
                case ANG_PMB:
                    desk = "PM B";
                    break;
                case ANG_PMC:
                    desk = "PM C";
                    break;

            }

            if (desk == "None" || !Hpositions.ContainsKey(desk))
            {
                AddLog("Error", "Robot", $"Invalid Angle or Desk: {angle}");
                isRobotMoving = false;
                return;
            }

            if (isHardware)
            {
                EtherCAT_M.Axis2_LR_POS_Update(Hpositions[desk]);
                EtherCAT_M.Axis2_LR_Move_Send();
                var hardwareTask = WaitAxis2(Hpositions[desk], token);
                var uiTask = WaitForAnimation(angle, token);

                await Task.WhenAll(hardwareTask, uiTask);
            }
            else
            {
                await WaitForAnimation(angle, token); // 하드웨어여도 UI 싱크를 위해 대기
            }

            robotAngle = targetAngle;
            isRobotMoving = false;
        }

        private async Task Common_Cylinder(string cmd, CancellationToken token, bool isHardware)
        {
            if (!CheckRobotSafety(cmd)) throw new Exception("Safety Error: " + cmd);

            isRobotMoving = true;
            if (isHardware)
            {
                CylinderMove(cmd);
                await Task.Delay(2000, token);
            }
            else
            {
                if (cmd == "Forward")
                {
                    currentRobotState = ROBOT_STATE_FORWARD;
                    while (robotExtension < MAX_EXTENSION - 1.0f) await Task.Delay(50, token);
                    robotExtension = MAX_EXTENSION;
                }
                else
                {
                    currentRobotState = ROBOT_STATE_BACKWARD;
                    while (robotExtension > 1.0f) await Task.Delay(50, token);
                    robotExtension = 0;
                }
            }
            isRobotMoving = false;
        }

        private async Task RunProcessBackground(string pmName, CancellationToken token)
        {
            int targetSec = 5;
            if (pmName == "PMA") targetSec = timePmA;
            else if (pmName == "PMB") targetSec = timePmB;
            else if (pmName == "PMC") targetSec = timePmC;
            if (targetSec < 1) targetSec = 1;

            if (pmName == "PMA") statusPmA = 1;
            else if (pmName == "PMB") statusPmB = 1;
            else if (pmName == "PMC") statusPmC = 1;
            UpdateProcessUI();

            int stepDelay = (targetSec * 1000) / 20;
            for (int i = 0; i <= 100; i += 5)
            {
                if (pmName == "PMA") progressA = i;
                else if (pmName == "PMB") progressB = i;
                else if (pmName == "PMC") progressC = i;
                UpdateProcessUI();
                await Task.Delay(stepDelay, token);
            }

            if (pmName == "PMA") statusPmA = 2;
            else if (pmName == "PMB") statusPmB = 2;
            else if (pmName == "PMC") statusPmC = 2;
            UpdateProcessUI();
            pnlCenter.Invalidate();
        }

        // =========================================================================
        // [7] Helper & Utility Functions
        // =========================================================================
        private async Task WaitForAnimation(float targetAng, CancellationToken token)
        {
            currentRobotState = ROBOT_STATE_ROTATE;
            while (GetAngleDiff(robotAngle, targetAng) > 1.0f)
            {
                await Task.Delay(50, token);
            }
        }

        private float GetAngleDiff(float a1, float a2)
        {
            float diff = a1 - a2;
            while (diff <= -180) diff += 360;
            while (diff > 180) diff -= 360;
            return Math.Abs(diff);
        }

        private async Task WaitAxis1(long targetPos, CancellationToken token)
        {
            int timeout = 0;
            while (timeout < 100)
            {
                if (token.IsCancellationRequested) return;

                try
                {
                    string sPos = EtherCAT_M.Axis1_is_PosData();
                    if (long.TryParse(sPos, out long curPos))
                    {
                        // [팁] 실제 장비는 목표값에 정확히 1단위까지 맞추기 어려울 수 있음
                        // 오차 범위(Tolerance)를 100 -> 500~1000 정도로 넉넉히 잡는 것이 좋습니다.
                        if (Math.Abs(curPos - targetPos) < 500) return;
                    }
                }
                catch { }
                await Task.Delay(100, token);
                timeout++;
            }
            AddLog("Error", "Axis1", $"Z-Axis Timeout. Target: {targetPos}");
            //throw new Exception("Axis1 Timeout");
        }

        private async Task WaitAxis2(long targetPos, CancellationToken token)
        {
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
                await Task.Delay(100, token);
                timeout++;
            }
            throw new Exception("Axis2 Timeout");
        }

        private void SetVacuum(bool on, bool isHardware)
        {
            robotHasWafer = on;
            if (isHardware)
            {
                if (on) EtherCAT_M.Digital_Output(ROBOT_VACUUM_ON_PORT, true);
                else EtherCAT_M.Digital_Output(ROBOT_VACUUM_ON_PORT, false);
            }
            pnlCenter.Invalidate();
        }

        private void SetDoorState(string pmName, bool isOpen)
        {
            if (pmName == "PMA") isDoorOpenPmA = isOpen;
            else if (pmName == "PMB") isDoorOpenPmB = isOpen;
            else if (pmName == "PMC") isDoorOpenPmC = isOpen;

            if (isEtherConnected)
            {
                if (pmName == "PMA") ChamberA_SetDoor(isOpen);
                else if (pmName == "PMB") ChamberB_SetDoor(isOpen);
                else if (pmName == "PMC") ChamberC_SetDoor(isOpen);
            }
            pnlCenter.Invalidate();
        }

        private void ResetSimulationState()
        {
            robotSource = ""; robotDestination = "";
            isRobotMoving = false;
        }

        private bool CheckNeedHomeReturn()
        {
            return (Math.Abs(robotAngle - 90) < 45);
        }

        // =========================================================================
        // [8] Animation & Safety Logic (Dead Zone Avoidance)
        // =========================================================================
        private void AnimateRobot()
        {
            float speed = this.robotSpeed;

            switch (currentRobotState)
            {
                case ROBOT_STATE_ROTATE:
                    float diff = targetAngle - robotAngle;
                    while (diff <= -180) diff += 360;
                    while (diff > 180) diff -= 360;

                    // [Dead Zone 회피 - Zone 기반]
                    bool isTargetLeft = (targetAngle > 90 && targetAngle < 270);
                    bool isCurrentLeft = (robotAngle > 90 && robotAngle < 270);

                    if (isTargetLeft != isCurrentLeft)
                    {
                        if (isTargetLeft) { if (diff > 0) diff -= 360; }
                        else { if (diff < 0) diff += 360; }
                    }

                    if (Math.Abs(diff) > speed)
                    {
                        if (diff > 0) robotAngle += speed; else robotAngle -= speed;
                        if (robotAngle >= 360) robotAngle -= 360;
                        if (robotAngle < 0) robotAngle += 360;
                    }
                    else
                    {
                        robotAngle = targetAngle;
                        if (isSimulation || isAutoRun) return;

                        if (isResetting && robotAngle == 180) { isRobotMoving = false; return; }

                        currentRobotState = ROBOT_STATE_FORWARD;
                        if (isEtherConnected)
                        {
                            EtherCAT_M.Digital_Output(ROBOT_BACKWARD_PORT, false);
                            EtherCAT_M.Digital_Output(ROBOT_FORWARD_PORT, true);
                        }
                    }
                    break;

                case ROBOT_STATE_FORWARD:
                    robotExtension += EXTENSION_SPEED;
                    if (robotExtension >= MAX_EXTENSION)
                    {
                        robotExtension = MAX_EXTENSION;
                        if (isSimulation || isAutoRun) return;

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
                        if (isSimulation || isAutoRun) return;
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
                        if (isSimulation || isAutoRun) return;

                        currentRobotState = ROBOT_STATE_ROTATE;
                        if (isResetting) { targetAngle = 180; return; }

                        if (robotHasWafer) targetAngle = GetAngle(robotDestination);
                        else { isRobotMoving = false; }
                    }
                    break;
            }
        }

        // =========================================================================
        // [9] UI 이벤트 핸들러 및 기타 함수들
        // =========================================================================
        private bool CheckRobotSafety(string requst = "None")
        {
            long currentVPos = 0;
            long currentHPos = 0;
            try
            {
                if (isEtherConnected)
                {
                    currentVPos = long.Parse(EtherCAT_M.Axis1_is_PosData());
                    currentHPos = long.Parse(EtherCAT_M.Axis2_is_PosData());
                    label1.Text = $"HPos: {currentHPos}, VPos: {currentVPos}";
                }
                else return true;
            }
            catch { return false; }

            string dest = "UNKNOWN";
            if (IsRange(currentHPos, PM_A_HPOS)) dest = "PM A";
            else if (IsRange(currentHPos, PM_B_HPOS)) dest = "PM B";
            else if (IsRange(currentHPos, PM_C_HPOS)) dest = "PM C";
            else if (IsRange(currentHPos, FOUP_A_HPOS)) dest = "FOUP A";
            else if (IsRange(currentHPos, FOUP_B_HPOS)) dest = "FOUP B";

            if (dest == "FOUP A" || dest == "FOUP B")
            {
                bool isAtPickPos = IsRange(currentVPos, FOUP_SLOTS_POS[-( dest == "FOUP A" ? foupACount - 5 : foupBCount - 5)]);
                bool isAtEntryPos = IsRange(currentVPos, FOUP_SLOTS_IN_POS[-(dest == "FOUP A" ? foupACount - 5 : foupBCount - 5)]);
                if (requst == "Forward" || requst == "Backward") return (isAtEntryPos || isAtPickPos);
            }
            return true;
        }

        private bool IsRange(long current, long target, int tolerance = 1000)
        {
            return (current >= target - tolerance && current <= target + tolerance);
        }

        // Manual/Simple Run (AutoRun 아닌 경우)
        // [수정] 수동 모드용 공정 함수 (안전장치 추가)
        private async void RunProcessAsync(string pmName, int durationSec)
        {
            // 중복 실행 방지 플래그 설정
            if (pmName == "PMA") isRunningPmA = true;
            else if (pmName == "PMB") isRunningPmB = true;
            else if (pmName == "PMC") isRunningPmC = true;

            DateTime startTime = DateTime.Now;
            double targetDuration = durationSec;

            while (true)
            {
                // STOP 버튼(isAutoRun = false) 감지 시 즉시 루프 탈출
                if (!isAutoRun)
                {
                    // 실행 플래그 초기화 후 함수 종료
                    if (pmName == "PMA") isRunningPmA = false;
                    else if (pmName == "PMB") isRunningPmB = false;
                    else if (pmName == "PMC") isRunningPmC = false;
                    return;
                }
                

                // STOP을 눌러서 상태가 초기화되었을 때 이 로직이 루프를 끊어줍니다.
                bool isTargetRunning = false;
                if (pmName == "PMA" && statusPmA == 1) isTargetRunning = true;
                if (pmName == "PMB" && statusPmB == 1) isTargetRunning = true;
                if (pmName == "PMC" && statusPmC == 1) isTargetRunning = true;

                // 공정 중이어야 하는데 상태가 1이 아니라면(0이나 2로 강제 변경됨) -> 종료
                if (!isTargetRunning)
                {
                    // 플래그 해제 후 리턴
                    if (pmName == "PMA") isRunningPmA = false;
                    if (pmName == "PMB") isRunningPmB = false;
                    if (pmName == "PMC") isRunningPmC = false;
                    return;
                }

                double elapsed = (DateTime.Now - startTime).TotalSeconds;
                double progress = (elapsed / targetDuration) * 100.0;

                if (pmName == "PMA") progressA = progress;
                else if (pmName == "PMB") progressB = progress;
                else if (pmName == "PMC") progressC = progress;

                UpdateProcessUI();

                if (progress >= 100.0)
                {
                    // 100% 도달 시 마무리
                    if (pmName == "PMA") progressA = 100.0;
                    else if (pmName == "PMB") progressB = 100.0;
                    else if (pmName == "PMC") progressC = 100.0;
                    UpdateProcessUI();
                    break;
                }
                await Task.Delay(50);
            }

            // 정상 완료 처리
            if (pmName == "PMA") { statusPmA = 2; isRunningPmA = false; }
            else if (pmName == "PMB") { statusPmB = 2; isRunningPmB = false; }
            else if (pmName == "PMC") { statusPmC = 2; isRunningPmC = false; }

            UpdateProcessUI();
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
            if (targetVpos <= 0) { MessageBox.Show("0 이하 위치 이동 불가"); return; }
            EtherCAT_M.Axis1_UD_POS_Update(targetVpos);
            EtherCAT_M.Axis1_UD_Move_Send();
        }

        private void btnRobot_Click(object sender, EventArgs e)
        {
            if (isEtherConnected)
            {
                Form robotForm = new Form() { Text = "Robot", Size = new Size(1263, 759), StartPosition = FormStartPosition.CenterScreen };
                RobotControl rc = new RobotControl() { Dock = DockStyle.Fill };
                robotForm.Controls.Add(rc);
                robotForm.ShowDialog();
            }
            
        }

        // Hardware Connect/Disconnect
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isEtherConnected)
            {
                // [수정] try-catch 문으로 감싸서 DLL이 없을 때 프로그램 종료 방지
                try
                {
                    // 하드웨어 드라이버(DLL)가 없으면 여기서 에러가 발생하여 catch로 넘어갑니다.
                    if (EtherCAT_M.CIFX_50RE_Connect() == true)
                    {
                        isEtherConnected = true;
                        btnConnect.Text = "DISCONNECT";
                        btnConnect.BackColor = Color.LimeGreen;

                        EtherCAT_M.ReadData_Send_Start(300);
                        EtherCAT_M.ReadData_Timer_Start();
                        EtherCAT_M.Axis1_ON();
                        EtherCAT_M.Axis2_ON();
                        EtherCAT_M.Axis1_UD_Config_Update(1000000, 1000000, 1000000, 1000000);
                        EtherCAT_M.Axis2_LR_Config_Update(1000000, 1000000, 1000000, 1000000);

                        AddLog("Event", "System", "EtherCAT Connected Success");
                    }
                    else
                    {
                        MessageBox.Show("EtherCAT 연결 실패 (장비를 찾을 수 없음)");
                        AddLog("Error", "System", "EtherCAT Connection Failed");
                    }
                }
                catch (DllNotFoundException)
                {
                    // ★ 드라이버 파일이 없는 경우 여기로 들어옵니다.
                    MessageBox.Show("드라이버(cifx32dll.dll)가 없어 시뮬레이션 모드로 동작합니다.");
                    AddLog("Info", "System", "Driver not found. Running in Simulation Mode.");
                    isEtherConnected = false;
                }
                catch (Exception ex)
                {
                    // 그 외 다른 에러 처리
                    MessageBox.Show($"연결 중 오류 발생: {ex.Message}");
                    isEtherConnected = false;
                }
            }
            else
            {
                // 연결 해제 로직 (기존과 동일하되 안전장치 추가)
                try
                {
                    EtherCAT_M.Axis1_OFF();
                    EtherCAT_M.Axis2_OFF();
                    EtherCAT_M.CIFX_50RE_Disconnect();
                }
                catch { /* 무시 */ }

                isEtherConnected = false;
                btnConnect.Text = "CONNECT";
                btnConnect.BackColor = Color.Khaki;
                AddLog("Event", "System", "EtherCAT Disconnected");
            }
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
                                if (int.TryParse(val, out interval)) if (sysTimer != null) sysTimer.Interval = interval;
                            }
                        }
                    }
                }
            }
            catch { }
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
            catch { }
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
            catch { }
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            Form cPop = new Form() { Text = "Configuration", Size = new Size(820, 640), StartPosition = FormStartPosition.CenterScreen };
            ConfigControl cc = new ConfigControl() { Dock = DockStyle.Fill };
            cc.ConfigSaved += (s2, e2) => { LoadSystemConfig(); AddLog("Event", "System", "Configuration Updated"); };
            cc.btnClose.Click += (s2, e2) => cPop.Close();
            cPop.Controls.Add(cc);
            cPop.ShowDialog();
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            Form lPop = new Form() { Text = "Log", Size = new Size(1280, 760), StartPosition = FormStartPosition.CenterScreen };
            LogControl lc = new LogControl() { Dock = DockStyle.Fill };
            lPop.Controls.Add(lc);
            lPop.Show();
        }

        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            Form rPop = new Form() { Text = "Recipe", Size = new Size(1290, 760), StartPosition = FormStartPosition.CenterScreen };
            RecipeControl rc = new RecipeControl() { Dock = DockStyle.Fill };
            rc.ApplyToMainRequested += (s2, data) => ApplyRecipeData(data);
            rc.btnCancel.Click += (s2, e2) => rPop.Close();
            rPop.Controls.Add(rc);
            rPop.ShowDialog();
        }

        private void ApplyRecipeData(RecipeControl.RecipeModel data)
        {
            if (data.PmA_Params != null) { valTargetA.Text = data.PmA_Params[0]; valGasA.Text = data.PmA_Params[1]; valTimeA.Text = data.PmA_Params[2]; valCurrA.Text = data.PmA_Params[0]; int.TryParse(data.PmA_Params[2], out timePmA); }
            if (data.PmB_Params != null) { valAlignB.Text = data.PmB_Params[0]; valRpmB.Text = data.PmB_Params[1]; valTimeB.Text = data.PmB_Params[2]; int.TryParse(data.PmB_Params[2], out timePmB); }
            if (data.PmC_Params != null) { valPressC.Text = data.PmC_Params[0]; valGasC.Text = data.PmC_Params[1]; valSpinTimeC.Text = data.PmC_Params[2]; int.TryParse(data.PmC_Params[2], out timePmC); }
            AddLog("Event", "Recipe", $"Recipe Applied: {data.Name}");
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (isLoggedIn)
            {
                if (MessageBox.Show("로그아웃 하시겠습니까?", "Logout", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    AddLog("Event", "System", "User Logged Out");
                    SetLoginState(false);
                }
                return;
            }
            if (txtId.Text.Trim() == "admin" && txtPw.Text.Trim() == "1234")
            {
                MessageBox.Show("로그인 되었습니다.");
                SetLoginState(true);
            }
            else MessageBox.Show("ID/PW 불일치");
        }

        private void SetLoginState(bool login)
        {
            isLoggedIn = login;
            pnlLeft.Visible = login; pnlRight.Visible = login; pnlCenter.Visible = login; pnlBottom.Visible = login;
            txtId.Enabled = !login; txtPw.Enabled = !login;
            btnLogin.Text = login ? "LOGOUT" : "LOGIN";
            if (!login) { txtId.Text = ""; txtPw.Text = ""; txtId.Focus(); }
            else AddLog("Event", "System", "User Logged In");
            if (login) UpdateLayout();
        }

        private void BtnMain_Click(object sender, EventArgs e) { pnlCenter.Invalidate(); UpdateLayout(); }
        // [수정] async 키워드 추가 (하드웨어 대기 시간을 위해)
        private async void btnResetRobot_Click(object sender, EventArgs e)
        {
            // 1. AutoRun 중이면 리셋 금지 (안전장치)
            if (isAutoRun)
            {
                MessageBox.Show("Auto Run을 먼저 정지해주세요.");
                return;
            }

            // 2. 로그 기록
            AddLog("System", "Robot", "Reset Process Started (Hardware & UI)");

            // =========================================================
            // [A] 하드웨어 리셋 로직 (EtherCAT 연결 시에만 동작)
            // =========================================================
            if (isEtherConnected)
            {
                try
                {
                    // 1. 실린더(Arm) 강제 후진 (가장 중요: 충돌 방지)
                    EtherCAT_M.Digital_Output(ROBOT_FORWARD_PORT, false);
                    EtherCAT_M.Digital_Output(ROBOT_BACKWARD_PORT, true);

                    // 2. 물리적으로 실린더가 들어올 때까지 잠시 대기 (1~2초)
                    // (센서가 있다면 센서 확인 로직을 넣는 것이 가장 좋음)
                    await Task.Delay(1500);

                    // 3. 축 원점 복귀 (Homing) 명령 전송
                    // Z축과 회전축을 동시에 혹은 순차적으로 보냅니다.
                    EtherCAT_M.Axis1_UD_Homming(); // Z축(상하) 원점 잡기
                    await Task.Delay(100);         // 통신 꼬임 방지용 미세 딜레이
                    EtherCAT_M.Axis2_LR_Homming(); // 회전축(좌우) 원점 잡기
                    await Task.Delay(100);
                    EtherCAT_M.Digital_Output(14, false); // 흡기 끄기
                    await Task.Delay(100);
                    EtherCAT_M.Digital_Output(15, false); // 배기 끄기

                    MessageBox.Show("로봇 상하 좌우 원점 복귀 완료됨");
                }
                catch (Exception ex)
                {
                    AddLog("Error", "RobotReset", "Hardware Homing Failed: " + ex.Message);
                    MessageBox.Show("하드웨어 원점 복귀 실패: " + ex.Message);
                    return; // 하드웨어 에러 시 UI 리셋도 중단할지 결정 필요
                }
            }

            // =========================================================
            // [B] UI 애니메이션 리셋 로직
            // =========================================================
            isResetting = true;
            isRobotMoving = true; // 타이머가 애니메이션을 수행하도록 설정

            // 로봇 팔이 뻗어있다면? -> 화면상에서도 후진 모드로 시작
            if (robotExtension > 0)
            {
                currentRobotState = ROBOT_STATE_BACKWARD;
            }
            // 팔이 접혀있다면? -> 바로 회전 시작 (홈 위치 180도)
            else
            {
                currentRobotState = ROBOT_STATE_ROTATE;
                targetAngle = 180;
            }

            // 타이머가 꺼져있다면 실행
            if (!sysTimer.Enabled) sysTimer.Start();
        }
        private void BtnResetChambers_Click(object sender, EventArgs e)
        { 
            statusPmA = 0;
            statusPmB = 0;
            statusPmC = 0;
            progressA = 0; 
            progressB = 0; 
            progressC = 0;

            robotHasWafer = false;

            UpdateWaferUI();
            UpdateProcessUI();

            pnlCenter.Invalidate();
        }
        private void PerformRobotAction() { /* 수동 모드 로직 */ }
        private float GetAngle(string n) { return 0; /* 수동 모드 로직 */ }

        private void CheckAlarms()
        {
            int prevLevel = currentAlarmLevel;
            currentAlarmLevel = 0;
            string msg = "System Normal";
            if (foupBCount >= 5 && statusPmC == 2) { currentAlarmLevel = 2; msg = "DANGER: FOUP B Full!"; }
            else if (foupACount == 0) { currentAlarmLevel = 1; msg = "WARNING: FOUP A Empty"; }

            if (prevLevel != currentAlarmLevel)
            {
                if (currentAlarmLevel > 0) lblAlarmMsg.Text = msg; else lblAlarmMsg.Text = "";
                pnlAlarm.Invalidate();
            }
        }

        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            if (!isLoggedIn) return;
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int cx = pnlCenter.Width / 2; int cy = pnlCenter.Height / 2;

            g.ResetTransform();
            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);

            DrawRotatedFoup(g, cx, cy, ANG_FOUP_A, "FOUP A", foupACount);
            DrawRotatedFoup(g, cx, cy, ANG_FOUP_B, "FOUP B", foupBCount);

            g.ResetTransform();
            g.TranslateTransform(cx, cy);
            g.RotateTransform(robotAngle);
            g.FillRectangle(Brushes.DimGray, -20, -20, 100, 40);
            float armX = 0 + robotExtension;
            g.FillRectangle(Brushes.Gray, armX, -15, 100, 30);
            if (robotHasWafer) g.FillEllipse(Brushes.CornflowerBlue, armX + 70, -20, 40, 40);

            g.ResetTransform();
            int lightX = pnlCenter.Width - 60; int lightY = 50;
            g.FillRectangle(isAutoRun ? Brushes.Maroon : Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Olive, lightX, lightY + 30, 30, 30);
            g.FillRectangle(isAutoRun ? Brushes.Lime : Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);
        }

        private void DrawRotatedFoup(Graphics g, int cx, int cy, float angle, string label, int waferCount)
        {
            var state = g.Save();
            g.TranslateTransform(cx, cy); g.RotateTransform(angle);
            g.TranslateTransform(160, 0); g.RotateTransform(180);
            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), -40, -40, 80, 80);
            for (int i = 0; i < 5; i++)
            {
                Color c = (i < waferCount) ? Color.Blue : Color.Black;
                g.FillRectangle(new SolidBrush(c), -35 + (i * 14), -35, 10, 70);
            }
            using (Pen p = new Pen(Color.DimGray, 4)) { g.DrawLine(p, -40, -40, 40, -40); g.DrawLine(p, -40, 40, 40, 40); g.DrawLine(p, -40, -42, -40, 42); }
            using (Font f = new Font("Arial", 10, FontStyle.Bold))
            {
                g.TranslateTransform(-50, 0); g.RotateTransform(90);
                g.DrawString(label, f, Brushes.Black, 0, 0);
            }
            g.Restore(state);
        }

        private void pnlAlarm_Paint(object sender, PaintEventArgs e)
        {
            if (currentAlarmLevel == 0) return;
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            Point[] p = { new Point(30, 5), new Point(5, 55), new Point(55, 55) };
            g.FillPolygon(currentAlarmLevel == 2 ? Brushes.Red : Brushes.Gold, p);
            using (Font f = new Font("Arial", 24, FontStyle.Bold)) g.DrawString("!", f, Brushes.White, 12, 10);
        }

        private void UpdateWaferUI() { txtCarrierA.Text = $"FOUP_LOT01 ({foupACount})"; txtCarrierB.Text = $"FOUP_LOT02 ({foupBCount})"; }

        private void UpdateProcessUI()
        {
            UpdateProgressBar(progA, (int)Math.Min(progressA, 100)); pnlChamberA.BackColor = GetStateColor(statusPmA);
            UpdateProgressBar(progB, (int)Math.Min(progressB, 100)); pnlChamberB.BackColor = GetStateColor(statusPmB);
            UpdateProgressBar(progC, (int)Math.Min(progressC, 100)); pnlChamberC.BackColor = GetStateColor(statusPmC);
        }

        private void UpdateProgressBar(ProgressBar pb, int value)
        {
            // 1. 값이 변하지 않았으면 무시
            if (value == pb.Value) return;

            // 2. 값의 범위를 0 ~ Maximum(100) 사이로 강제 고정
            if (value < 0) value = 0;
            if (value > pb.Maximum) value = pb.Maximum;

            // 3. 애니메이션 딜레이 제거 트릭 (값 + 1 했다가 원복)
            // [중요] 단, 값이 Maximum보다 작을 때만 +1을 해야 함 (100일 때 101이 되면 에러 발생)
            if (value < pb.Maximum)
            {
                pb.Value = value + 1; // 예: 99 -> 100 (OK)
                pb.Value = value;     //     100 -> 99 (OK)
            }
            else
            {
                // 값이 100(Maximum)인 경우, +1을 하지 않고 그냥 설정
                pb.Value = value;
            }
        }
        private Color GetStateColor(int state) { if (state == 1) return Color.LimeGreen; if (state == 2) return Color.Yellow; return Color.FromArgb(220, 220, 220); }

        private void UpdateLayout()
        {
            if (pnlCenter.Width == 0) return;
            int cx = pnlCenter.Width / 2; int cy = pnlCenter.Height / 2;
            pnlChamberB.Location = new Point(cx - 40, cy - 220);
            pnlChamberA.Location = new Point(cx - 210, cy - 50);
            pnlChamberC.Location = new Point(cx + 120, cy - 50);
            pnlFoupA.Location = new Point(cx - 150, cy + 70);
            pnlFoupB.Location = new Point(cx + 70, cy + 70);
            pnlCassetteL.Location = new Point(cx - 100, cy + 160);
            pnlCassetteR.Location = new Point(cx + 20, cy + 160);
            if (lblNameA != null) lblNameA.Location = new Point(pnlChamberA.Left, pnlChamberA.Top - 20);
            if (lblNameB != null) lblNameB.Location = new Point(pnlChamberB.Left, pnlChamberB.Top - 20);
            if (lblNameC != null) lblNameC.Location = new Point(pnlChamberC.Left, pnlChamberC.Top - 20);
        }

        private void ChamberA_SetDoor(bool isOpen) { EtherCAT_M.Digital_Output(5, isOpen); EtherCAT_M.Digital_Output(4, !isOpen); }
        private void ChamberB_SetDoor(bool isOpen) { EtherCAT_M.Digital_Output(8, isOpen); EtherCAT_M.Digital_Output(7, !isOpen); }
        private void ChamberC_SetDoor(bool isOpen) { EtherCAT_M.Digital_Output(11, isOpen); EtherCAT_M.Digital_Output(10, !isOpen); }

    }
}