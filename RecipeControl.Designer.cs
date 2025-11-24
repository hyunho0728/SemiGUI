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
            System.Windows.Forms.DataGridViewCellStyle gridStyleHeader = new System.Windows.Forms.DataGridViewCellStyle();

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
            this.btnMaint = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();

            this.grpHost = new System.Windows.Forms.GroupBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.lblHostInfo = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();

            this.pnlCenter = new System.Windows.Forms.Panel();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.dgvSteps = new System.Windows.Forms.DataGridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlow = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grpParams = new System.Windows.Forms.GroupBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.lblRf = new System.Windows.Forms.Label();
            this.txtRf = new System.Windows.Forms.TextBox();
            this.lblPress = new System.Windows.Forms.Label();
            this.txtPress = new System.Windows.Forms.TextBox();
            this.lblGasA = new System.Windows.Forms.Label();
            this.txtGasA = new System.Windows.Forms.TextBox();
            this.lblGasB = new System.Windows.Forms.Label();
            this.txtGasB = new System.Windows.Forms.TextBox();

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
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).BeginInit();
            this.grpParams.SuspendLayout();
            this.SuspendLayout();

            // 
            // RecipeControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(232, 234, 237);
            this.Size = new System.Drawing.Size(1280, 720);

            // 
            // Header Title
            // 
            this.lblTitleHeader.Text = "UTILITY > Recipe Management";
            this.lblTitleHeader.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitleHeader.Location = new System.Drawing.Point(20, 15);
            this.lblTitleHeader.AutoSize = true;

            // ==========================================
            // Left Panel (Recipe List)
            // ==========================================
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Width = 300;
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(20, 60, 10, 20);

            this.grpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpList.Text = "Recipe List";
            this.grpList.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpList.BackColor = System.Drawing.Color.White;

            this.lstRecipes.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstRecipes.Height = 500;
            this.lstRecipes.Font = new System.Drawing.Font("Arial", 10F);
            this.lstRecipes.ItemHeight = 20;
            this.lstRecipes.Items.AddRange(new object[] {
                "Recipe_001_PM_A_V1.0", "Recipe_002_PM_B_V2.1", "Test_PM_C_Initial",
                "Recipe_003_Etch_Poly", "DeenecPM_C_Final", "Recipe_Clean_Daily"
            });

            this.txtSearch.Location = new System.Drawing.Point(15, 530);
            this.txtSearch.Size = new System.Drawing.Size(240, 23);
            this.txtSearch.Text = "Search...";
            this.txtSearch.Font = new System.Drawing.Font("Arial", 9F);

            this.btnNew.Text = "New";
            this.btnNew.Location = new System.Drawing.Point(15, 570);
            this.btnNew.Size = new System.Drawing.Size(110, 35);
            this.btnNew.BackColor = System.Drawing.Color.WhiteSmoke;

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new System.Drawing.Point(145, 570);
            this.btnDelete.Size = new System.Drawing.Size(110, 35);
            this.btnDelete.BackColor = System.Drawing.Color.WhiteSmoke;

            this.grpList.Controls.Add(this.lstRecipes);
            this.grpList.Controls.Add(this.txtSearch);
            this.grpList.Controls.Add(this.btnNew);
            this.grpList.Controls.Add(this.btnDelete);
            this.pnlLeft.Controls.Add(this.grpList);

            // ==========================================
            // Right Panel (Transfer & Host)
            // ==========================================
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Width = 320;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10, 60, 20, 20);

            // Group: Transfer
            this.grpTransfer.Text = "Transfer & Control";
            this.grpTransfer.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpTransfer.BackColor = System.Drawing.Color.White;
            this.grpTransfer.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTransfer.Height = 220;

            this.lblCurrVer.Text = "Current Version on Equipment:\nRecipe_001_PM_A_V1.0";
            this.lblCurrVer.Location = new System.Drawing.Point(15, 30);
            this.lblCurrVer.AutoSize = true;
            this.lblCurrVer.Font = new System.Drawing.Font("Arial", 9F);

            this.btnMaint.Text = "Maintenance";
            this.btnMaint.Location = new System.Drawing.Point(15, 80);
            this.btnMaint.Size = new System.Drawing.Size(260, 40);
            this.btnMaint.BackColor = System.Drawing.Color.Gainsboro;
            this.btnMaint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnUpload.Text = "Upload from Equipment";
            this.btnUpload.Location = new System.Drawing.Point(15, 130);
            this.btnUpload.Size = new System.Drawing.Size(260, 40);
            this.btnUpload.BackColor = System.Drawing.Color.Gainsboro;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.grpTransfer.Controls.Add(this.lblCurrVer);
            this.grpTransfer.Controls.Add(this.btnMaint);
            this.grpTransfer.Controls.Add(this.btnUpload);

            // Group: Host
            this.grpHost.Text = "Host System (MES)";
            this.grpHost.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpHost.BackColor = System.Drawing.Color.White;
            this.grpHost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpHost.Height = 350; // 나머지 공간

            this.btnSync.Text = "Sync with Host";
            this.btnSync.Location = new System.Drawing.Point(15, 30);
            this.btnSync.Size = new System.Drawing.Size(260, 40);
            this.btnSync.BackColor = System.Drawing.Color.LightGreen;
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.lblHostInfo.Text = "Host Version: V2.1 (2025-10-27)";
            this.lblHostInfo.Location = new System.Drawing.Point(15, 85);
            this.lblHostInfo.AutoSize = true;
            this.lblHostInfo.Font = new System.Drawing.Font("Arial", 8F);

            this.txtLog.Location = new System.Drawing.Point(15, 110);
            this.txtLog.Multiline = true;
            this.txtLog.Size = new System.Drawing.Size(260, 220);
            this.txtLog.Text = "[2025-11-24 12:00] Sync Started...\r\n[2025-11-24 12:01] Recipe_01 Updated.\r\n[2025-11-24 12:02] Completed.";
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8F);
            this.txtLog.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            this.grpHost.Controls.Add(this.btnSync);
            this.grpHost.Controls.Add(this.lblHostInfo);
            this.grpHost.Controls.Add(this.txtLog);

            this.pnlRight.Controls.Add(this.grpHost);
            System.Windows.Forms.Panel pnlSpacer = new System.Windows.Forms.Panel();
            pnlSpacer.Dock = System.Windows.Forms.DockStyle.Top; pnlSpacer.Height = 20;
            this.pnlRight.Controls.Add(pnlSpacer);
            this.pnlRight.Controls.Add(this.grpTransfer);


            // ==========================================
            // Center Panel (Details)
            // ==========================================
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(10, 60, 10, 20);

            this.grpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetails.Text = "Recipe Details: Recipe_002_PM_B_V2.1";
            this.grpDetails.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpDetails.BackColor = System.Drawing.Color.White;

            // 1. Grid (Step Sequence)
            this.dgvSteps.Location = new System.Drawing.Point(20, 40);
            this.dgvSteps.Size = new System.Drawing.Size(560, 200);
            this.dgvSteps.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvSteps.AllowUserToAddRows = false;
            this.dgvSteps.RowHeadersVisible = false;
            this.dgvSteps.Font = new System.Drawing.Font("Arial", 9F);
            this.dgvSteps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            gridStyleHeader.BackColor = System.Drawing.Color.CadetBlue;
            gridStyleHeader.ForeColor = System.Drawing.Color.White;
            gridStyleHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dgvSteps.ColumnHeadersDefaultCellStyle = gridStyleHeader;
            this.dgvSteps.EnableHeadersVisualStyles = false;

            this.colNo.HeaderText = "Step No.";
            this.colType.HeaderText = "Type";
            this.colDuration.HeaderText = "Duration (sec)";
            this.colFlow.HeaderText = "Gas Flow";
            this.dgvSteps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colNo, colType, colDuration, colFlow });

            this.dgvSteps.Rows.Add("1: Pre-Clean", "Clean", "60", "100");
            this.dgvSteps.Rows.Add("2: Deposition", "Depo", "120", "500");
            this.dgvSteps.Rows.Add("3: Etch", "Etch", "30", "50");
            this.dgvSteps.Rows.Add("4: Rinse", "Rinse", "45", "0");

            // 2. Parameters Group
            this.grpParams.Text = "Parameters (PM B)";
            this.grpParams.Location = new System.Drawing.Point(20, 260);
            this.grpParams.Size = new System.Drawing.Size(560, 260);
            this.grpParams.BackColor = System.Drawing.Color.WhiteSmoke;
            this.grpParams.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);

            // --- [수정] 파라미터 UI 수동 생성 (함수 호출 제거) ---

            // Target Temp
            this.lblTarget.Text = "Target Temp (°C)";
            this.lblTarget.Location = new System.Drawing.Point(20, 33);
            this.lblTarget.AutoSize = true;
            this.txtTarget.Text = "150.0";
            this.txtTarget.Location = new System.Drawing.Point(250, 30);
            this.txtTarget.Size = new System.Drawing.Size(100, 23);
            this.txtTarget.Font = new System.Drawing.Font("Arial", 9F);
            this.grpParams.Controls.Add(this.lblTarget);
            this.grpParams.Controls.Add(this.txtTarget);

            // RF Power
            this.lblRf.Text = "RF Power (W)";
            this.lblRf.Location = new System.Drawing.Point(20, 73);
            this.lblRf.AutoSize = true;
            this.txtRf.Text = "1200";
            this.txtRf.Location = new System.Drawing.Point(250, 70);
            this.txtRf.Size = new System.Drawing.Size(100, 23);
            this.txtRf.Font = new System.Drawing.Font("Arial", 9F);
            this.grpParams.Controls.Add(this.lblRf);
            this.grpParams.Controls.Add(this.txtRf);

            // Pressure
            this.lblPress.Text = "Pressure (mT)";
            this.lblPress.Location = new System.Drawing.Point(20, 113);
            this.lblPress.AutoSize = true;
            this.txtPress.Text = "25";
            this.txtPress.Location = new System.Drawing.Point(250, 110);
            this.txtPress.Size = new System.Drawing.Size(100, 23);
            this.txtPress.Font = new System.Drawing.Font("Arial", 9F);
            this.grpParams.Controls.Add(this.lblPress);
            this.grpParams.Controls.Add(this.txtPress);

            // Gas A
            this.lblGasA.Text = "Gas A Flow (sccm)";
            this.lblGasA.Location = new System.Drawing.Point(20, 153);
            this.lblGasA.AutoSize = true;
            this.txtGasA.Text = "50";
            this.txtGasA.Location = new System.Drawing.Point(250, 150);
            this.txtGasA.Size = new System.Drawing.Size(100, 23);
            this.txtGasA.Font = new System.Drawing.Font("Arial", 9F);
            this.grpParams.Controls.Add(this.lblGasA);
            this.grpParams.Controls.Add(this.txtGasA);

            // Gas B
            this.lblGasB.Text = "Gas B Flow (sccm)";
            this.lblGasB.Location = new System.Drawing.Point(20, 193);
            this.lblGasB.AutoSize = true;
            this.txtGasB.Text = "50";
            this.txtGasB.Location = new System.Drawing.Point(250, 190);
            this.txtGasB.Size = new System.Drawing.Size(100, 23);
            this.txtGasB.Font = new System.Drawing.Font("Arial", 9F);
            this.grpParams.Controls.Add(this.lblGasB);
            this.grpParams.Controls.Add(this.txtGasB);
            // ---------------------------------------------------

            // 3. Footer Buttons
            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(20, 560);
            this.btnSave.Size = new System.Drawing.Size(100, 40);
            this.btnSave.BackColor = System.Drawing.Color.LightGray;

            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.Location = new System.Drawing.Point(130, 560);
            this.btnSaveAs.Size = new System.Drawing.Size(100, 40);
            this.btnSaveAs.BackColor = System.Drawing.Color.LightGray;

            this.btnValidate.Text = "Validate";
            this.btnValidate.Location = new System.Drawing.Point(240, 560);
            this.btnValidate.Size = new System.Drawing.Size(100, 40);
            this.btnValidate.BackColor = System.Drawing.Color.CadetBlue;
            this.btnValidate.ForeColor = System.Drawing.Color.White;

            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(350, 560);
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.BackColor = System.Drawing.Color.IndianRed;
            this.btnCancel.ForeColor = System.Drawing.Color.White;

            this.grpDetails.Controls.Add(this.dgvSteps);
            this.grpDetails.Controls.Add(this.grpParams);
            this.grpDetails.Controls.Add(this.btnSave);
            this.grpDetails.Controls.Add(this.btnSaveAs);
            this.grpDetails.Controls.Add(this.btnValidate);
            this.grpDetails.Controls.Add(this.btnCancel);
            this.pnlCenter.Controls.Add(this.grpDetails);

            // Add Panels to Control
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
            this.grpDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).EndInit();
            this.grpParams.ResumeLayout(false);
            this.grpParams.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // Controls
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
        private System.Windows.Forms.Button btnMaint;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.GroupBox grpHost;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Label lblHostInfo;
        private System.Windows.Forms.TextBox txtLog;

        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.GroupBox grpDetails;
        public System.Windows.Forms.DataGridView dgvSteps;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlow;

        private System.Windows.Forms.GroupBox grpParams;
        private System.Windows.Forms.Label lblTarget; private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Label lblRf; private System.Windows.Forms.TextBox txtRf;
        private System.Windows.Forms.Label lblPress; private System.Windows.Forms.TextBox txtPress;
        private System.Windows.Forms.Label lblGasA; private System.Windows.Forms.TextBox txtGasA;
        private System.Windows.Forms.Label lblGasB; private System.Windows.Forms.TextBox txtGasB;

        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnSaveAs;
        public System.Windows.Forms.Button btnValidate;
        public System.Windows.Forms.Button btnCancel;
    }
}