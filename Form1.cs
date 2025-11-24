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
            InitializeComponent(); // Designer.cs의 코드가 실행됨
            SetupLogic();
        }

        private void SetupLogic()
        {
            // 화면 깜빡임 방지
            this.DoubleBuffered = true;

            // 시계 타이머
            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += (s, e) => {
                // Designer에서 만든 lblTime, lblDate에 접근 가능
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            };
            clockTimer.Start();

            // Paint 이벤트 연결
            pnlCenter.Paint += pnlCenter_Paint;
        }

        // 중앙 그래픽 그리기 로직 (동일함)
        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            Brush chamberBrush = new SolidBrush(Color.FromArgb(220, 220, 220));

            // Robot
            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);
            g.TranslateTransform(cx, cy);
            g.RotateTransform(-30); // 로봇 팔 각도
            g.FillRectangle(Brushes.Gray, -15, -80, 30, 80);
            g.ResetTransform();

            // Signal Tower
            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Gold, lightX, lightY + 30, 30, 30);
            g.FillRectangle(Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);
        }
    }
}