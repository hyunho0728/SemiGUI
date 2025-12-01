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
            this.pnlAlarm = new System.Windows.Forms.Panel();
            this.lblAlarmMsg = new System.Windows.Forms.Label();
            this.pnlTime = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnMain = new System.Windows.Forms.Button();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.grpPmA = new System.Windows.Forms.GroupBox();
            this.lblStepA = new System.Windows.Forms.Label();
            this.txtProcessA = new System.Windows.Forms.TextBox();
            this.lblTargetA = new System.Windows.Forms.Label();
            this.valTargetA = new System.Windows.Forms.Label();
            this.lblGasA = new System.Windows.Forms.Label();
            this.valGasA = new System.Windows.Forms.Label();
            this.lblCurrA = new System.Windows.Forms.Label();
            this.valCurrA = new System.Windows.Forms.Label();
            this.lblTimeA = new System.Windows.Forms.Label();
            this.valTimeA = new System.Windows.Forms.Label();
            this.lblProgA = new System.Windows.Forms.Label();
            this.progA = new System.Windows.Forms.ProgressBar();
            this.btnStopA = new System.Windows.Forms.Button();
            this.btnStartA = new System.Windows.Forms.Button();
            this.grpPmB = new System.Windows.Forms.GroupBox();
            this.lblStepB = new System.Windows.Forms.Label();
            this.txtProcessB = new System.Windows.Forms.TextBox();
            this.lblAlignB = new System.Windows.Forms.Label();
            this.valAlignB = new System.Windows.Forms.Label();
            this.lblRpmB = new System.Windows.Forms.Label();
            this.valRpmB = new System.Windows.Forms.Label();
            this.lblTimeB = new System.Windows.Forms.Label();
            this.valTimeB = new System.Windows.Forms.Label();
            this.lblProgB = new System.Windows.Forms.Label();
            this.progB = new System.Windows.Forms.ProgressBar();
            this.btnStopB = new System.Windows.Forms.Button();
            this.btnStartB = new System.Windows.Forms.Button();
            this.grpPmC = new System.Windows.Forms.GroupBox();
            this.lblStepC = new System.Windows.Forms.Label();
            this.txtProcessC = new System.Windows.Forms.TextBox();
            this.lblPressC = new System.Windows.Forms.Label();
            this.valPressC = new System.Windows.Forms.Label();
            this.lblGasC = new System.Windows.Forms.Label();
            this.valGasC = new System.Windows.Forms.Label();
            this.lblSpinTimeC = new System.Windows.Forms.Label();
            this.valSpinTimeC = new System.Windows.Forms.Label();
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
            this.lblNameA = new System.Windows.Forms.Label();
            this.lblNameB = new System.Windows.Forms.Label();
            this.lblNameC = new System.Windows.Forms.Label();
            this.pnlChamberB = new System.Windows.Forms.Panel();
            this.pnlChamberA = new System.Windows.Forms.Panel();
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

            // [추가] 리셋 버튼 선언
            this.btnResetChambers = new System.Windows.Forms.Button();

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
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "Form1";
            this.Text = "Equipment Control System";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            // Top Panel
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top; this.pnlTop.Height = 80; this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grpLogin.Text = "LOGIN"; this.grpLogin.Location = new System.Drawing.Point(10, 5); this.grpLogin.Size = new System.Drawing.Size(180, 65);
            this.txtId.Text = "ID"; this.txtId.Location = new System.Drawing.Point(10, 20); this.txtId.Width = 80;
            this.txtPw.Text = "****"; this.txtPw.Location = new System.Drawing.Point(10, 42); this.txtPw.Width = 80; this.txtPw.PasswordChar = '*';
            this.btnLogin.Text = "LOGIN"; this.btnLogin.Location = new System.Drawing.Point(100, 20); this.btnLogin.Size = new System.Drawing.Size(70, 40); this.btnLogin.BackColor = System.Drawing.Color.White;
            this.grpLogin.Controls.Add(this.txtId); this.grpLogin.Controls.Add(this.txtPw); this.grpLogin.Controls.Add(this.btnLogin);
            this.pnlTop.Controls.Add(this.grpLogin);
            this.lblTm.Text = "TM"; this.lblTm.Location = new System.Drawing.Point(220, 10); this.lblTm.AutoSize = true;
            this.pnlLedTm.Location = new System.Drawing.Point(220, 30); this.pnlLedTm.Size = new System.Drawing.Size(30, 30); this.pnlLedTm.BackColor = System.Drawing.Color.SeaGreen;
            this.lblPmA_Led.Text = "PM A"; this.lblPmA_Led.Location = new System.Drawing.Point(270, 10); this.lblPmA_Led.AutoSize = true;
            this.pnlLedPmA.Location = new System.Drawing.Point(270, 30); this.pnlLedPmA.Size = new System.Drawing.Size(30, 30); this.pnlLedPmA.BackColor = System.Drawing.Color.SeaGreen;
            this.lblPmB_Led.Text = "PM B"; this.lblPmB_Led.Location = new System.Drawing.Point(320, 10); this.lblPmB_Led.AutoSize = true;
            this.pnlLedPmB.Location = new System.Drawing.Point(320, 30); this.pnlLedPmB.Size = new System.Drawing.Size(30, 30); this.pnlLedPmB.BackColor = System.Drawing.Color.SeaGreen;
            this.lblPmC_Led.Text = "PM C"; this.lblPmC_Led.Location = new System.Drawing.Point(370, 10); this.lblPmC_Led.AutoSize = true;
            this.pnlLedPmC.Location = new System.Drawing.Point(370, 30); this.pnlLedPmC.Size = new System.Drawing.Size(30, 30); this.pnlLedPmC.BackColor = System.Drawing.Color.SeaGreen;
            this.pnlTop.Controls.Add(this.lblTm); this.pnlTop.Controls.Add(this.pnlLedTm);
            this.pnlTop.Controls.Add(this.lblPmA_Led); this.pnlTop.Controls.Add(this.pnlLedPmA);
            this.pnlTop.Controls.Add(this.lblPmB_Led); this.pnlTop.Controls.Add(this.pnlLedPmB);
            this.pnlTop.Controls.Add(this.lblPmC_Led); this.pnlTop.Controls.Add(this.pnlLedPmC);
            this.pnlHost.Location = new System.Drawing.Point(500, 10); this.pnlHost.Size = new System.Drawing.Size(150, 60); this.pnlHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHostTitle.Text = "HOST"; this.lblHostTitle.Location = new System.Drawing.Point(10, 10); this.lblHostTitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblHostState.Text = "NONE"; this.lblHostState.Location = new System.Drawing.Point(10, 35);
            this.pnlHost.Controls.Add(this.lblHostTitle); this.pnlHost.Controls.Add(this.lblHostState);
            this.pnlTop.Controls.Add(this.pnlHost);
            this.btnConnect.Text = "CONNECT"; this.btnConnect.Location = new System.Drawing.Point(660, 10); this.btnConnect.Size = new System.Drawing.Size(80, 60); this.btnConnect.BackColor = System.Drawing.Color.Khaki;
            this.pnlTop.Controls.Add(this.btnConnect);

            this.pnlAlarm.Location = new System.Drawing.Point(880, 10);
            this.pnlAlarm.Size = new System.Drawing.Size(60, 60);
            this.pnlAlarm.BackColor = System.Drawing.Color.Transparent;
            this.pnlTop.Controls.Add(this.pnlAlarm);

            this.lblAlarmMsg.Location = new System.Drawing.Point(950, 25);
            this.lblAlarmMsg.AutoSize = true;
            this.lblAlarmMsg.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblAlarmMsg.ForeColor = System.Drawing.Color.Red;
            this.lblAlarmMsg.Text = "";
            this.pnlTop.Controls.Add(this.lblAlarmMsg);

            this.pnlTime.Dock = System.Windows.Forms.DockStyle.Right; this.pnlTime.Width = 150; this.pnlTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Text = "2025/11/10"; this.lblDate.Location = new System.Drawing.Point(10, 10); this.lblDate.Font = new System.Drawing.Font("Arial", 12);
            this.lblTime.Text = "11:11:11"; this.lblTime.Location = new System.Drawing.Point(10, 35); this.lblTime.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            this.pnlTime.Controls.Add(this.lblDate); this.pnlTime.Controls.Add(this.lblTime);
            this.pnlTop.Controls.Add(this.pnlTime);

            // Left Panel
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left; this.pnlLeft.Width = 300; this.pnlLeft.Padding = new System.Windows.Forms.Padding(10);

            // PM A
            this.grpPmA.Text = "PM A"; this.grpPmA.Location = new System.Drawing.Point(10, 10); this.grpPmA.Size = new System.Drawing.Size(280, 180); this.grpPmA.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStepA.Text = "Step Name"; this.lblStepA.Location = new System.Drawing.Point(10, 25); this.lblStepA.AutoSize = true;
            this.txtProcessA.Text = "Temp Ramp-up"; this.txtProcessA.Location = new System.Drawing.Point(90, 22); this.txtProcessA.Width = 180; this.txtProcessA.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.lblTargetA.Text = "Target Temp"; this.lblTargetA.Location = new System.Drawing.Point(10, 55); this.lblTargetA.AutoSize = true; this.lblTargetA.Font = new System.Drawing.Font("Arial", 8F);
            this.valTargetA.Text = "1000.0"; this.valTargetA.Location = new System.Drawing.Point(90, 52); this.valTargetA.Size = new System.Drawing.Size(40, 20); this.valTargetA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valTargetA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valTargetA.BackColor = System.Drawing.Color.White;
            this.lblGasA.Text = "Gas Flow"; this.lblGasA.Location = new System.Drawing.Point(145, 55); this.lblGasA.AutoSize = true; this.lblGasA.Font = new System.Drawing.Font("Arial", 8F);
            this.valGasA.Text = "0.0"; this.valGasA.Location = new System.Drawing.Point(210, 52); this.valGasA.Size = new System.Drawing.Size(40, 20); this.valGasA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valGasA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valGasA.BackColor = System.Drawing.Color.White;
            this.lblCurrA.Text = "Current Temp"; this.lblCurrA.Location = new System.Drawing.Point(10, 85); this.lblCurrA.AutoSize = true; this.lblCurrA.Font = new System.Drawing.Font("Arial", 8F);
            this.valCurrA.Text = "850.0"; this.valCurrA.Location = new System.Drawing.Point(90, 82); this.valCurrA.Size = new System.Drawing.Size(40, 20); this.valCurrA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valCurrA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valCurrA.BackColor = System.Drawing.Color.White;
            this.lblTimeA.Text = "Step Time"; this.lblTimeA.Location = new System.Drawing.Point(145, 85); this.lblTimeA.AutoSize = true; this.lblTimeA.Font = new System.Drawing.Font("Arial", 8F);
            this.valTimeA.Text = "00 / 00"; this.valTimeA.Location = new System.Drawing.Point(210, 82); this.valTimeA.Size = new System.Drawing.Size(60, 20); this.valTimeA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valTimeA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valTimeA.BackColor = System.Drawing.Color.White;
            this.lblProgA.Text = "Progress"; this.lblProgA.Location = new System.Drawing.Point(10, 115); this.lblProgA.AutoSize = true;
            this.progA.Value = 0; this.progA.Location = new System.Drawing.Point(90, 115); this.progA.Size = new System.Drawing.Size(180, 15);
            this.btnStopA.Text = "STOP"; this.btnStopA.Location = new System.Drawing.Point(10, 140); this.btnStopA.Size = new System.Drawing.Size(80, 30); this.btnStopA.BackColor = System.Drawing.Color.White;
            this.btnStartA.Text = "START"; this.btnStartA.Location = new System.Drawing.Point(100, 140); this.btnStartA.Size = new System.Drawing.Size(80, 30); this.btnStartA.BackColor = System.Drawing.Color.White;
            this.grpPmA.Controls.Add(this.lblStepA); this.grpPmA.Controls.Add(this.txtProcessA);
            this.grpPmA.Controls.Add(this.lblTargetA); this.grpPmA.Controls.Add(this.valTargetA);
            this.grpPmA.Controls.Add(this.lblGasA); this.grpPmA.Controls.Add(this.valGasA);
            this.grpPmA.Controls.Add(this.lblCurrA); this.grpPmA.Controls.Add(this.valCurrA);
            this.grpPmA.Controls.Add(this.lblTimeA); this.grpPmA.Controls.Add(this.valTimeA);
            this.grpPmA.Controls.Add(this.lblProgA); this.grpPmA.Controls.Add(this.progA);
            this.grpPmA.Controls.Add(this.btnStopA); this.grpPmA.Controls.Add(this.btnStartA);
            this.pnlLeft.Controls.Add(this.grpPmA);

            // PM B
            this.grpPmB.Text = "PM B"; this.grpPmB.Location = new System.Drawing.Point(10, 200); this.grpPmB.Size = new System.Drawing.Size(280, 180); this.grpPmB.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStepB.Text = "Step Name"; this.lblStepB.Location = new System.Drawing.Point(10, 25); this.lblStepB.AutoSize = true;
            this.txtProcessB.Text = "PR Coating"; this.txtProcessB.Location = new System.Drawing.Point(90, 22); this.txtProcessB.Width = 180; this.txtProcessB.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.lblAlignB.Text = "Align Accu"; this.lblAlignB.Location = new System.Drawing.Point(10, 55); this.lblAlignB.AutoSize = true; this.lblAlignB.Font = new System.Drawing.Font("Arial", 8F);
            this.valAlignB.Text = "0.0"; this.valAlignB.Location = new System.Drawing.Point(90, 52); this.valAlignB.Size = new System.Drawing.Size(180, 20); this.valAlignB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valAlignB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valAlignB.BackColor = System.Drawing.Color.White;
            this.lblRpmB.Text = "Spin RPM"; this.lblRpmB.Location = new System.Drawing.Point(10, 85); this.lblRpmB.AutoSize = true; this.lblRpmB.Font = new System.Drawing.Font("Arial", 8F);
            this.valRpmB.Text = "3000"; this.valRpmB.Location = new System.Drawing.Point(90, 82); this.valRpmB.Size = new System.Drawing.Size(40, 20); this.valRpmB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valRpmB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valRpmB.BackColor = System.Drawing.Color.White;
            this.lblTimeB.Text = "Step Time"; this.lblTimeB.Location = new System.Drawing.Point(145, 85); this.lblTimeB.AutoSize = true; this.lblTimeB.Font = new System.Drawing.Font("Arial", 8F);
            this.valTimeB.Text = "00 / 00"; this.valTimeB.Location = new System.Drawing.Point(210, 82); this.valTimeB.Size = new System.Drawing.Size(60, 20); this.valTimeB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valTimeB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valTimeB.BackColor = System.Drawing.Color.White;
            this.lblProgB.Text = "Progress"; this.lblProgB.Location = new System.Drawing.Point(10, 115); this.lblProgB.AutoSize = true;
            this.progB.Value = 0; this.progB.Location = new System.Drawing.Point(90, 115); this.progB.Size = new System.Drawing.Size(180, 15);
            this.btnStopB.Text = "STOP"; this.btnStopB.Location = new System.Drawing.Point(10, 140); this.btnStopB.Size = new System.Drawing.Size(80, 30); this.btnStopB.BackColor = System.Drawing.Color.White;
            this.btnStartB.Text = "START"; this.btnStartB.Location = new System.Drawing.Point(100, 140); this.btnStartB.Size = new System.Drawing.Size(80, 30); this.btnStartB.BackColor = System.Drawing.Color.White;
            this.grpPmB.Controls.Add(this.lblStepB); this.grpPmB.Controls.Add(this.txtProcessB);
            this.grpPmB.Controls.Add(this.lblAlignB); this.grpPmB.Controls.Add(this.valAlignB);
            this.grpPmB.Controls.Add(this.lblRpmB); this.grpPmB.Controls.Add(this.valRpmB);
            this.grpPmB.Controls.Add(this.lblTimeB); this.grpPmB.Controls.Add(this.valTimeB);
            this.grpPmB.Controls.Add(this.lblProgB); this.grpPmB.Controls.Add(this.progB);
            this.grpPmB.Controls.Add(this.btnStopB); this.grpPmB.Controls.Add(this.btnStartB);
            this.pnlLeft.Controls.Add(this.grpPmB);

            // PM C
            this.grpPmC.Text = "PM C"; this.grpPmC.Location = new System.Drawing.Point(10, 390); this.grpPmC.Size = new System.Drawing.Size(280, 180); this.grpPmC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStepC.Text = "Step Name"; this.lblStepC.Location = new System.Drawing.Point(10, 25); this.lblStepC.AutoSize = true;
            this.txtProcessC.Text = "Plasma Etch"; this.txtProcessC.Location = new System.Drawing.Point(90, 22); this.txtProcessC.Width = 180; this.txtProcessC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.lblPressC.Text = "Pressure"; this.lblPressC.Location = new System.Drawing.Point(10, 55); this.lblPressC.AutoSize = true; this.lblPressC.Font = new System.Drawing.Font("Arial", 8F);
            this.valPressC.Text = "15 mT"; this.valPressC.Location = new System.Drawing.Point(90, 52); this.valPressC.Size = new System.Drawing.Size(40, 20); this.valPressC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valPressC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valPressC.BackColor = System.Drawing.Color.White;
            this.lblGasC.Text = "Gas Flow"; this.lblGasC.Location = new System.Drawing.Point(145, 55); this.lblGasC.AutoSize = true; this.lblGasC.Font = new System.Drawing.Font("Arial", 8F);
            this.valGasC.Text = "100"; this.valGasC.Location = new System.Drawing.Point(210, 52); this.valGasC.Size = new System.Drawing.Size(40, 20); this.valGasC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valGasC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valGasC.BackColor = System.Drawing.Color.White;
            this.lblSpinTimeC.Text = "Spin Time"; this.lblSpinTimeC.Location = new System.Drawing.Point(10, 85); this.lblSpinTimeC.AutoSize = true; this.lblSpinTimeC.Font = new System.Drawing.Font("Arial", 8F);
            this.valSpinTimeC.Text = "00 / 00"; this.valSpinTimeC.Location = new System.Drawing.Point(90, 82); this.valSpinTimeC.Size = new System.Drawing.Size(180, 20); this.valSpinTimeC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.valSpinTimeC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; this.valSpinTimeC.BackColor = System.Drawing.Color.White;
            this.lblProgC.Text = "Progress"; this.lblProgC.Location = new System.Drawing.Point(10, 115); this.lblProgC.AutoSize = true;
            this.progC.Value = 0; this.progC.Location = new System.Drawing.Point(90, 115); this.progC.Size = new System.Drawing.Size(180, 15);
            this.btnStopC.Text = "STOP"; this.btnStopC.Location = new System.Drawing.Point(10, 140); this.btnStopC.Size = new System.Drawing.Size(80, 30); this.btnStopC.BackColor = System.Drawing.Color.White;
            this.btnStartC.Text = "START"; this.btnStartC.Location = new System.Drawing.Point(100, 140); this.btnStartC.Size = new System.Drawing.Size(80, 30); this.btnStartC.BackColor = System.Drawing.Color.White;
            this.grpPmC.Controls.Add(this.lblStepC); this.grpPmC.Controls.Add(this.txtProcessC);
            this.grpPmC.Controls.Add(this.lblPressC); this.grpPmC.Controls.Add(this.valPressC);
            this.grpPmC.Controls.Add(this.lblGasC); this.grpPmC.Controls.Add(this.valGasC);
            this.grpPmC.Controls.Add(this.lblSpinTimeC); this.grpPmC.Controls.Add(this.valSpinTimeC);
            this.grpPmC.Controls.Add(this.lblProgC); this.grpPmC.Controls.Add(this.progC);
            this.grpPmC.Controls.Add(this.btnStopC); this.grpPmC.Controls.Add(this.btnStartC);
            this.pnlLeft.Controls.Add(this.grpPmC);

            // [추가] RESET CHAMBERS 버튼 생성 및 배치
            this.btnResetChambers.Text = "RESET CHAMBERS";
            this.btnResetChambers.Location = new System.Drawing.Point(10, 580);
            this.btnResetChambers.Size = new System.Drawing.Size(280, 40);
            this.btnResetChambers.BackColor = System.Drawing.Color.LightCoral;
            this.btnResetChambers.ForeColor = System.Drawing.Color.White;
            this.btnResetChambers.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnResetChambers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnlLeft.Controls.Add(this.btnResetChambers);


            // Right Panel (기존과 동일)
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right; this.pnlRight.Width = 300; this.pnlRight.Padding = new System.Windows.Forms.Padding(10);
            this.grpFormA.Text = "Form A"; this.grpFormA.Location = new System.Drawing.Point(10, 10); this.grpFormA.Size = new System.Drawing.Size(280, 250); this.grpFormA.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblInfoA.Text = "LPB Status: Ready\nMode: MANUAL\nRun: Idle"; this.lblInfoA.Location = new System.Drawing.Point(10, 25); this.lblInfoA.AutoSize = true;
            this.lblCarrierA.Text = "Carrier ID"; this.lblCarrierA.Location = new System.Drawing.Point(10, 90);
            this.txtCarrierA.Text = "FOUP_LOT01"; this.txtCarrierA.Location = new System.Drawing.Point(80, 88); this.txtCarrierA.Width = 180;
            this.btnLoadA.Text = "LOAD FOUP"; this.btnLoadA.Location = new System.Drawing.Point(10, 150); this.btnLoadA.Size = new System.Drawing.Size(260, 40); this.btnLoadA.BackColor = System.Drawing.Color.FromArgb(0, 192, 0); this.btnLoadA.ForeColor = System.Drawing.Color.White; this.btnLoadA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadA.Text = "UNLOAD FOUP"; this.btnUnloadA.Location = new System.Drawing.Point(10, 200); this.btnUnloadA.Size = new System.Drawing.Size(260, 40); this.btnUnloadA.BackColor = System.Drawing.Color.Red; this.btnUnloadA.ForeColor = System.Drawing.Color.White; this.btnUnloadA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpFormA.Controls.Add(this.lblInfoA); this.grpFormA.Controls.Add(this.lblCarrierA); this.grpFormA.Controls.Add(this.txtCarrierA); this.grpFormA.Controls.Add(this.btnLoadA); this.grpFormA.Controls.Add(this.btnUnloadA);
            this.pnlRight.Controls.Add(this.grpFormA);
            this.grpFormB.Text = "Form B"; this.grpFormB.Location = new System.Drawing.Point(10, 270); this.grpFormB.Size = new System.Drawing.Size(280, 250); this.grpFormB.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblInfoB.Text = "LPB Status: Ready\nMode: MANUAL\nRun: Idle"; this.lblInfoB.Location = new System.Drawing.Point(10, 25); this.lblInfoB.AutoSize = true;
            this.lblCarrierB.Text = "Carrier ID"; this.lblCarrierB.Location = new System.Drawing.Point(10, 90);
            this.txtCarrierB.Text = "FOUP_LOT02"; this.txtCarrierB.Location = new System.Drawing.Point(80, 88); this.txtCarrierB.Width = 180;
            this.btnLoadB.Text = "LOAD FOUP"; this.btnLoadB.Location = new System.Drawing.Point(10, 150); this.btnLoadB.Size = new System.Drawing.Size(260, 40); this.btnLoadB.BackColor = System.Drawing.Color.FromArgb(0, 192, 0); this.btnLoadB.ForeColor = System.Drawing.Color.White; this.btnLoadB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadB.Text = "UNLOAD FOUP"; this.btnUnloadB.Location = new System.Drawing.Point(10, 200); this.btnUnloadB.Size = new System.Drawing.Size(260, 40); this.btnUnloadB.BackColor = System.Drawing.Color.Red; this.btnUnloadB.ForeColor = System.Drawing.Color.White; this.btnUnloadB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpFormB.Controls.Add(this.lblInfoB); this.grpFormB.Controls.Add(this.lblCarrierB); this.grpFormB.Controls.Add(this.txtCarrierB); this.grpFormB.Controls.Add(this.btnLoadB); this.grpFormB.Controls.Add(this.btnUnloadB);
            this.pnlRight.Controls.Add(this.grpFormB);

            // Bottom Panel
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom; this.pnlBottom.Height = 60; this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 버튼들을 중앙 정렬하기 위해 X 좌표 조정 (화면 너비 1920 기준)
            // 전체 버튼 그룹 너비: (80*4) + (10*3) = 350
            // 시작점: (1920 - 350) / 2 = 785

            this.btnMain.Text = "MAIN";
            this.btnMain.Location = new System.Drawing.Point(785, 5);
            this.btnMain.Size = new System.Drawing.Size(80, 50);
            this.btnMain.BackColor = System.Drawing.Color.White;

            this.btnRecipe.Text = "RECIPE";
            this.btnRecipe.Location = new System.Drawing.Point(875, 5); // 785 + 90
            this.btnRecipe.Size = new System.Drawing.Size(80, 50);
            this.btnRecipe.BackColor = System.Drawing.Color.White;

            this.btnLog.Text = "LOG";
            this.btnLog.Location = new System.Drawing.Point(965, 5); // 875 + 90
            this.btnLog.Size = new System.Drawing.Size(80, 50);
            this.btnLog.BackColor = System.Drawing.Color.White;

            // [추가] CONFIG 버튼
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnConfig.Text = "CONFIG";
            this.btnConfig.Location = new System.Drawing.Point(1055, 5); // 965 + 90
            this.btnConfig.Size = new System.Drawing.Size(80, 50);
            this.btnConfig.BackColor = System.Drawing.Color.White;

            // [수정] 모든 하단 버튼을 pnlBottom에 추가
            this.pnlBottom.Controls.Add(this.btnMain);
            this.pnlBottom.Controls.Add(this.btnRecipe);
            this.pnlBottom.Controls.Add(this.btnLog);
            this.pnlBottom.Controls.Add(this.btnConfig);

            // Center Panel
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;

            // Center Panel
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.BackColor = System.Drawing.Color.WhiteSmoke;

            this.lblNameA.AutoSize = true; this.lblNameA.BackColor = System.Drawing.Color.Transparent; this.lblNameA.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold); this.lblNameA.Name = "lblNameA"; this.lblNameA.Text = "PM A";
            this.lblNameB.AutoSize = true; this.lblNameB.BackColor = System.Drawing.Color.Transparent; this.lblNameB.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold); this.lblNameB.Name = "lblNameB"; this.lblNameB.Text = "PM B";
            this.lblNameC.AutoSize = true; this.lblNameC.BackColor = System.Drawing.Color.Transparent; this.lblNameC.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold); this.lblNameC.Name = "lblNameC"; this.lblNameC.Text = "PM C";
            this.pnlCenter.Controls.Add(this.lblNameA);
            this.pnlCenter.Controls.Add(this.lblNameB);
            this.pnlCenter.Controls.Add(this.lblNameC);

            this.pnlChamberB.BackColor = System.Drawing.Color.FromArgb(220, 220, 220); this.pnlChamberB.Location = new System.Drawing.Point(620, 150); this.pnlChamberB.Size = new System.Drawing.Size(80, 100); this.pnlCenter.Controls.Add(this.pnlChamberB);
            this.pnlChamberA.BackColor = System.Drawing.Color.FromArgb(220, 220, 220); this.pnlChamberA.Location = new System.Drawing.Point(350, 420); this.pnlChamberA.Size = new System.Drawing.Size(80, 100); this.pnlCenter.Controls.Add(this.pnlChamberA);
            this.pnlChamberC.BackColor = System.Drawing.Color.FromArgb(220, 220, 220); this.pnlChamberC.Location = new System.Drawing.Point(890, 420); this.pnlChamberC.Size = new System.Drawing.Size(80, 100); this.pnlCenter.Controls.Add(this.pnlChamberC);
            this.pnlFoupA.BackColor = System.Drawing.Color.Silver; this.pnlFoupA.Location = new System.Drawing.Point(450, 650); this.pnlFoupA.Size = new System.Drawing.Size(80, 80); this.pnlCenter.Controls.Add(this.pnlFoupA);
            this.pnlFoupB.BackColor = System.Drawing.Color.Silver; this.pnlFoupB.Location = new System.Drawing.Point(790, 650); this.pnlFoupB.Size = new System.Drawing.Size(80, 80); this.pnlCenter.Controls.Add(this.pnlFoupB);

            this.pnlCassetteL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.pnlCassetteL.Location = new System.Drawing.Point(560, 770); this.pnlCassetteL.Size = new System.Drawing.Size(80, 80); this.pnlCassetteL.BackColor = System.Drawing.Color.White;
            this.pnlWaferL1.BackColor = System.Drawing.Color.Blue; this.pnlWaferL1.Location = new System.Drawing.Point(5, 10); this.pnlWaferL1.Size = new System.Drawing.Size(70, 8); this.pnlCassetteL.Controls.Add(this.pnlWaferL1);
            this.pnlWaferL2.BackColor = System.Drawing.Color.Blue; this.pnlWaferL2.Location = new System.Drawing.Point(5, 24); this.pnlWaferL2.Size = new System.Drawing.Size(70, 8); this.pnlCassetteL.Controls.Add(this.pnlWaferL2);
            this.pnlWaferL3.BackColor = System.Drawing.Color.Blue; this.pnlWaferL3.Location = new System.Drawing.Point(5, 38); this.pnlWaferL3.Size = new System.Drawing.Size(70, 8); this.pnlCassetteL.Controls.Add(this.pnlWaferL3);
            this.pnlWaferL4.BackColor = System.Drawing.Color.Blue; this.pnlWaferL4.Location = new System.Drawing.Point(5, 52); this.pnlWaferL4.Size = new System.Drawing.Size(70, 8); this.pnlCassetteL.Controls.Add(this.pnlWaferL4);
            this.pnlWaferL5.BackColor = System.Drawing.Color.Blue; this.pnlWaferL5.Location = new System.Drawing.Point(5, 66); this.pnlWaferL5.Size = new System.Drawing.Size(70, 8); this.pnlCassetteL.Controls.Add(this.pnlWaferL5);
            this.pnlCenter.Controls.Add(this.pnlCassetteL);

            this.pnlCassetteR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.pnlCassetteR.Location = new System.Drawing.Point(680, 770); this.pnlCassetteR.Size = new System.Drawing.Size(80, 80); this.pnlCassetteR.BackColor = System.Drawing.Color.White;
            this.pnlWaferR1.BackColor = System.Drawing.Color.Black; this.pnlWaferR1.Location = new System.Drawing.Point(5, 10); this.pnlWaferR1.Size = new System.Drawing.Size(70, 8); this.pnlCassetteR.Controls.Add(this.pnlWaferR1);
            this.pnlWaferR2.BackColor = System.Drawing.Color.Black; this.pnlWaferR2.Location = new System.Drawing.Point(5, 24); this.pnlWaferR2.Size = new System.Drawing.Size(70, 8); this.pnlCassetteR.Controls.Add(this.pnlWaferR2);
            this.pnlWaferR3.BackColor = System.Drawing.Color.Black; this.pnlWaferR3.Location = new System.Drawing.Point(5, 38); this.pnlWaferR3.Size = new System.Drawing.Size(70, 8); this.pnlCassetteR.Controls.Add(this.pnlWaferR3);
            this.pnlWaferR4.BackColor = System.Drawing.Color.Black; this.pnlWaferR4.Location = new System.Drawing.Point(5, 52); this.pnlWaferR4.Size = new System.Drawing.Size(70, 8); this.pnlCassetteR.Controls.Add(this.pnlWaferR4);
            this.pnlWaferR5.BackColor = System.Drawing.Color.Black; this.pnlWaferR5.Location = new System.Drawing.Point(5, 66); this.pnlWaferR5.Size = new System.Drawing.Size(70, 8); this.pnlCassetteR.Controls.Add(this.pnlWaferR5);
            this.pnlCenter.Controls.Add(this.pnlCassetteR);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            this.pnlHost.ResumeLayout(false);
            this.pnlTime.ResumeLayout(false);
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
            this.pnlBottom.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.pnlCenter.PerformLayout();
            this.pnlCassetteL.ResumeLayout(false);
            this.pnlCassetteR.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // ... (기존 패널/컨트롤 선언 생략) ...
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
        private System.Windows.Forms.Label lblTargetA;
        private System.Windows.Forms.Label valTargetA;
        private System.Windows.Forms.Label lblGasA;
        private System.Windows.Forms.Label valGasA;
        private System.Windows.Forms.Label lblCurrA;
        private System.Windows.Forms.Label valCurrA;
        private System.Windows.Forms.Label lblTimeA;
        private System.Windows.Forms.Label valTimeA;
        private System.Windows.Forms.Label lblProgA;
        private System.Windows.Forms.ProgressBar progA;
        private System.Windows.Forms.Button btnStopA;
        private System.Windows.Forms.Button btnStartA;

        private System.Windows.Forms.GroupBox grpPmB;
        private System.Windows.Forms.Label lblStepB;
        private System.Windows.Forms.TextBox txtProcessB;
        private System.Windows.Forms.Label lblAlignB;
        private System.Windows.Forms.Label valAlignB;
        private System.Windows.Forms.Label lblRpmB;
        private System.Windows.Forms.Label valRpmB;
        private System.Windows.Forms.Label lblTimeB;
        private System.Windows.Forms.Label valTimeB;
        private System.Windows.Forms.Label lblProgB;
        private System.Windows.Forms.ProgressBar progB;
        private System.Windows.Forms.Button btnStopB;
        private System.Windows.Forms.Button btnStartB;

        private System.Windows.Forms.GroupBox grpPmC;
        private System.Windows.Forms.Label lblStepC;
        private System.Windows.Forms.TextBox txtProcessC;
        private System.Windows.Forms.Label lblPressC;
        private System.Windows.Forms.Label valPressC;
        private System.Windows.Forms.Label lblGasC;
        private System.Windows.Forms.Label valGasC;
        private System.Windows.Forms.Label lblSpinTimeC;
        private System.Windows.Forms.Label valSpinTimeC;
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
        private System.Windows.Forms.Button btnRecipe;
        private System.Windows.Forms.Button btnLog;

        private System.Windows.Forms.Panel pnlChamberA;
        private System.Windows.Forms.Panel pnlChamberB;
        private System.Windows.Forms.Panel pnlChamberC;
        private System.Windows.Forms.Panel pnlFoupA;
        private System.Windows.Forms.Panel pnlFoupB;
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

        private System.Windows.Forms.Label lblNameA;
        private System.Windows.Forms.Label lblNameB;
        private System.Windows.Forms.Label lblNameC;

        private System.Windows.Forms.Panel pnlAlarm;
        private System.Windows.Forms.Label lblAlarmMsg;

        private System.Windows.Forms.Button btnResetChambers;

        private System.Windows.Forms.Button btnConfig;
    }
}