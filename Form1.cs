using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        string connectDB = "Server=localhost;Port=3306;Uid=root;Pwd=1234;Charset=utf8;";
        private Timer clockTimer;

        // 화면 컨트롤 인스턴스
        private UtilityControl utilityView;
        private RecipeControl recipeView;
        private LogControl logView;

        public Form1()
        {
            InitializeComponent();
            SetupLogic();
            InitializeDB();

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

            // 3. 로그 화면 생성
            logView = new LogControl();
            logView.Dock = DockStyle.Fill;
            logView.Visible = false;
            this.pnlCenter.Controls.Add(logView);

            // 4. 이벤트 연결
            this.btnMain.Click += BtnMain_Click;
            this.btnUtility.Click += BtnUtility_Click;
            this.btnLog.Click += BtnLog_Click;

            utilityView.btnRecipe.Click += BtnRecipe_Click;

            recipeView.btnCancel.Click += (s, e) => {
                recipeView.Visible = false;
                utilityView.Visible = true; // Utility로 복귀
            };
        }

        private void InitializeDB()
        {
            using (MySqlConnection conn = new MySqlConnection(connectDB))
            {
                bool inputDummy = true; // 더미 데이터 입력 여부

                try
                {
                    List<string> query = new List<string>();

                    query.Add("CREATE DATABASE IF NOT EXISTS `SemiGuiData`");
                    query.Add("USE `SemiGuiData`");
                    query.Add(@"CREATE TABLE IF NOT EXISTS logs (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                timestamp DATETIME NOT NULL,
                                type VARCHAR(20) NOT NULL,      -- 'Alarm', 'Warning', 'Event'
                                equipment VARCHAR(50) NOT NULL, -- 'PM A', 'PM B', etc.
                                message TEXT NOT NULL
                                );");

                    if (inputDummy)
                    {
                        query.Add(@"INSERT INTO logs (timestamp, type, equipment, message) VALUES 
                                    (NOW(), 'Event', 'System', 'System Initialized'),
                                    (DATE_SUB(NOW(), INTERVAL 10 MINUTE), 'Alarm', 'PM A', 'Temperature sensor timeout'),
                                    (DATE_SUB(NOW(), INTERVAL 30 MINUTE), 'Warning', 'PM B', 'Vacuum pressure unstable'),
                                    (DATE_SUB(NOW(), INTERVAL 1 HOUR), 'Event', 'PM C', 'Process Started (Recipe_001)'),
                                    (DATE_SUB(NOW(), INTERVAL 2 HOUR), 'Event', 'System', 'User (Admin) Logged In');");
                    }

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    MySqlCommand vcmd = new MySqlCommand("select version();", conn);
                    string version = vcmd.ExecuteScalar().ToString();

                    MessageBox.Show("현재 MySQL 버전: " + version);

                    foreach (string q in query)
                    {
                        cmd.CommandText = q;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DB 연결 실패: " + ex.Message);
                }
            }
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
            if (utilityView.Visible || recipeView.Visible || logView.Visible) return;

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
            recipeView.Visible = false;
            logView.Visible = false;

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
            logView.Visible = false;

            utilityView.Visible = true;
            utilityView.BringToFront();
        }

        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            pnlLeft.Visible = false;
            pnlRight.Visible = false;
            utilityView.Visible = false;
            logView.Visible = false;

            recipeView.Visible = true;   // 레시피 보이기
            recipeView.BringToFront();
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            pnlLeft.Visible = false;
            pnlRight.Visible = false;
            utilityView.Visible = false;
            recipeView.Visible = false;

            logView.Visible = true;
            logView.BringToFront();
        }
    }
}