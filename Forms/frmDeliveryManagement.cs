using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmDeliveryManagement : Form
    {
        private TabControl tabControl;
        private TabPage tabAllDeliveries;
        private TabPage tabPendingDeliveries;
        private TabPage tabMyDeliveries;
        private TabPage tabAvailableOrders;

        private DataGridView dgvAllDeliveries;
        private DataGridView dgvPendingDeliveries;
        private DataGridView dgvMyDeliveries;
        private DataGridView dgvAvailableOrders;

        private Button btnRefreshAll;
        private Button btnRefreshPending;
        private Button btnRefreshMy;
        private Button btnRefreshAvailable;
        private Button btnMarkPickedUp;
        private Button btnMarkDelivered;
        private Button btnAssignToStaff;
        private Button btnAssignToMe;
        private ComboBox cmbDeliveryStaff;
        private Label lblStatus;
        private Timer refreshTimer;

        private List<Staff> deliveryStaff;
        private bool isAdmin;
        private bool isDelivery;

        public frmDeliveryManagement()
        {
            isAdmin = (CurrentUser.Role == "Admin");
            isDelivery = (CurrentUser.Role == "Delivery");

            // If neither Admin nor Delivery, show error and close
            if (!isAdmin && !isDelivery)
            {
                MessageBox.Show("Access Denied. Only Admin and Delivery Staff can access this page.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            InitializeComponent();

            if (isAdmin)
            {
                LoadAllDeliveries();
                LoadPendingDeliveries();
                LoadDeliveryStaff();
                if (tabAllDeliveries != null)
                    tabControl.SelectedTab = tabAllDeliveries;
            }

            if (isDelivery)
            {
                LoadMyDeliveries();
                LoadAvailableOrders();
                if (tabMyDeliveries != null)
                    tabControl.SelectedTab = tabMyDeliveries;
            }

            refreshTimer = new Timer();
            refreshTimer.Interval = 30000;
            refreshTimer.Tick += (s, e) => {
                if (isAdmin && this.IsHandleCreated)
                {
                    LoadAllDeliveries();
                    LoadPendingDeliveries();
                }
                if (isDelivery && this.IsHandleCreated)
                {
                    LoadMyDeliveries();
                    LoadAvailableOrders();
                }
            };
            refreshTimer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "Delivery Management - Appeliano Restaurant";
            this.Size = new Size(1400, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.MinimumSize = new Size(1200, 650);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "🚚 DELIVERY MANAGEMENT";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Size = new Size(400, 40);
            this.Controls.Add(lblTitle);

            // Role info
            Label lblRole = new Label();
            if (isAdmin)
                lblRole.Text = $"Viewing as: Admin - All deliveries shown below";
            else
                lblRole.Text = $"Welcome, {CurrentUser.FullName} (Delivery Staff)";
            lblRole.Font = new Font("Segoe UI", 11);
            lblRole.ForeColor = Color.FromArgb(100, 100, 100);
            lblRole.Location = new Point(20, 55);
            lblRole.Size = new Size(500, 25);
            this.Controls.Add(lblRole);

            // Tab Control
            tabControl = new TabControl();
            tabControl.Location = new Point(20, 90);
            tabControl.Size = new Size(1350, 540);
            tabControl.Font = new Font("Segoe UI", 10);
            this.Controls.Add(tabControl);

            // ========== ALL DELIVERIES TAB (Admin View) ==========
            tabAllDeliveries = new TabPage("📋 All Deliveries");
            if (isAdmin) tabControl.TabPages.Add(tabAllDeliveries);

            Panel panelAll = new Panel();
            panelAll.Dock = DockStyle.Fill;
            panelAll.Padding = new Padding(10);
            tabAllDeliveries.Controls.Add(panelAll);

            dgvAllDeliveries = new DataGridView();
            dgvAllDeliveries.Location = new Point(0, 0);
            dgvAllDeliveries.Size = new Size(1310, 400);
            dgvAllDeliveries.BackgroundColor = Color.White;
            dgvAllDeliveries.BorderStyle = BorderStyle.FixedSingle;
            dgvAllDeliveries.AllowUserToAddRows = false;
            dgvAllDeliveries.AllowUserToDeleteRows = false;
            dgvAllDeliveries.ReadOnly = true;
            dgvAllDeliveries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAllDeliveries.RowHeadersVisible = false;
            dgvAllDeliveries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAllDeliveries.CellFormatting += DgvAllDeliveries_CellFormatting;
            panelAll.Controls.Add(dgvAllDeliveries);

            Panel panelAllButtons = new Panel();
            panelAllButtons.Location = new Point(0, 410);
            panelAllButtons.Size = new Size(1310, 50);
            panelAllButtons.BackColor = Color.White;
            panelAll.Controls.Add(panelAllButtons);

            btnRefreshAll = new Button();
            btnRefreshAll.Text = "🔄 Refresh";
            btnRefreshAll.Location = new Point(10, 10);
            btnRefreshAll.Size = new Size(100, 32);
            btnRefreshAll.BackColor = Color.FromArgb(52, 152, 219);
            btnRefreshAll.ForeColor = Color.White;
            btnRefreshAll.FlatStyle = FlatStyle.Flat;
            btnRefreshAll.Click += BtnRefreshAll_Click;
            panelAllButtons.Controls.Add(btnRefreshAll);

            // ========== PENDING DELIVERIES TAB (Admin View) ==========
            tabPendingDeliveries = new TabPage("⏳ Pending Deliveries");
            if (isAdmin) tabControl.TabPages.Add(tabPendingDeliveries);

            Panel panelPending = new Panel();
            panelPending.Dock = DockStyle.Fill;
            panelPending.Padding = new Padding(10);
            tabPendingDeliveries.Controls.Add(panelPending);

            dgvPendingDeliveries = new DataGridView();
            dgvPendingDeliveries.Location = new Point(0, 0);
            dgvPendingDeliveries.Size = new Size(1310, 350);
            dgvPendingDeliveries.BackgroundColor = Color.White;
            dgvPendingDeliveries.BorderStyle = BorderStyle.FixedSingle;
            dgvPendingDeliveries.AllowUserToAddRows = false;
            dgvPendingDeliveries.AllowUserToDeleteRows = false;
            dgvPendingDeliveries.ReadOnly = true;
            dgvPendingDeliveries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPendingDeliveries.RowHeadersVisible = false;
            dgvPendingDeliveries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            panelPending.Controls.Add(dgvPendingDeliveries);

            Panel panelPendingButtons = new Panel();
            panelPendingButtons.Location = new Point(0, 360);
            panelPendingButtons.Size = new Size(1310, 80);
            panelPendingButtons.BackColor = Color.White;
            panelPending.Controls.Add(panelPendingButtons);

            btnRefreshPending = new Button();
            btnRefreshPending.Text = "🔄 Refresh";
            btnRefreshPending.Location = new Point(10, 25);
            btnRefreshPending.Size = new Size(100, 32);
            btnRefreshPending.BackColor = Color.FromArgb(52, 152, 219);
            btnRefreshPending.ForeColor = Color.White;
            btnRefreshPending.FlatStyle = FlatStyle.Flat;
            btnRefreshPending.Click += BtnRefreshPending_Click;
            panelPendingButtons.Controls.Add(btnRefreshPending);

            Label lblAssignTo = new Label();
            lblAssignTo.Text = "Assign to:";
            lblAssignTo.Font = new Font("Segoe UI", 10);
            lblAssignTo.Location = new Point(130, 30);
            lblAssignTo.Size = new Size(70, 25);
            panelPendingButtons.Controls.Add(lblAssignTo);

            cmbDeliveryStaff = new ComboBox();
            cmbDeliveryStaff.Location = new Point(205, 28);
            cmbDeliveryStaff.Size = new Size(150, 25);
            cmbDeliveryStaff.DropDownStyle = ComboBoxStyle.DropDownList;
            panelPendingButtons.Controls.Add(cmbDeliveryStaff);

            btnAssignToStaff = new Button();
            btnAssignToStaff.Text = "📋 Assign to Staff";
            btnAssignToStaff.Location = new Point(370, 25);
            btnAssignToStaff.Size = new Size(130, 32);
            btnAssignToStaff.BackColor = Color.FromArgb(46, 204, 113);
            btnAssignToStaff.ForeColor = Color.White;
            btnAssignToStaff.FlatStyle = FlatStyle.Flat;
            btnAssignToStaff.Click += BtnAssignToStaff_Click;
            panelPendingButtons.Controls.Add(btnAssignToStaff);

            // ========== MY DELIVERIES TAB (Delivery Staff Only) ==========
            tabMyDeliveries = new TabPage("📦 My Deliveries");
            if (isDelivery) tabControl.TabPages.Add(tabMyDeliveries);

            Panel panelMy = new Panel();
            panelMy.Dock = DockStyle.Fill;
            panelMy.Padding = new Padding(10);
            tabMyDeliveries.Controls.Add(panelMy);

            dgvMyDeliveries = new DataGridView();
            dgvMyDeliveries.Location = new Point(0, 0);
            dgvMyDeliveries.Size = new Size(1310, 350);
            dgvMyDeliveries.BackgroundColor = Color.White;
            dgvMyDeliveries.BorderStyle = BorderStyle.FixedSingle;
            dgvMyDeliveries.AllowUserToAddRows = false;
            dgvMyDeliveries.AllowUserToDeleteRows = false;
            dgvMyDeliveries.ReadOnly = true;
            dgvMyDeliveries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMyDeliveries.RowHeadersVisible = false;
            dgvMyDeliveries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMyDeliveries.CellFormatting += DgvMyDeliveries_CellFormatting;
            panelMy.Controls.Add(dgvMyDeliveries);

            Panel panelMyButtons = new Panel();
            panelMyButtons.Location = new Point(0, 360);
            panelMyButtons.Size = new Size(1310, 50);
            panelMyButtons.BackColor = Color.White;
            panelMy.Controls.Add(panelMyButtons);

            btnRefreshMy = new Button();
            btnRefreshMy.Text = "🔄 Refresh";
            btnRefreshMy.Location = new Point(10, 10);
            btnRefreshMy.Size = new Size(100, 32);
            btnRefreshMy.BackColor = Color.FromArgb(52, 152, 219);
            btnRefreshMy.ForeColor = Color.White;
            btnRefreshMy.FlatStyle = FlatStyle.Flat;
            btnRefreshMy.Click += BtnRefreshMy_Click;
            panelMyButtons.Controls.Add(btnRefreshMy);

            btnMarkPickedUp = new Button();
            btnMarkPickedUp.Text = "📦 Mark as Picked Up";
            btnMarkPickedUp.Location = new Point(120, 10);
            btnMarkPickedUp.Size = new Size(140, 32);
            btnMarkPickedUp.BackColor = Color.FromArgb(241, 196, 15);
            btnMarkPickedUp.ForeColor = Color.White;
            btnMarkPickedUp.FlatStyle = FlatStyle.Flat;
            btnMarkPickedUp.Click += BtnMarkPickedUp_Click;
            panelMyButtons.Controls.Add(btnMarkPickedUp);

            btnMarkDelivered = new Button();
            btnMarkDelivered.Text = "✅ Mark as Delivered";
            btnMarkDelivered.Location = new Point(270, 10);
            btnMarkDelivered.Size = new Size(140, 32);
            btnMarkDelivered.BackColor = Color.FromArgb(46, 204, 113);
            btnMarkDelivered.ForeColor = Color.White;
            btnMarkDelivered.FlatStyle = FlatStyle.Flat;
            btnMarkDelivered.Click += BtnMarkDelivered_Click;
            panelMyButtons.Controls.Add(btnMarkDelivered);

            // ========== AVAILABLE ORDERS TAB (Delivery Staff Only) ==========
            tabAvailableOrders = new TabPage("🆕 Available Orders");
            if (isDelivery) tabControl.TabPages.Add(tabAvailableOrders);

            Panel panelAvailable = new Panel();
            panelAvailable.Dock = DockStyle.Fill;
            panelAvailable.Padding = new Padding(10);
            tabAvailableOrders.Controls.Add(panelAvailable);

            dgvAvailableOrders = new DataGridView();
            dgvAvailableOrders.Location = new Point(0, 0);
            dgvAvailableOrders.Size = new Size(1310, 350);
            dgvAvailableOrders.BackgroundColor = Color.White;
            dgvAvailableOrders.BorderStyle = BorderStyle.FixedSingle;
            dgvAvailableOrders.AllowUserToAddRows = false;
            dgvAvailableOrders.AllowUserToDeleteRows = false;
            dgvAvailableOrders.ReadOnly = true;
            dgvAvailableOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableOrders.RowHeadersVisible = false;
            dgvAvailableOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            panelAvailable.Controls.Add(dgvAvailableOrders);

            Panel panelAvailableButtons = new Panel();
            panelAvailableButtons.Location = new Point(0, 360);
            panelAvailableButtons.Size = new Size(1310, 50);
            panelAvailableButtons.BackColor = Color.White;
            panelAvailable.Controls.Add(panelAvailableButtons);

            btnRefreshAvailable = new Button();
            btnRefreshAvailable.Text = "🔄 Refresh";
            btnRefreshAvailable.Location = new Point(10, 10);
            btnRefreshAvailable.Size = new Size(100, 32);
            btnRefreshAvailable.BackColor = Color.FromArgb(52, 152, 219);
            btnRefreshAvailable.ForeColor = Color.White;
            btnRefreshAvailable.FlatStyle = FlatStyle.Flat;
            btnRefreshAvailable.Click += BtnRefreshAvailable_Click;
            panelAvailableButtons.Controls.Add(btnRefreshAvailable);

            btnAssignToMe = new Button();
            btnAssignToMe.Text = "📋 Assign to Me";
            btnAssignToMe.Location = new Point(120, 10);
            btnAssignToMe.Size = new Size(120, 32);
            btnAssignToMe.BackColor = Color.FromArgb(46, 204, 113);
            btnAssignToMe.ForeColor = Color.White;
            btnAssignToMe.FlatStyle = FlatStyle.Flat;
            btnAssignToMe.Click += BtnAssignToMe_Click;
            panelAvailableButtons.Controls.Add(btnAssignToMe);

            // Instructions
            Label lblInstruction = new Label();
            if (isDelivery)
                lblInstruction.Text = "💡 Select an order and click 'Assign to Me' to start delivery";
            else
                lblInstruction.Text = "💡 Admin View - All deliveries and pending orders shown above";
            lblInstruction.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblInstruction.ForeColor = Color.FromArgb(100, 100, 100);
            lblInstruction.Location = new Point(20, 640);
            lblInstruction.Size = new Size(500, 25);
            this.Controls.Add(lblInstruction);

            // Status
            lblStatus = new Label();
            lblStatus.Text = "✅ Ready";
            lblStatus.Font = new Font("Segoe UI", 10);
            lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatus.Location = new Point(20, 670);
            lblStatus.Size = new Size(600, 25);
            this.Controls.Add(lblStatus);
        }

        private void LoadDeliveryStaff()
        {
            try
            {
                var allStaff = DatabaseManager.Instance.GetAllStaff();
                deliveryStaff = new List<Staff>();
                cmbDeliveryStaff.Items.Clear();

                foreach (var staff in allStaff)
                {
                    if (staff.Role == "Delivery" && staff.IsActive)
                    {
                        deliveryStaff.Add(staff);
                        cmbDeliveryStaff.Items.Add($"{staff.FullName} ({staff.Username})");
                    }
                }

                if (cmbDeliveryStaff.Items.Count > 0)
                    cmbDeliveryStaff.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading delivery staff: {ex.Message}");
            }
        }

        private void LoadAllDeliveries()
        {
            try
            {
                var allOrders = DatabaseManager.Instance.GetOrders();
                var allStaff = DatabaseManager.Instance.GetAllStaff();
                var dt = new System.Data.DataTable();
                dt.Columns.Add("Order #", typeof(string));
                dt.Columns.Add("Customer", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Assigned To", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("Status", typeof(string));

                foreach (var order in allOrders)
                {
                    if (!string.IsNullOrEmpty(order.DeliveryAddress))
                    {
                        string staffName = "Unassigned";
                        if (order.AssignedTo != null)
                        {
                            foreach (var staff in allStaff)
                            {
                                if (staff.StaffID == order.AssignedTo)
                                {
                                    staffName = staff.FullName;
                                    break;
                                }
                            }
                        }

                        string status = order.Status;
                        if (status == "OutForDelivery") status = "Out for Delivery";
                        if (status == "Pending") status = "Waiting for Assignment";

                        dt.Rows.Add(
                            order.OrderNumber,
                            order.CustomerName,
                            order.CustomerPhone,
                            order.DeliveryAddress,
                            staffName,
                            order.OrderDate.ToString("HH:mm"),
                            status
                        );
                    }
                }

                dgvAllDeliveries.DataSource = dt;
                lblStatus.Text = $"✅ All Deliveries: {dt.Rows.Count} | Last updated: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"❌ Error: {ex.Message}";
            }
        }

        private void LoadPendingDeliveries()
        {
            try
            {
                var allOrders = DatabaseManager.Instance.GetOrders();
                var dt = new System.Data.DataTable();
                dt.Columns.Add("OrderID", typeof(int));
                dt.Columns.Add("Order #", typeof(string));
                dt.Columns.Add("Customer", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("Amount", typeof(string));

                foreach (var order in allOrders)
                {
                    if (!string.IsNullOrEmpty(order.DeliveryAddress) &&
                        order.Status == "Pending" &&
                        order.AssignedTo == null)
                    {
                        dt.Rows.Add(
                            order.OrderID,
                            order.OrderNumber,
                            order.CustomerName,
                            order.CustomerPhone,
                            order.DeliveryAddress,
                            order.OrderDate.ToString("HH:mm"),
                            order.TotalAmount.ToString("C")
                        );
                    }
                }

                dgvPendingDeliveries.DataSource = dt;
                if (dgvPendingDeliveries.Columns["OrderID"] != null)
                    dgvPendingDeliveries.Columns["OrderID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pending deliveries: {ex.Message}");
            }
        }

        private void LoadMyDeliveries()
        {
            try
            {
                var myDeliveries = DatabaseManager.Instance.GetDeliveriesByStaff(CurrentUser.UserID);
                if (myDeliveries == null) myDeliveries = new List<Delivery>();

                var dt = new System.Data.DataTable();
                dt.Columns.Add("DeliveryID", typeof(int));
                dt.Columns.Add("Order #", typeof(string));
                dt.Columns.Add("Customer", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("Status", typeof(string));

                int active = 0;
                foreach (var d in myDeliveries)
                {
                    string statusDisplay = d.Status == "Assigned" ? "Assigned" :
                                          d.Status == "PickedUp" ? "Picked Up" :
                                          d.Status == "Delivered" ? "Delivered" : d.Status;

                    if (d.Status == "Assigned" || d.Status == "PickedUp") active++;

                    dt.Rows.Add(
                        d.DeliveryID,
                        d.OrderNumber ?? "N/A",
                        d.CustomerName ?? "Unknown",
                        d.CustomerPhone ?? "",
                        d.DeliveryAddress ?? "",
                        d.AssignedTime.ToString("HH:mm"),
                        statusDisplay
                    );
                }

                dgvMyDeliveries.DataSource = dt;
                if (dgvMyDeliveries.Columns["DeliveryID"] != null)
                    dgvMyDeliveries.Columns["DeliveryID"].Visible = false;

                lblStatus.Text = $"✅ My Deliveries: {myDeliveries.Count} | Active: {active} | Last updated: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"❌ Error: {ex.Message}";
            }
        }

        private void LoadAvailableOrders()
        {
            try
            {
                var allOrders = DatabaseManager.Instance.GetOrders();
                var dt = new System.Data.DataTable();
                dt.Columns.Add("OrderID", typeof(int));
                dt.Columns.Add("Order #", typeof(string));
                dt.Columns.Add("Customer", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("Amount", typeof(string));

                foreach (var order in allOrders)
                {
                    if (!string.IsNullOrEmpty(order.DeliveryAddress) &&
                        order.Status == "Pending" &&
                        order.AssignedTo == null)
                    {
                        dt.Rows.Add(
                            order.OrderID,
                            order.OrderNumber,
                            order.CustomerName,
                            order.CustomerPhone,
                            order.DeliveryAddress,
                            order.OrderDate.ToString("HH:mm"),
                            order.TotalAmount.ToString("C")
                        );
                    }
                }

                dgvAvailableOrders.DataSource = dt;
                if (dgvAvailableOrders.Columns["OrderID"] != null)
                    dgvAvailableOrders.Columns["OrderID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading available orders: {ex.Message}");
            }
        }

        private void DgvAllDeliveries_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAllDeliveries.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status.Contains("Waiting") || status.Contains("Pending"))
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 240, 200);
                    e.CellStyle.ForeColor = Color.FromArgb(194, 124, 14);
                }
                else if (status.Contains("Out") || status.Contains("Picked"))
                {
                    e.CellStyle.BackColor = Color.FromArgb(200, 220, 255);
                    e.CellStyle.ForeColor = Color.FromArgb(41, 128, 185);
                }
                else if (status == "Delivered")
                {
                    e.CellStyle.BackColor = Color.FromArgb(200, 255, 200);
                    e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                }
            }
        }

        private void DgvMyDeliveries_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvMyDeliveries.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Assigned")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 240, 200);
                    e.CellStyle.ForeColor = Color.FromArgb(194, 124, 14);
                }
                else if (status == "Picked Up")
                {
                    e.CellStyle.BackColor = Color.FromArgb(200, 220, 255);
                    e.CellStyle.ForeColor = Color.FromArgb(41, 128, 185);
                }
                else if (status == "Delivered")
                {
                    e.CellStyle.BackColor = Color.FromArgb(200, 255, 200);
                    e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                }
            }
        }

        private void BtnAssignToStaff_Click(object sender, EventArgs e)
        {
            if (dgvPendingDeliveries.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to assign.");
                return;
            }

            if (cmbDeliveryStaff.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a delivery staff member.");
                return;
            }

            int orderId = Convert.ToInt32(dgvPendingDeliveries.SelectedRows[0].Cells["OrderID"].Value);
            string orderNumber = dgvPendingDeliveries.SelectedRows[0].Cells["Order #"].Value.ToString();
            int staffId = deliveryStaff[cmbDeliveryStaff.SelectedIndex].StaffID;
            string staffName = deliveryStaff[cmbDeliveryStaff.SelectedIndex].FullName;

            if (MessageBox.Show($"Assign order {orderNumber} to {staffName}?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.UpdateOrderStatus(orderId, "OutForDelivery");
                    DatabaseManager.Instance.AssignDeliveryStaff(orderId, staffId);

                    Delivery newDelivery = new Delivery
                    {
                        OrderID = orderId,
                        DeliveryStaffID = staffId
                    };
                    DatabaseManager.Instance.CreateDelivery(newDelivery);

                    MessageBox.Show($"✅ Order {orderNumber} assigned to {staffName}!");

                    LoadPendingDeliveries();
                    LoadAllDeliveries();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void BtnMarkPickedUp_Click(object sender, EventArgs e)
        {
            if (dgvMyDeliveries.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a delivery order.");
                return;
            }

            int deliveryId = Convert.ToInt32(dgvMyDeliveries.SelectedRows[0].Cells["DeliveryID"].Value);
            string orderNumber = dgvMyDeliveries.SelectedRows[0].Cells["Order #"].Value.ToString();

            if (MessageBox.Show($"Mark order {orderNumber} as Picked Up?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.UpdateDeliveryStatus(deliveryId, "PickedUp", DateTime.Now);
                    DatabaseManager.Instance.UpdateOrderStatus(GetOrderIdFromDelivery(deliveryId), "OutForDelivery");
                    MessageBox.Show($"✅ Order {orderNumber} marked as Picked Up!");
                    LoadMyDeliveries();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void BtnMarkDelivered_Click(object sender, EventArgs e)
        {
            if (dgvMyDeliveries.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a delivery order.");
                return;
            }

            int deliveryId = Convert.ToInt32(dgvMyDeliveries.SelectedRows[0].Cells["DeliveryID"].Value);
            string orderNumber = dgvMyDeliveries.SelectedRows[0].Cells["Order #"].Value.ToString();
            string currentStatus = dgvMyDeliveries.SelectedRows[0].Cells["Status"].Value.ToString();

            if (currentStatus == "Delivered")
            {
                MessageBox.Show("This order is already delivered.");
                return;
            }

            if (currentStatus != "Picked Up")
            {
                MessageBox.Show("Please mark as Picked Up first.");
                return;
            }

            if (MessageBox.Show($"Mark order {orderNumber} as Delivered?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.UpdateDeliveryStatus(deliveryId, "Delivered", DateTime.Now);
                    DatabaseManager.Instance.UpdateOrderStatus(GetOrderIdFromDelivery(deliveryId), "Delivered");
                    MessageBox.Show($"✅ Order {orderNumber} marked as Delivered!");
                    LoadMyDeliveries();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void BtnAssignToMe_Click(object sender, EventArgs e)
        {
            if (dgvAvailableOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to assign.");
                return;
            }

            int orderId = Convert.ToInt32(dgvAvailableOrders.SelectedRows[0].Cells["OrderID"].Value);
            string orderNumber = dgvAvailableOrders.SelectedRows[0].Cells["Order #"].Value.ToString();

            if (MessageBox.Show($"Assign order {orderNumber} to yourself?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.UpdateOrderStatus(orderId, "OutForDelivery");
                    DatabaseManager.Instance.AssignDeliveryStaff(orderId, CurrentUser.UserID);

                    Delivery newDelivery = new Delivery
                    {
                        OrderID = orderId,
                        DeliveryStaffID = CurrentUser.UserID
                    };
                    DatabaseManager.Instance.CreateDelivery(newDelivery);

                    MessageBox.Show($"✅ Order {orderNumber} assigned to you!");

                    LoadAvailableOrders();
                    LoadMyDeliveries();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void BtnRefreshAll_Click(object sender, EventArgs e)
        {
            LoadAllDeliveries();
        }

        private void BtnRefreshPending_Click(object sender, EventArgs e)
        {
            LoadPendingDeliveries();
            LoadDeliveryStaff();
        }

        private void BtnRefreshMy_Click(object sender, EventArgs e)
        {
            LoadMyDeliveries();
        }

        private void BtnRefreshAvailable_Click(object sender, EventArgs e)
        {
            LoadAvailableOrders();
        }

        private int GetOrderIdFromDelivery(int deliveryId)
        {
            var myDeliveries = DatabaseManager.Instance.GetDeliveriesByStaff(CurrentUser.UserID);
            foreach (var delivery in myDeliveries)
            {
                if (delivery.DeliveryID == deliveryId)
                    return delivery.OrderID;
            }
            return 0;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
}