using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Takeaway_Restaurant_Management_System.Classes.Database;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public partial class frmReports : Form
    {
        private TabControl tabControl;
        private TabPage tabDailySales;
        private TabPage tabPopularItems;
        private TabPage tabStaffPerformance;

        // Daily Sales Tab Controls
        private DateTimePicker dtpSalesDate;
        private Button btnGenerateSales;
        private DataGridView dgvSales;
        private Label lblTotalOrders;
        private Label lblTotalRevenue;
        private Label lblAverageOrder;

        // Popular Items Tab Controls
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private Button btnGenerateItems;
        private DataGridView dgvPopularItems;
        private Label lblTotalItemsSold;

        // Staff Performance Tab Controls
        private DateTimePicker dtpStaffStart;
        private DateTimePicker dtpStaffEnd;
        private Button btnGenerateStaff;
        private DataGridView dgvStaffPerformance;
        private Label lblTopPerformer;

        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        public frmReports()
        {
            InitializeComponent();
            SetDefaultDates();
        }

        private void InitializeComponent()
        {
            this.Text = "📊 Reports - Takeaway Restaurant System";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.MinimumSize = new Size(1000, 600);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "📊 REPORTS & ANALYTICS";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 45);
            this.Controls.Add(lblTitle);

            // Tab Control
            tabControl = new TabControl();
            tabControl.Location = new Point(20, 80);
            tabControl.Size = new Size(1040, 540);
            tabControl.Font = new Font("Segoe UI", 10);
            this.Controls.Add(tabControl);

            // ========== TAB 1: DAILY SALES ==========
            tabDailySales = new TabPage("📅 Daily Sales");
            tabControl.TabPages.Add(tabDailySales);

            Panel panelDailySales = new Panel();
            panelDailySales.Dock = DockStyle.Fill;
            panelDailySales.BackColor = Color.White;
            panelDailySales.Padding = new Padding(15);
            tabDailySales.Controls.Add(panelDailySales);

            // Date Selection
            Label lblDate = new Label();
            lblDate.Text = "Select Date:";
            lblDate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDate.Location = new Point(15, 15);
            lblDate.Size = new Size(100, 25);
            panelDailySales.Controls.Add(lblDate);

            dtpSalesDate = new DateTimePicker();
            dtpSalesDate.Location = new Point(120, 12);
            dtpSalesDate.Size = new Size(150, 25);
            dtpSalesDate.Font = new Font("Segoe UI", 10);
            panelDailySales.Controls.Add(dtpSalesDate);

            btnGenerateSales = new Button();
            btnGenerateSales.Text = "📊 Generate Report";
            btnGenerateSales.Location = new Point(290, 10);
            btnGenerateSales.Size = new Size(150, 30);
            btnGenerateSales.BackColor = Color.FromArgb(52, 152, 219);
            btnGenerateSales.ForeColor = Color.White;
            btnGenerateSales.FlatStyle = FlatStyle.Flat;
            btnGenerateSales.Cursor = Cursors.Hand;
            btnGenerateSales.Click += BtnGenerateSales_Click;
            panelDailySales.Controls.Add(btnGenerateSales);

            // Summary Cards
            Panel summaryPanel = new Panel();
            summaryPanel.Location = new Point(15, 55);
            summaryPanel.Size = new Size(1000, 80);
            summaryPanel.BackColor = Color.FromArgb(240, 240, 240);
            panelDailySales.Controls.Add(summaryPanel);

            // Total Orders Card
            Panel cardOrders = CreateSummaryCard("📊 Total Orders", "0", 10, Color.FromArgb(52, 152, 219));
            summaryPanel.Controls.Add(cardOrders);

            // Total Revenue Card
            Panel cardRevenue = CreateSummaryCard("💰 Total Revenue", "$0.00", 210, Color.FromArgb(46, 204, 113));
            summaryPanel.Controls.Add(cardRevenue);

            // Average Order Card
            Panel cardAverage = CreateSummaryCard("📈 Average Order", "$0.00", 410, Color.FromArgb(241, 196, 15));
            summaryPanel.Controls.Add(cardAverage);

            lblTotalOrders = (Label)cardOrders.Controls.Find("lblValue", true)[0];
            lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
            lblAverageOrder = (Label)cardAverage.Controls.Find("lblValue", true)[0];

            // DataGridView
            dgvSales = new DataGridView();
            dgvSales.Location = new Point(15, 150);
            dgvSales.Size = new Size(1000, 320);
            dgvSales.BackgroundColor = Color.White;
            dgvSales.BorderStyle = BorderStyle.FixedSingle;
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            panelDailySales.Controls.Add(dgvSales);

            // ========== TAB 2: POPULAR ITEMS ==========
            tabPopularItems = new TabPage("🔥 Popular Items");
            tabControl.TabPages.Add(tabPopularItems);

            Panel panelPopularItems = new Panel();
            panelPopularItems.Dock = DockStyle.Fill;
            panelPopularItems.BackColor = Color.White;
            panelPopularItems.Padding = new Padding(15);
            tabPopularItems.Controls.Add(panelPopularItems);

            Label lblDateRange = new Label();
            lblDateRange.Text = "Date Range:";
            lblDateRange.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDateRange.Location = new Point(15, 15);
            lblDateRange.Size = new Size(100, 25);
            panelPopularItems.Controls.Add(lblDateRange);

            dtpStartDate = new DateTimePicker();
            dtpStartDate.Location = new Point(120, 12);
            dtpStartDate.Size = new Size(130, 25);
            panelPopularItems.Controls.Add(dtpStartDate);

            Label lblTo = new Label();
            lblTo.Text = "to";
            lblTo.Location = new Point(260, 15);
            lblTo.Size = new Size(30, 25);
            panelPopularItems.Controls.Add(lblTo);

            dtpEndDate = new DateTimePicker();
            dtpEndDate.Location = new Point(300, 12);
            dtpEndDate.Size = new Size(130, 25);
            panelPopularItems.Controls.Add(dtpEndDate);

            btnGenerateItems = new Button();
            btnGenerateItems.Text = "🔥 Generate Report";
            btnGenerateItems.Location = new Point(450, 10);
            btnGenerateItems.Size = new Size(150, 30);
            btnGenerateItems.BackColor = Color.FromArgb(52, 152, 219);
            btnGenerateItems.ForeColor = Color.White;
            btnGenerateItems.FlatStyle = FlatStyle.Flat;
            btnGenerateItems.Click += BtnGenerateItems_Click;
            panelPopularItems.Controls.Add(btnGenerateItems);

            Panel summaryItemsPanel = new Panel();
            summaryItemsPanel.Location = new Point(15, 55);
            summaryItemsPanel.Size = new Size(1000, 60);
            summaryItemsPanel.BackColor = Color.FromArgb(240, 240, 240);
            panelPopularItems.Controls.Add(summaryItemsPanel);

            Label lblTotalSoldLabel = new Label();
            lblTotalSoldLabel.Text = "📦 Total Items Sold:";
            lblTotalSoldLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTotalSoldLabel.Location = new Point(15, 18);
            lblTotalSoldLabel.Size = new Size(150, 25);
            summaryItemsPanel.Controls.Add(lblTotalSoldLabel);

            lblTotalItemsSold = new Label();
            lblTotalItemsSold.Text = "0";
            lblTotalItemsSold.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTotalItemsSold.ForeColor = Color.FromArgb(52, 152, 219);
            lblTotalItemsSold.Location = new Point(170, 15);
            lblTotalItemsSold.Size = new Size(100, 30);
            summaryItemsPanel.Controls.Add(lblTotalItemsSold);

            dgvPopularItems = new DataGridView();
            dgvPopularItems.Location = new Point(15, 130);
            dgvPopularItems.Size = new Size(1000, 340);
            dgvPopularItems.BackgroundColor = Color.White;
            dgvPopularItems.BorderStyle = BorderStyle.FixedSingle;
            dgvPopularItems.AllowUserToAddRows = false;
            dgvPopularItems.AllowUserToDeleteRows = false;
            dgvPopularItems.ReadOnly = true;
            dgvPopularItems.RowHeadersVisible = false;
            dgvPopularItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            panelPopularItems.Controls.Add(dgvPopularItems);

            // ========== TAB 3: STAFF PERFORMANCE ==========
            tabStaffPerformance = new TabPage("👥 Staff Performance");
            tabControl.TabPages.Add(tabStaffPerformance);

            Panel panelStaff = new Panel();
            panelStaff.Dock = DockStyle.Fill;
            panelStaff.BackColor = Color.White;
            panelStaff.Padding = new Padding(15);
            tabStaffPerformance.Controls.Add(panelStaff);

            Label lblStaffRange = new Label();
            lblStaffRange.Text = "Date Range:";
            lblStaffRange.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblStaffRange.Location = new Point(15, 15);
            lblStaffRange.Size = new Size(100, 25);
            panelStaff.Controls.Add(lblStaffRange);

            dtpStaffStart = new DateTimePicker();
            dtpStaffStart.Location = new Point(120, 12);
            dtpStaffStart.Size = new Size(130, 25);
            panelStaff.Controls.Add(dtpStaffStart);

            Label lblStaffTo = new Label();
            lblStaffTo.Text = "to";
            lblStaffTo.Location = new Point(260, 15);
            lblStaffTo.Size = new Size(30, 25);
            panelStaff.Controls.Add(lblStaffTo);

            dtpStaffEnd = new DateTimePicker();
            dtpStaffEnd.Location = new Point(300, 12);
            dtpStaffEnd.Size = new Size(130, 25);
            panelStaff.Controls.Add(dtpStaffEnd);

            btnGenerateStaff = new Button();
            btnGenerateStaff.Text = "👥 Generate Report";
            btnGenerateStaff.Location = new Point(450, 10);
            btnGenerateStaff.Size = new Size(150, 30);
            btnGenerateStaff.BackColor = Color.FromArgb(52, 152, 219);
            btnGenerateStaff.ForeColor = Color.White;
            btnGenerateStaff.Click += BtnGenerateStaff_Click;
            panelStaff.Controls.Add(btnGenerateStaff);

            Panel summaryStaffPanel = new Panel();
            summaryStaffPanel.Location = new Point(15, 55);
            summaryStaffPanel.Size = new Size(1000, 60);
            summaryStaffPanel.BackColor = Color.FromArgb(240, 240, 240);
            panelStaff.Controls.Add(summaryStaffPanel);

            Label lblTopPerformerLabel = new Label();
            lblTopPerformerLabel.Text = "🏆 Top Performer:";
            lblTopPerformerLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTopPerformerLabel.Location = new Point(15, 18);
            lblTopPerformerLabel.Size = new Size(130, 25);
            summaryStaffPanel.Controls.Add(lblTopPerformerLabel);

            lblTopPerformer = new Label();
            lblTopPerformer.Text = "Loading...";
            lblTopPerformer.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTopPerformer.ForeColor = Color.FromArgb(46, 204, 113);
            lblTopPerformer.Location = new Point(150, 15);
            lblTopPerformer.Size = new Size(300, 30);
            summaryStaffPanel.Controls.Add(lblTopPerformer);

            dgvStaffPerformance = new DataGridView();
            dgvStaffPerformance.Location = new Point(15, 130);
            dgvStaffPerformance.Size = new Size(1000, 340);
            dgvStaffPerformance.BackgroundColor = Color.White;
            dgvStaffPerformance.BorderStyle = BorderStyle.FixedSingle;
            dgvStaffPerformance.AllowUserToAddRows = false;
            dgvStaffPerformance.AllowUserToDeleteRows = false;
            dgvStaffPerformance.ReadOnly = true;
            dgvStaffPerformance.RowHeadersVisible = false;
            dgvStaffPerformance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            panelStaff.Controls.Add(dgvStaffPerformance);

            // Status Strip
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(52, 73, 94);
            statusStrip.ForeColor = Color.White;

            lblStatus = new ToolStripStatusLabel("✅ Ready");
            lblStatus.ForeColor = Color.White;

            statusStrip.Items.Add(lblStatus);
            this.Controls.Add(statusStrip);
        }

        private Panel CreateSummaryCard(string title, string value, int x, Color color)
        {
            Panel card = new Panel();
            card.Size = new Size(180, 70);
            card.Location = new Point(x, 5);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblTitle.Location = new Point(10, 8);
            lblTitle.Size = new Size(160, 20);
            card.Controls.Add(lblTitle);

            Label lblValue = new Label();
            lblValue.Name = "lblValue";
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblValue.ForeColor = color;
            lblValue.Location = new Point(10, 30);
            lblValue.Size = new Size(160, 35);
            lblValue.TextAlign = ContentAlignment.MiddleLeft;
            card.Controls.Add(lblValue);

            return card;
        }

        private void SetDefaultDates()
        {
            dtpSalesDate.Value = DateTime.Today;
            dtpStartDate.Value = DateTime.Today.AddDays(-30);
            dtpEndDate.Value = DateTime.Today;
            dtpStaffStart.Value = DateTime.Today.AddDays(-30);
            dtpStaffEnd.Value = DateTime.Today;
        }

        private void BtnGenerateSales_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "⏳ Loading sales report...";
                Application.DoEvents();

                var dt = DatabaseManager.Instance.GetDailySalesReport(dtpSalesDate.Value);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dgvSales.DataSource = dt;

                    int totalOrders = dt.Rows.Count;
                    decimal totalRevenue = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["TotalAmount"] != DBNull.Value)
                            totalRevenue += Convert.ToDecimal(row["TotalAmount"]);
                    }
                    decimal avgOrder = totalOrders > 0 ? totalRevenue / totalOrders : 0;

                    lblTotalOrders.Text = totalOrders.ToString();
                    lblTotalRevenue.Text = totalRevenue.ToString("C");
                    lblAverageOrder.Text = avgOrder.ToString("C");

                    lblStatus.Text = "✅ Sales report generated";
                }
                else
                {
                    dgvSales.DataSource = null;
                    lblTotalOrders.Text = "0";
                    lblTotalRevenue.Text = "$0.00";
                    lblAverageOrder.Text = "$0.00";
                    lblStatus.Text = "ℹ️ No sales data for this date";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating sales report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "❌ Error generating report";
            }
        }

        private void BtnGenerateItems_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "⏳ Loading popular items report...";
                Application.DoEvents();

                var dt = DatabaseManager.Instance.GetPopularItemsReport(dtpStartDate.Value, dtpEndDate.Value);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dgvPopularItems.DataSource = dt;

                    int totalItems = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["TotalQuantity"] != DBNull.Value)
                            totalItems += Convert.ToInt32(row["TotalQuantity"]);
                    }
                    lblTotalItemsSold.Text = totalItems.ToString();

                    lblStatus.Text = "✅ Popular items report generated";
                }
                else
                {
                    dgvPopularItems.DataSource = null;
                    lblTotalItemsSold.Text = "0";
                    lblStatus.Text = "ℹ️ No sales data in this date range";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating popular items report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "❌ Error generating report";
            }
        }

        private void BtnGenerateStaff_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "⏳ Loading staff performance report...";
                Application.DoEvents();

                var dt = DatabaseManager.Instance.GetStaffPerformanceReport(dtpStaffStart.Value, dtpStaffEnd.Value);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dgvStaffPerformance.DataSource = dt;

                    string topPerformer = "";
                    decimal maxSales = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["TotalSales"] != DBNull.Value)
                        {
                            decimal sales = Convert.ToDecimal(row["TotalSales"]);
                            if (sales > maxSales)
                            {
                                maxSales = sales;
                                topPerformer = row["FullName"].ToString();
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(topPerformer))
                    {
                        lblTopPerformer.Text = $"{topPerformer} - {maxSales:C}";
                    }
                    else
                    {
                        lblTopPerformer.Text = "No data";
                    }

                    lblStatus.Text = "✅ Staff performance report generated";
                }
                else
                {
                    dgvStaffPerformance.DataSource = null;
                    lblTopPerformer.Text = "No data";
                    lblStatus.Text = "ℹ️ No staff data in this date range";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating staff report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "❌ Error generating report";
            }
        }
    }
}