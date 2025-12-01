using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace SemiGUI
{
    public partial class LogControl : UserControl
    {
        private string connectionString = "Server=localhost;Port=3306;Database=SemiGuiData;Uid=root;Pwd=1234;Charset=utf8;";
        private const string DEFAULT_SEARCH_TEXT = "Search Message...";
        private bool isErrorShown = false;

        // [추가] 마지막으로 확인한 데이터 개수 저장 (초기값 -1)
        private int lastRowCount = -1;

        public LogControl()
        {
            InitializeComponent();
            InitializeTimeCombos();

            // [초기값]
            dtpStartDate.Value = DateTime.Now.AddDays(-7);
            dtpEndDate.Value = DateTime.Now;

            cboStartHour.SelectedIndex = 0;
            cboStartMin.SelectedIndex = 0;

            cboEndHour.SelectedIndex = 23;
            cboEndMin.SelectedIndex = 59;

            chkAlarm.Checked = false;
            chkWarning.Checked = false;
            chkEvent.Checked = false;

            if (cboEquipment.Items.Count > 0) cboEquipment.SelectedIndex = 0;

            InitializeEvents();

            if (!this.DesignMode)
            {
                // 최초 로드 시에는 개수 상관없이 로드
                LoadLogsFromDB();
                tmrRefresh.Start();
            }
        }

        private void InitializeTimeCombos()
        {
            for (int i = 0; i < 24; i++)
            {
                string val = i.ToString("00");
                cboStartHour.Items.Add(val);
                cboEndHour.Items.Add(val);
            }

            for (int i = 0; i < 60; i++)
            {
                string val = i.ToString("00");
                cboStartMin.Items.Add(val);
                cboEndMin.Items.Add(val);
            }
        }

        private void InitializeEvents()
        {
            this.btnRefresh.Click += (s, e) => LoadLogsFromDB(); // 수동 새로고침

            this.btnExport.Click += BtnExport_Click;

            // 필터 변경 시 즉시 로드
            this.dtpStartDate.ValueChanged += (s, e) => LoadLogsFromDB();
            this.dtpEndDate.ValueChanged += (s, e) => LoadLogsFromDB();
            this.cboStartHour.SelectedIndexChanged += (s, e) => LoadLogsFromDB();
            this.cboStartMin.SelectedIndexChanged += (s, e) => LoadLogsFromDB();
            this.cboEndHour.SelectedIndexChanged += (s, e) => LoadLogsFromDB();
            this.cboEndMin.SelectedIndexChanged += (s, e) => LoadLogsFromDB();

            this.chkAlarm.CheckedChanged += (s, e) => LoadLogsFromDB();
            this.chkWarning.CheckedChanged += (s, e) => LoadLogsFromDB();
            this.chkEvent.CheckedChanged += (s, e) => LoadLogsFromDB();

            this.cboEquipment.SelectedIndexChanged += (s, e) => LoadLogsFromDB();

            this.txtSearch.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter) LoadLogsFromDB();
            };

            this.txtSearch.Enter += (s, e) => {
                if (txtSearch.Text == DEFAULT_SEARCH_TEXT)
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = System.Drawing.Color.Black;
                }
            };

            this.txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = DEFAULT_SEARCH_TEXT;
                    txtSearch.ForeColor = System.Drawing.Color.Gray;
                }
            };

            // 타이머: 변경 사항 확인 후 갱신
            this.tmrRefresh.Tick += (s, e) => {
                CheckAndRefreshLogs();
            };
        }

        // [핵심] 데이터 변경 여부 확인 후 갱신 (쿼리 최소화)
        private void CheckAndRefreshLogs()
        {
            // 현재 필터 조건에 맞는 데이터 개수만 빠르게 조회 (SELECT COUNT(*))
            int currentCount = GetLogCountFromDB();

            // DB 에러(-1)가 아니고, 개수가 다를 때만(데이터 추가/삭제) 갱신
            if (currentCount != -1 && currentCount != lastRowCount)
            {
                // silent=true: 스크롤 위치 유지
                LoadLogsFromDB(silent: true);
            }
        }

        // [핵심] 조건에 맞는 로그 개수 반환
        private int GetLogCountFromDB()
        {
            if (cboStartHour.SelectedItem == null) return -1;

            int startH = int.Parse(cboStartHour.SelectedItem.ToString());
            int startM = int.Parse(cboStartMin.SelectedItem.ToString());
            DateTime start = dtpStartDate.Value.Date.AddHours(startH).AddMinutes(startM);

            int endH = int.Parse(cboEndHour.SelectedItem.ToString());
            int endM = int.Parse(cboEndMin.SelectedItem.ToString());
            DateTime end = dtpEndDate.Value.Date.AddHours(endH).AddMinutes(endM).AddSeconds(59);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT COUNT(*) FROM logs "); // 개수만 조회
                    query.Append("WHERE timestamp BETWEEN @startTime AND @endTime ");

                    // 필터 조건 (LoadLogsFromDB와 동일)
                    bool anyChecked = chkAlarm.Checked || chkWarning.Checked || chkEvent.Checked;
                    if (anyChecked)
                    {
                        query.Append("AND type IN (");
                        bool first = true;
                        if (chkAlarm.Checked) { query.Append("'Alarm'"); first = false; }
                        if (chkWarning.Checked) { query.Append(first ? "'Warning'" : ", 'Warning'"); first = false; }
                        if (chkEvent.Checked) { query.Append(first ? "'Event'" : ", 'Event'"); }
                        query.Append(") ");
                    }

                    if (cboEquipment.SelectedItem != null)
                    {
                        string selectedEqp = cboEquipment.SelectedItem.ToString();
                        if (!selectedEqp.Contains("ALL"))
                        {
                            query.Append("AND equipment = @eqp ");
                        }
                    }

                    string searchText = txtSearch.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != DEFAULT_SEARCH_TEXT)
                    {
                        query.Append("AND message LIKE @search ");
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@startTime", start);
                        cmd.Parameters.AddWithValue("@endTime", end);

                        if (cboEquipment.SelectedItem != null && !cboEquipment.SelectedItem.ToString().Contains("ALL"))
                        {
                            string eqpVal = cboEquipment.SelectedItem.ToString();
                            if (eqpVal.Contains("(")) eqpVal = eqpVal.Split('(')[0].Trim();
                            cmd.Parameters.AddWithValue("@eqp", eqpVal);
                        }

                        if (!string.IsNullOrWhiteSpace(searchText) && searchText != DEFAULT_SEARCH_TEXT)
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        }

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch
            {
                return -1; // 에러 시 -1
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvLogs.Rows.Count == 0)
            {
                MessageBox.Show("저장할 데이터가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "로그 데이터 내보내기";
                sfd.Filter = "텍스트 파일 (*.txt)|*.txt|CSV 파일 (*.csv)|*.csv";
                sfd.FileName = $"LogExport_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool timerWasRunning = tmrRefresh.Enabled;
                    tmrRefresh.Stop();

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            string ext = Path.GetExtension(sfd.FileName).ToLower();
                            string separator = (ext == ".csv") ? "," : "\t";

                            for (int i = 0; i < dgvLogs.Columns.Count; i++)
                            {
                                sw.Write(dgvLogs.Columns[i].HeaderText);
                                if (i < dgvLogs.Columns.Count - 1) sw.Write(separator);
                            }
                            sw.WriteLine();

                            foreach (DataGridViewRow row in dgvLogs.Rows)
                            {
                                if (row.IsNewRow) continue;

                                for (int i = 0; i < dgvLogs.Columns.Count; i++)
                                {
                                    string value = row.Cells[i].Value?.ToString() ?? "";
                                    if (ext == ".csv" && (value.Contains(",") || value.Contains("\n") || value.Contains("\"")))
                                    {
                                        value = $"\"{value.Replace("\"", "\"\"")}\"";
                                    }
                                    sw.Write(value);
                                    if (i < dgvLogs.Columns.Count - 1) sw.Write(separator);
                                }
                                sw.WriteLine();
                            }
                        }
                        MessageBox.Show("저장이 완료되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"저장 중 오류가 발생했습니다:\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (timerWasRunning) tmrRefresh.Start();
                    }
                }
            }
        }

        private void LoadLogsFromDB(bool silent = false)
        {
            if (cboStartHour.SelectedItem == null || cboStartMin.SelectedItem == null ||
                cboEndHour.SelectedItem == null || cboEndMin.SelectedItem == null) return;

            // [수정] 수동 호출 시에는 lastRowCount를 갱신하지 않고 -1로 두어, 다음 틱에 개수 체크가 확실히 동작하도록 유도
            // (혹은 여기서 로드한 개수로 바로 업데이트해도 됩니다. 여기서는 업데이트하도록 수정)

            int startH = int.Parse(cboStartHour.SelectedItem.ToString());
            int startM = int.Parse(cboStartMin.SelectedItem.ToString());
            DateTime start = dtpStartDate.Value.Date.AddHours(startH).AddMinutes(startM);

            int endH = int.Parse(cboEndHour.SelectedItem.ToString());
            int endM = int.Parse(cboEndMin.SelectedItem.ToString());
            DateTime end = dtpEndDate.Value.Date.AddHours(endH).AddMinutes(endM).AddSeconds(59);

            // [핵심] 스크롤 위치 및 선택 행 저장
            int firstVisibleRow = dgvLogs.FirstDisplayedScrollingRowIndex;
            int selectedRowIndex = (dgvLogs.SelectedRows.Count > 0) ? dgvLogs.SelectedRows[0].Index : -1;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    isErrorShown = false;

                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT timestamp, type, equipment, message FROM logs ");
                    query.Append("WHERE timestamp BETWEEN @startTime AND @endTime ");

                    bool anyChecked = chkAlarm.Checked || chkWarning.Checked || chkEvent.Checked;
                    if (anyChecked)
                    {
                        query.Append("AND type IN (");
                        bool first = true;
                        if (chkAlarm.Checked) { query.Append("'Alarm'"); first = false; }
                        if (chkWarning.Checked) { query.Append(first ? "'Warning'" : ", 'Warning'"); first = false; }
                        if (chkEvent.Checked) { query.Append(first ? "'Event'" : ", 'Event'"); }
                        query.Append(") ");
                    }

                    if (cboEquipment.SelectedItem != null)
                    {
                        string selectedEqp = cboEquipment.SelectedItem.ToString();
                        if (!selectedEqp.Contains("ALL"))
                        {
                            query.Append("AND equipment = @eqp ");
                        }
                    }

                    string searchText = txtSearch.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(searchText) && searchText != DEFAULT_SEARCH_TEXT)
                    {
                        query.Append("AND message LIKE @search ");
                    }

                    query.Append("ORDER BY timestamp DESC"); // 최신순

                    using (MySqlCommand cmd = new MySqlCommand(query.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@startTime", start);
                        cmd.Parameters.AddWithValue("@endTime", end);

                        if (cboEquipment.SelectedItem != null && !cboEquipment.SelectedItem.ToString().Contains("ALL"))
                        {
                            string eqpVal = cboEquipment.SelectedItem.ToString();
                            if (eqpVal.Contains("(")) eqpVal = eqpVal.Split('(')[0].Trim();
                            cmd.Parameters.AddWithValue("@eqp", eqpVal);
                        }

                        if (!string.IsNullOrWhiteSpace(searchText) && searchText != DEFAULT_SEARCH_TEXT)
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dgvLogs.Rows.Clear();

                            while (reader.Read())
                            {
                                string time = reader.GetDateTime("timestamp").ToString("yyyy-MM-dd HH:mm:ss");
                                string type = reader.GetString("type");
                                string eqp = reader.GetString("equipment");
                                string msg = reader.GetString("message");

                                dgvLogs.Rows.Add(time, type, eqp, msg);
                            }
                        }
                    }
                }

                // 로드 완료 후 개수 동기화
                lastRowCount = dgvLogs.Rows.Count;

                // [핵심] 스크롤 위치 및 선택 행 복구
                if (silent && dgvLogs.Rows.Count > 0)
                {
                    // 1. 스크롤 위치 복구
                    if (firstVisibleRow >= 0 && firstVisibleRow < dgvLogs.Rows.Count)
                    {
                        dgvLogs.FirstDisplayedScrollingRowIndex = firstVisibleRow;
                    }

                    // 2. 선택 상태 복구
                    if (selectedRowIndex >= 0 && selectedRowIndex < dgvLogs.Rows.Count)
                    {
                        dgvLogs.Rows[selectedRowIndex].Selected = true;
                    }
                    else
                    {
                        dgvLogs.ClearSelection();
                    }
                }
                else if (!silent && dgvLogs.Rows.Count > 0)
                {
                    // 수동 새로고침(버튼/필터) 시에는 맨 위로
                    dgvLogs.FirstDisplayedScrollingRowIndex = 0;
                    dgvLogs.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                if (!silent && !isErrorShown)
                {
                    isErrorShown = true;
                    MessageBox.Show($"DB 연결 실패:\n{ex.Message}", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                AddDummyDataForFallback();
            }
        }

        private void AddDummyDataForFallback()
        {
            if (dgvLogs.Rows.Count > 0) return;
            dgvLogs.Rows.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "System", "PC", "DB Connection Failed - Showing Dummy Data");
        }
    }
}