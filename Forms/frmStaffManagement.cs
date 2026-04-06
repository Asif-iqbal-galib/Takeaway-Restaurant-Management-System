using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmStaffManagement : Form
    {
        // Controls
        private Label lblTitle;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnRefresh;
        private DataGridView dgvStaff;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDeactivate;
        private Button btnActivate;
        private Button btnClose;
        private Panel panelSearch;
        private Panel panelButtons;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblRecordCount;

        // Data
        private List<Staff> staffList;

        public frmStaffManagement()
        {
            // Check if user is Admin first
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show("Only Administrators can access Staff Management.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            InitializeComponent();
            LoadStaff();
        }

        private void InitializeComponent()
        {
            // Form Settings
            this.Text = "👤 Staff Management - Takeaway Restaurant System";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(900, 600);
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title
            lblTitle = new Label();
            lblTitle.Text = "👤 STAFF MANAGEMENT";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 45);

            // Search Panel
            panelSearch = new Panel();
            panelSearch.Location = new Point(20, 70);
            panelSearch.Size = new Size(1040, 60);
            panelSearch.BackColor = Color.White;

            Label lblSearch = new Label();
            lblSearch.Text = "🔍 Search:";
            lblSearch.Font = new Font("Segoe UI", 11);
            lblSearch.Location = new Point(15, 18);
            lblSearch.Size = new Size(70, 25);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(90, 15);
            txtSearch.Size = new Size(250, 25);
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnSearch = new Button();
            btnSearch.Text = "🔍 Search";
            btnSearch.Location = new Point(350, 13);
            btnSearch.Size = new Size(100, 30);
            btnSearch.BackColor = Color.FromArgb(52, 152, 219);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Click += BtnSearch_Click;

            btnRefresh = new Button();
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Location = new Point(460, 13);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.BackColor = Color.FromArgb(46, 204, 113);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += BtnRefresh_Click;

            panelSearch.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, btnSearch, btnRefresh
            });

            // DataGridView
            dgvStaff = new DataGridView();
            dgvStaff.Location = new Point(20, 140);
            dgvStaff.Size = new Size(1040, 400);
            dgvStaff.BackgroundColor = Color.White;
            dgvStaff.BorderStyle = BorderStyle.FixedSingle;
            dgvStaff.AllowUserToAddRows = false;
            dgvStaff.AllowUserToDeleteRows = false;
            dgvStaff.ReadOnly = true;
            dgvStaff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStaff.MultiSelect = false;
            dgvStaff.RowHeadersVisible = false;
            dgvStaff.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Button Panel
            panelButtons = new Panel();
            panelButtons.Location = new Point(20, 550);
            panelButtons.Size = new Size(1040, 70);
            panelButtons.BackColor = Color.White;

            btnAdd = new Button();
            btnAdd.Text = "➕ ADD STAFF";
            btnAdd.Location = new Point(15, 15);
            btnAdd.Size = new Size(120, 40);
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.BackColor = Color.FromArgb(46, 204, 113);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button();
            btnEdit.Text = "✏️ EDIT";
            btnEdit.Location = new Point(145, 15);
            btnEdit.Size = new Size(100, 40);
            btnEdit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnEdit.BackColor = Color.FromArgb(241, 196, 15);
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Click += BtnEdit_Click;

            btnDeactivate = new Button();
            btnDeactivate.Text = "🔴 DEACTIVATE";
            btnDeactivate.Location = new Point(255, 15);
            btnDeactivate.Size = new Size(120, 40);
            btnDeactivate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnDeactivate.BackColor = Color.FromArgb(231, 76, 60);
            btnDeactivate.ForeColor = Color.White;
            btnDeactivate.FlatStyle = FlatStyle.Flat;
            btnDeactivate.Click += BtnDeactivate_Click;

            btnActivate = new Button();
            btnActivate.Text = "🟢 ACTIVATE";
            btnActivate.Location = new Point(385, 15);
            btnActivate.Size = new Size(100, 40);
            btnActivate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnActivate.BackColor = Color.FromArgb(46, 204, 113);
            btnActivate.ForeColor = Color.White;
            btnActivate.FlatStyle = FlatStyle.Flat;
            btnActivate.Click += BtnActivate_Click;

            btnClose = new Button();
            btnClose.Text = "❌ CLOSE";
            btnClose.Location = new Point(900, 15);
            btnClose.Size = new Size(100, 40);
            btnClose.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnClose.BackColor = Color.FromArgb(149, 165, 166);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();

            panelButtons.Controls.AddRange(new Control[] {
                btnAdd, btnEdit, btnDeactivate, btnActivate, btnClose
            });

            // Status Strip
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(52, 73, 94);
            statusStrip.ForeColor = Color.White;

            lblStatus = new ToolStripStatusLabel("✅ Ready");
            lblRecordCount = new ToolStripStatusLabel("👥 0 staff");

            statusStrip.Items.Add(lblStatus);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(lblRecordCount);

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblTitle, panelSearch, dgvStaff, panelButtons, statusStrip
            });
        }

        private void LoadStaff()
        {
            try
            {
                staffList = DatabaseManager.Instance.GetAllStaff();

                if (staffList == null)
                    staffList = new List<Staff>();

                RefreshDataGridView();
                lblRecordCount.Text = $"👥 {staffList.Count} staff";
                lblStatus.Text = "✅ Loaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                staffList = new List<Staff>();
            }
        }

        private void RefreshDataGridView(string searchTerm = "")
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("Full Name", typeof(string));
            dt.Columns.Add("Role", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Hire Date", typeof(string));

            foreach (var staff in staffList)
            {
                if (staff == null) continue;

                // Apply search filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    string term = searchTerm.ToLower();
                    bool matches = (staff.Username != null && staff.Username.ToLower().Contains(term)) ||
                                  (staff.FullName != null && staff.FullName.ToLower().Contains(term)) ||
                                  (staff.Role != null && staff.Role.ToLower().Contains(term)) ||
                                  (staff.Phone != null && staff.Phone.Contains(term));

                    if (!matches) continue;
                }

                dt.Rows.Add(
                    staff.StaffID,
                    staff.Username ?? "",
                    staff.FullName ?? "",
                    staff.Role ?? "",
                    staff.Phone ?? "",
                    staff.Email ?? "",
                    staff.IsActive ? "✅ Active" : "❌ Inactive",
                    staff.HireDate.ToShortDateString()
                );
            }

            dgvStaff.DataSource = dt;

            if (dgvStaff.Columns["ID"] != null)
                dgvStaff.Columns["ID"].Visible = false;
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
            LoadStaff();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new frmStaffDialog(null))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadStaff();
                    lblStatus.Text = "✅ Staff added";
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStaff.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a staff member to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int staffId = Convert.ToInt32(dgvStaff.SelectedRows[0].Cells["ID"].Value);
            Staff selected = staffList.Find(s => s.StaffID == staffId);

            if (selected != null)
            {
                using (var dialog = new frmStaffDialog(selected))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        LoadStaff();
                        lblStatus.Text = "✅ Staff updated";
                    }
                }
            }
        }

        private void BtnDeactivate_Click(object sender, EventArgs e)
        {
            if (dgvStaff.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a staff member.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int staffId = Convert.ToInt32(dgvStaff.SelectedRows[0].Cells["ID"].Value);
            string fullName = dgvStaff.SelectedRows[0].Cells["Full Name"].Value.ToString();

            // Don't let deactivate yourself
            if (staffId == CurrentUser.UserID)
            {
                MessageBox.Show("You cannot deactivate your own account.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Deactivate {fullName}? They will not be able to login.",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.SetStaffActiveStatus(staffId, false);
                    MessageBox.Show($"✅ {fullName} has been deactivated.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStaff();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnActivate_Click(object sender, EventArgs e)
        {
            if (dgvStaff.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a staff member.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int staffId = Convert.ToInt32(dgvStaff.SelectedRows[0].Cells["ID"].Value);
            string fullName = dgvStaff.SelectedRows[0].Cells["Full Name"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Activate {fullName}? They will be able to login again.",
                "Confirm Activate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DatabaseManager.Instance.SetStaffActiveStatus(staffId, true);
                    MessageBox.Show($"✅ {fullName} has been activated.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStaff();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    // Dialog for Add/Edit Staff
    public class frmStaffDialog : Form
    {
        private Staff staff;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtFullName;
        private ComboBox cmbRole;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnCancel;
        private Label lblPasswordHint;

        public frmStaffDialog(Staff existingStaff = null)
        {
            staff = existingStaff;
            InitializeComponent();

            if (staff != null)
            {
                this.Text = "✏️ Edit Staff";
                LoadStaffData();
            }
            else
            {
                this.Text = "➕ Add New Staff";
                chkActive.Checked = true;
                chkActive.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Username
            Label lblUsernameLabel = new Label();
            lblUsernameLabel.Text = "Username:";
            lblUsernameLabel.Location = new Point(20, 20);
            lblUsernameLabel.Size = new Size(100, 25);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(130, 17);
            txtUsername.Size = new Size(250, 25);

            // Password
            Label lblPasswordLabel = new Label();
            lblPasswordLabel.Text = "Password:";
            lblPasswordLabel.Location = new Point(20, 55);
            lblPasswordLabel.Size = new Size(100, 25);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(130, 52);
            txtPassword.Size = new Size(250, 25);
            txtPassword.UseSystemPasswordChar = true;

            // Password hint label
            lblPasswordHint = new Label();
            lblPasswordHint.Text = "(Leave blank to keep current password)";
            lblPasswordHint.ForeColor = Color.Gray;
            lblPasswordHint.Location = new Point(130, 77);
            lblPasswordHint.Size = new Size(250, 20);
            lblPasswordHint.Font = new Font("Segoe UI", 8, FontStyle.Italic);

            // Full Name
            Label lblFullNameLabel = new Label();
            lblFullNameLabel.Text = "Full Name:";
            lblFullNameLabel.Location = new Point(20, 105);
            lblFullNameLabel.Size = new Size(100, 25);

            txtFullName = new TextBox();
            txtFullName.Location = new Point(130, 102);
            txtFullName.Size = new Size(250, 25);

            // Role
            Label lblRoleLabel = new Label();
            lblRoleLabel.Text = "Role:";
            lblRoleLabel.Location = new Point(20, 140);
            lblRoleLabel.Size = new Size(100, 25);

            cmbRole = new ComboBox();
            cmbRole.Location = new Point(130, 137);
            cmbRole.Size = new Size(150, 25);
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Items.AddRange(new object[] { "Admin", "Cashier", "Kitchen", "Delivery" });

            // Phone
            Label lblPhoneLabel = new Label();
            lblPhoneLabel.Text = "Phone:";
            lblPhoneLabel.Location = new Point(20, 175);
            lblPhoneLabel.Size = new Size(100, 25);

            txtPhone = new TextBox();
            txtPhone.Location = new Point(130, 172);
            txtPhone.Size = new Size(250, 25);

            // Email
            Label lblEmailLabel = new Label();
            lblEmailLabel.Text = "Email:";
            lblEmailLabel.Location = new Point(20, 210);
            lblEmailLabel.Size = new Size(100, 25);

            txtEmail = new TextBox();
            txtEmail.Location = new Point(130, 207);
            txtEmail.Size = new Size(250, 25);

            // Active checkbox
            chkActive = new CheckBox();
            chkActive.Text = "Active Account";
            chkActive.Location = new Point(130, 245);
            chkActive.Size = new Size(120, 25);

            // Buttons
            btnSave = new Button();
            btnSave.Text = "💾 Save";
            btnSave.Location = new Point(130, 290);
            btnSave.Size = new Size(100, 35);
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button();
            btnCancel.Text = "❌ Cancel";
            btnCancel.Location = new Point(240, 290);
            btnCancel.Size = new Size(100, 35);
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Add all controls
            this.Controls.AddRange(new Control[] {
                lblUsernameLabel, txtUsername,
                lblPasswordLabel, txtPassword, lblPasswordHint,
                lblFullNameLabel, txtFullName,
                lblRoleLabel, cmbRole,
                lblPhoneLabel, txtPhone,
                lblEmailLabel, txtEmail,
                chkActive,
                btnSave, btnCancel
            });
        }

        private void LoadStaffData()
        {
            txtUsername.Text = staff.Username;
            txtFullName.Text = staff.FullName;
            cmbRole.SelectedItem = staff.Role;
            txtPhone.Text = staff.Phone;
            txtEmail.Text = staff.Email;
            chkActive.Checked = staff.IsActive;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (staff == null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required for new staff.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full Name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (staff == null)
                {
                    // ADD NEW STAFF
                    Staff newStaff = new Staff
                    {
                        Username = txtUsername.Text.Trim(),
                        PasswordHash = txtPassword.Text, // In real app, hash this!
                        FullName = txtFullName.Text.Trim(),
                        Role = cmbRole.SelectedItem.ToString(),
                        Phone = txtPhone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        HireDate = DateTime.Now,
                        IsActive = true
                    };

                    int newId = DatabaseManager.Instance.AddStaff(newStaff);

                    if (newId > 0)
                    {
                        MessageBox.Show($"✅ Staff added successfully!\n\nUsername: {txtUsername.Text}\nPassword: {txtPassword.Text}\n\nPlease save these credentials.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add staff.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // UPDATE EXISTING STAFF
                    staff.Username = txtUsername.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        staff.PasswordHash = txtPassword.Text; // In real app, hash this!
                    }
                    staff.FullName = txtFullName.Text.Trim();
                    staff.Role = cmbRole.SelectedItem.ToString();
                    staff.Phone = txtPhone.Text.Trim();
                    staff.Email = txtEmail.Text.Trim();
                    staff.IsActive = chkActive.Checked;

                    int rowsAffected = DatabaseManager.Instance.UpdateStaff(staff);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"✅ Staff updated successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update staff.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}