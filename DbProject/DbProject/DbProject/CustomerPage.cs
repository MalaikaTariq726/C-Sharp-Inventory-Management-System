using DbProject.Resources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;
using System.Linq;


namespace DbProject
{

    public partial class CustomerPage : Form
    {
        String connectionString = @"DATA SOURCE =localhost:1521/XE;USER ID =dbfinal; PASSWORD=dbfinal";
        string email;
        DateTime logInTimestamp;
        string activity = "Viewed Site";
        string category = "";
        string orderid = "";
        List<cart> cartProd = new List<cart>();
        public CustomerPage(string email, DateTime currentTimestamp)
        {
            InitializeComponent();

            getCategoryData();
            this.email = email;
            this.logInTimestamp = currentTimestamp;
              }
        public int getTypeData(string category)
        {
            int i = 0;
            try
            {
               

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT Type_Name FROM Type WHERE Type_Id IN (SELECT Typeid FROM CategoryType WHERE CategoryId = (SELECT category_id FROM Category WHERE Category_Name = :category))";

                    using (OracleCommand command = new OracleCommand(selectQuery, connection))
                    {
                        command.Parameters.Add("category", OracleDbType.Varchar2).Value = category;

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                           
                            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

                            while (reader.Read())
                            {
                               
                                string typeName = reader["Type_Name"].ToString();
                                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(typeName);
                                toolStripMenuItem.Click += ToolStripMenuItem_Click;
                                contextMenuStrip.Items.Add(toolStripMenuItem);
                                i++;
                            }

                           
                            contextMenuStrip.Show(Cursor.Position);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

            }
            return i;   
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cartPanel.Visible = false;  
            trackPanel.Visible = false;


           
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;

            if (clickedItem != null)
            {
               
                string typeName = clickedItem.Text;
               int id= getCategoryTypeId(typeName, category);
                if (id == -1)
                {
                    Label nodtfound = new Label();
                    nodtfound.Text = "No data found";
                    nodtfound.AutoSize = true;

                    nodtfound.BackColor = Color.LightGray;
                    nodtfound.ForeColor = Color.Black;
                    nodtfound.Location = new System.Drawing.Point(25, 456);
                    productPanel.Controls.Add(nodtfound);
                }
                else
                {
                    addPanel(id);
                }
            }
        }

        public int getCategoryTypeId(string type, string category)
        {
            int typeId = getTypeId(type);
            int categoryId = getCategoryId(category);
            int categoryTypeId = -1;

          

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT ID FROM CATEGORYTYPE WHERE CATEGORYID = :categoryId AND typeid = :typeId";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("categoryId", OracleDbType.Int32).Value = categoryId;
                    command.Parameters.Add("typeId", OracleDbType.Int32).Value = typeId;

                    try
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryTypeId = reader.GetInt32(0);
                            }
                        }
                    }
                    catch (Exception ex)
                    { 
                        Console.WriteLine("Error executing SQL query: " + ex.Message);
                    }
                }
            }

            return categoryTypeId;
        }


        public int getCategoryId(String categoryName)
        {

            int c = -1;
           

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Category_ID FROM Category WHERE Category_Name = :categoryName";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("categoryName", OracleDbType.Varchar2).Value = categoryName;

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            c = reader.GetInt32(0);
                        }
                    }
                }
            }

            return c;

        }

        public int getTypeId(String type)
        {

            int c = -1;
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Type_ID FROM Type WHERE Type_Name = :type";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("type", OracleDbType.Varchar2).Value = type;

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            c = reader.GetInt32(0);
                        }
                    }
                }
            }

            return c;

        }

        private void getCategoryData()
        {
          

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                int i = 1;

                string selectQuery = "SELECT Category_Name FROM Category";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string categoryName = reader["Category_Name"].ToString();

                            System.Windows.Forms.Button categoryButton = new System.Windows.Forms. Button();
                            categoryButton.Text = categoryName;
                            categoryButton.Size = new Size(100, 30);
                            categoryButton.BackColor = Color.Black; categoryButton.ForeColor = Color.White;
                            categoryButton.FlatStyle = FlatStyle.Flat;
                            categoryButton.FlatAppearance.BorderColor = Color.Black;    
                            categoryButton.Click += CategoryButton_Click;
                            categoryButton.Location=new System.Drawing.Point(categoryButton.Size.Width * i, 8);
                            btnpanel4.Controls.Add(categoryButton);
                            i++;
                        }

                    }
                }
            }
        }
        private void tracking()
        {
            tableLayoutPanel1.Controls.Clear();
            int custID = getCustomerId(email);
           

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = @"
                SELECT O.OrderID, O.Status, SUM(P.Price * OL.Quantity) AS TotalBill
                FROM Orders O
                JOIN Customer C ON O.CustomerID = C.ID
                JOIN OrderLineItems OL ON O.OrderID = OL.OrderID
                JOIN Product P ON OL.ProductID = P.Product_ID
                WHERE C.ID = :custID AND (O.Status = 'on the way' OR O.Status = 'processing')
                GROUP BY O.OrderID, O.Status, O.OrderDate
                ORDER BY O.OrderID
            ";
                    using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("custID", OracleDbType.Int32).Value = custID;

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string orderId = reader["OrderID"].ToString();
                                string status = reader["Status"].ToString();
                                string totalBill = reader["TotalBill"].ToString();

                                PictureBox pictureBox2 = new PictureBox();
                              
                                pictureBox2.Location = new System.Drawing.Point(3, 3);
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                                if (status == "on the way")
                                {
                                    pictureBox2.Width = 450;
                                    pictureBox2.Height = 200;
                                    pictureBox2.Image = Resource1.Processing_removebg_preview;
                                    //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\Processing-removebg-preview.png");
                                }
                                else if (status == "processing")
                                {
                                    pictureBox2.Width = 450;
                                    pictureBox2.Height = 100;
                                    pictureBox2.Image = Resource1.Processing__3__removebg_preview;
                                    // System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\Processing__3_-removebg-preview.png");
                                     RoundBtn roundBtn = new RoundBtn();
                                    roundBtn.BackColor = System.Drawing.Color.Black;
                                    roundBtn.ForeColor = System.Drawing.Color.White;
                                    roundBtn.Text = "Cancel Order ";
                                    roundBtn.Tag = orderId;
                                    roundBtn.Click += CancelOrderButtonClick;
                                    roundBtn.Location = new System.Drawing.Point(900 + 10, 3);
                                    tableLayoutPanel1.Controls.Add(roundBtn);
                                }

                                tableLayoutPanel1.Controls.Add(pictureBox2);

                                Label order = new Label();
                                order.Text = "Order Id: " + orderId;
                                order.Width = 150;
                                order.Height = 20;
                                order.Location = new System.Drawing.Point(3, pictureBox2.Bottom + 5);
                                order.Font = new System.Drawing.Font("Arial", 12, FontStyle.Regular);
                                order.ForeColor = System.Drawing.Color.Black;
                                order.BackColor = System.Drawing.Color.Transparent;

                                tableLayoutPanel1.Controls.Add(order);

                                
                                    Label bill = new Label();
                                string s = $"Total Bill: $ {totalBill}";
                                bill.Text = s;
                                    bill.Width = 200;
                                    bill.Height = 20;
                                    bill.Location = new System.Drawing.Point(50, order.Bottom + 5);
                                    bill.Font = new System.Drawing.Font("Arial", 12, FontStyle.Regular);
                                    bill.ForeColor = System.Drawing.Color.Black;
                                    bill.BackColor = System.Drawing.Color.Transparent;

                                    tableLayoutPanel1.Controls.Add(bill);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        private void CancelOrderButtonClick(object sender, EventArgs e)
        {
            int parsedValue = 0;
            
            if (sender is RoundBtn roundBtn)
            {
                string orderId = roundBtn.Tag?.ToString();
                try
                {
                    parsedValue = int.Parse(orderId);
                    Console.WriteLine($"Parsed value: {parsedValue}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format for parsing.");
                }
                if (!string.IsNullOrEmpty(orderId))
                {
                    updateStatus(parsedValue, "cancelled");
                    MessageBox.Show($"Cancel Order button clicked for OrderId: {orderId}");
                }
            }
            tracking();
            
        }

        private void trackOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel4.AutoScroll = false;
            cartPanel.Visible = true;
            panel4.Visible = true;
            trackPanel.Visible = true;
            orderPanel.Visible = true;
            trackorderPanel.Visible = true;
            tracking();
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();

            //DateTime currentTimestamp = DateTime.Now;
            InsertInLogs();
            About_us_page about_Us = new About_us_page(connectionString);
            about_Us.Show();
        }
        private void viewProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel4.AutoScroll = true;
            cartPanel.Visible= false;
            trackPanel.Visible = false;
            getCategoryData();
            productPanel.Controls.Clear();
            panel4.Visible = true;
            
          

        }
        private void orderHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            panel4.AutoScroll = false;
            cartPanel.Visible = true;
            trackPanel.Visible = true;
            trackorderPanel.Visible = false;
            panel4.Visible = true;
            GetOrderHistory();
            orderPanel.Visible = true;
            
        }


        public bool InsertInLogs()
        {
            DateTime currentTimestamp = DateTime.Now;
            int custID = getCustomerId(email);
            try
            {
               

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                   
                    string insertQuery = "INSERT INTO USERLOGS (Logintimestamp, logouttimestamp, customerid, activity) VALUES " +
                                         "(:loginTimestamp, :logoutTimestamp, :customerId, :activity)";

                    using (OracleCommand command = new OracleCommand(insertQuery, connection))
                    { 
                        command.Parameters.Add("loginTimestamp", OracleDbType.TimeStamp).Value = logInTimestamp;
                        command.Parameters.Add("logoutTimestamp", OracleDbType.TimeStamp).Value = currentTimestamp;
                        command.Parameters.Add("customerId", OracleDbType.Int32).Value = custID;
                        command.Parameters.Add("activity", OracleDbType.Varchar2).Value = activity;

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"OracleException: Code={ex.Number}, Message={ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }


        private void CategoryButton_Click(object sender, EventArgs e)
            {
            
            System.Windows.Forms.Button clickedButton = (System.Windows.Forms.Button)sender;
            category= clickedButton.Text;
         int count=   getTypeData(category);
            if(count==0)
            {
                productPanel.Controls.Clear();
                Label nodtfound = new Label();
                nodtfound.Text = "No data found";
                nodtfound.AutoSize = true;

                nodtfound.BackColor = Color.LightGray;
                nodtfound.ForeColor = Color.Black;
                nodtfound.Location = new System.Drawing.Point(25, 456);
                productPanel.Controls.Add(nodtfound);

            }
          
            }

        public int getCustomerId(string email)
        {
            int c = -1;
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT ID FROM  CUSTOMER WHERE EMAIL = :email";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("email", OracleDbType.Varchar2).Value = email;

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            c = reader.GetInt32(0);
                        }
                    }
                }
            }

            return c;

        }
        void addPanel(int ct)
           {
            cartPanel.Visible=false; 
            trackPanel.Visible=false;
            productPanel.Visible = true;
            productPanel.Controls.Clear();
            int i = 0;


            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT  NAME, image,price FROM product WHERE CATEGORYTYPEID =:ct";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add("ct", OracleDbType.Int32).Value = ct;
                        using (OracleDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                byte[] imageData = (byte[])reader["image"];
                                System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageData));

                                RoundPanel panel = new RoundPanel();
                                panel.Width = 300;
                                panel.Height = 350;
                                panel.BorderStyle = BorderStyle.FixedSingle;
                                //panel.BackColor = Color.DarkGray; panel.ForeColor=Color.DarkGray;
                                panel.GradientBottomColor = Color.LightGray;
                                panel.GradientTopColor = Color.LightGray;
                                panel.BorderStyle = BorderStyle.None;
                                panel.BackColor = Color.White;

                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Width = panel.Width;
                                pictureBox.Height = panel.Height;
                                pictureBox.Image = image;
                                
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox.Width = 250; pictureBox.Height = 250;
                                pictureBox.Location = new System.Drawing.Point(25, 25);
                                

                                panel.Controls.Add(pictureBox);
                                int panelSpacing = 20;
                                panel.Location = new System.Drawing.Point(i * (panel.Width + panelSpacing), 100);
                                //Labels 
                             
                                System.Windows.Forms.Label nameLabel = new System.Windows.Forms.Label();
                                nameLabel.Text = reader["NAME"].ToString();
                                nameLabel.AutoSize = true;
                                nameLabel.Location = new System.Drawing.Point(25, pictureBox.Bottom + 20);
                                nameLabel.Font = new System.Drawing.Font("Sans Serif Collection", 5, FontStyle.Regular);
                                nameLabel.BackColor = Color.LightGray;
                                nameLabel.ForeColor = Color.Black;
   
                                panel.Controls.Add(nameLabel);

                                
                                Label lbl=new Label();
                                lbl.Text = "$";
                                lbl.AutoSize = true;

                                lbl.BackColor = Color.LightGray;
                                lbl.ForeColor = Color.Black;
                                lbl.Location = new System.Drawing.Point(27, nameLabel.Bottom + 2);
                                panel.Controls.Add( lbl);
                                Label priceLabel = new Label(); 
                                priceLabel.Text =  reader["price"].ToString();
                                priceLabel.AutoSize = true;
              
                                priceLabel.BackColor = Color.LightGray;
                                priceLabel.ForeColor = Color.Black;
                                priceLabel.Location= new System.Drawing.Point(lbl.Right+2, nameLabel.Bottom + 2);
                                panel.Controls.Add(priceLabel);
                                PictureBox pictureBox2 = new PictureBox();

                                pictureBox2.Image = Resource1.icons8_cart_64;
                                //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-cart-64.png");
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox2.BackColor = Color.Transparent;
                                pictureBox2.Width = 30; pictureBox2.Height = 30;
                                pictureBox2.Location = new System.Drawing.Point(240, pictureBox.Bottom + 20);
                                pictureBox2.Click += PictureBox2_Click;

                                pictureBox2.Tag = new Tuple<Label, Label>(nameLabel, priceLabel);

                                panel.Controls.Add(pictureBox2);

                                productPanel.Controls.Add(panel);

                                i++;
                            }
                        }

                    }
                }
            }
            catch (Exception ex) 
            {
                
            }
           

            if (i == 0)
            {

                productPanel.Controls.Clear();
                Label nodtfound = new Label();
                nodtfound.Text = "No data found";
                nodtfound.AutoSize = true;

                nodtfound.BackColor = Color.LightGray;
                nodtfound.ForeColor = Color.Black;
                nodtfound.Location = new System.Drawing.Point(25,456);
                productPanel.Controls.Add(nodtfound);
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
           
            PictureBox clickedPictureBox = sender as PictureBox;

          
            if (clickedPictureBox != null && clickedPictureBox.Tag != null)
            {
                Tuple<Label, Label> associatedLabels = clickedPictureBox.Tag as Tuple<Label, Label>;

               
                if (associatedLabels != null)
                {
                    
                    if (FindProductIndex(associatedLabels.Item1.Text) == -1)
                    {
                        if (double.TryParse(associatedLabels.Item2.Text, out double price))
                        {
                            cartProd.Add(new cart(associatedLabels.Item1.Text, 1, price));
                            infoForm info = new infoForm("Product Added to cart Successfully!");
                            info.Show();

                        }
                    }
                    else
                    {
                        int index = FindProductIndex(associatedLabels.Item1.Text);
                        cartProd[index].Quantity = cartProd[index].Quantity + 1;
                        infoForm info = new infoForm("Cart Updated Successfully!");
                        info.Show();

                    }

                    
                   
                }
            }
        }


        public int FindProductIndex(string productName)
        {
           
            for (int i = 0; i < cartProd.Count; i++)
            {
                if (cartProd[i].ProductName == productName)
                {
                    return i; 
                }
            }

            return -1; 
        }
        public int prodIncartExist(string productName)
        {
            foreach (var item in cartProd)
            {
                if(item.ProductName == productName)
                {
                    return item.Quantity;
                }

            }
            return -1;
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)  { }
        

        private void label2_Click(object sender, EventArgs e)
        { 
        }

        private void CustomerPage_Load(object sender, EventArgs e)
        { trackorderPanel.Visible = false;
            orderPanel.Visible = false;
          
            cartPanel.Visible = false;
            trackPanel.Visible = false;
            productPanel.Visible = true;
        }
        static System.Drawing.Image ResizeImage(System.Drawing.Image originalImage, int newWidth, int newHeight)
        {
            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);


            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resizedBitmap;
        }
        public void addProdinCart()
        {
            cartview.Rows.Clear();

            foreach (var item in cartProd)
            {
                DataGridViewRow row = new DataGridViewRow();
               
                row.Height = 100;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT IMAGE FROM PRODUCT WHERE NAME = :prodName";

                    using (OracleCommand command = new OracleCommand(selectQuery, connection))
                    {
                        command.Parameters.Add("prodName", OracleDbType.Varchar2).Value = item.ProductName;

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                byte[] imageData = (byte[])reader["IMAGE"];
                                System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageData));
                                System.Drawing.Image resizedImage = ResizeImage(image, 100, 100);

                                row.Cells.Add(new DataGridViewImageCell { Value = resizedImage });
                            }
                        }
                    }
                }
                row.Cells.Add(new DataGridViewTextBoxCell { Value = item.ProductName });
                System.Drawing.Image add = Resource1.icons8_minus_48;
                    // System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-plus-48.png");
                row.Cells.Add(new DataGridViewImageCell { Value = add});

                
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Quantity });

                System.Drawing.Image minus = Resource1.icons8_plus_48;
                    //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-minus-48.png");
                row.Cells.Add(new DataGridViewImageCell { Value = minus });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = item.Price });
                cartview.Rows.Add(row);
            }
        }
        private void cartview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }
            private void button1_Click(object sender, EventArgs e)
        {
            trackorderPanel.Visible = false;
            orderPanel.Visible = false;
            panel4.AutoScroll = false;
            btnpanel4.Controls.Clear();
            trackPanel.Visible = true;
            panel4.Visible = true;
            cartPanel.Visible = true;
            addProdinCart();
            if(cartview.Rows.Count>=1)
            {
                generateTotal(100, 16, 0);
            }
        }
        public void GetOrderHistory()
        {
            int custID = getCustomerId(email);


            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT O.OrderID, O.OrderDate, SUM(P.Price * OL.Quantity) AS TotalBill, O.Status " +
                                   "FROM Orders O " +
                                   "JOIN Customer C ON O.CustomerID = C.ID " +
                                   "JOIN OrderLineItems OL ON O.OrderID = OL.OrderID " +
                                   "JOIN Product P ON OL.ProductID = P.Product_ID " +
                                   "WHERE C.ID = :custID AND O.Status='delivered' OR O.Status='cancelled' " +
                                   "GROUP BY O.OrderID, O.OrderDate, O.Status";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add("custID", OracleDbType.Int32).Value = custID;

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(orderhistory);
                                row.Height = 30;
                                row.Cells[0].Value = reader["OrderID"].ToString();

                                // Assuming "OrderDate" is a DateTime
                                row.Cells[1].Value = Convert.ToDateTime(reader["OrderDate"]).ToString("yyyy-MM-dd");

                                row.Cells[2].Value = reader["TotalBill"].ToString();
                                row.Cells[3].Value = reader["Status"].ToString();
                                orderhistory.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        

       
        private void button2_Click(object sender, EventArgs e)
        {
            logoutToolStripMenuItem.Image = Resource1.icons8_logout_50__1_;
            //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-logout-50 (1).png");
            trackOrderToolStripMenuItem.Image = Resource1.icons8_track_64__1_;
            //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-track-64 (1).png");
            viewProductsToolStripMenuItem.Image = Resource1.icons8_product_24;
            //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-product-24.png");
            orderHistoryToolStripMenuItem.Image = Resource1.icons8_history_80;
            //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-history-80.png");
            contextMenuStrip1.BackColor = Color.Gainsboro;
            contextMenuStrip1.Show(button2, 0, button2.Height);
        }

       
        public bool updateStatus(int orderID, string status)
        {
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE ORDERS SET STATUS = :status WHERE ORDERID= :orderID";

                using (OracleCommand command = new OracleCommand(updateQuery, connection))
                {
                    command.Parameters.Add("status", OracleDbType.Varchar2).Value = status;
                    command.Parameters.Add("orderID", OracleDbType.Int32).Value = orderID;
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if(e.ColumnIndex==2)
            {
                object cellValue = cartview.Rows[e.RowIndex].Cells[1].Value;
                string productName = cellValue.ToString();
                if (FindProductIndex(productName) == -1)
                {
                   
                }
                else
                {
                    int index = FindProductIndex(productName);
                    cartProd[index].Quantity = cartProd[index].Quantity + 1;
                    addProdinCart();

                }
                generateTotal(100, 16, 0);

            }
            if(e.ColumnIndex==4) {

                object cellValue = cartview.Rows[e.RowIndex].Cells[1].Value;
                string productName = cellValue.ToString();
               
                    int index = FindProductIndex(productName);
                    cartProd[index].Quantity = cartProd[index].Quantity -1;
                if (cartProd[index].Quantity == 0)
                {
                    removefromcart(index);
                    addProdinCart();
                }
                addProdinCart();
                generateTotal(100, 16, 0);
            }
            if(e.ColumnIndex==6) {

                object cellValue = cartview.Rows[e.RowIndex].Cells[1].Value;
                string productName = cellValue.ToString();

                int index = FindProductIndex(productName);
                removefromcart(index);
                addProdinCart();
                generateTotal(100, 16, 0);
            }
        }
        public void removefromcart(int index)
        {
            cartProd.RemoveAt(index);   
        }
        public void generateTotal(double shippinfee,double tax , double discount )
        {
            double total = 0;
            foreach (var item in cartProd)
            {
                double t1 = item.Quantity*item.Price;
                total = total + t1;


            }
            total = total + shippinfee;
            total = total+total * (tax / 100);
            total = total-total * (discount / 100);
            shipingfee.Text="$"+ shippinfee.ToString();
            taxLbl.Text= tax.ToString()+"%";
            discountLbl.Text=  discount.ToString()+"%";
            totallbl.Text= "$" + total.ToString();

            }
        public bool checkStock()
        {
            int size = cartProd.Count;
            int i = 0;
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // Create a copy of the cartProd list
                List<cart> cartCopy = cartProd.ToList();


                foreach (var item in cartCopy)
                {
                    string selectQuery = "select stock from product where NAME=:prodName";
                    using (OracleCommand command = new OracleCommand(selectQuery, connection))
                    {
                        command.Parameters.Add("prodName", OracleDbType.Varchar2).Value = item.ProductName;
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int stck = reader.GetInt32(0);
                                if (stck == 0)
                                {
                                    int index = FindProductIndex(item.ProductName);
                                    removefromcart(index);
                                    i++;
                                }
                                if (stck < item.Quantity && stck != 0)
                                {
                                    int index = FindProductIndex(item.ProductName);
                                    cartProd[index].Quantity = stck;
                                    i++;
                                }
                            }
                        }
                    }
                }

                if (i > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public void insertOrderLine(int orderId)
            {
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                foreach (var item in cartProd)
                {
                    string insertQuery = "INSERT INTO OrderLINEITEMS (orderid,productid,quantity) VALUES (:oi,:pi,:q)";
                    using (OracleCommand command = new OracleCommand(insertQuery, connection))
                    {
                        int pi=getProductId(item.ProductName);
                        command.Parameters.Add("oi", OracleDbType.Int32).Value = orderId;
                        command.Parameters.Add("pi", OracleDbType.Int32).Value = pi;
                        command.Parameters.Add("q", OracleDbType.Int32).Value = item.Quantity;
                        UpdateProduct(item.ProductName,item.Quantity);
                        command.ExecuteNonQuery();
                    }

                    }
                }

            }
        private int getProductId(string bpname)
        {
            try
            {
              
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT PRODUCT_ID FROM PRODUCT WHERE NAME = :bpname";
                        command.Parameters.Add("bpname", OracleDbType.Varchar2).Value = bpname;

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
                return -1;
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"OracleException: Code={ex.Number}, Message={ex.Message}");

                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");

                return -1;
            }
        }
        public void insertOrder(DateTime currentDate)
        {
            int custid = getCustomerId(email);
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Orders (orderdate, status, customerid) VALUES (:od, :s, :cI)";

                using (OracleCommand command = new OracleCommand(insertQuery, connection))
                {
                    command.Parameters.Add("od", OracleDbType.TimeStamp).Value = currentDate;
                    command.Parameters.Add("s", OracleDbType.Varchar2).Value = "processing";
                    command.Parameters.Add("cI", OracleDbType.Int32).Value = custid;

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Order inserted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to insert order. No rows affected.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error inserting order: " + ex.Message);
                    }
                }
            }
        }

        public int getOrderId(string status, DateTime dateTime, int custid)
        {
            int orderId = -1;

          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT ORDERID FROM Orders WHERE status = :st AND TO_CHAR(orderdate, 'YYYY-MM-DD HH24:MI:SS') = :dt AND customerid = :ci";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("st", OracleDbType.Varchar2).Value = status;
                    command.Parameters.Add("dt", OracleDbType.Varchar2).Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    command.Parameters.Add("ci", OracleDbType.Int32).Value = custid;

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderId = reader.GetInt32(0);
                        }
                    }
                }
            }

            return orderId;
        }
        public bool ordermail( DataGridView dataGridView)
        {
            string otpmail_body = "Your Order is placed .Order Details are :" +
                "";

            string tableHeader = "<br/><br/><h2>Your Cart</h2><table border='1'><tr>";

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                tableHeader += $"<th>{column.HeaderText}</th>";
            }
            tableHeader += "</tr>";

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                tableHeader += "<tr>";
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn is DataGridViewImageColumn)
                    {
                        string imageUrl = cell.Value?.ToString();
                        tableHeader += $"<td><img src='{imageUrl}' alt='Image' width='100' height='100'></td>";
                    }
                    else
                    {
                        tableHeader += $"<td>{cell.Value}</td>";
                    }
                }
                tableHeader += "</tr>";
            }

            string tableFooter = "</table>";

            string emailBody = otpmail_body + tableHeader + tableFooter;

            try
            {
                using (MailMessage mail = new MailMessage("mamshoes089158@gmail.com", email, "Order placed", emailBody))
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("mamshoes089158@gmail.com", "tykhlxszcmdkepbz");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    return true;
                }
            }
            catch (Exception)
            {
                ErrorForm errorForm = new ErrorForm("Incorrect Email");
                errorForm.Show();
                return false;
            }
        }

        private bool UpdateProduct(string bpname, int stockChange)
        {
            int productId = getProductId(bpname);

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Use a parameterized query to update the stock
                    string updateQuery = "UPDATE PRODUCT " +
                                         "SET STOCK = STOCK - :stockChange " +
                                         "WHERE PRODUCT_ID = :productId";

                    using (OracleCommand command = new OracleCommand(updateQuery, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.Add("stockChange", OracleDbType.Int32).Value = stockChange;
                        command.Parameters.Add("productId", OracleDbType.Int32).Value = productId;

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception during update: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"OracleException: Code={ex.Number}, Message={ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }


        private void cnfrmOrder_Click(object sender, EventArgs e)
        {
            activity = "Placed Order";
            if(cartview.Rows.Count<=0)
            {
                shipingfee.Text = "";
                taxLbl.Text = "";
                discountLbl.Text = "";
                totallbl.Text = "";
                infoForm info = new infoForm("Cart is empty!");
                info.Show();

            }
            else
            {
                if(checkStock()==true)
                {
                    addProdinCart();
                    infoForm info = new infoForm("Cart is updated due to product shortage!");
                    info.Show();
                    if(cartview.Rows.Count<=0)
                    {
                        shipingfee.Text = "";
                        taxLbl.Text = "";
                        discountLbl.Text = "";
                        totallbl.Text = "";
                        infoForm info2= new infoForm("Cart is empty Order Failed!");
                        info2.Show();
                    }
                    else
                    {
                        DateTime currentTimestamp = DateTime.Now;
                        ordermail(cartview);
                        insertOrder(currentTimestamp);
                        int custid = getCustomerId(email);
                        int orderId=getOrderId("processing",currentTimestamp,custid);
                        insertOrderLine(orderId);
                        cartProd.Clear();
                        addProdinCart();
                        shipingfee.Text = "";
                        taxLbl.Text = "";
                        discountLbl.Text = "";
                        totallbl.Text = "";
                        infoForm infoForm = new infoForm("Order placed");
                        infoForm.Show();
                    }

                }
                else
                {
                    DateTime currentTimestamp = DateTime.Now;
                    ordermail(cartview);
                    insertOrder(currentTimestamp);
                    int custid = getCustomerId(email);
                    int orderId = getOrderId("processing", currentTimestamp, custid);
                    insertOrderLine(orderId);
                    cartProd.Clear();
                    addProdinCart();
                    shipingfee.Text ="";
                    taxLbl.Text = "";
                    discountLbl.Text = "";
                    totallbl.Text = "";
                    infoForm infoForm = new infoForm("Order placed");
                    infoForm.Show();

                }

            }


        }

        private void cartPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cancelOrder_Click_1(object sender, EventArgs e)
        {
          //  pictureBox2.Visible = false;
            int orderId = 0;
            if (int.TryParse(orderid, out orderId))
            {
                updateStatus(orderId, "cancelled");
            }
            //cancelOrder.Visible = false;

        }

        private void roundPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
