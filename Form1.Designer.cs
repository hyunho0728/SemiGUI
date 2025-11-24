namespace SemiGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.grpLogin = new System.Windows.Forms.GroupBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtPw = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblTm = new System.Windows.Forms.Label();
            this.pnlLedTm = new System.Windows.Forms.Panel();
            this.lblPmA_Led = new System.Windows.Forms.Label();
            this.pnlLedPmA = new System.Windows.Forms.Panel();
            this.lblPmB_Led = new System.Windows.Forms.Label();
            this.pnlLedPmB = new System.Windows.Forms.Panel();
            this.lblPmC_Led = new System.Windows.Forms.Label();
            this.pnlLedPmC = new System.Windows.Forms.Panel();
            this.pnlHost = new System.Windows.Forms.Panel();
            this.lblHostTitle = new System.Windows.Forms.Label();
            this.lblHostState = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pnlTime = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnMain = new System.Windows.Forms.Button();
            this.btnUtility = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.grpPmA = new System.Windows.Forms.GroupBox();
            this.lblStepA = new System.Windows.Forms.Label();
            this.txtProcessA = new System.Windows.Forms.TextBox();
            this.lblPressA = new System.Windows.Forms.Label();
            this.lblPressValA = new System.Windows.Forms.Label();
            this.lblProgA = new System.Windows.Forms.Label();
            this.progA = new System.Windows.Forms.ProgressBar();
            this.btnStopA = new System.Windows.Forms.Button();
            this.btnStartA = new System.Windows.Forms.Button();
            this.grpPmB = new System.Windows.Forms.GroupBox();
            this.lblStepB = new System.Windows.Forms.Label();
            this.txtProcessB = new System.Windows.Forms.TextBox();
            this.lblPressB = new System.Windows.Forms.Label();
            this.lblPressValB = new System.Windows.Forms.Label();
            this.lblProgB = new System.Windows.Forms.Label();
            this.progB = new System.Windows.Forms.ProgressBar();
            this.btnStopB = new System.Windows.Forms.Button();
            this.btnStartB = new System.Windows.Forms.Button();
            this.grpPmC = new System.Windows.Forms.GroupBox();
            this.lblStepC = new System.Windows.Forms.Label();
            this.txtProcessC = new System.Windows.Forms.TextBox();
            this.lblPressC = new System.Windows.Forms.Label();
            this.lblPressValC = new System.Windows.Forms.Label();
            this.lblProgC = new System.Windows.Forms.Label();
            this.progC = new System.Windows.Forms.ProgressBar();
            this.btnStopC = new System.Windows.Forms.Button();
            this.btnStartC = new System.Windows.Forms.Button();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpFormA = new System.Windows.Forms.GroupBox();
            this.lblInfoA = new System.Windows.Forms.Label();
            this.lblCarrierA = new System.Windows.Forms.Label();
            this.txtCarrierA = new System.Windows.Forms.TextBox();
            this.btnLoadA = new System.Windows.Forms.Button();
            this.btnUnloadA = new System.Windows.Forms.Button();
            this.grpFormB = new System.Windows.Forms.GroupBox();
            this.lblInfoB = new System.Windows.Forms.Label();
            this.lblCarrierB = new System.Windows.Forms.Label();
            this.txtCarrierB = new System.Windows.Forms.TextBox();
            this.btnLoadB = new System.Windows.Forms.Button();
            this.btnUnloadB = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlChamberA = new System.Windows.Forms.Panel();
            this.pnlChamberB = new System.Windows.Forms.Panel();
            this.pnlChamberC = new System.Windows.Forms.Panel();
            this.pnlFoupA = new System.Windows.Forms.Panel();
            this.pnlFoupB = new System.Windows.Forms.Panel();
            this.pnlCassetteL = new System.Windows.Forms.Panel();
            this.pnlWaferL1 = new System.Windows.Forms.Panel();
            this.pnlWaferL2 = new System.Windows.Forms.Panel();
            this.pnlWaferL3 = new System.Windows.Forms.Panel();
            this.pnlWaferL4 = new System.Windows.Forms.Panel();
            this.pnlWaferL5 = new System.Windows.Forms.Panel();
            this.pnlCassetteR = new System.Windows.Forms.Panel();
            this.pnlWaferR1 = new System.Windows.Forms.Panel();
            this.pnlWaferR2 = new System.Windows.Forms.Panel();
            this.pnlWaferR3 = new System.Windows.Forms.Panel();
            this.pnlWaferR4 = new System.Windows.Forms.Panel();
            this.pnlWaferR5 = new System.Windows.Forms.Panel();
            this.pnlTop.SuspendLayout();
            this.grpLogin.SuspendLayout();
            this.pnlHost.SuspendLayout();
            this.pnlTime.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.grpPmA.SuspendLayout();
            this.grpPmB.SuspendLayout();
            this.grpPmC.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.grpFormA.SuspendLayout();
            this.grpFormB.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.pnlCassetteL.SuspendLayout();
            this.pnlCassetteR.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTop.Controls.Add(this.grpLogin);
            this.pnlTop.Controls.Add(this.lblTm);
            this.pnlTop.Controls.Add(this.pnlLedTm);
            this.pnlTop.Controls.Add(this.lblPmA_Led);
            this.pnlTop.Controls.Add(this.pnlLedPmA);
            this.pnlTop.Controls.Add(this.lblPmB_Led);
            this.pnlTop.Controls.Add(this.pnlLedPmB);
            this.pnlTop.Controls.Add(this.lblPmC_Led);
            this.pnlTop.Controls.Add(this.pnlLedPmC);
            this.pnlTop.Controls.Add(this.pnlHost);
            this.pnlTop.Controls.Add(this.btnConnect);
            this.pnlTop.Controls.Add(this.pnlTime);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1280, 80);
            this.pnlTop.TabIndex = 4;
            // 
            // grpLogin
            // 
            this.grpLogin.Controls.Add(this.txtId);
            this.grpLogin.Controls.Add(this.txtPw);
            this.grpLogin.Controls.Add(this.btnLogin);
            this.grpLogin.Location = new System.Drawing.Point(10, 5);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new System.Drawing.Size(180, 65);
            this.grpLogin.TabIndex = 0;
            this.grpLogin.TabStop = false;
            this.grpLogin.Text = "LOGIN";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(10, 20);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(80, 21);
            this.txtId.TabIndex = 0;
            this.txtId.Text = "ID";
            // 
            // txtPw
            // 
            this.txtPw.Location = new System.Drawing.Point(10, 42);
            this.txtPw.Name = "txtPw";
            this.txtPw.PasswordChar = '*';
            this.txtPw.Size = new System.Drawing.Size(80, 21);
            this.txtPw.TabIndex = 1;
            this.txtPw.Text = "****";
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(100, 20);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(70, 40);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.UseVisualStyleBackColor = false;
            // 
            // lblTm
            // 
            this.lblTm.AutoSize = true;
            this.lblTm.Location = new System.Drawing.Point(220, 10);
            this.lblTm.Name = "lblTm";
            this.lblTm.Size = new System.Drawing.Size(24, 12);
            this.lblTm.TabIndex = 1;
            this.lblTm.Text = "TM";
            // 
            // pnlLedTm
            // 
            this.pnlLedTm.BackColor = System.Drawing.Color.SeaGreen;
            this.pnlLedTm.Location = new System.Drawing.Point(220, 30);
            this.pnlLedTm.Name = "pnlLedTm";
            this.pnlLedTm.Size = new System.Drawing.Size(30, 30);
            this.pnlLedTm.TabIndex = 2;
            // 
            // lblPmA_Led
            // 
            this.lblPmA_Led.AutoSize = true;
            this.lblPmA_Led.Location = new System.Drawing.Point(270, 10);
            this.lblPmA_Led.Name = "lblPmA_Led";
            this.lblPmA_Led.Size = new System.Drawing.Size(36, 12);
            this.lblPmA_Led.TabIndex = 3;
            this.lblPmA_Led.Text = "PM A";
            // 
            // pnlLedPmA
            // 
            this.pnlLedPmA.BackColor = System.Drawing.Color.SeaGreen;
            this.pnlLedPmA.Location = new System.Drawing.Point(270, 30);
            this.pnlLedPmA.Name = "pnlLedPmA";
            this.pnlLedPmA.Size = new System.Drawing.Size(30, 30);
            this.pnlLedPmA.TabIndex = 4;
            // 
            // lblPmB_Led
            // 
            this.lblPmB_Led.AutoSize = true;
            this.lblPmB_Led.Location = new System.Drawing.Point(320, 10);
            this.lblPmB_Led.Name = "lblPmB_Led";
            this.lblPmB_Led.Size = new System.Drawing.Size(36, 12);
            this.lblPmB_Led.TabIndex = 5;
            this.lblPmB_Led.Text = "PM B";
            // 
            // pnlLedPmB
            // 
            this.pnlLedPmB.BackColor = System.Drawing.Color.SeaGreen;
            this.pnlLedPmB.Location = new System.Drawing.Point(320, 30);
            this.pnlLedPmB.Name = "pnlLedPmB";
            this.pnlLedPmB.Size = new System.Drawing.Size(30, 30);
            this.pnlLedPmB.TabIndex = 6;
            // 
            // lblPmC_Led
            // 
            this.lblPmC_Led.AutoSize = true;
            this.lblPmC_Led.Location = new System.Drawing.Point(370, 10);
            this.lblPmC_Led.Name = "lblPmC_Led";
            this.lblPmC_Led.Size = new System.Drawing.Size(37, 12);
            this.lblPmC_Led.TabIndex = 7;
            this.lblPmC_Led.Text = "PM C";
            // 
            // pnlLedPmC
            // 
            this.pnlLedPmC.BackColor = System.Drawing.Color.SeaGreen;
            this.pnlLedPmC.Location = new System.Drawing.Point(370, 30);
            this.pnlLedPmC.Name = "pnlLedPmC";
            this.pnlLedPmC.Size = new System.Drawing.Size(30, 30);
            this.pnlLedPmC.TabIndex = 8;
            // 
            // pnlHost
            // 
            this.pnlHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHost.Controls.Add(this.lblHostTitle);
            this.pnlHost.Controls.Add(this.lblHostState);
            this.pnlHost.Location = new System.Drawing.Point(500, 10);
            this.pnlHost.Name = "pnlHost";
            this.pnlHost.Size = new System.Drawing.Size(150, 60);
            this.pnlHost.TabIndex = 9;
            // 
            // lblHostTitle
            // 
            this.lblHostTitle.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblHostTitle.Location = new System.Drawing.Point(10, 10);
            this.lblHostTitle.Name = "lblHostTitle";
            this.lblHostTitle.Size = new System.Drawing.Size(100, 23);
            this.lblHostTitle.TabIndex = 0;
            this.lblHostTitle.Text = "HOST";
            // 
            // lblHostState
            // 
            this.lblHostState.Location = new System.Drawing.Point(10, 35);
            this.lblHostState.Name = "lblHostState";
            this.lblHostState.Size = new System.Drawing.Size(100, 23);
            this.lblHostState.TabIndex = 1;
            this.lblHostState.Text = "NONE";
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Khaki;
            this.btnConnect.Location = new System.Drawing.Point(660, 10);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(80, 60);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = false;
            // 
            // pnlTime
            // 
            this.pnlTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTime.Controls.Add(this.lblDate);
            this.pnlTime.Controls.Add(this.lblTime);
            this.pnlTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTime.Location = new System.Drawing.Point(1128, 0);
            this.pnlTime.Name = "pnlTime";
            this.pnlTime.Size = new System.Drawing.Size(150, 78);
            this.pnlTime.TabIndex = 11;
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Arial", 12F);
            this.lblDate.Location = new System.Drawing.Point(10, 10);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(100, 23);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "2025/11/10";
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTime.Location = new System.Drawing.Point(10, 35);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 23);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "11:11:11";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottom.Controls.Add(this.btnMain);
            this.pnlBottom.Controls.Add(this.btnUtility);
            this.pnlBottom.Controls.Add(this.btnLog);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 840);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1280, 60);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnMain
            // 
            this.btnMain.BackColor = System.Drawing.Color.White;
            this.btnMain.Location = new System.Drawing.Point(500, 5);
            this.btnMain.Name = "btnMain";
            this.btnMain.Size = new System.Drawing.Size(80, 50);
            this.btnMain.TabIndex = 0;
            this.btnMain.Text = "MAIN";
            this.btnMain.UseVisualStyleBackColor = false;
            // 
            // btnUtility
            // 
            this.btnUtility.BackColor = System.Drawing.Color.White;
            this.btnUtility.Location = new System.Drawing.Point(590, 5);
            this.btnUtility.Name = "btnUtility";
            this.btnUtility.Size = new System.Drawing.Size(80, 50);
            this.btnUtility.TabIndex = 1;
            this.btnUtility.Text = "UTILITY";
            this.btnUtility.UseVisualStyleBackColor = false;
            // 
            // btnLog
            // 
            this.btnLog.BackColor = System.Drawing.Color.White;
            this.btnLog.Location = new System.Drawing.Point(680, 5);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(80, 50);
            this.btnLog.TabIndex = 2;
            this.btnLog.Text = "LOG";
            this.btnLog.UseVisualStyleBackColor = false;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.grpPmA);
            this.pnlLeft.Controls.Add(this.grpPmB);
            this.pnlLeft.Controls.Add(this.grpPmC);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 80);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(10);
            this.pnlLeft.Size = new System.Drawing.Size(300, 760);
            this.pnlLeft.TabIndex = 2;
            // 
            // grpPmA
            // 
            this.grpPmA.Controls.Add(this.lblStepA);
            this.grpPmA.Controls.Add(this.txtProcessA);
            this.grpPmA.Controls.Add(this.lblPressA);
            this.grpPmA.Controls.Add(this.lblPressValA);
            this.grpPmA.Controls.Add(this.lblProgA);
            this.grpPmA.Controls.Add(this.progA);
            this.grpPmA.Controls.Add(this.btnStopA);
            this.grpPmA.Controls.Add(this.btnStartA);
            this.grpPmA.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpPmA.Location = new System.Drawing.Point(10, 10);
            this.grpPmA.Name = "grpPmA";
            this.grpPmA.Size = new System.Drawing.Size(280, 180);
            this.grpPmA.TabIndex = 0;
            this.grpPmA.TabStop = false;
            this.grpPmA.Text = "PM A";
            // 
            // lblStepA
            // 
            this.lblStepA.Location = new System.Drawing.Point(10, 25);
            this.lblStepA.Name = "lblStepA";
            this.lblStepA.Size = new System.Drawing.Size(100, 23);
            this.lblStepA.TabIndex = 0;
            this.lblStepA.Text = "Step Name";
            // 
            // txtProcessA
            // 
            this.txtProcessA.Font = new System.Drawing.Font("Arial", 9F);
            this.txtProcessA.Location = new System.Drawing.Point(110, 22);
            this.txtProcessA.Name = "txtProcessA";
            this.txtProcessA.Size = new System.Drawing.Size(140, 21);
            this.txtProcessA.TabIndex = 1;
            this.txtProcessA.Text = "Process";
            // 
            // lblPressA
            // 
            this.lblPressA.Location = new System.Drawing.Point(10, 55);
            this.lblPressA.Name = "lblPressA";
            this.lblPressA.Size = new System.Drawing.Size(100, 23);
            this.lblPressA.TabIndex = 2;
            this.lblPressA.Text = "Pressure";
            // 
            // lblPressValA
            // 
            this.lblPressValA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPressValA.Location = new System.Drawing.Point(110, 52);
            this.lblPressValA.Name = "lblPressValA";
            this.lblPressValA.Size = new System.Drawing.Size(140, 20);
            this.lblPressValA.TabIndex = 3;
            this.lblPressValA.Text = "0.0 / 0.0";
            this.lblPressValA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgA
            // 
            this.lblProgA.Location = new System.Drawing.Point(10, 85);
            this.lblProgA.Name = "lblProgA";
            this.lblProgA.Size = new System.Drawing.Size(100, 23);
            this.lblProgA.TabIndex = 4;
            this.lblProgA.Text = "Progress";
            // 
            // progA
            // 
            this.progA.Location = new System.Drawing.Point(110, 85);
            this.progA.Name = "progA";
            this.progA.Size = new System.Drawing.Size(140, 20);
            this.progA.TabIndex = 5;
            this.progA.Value = 45;
            // 
            // btnStopA
            // 
            this.btnStopA.BackColor = System.Drawing.Color.White;
            this.btnStopA.Location = new System.Drawing.Point(10, 140);
            this.btnStopA.Name = "btnStopA";
            this.btnStopA.Size = new System.Drawing.Size(80, 30);
            this.btnStopA.TabIndex = 6;
            this.btnStopA.Text = "STOP";
            this.btnStopA.UseVisualStyleBackColor = false;
            // 
            // btnStartA
            // 
            this.btnStartA.BackColor = System.Drawing.Color.White;
            this.btnStartA.Location = new System.Drawing.Point(100, 140);
            this.btnStartA.Name = "btnStartA";
            this.btnStartA.Size = new System.Drawing.Size(80, 30);
            this.btnStartA.TabIndex = 7;
            this.btnStartA.Text = "START";
            this.btnStartA.UseVisualStyleBackColor = false;
            // 
            // grpPmB
            // 
            this.grpPmB.Controls.Add(this.lblStepB);
            this.grpPmB.Controls.Add(this.txtProcessB);
            this.grpPmB.Controls.Add(this.lblPressB);
            this.grpPmB.Controls.Add(this.lblPressValB);
            this.grpPmB.Controls.Add(this.lblProgB);
            this.grpPmB.Controls.Add(this.progB);
            this.grpPmB.Controls.Add(this.btnStopB);
            this.grpPmB.Controls.Add(this.btnStartB);
            this.grpPmB.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpPmB.Location = new System.Drawing.Point(10, 200);
            this.grpPmB.Name = "grpPmB";
            this.grpPmB.Size = new System.Drawing.Size(280, 180);
            this.grpPmB.TabIndex = 1;
            this.grpPmB.TabStop = false;
            this.grpPmB.Text = "PM B";
            // 
            // lblStepB
            // 
            this.lblStepB.Location = new System.Drawing.Point(10, 25);
            this.lblStepB.Name = "lblStepB";
            this.lblStepB.Size = new System.Drawing.Size(100, 23);
            this.lblStepB.TabIndex = 0;
            this.lblStepB.Text = "Step Name";
            // 
            // txtProcessB
            // 
            this.txtProcessB.Font = new System.Drawing.Font("Arial", 9F);
            this.txtProcessB.Location = new System.Drawing.Point(110, 22);
            this.txtProcessB.Name = "txtProcessB";
            this.txtProcessB.Size = new System.Drawing.Size(140, 21);
            this.txtProcessB.TabIndex = 1;
            this.txtProcessB.Text = "Process";
            // 
            // lblPressB
            // 
            this.lblPressB.Location = new System.Drawing.Point(10, 55);
            this.lblPressB.Name = "lblPressB";
            this.lblPressB.Size = new System.Drawing.Size(100, 23);
            this.lblPressB.TabIndex = 2;
            this.lblPressB.Text = "Pressure";
            // 
            // lblPressValB
            // 
            this.lblPressValB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPressValB.Location = new System.Drawing.Point(110, 52);
            this.lblPressValB.Name = "lblPressValB";
            this.lblPressValB.Size = new System.Drawing.Size(140, 20);
            this.lblPressValB.TabIndex = 3;
            this.lblPressValB.Text = "0.0 / 0.0";
            this.lblPressValB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgB
            // 
            this.lblProgB.Location = new System.Drawing.Point(10, 85);
            this.lblProgB.Name = "lblProgB";
            this.lblProgB.Size = new System.Drawing.Size(100, 23);
            this.lblProgB.TabIndex = 4;
            this.lblProgB.Text = "Progress";
            // 
            // progB
            // 
            this.progB.Location = new System.Drawing.Point(110, 85);
            this.progB.Name = "progB";
            this.progB.Size = new System.Drawing.Size(140, 20);
            this.progB.TabIndex = 5;
            this.progB.Value = 10;
            // 
            // btnStopB
            // 
            this.btnStopB.BackColor = System.Drawing.Color.White;
            this.btnStopB.Location = new System.Drawing.Point(10, 140);
            this.btnStopB.Name = "btnStopB";
            this.btnStopB.Size = new System.Drawing.Size(80, 30);
            this.btnStopB.TabIndex = 6;
            this.btnStopB.Text = "STOP";
            this.btnStopB.UseVisualStyleBackColor = false;
            // 
            // btnStartB
            // 
            this.btnStartB.BackColor = System.Drawing.Color.White;
            this.btnStartB.Location = new System.Drawing.Point(100, 140);
            this.btnStartB.Name = "btnStartB";
            this.btnStartB.Size = new System.Drawing.Size(80, 30);
            this.btnStartB.TabIndex = 7;
            this.btnStartB.Text = "START";
            this.btnStartB.UseVisualStyleBackColor = false;
            // 
            // grpPmC
            // 
            this.grpPmC.Controls.Add(this.lblStepC);
            this.grpPmC.Controls.Add(this.txtProcessC);
            this.grpPmC.Controls.Add(this.lblPressC);
            this.grpPmC.Controls.Add(this.lblPressValC);
            this.grpPmC.Controls.Add(this.lblProgC);
            this.grpPmC.Controls.Add(this.progC);
            this.grpPmC.Controls.Add(this.btnStopC);
            this.grpPmC.Controls.Add(this.btnStartC);
            this.grpPmC.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpPmC.Location = new System.Drawing.Point(10, 390);
            this.grpPmC.Name = "grpPmC";
            this.grpPmC.Size = new System.Drawing.Size(280, 180);
            this.grpPmC.TabIndex = 2;
            this.grpPmC.TabStop = false;
            this.grpPmC.Text = "PM C";
            // 
            // lblStepC
            // 
            this.lblStepC.Location = new System.Drawing.Point(10, 25);
            this.lblStepC.Name = "lblStepC";
            this.lblStepC.Size = new System.Drawing.Size(100, 23);
            this.lblStepC.TabIndex = 0;
            this.lblStepC.Text = "Step Name";
            // 
            // txtProcessC
            // 
            this.txtProcessC.Font = new System.Drawing.Font("Arial", 9F);
            this.txtProcessC.Location = new System.Drawing.Point(110, 22);
            this.txtProcessC.Name = "txtProcessC";
            this.txtProcessC.Size = new System.Drawing.Size(140, 21);
            this.txtProcessC.TabIndex = 1;
            this.txtProcessC.Text = "Process";
            // 
            // lblPressC
            // 
            this.lblPressC.Location = new System.Drawing.Point(10, 55);
            this.lblPressC.Name = "lblPressC";
            this.lblPressC.Size = new System.Drawing.Size(100, 23);
            this.lblPressC.TabIndex = 2;
            this.lblPressC.Text = "Pressure";
            // 
            // lblPressValC
            // 
            this.lblPressValC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPressValC.Location = new System.Drawing.Point(110, 52);
            this.lblPressValC.Name = "lblPressValC";
            this.lblPressValC.Size = new System.Drawing.Size(140, 20);
            this.lblPressValC.TabIndex = 3;
            this.lblPressValC.Text = "0.0 / 0.0";
            this.lblPressValC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgC
            // 
            this.lblProgC.Location = new System.Drawing.Point(10, 85);
            this.lblProgC.Name = "lblProgC";
            this.lblProgC.Size = new System.Drawing.Size(100, 23);
            this.lblProgC.TabIndex = 4;
            this.lblProgC.Text = "Progress";
            // 
            // progC
            // 
            this.progC.Location = new System.Drawing.Point(110, 85);
            this.progC.Name = "progC";
            this.progC.Size = new System.Drawing.Size(140, 20);
            this.progC.TabIndex = 5;
            // 
            // btnStopC
            // 
            this.btnStopC.BackColor = System.Drawing.Color.White;
            this.btnStopC.Location = new System.Drawing.Point(10, 140);
            this.btnStopC.Name = "btnStopC";
            this.btnStopC.Size = new System.Drawing.Size(80, 30);
            this.btnStopC.TabIndex = 6;
            this.btnStopC.Text = "STOP";
            this.btnStopC.UseVisualStyleBackColor = false;
            // 
            // btnStartC
            // 
            this.btnStartC.BackColor = System.Drawing.Color.White;
            this.btnStartC.Location = new System.Drawing.Point(100, 140);
            this.btnStartC.Name = "btnStartC";
            this.btnStartC.Size = new System.Drawing.Size(80, 30);
            this.btnStartC.TabIndex = 7;
            this.btnStartC.Text = "START";
            this.btnStartC.UseVisualStyleBackColor = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.grpFormA);
            this.pnlRight.Controls.Add(this.grpFormB);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(980, 80);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);
            this.pnlRight.Size = new System.Drawing.Size(300, 760);
            this.pnlRight.TabIndex = 1;
            // 
            // grpFormA
            // 
            this.grpFormA.Controls.Add(this.lblInfoA);
            this.grpFormA.Controls.Add(this.lblCarrierA);
            this.grpFormA.Controls.Add(this.txtCarrierA);
            this.grpFormA.Controls.Add(this.btnLoadA);
            this.grpFormA.Controls.Add(this.btnUnloadA);
            this.grpFormA.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpFormA.Location = new System.Drawing.Point(10, 10);
            this.grpFormA.Name = "grpFormA";
            this.grpFormA.Size = new System.Drawing.Size(280, 250);
            this.grpFormA.TabIndex = 0;
            this.grpFormA.TabStop = false;
            this.grpFormA.Text = "Form A";
            // 
            // lblInfoA
            // 
            this.lblInfoA.AutoSize = true;
            this.lblInfoA.Location = new System.Drawing.Point(10, 25);
            this.lblInfoA.Name = "lblInfoA";
            this.lblInfoA.Size = new System.Drawing.Size(133, 48);
            this.lblInfoA.TabIndex = 0;
            this.lblInfoA.Text = "LPB Status: Ready\nMode: MANUAL\nRun: Idle";
            // 
            // lblCarrierA
            // 
            this.lblCarrierA.Location = new System.Drawing.Point(10, 90);
            this.lblCarrierA.Name = "lblCarrierA";
            this.lblCarrierA.Size = new System.Drawing.Size(100, 23);
            this.lblCarrierA.TabIndex = 1;
            this.lblCarrierA.Text = "Carrier ID";
            // 
            // txtCarrierA
            // 
            this.txtCarrierA.Location = new System.Drawing.Point(80, 88);
            this.txtCarrierA.Name = "txtCarrierA";
            this.txtCarrierA.Size = new System.Drawing.Size(180, 23);
            this.txtCarrierA.TabIndex = 2;
            this.txtCarrierA.Text = "FOUP_LOT01";
            // 
            // btnLoadA
            // 
            this.btnLoadA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnLoadA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadA.ForeColor = System.Drawing.Color.White;
            this.btnLoadA.Location = new System.Drawing.Point(10, 150);
            this.btnLoadA.Name = "btnLoadA";
            this.btnLoadA.Size = new System.Drawing.Size(260, 40);
            this.btnLoadA.TabIndex = 3;
            this.btnLoadA.Text = "LOAD FOUP";
            this.btnLoadA.UseVisualStyleBackColor = false;
            // 
            // btnUnloadA
            // 
            this.btnUnloadA.BackColor = System.Drawing.Color.Red;
            this.btnUnloadA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadA.ForeColor = System.Drawing.Color.White;
            this.btnUnloadA.Location = new System.Drawing.Point(10, 200);
            this.btnUnloadA.Name = "btnUnloadA";
            this.btnUnloadA.Size = new System.Drawing.Size(260, 40);
            this.btnUnloadA.TabIndex = 4;
            this.btnUnloadA.Text = "UNLOAD FOUP";
            this.btnUnloadA.UseVisualStyleBackColor = false;
            // 
            // grpFormB
            // 
            this.grpFormB.Controls.Add(this.lblInfoB);
            this.grpFormB.Controls.Add(this.lblCarrierB);
            this.grpFormB.Controls.Add(this.txtCarrierB);
            this.grpFormB.Controls.Add(this.btnLoadB);
            this.grpFormB.Controls.Add(this.btnUnloadB);
            this.grpFormB.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpFormB.Location = new System.Drawing.Point(10, 270);
            this.grpFormB.Name = "grpFormB";
            this.grpFormB.Size = new System.Drawing.Size(280, 250);
            this.grpFormB.TabIndex = 1;
            this.grpFormB.TabStop = false;
            this.grpFormB.Text = "Form B";
            // 
            // lblInfoB
            // 
            this.lblInfoB.AutoSize = true;
            this.lblInfoB.Location = new System.Drawing.Point(10, 25);
            this.lblInfoB.Name = "lblInfoB";
            this.lblInfoB.Size = new System.Drawing.Size(133, 48);
            this.lblInfoB.TabIndex = 0;
            this.lblInfoB.Text = "LPB Status: Ready\nMode: MANUAL\nRun: Idle";
            // 
            // lblCarrierB
            // 
            this.lblCarrierB.Location = new System.Drawing.Point(10, 90);
            this.lblCarrierB.Name = "lblCarrierB";
            this.lblCarrierB.Size = new System.Drawing.Size(100, 23);
            this.lblCarrierB.TabIndex = 1;
            this.lblCarrierB.Text = "Carrier ID";
            // 
            // txtCarrierB
            // 
            this.txtCarrierB.Location = new System.Drawing.Point(80, 88);
            this.txtCarrierB.Name = "txtCarrierB";
            this.txtCarrierB.Size = new System.Drawing.Size(180, 23);
            this.txtCarrierB.TabIndex = 2;
            this.txtCarrierB.Text = "FOUP_LOT02";
            // 
            // btnLoadB
            // 
            this.btnLoadB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnLoadB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadB.ForeColor = System.Drawing.Color.White;
            this.btnLoadB.Location = new System.Drawing.Point(10, 150);
            this.btnLoadB.Name = "btnLoadB";
            this.btnLoadB.Size = new System.Drawing.Size(260, 40);
            this.btnLoadB.TabIndex = 3;
            this.btnLoadB.Text = "LOAD FOUP";
            this.btnLoadB.UseVisualStyleBackColor = false;
            // 
            // btnUnloadB
            // 
            this.btnUnloadB.BackColor = System.Drawing.Color.Red;
            this.btnUnloadB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadB.ForeColor = System.Drawing.Color.White;
            this.btnUnloadB.Location = new System.Drawing.Point(10, 200);
            this.btnUnloadB.Name = "btnUnloadB";
            this.btnUnloadB.Size = new System.Drawing.Size(260, 40);
            this.btnUnloadB.TabIndex = 4;
            this.btnUnloadB.Text = "UNLOAD FOUP";
            this.btnUnloadB.UseVisualStyleBackColor = false;
            // 
            // pnlCenter
            // 
            this.pnlCenter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlCenter.Controls.Add(this.pnlChamberA);
            this.pnlCenter.Controls.Add(this.pnlChamberB);
            this.pnlCenter.Controls.Add(this.pnlChamberC);
            this.pnlCenter.Controls.Add(this.pnlFoupA);
            this.pnlCenter.Controls.Add(this.pnlFoupB);
            this.pnlCenter.Controls.Add(this.pnlCassetteL);
            this.pnlCenter.Controls.Add(this.pnlCassetteR);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(300, 80);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(680, 760);
            this.pnlCenter.TabIndex = 0;
            // 
            // pnlChamberA
            // 
            this.pnlChamberA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.pnlChamberA.Location = new System.Drawing.Point(100, 300);
            this.pnlChamberA.Name = "pnlChamberA";
            this.pnlChamberA.Size = new System.Drawing.Size(80, 100);
            this.pnlChamberA.TabIndex = 0;
            // 
            // pnlChamberB
            // 
            this.pnlChamberB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.pnlChamberB.Location = new System.Drawing.Point(300, 100);
            this.pnlChamberB.Name = "pnlChamberB";
            this.pnlChamberB.Size = new System.Drawing.Size(80, 100);
            this.pnlChamberB.TabIndex = 1;
            // 
            // pnlChamberC
            // 
            this.pnlChamberC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.pnlChamberC.Location = new System.Drawing.Point(500, 300);
            this.pnlChamberC.Name = "pnlChamberC";
            this.pnlChamberC.Size = new System.Drawing.Size(80, 100);
            this.pnlChamberC.TabIndex = 2;
            // 
            // pnlFoupA
            // 
            this.pnlFoupA.BackColor = System.Drawing.Color.Silver;
            this.pnlFoupA.Location = new System.Drawing.Point(150, 500);
            this.pnlFoupA.Name = "pnlFoupA";
            this.pnlFoupA.Size = new System.Drawing.Size(80, 80);
            this.pnlFoupA.TabIndex = 3;
            // 
            // pnlFoupB
            // 
            this.pnlFoupB.BackColor = System.Drawing.Color.Silver;
            this.pnlFoupB.Location = new System.Drawing.Point(450, 500);
            this.pnlFoupB.Name = "pnlFoupB";
            this.pnlFoupB.Size = new System.Drawing.Size(80, 80);
            this.pnlFoupB.TabIndex = 4;
            // 
            // pnlCassetteL
            // 
            this.pnlCassetteL.BackColor = System.Drawing.Color.White;
            this.pnlCassetteL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCassetteL.Controls.Add(this.pnlWaferL1);
            this.pnlCassetteL.Controls.Add(this.pnlWaferL2);
            this.pnlCassetteL.Controls.Add(this.pnlWaferL3);
            this.pnlCassetteL.Controls.Add(this.pnlWaferL4);
            this.pnlCassetteL.Controls.Add(this.pnlWaferL5);
            this.pnlCassetteL.Location = new System.Drawing.Point(240, 650);
            this.pnlCassetteL.Name = "pnlCassetteL";
            this.pnlCassetteL.Size = new System.Drawing.Size(80, 80);
            this.pnlCassetteL.TabIndex = 5;
            // 
            // pnlWaferL1
            // 
            this.pnlWaferL1.BackColor = System.Drawing.Color.Blue;
            this.pnlWaferL1.Location = new System.Drawing.Point(5, 10);
            this.pnlWaferL1.Name = "pnlWaferL1";
            this.pnlWaferL1.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferL1.TabIndex = 0;
            // 
            // pnlWaferL2
            // 
            this.pnlWaferL2.BackColor = System.Drawing.Color.Blue;
            this.pnlWaferL2.Location = new System.Drawing.Point(5, 24);
            this.pnlWaferL2.Name = "pnlWaferL2";
            this.pnlWaferL2.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferL2.TabIndex = 1;
            // 
            // pnlWaferL3
            // 
            this.pnlWaferL3.BackColor = System.Drawing.Color.Blue;
            this.pnlWaferL3.Location = new System.Drawing.Point(5, 38);
            this.pnlWaferL3.Name = "pnlWaferL3";
            this.pnlWaferL3.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferL3.TabIndex = 2;
            // 
            // pnlWaferL4
            // 
            this.pnlWaferL4.BackColor = System.Drawing.Color.Blue;
            this.pnlWaferL4.Location = new System.Drawing.Point(5, 52);
            this.pnlWaferL4.Name = "pnlWaferL4";
            this.pnlWaferL4.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferL4.TabIndex = 3;
            // 
            // pnlWaferL5
            // 
            this.pnlWaferL5.BackColor = System.Drawing.Color.Blue;
            this.pnlWaferL5.Location = new System.Drawing.Point(5, 66);
            this.pnlWaferL5.Name = "pnlWaferL5";
            this.pnlWaferL5.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferL5.TabIndex = 4;
            // 
            // pnlCassetteR
            // 
            this.pnlCassetteR.BackColor = System.Drawing.Color.White;
            this.pnlCassetteR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCassetteR.Controls.Add(this.pnlWaferR1);
            this.pnlCassetteR.Controls.Add(this.pnlWaferR2);
            this.pnlCassetteR.Controls.Add(this.pnlWaferR3);
            this.pnlCassetteR.Controls.Add(this.pnlWaferR4);
            this.pnlCassetteR.Controls.Add(this.pnlWaferR5);
            this.pnlCassetteR.Location = new System.Drawing.Point(360, 650);
            this.pnlCassetteR.Name = "pnlCassetteR";
            this.pnlCassetteR.Size = new System.Drawing.Size(80, 80);
            this.pnlCassetteR.TabIndex = 6;
            // 
            // pnlWaferR1
            // 
            this.pnlWaferR1.BackColor = System.Drawing.Color.Black;
            this.pnlWaferR1.Location = new System.Drawing.Point(5, 10);
            this.pnlWaferR1.Name = "pnlWaferR1";
            this.pnlWaferR1.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferR1.TabIndex = 0;
            // 
            // pnlWaferR2
            // 
            this.pnlWaferR2.BackColor = System.Drawing.Color.Black;
            this.pnlWaferR2.Location = new System.Drawing.Point(5, 24);
            this.pnlWaferR2.Name = "pnlWaferR2";
            this.pnlWaferR2.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferR2.TabIndex = 1;
            // 
            // pnlWaferR3
            // 
            this.pnlWaferR3.BackColor = System.Drawing.Color.Black;
            this.pnlWaferR3.Location = new System.Drawing.Point(5, 38);
            this.pnlWaferR3.Name = "pnlWaferR3";
            this.pnlWaferR3.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferR3.TabIndex = 2;
            // 
            // pnlWaferR4
            // 
            this.pnlWaferR4.BackColor = System.Drawing.Color.Black;
            this.pnlWaferR4.Location = new System.Drawing.Point(5, 52);
            this.pnlWaferR4.Name = "pnlWaferR4";
            this.pnlWaferR4.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferR4.TabIndex = 3;
            // 
            // pnlWaferR5
            // 
            this.pnlWaferR5.BackColor = System.Drawing.Color.Black;
            this.pnlWaferR5.Location = new System.Drawing.Point(5, 66);
            this.pnlWaferR5.Name = "pnlWaferR5";
            this.pnlWaferR5.Size = new System.Drawing.Size(70, 8);
            this.pnlWaferR5.TabIndex = 4;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Equipment Control System";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            this.pnlHost.ResumeLayout(false);
            this.pnlTime.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.grpPmA.ResumeLayout(false);
            this.grpPmA.PerformLayout();
            this.grpPmB.ResumeLayout(false);
            this.grpPmB.PerformLayout();
            this.grpPmC.ResumeLayout(false);
            this.grpPmC.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.grpFormA.ResumeLayout(false);
            this.grpFormA.PerformLayout();
            this.grpFormB.ResumeLayout(false);
            this.grpFormB.PerformLayout();
            this.pnlCenter.ResumeLayout(false);
            this.pnlCassetteL.ResumeLayout(false);
            this.pnlCassetteR.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // ... (기존 변수 선언들) ...
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.GroupBox grpLogin;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtPw;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblTm;
        private System.Windows.Forms.Panel pnlLedTm;
        private System.Windows.Forms.Label lblPmA_Led;
        private System.Windows.Forms.Panel pnlLedPmA;
        private System.Windows.Forms.Label lblPmB_Led;
        private System.Windows.Forms.Panel pnlLedPmB;
        private System.Windows.Forms.Label lblPmC_Led;
        private System.Windows.Forms.Panel pnlLedPmC;
        private System.Windows.Forms.Panel pnlHost;
        private System.Windows.Forms.Label lblHostTitle;
        private System.Windows.Forms.Label lblHostState;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel pnlTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.GroupBox grpPmA;
        private System.Windows.Forms.Label lblStepA;
        private System.Windows.Forms.TextBox txtProcessA;
        private System.Windows.Forms.Label lblPressA;
        private System.Windows.Forms.Label lblPressValA;
        private System.Windows.Forms.Label lblProgA;
        private System.Windows.Forms.ProgressBar progA;
        private System.Windows.Forms.Button btnStopA;
        private System.Windows.Forms.Button btnStartA;
        private System.Windows.Forms.GroupBox grpPmB;
        private System.Windows.Forms.Label lblStepB;
        private System.Windows.Forms.TextBox txtProcessB;
        private System.Windows.Forms.Label lblPressB;
        private System.Windows.Forms.Label lblPressValB;
        private System.Windows.Forms.Label lblProgB;
        private System.Windows.Forms.ProgressBar progB;
        private System.Windows.Forms.Button btnStopB;
        private System.Windows.Forms.Button btnStartB;
        private System.Windows.Forms.GroupBox grpPmC;
        private System.Windows.Forms.Label lblStepC;
        private System.Windows.Forms.TextBox txtProcessC;
        private System.Windows.Forms.Label lblPressC;
        private System.Windows.Forms.Label lblPressValC;
        private System.Windows.Forms.Label lblProgC;
        private System.Windows.Forms.ProgressBar progC;
        private System.Windows.Forms.Button btnStopC;
        private System.Windows.Forms.Button btnStartC;
        private System.Windows.Forms.GroupBox grpFormA;
        private System.Windows.Forms.Label lblInfoA;
        private System.Windows.Forms.Label lblCarrierA;
        private System.Windows.Forms.TextBox txtCarrierA;
        private System.Windows.Forms.Button btnLoadA;
        private System.Windows.Forms.Button btnUnloadA;
        private System.Windows.Forms.GroupBox grpFormB;
        private System.Windows.Forms.Label lblInfoB;
        private System.Windows.Forms.Label lblCarrierB;
        private System.Windows.Forms.TextBox txtCarrierB;
        private System.Windows.Forms.Button btnLoadB;
        private System.Windows.Forms.Button btnUnloadB;
        private System.Windows.Forms.Button btnMain;
        private System.Windows.Forms.Button btnUtility;
        private System.Windows.Forms.Button btnLog;

        // 챔버 및 FOUP 패널
        private System.Windows.Forms.Panel pnlChamberA;
        private System.Windows.Forms.Panel pnlChamberB;
        private System.Windows.Forms.Panel pnlChamberC;
        private System.Windows.Forms.Panel pnlFoupA;
        private System.Windows.Forms.Panel pnlFoupB;

        // 카세트 및 웨이퍼
        private System.Windows.Forms.Panel pnlCassetteL;
        private System.Windows.Forms.Panel pnlWaferL1;
        private System.Windows.Forms.Panel pnlWaferL2;
        private System.Windows.Forms.Panel pnlWaferL3;
        private System.Windows.Forms.Panel pnlWaferL4;
        private System.Windows.Forms.Panel pnlWaferL5;

        private System.Windows.Forms.Panel pnlCassetteR;
        private System.Windows.Forms.Panel pnlWaferR1;
        private System.Windows.Forms.Panel pnlWaferR2;
        private System.Windows.Forms.Panel pnlWaferR3;
        private System.Windows.Forms.Panel pnlWaferR4;
        private System.Windows.Forms.Panel pnlWaferR5;
    }
}