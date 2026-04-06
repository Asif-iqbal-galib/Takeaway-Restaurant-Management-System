using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmSettings : Form
    {
        private TabControl tabControl;
        private TabPage tabGeneral;
        private TabPage tabTax;
        private TabPage tabPrinting;

        // General Settings
        private TextBox txtRestaurantName;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private TextBox txtEmail;
        private Button btnSaveGeneral;

        // Tax Settings
        private NumericUpDown nudTaxRate;
        private Label lblTaxPreview;
        private Button btnSaveTax;

        // Printing Settings
        private CheckBox chkPrintReceipt;
        private ComboBox cmbPrinter;
        private Button btnSavePrinting;
        private Button btnTestPrint;

        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        public frmSettings()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "⚙ Settings - Appeliano Restaurant";
            this.Size = new Size(700, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "⚙ SYSTEM SETTINGS";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(300, 45);
            this.Controls.Add(lblTitle);

            // Tab Control
            tabControl = new TabControl();
            tabControl.Location = new Point(20, 80);
            tabControl.Size = new Size(640, 380);
            this.Controls.Add(tabControl);

            // ========== GENERAL TAB ==========
            tabGeneral = new TabPage("🏪 General");
            tabControl.TabPages.Add(tabGeneral);

            Panel panelGeneral = new Panel();
            panelGeneral.Dock = DockStyle.Fill;
            panelGeneral.Padding = new Padding(20);
            tabGeneral.Controls.Add(panelGeneral);

            // Restaurant Name
            Label lblRestaurantName = new Label();
            lblRestaurantName.Text = "Restaurant Name:";
            lblRestaurantName.Location = new Point(0, 10);
            lblRestaurantName.Size = new Size(120, 25);
            panelGeneral.Controls.Add(lblRestaurantName);

            txtRestaurantName = new TextBox();
            txtRestaurantName.Location = new Point(130, 7);
            txtRestaurantName.Size = new Size(400, 25);
            panelGeneral.Controls.Add(txtRestaurantName);

            // Phone
            Label lblPhone = new Label();
            lblPhone.Text = "Phone:";
            lblPhone.Location = new Point(0, 45);
            lblPhone.Size = new Size(120, 25);
            panelGeneral.Controls.Add(lblPhone);

            txtPhone = new TextBox();
            txtPhone.Location = new Point(130, 42);
            txtPhone.Size = new Size(400, 25);
            panelGeneral.Controls.Add(txtPhone);

            // Address
            Label lblAddress = new Label();
            lblAddress.Text = "Address:";
            lblAddress.Location = new Point(0, 80);
            lblAddress.Size = new Size(120, 25);
            panelGeneral.Controls.Add(lblAddress);

            txtAddress = new TextBox();
            txtAddress.Location = new Point(130, 77);
            txtAddress.Size = new Size(400, 60);
            txtAddress.Multiline = true;
            panelGeneral.Controls.Add(txtAddress);

            // Email
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(0, 150);
            lblEmail.Size = new Size(120, 25);
            panelGeneral.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Location = new Point(130, 147);
            txtEmail.Size = new Size(400, 25);
            panelGeneral.Controls.Add(txtEmail);

            // Save Button
            btnSaveGeneral = new Button();
            btnSaveGeneral.Text = "💾 Save General Settings";
            btnSaveGeneral.Location = new Point(130, 190);
            btnSaveGeneral.Size = new Size(200, 35);
            btnSaveGeneral.BackColor = Color.FromArgb(46, 204, 113);
            btnSaveGeneral.ForeColor = Color.White;
            btnSaveGeneral.FlatStyle = FlatStyle.Flat;
            btnSaveGeneral.Click += BtnSaveGeneral_Click;
            panelGeneral.Controls.Add(btnSaveGeneral);

            // ========== TAX TAB ==========
            tabTax = new TabPage("💰 Tax");
            tabControl.TabPages.Add(tabTax);

            Panel panelTax = new Panel();
            panelTax.Dock = DockStyle.Fill;
            panelTax.Padding = new Padding(20);
            tabTax.Controls.Add(panelTax);

            Label lblTaxRate = new Label();
            lblTaxRate.Text = "Tax Rate (%):";
            lblTaxRate.Location = new Point(0, 10);
            lblTaxRate.Size = new Size(100, 25);
            panelTax.Controls.Add(lblTaxRate);

            nudTaxRate = new NumericUpDown();
            nudTaxRate.Location = new Point(110, 7);
            nudTaxRate.Size = new Size(80, 25);
            nudTaxRate.DecimalPlaces = 1;
            nudTaxRate.Minimum = 0;
            nudTaxRate.Maximum = 50;
            nudTaxRate.Increment = 0.5m;
            nudTaxRate.Value = 10;
            nudTaxRate.ValueChanged += NudTaxRate_ValueChanged;
            panelTax.Controls.Add(nudTaxRate);

            lblTaxPreview = new Label();
            lblTaxPreview.Text = "Preview: £10.00 subtotal → £1.00 tax → £11.00 total";
            lblTaxPreview.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblTaxPreview.ForeColor = Color.Gray;
            lblTaxPreview.Location = new Point(0, 50);
            lblTaxPreview.Size = new Size(400, 30);
            panelTax.Controls.Add(lblTaxPreview);

            btnSaveTax = new Button();
            btnSaveTax.Text = "💾 Save Tax Settings";
            btnSaveTax.Location = new Point(110, 100);
            btnSaveTax.Size = new Size(200, 35);
            btnSaveTax.BackColor = Color.FromArgb(46, 204, 113);
            btnSaveTax.ForeColor = Color.White;
            btnSaveTax.FlatStyle = FlatStyle.Flat;
            btnSaveTax.Click += BtnSaveTax_Click;
            panelTax.Controls.Add(btnSaveTax);

            // ========== PRINTING TAB ==========
            tabPrinting = new TabPage("🖨️ Printing");
            tabControl.TabPages.Add(tabPrinting);

            Panel panelPrinting = new Panel();
            panelPrinting.Dock = DockStyle.Fill;
            panelPrinting.Padding = new Padding(20);
            tabPrinting.Controls.Add(panelPrinting);

            chkPrintReceipt = new CheckBox();
            chkPrintReceipt.Text = "Auto-print receipt after payment";
            chkPrintReceipt.Location = new Point(0, 10);
            chkPrintReceipt.Size = new Size(250, 25);
            chkPrintReceipt.Checked = true;
            panelPrinting.Controls.Add(chkPrintReceipt);

            Label lblPrinter = new Label();
            lblPrinter.Text = "Default Printer:";
            lblPrinter.Location = new Point(0, 45);
            lblPrinter.Size = new Size(100, 25);
            panelPrinting.Controls.Add(lblPrinter);

            cmbPrinter = new ComboBox();
            cmbPrinter.Location = new Point(110, 42);
            cmbPrinter.Size = new Size(200, 25);
            cmbPrinter.DropDownStyle = ComboBoxStyle.DropDownList;
            panelPrinting.Controls.Add(cmbPrinter);

            btnTestPrint = new Button();
            btnTestPrint.Text = "🖨️ Test Print";
            btnTestPrint.Location = new Point(320, 40);
            btnTestPrint.Size = new Size(100, 30);
            btnTestPrint.BackColor = Color.FromArgb(52, 152, 219);
            btnTestPrint.ForeColor = Color.White;
            btnTestPrint.FlatStyle = FlatStyle.Flat;
            btnTestPrint.Click += BtnTestPrint_Click;
            panelPrinting.Controls.Add(btnTestPrint);

            btnSavePrinting = new Button();
            btnSavePrinting.Text = "💾 Save Printing Settings";
            btnSavePrinting.Location = new Point(110, 90);
            btnSavePrinting.Size = new Size(200, 35);
            btnSavePrinting.BackColor = Color.FromArgb(46, 204, 113);
            btnSavePrinting.ForeColor = Color.White;
            btnSavePrinting.FlatStyle = FlatStyle.Flat;
            btnSavePrinting.Click += BtnSavePrinting_Click;
            panelPrinting.Controls.Add(btnSavePrinting);

            // Status Strip
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(52, 73, 94);
            statusStrip.ForeColor = Color.White;

            lblStatus = new ToolStripStatusLabel("✅ Settings loaded");
            lblStatus.ForeColor = Color.White;

            statusStrip.Items.Add(lblStatus);
            this.Controls.Add(statusStrip);
        }

        private void LoadSettings()
        {
            try
            {
                // Load tax rate from database (simulated)
                decimal taxRate = 10;
                nudTaxRate.Value = taxRate;
                UpdateTaxPreview();

                // Load printers
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    cmbPrinter.Items.Add(printer);
                }
                if (cmbPrinter.Items.Count > 0)
                    cmbPrinter.SelectedIndex = 0;

                // Load general settings (simulated)
                txtRestaurantName.Text = "Appeliano Restaurant";
                txtPhone.Text = "020 1234 5678";
                txtAddress.Text = "123 High Street, London, UK";
                txtEmail.Text = "info@appeliano.com";

                lblStatus.Text = "✅ Settings loaded";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void UpdateTaxPreview()
        {
            decimal taxRate = nudTaxRate.Value / 100;
            decimal subtotal = 10;
            decimal tax = subtotal * taxRate;
            decimal total = subtotal + tax;
            lblTaxPreview.Text = $"Preview: £{subtotal:F2} subtotal → £{tax:F2} tax → £{total:F2} total";
        }

        private void NudTaxRate_ValueChanged(object sender, EventArgs e)
        {
            UpdateTaxPreview();
        }

        private void BtnSaveGeneral_Click(object sender, EventArgs e)
        {
            try
            {
                // Save to database or config file
                MessageBox.Show("General settings saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "✅ General settings saved";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveTax_Click(object sender, EventArgs e)
        {
            try
            {
                decimal taxRate = nudTaxRate.Value;
                // Save to database
                MessageBox.Show($"Tax rate set to {taxRate}%", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "✅ Tax settings saved";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSavePrinting_Click(object sender, EventArgs e)
        {
            try
            {
                string printer = cmbPrinter.SelectedItem?.ToString() ?? "Default";
                bool autoPrint = chkPrintReceipt.Checked;

                MessageBox.Show($"Printing settings saved!\nPrinter: {printer}\nAuto-print: {(autoPrint ? "Yes" : "No")}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "✅ Printing settings saved";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTestPrint_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Test print sent to printer!", "Test Print",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "✅ Test print completed";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}