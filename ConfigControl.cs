using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SemiGUI
{
    public partial class ConfigControl : UserControl
    {
        private string connectionString = "Server=localhost;Port=3306;Database=SemiGuiData;Uid=root;Pwd=1234;Charset=utf8;";

        // 메인 폼에 변경 사항을 알리기 위한 이벤트
        public event EventHandler ConfigSaved;

        public ConfigControl()
        {
            InitializeComponent();
            LoadConfigFromDB();

            this.btnSave.Click += BtnSave_Click;
        }

        private void LoadConfigFromDB()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT cfg_key, cfg_value FROM sys_config";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string key = reader.GetString("cfg_key");
                            string val = reader.GetString("cfg_value");

                            if (key == "RobotSpeed") numRobotSpeed.Value = decimal.Parse(val);
                            if (key == "TimerInterval") numInterval.Value = decimal.Parse(val);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 로드 실패: {ex.Message}", "Error");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Insert or Update (ON DUPLICATE KEY UPDATE)
                    string sql = @"INSERT INTO sys_config (cfg_key, cfg_value) VALUES (@k1, @v1), (@k2, @v2)
                                   ON DUPLICATE KEY UPDATE cfg_value = VALUES(cfg_value)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@k1", "RobotSpeed");
                        cmd.Parameters.AddWithValue("@v1", numRobotSpeed.Value.ToString());
                        cmd.Parameters.AddWithValue("@k2", "TimerInterval");
                        cmd.Parameters.AddWithValue("@v2", numInterval.Value.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("설정이 저장되었습니다.", "Success");

                // 메인 폼에 알림
                ConfigSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 저장 실패: {ex.Message}", "Error");
            }
        }
    }
}