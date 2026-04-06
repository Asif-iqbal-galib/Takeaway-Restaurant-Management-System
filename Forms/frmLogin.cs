using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmLogin : Form
    {
        // Controls
        private Panel panelLeft;
        private Panel panelRight;
        private Label lblTitle;
        private Label lblSubtitle;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;
        private Label lblForgotPassword;
        private Label lblFooter;
        private PictureBox pictureBoxLogo;

        public frmLogin()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Appeliano Restaurant";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Padding = new Padding(2);

            // Left Panel (Branding Section)
            panelLeft = new Panel();
            panelLeft.Size = new Size(500, this.Height);
            panelLeft.Location = new Point(0, 0);
            panelLeft.BackColor = Color.FromArgb(44, 62, 80);
            panelLeft.Paint += PanelLeft_Paint;
            this.Controls.Add(panelLeft);

            // Logo
            pictureBoxLogo = new PictureBox();
            pictureBoxLogo.Size = new Size(120, 120);
            pictureBoxLogo.Location = new Point(190, 120);
            pictureBoxLogo.BackColor = Color.Transparent;
            pictureBoxLogo.BackgroundImageLayout = ImageLayout.Stretch;
            // Add your logo: pictureBoxLogo.Image = Image.FromFile("logo.png");
            panelLeft.Controls.Add(pictureBoxLogo);

            // Brand Name
            Label lblBrandName = new Label();
            lblBrandName.Text = "APPELIANO";
            lblBrandName.Font = new Font("Poppins", 32, FontStyle.Bold);
            lblBrandName.ForeColor = Color.FromArgb(255, 102, 0);
            lblBrandName.Location = new Point(120, 260);
            lblBrandName.Size = new Size(280, 55);
            lblBrandName.TextAlign = ContentAlignment.MiddleCenter;
            panelLeft.Controls.Add(lblBrandName);

            // Tagline
            Label lblTagline = new Label();
            lblTagline.Text = "Authentic Italian Taste";
            lblTagline.Font = new Font("Segoe UI", 14, FontStyle.Italic);
            lblTagline.ForeColor = Color.FromArgb(220, 220, 220);
            lblTagline.Location = new Point(140, 315);
            lblTagline.Size = new Size(240, 30);
            lblTagline.TextAlign = ContentAlignment.MiddleCenter;
            panelLeft.Controls.Add(lblTagline);

            // Features List
            Label lblFeature1 = new Label();
            lblFeature1.Text = "✓ Premium Quality Ingredients";
            lblFeature1.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblFeature1.ForeColor = Color.FromArgb(180, 180, 180);
            lblFeature1.Location = new Point(100, 380);
            lblFeature1.Size = new Size(300, 25);
            panelLeft.Controls.Add(lblFeature1);

            Label lblFeature2 = new Label();
            lblFeature2.Text = "✓ Authentic Italian Recipes";
            lblFeature2.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblFeature2.ForeColor = Color.FromArgb(180, 180, 180);
            lblFeature2.Location = new Point(100, 410);
            lblFeature2.Size = new Size(300, 25);
            panelLeft.Controls.Add(lblFeature2);

            Label lblFeature3 = new Label();
            lblFeature3.Text = "✓ Fresh Daily";
            lblFeature3.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblFeature3.ForeColor = Color.FromArgb(180, 180, 180);
            lblFeature3.Location = new Point(100, 440);
            lblFeature3.Size = new Size(300, 25);
            panelLeft.Controls.Add(lblFeature3);

            Label lblFeature4 = new Label();
            lblFeature4.Text = "✓ Fast Delivery";
            lblFeature4.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblFeature4.ForeColor = Color.FromArgb(180, 180, 180);
            lblFeature4.Location = new Point(100, 470);
            lblFeature4.Size = new Size(300, 25);
            panelLeft.Controls.Add(lblFeature4);

            // Right Panel (Login Form)
            panelRight = new Panel();
            panelRight.Size = new Size(600, this.Height);
            panelRight.Location = new Point(500, 0);
            panelRight.BackColor = Color.White;
            panelRight.Padding = new Padding(60, 80, 60, 40);
            this.Controls.Add(panelRight);

            // Welcome Title
            lblTitle = new Label();
            lblTitle.Text = "Welcome Back";
            lblTitle.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitle.Location = new Point(0, 30);
            lblTitle.Size = new Size(480, 55);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            panelRight.Controls.Add(lblTitle);

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Please sign in to continue";
            lblSubtitle.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(120, 120, 120);
            lblSubtitle.Location = new Point(0, 85);
            lblSubtitle.Size = new Size(480, 30);
            lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            panelRight.Controls.Add(lblSubtitle);

            // Username Field
            Label lblUsername = new Label();
            lblUsername.Text = "Username";
            lblUsername.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblUsername.ForeColor = Color.FromArgb(80, 80, 80);
            lblUsername.Location = new Point(0, 160);
            lblUsername.Size = new Size(80, 25);
            panelRight.Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(0, 188);
            txtUsername.Size = new Size(480, 45);
            txtUsername.Font = new Font("Segoe UI", 12);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.BackColor = Color.FromArgb(248, 249, 250);
            txtUsername.ForeColor = Color.FromArgb(51, 51, 51);
            txtUsername.Padding = new Padding(12, 8, 12, 8);
            panelRight.Controls.Add(txtUsername);

            // Password Field
            Label lblPassword = new Label();
            lblPassword.Text = "Password";
            lblPassword.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblPassword.ForeColor = Color.FromArgb(80, 80, 80);
            lblPassword.Location = new Point(0, 255);
            lblPassword.Size = new Size(80, 25);
            panelRight.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(0, 283);
            txtPassword.Size = new Size(480, 45);
            txtPassword.Font = new Font("Segoe UI", 12);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BackColor = Color.FromArgb(248, 249, 250);
            txtPassword.Padding = new Padding(12, 8, 12, 8);
            txtPassword.PasswordChar = '●';
            txtPassword.ForeColor = Color.FromArgb(51, 51, 51);
            panelRight.Controls.Add(txtPassword);

            // Forgot Password Link
            lblForgotPassword = new Label();
            lblForgotPassword.Text = "Forgot password?";
            lblForgotPassword.Font = new Font("Segoe UI", 9, FontStyle.Underline);
            lblForgotPassword.ForeColor = Color.FromArgb(255, 102, 0);
            lblForgotPassword.Location = new Point(380, 340);
            lblForgotPassword.Size = new Size(100, 25);
            lblForgotPassword.Cursor = Cursors.Hand;
            lblForgotPassword.TextAlign = ContentAlignment.MiddleRight;
            lblForgotPassword.Click += LblForgotPassword_Click;
            panelRight.Controls.Add(lblForgotPassword);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "SIGN IN";
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.BackColor = Color.FromArgb(255, 102, 0);
            btnLogin.Size = new Size(480, 50);
            btnLogin.Location = new Point(0, 390);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(230, 92, 0);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(255, 102, 0);
            panelRight.Controls.Add(btnLogin);

            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            btnCancel.ForeColor = Color.FromArgb(100, 100, 100);
            btnCancel.BackColor = Color.White;
            btnCancel.Size = new Size(480, 45);
            btnCancel.Location = new Point(0, 455);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += BtnCancel_Click;
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = Color.FromArgb(248, 248, 248);
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = Color.White;
            panelRight.Controls.Add(btnCancel);

            // Footer
            lblFooter = new Label();
            lblFooter.Text = "© 2024 Appeliano Restaurant. All rights reserved.";
            lblFooter.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblFooter.ForeColor = Color.FromArgb(180, 180, 180);
            lblFooter.Location = new Point(100, 530);
            lblFooter.Size = new Size(320, 20);
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            panelRight.Controls.Add(lblFooter);
        }

        private void PanelLeft_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = panelLeft.ClientRectangle;

            // Gradient Background
            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                Color.FromArgb(44, 62, 80),
                Color.FromArgb(52, 73, 94),
                135f))
            {
                g.FillRectangle(brush, rect);
            }

            // Add subtle pattern (cross lines)
            using (Pen pen = new Pen(Color.FromArgb(30, 255, 255, 255), 1))
            {
                for (int x = 0; x < rect.Width; x += 40)
                {
                    g.DrawLine(pen, x, 0, x + 20, rect.Height);
                }
                for (int y = 0; y < rect.Height; y += 40)
                {
                    g.DrawLine(pen, 0, y, rect.Width, y + 20);
                }
            }

            // Add decorative circle
            using (Pen pen = new Pen(Color.FromArgb(80, 255, 255, 255), 2))
            {
                g.DrawEllipse(pen, rect.Width - 80, rect.Height - 80, 60, 60);
            }
        }

        private void LblForgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your system administrator to reset your password.\n\nEmail: admin@appeliano.com\nPhone: 020 1234 5678",
                "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnLogin.Text = "SIGNING IN...";
                btnLogin.Enabled = false;
                Application.DoEvents();

                Staff authenticatedUser = DatabaseManager.Instance.AuthenticateStaff(username, password);

                if (authenticatedUser != null)
                {
                    CurrentUser.UserID = authenticatedUser.StaffID;
                    CurrentUser.Username = authenticatedUser.Username;
                    CurrentUser.FullName = authenticatedUser.FullName;
                    CurrentUser.Role = authenticatedUser.Role;

                    MessageBox.Show($"Welcome to Appeliano, {authenticatedUser.FullName}!",
                        "Login Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    frmMainDashboard dashboard = new frmMainDashboard(authenticatedUser);
                    dashboard.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.",
                        "Login Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}\n\nPlease check your connection.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Text = "SIGN IN";
                btnLogin.Enabled = true;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtUsername.Focus();

#if DEBUG
            txtUsername.Text = "admin123";
            txtPassword.Text = "admin123";
#endif
        }

        // Add rounded corners to the form
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundedRectangle(rect, 20))
            {
                this.Region = new Region(path);
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}