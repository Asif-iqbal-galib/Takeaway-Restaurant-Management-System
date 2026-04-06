using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmInventory : Form
    {
        private DataGridView dgvInventory;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnRefresh;
        private Button btnUpdateStock;
        private Button btnAddItem;
        private Button btnClose;
        private Label lblLowStockWarning;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblRecordCount;

        private List<Inventory> inventory;

        public frmInventory()
        {
            InitializeComponent();
            LoadInventory();
        }

        private void InitializeComponent()
        {
            this.Text = "📦 Inventory Management - Appeliano Restaurant";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "📦 INVENTORY MANAGEMENT";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 45);
            this.Controls.Add(lblTitle);

            // Low Stock Warning
            lblLowStockWarning = new Label();
            lblLowStockWarning.Text = "⚠️ Low Stock Items";
            lblLowStockWarning.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblLowStockWarning.ForeColor = Color.FromArgb(231, 76, 60);
            lblLowStockWarning.Location = new Point(20, 70);
            lblLowStockWarning.Size = new Size(150, 25);
            this.Controls.Add(lblLowStockWarning);

            // Search Panel
            Panel panelSearch = new Panel();
            panelSearch.Location = new Point(20, 100);
            panelSearch.Size = new Size(1040, 60);
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

            // DataGridView
            dgvInventory = new DataGridView();
            dgvInventory.Location = new Point(20, 170);
            dgvInventory.Size = new Size(1040, 330);
            dgvInventory.BackgroundColor = Color.White;
            dgvInventory.BorderStyle = BorderStyle.FixedSingle;
            dgvInventory.AllowUserToAddRows = false;
            dgvInventory.AllowUserToDeleteRows = false;
            dgvInventory.ReadOnly = true;
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventory.CellFormatting += DgvInventory_CellFormatting;
            this.Controls.Add(dgvInventory);

            // Button Panel
            Panel panelButtons = new Panel();
            panelButtons.Location = new Point(20, 510);
            panelButtons.Size = new Size(1040, 60);
            panelButtons.BackColor = Color.White;
            this.Controls.Add(panelButtons);

            btnUpdateStock = new Button();
            btnUpdateStock.Text = "📦 UPDATE STOCK";
            btnUpdateStock.Location = new Point(15, 12);
            btnUpdateStock.Size = new Size(130, 35);
            btnUpdateStock.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnUpdateStock.BackColor = Color.FromArgb(52, 152, 219);
            btnUpdateStock.ForeColor = Color.White;
            btnUpdateStock.FlatStyle = FlatStyle.Flat;
            btnUpdateStock.Click += BtnUpdateStock_Click;
            panelButtons.Controls.Add(btnUpdateStock);

            btnAddItem = new Button();
            btnAddItem.Text = "➕ ADD ITEM";
            btnAddItem.Location = new Point(160, 12);
            btnAddItem.Size = new Size(120, 35);
            btnAddItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAddItem.BackColor = Color.FromArgb(46, 204, 113);
            btnAddItem.ForeColor = Color.White;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Click += BtnAddItem_Click;
            panelButtons.Controls.Add(btnAddItem);

            btnClose = new Button();
            btnClose.Text = "❌ CLOSE";
            btnClose.Location = new Point(920, 12);
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

            lblRecordCount = new ToolStripStatusLabel("📦 0 items");
            lblRecordCount.ForeColor = Color.White;

            statusStrip.Items.Add(lblStatus);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(lblRecordCount);
            this.Controls.Add(statusStrip);
        }

        private void LoadInventory()
        {
            try
            {
                inventory = DatabaseManager.Instance.GetInventory();
                if (inventory == null) inventory = new List<Inventory>();
                RefreshDataGridView();
                CheckLowStock();
                lblRecordCount.Text = $"📦 {inventory.Count} items";
                lblStatus.Text = "✅ Loaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading inventory: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                inventory = new List<Inventory>();
            }
        }

        private void RefreshDataGridView(string searchTerm = "")
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Item Name", typeof(string));
            dt.Columns.Add("Quantity", typeof(decimal));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("Reorder Level", typeof(decimal));
            dt.Columns.Add("Status", typeof(string));

            int filteredCount = 0;
            int lowStockCount = 0;

            foreach (var item in inventory)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    if (!item.ItemName.ToLower().Contains(searchTerm.ToLower()))
                        continue;
                }

                string status = item.IsLowStock ? "⚠️ LOW STOCK" : "✅ OK";
                if (item.IsLowStock) lowStockCount++;

                dt.Rows.Add(
                    item.ItemID,
                    item.ItemName,
                    item.Quantity,
                    item.Unit,
                    item.ReorderLevel,
                    status
                );
                filteredCount++;
            }

            dgvInventory.DataSource = dt;
            if (dgvInventory.Columns["ID"] != null)
                dgvInventory.Columns["ID"].Visible = false;

            lblRecordCount.Text = $"📦 {filteredCount} items ({lowStockCount} low stock)";
        }

        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvInventory.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                if (e.Value.ToString().Contains("LOW"))
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 200, 200);
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }
        }

        private void CheckLowStock()
        {
            int lowStockCount = 0;
            foreach (var item in inventory)
            {
                if (item.IsLowStock) lowStockCount++;
            }

            if (lowStockCount > 0)
            {
                lblLowStockWarning.Text = $"⚠️ {lowStockCount} items are LOW on stock!";
                lblLowStockWarning.ForeColor = Color.FromArgb(231, 76, 60);
            }
            else
            {
                lblLowStockWarning.Text = "✅ All items in stock";
                lblLowStockWarning.ForeColor = Color.FromArgb(46, 204, 113);
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
            LoadInventory();
        }

        private void BtnUpdateStock_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int itemId = Convert.ToInt32(dgvInventory.SelectedRows[0].Cells["ID"].Value);
            string itemName = dgvInventory.SelectedRows[0].Cells["Item Name"].Value.ToString();
            decimal currentQty = Convert.ToDecimal(dgvInventory.SelectedRows[0].Cells["Quantity"].Value);

            using (var dialog = new frmStockUpdateDialog(itemId, itemName, currentQty))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadInventory();
                    lblStatus.Text = "✅ Stock updated";
                }
            }
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new frmInventoryItemDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadInventory();
                    lblStatus.Text = "✅ Item added";
                }
            }
        }
    }

    public class frmStockUpdateDialog : Form
    {
        private int itemId;
        private string itemName;
        private decimal currentQty;
        private NumericUpDown nudQuantity;
        private RadioButton rbAdd;
        private RadioButton rbSubtract;
        private Button btnUpdate;
        private Button btnCancel;

        public frmStockUpdateDialog(int id, string name, decimal qty)
        {
            itemId = id;
            itemName = name;
            currentQty = qty;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "📦 Update Stock";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblItem = new Label();
            lblItem.Text = $"Item: {itemName}";
            lblItem.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblItem.Location = new Point(20, 20);
            lblItem.Size = new Size(300, 25);
            this.Controls.Add(lblItem);

            Label lblCurrent = new Label();
            lblCurrent.Text = $"Current Stock: {currentQty}";
            lblCurrent.Location = new Point(20, 55);
            lblCurrent.Size = new Size(200, 25);
            this.Controls.Add(lblCurrent);

            rbAdd = new RadioButton();
            rbAdd.Text = "➕ Add Stock";
            rbAdd.Location = new Point(20, 95);
            rbAdd.Size = new Size(120, 25);
            rbAdd.Checked = true;
            this.Controls.Add(rbAdd);

            rbSubtract = new RadioButton();
            rbSubtract.Text = "➖ Remove Stock";
            rbSubtract.Location = new Point(150, 95);
            rbSubtract.Size = new Size(120, 25);
            this.Controls.Add(rbSubtract);

            Label lblQty = new Label();
            lblQty.Text = "Quantity:";
            lblQty.Location = new Point(20, 135);
            lblQty.Size = new Size(70, 25);
            this.Controls.Add(lblQty);

            nudQuantity = new NumericUpDown();
            nudQuantity.Location = new Point(95, 132);
            nudQuantity.Size = new Size(100, 25);
            nudQuantity.Minimum = 1;
            nudQuantity.Maximum = 1000;
            nudQuantity.Value = 1;
            this.Controls.Add(nudQuantity);

            btnUpdate = new Button();
            btnUpdate.Text = "💾 Update";
            btnUpdate.Location = new Point(95, 180);
            btnUpdate.Size = new Size(100, 35);
            btnUpdate.BackColor = Color.FromArgb(46, 204, 113);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Click += BtnUpdate_Click;
            this.Controls.Add(btnUpdate);

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(205, 180);
            btnCancel.Size = new Size(100, 35);
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnCancel);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            decimal qty = nudQuantity.Value;
            bool isAdd = rbAdd.Checked;

            try
            {
                DatabaseManager.Instance.UpdateInventoryQuantity(itemId, qty, isAdd);
                MessageBox.Show($"Stock updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class frmInventoryItemDialog : Form
    {
        private TextBox txtName;
        private NumericUpDown nudQuantity;
        private ComboBox cmbUnit;
        private NumericUpDown nudReorderLevel;
        private Button btnSave;
        private Button btnCancel;

        public frmInventoryItemDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "➕ Add Inventory Item";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblName = new Label();
            lblName.Text = "Item Name:";
            lblName.Location = new Point(20, 20);
            lblName.Size = new Size(100, 25);
            this.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(130, 17);
            txtName.Size = new Size(230, 25);
            this.Controls.Add(txtName);

            Label lblQty = new Label();
            lblQty.Text = "Initial Quantity:";
            lblQty.Location = new Point(20, 55);
            lblQty.Size = new Size(100, 25);
            this.Controls.Add(lblQty);

            nudQuantity = new NumericUpDown();
            nudQuantity.Location = new Point(130, 52);
            nudQuantity.Size = new Size(100, 25);
            nudQuantity.Minimum = 0;
            nudQuantity.Maximum = 10000;
            nudQuantity.Value = 100;
            this.Controls.Add(nudQuantity);

            Label lblUnit = new Label();
            lblUnit.Text = "Unit:";
            lblUnit.Location = new Point(20, 90);
            lblUnit.Size = new Size(100, 25);
            this.Controls.Add(lblUnit);

            cmbUnit = new ComboBox();
            cmbUnit.Location = new Point(130, 87);
            cmbUnit.Size = new Size(100, 25);
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.Items.AddRange(new object[] { "kg", "g", "liter", "ml", "piece", "box", "bag" });
            cmbUnit.SelectedIndex = 0;
            this.Controls.Add(cmbUnit);

            Label lblReorder = new Label();
            lblReorder.Text = "Reorder Level:";
            lblReorder.Location = new Point(20, 125);
            lblReorder.Size = new Size(100, 25);
            this.Controls.Add(lblReorder);

            nudReorderLevel = new NumericUpDown();
            nudReorderLevel.Location = new Point(130, 122);
            nudReorderLevel.Size = new Size(100, 25);
            nudReorderLevel.Minimum = 1;
            nudReorderLevel.Maximum = 1000;
            nudReorderLevel.Value = 20;
            this.Controls.Add(nudReorderLevel);

            btnSave = new Button();
            btnSave.Text = "💾 Save";
            btnSave.Location = new Point(130, 180);
            btnSave.Size = new Size(100, 35);
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(240, 180);
            btnCancel.Size = new Size(100, 35);
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Item name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Inventory newItem = new Inventory
                {
                    ItemName = txtName.Text.Trim(),
                    Quantity = nudQuantity.Value,
                    Unit = cmbUnit.SelectedItem.ToString(),
                    ReorderLevel = nudReorderLevel.Value
                };

                int newId = DatabaseManager.Instance.AddInventoryItem(newItem);

                if (newId > 0)
                {
                    MessageBox.Show("Item added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
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