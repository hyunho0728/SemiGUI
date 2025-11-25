using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        private Timer clockTimer;

        public Form1()
        {
            InitializeComponent();

            // 1920x1080 고정
            this.Size = new Size(1920, 1080);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            SetupLogic();

            this.btnMain.Click += BtnMain_Click;
            this.btnRecipe.Click += BtnRecipe_Click;
            this.btnLog.Click += BtnLog_Click;
        }

        private void SetupLogic()
        {
            this.DoubleBuffered = true;
            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += (s, e) => {
                if (lblTime != null) lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                if (lblDate != null) lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            };
            clockTimer.Start();
            pnlCenter.Paint += pnlCenter_Paint;
        }

        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            // 로봇
            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);
            g.TranslateTransform(cx, cy);
            g.RotateTransform(-30);
            g.FillRectangle(Brushes.Gray, -15, -80, 30, 80);
            g.ResetTransform();

            // 신호등
            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Gold, lightX, lightY + 30, 30, 30);
            g.FillRectangle(Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);
        }

        private void BtnMain_Click(object sender, EventArgs e)
        {
            pnlCenter.Invalidate();
        }

        // [수정] Recipe 팝업 띄우기 및 이벤트 연결
        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            Form recipePopup = new Form();
            recipePopup.Text = "Recipe Management";
            recipePopup.Size = new Size(1290, 760); // [수정] 1280x720 컨트롤에 맞는 적절한 윈도우 크기
            recipePopup.StartPosition = FormStartPosition.CenterScreen;

            RecipeControl control = new RecipeControl();
            control.Dock = DockStyle.Fill;

            // [핵심] 레시피 화면에서 Save 누르면 여기로 데이터가 옴 -> 메인 화면 UI 업데이트
            control.ApplyToMainRequested += (s, data) => {
                ApplyRecipeData(data);
            };

            // 취소/닫기 버튼
            control.btnCancel.Click += (s2, e2) => recipePopup.Close();

            recipePopup.Controls.Add(control);
            recipePopup.ShowDialog();
        }

        // [추가] 메인 화면 값 업데이트 메서드
        private void ApplyRecipeData(RecipeControl.RecipeModel data)
        {
            // PM A (Target, Gas, Time)
            if (data.PmA_Params != null)
            {
                valTargetA.Text = data.PmA_Params[0];
                valGasA.Text = data.PmA_Params[1];
                valTimeA.Text = data.PmA_Params[2];
            }

            // PM B (Align, RPM, Time)
            if (data.PmB_Params != null)
            {
                valAlignB.Text = data.PmB_Params[0];
                valRpmB.Text = data.PmB_Params[1];
                valTimeB.Text = data.PmB_Params[2];
            }

            // PM C (Press, Gas, SpinTime)
            if (data.PmC_Params != null)
            {
                valPressC.Text = data.PmC_Params[0];
                valGasC.Text = data.PmC_Params[1];
                valSpinTimeC.Text = data.PmC_Params[2];
            }
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            Form logPopup = new Form();
            logPopup.Text = "System Log";
            logPopup.Size = new Size(1280, 760);
            logPopup.StartPosition = FormStartPosition.CenterScreen;

            LogControl control = new LogControl();
            control.Dock = DockStyle.Fill;

            logPopup.Controls.Add(control);
            logPopup.Show();
        }
    }
}