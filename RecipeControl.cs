using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace SemiGUI
{
    public partial class RecipeControl : UserControl
    {
        public class RecipeModel
        {
            public string Name { get; set; }
            public string[] PmA_Params { get; set; } = new string[] { "0", "0", "0" };
            public string[] PmB_Params { get; set; } = new string[] { "0", "0", "0" };
            public string[] PmC_Params { get; set; } = new string[] { "0", "0", "0" };
            public List<string[]> Sequence { get; set; } = new List<string[]>();
        }

        public event EventHandler<RecipeModel> ApplyToMainRequested;
        private List<RecipeModel> recipeList = new List<RecipeModel>();

        public RecipeControl()
        {
            InitializeComponent();
            LoadDummyRecipes();
            InitializeEvents();
        }

        private void LoadDummyRecipes()
        {
            var r1 = new RecipeModel
            {
                Name = "Standard_Process",
                PmA_Params = new string[] { "1000.0", "500", "60" },
                PmB_Params = new string[] { "0.001", "3000", "45" },
                PmC_Params = new string[] { "15", "100", "120" }
            };
            recipeList.Add(r1);

            var r2 = new RecipeModel
            {
                Name = "High_Temp_Fast",
                PmA_Params = new string[] { "1200.0", "800", "30" },
                PmB_Params = new string[] { "0.002", "4000", "30" },
                PmC_Params = new string[] { "20", "200", "90" }
            };
            recipeList.Add(r2);

            RefreshList();
            if (lstRecipes.Items.Count > 0) lstRecipes.SelectedIndex = 0;
        }

        private void RefreshList()
        {
            lstRecipes.Items.Clear();
            foreach (var r in recipeList) lstRecipes.Items.Add(r.Name);
        }

        private void InitializeEvents()
        {
            lstRecipes.SelectedIndexChanged += (s, e) => {
                if (lstRecipes.SelectedIndex >= 0) UpdateUI(recipeList[lstRecipes.SelectedIndex]);
            };

            btnNew.Click += (s, e) => {
                string inputName = ShowInputDialog("Enter new recipe name:", "Create New Recipe");
                if (!string.IsNullOrWhiteSpace(inputName))
                {
                    var newRecipe = new RecipeModel { Name = inputName };
                    recipeList.Add(newRecipe);
                    RefreshList();
                    lstRecipes.SelectedIndex = recipeList.Count - 1;
                }
            };

            btnDelete.Click += (s, e) => {
                int idx = lstRecipes.SelectedIndex;
                if (idx < 0) return;
                if (MessageBox.Show($"Delete '{recipeList[idx].Name}'?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    recipeList.RemoveAt(idx);
                    RefreshList();
                    if (recipeList.Count > 0) lstRecipes.SelectedIndex = (idx >= recipeList.Count) ? recipeList.Count - 1 : idx;
                    else ClearUI();
                }
            };

            // Import
            btnImport.Click += (s, e) => {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                        var newRecipe = new RecipeModel { Name = fileName + "_Imported" };
                        newRecipe.PmA_Params = new string[] { "100", "100", "10" }; // Dummy Logic
                        recipeList.Add(newRecipe);
                        RefreshList();
                        lstRecipes.SelectedIndex = recipeList.Count - 1;
                        MessageBox.Show("Recipe imported successfully.", "Import");
                    }
                }
            };

            // Export
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
                        MessageBox.Show("Recipe exported successfully.", "Export");
                    }
                }
            };

            // [Save] 저장만 (메인 적용 X)
            btnSave.Click += (s, e) => {
                int idx = lstRecipes.SelectedIndex;
                if (idx < 0) return;
                UpdateModelFromUI(recipeList[idx]);
                MessageBox.Show($"[{recipeList[idx].Name}] Saved.", "Save");
            };

            // [Apply] 메인 화면에 적용
            btnApply.Click += (s, e) => {
                RecipeModel currentData = new RecipeModel();
                UpdateModelFromUI(currentData);
                currentData.Name = "Applied_Recipe";

                ApplyToMainRequested?.Invoke(this, currentData);
                MessageBox.Show("Settings applied to Main Screen.", "Apply");
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