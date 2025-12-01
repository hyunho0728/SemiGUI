namespace SemiGUI
{
    partial class ConfigControl
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpSystem = new System.Windows.Forms.GroupBox();
            this.lblRobotSpeed = new System.Windows.Forms.Label();
            this.numRobotSpeed = new System.Windows.Forms.NumericUpDown();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            this.grpSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRobotSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "SYSTEM CONFIGURATION";

            // grpSystem
            this.grpSystem.Controls.Add(this.lblRobotSpeed);
            this.grpSystem.Controls.Add(this.numRobotSpeed);
            this.grpSystem.Controls.Add(this.lblInterval);
            this.grpSystem.Controls.Add(this.numInterval);
            this.grpSystem.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpSystem.Location = new System.Drawing.Point(25, 70);
            this.grpSystem.Size = new System.Drawing.Size(400, 200);
            this.grpSystem.Text = "Simulation Settings";

            // Robot Speed
            this.lblRobotSpeed.AutoSize = true;
            this.lblRobotSpeed.Location = new System.Drawing.Point(30, 50);
            this.lblRobotSpeed.Text = "Robot Speed (deg/tick):";

            this.numRobotSpeed.Location = new System.Drawing.Point(220, 48);
            this.numRobotSpeed.Size = new System.Drawing.Size(100, 23);
            this.numRobotSpeed.Minimum = 1;
            this.numRobotSpeed.Maximum = 100;
            this.numRobotSpeed.Value = 10;

            // Interval
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(30, 100);
            this.lblInterval.Text = "Timer Interval (ms):";

            this.numInterval.Location = new System.Drawing.Point(220, 98);
            this.numInterval.Size = new System.Drawing.Size(100, 23);
            this.numInterval.Minimum = 10;
            this.numInterval.Maximum = 1000;
            this.numInterval.Value = 50;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(25, 300);
            this.btnSave.Size = new System.Drawing.Size(120, 45);
            this.btnSave.Text = "SAVE";

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.IndianRed;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(305, 300);
            this.btnClose.Size = new System.Drawing.Size(120, 45);
            this.btnClose.Text = "CLOSE";

            // ConfigControl
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpSystem);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Size = new System.Drawing.Size(800, 600);
            this.BackColor = System.Drawing.Color.White;

            this.grpSystem.ResumeLayout(false);
            this.grpSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRobotSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpSystem;
        private System.Windows.Forms.Label lblRobotSpeed;
        private System.Windows.Forms.NumericUpDown numRobotSpeed;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.NumericUpDown numInterval;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnClose;
    }
}