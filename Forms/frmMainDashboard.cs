using System;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmMainDashboard : Form
    {
        // Controls
        private Label lblWelcome;
        private Label lblDateTime;
        private Panel panelMenu;
        private Panel panelContent;
        private Button btnMenuManagement;
        private Button btnTakeOrder;
        private Button btnKitchenView;
        private Button btnCustomers;
        private Button btnInventory;
        private Button btnReports;
        private Button btnStaff;
        private Button btnDelivery;
        private Button btnSettings;
        private Button btnAIChatbot;
        private Button btnLogout;
        private Label lblStatus;
        private Timer timer;

        // Current user
        private Staff currentUser;

        public frmMainDashboard(Staff user = null)
        {
            currentUser = user;
            InitializeComponent();
            LoadUserInfo();
            StartTimer();
            LoadDashboardStats();
        }

        private void InitializeComponent()
        {
            // Increased form height from 700 to 850 to fit all buttons
            this.Text = "Main Dashboard - Appeliano Restaurant";
            this.Size = new Size(1200, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblWelcome = new Label();
            lblWelcome.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(52, 73, 94);
            lblWelcome.Location = new Point(20, 20);
            lblWelcome.Size = new Size(600, 40);
            this.Controls.Add(lblWelcome);

            lblDateTime = new Label();
            lblDateTime.Font = new Font("Segoe UI", 10);
            lblDateTime.ForeColor = Color.Gray;
            lblDateTime.Location = new Point(20, 60);
            lblDateTime.Size = new Size(400, 25);
            this.Controls.Add(lblDateTime);

            lblStatus = new Label();
            lblStatus.Font = new Font("Segoe UI", 9);
            lblStatus.ForeColor = Color.FromArgb(100, 100, 100);
            lblStatus.Location = new Point(20, 780);
            lblStatus.Size = new Size(400, 25);
            this.Controls.Add(lblStatus);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            // Increased panel height from 520 to 700
            panelMenu = new Panel();
            panelMenu.Location = new Point(20, 100);
            panelMenu.Size = new Size(250, 700);
            panelMenu.BackColor = Color.FromArgb(52, 73, 94);
            panelMenu.BorderStyle = BorderStyle.None;
            panelMenu.AutoScroll = true; // Added AutoScroll in case buttons overflow
            this.Controls.Add(panelMenu);

            panelContent = new Panel();
            panelContent.Location = new Point(290, 100);
            panelContent.Size = new Size(880, 700);
            panelContent.BackColor = Color.White;
            panelContent.BorderStyle = BorderStyle.FixedSingle;
            panelContent.AutoScroll = true;
            this.Controls.Add(panelContent);

            int yPos = 20;
            int btnHeight = 45;
            int btnSpacing = 5;

            btnMenuManagement = CreateMenuButton("📋 Menu Management", yPos);
            btnMenuManagement.Click += BtnMenuManagement_Click;
            yPos += btnHeight + btnSpacing;

            btnTakeOrder = CreateMenuButton("🛒 Take Order", yPos);
            btnTakeOrder.Click += BtnTakeOrder_Click;
            yPos += btnHeight + btnSpacing;

            btnKitchenView = CreateMenuButton("👨‍🍳 Kitchen View", yPos);
            btnKitchenView.Click += BtnKitchenView_Click;
            yPos += btnHeight + btnSpacing;

            btnCustomers = CreateMenuButton("👥 Customers", yPos);
            btnCustomers.Click += BtnCustomers_Click;
            yPos += btnHeight + btnSpacing;

            btnInventory = CreateMenuButton("📦 Inventory", yPos);
            btnInventory.Click += BtnInventory_Click;
            yPos += btnHeight + btnSpacing;

            btnReports = CreateMenuButton("📊 Reports", yPos);
            btnReports.Click += BtnReports_Click;
            yPos += btnHeight + btnSpacing;

            btnStaff = CreateMenuButton("👤 Staff Management", yPos);
            btnStaff.Click += BtnStaff_Click;
            yPos += btnHeight + btnSpacing;

            btnDelivery = CreateMenuButton("🚚 Delivery", yPos);
            btnDelivery.Click += BtnDelivery_Click;
            yPos += btnHeight + btnSpacing;

            btnSettings = CreateMenuButton("⚙ Settings", yPos);
            btnSettings.Click += BtnSettings_Click;
            yPos += btnHeight + btnSpacing;

            btnAIChatbot = CreateMenuButton("🤖 AI Assistant", yPos);
            btnAIChatbot.Click += BtnAIChatbot_Click;
            yPos += btnHeight + btnSpacing;

            btnLogout = CreateMenuButton("🚪 Logout", yPos);
            btnLogout.BackColor = Color.FromArgb(231, 76, 60);
            btnLogout.Click += BtnLogout_Click;

            panelMenu.Controls.AddRange(new Control[] {
                btnMenuManagement, btnTakeOrder, btnKitchenView, btnCustomers,
                btnInventory, btnReports, btnStaff, btnDelivery, btnSettings, btnAIChatbot, btnLogout
            });

            ConfigureRoleBasedAccess();
        }

        private Button CreateMenuButton(string text, int yPos)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.Transparent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0);
            btn.Size = new Size(panelMenu.Width - 20, 40);
            btn.Location = new Point(10, yPos);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(64, 87, 109);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.Transparent;

            return btn;
        }

        private void ConfigureRoleBasedAccess()
        {
            if (currentUser == null) return;

            // Admin sees everything
            if (currentUser.Role == "Admin")
            {
                btnMenuManagement.Visible = true;
                btnTakeOrder.Visible = true;
                btnKitchenView.Visible = true;
                btnCustomers.Visible = true;
                btnInventory.Visible = true;
                btnReports.Visible = true;
                btnStaff.Visible = true;
                btnDelivery.Visible = true;
                btnSettings.Visible = true;
                btnAIChatbot.Visible = true;
                btnLogout.Visible = true;
            }
            // Cashier Role
            else if (currentUser.Role == "Cashier")
            {
                btnMenuManagement.Visible = false;
                btnTakeOrder.Visible = true;
                btnKitchenView.Visible = true;
                btnCustomers.Visible = true;
                btnInventory.Visible = false;
                btnReports.Visible = false;
                btnStaff.Visible = false;
                btnDelivery.Visible = true;
                btnSettings.Visible = false;
                btnAIChatbot.Visible = true;
                btnLogout.Visible = true;
            }
            // Kitchen Staff Role
            else if (currentUser.Role == "Kitchen")
            {
                btnMenuManagement.Visible = false;
                btnTakeOrder.Visible = false;
                btnKitchenView.Visible = true;
                btnCustomers.Visible = false;
                btnInventory.Visible = false;
                btnReports.Visible = false;
                btnStaff.Visible = false;
                btnDelivery.Visible = false;
                btnSettings.Visible = false;
                btnAIChatbot.Visible = false;
                btnLogout.Visible = true;
            }
            // Delivery Staff Role
            else if (currentUser.Role == "Delivery")
            {
                btnMenuManagement.Visible = false;
                btnTakeOrder.Visible = false;
                btnKitchenView.Visible = false;
                btnCustomers.Visible = false;
                btnInventory.Visible = false;
                btnReports.Visible = false;
                btnStaff.Visible = false;
                btnDelivery.Visible = true;
                btnSettings.Visible = false;
                btnAIChatbot.Visible = false;
                btnLogout.Visible = true;
            }
        }

        private void LoadUserInfo()
        {
            if (currentUser != null)
            {
                lblWelcome.Text = $"Welcome, {currentUser.FullName}!";
            }
            else
            {
                lblWelcome.Text = "Welcome, Guest!";
            }
        }

        private void StartTimer()
        {
            Timer_Tick(null, null);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - hh:mm:ss tt");
        }

        public void LoadDashboardStats()
        {
            panelContent.Controls.Clear();

            Label lblTitle = new Label();
            lblTitle.Text = "Dashboard Overview";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 40);
            panelContent.Controls.Add(lblTitle);

            try
            {
                lblStatus.Text = "🔄 Loading stats...";
                Application.DoEvents();

                int todayOrders = DatabaseManager.Instance.GetTodayOrdersCount();
                decimal todayRevenue = DatabaseManager.Instance.GetTodayRevenue();
                int pendingOrders = DatabaseManager.Instance.GetPendingOrdersCount();
                var menuItems = DatabaseManager.Instance.GetMenuItems();
                int menuCount = menuItems?.Count ?? 0;

                // Using £ (pound) currency symbol
                CreateStatCard("📊 Today's Orders", todayOrders.ToString(), new Point(20, 80), Color.FromArgb(52, 152, 219));
                CreateStatCard("💰 Today's Revenue", $"£{todayRevenue:F2}", new Point(320, 80), Color.FromArgb(46, 204, 113));
                CreateStatCard("🍽️ Menu Items", menuCount.ToString(), new Point(620, 80), Color.FromArgb(155, 89, 182));
                CreateStatCard("⏳ Pending Orders", pendingOrders.ToString(), new Point(920, 80), Color.FromArgb(241, 196, 15));

                lblStatus.Text = "✅ Dashboard updated";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stats: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"❌ Error: {ex.Message}";
            }
        }

        private void CreateStatCard(string title, string value, Point location, Color color)
        {
            Panel card = new Panel();
            card.Size = new Size(280, 120);
            card.Location = location;
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.None;

            Panel colorBar = new Panel();
            colorBar.Size = new Size(10, 120);
            colorBar.Location = new Point(0, 0);
            colorBar.BackColor = color;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblTitle.ForeColor = Color.Gray;
            lblTitle.Location = new Point(25, 20);
            lblTitle.Size = new Size(200, 25);

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblValue.ForeColor = color;
            lblValue.Location = new Point(25, 50);
            lblValue.Size = new Size(200, 45);

            card.Controls.Add(colorBar);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            panelContent.Controls.Add(card);
        }

        private void BtnMenuManagement_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin")
            {
                frmMenuManagement menuForm = new frmMenuManagement();
                menuForm.ShowDialog();
                LoadDashboardStats();
            }
            else
            {
                MessageBox.Show("Access Denied. Only Admin can access Menu Management.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnTakeOrder_Click(object sender, EventArgs e)
        {
            frmTakeOrder orderForm = new frmTakeOrder();
            orderForm.ShowDialog();
            LoadDashboardStats();
        }

        private void BtnKitchenView_Click(object sender, EventArgs e)
        {
            frmKitchenView kitchenForm = new frmKitchenView();
            kitchenForm.ShowDialog();
            LoadDashboardStats();
        }

        private void BtnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomerManagement customerForm = new frmCustomerManagement();
            customerForm.ShowDialog();
            LoadDashboardStats();
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin")
            {
                frmInventory inventoryForm = new frmInventory();
                inventoryForm.ShowDialog();
                LoadDashboardStats();
            }
            else
            {
                MessageBox.Show("Access Denied. Only Admin can access Inventory.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin")
            {
                frmReports reportsForm = new frmReports();
                reportsForm.ShowDialog();
                LoadDashboardStats();
            }
            else
            {
                MessageBox.Show("Access Denied. Only Admin can access Reports.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnStaff_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin")
            {
                frmStaffManagement staffForm = new frmStaffManagement();
                staffForm.ShowDialog();
                LoadDashboardStats();
            }
            else
            {
                MessageBox.Show("Access Denied. Only Admin can access Staff Management.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelivery_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin" || currentUser.Role == "Cashier" || currentUser.Role == "Delivery")
            {
                frmDeliveryManagement deliveryForm = new frmDeliveryManagement();
                deliveryForm.ShowDialog();
                LoadDashboardStats();
            }
            else
            {
                MessageBox.Show("Access Denied.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin")
            {
                frmSettings settingsForm = new frmSettings();
                settingsForm.ShowDialog();
                LoadDashboardStats();
            }
            else
            {
                MessageBox.Show("Access Denied. Only Admin can access Settings.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAIChatbot_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin" || currentUser.Role == "Cashier")
            {
                frmAIChatbot chatbot = new frmAIChatbot();
                chatbot.ShowDialog();
            }
            else
            {
                MessageBox.Show("AI Chatbot is only available for Admin and Cashier.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                CurrentUser.Clear();
                frmLogin login = new frmLogin();
                login.Show();
                this.Close();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadDashboardStats();
        }
    }
}