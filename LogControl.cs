using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO; // 파일 저장을 위해 추가
using MySql.Data.MySqlClient;

namespace SemiGUI
{
    public partial class LogControl : UserControl
    {
        private string connectionString = "Server=localhost;Port=3306;Database=SemiGuiData;Uid=root;Pwd=1234;Charset=utf8;";
        private const string DEFAULT_SEARCH_TEXT = "Search Message...";
        private bool isErrorShown = false;

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
            this.btnRefresh.Click += (s, e) => LoadLogsFromDB();

            // [추가] Export 버튼 이벤트 연결
            this.btnExport.Click += BtnExport_Click;

            // 필터 변경 이벤트들
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

            this.tmrRefresh.Tick += (s, e) => {
                LoadLogsFromDB(silent: true);
            };
        }

        // [추가] Export 버튼 클릭 핸들러
        private void BtnExport_Click(object sender, EventArgs e)
        {
            // 데이터가 없으면 알림
            if (dgvLogs.Rows.Count == 0)
            {
                MessageBox.Show("저장할 데이터가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1. 저장 팝업 설정
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "로그 데이터 내보내기";
                sfd.Filter = "텍스트 파일 (*.txt)|*.txt|CSV 파일 (*.csv)|*.csv"; // TXT, CSV 선택 가능
                sfd.FileName = $"LogExport_{DateTime.Now:yyyyMMdd_HHmmss}"; // 기본 파일명

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool timerWasRunning = tmrRefresh.Enabled;
                    tmrRefresh.Stop(); // 저장 중 데이터 변경 방지

                    try
                    {
                        // 2. 파일 쓰기
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            string ext = Path.GetExtension(sfd.FileName).ToLower();
                            string separator = (ext == ".csv") ? "," : "\t"; // CSV는 쉼표, TXT는 탭 구분

                            // (1) 헤더 작성
                            for (int i = 0; i < dgvLogs.Columns.Count; i++)
                            {
                                sw.Write(dgvLogs.Columns[i].HeaderText);
                                if (i < dgvLogs.Columns.Count - 1) sw.Write(separator);
                            }
                            sw.WriteLine();

                            // (2) 데이터 작성
                            foreach (DataGridViewRow row in dgvLogs.Rows)
                            {
                                if (row.IsNewRow) continue;

                                for (int i = 0; i < dgvLogs.Columns.Count; i++)
                                {
                                    string value = row.Cells[i].Value?.ToString() ?? "";

                                    // CSV일 경우 데이터 내 쉼표(,) 처리 (따옴표로 감쌈)
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
                        // 저장 후 타이머 복구
                        if (timerWasRunning) tmrRefresh.Start();
                    }
                }
            }
        }

        private void LoadLogsFromDB(bool silent = false)
        {
            if (cboStartHour.SelectedItem == null || cboStartMin.SelectedItem == null ||
                cboEndHour.SelectedItem == null || cboEndMin.SelectedItem == null) return;

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

                    query.Append("ORDER BY timestamp DESC");

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