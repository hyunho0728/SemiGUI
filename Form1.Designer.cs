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
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlCenter = new System.Windows.Forms.Panel();

            // 상단 컨트롤들
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

            // 좌측 PM A
            this.grpPmA = new System.Windows.Forms.GroupBox();
            this.lblStepA = new System.Windows.Forms.Label();
            this.txtProcessA = new System.Windows.Forms.TextBox();
            this.lblPressA = new System.Windows.Forms.Label();
            this.lblPressValA = new System.Windows.Forms.Label();
            this.lblProgA = new System.Windows.Forms.Label();
            this.progA = new System.Windows.Forms.ProgressBar();
            this.btnStopA = new System.Windows.Forms.Button();
            this.btnStartA = new System.Windows.Forms.Button();

            // 좌측 PM B
            this.grpPmB = new System.Windows.Forms.GroupBox();
            this.lblStepB = new System.Windows.Forms.Label();
            this.txtProcessB = new System.Windows.Forms.TextBox();
            this.lblPressB = new System.Windows.Forms.Label();
            this.lblPressValB = new System.Windows.Forms.Label();
            this.lblProgB = new System.Windows.Forms.Label();
            this.progB = new System.Windows.Forms.ProgressBar();
            this.btnStopB = new System.Windows.Forms.Button();
            this.btnStartB = new System.Windows.Forms.Button();

            // 좌측 PM C
            this.grpPmC = new System.Windows.Forms.GroupBox();
            this.lblStepC = new System.Windows.Forms.Label();
            this.txtProcessC = new System.Windows.Forms.TextBox();
            this.lblPressC = new System.Windows.Forms.Label();
            this.lblPressValC = new System.Windows.Forms.Label();
            this.lblProgC = new System.Windows.Forms.Label();
            this.progC = new System.Windows.Forms.ProgressBar();
            this.btnStopC = new System.Windows.Forms.Button();
            this.btnStartC = new System.Windows.Forms.Button();

            // 우측 Form A
            this.grpFormA = new System.Windows.Forms.GroupBox();
            this.lblInfoA = new System.Windows.Forms.Label();
            this.lblCarrierA = new System.Windows.Forms.Label();
            this.txtCarrierA = new System.Windows.Forms.TextBox();
            this.btnLoadA = new System.Windows.Forms.Button();
            this.btnUnloadA = new System.Windows.Forms.Button();

            // 우측 Form B
            this.grpFormB = new System.Windows.Forms.GroupBox();
            this.lblInfoB = new System.Windows.Forms.Label();
            this.lblCarrierB = new System.Windows.Forms.Label();
            this.txtCarrierB = new System.Windows.Forms.TextBox();
            this.btnLoadB = new System.Windows.Forms.Button();
            this.btnUnloadB = new System.Windows.Forms.Button();

            // 하단 버튼
            this.btnMain = new System.Windows.Forms.Button();
            this.btnUtility = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();

            this.pnlTop.SuspendLayout();
            this.grpLogin.SuspendLayout();
            this.pnlHost.SuspendLayout();
            this.pnlTime.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.grpPmA.SuspendLayout();
            this.grpPmB.SuspendLayout();
            this.grpPmC.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.grpFormA.SuspendLayout();
            this.grpFormB.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();

            // Form1
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "Form1";
            this.Text = "Equipment Control System";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            // --- Top Panel ---
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 80;
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Login
            this.grpLogin.Text = "LOGIN";
            this.grpLogin.Location = new System.Drawing.Point(10, 5);
            this.grpLogin.Size = new System.Drawing.Size(180, 65);
            this.txtId.Text = "ID"; this.txtId.Location = new System.Drawing.Point(10, 20); this.txtId.Width = 80;
            this.txtPw.Text = "****"; this.txtPw.Location = new System.Drawing.Point(10, 42); this.txtPw.Width = 80; this.txtPw.PasswordChar = '*';
            this.btnLogin.Text = "LOGIN"; this.btnLogin.Location = new System.Drawing.Point(100, 20); this.btnLogin.Size = new System.Drawing.Size(70, 40); this.btnLogin.BackColor = System.Drawing.Color.White;
            this.grpLogin.Controls.Add(this.txtId); this.grpLogin.Controls.Add(this.txtPw); this.grpLogin.Controls.Add(this.btnLogin);
            this.pnlTop.Controls.Add(this.grpLogin);

            // LEDs
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

            // Host
            this.pnlHost.Location = new System.Drawing.Point(500, 10); this.pnlHost.Size = new System.Drawing.Size(150, 60); this.pnlHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHostTitle.Text = "HOST"; this.lblHostTitle.Location = new System.Drawing.Point(10, 10); this.lblHostTitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblHostState.Text = "NONE"; this.lblHostState.Location = new System.Drawing.Point(10, 35);
            this.pnlHost.Controls.Add(this.lblHostTitle); this.pnlHost.Controls.Add(this.lblHostState);
            this.pnlTop.Controls.Add(this.pnlHost);

            this.btnConnect.Text = "CONNECT"; this.btnConnect.Location = new System.Drawing.Point(660, 10); this.btnConnect.Size = new System.Drawing.Size(80, 60); this.btnConnect.BackColor = System.Drawing.Color.Khaki;
            this.pnlTop.Controls.Add(this.btnConnect);

            // Time
            this.pnlTime.Dock = System.Windows.Forms.DockStyle.Right; this.pnlTime.Width = 150; this.pnlTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Text = "2025/11/10"; this.lblDate.Location = new System.Drawing.Point(10, 10); this.lblDate.Font = new System.Drawing.Font("Arial", 12);
            this.lblTime.Text = "11:11:11"; this.lblTime.Location = new System.Drawing.Point(10, 35); this.lblTime.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            this.pnlTime.Controls.Add(this.lblDate); this.pnlTime.Controls.Add(this.lblTime);
            this.pnlTop.Controls.Add(this.pnlTime);


            // --- Left Panel (PM A, B, C) ---
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left; this.pnlLeft.Width = 300; this.pnlLeft.Padding = new System.Windows.Forms.Padding(10);

            // PM A
            this.grpPmA.Text = "PM A"; this.grpPmA.Location = new System.Drawing.Point(10, 10); this.grpPmA.Size = new System.Drawing.Size(280, 180); this.grpPmA.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblStepA.Text = "Step Name"; this.lblStepA.Location = new System.Drawing.Point(10, 25);
            // [수정] 폰트를 Regular로 변경하고 X위치를 110으로 조정하여 잘림 방지
            this.txtProcessA.Text = "Process";
            this.txtProcessA.Location = new System.Drawing.Point(110, 22);
            this.txtProcessA.Width = 140;
            this.txtProcessA.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);

            this.lblPressA.Text = "Pressure"; this.lblPressA.Location = new System.Drawing.Point(10, 55);
            this.lblPressValA.Text = "0.0 / 0.0"; this.lblPressValA.Location = new System.Drawing.Point(110, 52); this.lblPressValA.Size = new System.Drawing.Size(140, 20); this.lblPressValA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.lblPressValA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgA.Text = "Progress"; this.lblProgA.Location = new System.Drawing.Point(10, 85);
            this.progA.Value = 45; this.progA.Location = new System.Drawing.Point(110, 85); this.progA.Size = new System.Drawing.Size(140, 20);
            this.btnStopA.Text = "STOP"; this.btnStopA.Location = new System.Drawing.Point(10, 140); this.btnStopA.Size = new System.Drawing.Size(80, 30); this.btnStopA.BackColor = System.Drawing.Color.White;
            this.btnStartA.Text = "START"; this.btnStartA.Location = new System.Drawing.Point(100, 140); this.btnStartA.Size = new System.Drawing.Size(80, 30); this.btnStartA.BackColor = System.Drawing.Color.White;
            this.grpPmA.Controls.Add(this.lblStepA); this.grpPmA.Controls.Add(this.txtProcessA); this.grpPmA.Controls.Add(this.lblPressA); this.grpPmA.Controls.Add(this.lblPressValA); this.grpPmA.Controls.Add(this.lblProgA); this.grpPmA.Controls.Add(this.progA); this.grpPmA.Controls.Add(this.btnStopA); this.grpPmA.Controls.Add(this.btnStartA);
            this.pnlLeft.Controls.Add(this.grpPmA);

            // PM B
            this.grpPmB.Text = "PM B"; this.grpPmB.Location = new System.Drawing.Point(10, 200); this.grpPmB.Size = new System.Drawing.Size(280, 180); this.grpPmB.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblStepB.Text = "Step Name"; this.lblStepB.Location = new System.Drawing.Point(10, 25);
            // [수정] 폰트를 Regular로 변경
            this.txtProcessB.Text = "Process";
            this.txtProcessB.Location = new System.Drawing.Point(110, 22);
            this.txtProcessB.Width = 140;
            this.txtProcessB.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);

            this.lblPressB.Text = "Pressure"; this.lblPressB.Location = new System.Drawing.Point(10, 55);
            this.lblPressValB.Text = "0.0 / 0.0"; this.lblPressValB.Location = new System.Drawing.Point(110, 52); this.lblPressValB.Size = new System.Drawing.Size(140, 20); this.lblPressValB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.lblPressValB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgB.Text = "Progress"; this.lblProgB.Location = new System.Drawing.Point(10, 85);
            this.progB.Value = 10; this.progB.Location = new System.Drawing.Point(110, 85); this.progB.Size = new System.Drawing.Size(140, 20);
            this.btnStopB.Text = "STOP"; this.btnStopB.Location = new System.Drawing.Point(10, 140); this.btnStopB.Size = new System.Drawing.Size(80, 30); this.btnStopB.BackColor = System.Drawing.Color.White;
            this.btnStartB.Text = "START"; this.btnStartB.Location = new System.Drawing.Point(100, 140); this.btnStartB.Size = new System.Drawing.Size(80, 30); this.btnStartB.BackColor = System.Drawing.Color.White;
            this.grpPmB.Controls.Add(this.lblStepB); this.grpPmB.Controls.Add(this.txtProcessB); this.grpPmB.Controls.Add(this.lblPressB); this.grpPmB.Controls.Add(this.lblPressValB); this.grpPmB.Controls.Add(this.lblProgB); this.grpPmB.Controls.Add(this.progB); this.grpPmB.Controls.Add(this.btnStopB); this.grpPmB.Controls.Add(this.btnStartB);
            this.pnlLeft.Controls.Add(this.grpPmB);

            // PM C
            this.grpPmC.Text = "PM C"; this.grpPmC.Location = new System.Drawing.Point(10, 390); this.grpPmC.Size = new System.Drawing.Size(280, 180); this.grpPmC.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblStepC.Text = "Step Name"; this.lblStepC.Location = new System.Drawing.Point(10, 25);
            // [수정] 폰트를 Regular로 변경
            this.txtProcessC.Text = "Process";
            this.txtProcessC.Location = new System.Drawing.Point(110, 22);
            this.txtProcessC.Width = 140;
            this.txtProcessC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);

            this.lblPressC.Text = "Pressure"; this.lblPressC.Location = new System.Drawing.Point(10, 55);
            this.lblPressValC.Text = "0.0 / 0.0"; this.lblPressValC.Location = new System.Drawing.Point(110, 52); this.lblPressValC.Size = new System.Drawing.Size(140, 20); this.lblPressValC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.lblPressValC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgC.Text = "Progress"; this.lblProgC.Location = new System.Drawing.Point(10, 85);
            this.progC.Value = 0; this.progC.Location = new System.Drawing.Point(110, 85); this.progC.Size = new System.Drawing.Size(140, 20);
            this.btnStopC.Text = "STOP"; this.btnStopC.Location = new System.Drawing.Point(10, 140); this.btnStopC.Size = new System.Drawing.Size(80, 30); this.btnStopC.BackColor = System.Drawing.Color.White;
            this.btnStartC.Text = "START"; this.btnStartC.Location = new System.Drawing.Point(100, 140); this.btnStartC.Size = new System.Drawing.Size(80, 30); this.btnStartC.BackColor = System.Drawing.Color.White;
            this.grpPmC.Controls.Add(this.lblStepC); this.grpPmC.Controls.Add(this.txtProcessC); this.grpPmC.Controls.Add(this.lblPressC); this.grpPmC.Controls.Add(this.lblPressValC); this.grpPmC.Controls.Add(this.lblProgC); this.grpPmC.Controls.Add(this.progC); this.grpPmC.Controls.Add(this.btnStopC); this.grpPmC.Controls.Add(this.btnStartC);
            this.pnlLeft.Controls.Add(this.grpPmC);


            // --- Right Panel (Form A, B) ---
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right; this.pnlRight.Width = 300; this.pnlRight.Padding = new System.Windows.Forms.Padding(10);

            // Form A
            this.grpFormA.Text = "Form A"; this.grpFormA.Location = new System.Drawing.Point(10, 10); this.grpFormA.Size = new System.Drawing.Size(280, 250); this.grpFormA.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblInfoA.Text = "LPB Status: Ready\nMode: MANUAL\nRun: Idle"; this.lblInfoA.Location = new System.Drawing.Point(10, 25); this.lblInfoA.AutoSize = true;
            this.lblCarrierA.Text = "Carrier ID"; this.lblCarrierA.Location = new System.Drawing.Point(10, 90);
            this.txtCarrierA.Text = "FOUP_LOT01"; this.txtCarrierA.Location = new System.Drawing.Point(80, 88); this.txtCarrierA.Width = 180;
            this.btnLoadA.Text = "LOAD FOUP"; this.btnLoadA.Location = new System.Drawing.Point(10, 150); this.btnLoadA.Size = new System.Drawing.Size(260, 40); this.btnLoadA.BackColor = System.Drawing.Color.FromArgb(0, 192, 0); this.btnLoadA.ForeColor = System.Drawing.Color.White; this.btnLoadA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadA.Text = "UNLOAD FOUP"; this.btnUnloadA.Location = new System.Drawing.Point(10, 200); this.btnUnloadA.Size = new System.Drawing.Size(260, 40); this.btnUnloadA.BackColor = System.Drawing.Color.Red; this.btnUnloadA.ForeColor = System.Drawing.Color.White; this.btnUnloadA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpFormA.Controls.Add(this.lblInfoA); this.grpFormA.Controls.Add(this.lblCarrierA); this.grpFormA.Controls.Add(this.txtCarrierA); this.grpFormA.Controls.Add(this.btnLoadA); this.grpFormA.Controls.Add(this.btnUnloadA);
            this.pnlRight.Controls.Add(this.grpFormA);

            // Form B
            this.grpFormB.Text = "Form B"; this.grpFormB.Location = new System.Drawing.Point(10, 270); this.grpFormB.Size = new System.Drawing.Size(280, 250); this.grpFormB.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblInfoB.Text = "LPB Status: Ready\nMode: MANUAL\nRun: Idle"; this.lblInfoB.Location = new System.Drawing.Point(10, 25); this.lblInfoB.AutoSize = true;
            this.lblCarrierB.Text = "Carrier ID"; this.lblCarrierB.Location = new System.Drawing.Point(10, 90);
            this.txtCarrierB.Text = "FOUP_LOT02"; this.txtCarrierB.Location = new System.Drawing.Point(80, 88); this.txtCarrierB.Width = 180;
            this.btnLoadB.Text = "LOAD FOUP"; this.btnLoadB.Location = new System.Drawing.Point(10, 150); this.btnLoadB.Size = new System.Drawing.Size(260, 40); this.btnLoadB.BackColor = System.Drawing.Color.FromArgb(0, 192, 0); this.btnLoadB.ForeColor = System.Drawing.Color.White; this.btnLoadB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadB.Text = "UNLOAD FOUP"; this.btnUnloadB.Location = new System.Drawing.Point(10, 200); this.btnUnloadB.Size = new System.Drawing.Size(260, 40); this.btnUnloadB.BackColor = System.Drawing.Color.Red; this.btnUnloadB.ForeColor = System.Drawing.Color.White; this.btnUnloadB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpFormB.Controls.Add(this.lblInfoB); this.grpFormB.Controls.Add(this.lblCarrierB); this.grpFormB.Controls.Add(this.txtCarrierB); this.grpFormB.Controls.Add(this.btnLoadB); this.grpFormB.Controls.Add(this.btnUnloadB);
            this.pnlRight.Controls.Add(this.grpFormB);


            // --- Bottom Panel ---
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom; this.pnlBottom.Height = 60; this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnMain.Text = "MAIN"; this.btnMain.Location = new System.Drawing.Point(500, 5); this.btnMain.Size = new System.Drawing.Size(80, 50); this.btnMain.BackColor = System.Drawing.Color.White;
            this.btnUtility.Text = "UTILITY"; this.btnUtility.Location = new System.Drawing.Point(590, 5); this.btnUtility.Size = new System.Drawing.Size(80, 50); this.btnUtility.BackColor = System.Drawing.Color.White;
            this.btnLog.Text = "LOG"; this.btnLog.Location = new System.Drawing.Point(680, 5); this.btnLog.Size = new System.Drawing.Size(80, 50); this.btnLog.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.btnMain); this.pnlBottom.Controls.Add(this.btnUtility); this.pnlBottom.Controls.Add(this.btnLog);


            // --- Center Panel ---
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.BackColor = System.Drawing.Color.WhiteSmoke;

            this.pnlTop.ResumeLayout(false);
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
            this.ResumeLayout(false);
        }

        #endregion

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

        // PM A
        private System.Windows.Forms.GroupBox grpPmA;
        private System.Windows.Forms.Label lblStepA;
        private System.Windows.Forms.TextBox txtProcessA;
        private System.Windows.Forms.Label lblPressA;
        private System.Windows.Forms.Label lblPressValA;
        private System.Windows.Forms.Label lblProgA;
        private System.Windows.Forms.ProgressBar progA;
        private System.Windows.Forms.Button btnStopA;
        private System.Windows.Forms.Button btnStartA;

        // PM B
        private System.Windows.Forms.GroupBox grpPmB;
        private System.Windows.Forms.Label lblStepB;
        private System.Windows.Forms.TextBox txtProcessB;
        private System.Windows.Forms.Label lblPressB;
        private System.Windows.Forms.Label lblPressValB;
        private System.Windows.Forms.Label lblProgB;
        private System.Windows.Forms.ProgressBar progB;
        private System.Windows.Forms.Button btnStopB;
        private System.Windows.Forms.Button btnStartB;

        // PM C
        private System.Windows.Forms.GroupBox grpPmC;
        private System.Windows.Forms.Label lblStepC;
        private System.Windows.Forms.TextBox txtProcessC;
        private System.Windows.Forms.Label lblPressC;
        private System.Windows.Forms.Label lblPressValC;
        private System.Windows.Forms.Label lblProgC;
        private System.Windows.Forms.ProgressBar progC;
        private System.Windows.Forms.Button btnStopC;
        private System.Windows.Forms.Button btnStartC;

        // Forms
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
    }
}