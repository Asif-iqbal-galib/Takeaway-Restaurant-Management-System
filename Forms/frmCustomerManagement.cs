using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmCustomerManagement : Form
    {
        private DataGridView dgvCustomers;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnRefresh;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnViewOrders;
        private Button btnClose;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblRecordCount;
        private Panel panelMain;
        private SplitContainer splitContainer;

        private List<Customer> customers;
        private Customer selectedCustomer;

        public frmCustomerManagement()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.Text = "👥 Customer Management - Appeliano Restaurant";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.MinimumSize = new Size(1000, 600);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "👥 CUSTOMER MANAGEMENT";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 45);
            this.Controls.Add(lblTitle);

            // Search Panel
            Panel panelSearch = new Panel();
            panelSearch.Location = new Point(20, 70);
            panelSearch.Size = new Size(1140, 60);
            panelSearch.BackColor = Color.White;
            this.Controls.Add(panelSearch);

            Label lblSearch = new Label();
            lblSearch.Text = "🔍 Search:";
            lblSearch.Font = new Font("Segoe UI", 11);
            lblSearch.Location = new Point(15, 18);
            lblSearch.Size = new Size(70, 25);
            panelSearch.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(90, 15);
            txtSearch.Size = new Size(250, 27);
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.TextChanged += TxtSearch_TextChanged;
            panelSearch.Controls.Add(txtSearch);

            btnSearch = new Button();
            btnSearch.Text = "🔍 Search";
            btnSearch.Location = new Point(350, 13);
            btnSearch.Size = new Size(100, 30);
            btnSearch.BackColor = Color.FromArgb(52, 152, 219);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Click += BtnSearch_Click;
            panelSearch.Controls.Add(btnSearch);

            btnRefresh = new Button();
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Location = new Point(460, 13);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.BackColor = Color.FromArgb(46, 204, 113);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += BtnRefresh_Click;
            panelSearch.Controls.Add(btnRefresh);

            // Split Container for Customers and Order History
            splitContainer = new SplitContainer();
            splitContainer.Location = new Point(20, 140);
            splitContainer.Size = new Size(1140, 450);
            splitContainer.Orientation = Orientation.Vertical;
            splitContainer.SplitterDistance = 550;
            splitContainer.BackColor = Color.White;
            this.Controls.Add(splitContainer);

            // Customers Panel (Left Side)
            Panel panelCustomers = new Panel();
            panelCustomers.Dock = DockStyle.Fill;
            panelCustomers.BackColor = Color.White;
            splitContainer.Panel1.Controls.Add(panelCustomers);

            dgvCustomers = new DataGridView();
            dgvCustomers.Location = new Point(0, 0);
            dgvCustomers.Size = new Size(550, 450);
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.BorderStyle = BorderStyle.FixedSingle;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.CellClick += DgvCustomers_CellClick;
            panelCustomers.Controls.Add(dgvCustomers);

            // Order History Panel (Right Side)
            Panel panelOrders = new Panel();
            panelOrders.Dock = DockStyle.Fill;
            panelOrders.BackColor = Color.White;
            splitContainer.Panel2.Controls.Add(panelOrders);

            Label lblOrdersTitle = new Label();
            lblOrdersTitle.Text = "📋 ORDER HISTORY";
            lblOrdersTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblOrdersTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblOrdersTitle.Location = new Point(10, 10);
            lblOrdersTitle.Size = new Size(200, 30);
            panelOrders.Controls.Add(lblOrdersTitle);

            DataGridView dgvOrderHistory = new DataGridView();
            dgvOrderHistory.Name = "dgvOrderHistory";
            dgvOrderHistory.Location = new Point(10, 45);
            dgvOrderHistory.Size = new Size(540, 300);
            dgvOrderHistory.BackgroundColor = Color.White;
            dgvOrderHistory.BorderStyle = BorderStyle.FixedSingle;
            dgvOrderHistory.AllowUserToAddRows = false;
            dgvOrderHistory.AllowUserToDeleteRows = false;
            dgvOrderHistory.ReadOnly = true;
            dgvOrderHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrderHistory.RowHeadersVisible = false;
            dgvOrderHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            panelOrders.Controls.Add(dgvOrderHistory);

            // Order Details Panel
            Label lblDetailsTitle = new Label();
            lblDetailsTitle.Text = "📦 ORDER DETAILS";
            lblDetailsTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDetailsTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblDetailsTitle.Location = new Point(10, 360);
            lblDetailsTitle.Size = new Size(150, 25);
            panelOrders.Controls.Add(lblDetailsTitle);

            DataGridView dgvOrderDetails = new DataGridView();
            dgvOrderDetails.Name = "dgvOrderDetails";
            dgvOrderDetails.Location = new Point(10, 390);
            dgvOrderDetails.Size = new Size(540, 100);
            dgvOrderDetails.BackgroundColor = Color.White;
            dgvOrderDetails.BorderStyle = BorderStyle.FixedSingle;
            dgvOrderDetails.AllowUserToAddRows = false;
            dgvOrderDetails.AllowUserToDeleteRows = false;
            dgvOrderDetails.ReadOnly = true;
            dgvOrderDetails.RowHeadersVisible = false;
            dgvOrderDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            panelOrders.Controls.Add(dgvOrderDetails);

            // Button Panel
            Panel panelButtons = new Panel();
            panelButtons.Location = new Point(20, 600);
            panelButtons.Size = new Size(1140, 60);
            panelButtons.BackColor = Color.White;
            this.Controls.Add(panelButtons);

            btnAdd = new Button();
            btnAdd.Text = "➕ ADD CUSTOMER";
            btnAdd.Location = new Point(15, 12);
            btnAdd.Size = new Size(130, 35);
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.BackColor = Color.FromArgb(46, 204, 113);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Click += BtnAdd_Click;
            panelButtons.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "✏️ EDIT";
            btnEdit.Location = new Point(160, 12);
            btnEdit.Size = new Size(100, 35);
            btnEdit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnEdit.BackColor = Color.FromArgb(241, 196, 15);
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Click += BtnEdit_Click;
            panelButtons.Controls.Add(btnEdit);

            btnViewOrders = new Button();
            btnViewOrders.Text = "📋 VIEW ORDERS";
            btnViewOrders.Location = new Point(275, 12);
            btnViewOrders.Size = new Size(120, 35);
            btnViewOrders.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnViewOrders.BackColor = Color.FromArgb(52, 152, 219);
            btnViewOrders.ForeColor = Color.White;
            btnViewOrders.FlatStyle = FlatStyle.Flat;
            btnViewOrders.Click += BtnViewOrders_Click;
            panelButtons.Controls.Add(btnViewOrders);

            btnClose = new Button();
            btnClose.Text = "❌ CLOSE";
            btnClose.Location = new Point(1000, 12);
            btnClose.Size = new Size(100, 35);
            btnClose.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnClose.BackColor = Color.FromArgb(149, 165, 166);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();
            panelButtons.Controls.Add(btnClose);

            // Status Strip
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(52, 73, 94);
            statusStrip.ForeColor = Color.White;

            lblStatus = new ToolStripStatusLabel("✅ Ready");
            lblStatus.ForeColor = Color.White;

            lblRecordCount = new ToolStripStatusLabel("👥 0 customers");
            lblRecordCount.ForeColor = Color.White;

            statusStrip.Items.Add(lblStatus);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(lblRecordCount);
            this.Controls.Add(statusStrip);
        }

        private void LoadCustomers()
        {
            try
            {
                customers = DatabaseManager.Instance.GetAllCustomers();
                if (customers == null) customers = new List<Customer>();
                RefreshDataGridView();
                lblRecordCount.Text = $"👥 {customers.Count} customers";
                lblStatus.Text = "✅ Loaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                customers = new List<Customer>();
            }
        }

        private void RefreshDataGridView(string searchTerm = "")
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Loyalty Points", typeof(int));
            dt.Columns.Add("Member Since", typeof(string));

            int filteredCount = 0;

            foreach (var customer in customers)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    bool match = customer.FullName.ToLower().Contains(searchTerm.ToLower()) ||
                                 customer.Phone.Contains(searchTerm);
                    if (!match) continue;
                }

                dt.Rows.Add(
                    customer.CustomerID,
                    customer.FullName,
                    customer.Phone,
                    customer.Email ?? "",
                    customer.Address ?? "",
                    customer.LoyaltyPoints,
                    customer.RegistrationDate.ToShortDateString()
                );
                filteredCount++;
            }

            dgvCustomers.DataSource = dt;
            if (dgvCustomers.Columns["ID"] != null)
                dgvCustomers.Columns["ID"].Visible = false;

            lblRecordCount.Text = $"👥 {filteredCount} customers";
        }

        private void DgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int customerId = Convert.ToInt32(dgvCustomers.Rows[e.RowIndex].Cells["ID"].Value);
                selectedCustomer = customers.Find(c => c.CustomerID == customerId);
                LoadCustomerOrders();
            }
        }

        private void LoadCustomerOrders()
        {
            if (selectedCustomer == null) return;

            try
            {
                var allOrders = DatabaseManager.Instance.GetOrders();
                var customerOrders = new List<Order>();

                foreach (var order in allOrders)
                {
                    // Match by customer name or phone (since CustomerID might not be stored)
                    if (order.CustomerName == selectedCustomer.FullName ||
                        order.CustomerPhone == selectedCustomer.Phone)
                    {
                        customerOrders.Add(order);
                    }
                }

                var dtOrders = new System.Data.DataTable();
                dtOrders.Columns.Add("OrderID", typeof(int));
                dtOrders.Columns.Add("Order #", typeof(string));
                dtOrders.Columns.Add("Date", typeof(string));
                dtOrders.Columns.Add("Total", typeof(string));
                dtOrders.Columns.Add("Status", typeof(string));
                dtOrders.Columns.Add("Payment", typeof(string));

                foreach (var order in customerOrders)
                {
                    dtOrders.Rows.Add(
                        order.OrderID,
                        order.OrderNumber,
                        order.OrderDate.ToString("dd/MM/yyyy HH:mm"),
                        order.TotalAmount.ToString("C"),
                        order.Status,
                        order.PaymentMethod
                    );
                }

                var dgvOrderHistory = (DataGridView)splitContainer.Panel2.Controls.Find("dgvOrderHistory", true)[0];
                dgvOrderHistory.DataSource = dtOrders;
                if (dgvOrderHistory.Columns["OrderID"] != null)
                    dgvOrderHistory.Columns["OrderID"].Visible = false;

                // Clear order details when new customer selected
                var dgvOrderDetails = (DataGridView)splitContainer.Panel2.Controls.Find("dgvOrderDetails", true)[0];
                dgvOrderDetails.DataSource = null;

                // Add cell click event for order history
                dgvOrderHistory.CellClick -= DgvOrderHistory_CellClick;
                dgvOrderHistory.CellClick += DgvOrderHistory_CellClick;

                lblStatus.Text = $"✅ Loaded {customerOrders.Count} orders for {selectedCustomer.FullName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvOrderHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;
                int orderId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["OrderID"].Value);
                LoadOrderDetails(orderId);
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            try
            {
                var items = DatabaseManager.Instance.GetOrderDetails(orderId);

                var dt = new System.Data.DataTable();
                dt.Columns.Add("Item", typeof(string));
                dt.Columns.Add("Qty", typeof(int));
                dt.Columns.Add("Price", typeof(string));
                dt.Columns.Add("Total", typeof(string));

                foreach (var item in items)
                {
                    dt.Rows.Add(
                        item.ItemName,
                        item.Quantity,
                        item.UnitPrice.ToString("C"),
                        item.Subtotal.ToString("C")
                    );
                }

                var dgvOrderDetails = (DataGridView)splitContainer.Panel2.Controls.Find("dgvOrderDetails", true)[0];
                dgvOrderDetails.DataSource = dt;

                lblStatus.Text = $"✅ Loaded {items.Count} items for order #{orderId}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshDataGridView(txtSearch.Text.Trim());
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RefreshDataGridView(txtSearch.Text.Trim());
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCustomers();
            // Clear order history panel
            var dgvOrderHistory = (DataGridView)splitContainer.Panel2.Controls.Find("dgvOrderHistory", true)[0];
            dgvOrderHistory.DataSource = null;
            var dgvOrderDetails = (DataGridView)splitContainer.Panel2.Controls.Find("dgvOrderDetails", true)[0];
            dgvOrderDetails.DataSource = null;
            selectedCustomer = null;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new frmCustomerDialog(null))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers();
                    lblStatus.Text = "✅ Customer added";
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["ID"].Value);
            Customer selected = customers.Find(c => c.CustomerID == customerId);

            if (selected != null)
            {
                using (var dialog = new frmCustomerDialog(selected))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        LoadCustomers();
                        lblStatus.Text = "✅ Customer updated";
                    }
                }
            }
        }

        private void BtnViewOrders_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["ID"].Value);
            selectedCustomer = customers.Find(c => c.CustomerID == customerId);

            if (selectedCustomer != null)
            {
                LoadCustomerOrders();
                lblStatus.Text = $"📋 Viewing orders for {selectedCustomer.FullName}";
            }
        }
    }

    public class frmCustomerDialog : Form
    {
        private Customer customer;
        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private Button btnSave;
        private Button btnCancel;

        public frmCustomerDialog(Customer existingCustomer = null)
        {
            customer = existingCustomer;
            InitializeComponent();

            if (customer != null)
            {
                this.Text = "✏️ Edit Customer";
                LoadCustomerData();
            }
            else
            {
                this.Text = "➕ Add New Customer";
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Name
            Label lblName = new Label();
            lblName.Text = "Full Name:";
            lblName.Location = new Point(20, 20);
            lblName.Size = new Size(100, 25);
            this.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(130, 17);
            txtName.Size = new Size(280, 25);
            this.Controls.Add(txtName);

            // Phone
            Label lblPhone = new Label();
            lblPhone.Text = "Phone:";
            lblPhone.Location = new Point(20, 55);
            lblPhone.Size = new Size(100, 25);
            this.Controls.Add(lblPhone);

            txtPhone = new TextBox();
            txtPhone.Location = new Point(130, 52);
            txtPhone.Size = new Size(280, 25);
            this.Controls.Add(txtPhone);

            // Email
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(20, 90);
            lblEmail.Size = new Size(100, 25);
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Location = new Point(130, 87);
            txtEmail.Size = new Size(280, 25);
            this.Controls.Add(txtEmail);

            // Address
            Label lblAddress = new Label();
            lblAddress.Text = "Address:";
            lblAddress.Location = new Point(20, 125);
            lblAddress.Size = new Size(100, 25);
            this.Controls.Add(lblAddress);

            txtAddress = new TextBox();
            txtAddress.Location = new Point(130, 122);
            txtAddress.Size = new Size(280, 60);
            txtAddress.Multiline = true;
            this.Controls.Add(txtAddress);

            // Buttons
            btnSave = new Button();
            btnSave.Text = "💾 Save";
            btnSave.Location = new Point(130, 210);
            btnSave.Size = new Size(100, 35);
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button();
            btnCancel.Text = "❌ Cancel";
            btnCancel.Location = new Point(250, 210);
            btnCancel.Size = new Size(100, 35);
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnCancel);
        }

        private void LoadCustomerData()
        {
            txtName.Text = customer.FullName;
            txtPhone.Text = customer.Phone;
            txtEmail.Text = customer.Email;
            txtAddress.Text = customer.Address;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone number is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (customer == null)
                {
                    Customer newCustomer = new Customer
                    {
                        FullName = txtName.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Address = txtAddress.Text.Trim()
                    };

                    int newId = DatabaseManager.Instance.AddCustomer(newCustomer);

                    if (newId > 0)
                    {
                        MessageBox.Show("Customer added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    customer.FullName = txtName.Text.Trim();
                    customer.Phone = txtPhone.Text.Trim();
                    customer.Email = txtEmail.Text.Trim();
                    customer.Address = txtAddress.Text.Trim();

                    int rows = DatabaseManager.Instance.UpdateCustomer(customer);

                    if (rows > 0)
                    {
                        MessageBox.Show("Customer updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}