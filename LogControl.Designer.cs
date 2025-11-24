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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle gridHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle gridCellStyle = new System.Windows.Forms.DataGridViewCellStyle();

            this.lblTitle = new System.Windows.Forms.Label();

            // [수정] 날짜는 그대로, 시간은 콤보박스로 변경
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.cboStartHour = new System.Windows.Forms.ComboBox();
            this.lblStartColon = new System.Windows.Forms.Label();
            this.cboStartMin = new System.Windows.Forms.ComboBox();

            this.lblTilde = new System.Windows.Forms.Label();

            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.cboEndHour = new System.Windows.Forms.ComboBox();
            this.lblEndColon = new System.Windows.Forms.Label();
            this.cboEndMin = new System.Windows.Forms.ComboBox();

            // 필터 및 기능
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

            // 타이머
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);

            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();

            // 
            // LogControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(1280, 720);

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(63, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "LOG";

            // 
            // dtpStartDate (날짜)
            // 
            this.dtpStartDate.Font = new System.Drawing.Font("Arial", 10F);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(35, 60);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(110, 23);
            this.dtpStartDate.TabIndex = 1;

            // 
            // cboStartHour (시)
            // 
            this.cboStartHour.Font = new System.Drawing.Font("Arial", 10F);
            this.cboStartHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartHour.FormattingEnabled = true;
            this.cboStartHour.Location = new System.Drawing.Point(150, 59);
            this.cboStartHour.Name = "cboStartHour";
            this.cboStartHour.Size = new System.Drawing.Size(50, 24);
            this.cboStartHour.TabIndex = 2;

            // 
            // lblStartColon (:)
            // 
            this.lblStartColon.AutoSize = true;
            this.lblStartColon.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblStartColon.Location = new System.Drawing.Point(203, 63);
            this.lblStartColon.Name = "lblStartColon";
            this.lblStartColon.Size = new System.Drawing.Size(12, 16);
            this.lblStartColon.TabIndex = 3;
            this.lblStartColon.Text = ":";

            // 
            // cboStartMin (분)
            // 
            this.cboStartMin.Font = new System.Drawing.Font("Arial", 10F);
            this.cboStartMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartMin.FormattingEnabled = true;
            this.cboStartMin.Location = new System.Drawing.Point(218, 59);
            this.cboStartMin.Name = "cboStartMin";
            this.cboStartMin.Size = new System.Drawing.Size(50, 24);
            this.cboStartMin.TabIndex = 4;

            // 
            // lblTilde (~)
            // 
            this.lblTilde.AutoSize = true;
            this.lblTilde.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTilde.Location = new System.Drawing.Point(280, 62); // 위치 조정
            this.lblTilde.Name = "lblTilde";
            this.lblTilde.Size = new System.Drawing.Size(18, 19);
            this.lblTilde.TabIndex = 5;
            this.lblTilde.Text = "~";

            // 
            // dtpEndDate (날짜)
            // 
            this.dtpEndDate.Font = new System.Drawing.Font("Arial", 10F);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(310, 60);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(110, 23);
            this.dtpEndDate.TabIndex = 6;

            // 
            // cboEndHour (시)
            // 
            this.cboEndHour.Font = new System.Drawing.Font("Arial", 10F);
            this.cboEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEndHour.FormattingEnabled = true;
            this.cboEndHour.Location = new System.Drawing.Point(425, 59);
            this.cboEndHour.Name = "cboEndHour";
            this.cboEndHour.Size = new System.Drawing.Size(50, 24);
            this.cboEndHour.TabIndex = 7;

            // 
            // lblEndColon (:)
            // 
            this.lblEndColon.AutoSize = true;
            this.lblEndColon.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblEndColon.Location = new System.Drawing.Point(478, 63);
            this.lblEndColon.Name = "lblEndColon";
            this.lblEndColon.Size = new System.Drawing.Size(12, 16);
            this.lblEndColon.TabIndex = 8;
            this.lblEndColon.Text = ":";

            // 
            // cboEndMin (분)
            // 
            this.cboEndMin.Font = new System.Drawing.Font("Arial", 10F);
            this.cboEndMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEndMin.FormattingEnabled = true;
            this.cboEndMin.Location = new System.Drawing.Point(493, 59);
            this.cboEndMin.Name = "cboEndMin";
            this.cboEndMin.Size = new System.Drawing.Size(50, 24);
            this.cboEndMin.TabIndex = 9;

            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoSize = true;
            this.chkAlarm.Font = new System.Drawing.Font("Arial", 10F);
            this.chkAlarm.Location = new System.Drawing.Point(35, 95);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(63, 20);
            this.chkAlarm.TabIndex = 10;
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
            this.chkWarning.TabIndex = 11;
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
            this.chkEvent.TabIndex = 12;
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
            this.cboEquipment.Location = new System.Drawing.Point(900, 59);
            this.cboEquipment.Name = "cboEquipment";
            this.cboEquipment.Size = new System.Drawing.Size(200, 24);
            this.cboEquipment.TabIndex = 13;
            this.cboEquipment.Text = "Equipment (ALL)";

            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Arial", 10F);
            this.txtSearch.Location = new System.Drawing.Point(900, 92);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 14;
            this.txtSearch.Text = "Search Message...";

            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

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

            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colType,
            this.colEqp,
            this.colMsg});

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
            this.dgvLogs.Size = new System.Drawing.Size(1065, 450);
            this.dgvLogs.TabIndex = 15;

            // 
            // colTime
            // 
            this.colTime.HeaderText = "TimeStamp";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 200;

            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 100;

            // 
            // colEqp
            // 
            this.colEqp.HeaderText = "Equipment";
            this.colEqp.Name = "colEqp";
            this.colEqp.ReadOnly = true;
            this.colEqp.Width = 150;

            // 
            // colMsg
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
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "REFRESH";
            this.btnRefresh.UseVisualStyleBackColor = false;

            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.White;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(950, 600);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(150, 50);
            this.btnExport.TabIndex = 17;
            this.btnExport.Text = "EXPORT";
            this.btnExport.UseVisualStyleBackColor = false;

            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Interval = 1000;

            // 
            // LogControl Add
            // 
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvLogs);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cboEquipment);
            this.Controls.Add(this.chkEvent);
            this.Controls.Add(this.chkWarning);
            this.Controls.Add(this.chkAlarm);

            // 시간 관련 컨트롤 추가
            this.Controls.Add(this.cboEndMin);
            this.Controls.Add(this.lblEndColon);
            this.Controls.Add(this.cboEndHour);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblTilde);
            this.Controls.Add(this.cboStartMin);
            this.Controls.Add(this.lblStartColon);
            this.Controls.Add(this.cboStartHour);
            this.Controls.Add(this.dtpStartDate);

            this.Controls.Add(this.lblTitle);
            this.Name = "LogControl";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;

        // 변경된 날짜/시간 컨트롤
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.ComboBox cboStartHour;
        private System.Windows.Forms.Label lblStartColon;
        private System.Windows.Forms.ComboBox cboStartMin;

        private System.Windows.Forms.Label lblTilde;

        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.ComboBox cboEndHour;
        private System.Windows.Forms.Label lblEndColon;
        private System.Windows.Forms.ComboBox cboEndMin;

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
        private System.Windows.Forms.Timer tmrRefresh;
    }
}