namespace SemiGUI
{
    partial class LogControl
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
            System.Windows.Forms.DataGridViewCellStyle gridHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle gridCellStyle = new System.Windows.Forms.DataGridViewCellStyle();

            this.lblTitle = new System.Windows.Forms.Label();

            // 필터 컨트롤들
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblTilde = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.chkAlarm = new System.Windows.Forms.CheckBox();
            this.chkWarning = new System.Windows.Forms.CheckBox();
            this.chkEvent = new System.Windows.Forms.CheckBox();
            this.cboEquipment = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();

            // 그리드
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // 하단 버튼
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();

            // 
            // LogControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(1280, 720);

            // 
            // lblTitle (LOG)
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(63, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "LOG";

            // 
            // dtpStart (시작 날짜)
            // 
            this.dtpStart.Font = new System.Drawing.Font("Arial", 10F);
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpStart.Location = new System.Drawing.Point(35, 60);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(180, 23);
            this.dtpStart.TabIndex = 1;

            // 
            // lblTilde (~)
            // 
            this.lblTilde.AutoSize = true;
            this.lblTilde.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTilde.Location = new System.Drawing.Point(220, 62);
            this.lblTilde.Name = "lblTilde";
            this.lblTilde.Size = new System.Drawing.Size(18, 19);
            this.lblTilde.TabIndex = 2;
            this.lblTilde.Text = "~";

            // 
            // dtpEnd (종료 날짜)
            // 
            this.dtpEnd.Font = new System.Drawing.Font("Arial", 10F);
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEnd.Location = new System.Drawing.Point(245, 60);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(180, 23);
            this.dtpEnd.TabIndex = 3;

            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoSize = true;
            this.chkAlarm.Font = new System.Drawing.Font("Arial", 10F);
            this.chkAlarm.Location = new System.Drawing.Point(35, 95);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(63, 20);
            this.chkAlarm.TabIndex = 4;
            this.chkAlarm.Text = "Alarm";
            this.chkAlarm.UseVisualStyleBackColor = true;

            // 
            // chkWarning
            // 
            this.chkWarning.AutoSize = true;
            this.chkWarning.Font = new System.Drawing.Font("Arial", 10F);
            this.chkWarning.Location = new System.Drawing.Point(110, 95);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Size = new System.Drawing.Size(78, 20);
            this.chkWarning.TabIndex = 5;
            this.chkWarning.Text = "Warning";
            this.chkWarning.UseVisualStyleBackColor = true;

            // 
            // chkEvent
            // 
            this.chkEvent.AutoSize = true;
            this.chkEvent.Font = new System.Drawing.Font("Arial", 10F);
            this.chkEvent.Location = new System.Drawing.Point(200, 95);
            this.chkEvent.Name = "chkEvent";
            this.chkEvent.Size = new System.Drawing.Size(62, 20);
            this.chkEvent.TabIndex = 6;
            this.chkEvent.Text = "Event";
            this.chkEvent.UseVisualStyleBackColor = true;

            // 
            // cboEquipment
            // 
            this.cboEquipment.Font = new System.Drawing.Font("Arial", 10F);
            this.cboEquipment.FormattingEnabled = true;
            this.cboEquipment.Items.AddRange(new object[] {
            "ALL",
            "PM A (Oxidation)",
            "PM B (Photo)",
            "PM C (Etch)"});
            this.cboEquipment.Location = new System.Drawing.Point(900, 59); // 우측 상단 배치
            this.cboEquipment.Name = "cboEquipment";
            this.cboEquipment.Size = new System.Drawing.Size(200, 24);
            this.cboEquipment.TabIndex = 7;
            this.cboEquipment.Text = "Equipment (ALL)";

            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Arial", 10F);
            this.txtSearch.Location = new System.Drawing.Point(900, 92);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 8;
            this.txtSearch.Text = "Search Message...";

            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            // 헤더 스타일
            gridHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            gridHeaderStyle.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            gridHeaderStyle.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            gridHeaderStyle.ForeColor = System.Drawing.Color.Black;
            gridHeaderStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            gridHeaderStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            gridHeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLogs.ColumnHeadersDefaultCellStyle = gridHeaderStyle;
            this.dgvLogs.ColumnHeadersHeight = 35;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // 컬럼 추가
            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colType,
            this.colEqp,
            this.colMsg});

            // 셀 스타일
            gridCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            gridCellStyle.BackColor = System.Drawing.SystemColors.Window;
            gridCellStyle.Font = new System.Drawing.Font("Arial", 9F);
            gridCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            gridCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            gridCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            gridCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogs.DefaultCellStyle = gridCellStyle;

            this.dgvLogs.EnableHeadersVisualStyles = false;
            this.dgvLogs.Location = new System.Drawing.Point(35, 130);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowTemplate.Height = 25;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.Size = new System.Drawing.Size(1065, 450); // 중앙 그리드 영역
            this.dgvLogs.TabIndex = 9;

            // 
            // colTime (TimeStamp)
            // 
            this.colTime.HeaderText = "TimeStamp";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 200;

            // 
            // colType (Type)
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 100;

            // 
            // colEqp (Equipment)
            // 
            this.colEqp.HeaderText = "Equipment";
            this.colEqp.Name = "colEqp";
            this.colEqp.ReadOnly = true;
            this.colEqp.Width = 150;

            // 
            // colMsg (Message)
            // 
            this.colMsg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMsg.HeaderText = "Message";
            this.colMsg.Name = "colMsg";
            this.colMsg.ReadOnly = true;

            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(35, 600);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 50);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "REFRESH";
            this.btnRefresh.UseVisualStyleBackColor = false;

            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.White;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(950, 600); // 우측 하단
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(150, 50);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = false;

            // 
            // Add Controls to UserControl
            // 
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvLogs);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cboEquipment);
            this.Controls.Add(this.chkEvent);
            this.Controls.Add(this.chkWarning);
            this.Controls.Add(this.chkAlarm);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.lblTilde);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.lblTitle);
            this.Name = "LogControl";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblTilde;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.CheckBox chkAlarm;
        private System.Windows.Forms.CheckBox chkWarning;
        private System.Windows.Forms.CheckBox chkEvent;
        private System.Windows.Forms.ComboBox cboEquipment;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMsg;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;
    }
}