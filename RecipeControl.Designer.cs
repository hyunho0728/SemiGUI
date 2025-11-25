namespace SemiGUI
{
    partial class RecipeControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.lblTitleHeader = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.grpList = new System.Windows.Forms.GroupBox();
            this.lstRecipes = new System.Windows.Forms.ListBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();

            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpTransfer = new System.Windows.Forms.GroupBox();
            this.lblCurrVer = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.grpHost = new System.Windows.Forms.GroupBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.lblHostInfo = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();

            this.pnlCenter = new System.Windows.Forms.Panel();

            // [수정] 그리드 그룹박스 제거됨

            // PM A Params
            this.grpParamA = new System.Windows.Forms.GroupBox();
            this.lblTargetA = new System.Windows.Forms.Label(); this.txtTargetA = new System.Windows.Forms.TextBox();
            this.lblGasA = new System.Windows.Forms.Label(); this.txtGasA = new System.Windows.Forms.TextBox();
            this.lblTimeA = new System.Windows.Forms.Label(); this.txtTimeA = new System.Windows.Forms.TextBox();

            // PM B Params
            this.grpParamB = new System.Windows.Forms.GroupBox();
            this.lblAlignB = new System.Windows.Forms.Label(); this.txtAlignB = new System.Windows.Forms.TextBox();
            this.lblRpmB = new System.Windows.Forms.Label(); this.txtRpmB = new System.Windows.Forms.TextBox();
            this.lblTimeB = new System.Windows.Forms.Label(); this.txtTimeB = new System.Windows.Forms.TextBox();

            // PM C Params
            this.grpParamC = new System.Windows.Forms.GroupBox();
            this.lblPressC = new System.Windows.Forms.Label(); this.txtPressC = new System.Windows.Forms.TextBox();
            this.lblGasC = new System.Windows.Forms.Label(); this.txtGasC = new System.Windows.Forms.TextBox();
            this.lblSpinC = new System.Windows.Forms.Label(); this.txtSpinC = new System.Windows.Forms.TextBox();

            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlLeft.SuspendLayout();
            this.grpList.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.grpTransfer.SuspendLayout();
            this.grpHost.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.grpParamA.SuspendLayout();
            this.grpParamB.SuspendLayout();
            this.grpParamC.SuspendLayout();
            this.SuspendLayout();

            // 
            // RecipeControl (Size 1280x720으로 축소)
            // 
            this.BackColor = System.Drawing.Color.FromArgb(232, 234, 237);
            this.Size = new System.Drawing.Size(1280, 720);

            // Header
            this.lblTitleHeader.Text = "UTILITY > Recipe Management";
            this.lblTitleHeader.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitleHeader.Location = new System.Drawing.Point(20, 15);
            this.lblTitleHeader.AutoSize = true;

            // 
            // Left Panel (폭 줄임: 350 -> 240)
            // 
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Width = 240;
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(15, 60, 5, 15);

            this.grpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpList.Text = "Recipe List";
            this.grpList.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpList.BackColor = System.Drawing.Color.White;

            this.lstRecipes.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstRecipes.Height = 500;
            this.lstRecipes.Font = new System.Drawing.Font("Arial", 9F);
            this.lstRecipes.ItemHeight = 18;

            this.txtSearch.Location = new System.Drawing.Point(10, 540);
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.Text = "Search...";

            this.btnNew.Text = "New";
            this.btnNew.Location = new System.Drawing.Point(10, 580);
            this.btnNew.Size = new System.Drawing.Size(95, 35);
            this.btnNew.BackColor = System.Drawing.Color.WhiteSmoke;

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new System.Drawing.Point(115, 580);
            this.btnDelete.Size = new System.Drawing.Size(95, 35);
            this.btnDelete.BackColor = System.Drawing.Color.WhiteSmoke;

            this.grpList.Controls.Add(this.lstRecipes);
            this.grpList.Controls.Add(this.txtSearch);
            this.grpList.Controls.Add(this.btnNew);
            this.grpList.Controls.Add(this.btnDelete);
            this.pnlLeft.Controls.Add(this.grpList);

            // 
            // Right Panel (폭 줄임: 350 -> 260)
            // 
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Width = 260;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(5, 60, 15, 15);

            // Transfer Group
            this.grpTransfer.Text = "Control & Apply";
            this.grpTransfer.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpTransfer.BackColor = System.Drawing.Color.White;
            this.grpTransfer.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTransfer.Height = 200;

            this.lblCurrVer.Text = "Last Applied: None";
            this.lblCurrVer.Location = new System.Drawing.Point(15, 40);
            this.lblCurrVer.AutoSize = true;
            this.lblCurrVer.Font = new System.Drawing.Font("Arial", 9F);

            this.btnApply.Text = "Apply to ALL";
            this.btnApply.Location = new System.Drawing.Point(15, 80);
            this.btnApply.Size = new System.Drawing.Size(210, 50);
            this.btnApply.BackColor = System.Drawing.Color.CadetBlue;
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);

            this.grpTransfer.Controls.Add(this.lblCurrVer);
            this.grpTransfer.Controls.Add(this.btnApply);

            // Host Group
            this.grpHost.Text = "Host System";
            this.grpHost.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpHost.BackColor = System.Drawing.Color.White;
            this.grpHost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpHost.Height = 400;

            this.btnSync.Text = "Sync";
            this.btnSync.Location = new System.Drawing.Point(15, 30);
            this.btnSync.Size = new System.Drawing.Size(210, 40);
            this.btnSync.BackColor = System.Drawing.Color.LightGreen;
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.lblHostInfo.Text = "Host Connected";
            this.lblHostInfo.Location = new System.Drawing.Point(15, 90);
            this.lblHostInfo.AutoSize = true;
            this.lblHostInfo.Font = new System.Drawing.Font("Arial", 9F);

            this.txtLog.Location = new System.Drawing.Point(15, 120);
            this.txtLog.Multiline = true;
            this.txtLog.Size = new System.Drawing.Size(210, 260);
            this.txtLog.Text = "Ready...";
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8F);
            this.txtLog.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            this.grpHost.Controls.Add(this.btnSync);
            this.grpHost.Controls.Add(this.lblHostInfo);
            this.grpHost.Controls.Add(this.txtLog);

            this.pnlRight.Controls.Add(this.grpHost);
            System.Windows.Forms.Panel pnlSpacer = new System.Windows.Forms.Panel();
            pnlSpacer.Dock = System.Windows.Forms.DockStyle.Top; pnlSpacer.Height = 10;
            this.pnlRight.Controls.Add(pnlSpacer);
            this.pnlRight.Controls.Add(this.grpTransfer);

            // 
            // Center Panel (배치 변경: 그리드 삭제 -> 파라미터 그룹 상단 배치)
            // 
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(10, 60, 10, 20);

            // 그룹박스 배치 계산
            // Center Width = 1280 - 240 - 260 = 780
            // 각 그룹박스 폭 = 240 정도
            int groupW = 245;
            int groupH = 300;
            int startY = 60;
            int gap = 10;

            // PM A Group (Left)
            this.grpParamA.Text = "PM A (Oxidation)";
            this.grpParamA.Location = new System.Drawing.Point(10, startY);
            this.grpParamA.Size = new System.Drawing.Size(groupW, groupH);
            this.grpParamA.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParamA.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);

            this.lblTargetA.Text = "Target Temp (°C)"; this.lblTargetA.Location = new System.Drawing.Point(15, 40); this.lblTargetA.AutoSize = true;
            this.txtTargetA.Location = new System.Drawing.Point(15, 65); this.txtTargetA.Size = new System.Drawing.Size(210, 23);

            this.lblGasA.Text = "Gas Flow (sccm)"; this.lblGasA.Location = new System.Drawing.Point(15, 110); this.lblGasA.AutoSize = true;
            this.txtGasA.Location = new System.Drawing.Point(15, 135); this.txtGasA.Size = new System.Drawing.Size(210, 23);

            this.lblTimeA.Text = "Process Time (s)"; this.lblTimeA.Location = new System.Drawing.Point(15, 180); this.lblTimeA.AutoSize = true;
            this.txtTimeA.Location = new System.Drawing.Point(15, 205); this.txtTimeA.Size = new System.Drawing.Size(210, 23);

            this.grpParamA.Controls.Add(this.lblTargetA); this.grpParamA.Controls.Add(this.txtTargetA);
            this.grpParamA.Controls.Add(this.lblGasA); this.grpParamA.Controls.Add(this.txtGasA);
            this.grpParamA.Controls.Add(this.lblTimeA); this.grpParamA.Controls.Add(this.txtTimeA);
            this.pnlCenter.Controls.Add(this.grpParamA);

            // PM B Group (Center)
            this.grpParamB.Text = "PM B (Photo)";
            this.grpParamB.Location = new System.Drawing.Point(10 + groupW + gap, startY);
            this.grpParamB.Size = new System.Drawing.Size(groupW, groupH);
            this.grpParamB.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParamB.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);

            this.lblAlignB.Text = "Align Accu (um)"; this.lblAlignB.Location = new System.Drawing.Point(15, 40); this.lblAlignB.AutoSize = true;
            this.txtAlignB.Location = new System.Drawing.Point(15, 65); this.txtAlignB.Size = new System.Drawing.Size(210, 23);

            this.lblRpmB.Text = "Spin RPM"; this.lblRpmB.Location = new System.Drawing.Point(15, 110); this.lblRpmB.AutoSize = true;
            this.txtRpmB.Location = new System.Drawing.Point(15, 135); this.txtRpmB.Size = new System.Drawing.Size(210, 23);

            this.lblTimeB.Text = "Step Time (s)"; this.lblTimeB.Location = new System.Drawing.Point(15, 180); this.lblTimeB.AutoSize = true;
            this.txtTimeB.Location = new System.Drawing.Point(15, 205); this.txtTimeB.Size = new System.Drawing.Size(210, 23);

            this.grpParamB.Controls.Add(this.lblAlignB); this.grpParamB.Controls.Add(this.txtAlignB);
            this.grpParamB.Controls.Add(this.lblRpmB); this.grpParamB.Controls.Add(this.txtRpmB);
            this.grpParamB.Controls.Add(this.lblTimeB); this.grpParamB.Controls.Add(this.txtTimeB);
            this.pnlCenter.Controls.Add(this.grpParamB);

            // PM C Group (Right)
            this.grpParamC.Text = "PM C (Etch)";
            this.grpParamC.Location = new System.Drawing.Point(10 + (groupW + gap) * 2, startY);
            this.grpParamC.Size = new System.Drawing.Size(groupW, groupH);
            this.grpParamC.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParamC.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);

            this.lblPressC.Text = "Pressure (mT)"; this.lblPressC.Location = new System.Drawing.Point(15, 40); this.lblPressC.AutoSize = true;
            this.txtPressC.Location = new System.Drawing.Point(15, 65); this.txtPressC.Size = new System.Drawing.Size(210, 23);

            this.lblGasC.Text = "Gas Flow (sccm)"; this.lblGasC.Location = new System.Drawing.Point(15, 110); this.lblGasC.AutoSize = true;
            this.txtGasC.Location = new System.Drawing.Point(15, 135); this.txtGasC.Size = new System.Drawing.Size(210, 23);

            this.lblSpinC.Text = "Spin Time (s)"; this.lblSpinC.Location = new System.Drawing.Point(15, 180); this.lblSpinC.AutoSize = true;
            this.txtSpinC.Location = new System.Drawing.Point(15, 205); this.txtSpinC.Size = new System.Drawing.Size(210, 23);

            this.grpParamC.Controls.Add(this.lblPressC); this.grpParamC.Controls.Add(this.txtPressC);
            this.grpParamC.Controls.Add(this.lblGasC); this.grpParamC.Controls.Add(this.txtGasC);
            this.grpParamC.Controls.Add(this.lblSpinC); this.grpParamC.Controls.Add(this.txtSpinC);
            this.pnlCenter.Controls.Add(this.grpParamC);

            // Bottom Buttons
            int btnY = 400;

            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(10, btnY);
            this.btnSave.Size = new System.Drawing.Size(120, 45);
            this.btnSave.BackColor = System.Drawing.Color.LightGray;
            this.btnSave.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);

            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.Location = new System.Drawing.Point(140, btnY);
            this.btnSaveAs.Size = new System.Drawing.Size(120, 45);
            this.btnSaveAs.BackColor = System.Drawing.Color.LightGray;
            this.btnSaveAs.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);

            this.btnValidate.Text = "Validate";
            this.btnValidate.Location = new System.Drawing.Point(270, btnY);
            this.btnValidate.Size = new System.Drawing.Size(120, 45);
            this.btnValidate.BackColor = System.Drawing.Color.CadetBlue;
            this.btnValidate.ForeColor = System.Drawing.Color.White;
            this.btnValidate.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);

            this.btnCancel.Text = "Close";
            this.btnCancel.Location = new System.Drawing.Point(635, btnY); // 우측 끝 정렬
            this.btnCancel.Size = new System.Drawing.Size(120, 45);
            this.btnCancel.BackColor = System.Drawing.Color.IndianRed;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);

            this.pnlCenter.Controls.Add(this.btnSave);
            this.pnlCenter.Controls.Add(this.btnSaveAs);
            this.pnlCenter.Controls.Add(this.btnValidate);
            this.pnlCenter.Controls.Add(this.btnCancel);

            this.Controls.Add(this.lblTitleHeader);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);

            this.pnlLeft.ResumeLayout(false);
            this.grpList.ResumeLayout(false);
            this.grpList.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.grpTransfer.ResumeLayout(false);
            this.grpTransfer.PerformLayout();
            this.grpHost.ResumeLayout(false);
            this.grpHost.PerformLayout();
            this.pnlCenter.ResumeLayout(false);
            this.grpParamA.ResumeLayout(false);
            this.grpParamA.PerformLayout();
            this.grpParamB.ResumeLayout(false);
            this.grpParamB.PerformLayout();
            this.grpParamC.ResumeLayout(false);
            this.grpParamC.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        public System.Windows.Forms.Label lblTitleHeader;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.GroupBox grpList;
        public System.Windows.Forms.ListBox lstRecipes;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.GroupBox grpTransfer;
        private System.Windows.Forms.Label lblCurrVer;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox grpHost;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Label lblHostInfo;
        private System.Windows.Forms.TextBox txtLog;

        private System.Windows.Forms.Panel pnlCenter;

        // PM A Params
        private System.Windows.Forms.GroupBox grpParamA;
        private System.Windows.Forms.Label lblTargetA; private System.Windows.Forms.TextBox txtTargetA;
        private System.Windows.Forms.Label lblGasA; private System.Windows.Forms.TextBox txtGasA;
        private System.Windows.Forms.Label lblTimeA; private System.Windows.Forms.TextBox txtTimeA;

        // PM B Params
        private System.Windows.Forms.GroupBox grpParamB;
        private System.Windows.Forms.Label lblAlignB; private System.Windows.Forms.TextBox txtAlignB;
        private System.Windows.Forms.Label lblRpmB; private System.Windows.Forms.TextBox txtRpmB;
        private System.Windows.Forms.Label lblTimeB; private System.Windows.Forms.TextBox txtTimeB;

        // PM C Params
        private System.Windows.Forms.GroupBox grpParamC;
        private System.Windows.Forms.Label lblPressC; private System.Windows.Forms.TextBox txtPressC;
        private System.Windows.Forms.Label lblGasC; private System.Windows.Forms.TextBox txtGasC;
        private System.Windows.Forms.Label lblSpinC; private System.Windows.Forms.TextBox txtSpinC;

        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnSaveAs;
        public System.Windows.Forms.Button btnValidate;
        public System.Windows.Forms.Button btnCancel;
    }
}