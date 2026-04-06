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
        private Button btnLogout;
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
            this.Text = "Main Dashboard - Appeliano Restaurant";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Welcome Label
            lblWelcome = new Label();
            lblWelcome.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(52, 73, 94);
            lblWelcome.Location = new Point(20, 20);
            lblWelcome.Size = new Size(600, 40);
            this.Controls.Add(lblWelcome);

            // DateTime Label
            lblDateTime = new Label();
            lblDateTime.Font = new Font("Segoe UI", 10);
            lblDateTime.ForeColor = Color.Gray;
            lblDateTime.Location = new Point(20, 60);
            lblDateTime.Size = new Size(400, 25);
            this.Controls.Add(lblDateTime);

            // Timer for clock
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            // Menu Panel (Left Side)
            panelMenu = new Panel();
            panelMenu.Location = new Point(20, 100);
            panelMenu.Size = new Size(250, 520);
            panelMenu.BackColor = Color.FromArgb(52, 73, 94);
            panelMenu.BorderStyle = BorderStyle.None;
            this.Controls.Add(panelMenu);

            // Content Panel (Right Side)
            panelContent = new Panel();
            panelContent.Location = new Point(290, 100);
            panelContent.Size = new Size(880, 520);
            panelContent.BackColor = Color.White;
            panelContent.BorderStyle = BorderStyle.FixedSingle;
            panelContent.AutoScroll = true;
            this.Controls.Add(panelContent);

            int yPos = 20;
            int btnHeight = 45;
            int btnSpacing = 5;

            // 📋 Menu Management Button - Admin Only
            btnMenuManagement = CreateMenuButton("📋 Menu Management", yPos);
            btnMenuManagement.Click += BtnMenuManagement_Click;
            yPos += btnHeight + btnSpacing;

            // 🛒 Take Order Button - Cashier and Admin
            btnTakeOrder = CreateMenuButton("🛒 Take Order", yPos);
            btnTakeOrder.Click += BtnTakeOrder_Click;
            yPos += btnHeight + btnSpacing;

            // 👨‍🍳 Kitchen View Button - Kitchen Staff and Admin
            btnKitchenView = CreateMenuButton("👨‍🍳 Kitchen View", yPos);
            btnKitchenView.Click += BtnKitchenView_Click;
            yPos += btnHeight + btnSpacing;

            // 👥 Customers Button - Cashier and Admin
            btnCustomers = CreateMenuButton("👥 Customers", yPos);
            btnCustomers.Click += BtnCustomers_Click;
            yPos += btnHeight + btnSpacing;

            // 📦 Inventory Button - Admin Only
            btnInventory = CreateMenuButton("📦 Inventory", yPos);
            btnInventory.Click += BtnInventory_Click;
            yPos += btnHeight + btnSpacing;

            // 📊 Reports Button - Admin Only
            btnReports = CreateMenuButton("📊 Reports", yPos);
            btnReports.Click += BtnReports_Click;
            yPos += btnHeight + btnSpacing;

            // 👤 Staff Management Button - Admin Only
            btnStaff = CreateMenuButton("👤 Staff Management", yPos);
            btnStaff.Click += BtnStaff_Click;
            yPos += btnHeight + btnSpacing;

            // 🚚 Delivery Button - Delivery Staff and Admin (NOT Cashier)
            btnDelivery = CreateMenuButton("🚚 Delivery", yPos);
            btnDelivery.Click += BtnDelivery_Click;
            yPos += btnHeight + btnSpacing;

            // ⚙ Settings Button - Admin Only
            btnSettings = CreateMenuButton("⚙ Settings", yPos);
            btnSettings.Click += BtnSettings_Click;
            yPos += btnHeight + btnSpacing;

            // 🚪 Logout Button - Everyone
            btnLogout = CreateMenuButton("🚪 Logout", yPos);
            btnLogout.BackColor = Color.FromArgb(231, 76, 60);
            btnLogout.Click += BtnLogout_Click;

            // Add buttons to menu panel
            panelMenu.Controls.AddRange(new Control[] {
                btnMenuManagement, btnTakeOrder, btnKitchenView, btnCustomers,
                btnInventory, btnReports, btnStaff, btnDelivery, btnSettings, btnLogout
            });

            // Configure role-based visibility
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

            // Hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(64, 87, 109);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.Transparent;

            return btn;
        }

        private void ConfigureRoleBasedAccess()
        {
            if (currentUser == null) return;

            // First, hide all admin-only buttons by default
            btnMenuManagement.Visible = false;
            btnInventory.Visible = false;
            btnReports.Visible = false;
            btnStaff.Visible = false;
            btnSettings.Visible = false;

            // Show all buttons for Admin
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
            }
            // Cashier Role - NO DELIVERY BUTTON
            else if (currentUser.Role == "Cashier")
            {
                btnTakeOrder.Visible = true;
                btnCustomers.Visible = true;
                btnKitchenView.Visible = false;
                btnDelivery.Visible = false;  // Cashier should NOT see Delivery
                // All other admin buttons already hidden
            }
            // Kitchen Staff Role
            else if (currentUser.Role == "Kitchen")
            {
                btnKitchenView.Visible = true;
                btnDelivery.Visible = false;
            }
            // Delivery Staff Role
            else if (currentUser.Role == "Delivery")
            {
                btnDelivery.Visible = true;
                btnKitchenView.Visible = false;
                btnTakeOrder.Visible = false;
                btnCustomers.Visible = false;
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
            // Clear content panel
            panelContent.Controls.Clear();

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "Dashboard Overview";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 40);
            panelContent.Controls.Add(lblTitle);

            try
            {
                // Get real stats from database
                int todayOrders = DatabaseManager.Instance.GetTodayOrdersCount();
                decimal todayRevenue = DatabaseManager.Instance.GetTodayRevenue();
                int pendingOrders = DatabaseManager.Instance.GetPendingOrdersCount();
                var menuItems = DatabaseManager.Instance.GetMenuItems();
                int menuCount = menuItems?.Count ?? 0;

                // Create stats cards
                CreateStatCard("📊 Today's Orders", todayOrders.ToString(), new Point(20, 80), Color.FromArgb(52, 152, 219));
                CreateStatCard("💰 Today's Revenue", todayRevenue.ToString("C"), new Point(320, 80), Color.FromArgb(46, 204, 113));
                CreateStatCard("🍽️ Menu Items", menuCount.ToString(), new Point(620, 80), Color.FromArgb(155, 89, 182));
                CreateStatCard("⏳ Pending Orders", pendingOrders.ToString(), new Point(920, 80), Color.FromArgb(241, 196, 15));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stats: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // =============================================
        // BUTTON CLICK HANDLERS
        // =============================================

        private void BtnMenuManagement_Click(object sender, EventArgs e)
        {
            frmMenuManagement menuForm = new frmMenuManagement();
            menuForm.ShowDialog();
            LoadDashboardStats();
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
            frmInventory inventoryForm = new frmInventory();
            inventoryForm.ShowDialog();
            LoadDashboardStats();
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            frmReports reportsForm = new frmReports();
            reportsForm.ShowDialog();
            LoadDashboardStats();
        }

        private void BtnStaff_Click(object sender, EventArgs e)
        {
            frmStaffManagement staffForm = new frmStaffManagement();
            staffForm.ShowDialog();
            LoadDashboardStats();
        }

        private void BtnDelivery_Click(object sender, EventArgs e)
        {
            // Only show if user is Admin or Delivery
            if (currentUser.Role == "Admin" || currentUser.Role == "Delivery")
            {
                frmDeliveryManagement deliveryForm = new frmDeliveryManagement();
                deliveryForm.ShowDialog();
                LoadDashboardStats();
            }
            else
            {
                MessageBox.Show("Access Denied. Only Admin and Delivery Staff can access this page.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            frmSettings settingsForm = new frmSettings();
            settingsForm.ShowDialog();
            LoadDashboardStats();
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