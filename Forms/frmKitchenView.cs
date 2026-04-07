using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmKitchenView : Form
    {
        private DataGridView dgvOrders;
        private Button btnRefresh;
        private Button btnMarkPreparing;
        private Button btnMarkReady;
        private Button btnClose;
        private Label lblStatus;
        private Timer refreshTimer;
        private List<Order> pendingOrders;

        public frmKitchenView()
        {
            InitializeComponent();
            LoadPendingOrders();

            refreshTimer = new Timer();
            refreshTimer.Interval = 30000;
            refreshTimer.Tick += (s, e) => LoadPendingOrders();
            refreshTimer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "Kitchen View - Appeliano Restaurant";
            this.Size = new Size(1100, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "KITCHEN DISPLAY";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Size = new Size(350, 40);
            this.Controls.Add(lblTitle);

            // DataGridView
            dgvOrders = new DataGridView();
            dgvOrders.Location = new Point(20, 70);
            dgvOrders.Size = new Size(1040, 400);
            dgvOrders.BackgroundColor = Color.White;
            dgvOrders.BorderStyle = BorderStyle.FixedSingle;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.ReadOnly = true;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.CellFormatting += DgvOrders_CellFormatting;
            this.Controls.Add(dgvOrders);

            // Buttons
            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(960, 20);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            btnMarkPreparing = new Button();
            btnMarkPreparing.Text = "Mark as Preparing";
            btnMarkPreparing.Location = new Point(20, 480);
            btnMarkPreparing.Size = new Size(150, 35);
            btnMarkPreparing.BackColor = Color.FromArgb(241, 196, 15);
            btnMarkPreparing.ForeColor = Color.White;
            btnMarkPreparing.FlatStyle = FlatStyle.Flat;
            btnMarkPreparing.Click += BtnMarkPreparing_Click;
            this.Controls.Add(btnMarkPreparing);

            btnMarkReady = new Button();
            btnMarkReady.Text = "Mark as Ready";
            btnMarkReady.Location = new Point(180, 480);
            btnMarkReady.Size = new Size(150, 35);
            btnMarkReady.BackColor = Color.FromArgb(46, 204, 113);
            btnMarkReady.ForeColor = Color.White;
            btnMarkReady.FlatStyle = FlatStyle.Flat;
            btnMarkReady.Click += BtnMarkReady_Click;
            this.Controls.Add(btnMarkReady);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Location = new Point(960, 480);
            btnClose.Size = new Size(100, 35);
            btnClose.BackColor = Color.FromArgb(149, 165, 166);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // Status Label
            lblStatus = new Label();
            lblStatus.Text = "Ready";
            lblStatus.Font = new Font("Segoe UI", 10);
            lblStatus.Location = new Point(20, 530);
            lblStatus.Size = new Size(400, 25);
            this.Controls.Add(lblStatus);
        }

        private void LoadPendingOrders()
        {
            try
            {
                var allOrders = DatabaseManager.Instance.GetOrders();
                pendingOrders = new List<Order>();

                foreach (var order in allOrders)
                {
                    if (order != null && (order.Status == "Pending" || order.Status == "Preparing"))
                    {
                        pendingOrders.Add(order);
                    }
                }

                var dt = new System.Data.DataTable();
                dt.Columns.Add("OrderID", typeof(int));
                dt.Columns.Add("Order #", typeof(string));
                dt.Columns.Add("Customer", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("Items", typeof(string));
                dt.Columns.Add("Status", typeof(string));

                foreach (var order in pendingOrders)
                {
                    var items = DatabaseManager.Instance.GetOrderDetails(order.OrderID);
                    string itemsList = "";
                    foreach (var item in items)
                    {
                        itemsList += $"{item.Quantity}x {item.ItemName}\n";
                    }

                    dt.Rows.Add(
                        order.OrderID,
                        order.OrderNumber,
                        order.CustomerName,
                        order.OrderDate.ToString("HH:mm"),
                        itemsList,
                        order.Status
                    );
                }

                dgvOrders.DataSource = dt;
                if (dgvOrders.Columns["OrderID"] != null)
                    dgvOrders.Columns["OrderID"].Visible = false;

                lblStatus.Text = $"{pendingOrders.Count} pending orders | Last updated: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Pending")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 240, 200);
                    e.CellStyle.ForeColor = Color.FromArgb(194, 124, 14);
                }
                else if (status == "Preparing")
                {
                    e.CellStyle.BackColor = Color.FromArgb(200, 220, 255);
                    e.CellStyle.ForeColor = Color.FromArgb(41, 128, 185);
                }
            }
        }

        private void BtnMarkPreparing_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order.");
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
            string orderNumber = dgvOrders.SelectedRows[0].Cells["Order #"].Value.ToString();

            if (MessageBox.Show($"Mark order {orderNumber} as Preparing?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseManager.Instance.UpdateOrderStatus(orderId, "Preparing");
                LoadPendingOrders();
                MessageBox.Show($"Order {orderNumber} marked as Preparing.");
            }
        }

        private void BtnMarkReady_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order.");
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
            string orderNumber = dgvOrders.SelectedRows[0].Cells["Order #"].Value.ToString();

            if (MessageBox.Show($"Mark order {orderNumber} as Ready?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseManager.Instance.UpdateOrderStatus(orderId, "Ready");
                LoadPendingOrders();
                MessageBox.Show($"Order {orderNumber} marked as Ready.");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadPendingOrders();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
}