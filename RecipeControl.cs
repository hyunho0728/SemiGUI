using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SemiGUI
{
    public partial class RecipeControl : UserControl
    {
        public class RecipeModel
        {
            public string Name { get; set; }
            public string[] PmA_Params { get; set; } = new string[3];
            public string[] PmB_Params { get; set; } = new string[3];
            public string[] PmC_Params { get; set; } = new string[3];
            // Sequence 리스트는 UI에서 제거되었지만 데이터 모델에는 남겨둘 수 있습니다.
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
            // Recipe 1
            var r1 = new RecipeModel
            {
                Name = "Standard_Process",
                PmA_Params = new string[] { "1000.0", "500", "60" },
                PmB_Params = new string[] { "0.001", "3000", "45" },
                PmC_Params = new string[] { "15", "100", "120" }
            };
            recipeList.Add(r1);

            // Recipe 2
            var r2 = new RecipeModel
            {
                Name = "High_Temp_Fast",
                PmA_Params = new string[] { "1200.0", "800", "30" },
                PmB_Params = new string[] { "0.002", "4000", "30" },
                PmC_Params = new string[] { "20", "200", "90" }
            };
            recipeList.Add(r2);

            RefreshList();
        }

        private void RefreshList()
        {
            lstRecipes.Items.Clear();
            foreach (var r in recipeList)
            {
                lstRecipes.Items.Add(r.Name);
            }
        }

        private void InitializeEvents()
        {
            lstRecipes.SelectedIndexChanged += (s, e) => {
                if (lstRecipes.SelectedIndex >= 0)
                {
                    UpdateUI(recipeList[lstRecipes.SelectedIndex]);
                }
            };

            btnSave.Click += (s, e) => {
                RecipeModel currentData = new RecipeModel();
                currentData.Name = "User_Custom_Setting";

                currentData.PmA_Params[0] = txtTargetA.Text;
                currentData.PmA_Params[1] = txtGasA.Text;
                currentData.PmA_Params[2] = txtTimeA.Text;

                currentData.PmB_Params[0] = txtAlignB.Text;
                currentData.PmB_Params[1] = txtRpmB.Text;
                currentData.PmB_Params[2] = txtTimeB.Text;

                currentData.PmC_Params[0] = txtPressC.Text;
                currentData.PmC_Params[1] = txtGasC.Text;
                currentData.PmC_Params[2] = txtSpinC.Text;

                ApplyToMainRequested?.Invoke(this, currentData);
                MessageBox.Show("메인 화면에 설정값이 적용되었습니다.", "System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnApply.Click += (s, e) => btnSave.PerformClick();
        }

        private void UpdateUI(RecipeModel r)
        {
            // PM A
            txtTargetA.Text = r.PmA_Params[0];
            txtGasA.Text = r.PmA_Params[1];
            txtTimeA.Text = r.PmA_Params[2];

            // PM B
            txtAlignB.Text = r.PmB_Params[0];
            txtRpmB.Text = r.PmB_Params[1];
            txtTimeB.Text = r.PmB_Params[2];

            // PM C
            txtPressC.Text = r.PmC_Params[0];
            txtGasC.Text = r.PmC_Params[1];
            txtSpinC.Text = r.PmC_Params[2];

            // [수정] 그리드 관련 코드는 삭제됨
        }
    }
}