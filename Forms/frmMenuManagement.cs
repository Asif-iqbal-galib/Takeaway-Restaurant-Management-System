using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public partial class frmMenuManagement : Form
    {
        // Controls
        private Label lblTitle;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnRefresh;
        private ComboBox cmbCategory;
        private DataGridView dgvMenuItems;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClose;
        private Panel panelSearch;
        private Panel panelButtons;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblRecordCount;

        // Data
        private List<MenuItemModel> menuItems;
        private List<Category> categories;

        public frmMenuManagement()
        {
            InitializeComponent();
            LoadCategories();
            LoadMenuItems();
        }

        private void InitializeComponent()
        {
            // Form Settings
            this.Text = "📋 Menu Management - Appeliano Restaurant";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title Label
            lblTitle = new Label();
            lblTitle.Text = "📋 MENU MANAGEMENT";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 45);
            this.Controls.Add(lblTitle);

            // Search Panel
            panelSearch = new Panel();
            panelSearch.Location = new Point(20, 70);
            panelSearch.Size = new Size(1140, 70);
            panelSearch.BackColor = Color.White;
            panelSearch.BorderStyle = BorderStyle.None;
            this.Controls.Add(panelSearch);

            Label lblSearch = new Label();
            lblSearch.Text = "🔍 Search:";
            lblSearch.Font = new Font("Segoe UI", 11);
            lblSearch.Location = new Point(15, 25);
            lblSearch.Size = new Size(80, 25);
            panelSearch.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(100, 22);
            txtSearch.Size = new Size(200, 27);
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            panelSearch.Controls.Add(txtSearch);

            btnSearch = new Button();
            btnSearch.Text = "🔍 Search";
            btnSearch.Location = new Point(310, 20);
            btnSearch.Size = new Size(100, 32);
            btnSearch.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSearch.BackColor = Color.FromArgb(52, 152, 219);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Click += BtnSearch_Click;
            panelSearch.Controls.Add(btnSearch);

            Label lblFilter = new Label();
            lblFilter.Text = "📋 Category:";
            lblFilter.Font = new Font("Segoe UI", 11);
            lblFilter.Location = new Point(430, 25);
            lblFilter.Size = new Size(90, 25);
            panelSearch.Controls.Add(lblFilter);

            cmbCategory = new ComboBox();
            cmbCategory.Location = new Point(525, 22);
            cmbCategory.Size = new Size(180, 27);
            cmbCategory.Font = new Font("Segoe UI", 11);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.SelectedIndexChanged += CmbCategory_SelectedIndexChanged;
            panelSearch.Controls.Add(cmbCategory);

            btnRefresh = new Button();
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Location = new Point(720, 20);
            btnRefresh.Size = new Size(100, 32);
            btnRefresh.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRefresh.BackColor = Color.FromArgb(46, 204, 113);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += BtnRefresh_Click;
            panelSearch.Controls.Add(btnRefresh);

            // DataGridView
            dgvMenuItems = new DataGridView();
            dgvMenuItems.Location = new Point(20, 150);
            dgvMenuItems.Size = new Size(1140, 400);
            dgvMenuItems.BackgroundColor = Color.White;
            dgvMenuItems.BorderStyle = BorderStyle.FixedSingle;
            dgvMenuItems.AllowUserToAddRows = false;
            dgvMenuItems.AllowUserToDeleteRows = false;
            dgvMenuItems.ReadOnly = true;
            dgvMenuItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenuItems.MultiSelect = false;
            dgvMenuItems.RowHeadersVisible = false;
            dgvMenuItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMenuItems.CellFormatting += DgvMenuItems_CellFormatting;
            this.Controls.Add(dgvMenuItems);

            // Button Panel
            panelButtons = new Panel();
            panelButtons.Location = new Point(20, 560);
            panelButtons.Size = new Size(1140, 70);
            panelButtons.BackColor = Color.White;
            this.Controls.Add(panelButtons);

            btnAdd = new Button();
            btnAdd.Text = "➕ ADD NEW ITEM";
            btnAdd.Location = new Point(15, 15);
            btnAdd.Size = new Size(150, 40);
            btnAdd.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnAdd.BackColor = Color.FromArgb(46, 204, 113);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Click += BtnAdd_Click;
            panelButtons.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "✏️ EDIT SELECTED";
            btnEdit.Location = new Point(180, 15);
            btnEdit.Size = new Size(150, 40);
            btnEdit.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnEdit.BackColor = Color.FromArgb(241, 196, 15);
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.Click += BtnEdit_Click;
            panelButtons.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "🗑️ DELETE SELECTED";
            btnDelete.Location = new Point(345, 15);
            btnDelete.Size = new Size(150, 40);
            btnDelete.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Click += BtnDelete_Click;
            panelButtons.Controls.Add(btnDelete);

            btnClose = new Button();
            btnClose.Text = "❌ CLOSE";
            btnClose.Location = new Point(1000, 15);
            btnClose.Size = new Size(120, 40);
            btnClose.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnClose.BackColor = Color.FromArgb(149, 165, 166);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();
            panelButtons.Controls.Add(btnClose);

            // Status Strip
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(52, 73, 94);
            statusStrip.ForeColor = Color.White;

            lblStatus = new ToolStripStatusLabel("✅ Ready");
            lblStatus.ForeColor = Color.White;

            lblRecordCount = new ToolStripStatusLabel("📊 0 items");
            lblRecordCount.ForeColor = Color.White;

            statusStrip.Items.Add(lblStatus);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(lblRecordCount);
            this.Controls.Add(statusStrip);
        }

        private void LoadCategories()
        {
            try
            {
                categories = DatabaseManager.Instance.GetCategories();

                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("🍔 ALL CATEGORIES");

                foreach (var cat in categories)
                {
                    if (cat != null)
                    {
                        string icon = GetCategoryIcon(cat.CategoryName);
                        cmbCategory.Items.Add($"{icon} {cat.CategoryName}");
                    }
                }

                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenuItems()
        {
            try
            {
                menuItems = DatabaseManager.Instance.GetMenuItems();

                if (menuItems == null)
                    menuItems = new List<MenuItemModel>();

                RefreshDataGridView();

                lblRecordCount.Text = $"📊 {menuItems.Count} items";
                lblStatus.Text = "✅ Loaded successfully";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu items: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                menuItems = new List<MenuItemModel>();
                RefreshDataGridView();
                lblStatus.Text = "❌ Load failed";
            }
        }

        private string GetCategoryIcon(string category)
        {
            if (string.IsNullOrEmpty(category))
                return "📋";

            if (category == "Burgers") return "🍔";
            else if (category == "Pizzas") return "🍕";
            else if (category == "Appetizers") return "🍟";
            else if (category == "Beverages") return "🥤";
            else if (category == "Desserts") return "🍰";
            else if (category == "Salads") return "🥗";
            else if (category == "Chicken") return "🍗";
            else if (category == "Rice") return "🍚";
            else return "📋";
        }

        private void RefreshDataGridView(string searchTerm = "", string categoryFilter = "")
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("🍔 Item", typeof(string));
            dt.Columns.Add("📋 Category", typeof(string));
            dt.Columns.Add("💰 Price", typeof(string));
            dt.Columns.Add("⏱️ Prep Time", typeof(string));
            dt.Columns.Add("✅ Available", typeof(string));
            dt.Columns.Add("ID", typeof(int));

            int filteredCount = 0;

            if (menuItems == null || menuItems.Count == 0)
            {
                dgvMenuItems.DataSource = dt;
                return;
            }

            foreach (var item in menuItems)
            {
                if (item == null) continue;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    if (!item.Name.ToLower().Contains(searchTerm.ToLower()))
                        continue;
                }

                // Apply category filter
                if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "🍔 ALL CATEGORIES")
                {
                    string catName = categoryFilter.Contains(" ") ? categoryFilter.Substring(categoryFilter.IndexOf(' ') + 1) : categoryFilter;
                    if (item.CategoryName != catName)
                        continue;
                }

                string itemName = GetCategoryIcon(item.CategoryName) + " " + item.Name;

                dt.Rows.Add(
                    itemName,
                    item.CategoryName,
                    item.Price.ToString("C"),
                    item.PreparationTime + " min",
                    item.IsAvailable ? "✅ Yes" : "❌ No",
                    item.ItemID
                );

                filteredCount++;
            }

            dgvMenuItems.DataSource = dt;

            if (dgvMenuItems.Columns["ID"] != null)
                dgvMenuItems.Columns["ID"].Visible = false;

            lblRecordCount.Text = $"📊 {filteredCount} items";
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            string selectedCategory = cmbCategory.SelectedItem?.ToString() ?? "🍔 ALL CATEGORIES";
            RefreshDataGridView(searchTerm, selectedCategory);
        }

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            string selectedCategory = cmbCategory.SelectedItem?.ToString() ?? "🍔 ALL CATEGORIES";
            RefreshDataGridView(searchTerm, selectedCategory);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            string selectedCategory = cmbCategory.SelectedItem?.ToString() ?? "🍔 ALL CATEGORIES";
            RefreshDataGridView(searchTerm, selectedCategory);
            lblStatus.Text = "✅ Search applied";
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
            LoadMenuItems();
            lblStatus.Text = "✅ Refreshed";
        }

        private void DgvMenuItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvMenuItems.Columns[e.ColumnIndex].Name == "✅ Available" && e.Value != null)
            {
                if (e.Value.ToString().Contains("✅"))
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new frmMenuItemDialog(null, categories))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadMenuItems();
                    RefreshParentDashboard(); // Refresh dashboard stats
                    lblStatus.Text = "✅ Item added successfully";
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMenuItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Please select an item to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int itemId = Convert.ToInt32(dgvMenuItems.SelectedRows[0].Cells["ID"].Value);
            MenuItemModel selectedItem = menuItems.Find(i => i.ItemID == itemId);

            if (selectedItem != null)
            {
                using (var dialog = new frmMenuItemDialog(selectedItem, categories))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        LoadMenuItems();
                        RefreshParentDashboard(); // Refresh dashboard stats
                        lblStatus.Text = "✅ Item updated successfully";
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMenuItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Please select an item to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int itemId = Convert.ToInt32(dgvMenuItems.SelectedRows[0].Cells["ID"].Value);
            string itemName = dgvMenuItems.SelectedRows[0].Cells["🍔 Item"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"🗑️ Are you sure you want to delete:\n{itemName}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.DeleteMenuItem(itemId);
                    LoadMenuItems();
                    RefreshParentDashboard(); // Refresh dashboard stats
                    lblStatus.Text = "✅ Item deleted successfully";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting item: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshParentDashboard()
        {
            // Find the dashboard form and refresh its stats
            foreach (Form form in Application.OpenForms)
            {
                if (form is frmMainDashboard dashboard)
                {
                    dashboard.LoadDashboardStats();
                    break;
                }
            }
        }
    }

    // Dialog for Add/Edit Menu Items
    public class frmMenuItemDialog : Form
    {
        private MenuItemModel item;
        private List<Category> categories;
        private TextBox txtName;
        private ComboBox cmbCategory;
        private NumericUpDown nudPrice;
        private TextBox txtDescription;
        private NumericUpDown nudPrepTime;
        private CheckBox chkAvailable;
        private CheckBox chkVegetarian;
        private CheckBox chkSpicy;
        private Button btnSave;
        private Button btnCancel;

        public frmMenuItemDialog(MenuItemModel existingItem, List<Category> categoryList)
        {
            item = existingItem;
            categories = categoryList;
            InitializeComponent();
            LoadCategories();

            if (item != null)
            {
                this.Text = "✏️ Edit Menu Item";
                LoadItemData();
            }
            else
            {
                this.Text = "➕ Add New Menu Item";
                chkAvailable.Checked = true;
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Item Name
            Label lblName = new Label();
            lblName.Text = "Item Name:";
            lblName.Location = new Point(20, 20);
            lblName.Size = new Size(100, 25);
            this.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(130, 17);
            txtName.Size = new Size(330, 25);
            this.Controls.Add(txtName);

            // Category
            Label lblCategory = new Label();
            lblCategory.Text = "Category:";
            lblCategory.Location = new Point(20, 55);
            lblCategory.Size = new Size(100, 25);
            this.Controls.Add(lblCategory);

            cmbCategory = new ComboBox();
            cmbCategory.Location = new Point(130, 52);
            cmbCategory.Size = new Size(200, 25);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbCategory);

            // Price
            Label lblPrice = new Label();
            lblPrice.Text = "Price (£):";
            lblPrice.Location = new Point(20, 90);
            lblPrice.Size = new Size(100, 25);
            this.Controls.Add(lblPrice);

            nudPrice = new NumericUpDown();
            nudPrice.Location = new Point(130, 87);
            nudPrice.Size = new Size(100, 25);
            nudPrice.DecimalPlaces = 2;
            nudPrice.Minimum = 0;
            nudPrice.Maximum = 100;
            nudPrice.Increment = 0.5m;
            this.Controls.Add(nudPrice);

            // Preparation Time
            Label lblPrepTime = new Label();
            lblPrepTime.Text = "Prep Time (min):";
            lblPrepTime.Location = new Point(20, 125);
            lblPrepTime.Size = new Size(100, 25);
            this.Controls.Add(lblPrepTime);

            nudPrepTime = new NumericUpDown();
            nudPrepTime.Location = new Point(130, 122);
            nudPrepTime.Size = new Size(80, 25);
            nudPrepTime.Minimum = 1;
            nudPrepTime.Maximum = 60;
            nudPrepTime.Value = 10;
            this.Controls.Add(nudPrepTime);

            // Description
            Label lblDescription = new Label();
            lblDescription.Text = "Description:";
            lblDescription.Location = new Point(20, 160);
            lblDescription.Size = new Size(100, 25);
            this.Controls.Add(lblDescription);

            txtDescription = new TextBox();
            txtDescription.Location = new Point(130, 157);
            txtDescription.Size = new Size(330, 60);
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(txtDescription);

            // Availability
            chkAvailable = new CheckBox();
            chkAvailable.Text = "Available";
            chkAvailable.Location = new Point(130, 230);
            chkAvailable.Size = new Size(100, 25);
            this.Controls.Add(chkAvailable);

            // Buttons
            btnSave = new Button();
            btnSave.Text = "💾 Save";
            btnSave.Location = new Point(130, 280);
            btnSave.Size = new Size(100, 35);
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button();
            btnCancel.Text = "❌ Cancel";
            btnCancel.Location = new Point(250, 280);
            btnCancel.Size = new Size(100, 35);
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnCancel);
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            foreach (var cat in categories)
            {
                if (cat != null)
                {
                    cmbCategory.Items.Add(cat.CategoryName);
                }
            }
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void LoadItemData()
        {
            txtName.Text = item.Name;
            cmbCategory.SelectedItem = item.CategoryName;
            nudPrice.Value = item.Price;
            nudPrepTime.Value = item.PreparationTime;
            txtDescription.Text = item.Description;
            chkAvailable.Checked = item.IsAvailable;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter item name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (item == null)
                {
                    // ADD NEW ITEM
                    MenuItemModel newItem = new MenuItemModel
                    {
                        Name = txtName.Text.Trim(),
                        CategoryID = categories[cmbCategory.SelectedIndex].CategoryID,
                        Price = nudPrice.Value,
                        Description = txtDescription.Text,
                        PreparationTime = (int)nudPrepTime.Value,
                        IsAvailable = chkAvailable.Checked,
                        IsVegetarian = false,
                        IsSpicy = false
                    };

                    int newId = DatabaseManager.Instance.AddMenuItem(newItem);

                    if (newId > 0)
                    {
                        MessageBox.Show($"✅ Item '{txtName.Text}' added successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add item.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // UPDATE EXISTING ITEM
                    item.Name = txtName.Text.Trim();
                    item.CategoryID = categories[cmbCategory.SelectedIndex].CategoryID;
                    item.Price = nudPrice.Value;
                    item.Description = txtDescription.Text;
                    item.PreparationTime = (int)nudPrepTime.Value;
                    item.IsAvailable = chkAvailable.Checked;

                    int rowsAffected = DatabaseManager.Instance.UpdateMenuItem(item);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"✅ Item '{item.Name}' updated successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update item.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}