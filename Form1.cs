using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        private Timer clockTimer;

        // 화면 컨트롤 인스턴스
        private UtilityControl utilityView;
        private RecipeControl recipeView;

        public Form1()
        {
            InitializeComponent();
            SetupLogic();

            // 1. 유틸리티 화면 생성
            utilityView = new UtilityControl();
            utilityView.Dock = DockStyle.Fill;
            utilityView.Visible = false;
            this.pnlCenter.Controls.Add(utilityView);

            // 2. 레시피 화면 생성
            recipeView = new RecipeControl();
            recipeView.Dock = DockStyle.Fill;
            recipeView.Visible = false;
            this.pnlCenter.Controls.Add(recipeView);

            // 3. 이벤트 연결
            this.btnMain.Click += BtnMain_Click;
            this.btnUtility.Click += BtnUtility_Click;

            utilityView.btnRecipe.Click += BtnRecipe_Click;

            recipeView.btnCancel.Click += (s, e) => {
                recipeView.Visible = false;
                utilityView.Visible = true; // Utility로 복귀
            };
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
            // 유틸리티나 레시피 화면이 떠있으면 로봇 그리기 중단
            if (utilityView.Visible || recipeView.Visible) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            // 로봇 그리기
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

        // MAIN 버튼 클릭
        private void BtnMain_Click(object sender, EventArgs e)
        {
            utilityView.Visible = false;
            recipeView.Visible = false; // [추가] 레시피 숨김

            pnlLeft.Visible = true;
            pnlRight.Visible = true;
            pnlCenter.Invalidate();
        }

        // UTILITY 버튼 클릭
        private void BtnUtility_Click(object sender, EventArgs e)
        {
            pnlLeft.Visible = false;
            pnlRight.Visible = false;
            recipeView.Visible = false; // [추가] 레시피 숨김

            utilityView.Visible = true;
            utilityView.BringToFront();
        }

        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            // 좌우 패널은 이미 숨겨진 상태일 것이지만 안전하게 다시 숨김
            pnlLeft.Visible = false;
            pnlRight.Visible = false;

            utilityView.Visible = false; // 유틸리티 숨기고
            recipeView.Visible = true;   // 레시피 보이기
            recipeView.BringToFront();
        }
    }
}