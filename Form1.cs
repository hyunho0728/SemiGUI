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

            // Paint 이벤트 연결
            pnlCenter.Paint += pnlCenter_Paint;
        }

        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            Brush chamberBrush = new SolidBrush(Color.FromArgb(220, 220, 220));

            // Chambers
            g.FillRectangle(chamberBrush, cx - 150, cy - 50, 80, 100);
            g.FillRectangle(chamberBrush, cx - 40, cy - 150, 80, 100);
            g.FillRectangle(chamberBrush, cx + 70, cy - 50, 80, 100);

            // LEDs
            g.FillEllipse(Brushes.Green, cx - 140, cy - 40, 20, 20);
            g.FillEllipse(Brushes.Green, cx + 20, cy - 140, 20, 20);
            g.FillEllipse(Brushes.Green, cx + 80, cy + 20, 20, 20);

            // FOUPs
            g.TranslateTransform(cx, cy);
            g.RotateTransform(-45);
            g.FillRectangle(chamberBrush, -50, 120, 80, 80);
            g.RotateTransform(90);
            g.FillRectangle(chamberBrush, -50, 120, 80, 80);
            g.ResetTransform();

            // Robot
            g.FillEllipse(Brushes.LightGray, cx - 60, cy - 60, 120, 120);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 80)), cx - 25, cy - 25, 50, 50);
            g.TranslateTransform(cx, cy);
            g.RotateTransform(-30);
            g.FillRectangle(Brushes.Gray, -15, -80, 30, 80);
            g.ResetTransform();

            // Signal Tower
            int lightX = pnlCenter.Width - 60;
            int lightY = 50;
            g.FillRectangle(Brushes.Red, lightX, lightY, 30, 30);
            g.FillRectangle(Brushes.Gold, lightX, lightY + 30, 30, 30);
            g.FillRectangle(Brushes.Green, lightX, lightY + 60, 30, 30);
            g.DrawRectangle(Pens.Gray, lightX, lightY, 30, 90);

            // Cassettes
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