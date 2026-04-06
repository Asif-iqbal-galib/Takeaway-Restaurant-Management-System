using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmKitchenView : Form
    {
        private DataGridView dgvOrders;
        private Button btnRefresh;
        private Button btnMarkPreparing;
        private Button btnMarkReady;
        private Label lblStatus;
        private Timer refreshTimer;

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
            this.Text = "Kitchen View - Takeaway Restaurant System";
            this.Size = new Size(1100, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "KITCHEN DISPLAY";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Size = new Size(300, 35);
            this.Controls.Add(lblTitle);

            // Refresh Button
            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(950, 15);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // DataGridView
            dgvOrders = new DataGridView();
            dgvOrders.Location = new Point(20, 60);
            dgvOrders.Size = new Size(1040, 420);
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
            btnMarkPreparing = new Button();
            btnMarkPreparing.Text = "Mark as Preparing";
            btnMarkPreparing.Location = new Point(20, 490);
            btnMarkPreparing.Size = new Size(150, 35);
            btnMarkPreparing.BackColor = Color.FromArgb(241, 196, 15);
            btnMarkPreparing.ForeColor = Color.White;
            btnMarkPreparing.FlatStyle = FlatStyle.Flat;
            btnMarkPreparing.Click += BtnMarkPreparing_Click;
            this.Controls.Add(btnMarkPreparing);

            btnMarkReady = new Button();
            btnMarkReady.Text = "Mark as Ready";
            btnMarkReady.Location = new Point(180, 490);
            btnMarkReady.Size = new Size(150, 35);
            btnMarkReady.BackColor = Color.FromArgb(46, 204, 113);
            btnMarkReady.ForeColor = Color.White;
            btnMarkReady.FlatStyle = FlatStyle.Flat;
            btnMarkReady.Click += BtnMarkReady_Click;
            this.Controls.Add(btnMarkReady);

            Button btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Location = new Point(980, 490);
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
            lblStatus.Location = new Point(20, 540);
            lblStatus.Size = new Size(400, 25);
            this.Controls.Add(lblStatus);
        }

        private void LoadPendingOrders()
        {
            try
            {
                // Create DataTable
                var dt = new System.Data.DataTable();
                dt.Columns.Add("OrderID", typeof(int));
                dt.Columns.Add("Order Number", typeof(string));
                dt.Columns.Add("Customer", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("Items", typeof(string));
                dt.Columns.Add("Status", typeof(string));

                var allOrders = DatabaseManager.Instance.GetOrders();
                int pendingCount = 0;

                if (allOrders != null)
                {
                    foreach (var order in allOrders)
                    {
                        if (order != null && (order.Status == "Pending" || order.Status == "Preparing"))
                        {
                            pendingCount++;

                            string itemsList = "";
                            try
                            {
                                var items = DatabaseManager.Instance.GetOrderDetails(order.OrderID);
                                if (items != null)
                                {
                                    foreach (var item in items)
                                    {
                                        if (item != null && !string.IsNullOrEmpty(item.ItemName))
                                        {
                                            itemsList += $"{item.Quantity}x {item.ItemName}\n";
                                        }
                                    }
                                }
                            }
                            catch { }

                            if (string.IsNullOrEmpty(itemsList))
                                itemsList = "No items";

                            dt.Rows.Add(
                                order.OrderID,
                                order.OrderNumber ?? "N/A",
                                order.CustomerName ?? "Walk-in Customer",
                                order.OrderDate.ToString("HH:mm"),
                                itemsList,
                                order.Status ?? "Pending"
                            );
                        }
                    }
                }

                dgvOrders.DataSource = dt;

                // Hide ID column
                if (dgvOrders.Columns.Count > 0 && dgvOrders.Columns["OrderID"] != null)
                    dgvOrders.Columns["OrderID"].Visible = false;

                lblStatus.Text = $"{pendingCount} pending orders | Last updated: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
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
                MessageBox.Show("Please select an order.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
            string orderNumber = dgvOrders.SelectedRows[0].Cells["Order Number"].Value.ToString();

            DialogResult result = MessageBox.Show($"Mark order {orderNumber} as Preparing?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.UpdateOrderStatus(orderId, "Preparing");
                    MessageBox.Show($"Order {orderNumber} marked as Preparing.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPendingOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnMarkReady_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderID"].Value);
            string orderNumber = dgvOrders.SelectedRows[0].Cells["Order Number"].Value.ToString();

            DialogResult result = MessageBox.Show($"Mark order {orderNumber} as Ready?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.UpdateOrderStatus(orderId, "Ready");
                    MessageBox.Show($"Order {orderNumber} marked as Ready.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPendingOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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