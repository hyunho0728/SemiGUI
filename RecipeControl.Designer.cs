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
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();

            this.pnlCenter = new System.Windows.Forms.Panel();

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
            this.btnApply = new System.Windows.Forms.Button(); // [변경] 정식 버튼으로 사용
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlLeft.SuspendLayout();
            this.grpList.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.grpParamA.SuspendLayout();
            this.grpParamB.SuspendLayout();
            this.grpParamC.SuspendLayout();
            this.SuspendLayout();

            // 
            // RecipeControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(232, 234, 237);
            this.Size = new System.Drawing.Size(1020, 720);

            // Header
            this.lblTitleHeader.Text = "Recipe Management";
            this.lblTitleHeader.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitleHeader.Location = new System.Drawing.Point(20, 15);
            this.lblTitleHeader.AutoSize = true;

            // Left Panel
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

            // New
            this.btnNew.Text = "New";
            this.btnNew.Location = new System.Drawing.Point(10, 580);
            this.btnNew.Size = new System.Drawing.Size(95, 35);
            this.btnNew.BackColor = System.Drawing.Color.WhiteSmoke;

            // Delete
            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new System.Drawing.Point(115, 580);
            this.btnDelete.Size = new System.Drawing.Size(95, 35);
            this.btnDelete.BackColor = System.Drawing.Color.WhiteSmoke;

            // Import
            this.btnImport.Text = "Import";
            this.btnImport.Location = new System.Drawing.Point(10, 625);
            this.btnImport.Size = new System.Drawing.Size(95, 35);
            this.btnImport.BackColor = System.Drawing.Color.LightBlue;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // Export
            this.btnExport.Text = "Export";
            this.btnExport.Location = new System.Drawing.Point(115, 625);
            this.btnExport.Size = new System.Drawing.Size(95, 35);
            this.btnExport.BackColor = System.Drawing.Color.LightGray;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.grpList.Controls.Add(this.lstRecipes);
            this.grpList.Controls.Add(this.txtSearch);
            this.grpList.Controls.Add(this.btnNew);
            this.grpList.Controls.Add(this.btnDelete);
            this.grpList.Controls.Add(this.btnImport);
            this.grpList.Controls.Add(this.btnExport);
            this.pnlLeft.Controls.Add(this.grpList);

            // Center Panel
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(10, 60, 10, 20);

            // PM A Group
            this.grpParamA.Text = "PM A (Oxidation)";
            this.grpParamA.Location = new System.Drawing.Point(10, 60);
            this.grpParamA.Size = new System.Drawing.Size(245, 300);
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

            // PM B Group
            this.grpParamB.Text = "PM B (Photo)";
            this.grpParamB.Location = new System.Drawing.Point(265, 60);
            this.grpParamB.Size = new System.Drawing.Size(245, 300);
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

            // PM C Group
            this.grpParamC.Text = "PM C (Etch)";
            this.grpParamC.Location = new System.Drawing.Point(520, 60);
            this.grpParamC.Size = new System.Drawing.Size(245, 300);
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
            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(10, 400);
            this.btnSave.Size = new System.Drawing.Size(120, 45);
            this.btnSave.BackColor = System.Drawing.Color.LightGray;
            this.btnSave.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);

            this.btnApply.Text = "Apply";
            this.btnApply.Location = new System.Drawing.Point(140, 400);
            this.btnApply.Size = new System.Drawing.Size(120, 45);
            this.btnApply.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);

            this.btnCancel.Text = "Close";
            this.btnCancel.Location = new System.Drawing.Point(645, 400);
            this.btnCancel.Size = new System.Drawing.Size(120, 45);
            this.btnCancel.BackColor = System.Drawing.Color.IndianRed;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);

            this.pnlCenter.Controls.Add(this.btnSave);
            this.pnlCenter.Controls.Add(this.btnApply);
            this.pnlCenter.Controls.Add(this.btnCancel);

            this.Controls.Add(this.lblTitleHeader);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlLeft);

            this.pnlLeft.ResumeLayout(false);
            this.grpList.ResumeLayout(false);
            this.grpList.PerformLayout();
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
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;

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
        public System.Windows.Forms.Button btnApply;
        public System.Windows.Forms.Button btnCancel;
    }
}