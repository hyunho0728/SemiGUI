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
            SetupLogic();
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

            // Paint 이벤트 연결 (로봇만 그리기 위해)
            pnlCenter.Paint += pnlCenter_Paint;
        }

        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 중앙 좌표 계산
            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            // ===============================================
            // [중요] 챔버(PM A/B/C)와 FOUP은 이제 '패널' 컨트롤이므로
            // 여기서 그리지 않습니다. 디자이너에서 옮기세요.
            // ===============================================

            // 1. 로봇 그리기 (중앙에서 회전)
            // 로봇 몸체
            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);

            // 로봇 팔 (회전)
            g.TranslateTransform(cx, cy);
            g.RotateTransform(-30); // 팔 각도 (나중에 변수로 변경해서 움직이게 가능)
            g.FillRectangle(Brushes.Gray, -15, -80, 30, 80);
            g.ResetTransform();

            // 2. 신호등 그리기 (우측 상단 고정 - 이것도 패널로 만들면 편하지만 일단 코드로 유지)
            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Gold, lightX, lightY + 30, 30, 30);
            g.FillRectangle(Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);

            // 3. 카세트 줄무늬 (이건 패널 위에 그릴 수 없어서 별도로 Panel을 상속받거나 해야함. 일단 하단에 그림)
            // 위치 잡기 편하게 하단 고정 좌표 대신 화면 비율로 배치
            DrawCassette(g, cx - 100, pnlCenter.Height - 100, Color.Blue);
            DrawCassette(g, cx + 50, pnlCenter.Height - 100, Color.Black);
        }

        private void DrawCassette(Graphics g, int x, int y, Color color)
        {
            g.DrawRectangle(new Pen(Color.Black, 2), x, y, 80, 80);
            for (int i = 0; i < 5; i++)
            {
                g.FillRectangle(new SolidBrush(color), x + 5, y + 10 + (i * 14), 70, 8);
            }
        }
    }
}