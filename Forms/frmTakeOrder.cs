using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmTakeOrder : Form
    {
        // Customer Section
        private GroupBox grpCustomer;
        private TextBox txtCustomerName;
        private TextBox txtCustomerPhone;
        private TextBox txtCustomerAddress;
        private Button btnSearchCustomer;

        // Delivery Section
        private CheckBox chkDelivery;
        private Panel panelDelivery;
        private TextBox txtDeliveryAddress;

        // Menu Tabs
        private TabControl tabMenu;
        private TabPage tabBurgers;
        private TabPage tabPizzas;
        private TabPage tabChicken;
        private TabPage tabRice;
        private TabPage tabBeverages;
        private TabPage tabSides;

        // Order Display
        private GroupBox grpOrder;
        private DataGridView dgvOrder;
        private Button btnRemove;
        private Label lblItemCount;
        private Label lblSubtotal;
        private Label lblTax;
        private Label lblTotal;

        // Payment Section
        private GroupBox grpPayment;
        private ComboBox cmbPaymentMethod;
        private TextBox txtAmountReceived;
        private Label lblChange;
        private Button btnProcessPayment;
        private Button btnCancelOrder;

        // Add to Order Controls
        private NumericUpDown nudQuantity;
        private Button btnAddToOrder;
        private CheckBox chkMakeMeal;
        private ComboBox cmbDrinkChoice;
        private ComboBox cmbFriesChoice;

        // Data
        private List<MenuItemModel> menuItems;
        private List<OrderItem> currentOrder;
        private decimal taxRate = 0.10m;
        private decimal mealPrice = 2.00m;

        public frmTakeOrder()
        {
            InitializeComponent();
            LoadMenuItems();
            currentOrder = new List<OrderItem>();
            UpdateOrderDisplay();
        }

        private void InitializeComponent()
        {
            this.Text = "Take Order - Appeliano Restaurant";
            this.Size = new Size(1350, 780);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.MinimumSize = new Size(1200, 700);

            CreateCustomerSection();
            CreateDeliverySection();
            CreateMenuTabs();
            CreateOrderSection();
            CreatePaymentSection();
        }

        private void CreateCustomerSection()
        {
            grpCustomer = new GroupBox();
            grpCustomer.Text = "Customer Information";
            grpCustomer.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpCustomer.Location = new Point(12, 10);
            grpCustomer.Size = new Size(1310, 65);
            grpCustomer.BackColor = Color.White;
            this.Controls.Add(grpCustomer);

            // Name
            Label lblName = new Label();
            lblName.Text = "Name:";
            lblName.Font = new Font("Segoe UI", 9);
            lblName.Location = new Point(15, 25);
            lblName.Size = new Size(45, 22);
            grpCustomer.Controls.Add(lblName);

            txtCustomerName = new TextBox();
            txtCustomerName.Location = new Point(65, 22);
            txtCustomerName.Size = new Size(180, 22);
            txtCustomerName.Font = new Font("Segoe UI", 9);
            grpCustomer.Controls.Add(txtCustomerName);

            // Phone
            Label lblPhone = new Label();
            lblPhone.Text = "Phone:";
            lblPhone.Font = new Font("Segoe UI", 9);
            lblPhone.Location = new Point(260, 25);
            lblPhone.Size = new Size(45, 22);
            grpCustomer.Controls.Add(lblPhone);

            txtCustomerPhone = new TextBox();
            txtCustomerPhone.Location = new Point(310, 22);
            txtCustomerPhone.Size = new Size(130, 22);
            txtCustomerPhone.Font = new Font("Segoe UI", 9);
            grpCustomer.Controls.Add(txtCustomerPhone);

            // Address
            Label lblAddress = new Label();
            lblAddress.Text = "Address:";
            lblAddress.Font = new Font("Segoe UI", 9);
            lblAddress.Location = new Point(455, 25);
            lblAddress.Size = new Size(50, 22);
            grpCustomer.Controls.Add(lblAddress);

            txtCustomerAddress = new TextBox();
            txtCustomerAddress.Location = new Point(510, 22);
            txtCustomerAddress.Size = new Size(280, 22);
            txtCustomerAddress.Font = new Font("Segoe UI", 9);
            grpCustomer.Controls.Add(txtCustomerAddress);

            // Search Button
            btnSearchCustomer = new Button();
            btnSearchCustomer.Text = "Search";
            btnSearchCustomer.Font = new Font("Segoe UI", 8);
            btnSearchCustomer.Location = new Point(805, 20);
            btnSearchCustomer.Size = new Size(70, 26);
            btnSearchCustomer.BackColor = Color.FromArgb(52, 152, 219);
            btnSearchCustomer.ForeColor = Color.White;
            btnSearchCustomer.FlatStyle = FlatStyle.Flat;
            btnSearchCustomer.Click += (s, e) => MessageBox.Show("Customer search coming soon!");
            grpCustomer.Controls.Add(btnSearchCustomer);
        }

        private void CreateDeliverySection()
        {
            chkDelivery = new CheckBox();
            chkDelivery.Text = "Delivery Order";
            chkDelivery.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            chkDelivery.Location = new Point(12, 80);
            chkDelivery.Size = new Size(110, 22);
            chkDelivery.CheckedChanged += (s, e) => panelDelivery.Visible = chkDelivery.Checked;
            this.Controls.Add(chkDelivery);

            panelDelivery = new Panel();
            panelDelivery.Location = new Point(12, 105);
            panelDelivery.Size = new Size(550, 55);
            panelDelivery.BackColor = Color.WhiteSmoke;
            panelDelivery.BorderStyle = BorderStyle.FixedSingle;
            panelDelivery.Visible = false;
            this.Controls.Add(panelDelivery);

            Label lblDeliveryAddress = new Label();
            lblDeliveryAddress.Text = "Delivery Address:";
            lblDeliveryAddress.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblDeliveryAddress.Location = new Point(6, 6);
            lblDeliveryAddress.Size = new Size(100, 20);
            panelDelivery.Controls.Add(lblDeliveryAddress);

            txtDeliveryAddress = new TextBox();
            txtDeliveryAddress.Location = new Point(6, 28);
            txtDeliveryAddress.Size = new Size(530, 22);
            txtDeliveryAddress.Font = new Font("Segoe UI", 9);
            panelDelivery.Controls.Add(txtDeliveryAddress);
        }

        private void CreateMenuTabs()
        {
            tabMenu = new TabControl();
            tabMenu.Location = new Point(12, 165);
            tabMenu.Size = new Size(780, 420);
            tabMenu.Font = new Font("Segoe UI", 9);
            this.Controls.Add(tabMenu);

            tabBurgers = new TabPage("Burgers");
            tabPizzas = new TabPage("Pizzas");
            tabChicken = new TabPage("Chicken");
            tabRice = new TabPage("Rice");
            tabBeverages = new TabPage("Beverages");
            tabSides = new TabPage("Sides");

            tabMenu.TabPages.Add(tabBurgers);
            tabMenu.TabPages.Add(tabPizzas);
            tabMenu.TabPages.Add(tabChicken);
            tabMenu.TabPages.Add(tabRice);
            tabMenu.TabPages.Add(tabBeverages);
            tabMenu.TabPages.Add(tabSides);
        }

        private void CreateOrderSection()
        {
            grpOrder = new GroupBox();
            grpOrder.Text = "Current Order";
            grpOrder.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpOrder.Location = new Point(810, 165);
            grpOrder.Size = new Size(515, 280);
            grpOrder.BackColor = Color.White;
            this.Controls.Add(grpOrder);

            dgvOrder = new DataGridView();
            dgvOrder.Location = new Point(8, 18);
            dgvOrder.Size = new Size(495, 160);
            dgvOrder.BackgroundColor = Color.White;
            dgvOrder.AllowUserToAddRows = false;
            dgvOrder.AllowUserToDeleteRows = false;
            dgvOrder.ReadOnly = true;
            dgvOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrder.RowHeadersVisible = false;
            dgvOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrder.Font = new Font("Segoe UI", 8);
            grpOrder.Controls.Add(dgvOrder);

            btnRemove = new Button();
            btnRemove.Text = "Remove Selected";
            btnRemove.Font = new Font("Segoe UI", 8);
            btnRemove.Location = new Point(8, 185);
            btnRemove.Size = new Size(110, 28);
            btnRemove.BackColor = Color.FromArgb(231, 76, 60);
            btnRemove.ForeColor = Color.White;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Click += BtnRemove_Click;
            grpOrder.Controls.Add(btnRemove);

            int sumX = 320;
            // Summary labels
            Label lblCountLabel = new Label();
            lblCountLabel.Text = "Items:";
            lblCountLabel.Font = new Font("Segoe UI", 9);
            lblCountLabel.Location = new Point(sumX, 190);
            lblCountLabel.Size = new Size(40, 22);
            grpOrder.Controls.Add(lblCountLabel);

            lblItemCount = new Label();
            lblItemCount.Text = "0";
            lblItemCount.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblItemCount.Location = new Point(sumX + 45, 190);
            lblItemCount.Size = new Size(40, 22);
            grpOrder.Controls.Add(lblItemCount);

            Label lblSubtotalLabel = new Label();
            lblSubtotalLabel.Text = "Subtotal:";
            lblSubtotalLabel.Font = new Font("Segoe UI", 9);
            lblSubtotalLabel.Location = new Point(sumX, 215);
            lblSubtotalLabel.Size = new Size(55, 22);
            grpOrder.Controls.Add(lblSubtotalLabel);

            lblSubtotal = new Label();
            lblSubtotal.Text = "$0.00";
            lblSubtotal.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSubtotal.Location = new Point(sumX + 60, 215);
            lblSubtotal.Size = new Size(70, 22);
            grpOrder.Controls.Add(lblSubtotal);

            Label lblTaxLabel = new Label();
            lblTaxLabel.Text = "Tax (10%):";
            lblTaxLabel.Font = new Font("Segoe UI", 9);
            lblTaxLabel.Location = new Point(sumX, 240);
            lblTaxLabel.Size = new Size(60, 22);
            grpOrder.Controls.Add(lblTaxLabel);

            lblTax = new Label();
            lblTax.Text = "$0.00";
            lblTax.Font = new Font("Segoe UI", 9);
            lblTax.Location = new Point(sumX + 65, 240);
            lblTax.Size = new Size(70, 22);
            grpOrder.Controls.Add(lblTax);

            Label lblTotalLabel = new Label();
            lblTotalLabel.Text = "TOTAL:";
            lblTotalLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTotalLabel.Location = new Point(sumX, 265);
            lblTotalLabel.Size = new Size(55, 25);
            grpOrder.Controls.Add(lblTotalLabel);

            lblTotal = new Label();
            lblTotal.Text = "$0.00";
            lblTotal.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(52, 152, 219);
            lblTotal.Location = new Point(sumX + 60, 265);
            lblTotal.Size = new Size(90, 25);
            grpOrder.Controls.Add(lblTotal);
        }

        private void CreatePaymentSection()
        {
            grpPayment = new GroupBox();
            grpPayment.Text = "Payment";
            grpPayment.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpPayment.Location = new Point(12, 595);
            grpPayment.Size = new Size(1310, 60);
            grpPayment.BackColor = Color.White;
            this.Controls.Add(grpPayment);

            Label lblMethod = new Label();
            lblMethod.Text = "Payment Method:";
            lblMethod.Font = new Font("Segoe UI", 9);
            lblMethod.Location = new Point(15, 20);
            lblMethod.Size = new Size(95, 22);
            grpPayment.Controls.Add(lblMethod);

            cmbPaymentMethod = new ComboBox();
            cmbPaymentMethod.Location = new Point(115, 18);
            cmbPaymentMethod.Size = new Size(90, 22);
            cmbPaymentMethod.Font = new Font("Segoe UI", 9);
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Card", "Online" });
            cmbPaymentMethod.SelectedIndex = 0;
            grpPayment.Controls.Add(cmbPaymentMethod);

            Label lblReceived = new Label();
            lblReceived.Text = "Amount Received:";
            lblReceived.Font = new Font("Segoe UI", 9);
            lblReceived.Location = new Point(220, 20);
            lblReceived.Size = new Size(100, 22);
            grpPayment.Controls.Add(lblReceived);

            txtAmountReceived = new TextBox();
            txtAmountReceived.Location = new Point(325, 18);
            txtAmountReceived.Size = new Size(70, 22);
            txtAmountReceived.Font = new Font("Segoe UI", 9);
            txtAmountReceived.TextChanged += (s, e) => CalculateChange();
            grpPayment.Controls.Add(txtAmountReceived);

            Label lblChangeLabel = new Label();
            lblChangeLabel.Text = "Change:";
            lblChangeLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblChangeLabel.Location = new Point(410, 20);
            lblChangeLabel.Size = new Size(50, 22);
            grpPayment.Controls.Add(lblChangeLabel);

            lblChange = new Label();
            lblChange.Text = "$0.00";
            lblChange.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblChange.ForeColor = Color.FromArgb(46, 204, 113);
            lblChange.Location = new Point(465, 18);
            lblChange.Size = new Size(70, 22);
            grpPayment.Controls.Add(lblChange);

            btnProcessPayment = new Button();
            btnProcessPayment.Text = "Process Payment";
            btnProcessPayment.Font = new Font("Segoe UI", 9);
            btnProcessPayment.Location = new Point(560, 14);
            btnProcessPayment.Size = new Size(120, 30);
            btnProcessPayment.BackColor = Color.FromArgb(46, 204, 113);
            btnProcessPayment.ForeColor = Color.White;
            btnProcessPayment.FlatStyle = FlatStyle.Flat;
            btnProcessPayment.Click += BtnProcessPayment_Click;
            grpPayment.Controls.Add(btnProcessPayment);

            btnCancelOrder = new Button();
            btnCancelOrder.Text = "Cancel Order";
            btnCancelOrder.Font = new Font("Segoe UI", 9);
            btnCancelOrder.Location = new Point(695, 14);
            btnCancelOrder.Size = new Size(100, 30);
            btnCancelOrder.BackColor = Color.FromArgb(149, 165, 166);
            btnCancelOrder.ForeColor = Color.White;
            btnCancelOrder.FlatStyle = FlatStyle.Flat;
            btnCancelOrder.Click += BtnCancelOrder_Click;
            grpPayment.Controls.Add(btnCancelOrder);
        }

        private void LoadMenuItems()
        {
            try
            {
                menuItems = DatabaseManager.Instance.GetMenuItems();
                PopulateTabs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu: {ex.Message}");
            }
        }

        private void PopulateTabs()
        {
            CreateTabContent(tabBurgers, "Burgers");
            CreateTabContent(tabPizzas, "Pizzas");
            CreateTabContent(tabChicken, "Chicken");
            CreateTabContent(tabRice, "Rice");
            CreateTabContent(tabBeverages, "Beverages");
            CreateTabContent(tabSides, "Sides");
        }

        private void CreateTabContent(TabPage tab, string category)
        {
            Panel panel = new Panel();
            panel.AutoScroll = true;
            panel.Size = new Size(760, 400);
            panel.BackColor = Color.White;

            int y = 5;
            List<CheckBox> chkItems = new List<CheckBox>();

            // Add menu items
            foreach (var item in menuItems)
            {
                if (item.CategoryName == category && item.IsAvailable)
                {
                    CheckBox chk = new CheckBox();
                    chk.Text = $"{item.Name} - £{item.Price:F2}";
                    chk.Tag = item.Price;
                    chk.Font = new Font("Segoe UI", 8);
                    chk.Location = new Point(10, y);
                    chk.Size = new Size(320, 25);
                    panel.Controls.Add(chk);
                    chkItems.Add(chk);
                    y += 28;
                }
            }

            // Meal option
            chkMakeMeal = new CheckBox();
            chkMakeMeal.Text = "Make it a Meal (+£2 for fries & drink)";
            chkMakeMeal.Font = new Font("Segoe UI", 8);
            chkMakeMeal.Location = new Point(10, y);
            chkMakeMeal.Size = new Size(280, 25);
            panel.Controls.Add(chkMakeMeal);
            y += 30;

            // Drink choice
            cmbDrinkChoice = new ComboBox();
            cmbDrinkChoice.Location = new Point(20, y);
            cmbDrinkChoice.Size = new Size(110, 22);
            cmbDrinkChoice.Font = new Font("Segoe UI", 8);
            cmbDrinkChoice.Items.AddRange(new object[] { "Coke", "Pepsi", "Fanta", "Sprite", "Water" });
            cmbDrinkChoice.SelectedIndex = 0;
            cmbDrinkChoice.Visible = false;
            panel.Controls.Add(cmbDrinkChoice);

            // Fries choice
            cmbFriesChoice = new ComboBox();
            cmbFriesChoice.Location = new Point(140, y);
            cmbFriesChoice.Size = new Size(90, 22);
            cmbFriesChoice.Font = new Font("Segoe UI", 8);
            cmbFriesChoice.Items.AddRange(new object[] { "Small", "Medium", "Large" });
            cmbFriesChoice.SelectedIndex = 1;
            cmbFriesChoice.Visible = false;
            panel.Controls.Add(cmbFriesChoice);
            y += 35;

            // Show/hide meal options
            chkMakeMeal.CheckedChanged += (s, e) => {
                cmbDrinkChoice.Visible = chkMakeMeal.Checked;
                cmbFriesChoice.Visible = chkMakeMeal.Checked;
            };

            // Quantity
            Label lblQty = new Label();
            lblQty.Text = "Qty:";
            lblQty.Font = new Font("Segoe UI", 8);
            lblQty.Location = new Point(10, y);
            lblQty.Size = new Size(35, 25);
            panel.Controls.Add(lblQty);

            nudQuantity = new NumericUpDown();
            nudQuantity.Location = new Point(50, y);
            nudQuantity.Size = new Size(50, 22);
            nudQuantity.Font = new Font("Segoe UI", 8);
            nudQuantity.Minimum = 1;
            nudQuantity.Maximum = 99;
            nudQuantity.Value = 1;
            panel.Controls.Add(nudQuantity);

            // Add to Order Button
            btnAddToOrder = new Button();
            btnAddToOrder.Text = "Add to Order";
            btnAddToOrder.Font = new Font("Segoe UI", 8);
            btnAddToOrder.Location = new Point(110, y);
            btnAddToOrder.Size = new Size(100, 28);
            btnAddToOrder.BackColor = Color.FromArgb(46, 204, 113);
            btnAddToOrder.ForeColor = Color.White;
            btnAddToOrder.FlatStyle = FlatStyle.Flat;

            var itemsList = chkItems;
            var qtyControl = nudQuantity;
            var mealCheck = chkMakeMeal;
            var drinkCombo = cmbDrinkChoice;
            var friesCombo = cmbFriesChoice;

            btnAddToOrder.Click += (sender, e) => {
                int qty = (int)qtyControl.Value;
                bool isMeal = mealCheck.Checked;
                string drink = isMeal ? drinkCombo.SelectedItem.ToString() : "";
                string fries = isMeal ? friesCombo.SelectedItem.ToString() : "";

                foreach (CheckBox chk in itemsList)
                {
                    if (chk.Checked && chk.Tag is decimal price)
                    {
                        string name = chk.Text.Split('-')[0].Trim();
                        AddToOrder(name, price, qty);
                        if (isMeal)
                        {
                            AddToOrder($"Meal Upgrade ({fries} Fries + {drink})", mealPrice, qty);
                        }
                        chk.Checked = false;
                    }
                }
                mealCheck.Checked = false;
            };
            panel.Controls.Add(btnAddToOrder);

            tab.Controls.Add(panel);
        }

        private void AddToOrder(string name, decimal price, int qty)
        {
            foreach (var item in currentOrder)
            {
                if (item.ItemName == name)
                {
                    item.Quantity += qty;
                    UpdateOrderDisplay();
                    return;
                }
            }
            currentOrder.Add(new OrderItem { ItemName = name, Quantity = qty, UnitPrice = price });
            UpdateOrderDisplay();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                string name = dgvOrder.SelectedRows[0].Cells["Item"].Value.ToString();
                currentOrder.RemoveAll(x => x.ItemName == name);
                UpdateOrderDisplay();
            }
        }

        private void UpdateOrderDisplay()
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Qty", typeof(int));
            dt.Columns.Add("Price", typeof(string));
            dt.Columns.Add("Total", typeof(string));

            decimal sub = 0;
            int count = 0;
            foreach (var item in currentOrder)
            {
                decimal total = item.Quantity * item.UnitPrice;
                sub += total;
                count += item.Quantity;
                dt.Rows.Add(item.ItemName, item.Quantity, item.UnitPrice.ToString("C"), total.ToString("C"));
            }

            dgvOrder.DataSource = dt;
            lblItemCount.Text = count.ToString();
            lblSubtotal.Text = sub.ToString("C");
            lblTax.Text = (sub * taxRate).ToString("C");
            lblTotal.Text = (sub + (sub * taxRate)).ToString("C");
        }

        private void CalculateChange()
        {
            if (decimal.TryParse(txtAmountReceived.Text, out decimal received))
            {
                string totalStr = lblTotal.Text.Replace("$", "").Replace("£", "").Replace(",", "");
                if (decimal.TryParse(totalStr, out decimal total))
                {
                    if (received >= total)
                    {
                        lblChange.Text = (received - total).ToString("C");
                        lblChange.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblChange.Text = "Insufficient";
                        lblChange.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void BtnProcessPayment_Click(object sender, EventArgs e)
        {
            if (currentOrder.Count == 0)
            {
                MessageBox.Show("Order is empty!");
                return;
            }

            string name = txtCustomerName.Text.Trim();
            if (string.IsNullOrEmpty(name)) name = "Walk-in Customer";

            if (chkDelivery.Checked && string.IsNullOrEmpty(txtDeliveryAddress.Text))
            {
                MessageBox.Show("Please enter delivery address.");
                return;
            }

            string totalStr = lblTotal.Text.Replace("$", "").Replace("£", "").Replace(",", "");
            if (!decimal.TryParse(totalStr, out decimal total)) return;

            string method = cmbPaymentMethod.SelectedItem.ToString();
            if (method == "Cash")
            {
                if (!decimal.TryParse(txtAmountReceived.Text, out decimal rec) || rec < total)
                {
                    MessageBox.Show($"Insufficient amount. Total: {total:C}");
                    return;
                }
            }

            try
            {
                string orderNum = $"ORD-{DateTime.Now:yyyyMMdd-HHmmss}";
                Order order = new Order
                {
                    OrderNumber = orderNum,
                    CustomerName = name,
                    CustomerPhone = txtCustomerPhone.Text,
                    DeliveryAddress = chkDelivery.Checked ? txtDeliveryAddress.Text : "",
                    TotalAmount = total,
                    Status = chkDelivery.Checked ? "Pending" : "Ready",
                    PaymentMethod = method,
                    PaymentStatus = "Paid",
                    CreatedBy = CurrentUser.UserID
                };

                List<OrderItem> items = new List<OrderItem>();
                foreach (var item in currentOrder)
                {
                    items.Add(new OrderItem
                    {
                        ItemName = item.ItemName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }

                int id = DatabaseManager.Instance.CreateOrder(order, items);
                if (id > 0)
                {
                    string msg = chkDelivery.Checked ?
                        $"Delivery Order Created!\nOrder: {orderNum}\nTotal: {total:C}\nAddress: {txtDeliveryAddress.Text}" :
                        $"Payment Successful!\nOrder: {orderNum}\nTotal: {total:C}\nChange: {lblChange.Text}";
                    MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnCancelOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel current order?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}