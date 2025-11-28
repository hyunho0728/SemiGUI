using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient; // [추가] DB 연동

namespace SemiGUI
{
    public partial class RecipeControl : UserControl
    {
        // [추가] DB 연결 문자열
        private string connectionString = "Server=localhost;Port=3306;Database=SemiGuiData;Uid=root;Pwd=1234;Charset=utf8;";

        public class RecipeModel
        {
            public string Name { get; set; }
            public string[] PmA_Params { get; set; } = new string[] { "0", "0", "0" };
            public string[] PmB_Params { get; set; } = new string[] { "0", "0", "0" };
            public string[] PmC_Params { get; set; } = new string[] { "0", "0", "0" };
        }

        public event EventHandler<RecipeModel> ApplyToMainRequested;
        private List<RecipeModel> recipeList = new List<RecipeModel>();

        public RecipeControl()
        {
            InitializeComponent();
            LoadRecipesFromDB(); // [변경] DB에서 로드
            InitializeEvents();
        }

        // [변경] DB에서 레시피 목록 불러오기
        private void LoadRecipesFromDB()
        {
            recipeList.Clear();
            lstRecipes.Items.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM recipes ORDER BY name ASC";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = new RecipeModel();
                            r.Name = reader.GetString("name");

                            r.PmA_Params[0] = reader.GetString("pmA_target");
                            r.PmA_Params[1] = reader.GetString("pmA_gas");
                            r.PmA_Params[2] = reader.GetString("pmA_time");

                            r.PmB_Params[0] = reader.GetString("pmB_align");
                            r.PmB_Params[1] = reader.GetString("pmB_rpm");
                            r.PmB_Params[2] = reader.GetString("pmB_time");

                            r.PmC_Params[0] = reader.GetString("pmC_press");
                            r.PmC_Params[1] = reader.GetString("pmC_gas");
                            r.PmC_Params[2] = reader.GetString("pmC_time");

                            recipeList.Add(r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"레시피 로드 실패:\n{ex.Message}", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // 리스트박스 갱신
            foreach (var r in recipeList) lstRecipes.Items.Add(r.Name);
            if (lstRecipes.Items.Count > 0) lstRecipes.SelectedIndex = 0;
            else ClearUI();
        }

        private void InitializeEvents()
        {
            lstRecipes.SelectedIndexChanged += (s, e) => {
                if (lstRecipes.SelectedIndex >= 0) UpdateUI(recipeList[lstRecipes.SelectedIndex]);
            };

            // [New] DB에 새 레시피 추가
            btnNew.Click += (s, e) => {
                string inputName = ShowInputDialog("Enter new recipe name:", "Create New Recipe");
                if (!string.IsNullOrWhiteSpace(inputName))
                {
                    // 중복 이름 체크 (메모리상)
                    if (recipeList.Exists(x => x.Name == inputName))
                    {
                        MessageBox.Show("이미 존재하는 이름입니다.", "Warning");
                        return;
                    }

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            // 기본값(0)으로 INSERT
                            string sql = @"INSERT INTO recipes 
                                (name, pmA_target, pmA_gas, pmA_time, pmB_align, pmB_rpm, pmB_time, pmC_press, pmC_gas, pmC_time)
                                VALUES 
                                (@name, '0','0','0', '0','0','0', '0','0','0')";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@name", inputName);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        LoadRecipesFromDB(); // 다시 로드

                        // 방금 만든 항목 선택
                        for (int i = 0; i < lstRecipes.Items.Count; i++)
                        {
                            if (lstRecipes.Items[i].ToString() == inputName)
                            {
                                lstRecipes.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"레시피 생성 실패:\n{ex.Message}", "DB Error");
                    }
                }
            };

            // [Delete] DB에서 삭제
            btnDelete.Click += (s, e) => {
                int idx = lstRecipes.SelectedIndex;
                if (idx < 0) return;

                string targetName = recipeList[idx].Name;
                if (MessageBox.Show($"Delete '{targetName}'?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            string sql = "DELETE FROM recipes WHERE name = @name";
                            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@name", targetName);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        LoadRecipesFromDB();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"삭제 실패:\n{ex.Message}", "DB Error");
                    }
                }
            };

            // [Save] DB 업데이트
            btnSave.Click += (s, e) => {
                int idx = lstRecipes.SelectedIndex;
                if (idx < 0) return;

                // UI -> 메모리 모델 업데이트
                RecipeModel r = recipeList[idx];
                UpdateModelFromUI(r);

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = @"UPDATE recipes SET 
                                        pmA_target=@pa1, pmA_gas=@pa2, pmA_time=@pa3,
                                        pmB_align=@pb1, pmB_rpm=@pb2, pmB_time=@pb3,
                                        pmC_press=@pc1, pmC_gas=@pc2, pmC_time=@pc3
                                       WHERE name=@name";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", r.Name);

                            cmd.Parameters.AddWithValue("@pa1", r.PmA_Params[0]);
                            cmd.Parameters.AddWithValue("@pa2", r.PmA_Params[1]);
                            cmd.Parameters.AddWithValue("@pa3", r.PmA_Params[2]);

                            cmd.Parameters.AddWithValue("@pb1", r.PmB_Params[0]);
                            cmd.Parameters.AddWithValue("@pb2", r.PmB_Params[1]);
                            cmd.Parameters.AddWithValue("@pb3", r.PmB_Params[2]);

                            cmd.Parameters.AddWithValue("@pc1", r.PmC_Params[0]);
                            cmd.Parameters.AddWithValue("@pc2", r.PmC_Params[1]);
                            cmd.Parameters.AddWithValue("@pc3", r.PmC_Params[2]);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show($"[{r.Name}] 저장 완료.", "Save");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"저장 실패:\n{ex.Message}", "DB Error");
                }
            };

            // Import (CSV) - 기존 기능 유지하되, DB에 추가하는 방식은 아니므로 메모리에만 로드하고 저장 시 DB 반영 유도
            btnImport.Click += (s, e) => {
                MessageBox.Show("Import 기능은 DB 연동 버전에서 잠시 비활성화되었습니다.", "Info");
            };

            // Export (CSV) - 기존 기능 유지 (DB 데이터를 CSV로)
            btnExport.Click += (s, e) => {
                int idx = lstRecipes.SelectedIndex;
                if (idx < 0) return;
                RecipeModel r = recipeList[idx];

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV Files (*.csv)|*.csv";
                    sfd.FileName = r.Name;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"RecipeName,{r.Name}");
                        sb.AppendLine($"PM_A,{string.Join(",", r.PmA_Params)}");
                        sb.AppendLine($"PM_B,{string.Join(",", r.PmB_Params)}");
                        sb.AppendLine($"PM_C,{string.Join(",", r.PmC_Params)}");
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Export 완료.", "Export");
                    }
                }
            };

            // [Apply] 메인 화면에 적용 (기존 로직 유지)
            btnApply.Click += (s, e) => {
                RecipeModel currentData = new RecipeModel();
                UpdateModelFromUI(currentData);
                currentData.Name = "Applied_Recipe";

                ApplyToMainRequested?.Invoke(this, currentData);
                MessageBox.Show("설정이 메인 화면에 적용되었습니다.", "Apply");
            };
        }

        private void UpdateUI(RecipeModel r)
        {
            txtTargetA.Text = r.PmA_Params[0]; txtGasA.Text = r.PmA_Params[1]; txtTimeA.Text = r.PmA_Params[2];
            txtAlignB.Text = r.PmB_Params[0]; txtRpmB.Text = r.PmB_Params[1]; txtTimeB.Text = r.PmB_Params[2];
            txtPressC.Text = r.PmC_Params[0]; txtGasC.Text = r.PmC_Params[1]; txtSpinC.Text = r.PmC_Params[2];
        }

        private void UpdateModelFromUI(RecipeModel r)
        {
            r.PmA_Params[0] = txtTargetA.Text; r.PmA_Params[1] = txtGasA.Text; r.PmA_Params[2] = txtTimeA.Text;
            r.PmB_Params[0] = txtAlignB.Text; r.PmB_Params[1] = txtRpmB.Text; r.PmB_Params[2] = txtTimeB.Text;
            r.PmC_Params[0] = txtPressC.Text; r.PmC_Params[1] = txtGasC.Text; r.PmC_Params[2] = txtSpinC.Text;
        }

        private void ClearUI()
        {
            txtTargetA.Clear(); txtGasA.Clear(); txtTimeA.Clear();
            txtAlignB.Clear(); txtRpmB.Clear(); txtTimeB.Clear();
            txtPressC.Clear(); txtGasC.Clear(); txtSpinC.Clear();
        }

        private string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
            Button confirmation = new Button() { Text = "OK", Left = 250, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}