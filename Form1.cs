using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SemiGUI
{
    public partial class Form1 : Form
    {
        private Timer clockTimer;
        private bool isLoggedIn = false;

        public Form1()
        {
            InitializeComponent();

            // 1920x1080 고정
            this.Size = new Size(1920, 1080);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            SetupLogic();

            // 이벤트 연결
            this.btnMain.Click += BtnMain_Click;
            this.btnRecipe.Click += BtnRecipe_Click;
            this.btnLog.Click += BtnLog_Click;
            this.btnLogin.Click += BtnLogin_Click;

            // [추가] FOUP Load/Unload 버튼 이벤트 연결
            this.btnLoadA.Click += (s, e) => SetFoupState("A", true);
            this.btnUnloadA.Click += (s, e) => SetFoupState("A", false);
            this.btnLoadB.Click += (s, e) => SetFoupState("B", true);
            this.btnUnloadB.Click += (s, e) => SetFoupState("B", false);

            // [핵심] 화면 크기가 변하거나 패널이 보일 때 레이아웃 자동 재정렬
            this.pnlCenter.SizeChanged += (s, e) => UpdateLayout();

            // 초기 상태 설정 (로그아웃 상태, 웨이퍼는 장착된 상태로 시작)
            SetLoginState(false);

            // 초기 버튼 상태 동기화 (이미 웨이퍼가 디자인상 그려져 있으므로 Loaded 상태로 간주)
            SetFoupState("A", true);
            SetFoupState("B", true);
        }

        // [추가] 웨이퍼 장착/해제 로직 구현
        private void SetFoupState(string foupType, bool isLoaded)
        {
            if (foupType == "A")
            {
                // 왼쪽 카세트 (파란색 웨이퍼 L1~L5) 제어
                bool visible = isLoaded;
                pnlWaferL1.Visible = visible;
                pnlWaferL2.Visible = visible;
                pnlWaferL3.Visible = visible;
                pnlWaferL4.Visible = visible;
                pnlWaferL5.Visible = visible;

                // 상태 텍스트 및 버튼 활성화 업데이트
                lblInfoA.Text = isLoaded
                    ? "LPB Status: Busy\nMode: MANUAL\nRun: Loaded"
                    : "LPB Status: Ready\nMode: MANUAL\nRun: Empty";

                // 로드 상태면 Load버튼 비활성, Unload버튼 활성 (반대도 동일)
                btnLoadA.Enabled = !isLoaded;
                btnLoadA.BackColor = isLoaded ? Color.Gray : Color.FromArgb(0, 192, 0); // 시각적 피드백

                btnUnloadA.Enabled = isLoaded;
                btnUnloadA.BackColor = isLoaded ? Color.Red : Color.Gray;
            }
            else // B
            {
                // 오른쪽 카세트 (검정색 웨이퍼 R1~R5) 제어
                bool visible = isLoaded;
                pnlWaferR1.Visible = visible;
                pnlWaferR2.Visible = visible;
                pnlWaferR3.Visible = visible;
                pnlWaferR4.Visible = visible;
                pnlWaferR5.Visible = visible;

                lblInfoB.Text = isLoaded
                    ? "LPB Status: Busy\nMode: MANUAL\nRun: Loaded"
                    : "LPB Status: Ready\nMode: MANUAL\nRun: Empty";

                btnLoadB.Enabled = !isLoaded;
                btnLoadB.BackColor = isLoaded ? Color.Gray : Color.FromArgb(0, 192, 0);

                btnUnloadB.Enabled = isLoaded;
                btnUnloadB.BackColor = isLoaded ? Color.Red : Color.Gray;
            }
        }

        // 챔버와 카세트 위치를 화면 중앙 기준으로 재배치하는 함수
        private void UpdateLayout()
        {
            if (pnlCenter.Width == 0 || pnlCenter.Height == 0) return;

            int cx = pnlCenter.Width / 2;
            int cy = pnlCenter.Height / 2;

            // 1. PM B (Top) - 로봇 위쪽
            pnlChamberB.Location = new Point(cx - 40, cy - 250);

            // 2. PM A (Left) - 로봇 왼쪽
            pnlChamberA.Location = new Point(cx - 250, cy - 50);

            // 3. PM C (Right) - 로봇 오른쪽
            pnlChamberC.Location = new Point(cx + 170, cy - 50);

            // 4. FOUP A (Bottom Left) - 대각선 아래
            pnlFoupA.Location = new Point(cx - 200, cy + 180);

            // 5. FOUP B (Bottom Right) - 대각선 아래
            pnlFoupB.Location = new Point(cx + 120, cy + 180);

            // 6. Cassette L (Bottom Center Left)
            pnlCassetteL.Location = new Point(cx - 100, cy + 330);

            // 7. Cassette R (Bottom Center Right)
            pnlCassetteR.Location = new Point(cx + 20, cy + 330);
        }

        private void SetLoginState(bool login)
        {
            isLoggedIn = login;

            pnlLeft.Visible = login;
            pnlRight.Visible = login;
            pnlCenter.Visible = login;
            pnlBottom.Visible = login;

            txtId.Enabled = !login;
            txtPw.Enabled = !login;
            btnLogin.Text = login ? "LOGOUT" : "LOGIN";

            if (!login)
            {
                txtId.Text = "";
                txtPw.Text = "";
                txtId.Focus();
            }

            if (login) UpdateLayout();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (isLoggedIn)
            {
                if (MessageBox.Show("로그아웃 하시겠습니까?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SetLoginState(false);
                }
                return;
            }

            string id = txtId.Text.Trim();
            string pw = txtPw.Text.Trim();

            if (id == "admin" && pw == "1234")
            {
                MessageBox.Show("로그인 되었습니다.", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetLoginState(true);
            }
            else
            {
                MessageBox.Show("ID 또는 비밀번호가 일치하지 않습니다.\n(admin / 1234)", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPw.Text = "";
                txtPw.Focus();
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
            if (!isLoggedIn) return;

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

        private void BtnMain_Click(object sender, EventArgs e)
        {
            pnlLeft.Visible = true;
            pnlRight.Visible = true;
            pnlCenter.Invalidate();
            UpdateLayout();
        }

        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            Form recipePopup = new Form();
            recipePopup.Text = "Recipe Management";
            recipePopup.Size = new Size(1290, 760);
            recipePopup.StartPosition = FormStartPosition.CenterScreen;

            RecipeControl control = new RecipeControl();
            control.Dock = DockStyle.Fill;

            control.ApplyToMainRequested += (s, data) => {
                ApplyRecipeData(data);
            };

            control.btnCancel.Click += (s2, e2) => recipePopup.Close();

            recipePopup.Controls.Add(control);
            recipePopup.ShowDialog();
        }

        private void ApplyRecipeData(RecipeControl.RecipeModel data)
        {
            if (data.PmA_Params != null)
            {
                valTargetA.Text = data.PmA_Params[0];
                valGasA.Text = data.PmA_Params[1];
                valTimeA.Text = data.PmA_Params[2];
            }
            if (data.PmB_Params != null)
            {
                valAlignB.Text = data.PmB_Params[0];
                valRpmB.Text = data.PmB_Params[1];
                valTimeB.Text = data.PmB_Params[2];
            }
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