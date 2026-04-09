using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using Takeaway_Restaurant_Management_System.Classes.Utilities;

namespace Takeaway_Restaurant_Management_System.Forms
{
    public class frmAIChatbot : Form
    {
        // Chat UI Controls
        private RichTextBox rtbChatHistory;
        private TextBox txtUserInput;
        private Button btnSend;
        private Button btnClear;
        private Button btnClose;
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblStatus;
        private Timer animationTimer;
        private int dotCount = 0;

        // AI Knowledge Base
        private Dictionary<string, string> responses;
        private List<MenuItemModel> menuItems;
        private List<Order> recentOrders;

        // Current order being built by AI
        private List<OrderItem> aiOrder;
        private bool isBuildingOrder = false;

        public frmAIChatbot()
        {
            InitializeComponent();
            LoadKnowledgeBase();
            LoadMenuData();
            AddWelcomeMessage();

            // Start typing animation
            animationTimer = new Timer();
            animationTimer.Interval = 500;
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void InitializeComponent()
        {
            this.Text = "🤖 AI Chatbot Assistant - Appeliano Restaurant";
            this.Size = new Size(550, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Header Panel
            panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 70;
            panelHeader.BackColor = Color.FromArgb(52, 152, 219);
            this.Controls.Add(panelHeader);

            // Title with Robot Icon
            lblTitle = new Label();
            lblTitle.Text = "🤖 AI ORDER ASSISTANT";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(15, 15);
            lblTitle.Size = new Size(300, 35);
            panelHeader.Controls.Add(lblTitle);

            lblStatus = new Label();
            lblStatus.Text = "Online • Ready to help you take orders";
            lblStatus.Font = new Font("Segoe UI", 9);
            lblStatus.ForeColor = Color.FromArgb(220, 220, 220);
            lblStatus.Location = new Point(15, 48);
            lblStatus.Size = new Size(300, 20);
            panelHeader.Controls.Add(lblStatus);

            // Chat History
            rtbChatHistory = new RichTextBox();
            rtbChatHistory.Location = new Point(12, 85);
            rtbChatHistory.Size = new Size(520, 480);
            rtbChatHistory.BackColor = Color.White;
            rtbChatHistory.ReadOnly = true;
            rtbChatHistory.Font = new Font("Segoe UI", 10);
            rtbChatHistory.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(rtbChatHistory);

            // User Input Panel
            Panel panelInput = new Panel();
            panelInput.Location = new Point(12, 575);
            panelInput.Size = new Size(520, 80);
            panelInput.BackColor = Color.White;
            panelInput.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelInput);

            // User Input TextBox (with manual placeholder)
            txtUserInput = new TextBox();
            txtUserInput.Location = new Point(10, 10);
            txtUserInput.Size = new Size(400, 30);
            txtUserInput.Font = new Font("Segoe UI", 11);
            txtUserInput.Text = "Type your message here...";
            txtUserInput.ForeColor = Color.Gray;
            txtUserInput.Enter += TxtUserInput_Enter;
            txtUserInput.Leave += TxtUserInput_Leave;
            txtUserInput.KeyPress += TxtUserInput_KeyPress;
            panelInput.Controls.Add(txtUserInput);

            // Send Button
            btnSend = new Button();
            btnSend.Text = "📤 Send";
            btnSend.Location = new Point(420, 8);
            btnSend.Size = new Size(85, 35);
            btnSend.BackColor = Color.FromArgb(46, 204, 113);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSend.Click += BtnSend_Click;
            panelInput.Controls.Add(btnSend);

            // Quick Actions Panel
            Panel panelQuickActions = new Panel();
            panelQuickActions.Location = new Point(12, 575);
            panelQuickActions.Size = new Size(520, 35);
            panelQuickActions.BackColor = Color.White;
            this.Controls.Add(panelQuickActions);

            // Quick Action Buttons
            Button btnPopular = new Button();
            btnPopular.Text = "🔥 Popular Items";
            btnPopular.Size = new Size(110, 30);
            btnPopular.Location = new Point(5, 3);
            btnPopular.BackColor = Color.FromArgb(52, 152, 219);
            btnPopular.ForeColor = Color.White;
            btnPopular.FlatStyle = FlatStyle.Flat;
            btnPopular.Click += (s, e) => txtUserInput.Text = "What are the popular items?";
            panelQuickActions.Controls.Add(btnPopular);

            Button btnSpecials = new Button();
            btnSpecials.Text = "⭐ Today's Specials";
            btnSpecials.Size = new Size(120, 30);
            btnSpecials.Location = new Point(120, 3);
            btnSpecials.BackColor = Color.FromArgb(52, 152, 219);
            btnSpecials.ForeColor = Color.White;
            btnSpecials.FlatStyle = FlatStyle.Flat;
            btnSpecials.Click += (s, e) => txtUserInput.Text = "What are today's specials?";
            panelQuickActions.Controls.Add(btnSpecials);

            Button btnHelp = new Button();
            btnHelp.Text = "❓ Help";
            btnHelp.Size = new Size(80, 30);
            btnHelp.Location = new Point(245, 3);
            btnHelp.BackColor = Color.FromArgb(149, 165, 166);
            btnHelp.ForeColor = Color.White;
            btnHelp.FlatStyle = FlatStyle.Flat;
            btnHelp.Click += (s, e) => txtUserInput.Text = "help";
            panelQuickActions.Controls.Add(btnHelp);

            Button btnOrder = new Button();
            btnOrder.Text = "🛒 Start Order";
            btnOrder.Size = new Size(100, 30);
            btnOrder.Location = new Point(330, 3);
            btnOrder.BackColor = Color.FromArgb(46, 204, 113);
            btnOrder.ForeColor = Color.White;
            btnOrder.FlatStyle = FlatStyle.Flat;
            btnOrder.Click += BtnStartOrder_Click;
            panelQuickActions.Controls.Add(btnOrder);

            // Bottom Buttons
            btnClear = new Button();
            btnClear.Text = "🗑️ Clear Chat";
            btnClear.Location = new Point(12, 620);
            btnClear.Size = new Size(100, 30);
            btnClear.BackColor = Color.FromArgb(149, 165, 166);
            btnClear.ForeColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Click += BtnClear_Click;
            this.Controls.Add(btnClear);

            btnClose = new Button();
            btnClose.Text = "❌ Close";
            btnClose.Location = new Point(432, 620);
            btnClose.Size = new Size(100, 30);
            btnClose.BackColor = Color.FromArgb(231, 76, 60);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void TxtUserInput_Enter(object sender, EventArgs e)
        {
            if (txtUserInput.Text == "Type your message here...")
            {
                txtUserInput.Text = "";
                txtUserInput.ForeColor = Color.Black;
            }
        }

        private void TxtUserInput_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserInput.Text))
            {
                txtUserInput.Text = "Type your message here...";
                txtUserInput.ForeColor = Color.Gray;
            }
        }

        private void LoadKnowledgeBase()
        {
            responses = new Dictionary<string, string>();

            // General responses
            responses.Add("hello", "Hello! 👋 I'm your AI Order Assistant. How can I help you today?");
            responses.Add("hi", "Hi there! 👋 Ready to help you take orders!");
            responses.Add("help", "I can help you with:\n\n📋 **View Menu** - See all available items\n🔥 **Popular Items** - Best selling items\n⭐ **Today's Specials** - Daily specials\n🛒 **Take Order** - Start building an order\n💰 **Check Price** - Get item price\n📊 **Order Status** - Check order status\n\nJust type what you need!");
            responses.Add("menu", "Let me show you our menu...");
            responses.Add("thanks", "You're welcome! 😊 Anything else I can help with?");
            responses.Add("thank you", "You're welcome! 😊");
            responses.Add("bye", "Goodbye! 👋 Come back anytime!");

            // Order related
            responses.Add("order status", "Let me check your order status...");
            responses.Add("price", "Let me check the price for you.");
        }

        private void LoadMenuData()
        {
            try
            {
                menuItems = DatabaseManager.Instance.GetMenuItems();
            }
            catch (Exception ex)
            {
                AddBotMessage($"Error loading menu: {ex.Message}");
            }
        }

        private void AddWelcomeMessage()
        {
            string welcome = "🤖 **AI Order Assistant**\n\n";
            welcome += "Welcome! I'm your intelligent ordering assistant. I can help you:\n\n";
            welcome += "• 🍔 **Browse the menu** - Type 'menu' or 'show menu'\n";
            welcome += "• 🔥 **Find popular items** - Type 'popular items'\n";
            welcome += "• ⭐ **See today's specials** - Type 'specials'\n";
            welcome += "• 🛒 **Place an order** - Type 'start order'\n";
            welcome += "• 💰 **Check prices** - Type 'price of [item]'\n\n";
            welcome += "**Try it now!** What would you like to order today? 🍕";

            AppendToChat("🤖 AI Assistant", welcome, Color.FromArgb(52, 152, 219));
        }

        private void AddBotMessage(string message)
        {
            AppendToChat("🤖 AI Assistant", message, Color.FromArgb(52, 152, 219));
        }

        private void AddUserMessage(string message)
        {
            AppendToChat($"👤 {CurrentUser.FullName}", message, Color.FromArgb(46, 204, 113));
        }

        private void AppendToChat(string sender, string message, Color color)
        {
            if (rtbChatHistory.InvokeRequired)
            {
                rtbChatHistory.Invoke(new Action(() => AppendToChat(sender, message, color)));
                return;
            }

            rtbChatHistory.SelectionStart = rtbChatHistory.TextLength;
            rtbChatHistory.SelectionLength = 0;

            rtbChatHistory.SelectionColor = color;
            rtbChatHistory.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            rtbChatHistory.AppendText($"{sender}: ");

            rtbChatHistory.SelectionColor = Color.Black;
            rtbChatHistory.SelectionFont = new Font("Segoe UI", 10, FontStyle.Regular);
            rtbChatHistory.AppendText($"{message}\n\n");

            rtbChatHistory.ScrollToCaret();
        }

        private void TxtUserInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSend_Click(null, null);
                e.Handled = true;
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            string userInput = txtUserInput.Text.Trim();
            if (string.IsNullOrEmpty(userInput) || userInput == "Type your message here...")
                return;

            AddUserMessage(userInput);
            ProcessUserInput(userInput.ToLower());
            txtUserInput.Text = "Type your message here...";
            txtUserInput.ForeColor = Color.Gray;
            txtUserInput.Focus();
        }

        private void ProcessUserInput(string input)
        {
            // Start typing animation
            animationTimer.Start();

            // Check for menu items
            if (input.Contains("price of") || input.Contains("cost of"))
            {
                string itemName = input.Replace("price of", "").Replace("cost of", "").Trim();
                CheckPrice(itemName);
            }
            else if (input.Contains("menu") || input.Contains("show me the menu"))
            {
                ShowMenu();
            }
            else if (input.Contains("popular items") || input.Contains("best selling"))
            {
                ShowPopularItems();
            }
            else if (input.Contains("specials") || input.Contains("today's special"))
            {
                ShowTodaySpecials();
            }
            else if (input.Contains("start order") || input.Contains("new order"))
            {
                StartNewOrder();
            }
            else if (input.Contains("add") && input.Contains("to order"))
            {
                AddToOrder(input);
            }
            else if (input.Contains("show order") || input.Contains("view order"))
            {
                ShowCurrentOrder();
            }
            else if (input.Contains("complete order") || input.Contains("finish order"))
            {
                CompleteOrder();
            }
            else if (input.Contains("cancel order"))
            {
                CancelOrder();
            }
            else if (responses.ContainsKey(input))
            {
                AddBotMessage(responses[input]);
            }
            else
            {
                // Search for menu item
                var item = menuItems.FirstOrDefault(i => i.Name.ToLower().Contains(input));
                if (item != null)
                {
                    AddBotMessage($"🍔 **{item.Name}**\n💰 Price: £{item.Price:F2}\n⏱️ Prep time: {item.PreparationTime} minutes\n📝 {item.Description}\n\nWould you like to add this to your order? Type 'add {item.Name} to order'");
                }
                else
                {
                    AddBotMessage("I'm not sure about that. Try:\n• 'menu' - to see our menu\n• 'popular items' - best sellers\n• 'help' - for assistance\n• Or type an item name to check price");
                }
            }

            animationTimer.Stop();
        }

        private void ShowMenu()
        {
            if (menuItems == null || menuItems.Count == 0)
            {
                AddBotMessage("Sorry, I couldn't load the menu. Please try again later.");
                return;
            }

            var categories = menuItems.GroupBy(i => i.CategoryName);
            string menuText = "📋 **OUR MENU**\n\n";

            foreach (var category in categories)
            {
                menuText += $"**{category.Key}**\n";
                foreach (var item in category.Take(5))
                {
                    menuText += $"• {item.Name} - £{item.Price:F2}\n";
                }
                if (category.Count() > 5)
                {
                    menuText += $"  *and {category.Count() - 5} more items*\n";
                }
                menuText += "\n";
            }

            menuText += "Type an item name to see details or 'start order' to begin ordering!";
            AddBotMessage(menuText);
        }

        private void ShowPopularItems()
        {
            try
            {
                var popular = DatabaseManager.Instance.GetPopularItemsReport(
                    DateTime.Today.AddDays(-7), DateTime.Today);

                string popularText = "🔥 **POPULAR ITEMS THIS WEEK**\n\n";

                if (popular.Rows.Count > 0)
                {
                    for (int i = 0; i < Math.Min(5, popular.Rows.Count); i++)
                    {
                        popularText += $"{i + 1}. {popular.Rows[i]["ItemName"]} - " +
                                      $"Ordered {popular.Rows[i]["TotalQuantity"]} times\n";
                    }
                }
                else
                {
                    popularText = "Not enough data yet. Check back soon!";
                }

                AddBotMessage(popularText);
            }
            catch (Exception ex)
            {
                AddBotMessage("Sorry, couldn't load popular items right now.");
            }
        }

        private void ShowTodaySpecials()
        {
            string specials = "⭐ **TODAY'S SPECIALS** ⭐\n\n";

            if (menuItems != null && menuItems.Count > 0)
            {
                var random = new Random();
                var specialsList = menuItems.OrderBy(x => random.Next()).Take(3);

                foreach (var item in specialsList)
                {
                    specials += $"🍽️ **{item.Name}**\n";
                    specials += $"   💷 Price: £{item.Price:F2}\n";
                    specials += $"   ⏱️ Ready in {item.PreparationTime} min\n\n";
                }
            }

            specials += "Type 'start order' to order any of these specials!";
            AddBotMessage(specials);
        }

        private void CheckPrice(string itemName)
        {
            var item = menuItems.FirstOrDefault(i => i.Name.ToLower().Contains(itemName));
            if (item != null)
            {
                AddBotMessage($"🍔 **{item.Name}** costs **£{item.Price:F2}**\n⏱️ Preparation time: {item.PreparationTime} minutes\n\nWould you like to add it to your order?");
            }
            else
            {
                AddBotMessage($"Sorry, I couldn't find '{itemName}' in our menu. Try typing 'menu' to see all items.");
            }
        }

        private void StartNewOrder()
        {
            aiOrder = new List<OrderItem>();
            isBuildingOrder = true;
            AddBotMessage("🛒 **New Order Started!**\n\nI'll help you build your order.\n\n" +
                         "To add items, type:\n• 'add [item name]'\n• Example: 'add Burger'\n\n" +
                         "To see your order, type: 'show order'\n" +
                         "To finish, type: 'complete order'\n" +
                         "To cancel, type: 'cancel order'");
        }

        private void AddToOrder(string input)
        {
            if (!isBuildingOrder)
            {
                AddBotMessage("You don't have an active order. Type 'start order' to begin!");
                return;
            }

            string itemName = input.Replace("add", "").Replace("to order", "").Trim();
            var item = menuItems.FirstOrDefault(i => i.Name.ToLower().Contains(itemName));

            if (item != null)
            {
                var existing = aiOrder.FirstOrDefault(o => o.ItemName == item.Name);
                if (existing != null)
                {
                    existing.Quantity++;
                    AddBotMessage($"✅ Added another {item.Name} to your order! (Total: {existing.Quantity})");
                }
                else
                {
                    aiOrder.Add(new OrderItem { ItemName = item.Name, Quantity = 1, UnitPrice = item.Price });
                    AddBotMessage($"✅ Added **{item.Name}** (£{item.Price:F2}) to your order!");
                }

                ShowCurrentOrder();
            }
            else
            {
                AddBotMessage($"Sorry, I couldn't find '{itemName}'. Type 'menu' to see available items.");
            }
        }

        private void ShowCurrentOrder()
        {
            if (aiOrder == null || aiOrder.Count == 0)
            {
                AddBotMessage("Your order is empty. Add items by typing 'add [item name]'");
                return;
            }

            string orderText = "🛒 **YOUR CURRENT ORDER**\n\n";
            decimal total = 0;

            foreach (var item in aiOrder)
            {
                decimal itemTotal = item.Quantity * item.UnitPrice;
                total += itemTotal;
                orderText += $"• {item.Quantity}x {item.ItemName} - £{itemTotal:F2}\n";
            }

            orderText += $"\n**Total: £{total:F2}**\n\n";
            orderText += "Type 'complete order' to finish or 'add [item]' to add more.";

            AddBotMessage(orderText);
        }

        private void CompleteOrder()
        {
            if (aiOrder == null || aiOrder.Count == 0)
            {
                AddBotMessage("Your order is empty. Add items first by typing 'add [item name]'");
                return;
            }

            decimal total = aiOrder.Sum(i => i.Quantity * i.UnitPrice);

            string summary = "✅ **ORDER COMPLETE!**\n\n";
            summary += $"📋 {aiOrder.Count} item(s) in your order\n";
            summary += $"💰 Total: £{total:F2}\n\n";
            summary += "Please go to the **Take Order** form to complete payment and confirm your order.\n\n";
            summary += "Thank you for using AI Order Assistant! 🎉";

            AddBotMessage(summary);

            // Reset order
            aiOrder = null;
            isBuildingOrder = false;
        }

        private void CancelOrder()
        {
            aiOrder = null;
            isBuildingOrder = false;
            AddBotMessage("❌ Your order has been cancelled.\n\nType 'start order' to begin a new order.");
        }

        private void BtnStartOrder_Click(object sender, EventArgs e)
        {
            StartNewOrder();
        }

      