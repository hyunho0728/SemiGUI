using System;
using System.Windows.Forms;

namespace SemiGUI
{
    public partial class LogControl : UserControl
    {
        public LogControl()
        {
            InitializeComponent();
            LoadDummyData();
        }

        private void LoadDummyData()
        {
            // 예시 데이터 추가
            dgvLogs.Rows.Add("2025-11-24 12:00:01", "Event", "System", "Application Started");
            dgvLogs.Rows.Add("2025-11-24 12:05:33", "Alarm", "PM A", "Temp Sensor Timeout");
            dgvLogs.Rows.Add("2025-11-24 12:10:15", "Event", "PM B", "Process Started (Recipe_002)");
            dgvLogs.Rows.Add("2025-11-24 12:15:42", "Warning", "PM C", "Pressure Low Limit Warning");
            dgvLogs.Rows.Add("2025-11-24 12:20:00", "Event", "System", "User Login (Admin)");
        }
    }
}