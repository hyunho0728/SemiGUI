namespace SemiGUI
{
    partial class UtilityControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSysConfig = new System.Windows.Forms.Button();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.btnDataTrend = new System.Windows.Forms.Button();
            this.btnDiagnostics = new System.Windows.Forms.Button();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.btnSoftwareUpdate = new System.Windows.Forms.Button();
            this.btnCalibration = new System.Windows.Forms.Button();
            this.btnUserMgmt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(40, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(121, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "UTILITY";
            // 
            // btnSysConfig
            // 
            this.btnSysConfig.BackColor = System.Drawing.Color.White;
            this.btnSysConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSysConfig.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnSysConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSysConfig.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnSysConfig.Location = new System.Drawing.Point(60, 100);
            this.btnSysConfig.Name = "btnSysConfig";
            this.btnSysConfig.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnSysConfig.Size = new System.Drawing.Size(320, 120);
            this.btnSysConfig.TabIndex = 1;
            this.btnSysConfig.Text = "System\nConfiguration";
            this.btnSysConfig.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSysConfig.UseVisualStyleBackColor = false;
            // 
            // btnRecipe
            // 
            this.btnRecipe.BackColor = System.Drawing.Color.White;
            this.btnRecipe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecipe.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecipe.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnRecipe.Location = new System.Drawing.Point(410, 100);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnRecipe.Size = new System.Drawing.Size(320, 120);
            this.btnRecipe.TabIndex = 2;
            this.btnRecipe.Text = "Recipe\nManagement";
            this.btnRecipe.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRecipe.UseVisualStyleBackColor = false;
            // 
            // btnDataTrend
            // 
            this.btnDataTrend.BackColor = System.Drawing.Color.White;
            this.btnDataTrend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDataTrend.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnDataTrend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataTrend.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnDataTrend.Location = new System.Drawing.Point(760, 100);
            this.btnDataTrend.Name = "btnDataTrend";
            this.btnDataTrend.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnDataTrend.Size = new System.Drawing.Size(320, 120);
            this.btnDataTrend.TabIndex = 3;
            this.btnDataTrend.Text = "Data &\nTrend View";
            this.btnDataTrend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDataTrend.UseVisualStyleBackColor = false;
            // 
            // btnDiagnostics
            // 
            this.btnDiagnostics.BackColor = System.Drawing.Color.White;
            this.btnDiagnostics.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiagnostics.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnDiagnostics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiagnostics.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnDiagnostics.Location = new System.Drawing.Point(60, 250);
            this.btnDiagnostics.Name = "btnDiagnostics";
            this.btnDiagnostics.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnDiagnostics.Size = new System.Drawing.Size(320, 120);
            this.btnDiagnostics.TabIndex = 4;
            this.btnDiagnostics.Text = "Diagnostics";
            this.btnDiagnostics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDiagnostics.UseVisualStyleBackColor = false;
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.BackColor = System.Drawing.Color.White;
            this.btnMaintenance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaintenance.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnMaintenance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaintenance.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnMaintenance.Location = new System.Drawing.Point(410, 250);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnMaintenance.Size = new System.Drawing.Size(320, 120);
            this.btnMaintenance.TabIndex = 5;
            this.btnMaintenance.Text = "Maintenance";
            this.btnMaintenance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMaintenance.UseVisualStyleBackColor = false;
            // 
            // btnSoftwareUpdate
            // 
            this.btnSoftwareUpdate.BackColor = System.Drawing.Color.White;
            this.btnSoftwareUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSoftwareUpdate.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnSoftwareUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSoftwareUpdate.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnSoftwareUpdate.Location = new System.Drawing.Point(760, 250);
            this.btnSoftwareUpdate.Name = "btnSoftwareUpdate";
            this.btnSoftwareUpdate.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnSoftwareUpdate.Size = new System.Drawing.Size(320, 120);
            this.btnSoftwareUpdate.TabIndex = 6;
            this.btnSoftwareUpdate.Text = "Software\nUpdate";
            this.btnSoftwareUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSoftwareUpdate.UseVisualStyleBackColor = false;
            // 
            // btnCalibration
            // 
            this.btnCalibration.BackColor = System.Drawing.Color.White;
            this.btnCalibration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalibration.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibration.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnCalibration.Location = new System.Drawing.Point(60, 400);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnCalibration.Size = new System.Drawing.Size(320, 120);
            this.btnCalibration.TabIndex = 7;
            this.btnCalibration.Text = "Maintenance";
            this.btnCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCalibration.UseVisualStyleBackColor = false;
            // 
            // btnUserMgmt
            // 
            this.btnUserMgmt.BackColor = System.Drawing.Color.White;
            this.btnUserMgmt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserMgmt.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnUserMgmt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserMgmt.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnUserMgmt.Location = new System.Drawing.Point(410, 400);
            this.btnUserMgmt.Name = "btnUserMgmt";
            this.btnUserMgmt.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.btnUserMgmt.Size = new System.Drawing.Size(320, 120);
            this.btnUserMgmt.TabIndex = 8;
            this.btnUserMgmt.Text = "User\nManagement";
            this.btnUserMgmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUserMgmt.UseVisualStyleBackColor = false;
            // 
            // UtilityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.btnUserMgmt);
            this.Controls.Add(this.btnCalibration);
            this.Controls.Add(this.btnSoftwareUpdate);
            this.Controls.Add(this.btnMaintenance);
            this.Controls.Add(this.btnDiagnostics);
            this.Controls.Add(this.btnDataTrend);
            this.Controls.Add(this.btnRecipe);
            this.Controls.Add(this.btnSysConfig);
            this.Controls.Add(this.lblTitle);
            this.Name = "UtilityControl";
            this.Size = new System.Drawing.Size(1130, 700);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Button btnSysConfig;
        public System.Windows.Forms.Button btnRecipe;
        public System.Windows.Forms.Button btnDataTrend;
        public System.Windows.Forms.Button btnDiagnostics;
        public System.Windows.Forms.Button btnMaintenance;
        public System.Windows.Forms.Button btnSoftwareUpdate;
        public System.Windows.Forms.Button btnCalibration;
        public System.Windows.Forms.Button btnUserMgmt;
    }
}