using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using DbProject.Resources;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Excel = Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Oracle.DataAccess.Types;
using iText.Layout.Borders;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DbProject
{
    public partial class Admin : Form
    {
        String connectionString = @"DATA SOURCE =localhost:1521/XE;USER ID =dbfinal; PASSWORD=dbfinal";
        SignInPage signInPage;
        Boolean optionCheck = false;
        Boolean listOders = false;
        //category check
        Boolean categoryaddRowCheck = false;
        Boolean categoryUpdateRowCheck = false;
        int updateRowIndex;
        int addrowIndex;
        string category1 = "";

        //type check
        Boolean typeaddRowCheck = false;
        Boolean typeUpdateRowCheck = false;
        int typeupdateRowIndex;
        int typeaddrowIndex;
        string type1 = "";
        string categorytype;
        //product check
        Boolean productAddRowCheck = false;
        Boolean productUpdateRowCheck = false;
        int productUpdateRowIndex;
        int productAddRowIndex;
        string imageUrl = "";
        string productName = "";
        string pcategory = "";
        string ptype = "";
        //pdf integer variables
        int prodi = 0;
        int useri = 0;
        int ordersi = 0;
        int salei = 0;
        //  admincheck
        Boolean adminAddRowCheck = false;
        Boolean adminUpdateRowCheck = false;
        int adminUpdateRowIndex;
        int adminAddRowIndex;
        string adminemail = "";
        public Admin(SignInPage a)
        {
            this.signInPage = a;


            InitializeComponent();
            emailTxt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);
            NameTxt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);
            Authtxt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);
            email2Txt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);
            dashBoardPanel.Visible = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ListOrders.Visible = false;
            roundPanel2.Visible = false;
            imageURLPanel.Visible = false;
            manageAdmin.Visible = true;
            dashBoardPanel.Visible = true;
            userLogsPanel.Visible = true;
            CategoryPanel.Visible = true;
            TypePanel.Visible = true;
            OrderPanel.Visible = true;
            productPanel.Visible = true;
            Salepanel1.Visible = true;
            mAdminPanel.Visible = false;
            CustCountLbl.Text = totalCustomer();
            deliveredLbl.Text = totalorders();
            pendingLbl.Text = totalpendingOrders();
            revenueLbl.Text=totalRevenue ();
            setBarchart();
            SetDoughnutChart();
            DashPanel.Visible = true;


        }

        public string totalCustomer()
        {
            string cust = "";
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(ID) AS TOTALCUST FROM CUSTOMER";
                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           cust=  reader["TOTALCUST"].ToString();
                        }
                    }
                }
            }
            return cust;
                       
        }

        public string totalorders()
        {
            string ord = "";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(OrderId) AS TOTALOrd FROM ORDERS WHERE STATUS='delivered'";
                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ord = reader["TOTALOrd"].ToString();
                        }
                    }
                }
            }
            return ord;

        }
        public string totalpendingOrders()
        {
            string ord = "";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(OrderId) AS TOTALOrd FROM ORDERS WHERE STATUS='processing'";
                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ord = reader["TOTALOrd"].ToString();
                        }
                    }
                }
            }
            return ord;

        }
        public void SetDoughnutChart()
        {
            chart2.Series["OrderStatus"].Points.Clear();
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT STATUS, COUNT(*) AS OrderCount
            FROM Orders
            GROUP BY STATUS ";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get data from the reader
                            string orderStatus = reader.GetString(0);
                            int orderCount = reader.GetInt32(1);

                            // Add data points to the doughnut chart
                            chart2.Series["OrderStatus"].Points.AddXY(orderStatus, orderCount);
                        }
                    }
                }
            }
        }

        public void setBarchart()
        {
            chart3.Series["Weekly Revenue"].Points.Clear();
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TO_NUMBER(TO_CHAR(o.ORDERDATE, 'WW')) - TO_NUMBER(TO_CHAR(TRUNC(o.ORDERDATE, 'MONTH'), 'WW')) + 1 AS WeekNumber, SUM(p.PRICE * ol.QUANTITY) AS WeeklyRevenue FROM Orders o JOIN OrderLineItems ol ON o.ORDERID = ol.ORDERID JOIN Product p ON ol.PRODUCTID=p.PRODUCT_ID WHERE TRUNC(o.ORDERDATE, 'MONTH') = TRUNC(SYSDATE, 'MONTH') GROUP BY TO_NUMBER(TO_CHAR(o.ORDERDATE, 'WW')) - TO_NUMBER(TO_CHAR(TRUNC(o.ORDERDATE, 'MONTH'), 'WW')) + 1 ORDER BY TO_NUMBER(TO_CHAR(o.ORDERDATE, 'WW')) - TO_NUMBER(TO_CHAR(TRUNC(o.ORDERDATE, 'MONTH'), 'WW')) + 1";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add data points to the chart
                            int weekNumber = reader.GetInt32(0);
                            double weeklyRevenue = reader.GetDouble(1);

                            chart3.Series["Weekly Revenue"].Points.AddXY($"Week {weekNumber}", weeklyRevenue);
                        }
                    }
                }
            }
        }
        public string totalRevenue()
        {
            string rev = "";

           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT SUM(p.price * o.quantity) AS TotalRevenue FROM Product p  JOIN Orderlineitems o ON o.productid = p.product_id";
                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rev = reader["TotalRevenue"].ToString();
                        }
                    }
                }
            }
            return rev;

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            productAddRowCheck = false; productUpdateRowCheck = false;
            roundPanel2.Visible = false;
            DashPanel.Visible = false;
            ListOrders.Visible = false;
            imageURLPanel.Visible = false;
            manageAdmin.Visible = true;
            dashBoardPanel.Visible = true;
            userLogsPanel.Visible = true;
            CategoryPanel.Visible = true;
            TypePanel.Visible = true;
            OrderPanel.Visible = true;
            Salepanel1.Visible = false;
            productPanel.Visible = true;
            getProductData();
        }
        public void getProductData()
        {

            dataGridView5.Rows.Clear();
            dataGridView5.RowTemplate.Height = 50;
            dataGridView5.Columns[0].ReadOnly = true;
            int count = 1;
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT    P.NAME AS PRODUCT_NAME,   P.DESCRIPTION,    P.IMAGE,    P.PRICE,    C.CATEGORY_NAME,  T.TYPE_NAME,    P.STOCK FROM   PRODUCT P JOIN    CATEGORYTYPE CT ON P.CATEGORYTYPEID = CT.ID  JOIN   CATEGORY C ON CT.CATEGORYID = C.CATEGORY_ID JOIN   TYPE T ON CT.TYPEID = T.TYPE_ID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView5);
                            row.Height = 100;
                            byte[] imageData = (byte[])reader["IMAGE"];
                            System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageData));
                            System.Drawing.Image resizedImage = ResizeImage(image, 100, 100);
                            row.Cells[0].Value = count.ToString();

                            row.Cells[1].Value = resizedImage;
                            row.Cells[2].Value = reader["PRODUCT_NAME"].ToString();

                            row.Cells[3].Value = reader["DESCRIPTION"].ToString();
                            row.Cells[4].Value = reader["STOCK"].ToString();
                            row.Cells[5].Value = reader["PRICE"].ToString();
                            row.Cells[6].Value = reader["CATEGORY_NAME"].ToString();
                            row.Cells[7].Value = reader["TYPE_NAME"].ToString();
                            // ((DataGridViewImageColumn)dataGridView5.Columns[0]).ImageLayout = DataGridViewImageCellLayout.Stretch;

                            dataGridView5.Rows.Add(row);
                            count++;
                        }
                    }
                }
            }
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }







        private void settings_Click(object sender, EventArgs e)
        {

            // Settingsmenu.BackColor = Color.Silver;
            manageAdminToolStripMenuItem.Image = Resource1.icons8_logout_30;
                // System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-user-24.png");
            manageAdminToolStripMenuItem.BackColor = Color.Silver;
            logoutToolStripMenuItem.BackColor = Color.Silver;

            logoutToolStripMenuItem.Image = Resource1.icons8_user_24;
                //System.Drawing.Image.FromFile("C:\\Users\\aslam\\Downloads\\icons8-logout-30.png");
            // Settingsmenu.BackColor = Color.LightGray; Settingsmenu.ForeColor = Color.Black;
            Settingsmenu.Show(settings, 0, settings.Height);

        }

      
       public void getAdmindata()
        {
            dataGridView7.Rows.Clear();
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = " select name ,email,authority from admin";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataGridViewRow row = new DataGridViewRow();

                            row.CreateCells(dataGridView7);

                            row.Cells[0].Value = reader["name"].ToString();
                            row.Cells[1].Value = reader["email"].ToString();
                            row.Cells[2].Value = reader["authority"].ToString();
                          

                            dataGridView7.Rows.Add(row);
                        }
                    }
                }
            }

        }
        private void manageAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (signInPage.getAuthority().Equals("1"))
            {
                dashBoardPanel.Visible = true;
                userLogsPanel.Visible = true;
                manageAdmin.Visible = true;
                ListOrders.Visible = false;
                roundPanel2.Visible = false;
                imageURLPanel.Visible = false;
                manageAdmin.Visible = true;
                dashBoardPanel.Visible = true;
                userLogsPanel.Visible = true;
                CategoryPanel.Visible = true;
                TypePanel.Visible = true;
                OrderPanel.Visible = true;
                productPanel.Visible = true;
                Salepanel1.Visible = true;
                DashPanel.Visible = true;  
              
                getAdmindata();
                mAdminPanel.Visible = true;
            }
            else
            {
                WarningForm warningForm = new WarningForm("Authorities Issue");
                warningForm.Show();
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
           
            About_us_page about_Us = new About_us_page(connectionString);
            about_Us.Show();


        }







        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (optionCheck == false)
            {
                optionCheck = true;
                ListPanel.Visible = true;

            }
            else
            {
                optionCheck = false;
                ListPanel.Visible = false;
            }

        }

        private void AddAdminList_Click(object sender, EventArgs e)
        {
            optionCheck = false;
            optionTxt.Texts = "Add Admin";
            ListPanel.Visible = false;


        }

        private void deleteAdminList_Click(object sender, EventArgs e)
        {
            optionCheck = false;
            optionTxt.Texts = "Delete Admin";
            ListPanel.Visible = false;


        }

        private void roundBtn1_Click_1(object sender, EventArgs e)
        {
            String email = emailTxt.Texts.Trim();
            String auth = Authtxt.Texts.Trim();
            String name = NameTxt.Texts.Trim();

            if (!auth.Equals("0") && !auth.Equals("1"))
            {
                ErrorForm errorForm = new ErrorForm("Authority must be 0 or 1" + auth);
                errorForm.ShowDialog();
            }
            else
            {
                Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                bool isValid = regex.IsMatch(emailTxt.Texts.Trim());
                if (isValid == false)
                {
                    ErrorForm errorForm = new ErrorForm("Email is not Valid ");
                    errorForm.ShowDialog();
                }
                else
                {
                    String pass = "";

                    Random rand = new Random();
                    int num = rand.Next(9, 15);

                    int total = 0;
                    do
                    {
                        int ch = rand.Next(48, 132);
                        if ((ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122))
                        {
                            pass = pass + (char)ch;
                            total++;
                            if (total == num)
                                break;
                        }

                    } while (true);
                    if (sendPassword(email, pass) == true)
                    {
                       
                        using (OracleConnection connection = new OracleConnection(connectionString))
                        {
                            connection.Open();

                            string insertQuery = "INSERT INTO ADMIN (NAME, EMAIL, PASSWORD, AUTHORITY) VALUES (:name, :email, :password, :authority)";

                            using (OracleCommand command = new OracleCommand(insertQuery, connection))
                            {
                                // Replace the parameter values with actual values
                                command.Parameters.Add("name", OracleDbType.Varchar2).Value = name;
                                command.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                                command.Parameters.Add("password", OracleDbType.Varchar2).Value = pass;
                                command.Parameters.Add("authority", OracleDbType.Int32).Value = auth;

                                command.ExecuteNonQuery();
                            }
                        }
                        infoForm info = new infoForm("Admin Added Successfully ! ");
                        info.Show();
                    }


                }

            }


        }
        private bool sendPassword(String email, String rand)
        {

            string otpmail_body = "To Access your Admin Account \n " +
                "Your Acoount Password is " + rand + ".\n\nPlease do not share to anyone." +
                "\n\nThanks!\n\n\nSent through authorized source.\n\n";
            try
            {
                using (MailMessage mail = new MailMessage("mamshoes089158@gmail.com", email, "Admin Account Password", otpmail_body))
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

                ErrorForm errorForm = new ErrorForm("Incorrect Email ");
                errorForm.Show();
                return false;
            }
        }

        private void roundBtn2_Click(object sender, EventArgs e)
        {
            String email = email2Txt.Texts.Trim();
            
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM ADMIN WHERE EMAIL = :email";

                using (OracleCommand command = new OracleCommand(deleteQuery, connection))
                {
                    command.Parameters.Add("email", OracleDbType.Varchar2).Value = email;

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        ErrorForm errorForm = new ErrorForm("Deletion Unsuccessful !");
                        errorForm.Show();
                    }
                    else
                    {
                        infoForm info = new infoForm("Deleted successfully ! ");
                        info.Show();
                    }

                }
            }


        }

        private void optionTxt__TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (optionCheck == false)
            {
                optionCheck = true;
                ListPanel.Visible = true;

            }
            else
            {
                optionCheck = false;
                ListPanel.Visible = false;
            }
        }

        private void email2Txt__TextChanged(object sender, EventArgs e)
        {
            email2Txt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);

        }

        private void Authtxt__TextChanged(object sender, EventArgs e)
        {
            Authtxt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);
        }

        private void emailTxt__TextChanged(object sender, EventArgs e)
        {
            emailTxt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);
        }

        private void NameTxt__TextChanged(object sender, EventArgs e)
        {
            NameTxt.SetTextAlignment(System.Windows.Forms.HorizontalAlignment.Left);
        }

        private void UserLogsBtn_Click(object sender, EventArgs e)
        {
            roundPanel2.Visible = true;
            DashPanel.Visible = false;
            ListOrders.Visible = false;
            // OrderPanel.Visible = false;// dashBoardPanel.Visible = false;
            TypePanel.Visible = false;
            CategoryPanel.Visible = false;
            userLogsPanel.Visible = true;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void manageAdmin_Paint(object sender, PaintEventArgs e)
        {

        }



        private void pictureBox9_Click_1(object sender, EventArgs e)
        {
            if (optionCheck == false)
            {
                optionCheck = true;
                ListPanel.Visible = true;

            }
            else
            {
                optionCheck = false;
                ListPanel.Visible = false;
            }
        }

        private void AllActivity_Click(object sender, EventArgs e)
        {
            optionCheck = false;
            ListPanel.Visible = false;
            roundTxtBox1.Texts = "All";
            dataGridView1.Rows.Clear();
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT  c.email,   ul.LoginTimeStamp,   ul.LogoutTimeStamp,  ul.Activity FROM   Customer c JOIN   UserLogs ul ON c.id = ul.CustomerID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView1);

                            row.Cells[0].Value = reader["EMAIL"].ToString();
                            row.Cells[1].Value = reader["LoginTimeStamp"].ToString();
                            row.Cells[2].Value = reader["LogoutTimeStamp"].ToString();
                            row.Cells[3].Value = reader["Activity"].ToString();

                            dataGridView1.Rows.Add(row);
                        }
                    }
                }
            }

        }

        private void PlacedOrderBtn_Click(object sender, EventArgs e)
        {
            optionCheck = false;
            ListPanel.Visible = false;
            roundTxtBox1.Texts = "Place Order Logs";
            dataGridView1.Rows.Clear();
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT  c.email,   ul.LoginTimeStamp,   ul.LogoutTimeStamp,  ul.Activity FROM   Customer c JOIN   UserLogs ul ON c.id = ul.CustomerID WHERE Activity='Placed Order'";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataGridViewRow row = new DataGridViewRow();

                            row.CreateCells(dataGridView1);

                            row.Cells[0].Value = reader["EMAIL"].ToString();
                            row.Cells[1].Value = reader["LoginTimeStamp"].ToString();
                            row.Cells[2].Value = reader["LogoutTimeStamp"].ToString();
                            row.Cells[3].Value = reader["Activity"].ToString();

                            dataGridView1.Rows.Add(row);
                        }
                    }
                }
            }

        }

        private void ViewBtn_Click(object sender, EventArgs e)
        {
            optionCheck = false;
            ListPanel.Visible = false;
            roundTxtBox1.Texts = "Viewed Site";
            dataGridView1.Rows.Clear();
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT  c.email,   ul.LoginTimeStamp,   ul.LogoutTimeStamp,  ul.Activity FROM   Customer c JOIN   UserLogs ul ON c.id = ul.CustomerID WHERE Activity='Viewed Site'";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView1);


                            row.Cells[0].Value = reader["EMAIL"].ToString();
                            row.Cells[1].Value = reader["LoginTimeStamp"].ToString();
                            row.Cells[2].Value = reader["LogoutTimeStamp"].ToString();
                            row.Cells[3].Value = reader["Activity"].ToString();

                            dataGridView1.Rows.Add(row);
                        }
                    }
                }
            }
        }

        private void downloadpdf(DataGridView dataGridView1, String fileName)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = fileName + ".pdf";
                bool ErrorMessage = false;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                           // MessageBox.Show("Unable to write data to disk: " + ex.Message);
                            infoForm info = new infoForm("Unable to write data to disk!");
                            info.Show();
                        }
                    }

                    if (!ErrorMessage)
                    {
                        try
                        {
                            Document pdfDocument = new Document(PageSize.A4);

                            PdfWriter writer = PdfWriter.GetInstance(pdfDocument, new FileStream(save.FileName, FileMode.Create));

                            pdfDocument.Open();

                            PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);
                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                PdfPCell headerCell = new PdfPCell(new Phrase(col.HeaderText));
                                pdfTable.AddCell(headerCell);
                            }
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.Value != null)
                                        pdfTable.AddCell(cell.Value.ToString());
                                    else
                                        pdfTable.AddCell("N/A");
                                }
                            }

                            pdfDocument.Add(pdfTable);
                            pdfDocument.Close();

                            infoForm info = new infoForm("Data Exported Successfully!");
                            info.Show();
                        }
                        catch (Exception ex)
                        {
                            infoForm info = new infoForm("Error while Exporting!");
                            info.Show();
                        }
                    }
                }
            }
            else
            {
                infoForm info = new infoForm("No data found !");
                info.Show();
            }
        }
        private void pdfBtn_Click(object sender, EventArgs e)
        {
            if (roundTxtBox1.Equals(""))
            {
                ErrorForm errorForm = new ErrorForm("Option Not Selected!");
                errorForm.Show();

            }
            else
            {
                downloadpdf(dataGridView1, "UserLogs"+useri+1);
                useri++;
            }
        }

        private void ExcelBtn_Click(object sender, EventArgs e)
        {
            if (roundTxtBox1.Equals(""))
            {
                ErrorForm errorForm = new ErrorForm("Option Not Selected!");
                errorForm.Show();

            }
            else
            {
                dataGridView1.SelectAll();
                DataObject copydata = dataGridView1.GetClipboardContent();
                if (copydata != null) Clipboard.SetDataObject(copydata);
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook xlWbook;
                Microsoft.Office.Interop.Excel.Worksheet xlsheet;
                object miseddata = System.Reflection.Missing.Value;
                xlWbook = xlapp.Workbooks.Add(miseddata);

                xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlsheet.Cells[1, 1];
                xlr.Select();

                xlsheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            DashPanel.Visible = false;
            roundPanel2.Visible = false;
            ListOrders.Visible = false;
            categoryaddRowCheck = false;
            categoryUpdateRowCheck = false;
            //OrderPanel.Visible = false;
            TypePanel.Visible = false;
            CategoryPanel.Visible = true;
            dataGridView2.Rows.Clear();
            dataGridView2.Columns[0].ReadOnly = true;
            int count = 1;
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "Select Category_Name from Category";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView2);
                            row.Height = 30;
                            row.Cells[0].Value = count.ToString();
                            row.Cells[1].Value = reader["Category_Name"].ToString();



                            dataGridView2.Rows.Add(row);
                            count++;
                        }
                    }
                }
            }

        }
        private bool deleteCategory(string category)
        {
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                int categoryId = getCategoryId(category);

                int categorytypeId = 0;

                string selectQuery = "SELECT Type_Name FROM Type WHERE Type_Id IN (SELECT Typeid FROM CategoryType WHERE CategoryId = (SELECT category_id FROM Category WHERE Category_Name = :category))";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("category", OracleDbType.Varchar2).Value = category;
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string type = reader.GetString(0);
                            Console.WriteLine(type);
                            categorytypeId = getCategoryTypeId(type, category);
                            Console.WriteLine(categorytypeId);
                            if (categorytypeId != -1)
                            {
                                string deleteProductQuery = "DELETE FROM Product WHERE CATEGORYTYPEID=:categoryTypeId";
                                using (OracleCommand command2 = new OracleCommand(deleteProductQuery, connection))
                                {
                                    command2.Parameters.Add("categoryTypeId", OracleDbType.Int32).Value = categorytypeId;
                                    command2.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                string deleteCategoryTypeQuery = "DELETE FROM CategoryType WHERE CategoryID = :categoryId";
                using (OracleCommand command = new OracleCommand(deleteCategoryTypeQuery, connection))
                {
                    command.Parameters.Add("categoryId", OracleDbType.Int32).Value = categoryId;
                    command.ExecuteNonQuery();
                }

                // Delete records from Type
                string deleteTypeQuery = "DELETE FROM Type WHERE Type_id NOT IN (SELECT DISTINCT(Typeid) FROM CategoryType)";
                using (OracleCommand command = new OracleCommand(deleteTypeQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Delete record from Category
                string deleteCategoryQuery = "DELETE FROM Category WHERE Category_Name = :category";
                using (OracleCommand command = new OracleCommand(deleteCategoryQuery, connection))
                {
                    // Correct the parameter name from categorytype to category
                    command.Parameters.Add("category", OracleDbType.Varchar2).Value = category;

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            }
        }


        public void getCategoryData()
        {
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[1].ReadOnly = false;
            dataGridView2.Rows.Clear();
            int count = 1;
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "Select Category_Name from Category";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView2);
                            row.Height = 30;
                            row.Cells[0].Value = count.ToString();
                            row.Cells[1].Value = reader["Category_Name"].ToString();
                            dataGridView2.Rows.Add(row);
                            count++;
                        }
                    }
                }
            }

        }
        public bool deleteType(string type, string category)
        {
           
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Get Category ID and Type ID
                    int categoryId = getCategoryId(category);
                    int typeId = getTypeId(type);
                    int categoryTypeId = getCategoryTypeId(type, category);

                    // Delete products from category type
                    string deleteProductQuery = "DELETE FROM Product WHERE CATEGORYTYPEID = :categoryTypeId";
                    using (OracleCommand command = new OracleCommand(deleteProductQuery, connection))
                    {
                        command.Parameters.Add("categoryTypeId", OracleDbType.Int32).Value = categoryTypeId;
                        command.ExecuteNonQuery();
                    }

                    // Delete records from CategoryType
                    string deleteCategoryTypeQuery = "DELETE FROM CategoryType WHERE CategoryID = :categoryId AND Typeid = :typeId";
                    using (OracleCommand command = new OracleCommand(deleteCategoryTypeQuery, connection))
                    {
                        command.Parameters.Add("categoryId", OracleDbType.Int32).Value = categoryId;
                        command.Parameters.Add("typeId", OracleDbType.Int32).Value = typeId;
                        command.ExecuteNonQuery();
                    }

                    // Delete records from Type
                    string deleteTypeQuery = "DELETE FROM Type WHERE Type_id NOT IN (SELECT DISTINCT(Typeid) FROM CategoryType)";
                    using (OracleCommand command = new OracleCommand(deleteTypeQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }



                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        private bool categorytypecheck(string category, string type)
        {
            int count = 0;
            string typeName = "";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Type_Name FROM Type WHERE Type_Id IN (SELECT Typeid FROM CategoryType WHERE CategoryId = (SELECT category_id FROM Category WHERE Category_Name = :category))";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("category", OracleDbType.Varchar2).Value = category;
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            typeName = reader["Type_Name"].ToString();
                            if (typeName.Equals(type))
                            { count++; }

                        }
                    }
                }
            }
            if (count > 0)
            {
                return true;
            }
            else { return false; }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (typeaddRowCheck == true && e.RowIndex != typeaddrowIndex)
                {
                    ErrorForm errorForm = new ErrorForm("Enter Value in new Row");
                    errorForm.Show();
                }
                else if (typeUpdateRowCheck == true && e.RowIndex != typeupdateRowIndex)
                {
                    object cellValue = dataGridView3.Rows[e.RowIndex].Cells[1].Value;
                    type1 = cellValue.ToString();
                    getTypeData(categorytype);
                    typeUpdateRowCheck = false;
                }
                else if (typeUpdateRowCheck == false && typeaddRowCheck == false)
                {
                    object cellValue = dataGridView3.Rows[e.RowIndex].Cells[1].Value;
                    type1 = cellValue.ToString();
                    typeUpdateRowCheck = true;
                    typeupdateRowIndex = e.RowIndex;
                }

            }
            else if (e.ColumnIndex == 2)
            {

                getTypeData(categorytype);
                object cellValue = dataGridView3.Rows[e.RowIndex].Cells[1].Value;

                string type = cellValue.ToString();
                bool check = deleteType(type, categorytype);
                if (check == true)
                {
                    infoForm infoForm = new infoForm("Deleted Successfully !");
                    infoForm.Show();
                }
                else
                {
                    infoForm infoForm = new infoForm("Deletion Failed! ");
                    infoForm.Show();

                }
                getTypeData(categorytype);

            }
            else if (e.ColumnIndex == 3)
            {
                if (typeaddRowCheck == true && e.RowIndex != typeaddrowIndex)
                {
                    ErrorForm errorForm = new ErrorForm("Enter Value in new Row");
                    errorForm.Show();
                }
                else if (typeaddRowCheck == true && e.RowIndex == typeaddrowIndex)
                {

                    object cellValue = dataGridView3.Rows[e.RowIndex].Cells[1].Value;

                    string type = cellValue.ToString();

                    if (type == "" || type == null)
                    {
                        ErrorForm errorForm = new ErrorForm("Enter Value in new Row");
                        errorForm.Show();
                    }
                    else
                    {
                        bool check1 = insertType(type, categorytype);
                        if (check1 == true)
                        {
                            infoForm info = new infoForm("Inserted Successfully!");
                            info.Show();

                        }
                        else
                        {
                            infoForm info = new infoForm("Insertion Failed!");
                            info.Show();
                            dataGridView3.Rows.RemoveAt(typeaddrowIndex);
                        }
                        typeaddRowCheck = false;
                    }

                }
                else
                {
                    object cellValue = dataGridView3.Rows[e.RowIndex].Cells[1].Value;

                    string type = cellValue.ToString();
                    if (type1 == "")
                    {
                        infoForm info = new infoForm("Updated Successfully!");
                        info.Show();
                        getTypeData(categorytype);
                    }
                    else if (type == "")
                    {
                        ErrorForm errorForm = new ErrorForm("Enter Value in Row");
                        errorForm.Show();
                        typeUpdateRowCheck = false;
                        getTypeData(categorytype);
                    }
                    else
                    {
                        Boolean check = UpdateType(type1, type);
                        if (check == true)
                        {
                            infoForm info = new infoForm("Updated Successfully!");
                            info.Show();
                            getTypeData(categorytype);
                            typeUpdateRowCheck = false;
                        }
                        else if (check == false)
                        {
                            infoForm info = new infoForm("Updation Failed!");
                            info.Show();
                            getTypeData(categorytype);
                            typeUpdateRowCheck = false;
                        }

                    }
                    type1 = "";
                }

            }

        }

        private bool UpdateType(string type1, string type)
        {
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Type SET type_Name = :type WHERE Type_Name = :type1";

                using (OracleCommand command = new OracleCommand(updateQuery, connection))
                {
                    command.Parameters.Add("type", OracleDbType.Varchar2).Value = type;
                    command.Parameters.Add("type1", OracleDbType.Varchar2).Value = type1;

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {

                        return false;
                    }
                }
            }
        }

        public void getTypeData(string category)
        {
            int count = 1;
            dataGridView3.Rows.Clear();
            dataGridView3.Columns[0].ReadOnly = true;
            dataGridView3.Columns[1].ReadOnly = false;

           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Type_Name FROM Type WHERE Type_Id IN (SELECT Typeid FROM CategoryType WHERE CategoryId = (SELECT category_id FROM Category WHERE Category_Name = :category))";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    command.Parameters.Add("category", OracleDbType.Varchar2).Value = categorytype;
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView3);
                            row.Height = 30;
                            row.Cells[0].Value = count.ToString();
                            row.Cells[1].Value = reader["Type_Name"].ToString();

                            dataGridView3.Rows.Add(row);
                            count++;
                        }
                    }
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
                        // Add error handling/logging
                        Console.WriteLine("Error executing SQL query: " + ex.Message);
                    }
                }
            }

            return categoryTypeId;
        }
        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 5)
            {


            }
            else if (e.ColumnIndex == 6)
            {
                object cellValue = dataGridView4.Rows[e.RowIndex].Cells[5].Value;
                string status = cellValue.ToString();


                object cellValue2 = dataGridView4.Rows[e.RowIndex].Cells[0].Value;
                string order_id = cellValue2.ToString();
                int parsedOrderId = 0;
                try
                {
                    parsedOrderId = int.Parse(order_id);

                    Console.WriteLine($"Parsed Order ID: {parsedOrderId}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format for Order ID" + order_id);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Order ID is too large to fit into an integer");
                }
                if (OrdersCBTxt.Texts == "Orders on the way")
                {
                    if (status == "delivered")
                    {
                        updateStatus(parsedOrderId, status);
                        infoForm info = new infoForm("Updated Successfully!");
                        info.Show();

                        onthewayGetData();
                    }
                    else
                    {
                        ErrorForm error = new ErrorForm("Invalid Status !");
                        error.Show();
                        onthewayGetData();

                    }
                }
                else if (OrdersCBTxt.Texts == "Processing Orders")
                {

                    if (status == "delivered" || status == "cancelled" || status == "on the way" || status == "processing")
                    {
                        updateStatus(parsedOrderId, status);
                        infoForm info = new infoForm("Updated Successfully!");
                        info.Show();
                        processingData();
                    }
                    else
                    {
                        ErrorForm error = new ErrorForm("Invalid Status !");
                        error.Show();
                        processingData();
                    }
                }

            }
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
        public void processingData()
        {
            dataGridView4.Rows.Clear();
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[3].ReadOnly = true;
            dataGridView4.Columns[4].ReadOnly = true;
            dataGridView4.Columns[5].ReadOnly = false;

            int columnIndex = 7;
            if (columnIndex > dataGridView4.Columns.Count)
            {
                dataGridView4.Columns.Add(Column11);
            }



            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT    O.OrderID,  C.Email,   O.OrderDate,    SUM(P.Price * OL.Quantity) AS TotalBill,   C.Address,   O.Status  FROM   Orders O  JOIN    Customer C ON O.CustomerID = C.ID  JOIN    OrderLineItems OL ON O.OrderID = OL.OrderID JOIN   Product P ON OL.ProductID = P.Product_ID  WHERE   O.Status = 'processing'  GROUP BY    O.OrderID, C.Email, O.OrderDate, C.Address, O.Status  ORDER BY   O.OrderID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView4);
                            row.Height = 30;
                            row.Cells[0].Value = reader["OrderID"].ToString();
                            row.Cells[1].Value = reader["Email"].ToString();
                            row.Cells[2].Value = reader["OrderDate"].ToString();
                            row.Cells[3].Value = reader["TotalBill"].ToString();
                            row.Cells[4].Value = reader["Address"].ToString();
                            row.Cells[5].Value = reader["Status"].ToString();

                            dataGridView4.Rows.Add(row);
                        }
                    }
                }
            }
        }


        public void onthewayGetData()
        {
            dataGridView4.Rows.Clear();
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[3].ReadOnly = true;
            dataGridView4.Columns[4].ReadOnly = true;
            dataGridView4.Columns[5].ReadOnly = false;

            int columnIndex = 7;
            if (columnIndex > dataGridView4.Columns.Count)
            {
                dataGridView4.Columns.Add(Column11);
            }



            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT    O.OrderID,  C.Email,   O.OrderDate,    SUM(P.Price * OL.Quantity) AS TotalBill,   C.Address,   O.Status  FROM   Orders O  JOIN    Customer C ON O.CustomerID = C.ID  JOIN    OrderLineItems OL ON O.OrderID = OL.OrderID JOIN   Product P ON OL.ProductID = P.Product_ID  WHERE   O.Status = 'on the way'  GROUP BY    O.OrderID, C.Email, O.OrderDate, C.Address, O.Status  ORDER BY   O.OrderID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView4);
                            row.Height = 30;
                            row.Cells[0].Value = reader["OrderID"].ToString();
                            row.Cells[1].Value = reader["Email"].ToString();
                            row.Cells[2].Value = reader["OrderDate"].ToString();
                            row.Cells[3].Value = reader["TotalBill"].ToString();
                            row.Cells[4].Value = reader["Address"].ToString();
                            row.Cells[5].Value = reader["Status"].ToString();

                            dataGridView4.Rows.Add(row);
                        }
                    }
                }
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.ColumnIndex;
            if (index == 1)
            {

                if (categoryaddRowCheck == true && e.RowIndex != addrowIndex)
                {
                    ErrorForm errorForm = new ErrorForm("Enter Value in new Row");
                    errorForm.Show();
                }
                else if (categoryUpdateRowCheck == true && e.RowIndex != updateRowIndex)
                {
                    object cellValue = dataGridView2.Rows[e.RowIndex].Cells[1].Value;
                    category1 = cellValue.ToString();
                    getCategoryData();
                    categoryUpdateRowCheck = false;
                }
                else if (categoryUpdateRowCheck == false && categoryaddRowCheck == false)
                {
                    object cellValue = dataGridView2.Rows[e.RowIndex].Cells[1].Value;
                    category1 = cellValue.ToString();
                    categoryUpdateRowCheck = true;
                    updateRowIndex = e.RowIndex;
                }

            }
            else if (e.ColumnIndex == 2)
            {

                if (categoryaddRowCheck == true && e.RowIndex != addrowIndex)
                {
                    ErrorForm errorForm = new ErrorForm("Enter Value in new Row");
                    errorForm.Show();
                }
                else if (categoryUpdateRowCheck == true && e.RowIndex != updateRowIndex)
                {
                    getCategoryData();
                }
                else
                {

                    object cellValue = dataGridView2.Rows[e.RowIndex].Cells[1].Value;

                    string category = cellValue.ToString();
                    Console.WriteLine(category);
                    bool check = deleteCategory(category);
                    getCategoryData();
                    if (check == true)
                    {
                        infoForm infoForm = new infoForm("Deleted Successfully !");
                        infoForm.Show();
                    }
                    else if (check == false)
                    {
                        infoForm infoForm = new infoForm("Deleted Failed!");
                        infoForm.Show();

                    }
                }




            }
            else if (e.ColumnIndex == 3)
            {
                int i = e.RowIndex;
                getCategoryData();
                object cellValue = dataGridView2.Rows[e.RowIndex].Cells[1].Value;
                categorytype = cellValue.ToString();
                dataGridView3.Rows.Clear();
                dataGridView3.Columns[0].ReadOnly = true;
                dataGridView3.Columns[1].ReadOnly = false;
                dataGridView3.Rows.Clear();
                int count = 1;
               
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT Type_Name FROM Type WHERE Type_Id IN (SELECT Typeid FROM CategoryType WHERE CategoryId = (SELECT category_id FROM Category WHERE Category_Name = :category))";

                    using (OracleCommand command = new OracleCommand(selectQuery, connection))
                    {
                        command.Parameters.Add("category", OracleDbType.Varchar2).Value = categorytype;
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGridView3);
                                row.Height = 30;
                                row.Cells[0].Value = count.ToString();
                                row.Cells[1].Value = reader["Type_Name"].ToString();

                                dataGridView3.Rows.Add(row);
                                count++;
                            }
                        }
                    }
                }
                productPanel.Visible = false;
                OrderPanel.Visible = false;
                TypePanel.Visible = true;



            }
            else if (e.ColumnIndex == 4)
            {

                if (categoryaddRowCheck == true && e.RowIndex != addrowIndex)
                {
                    ErrorForm errorForm = new ErrorForm("Enter Value in new Row");
                    errorForm.Show();
                }
                else if (categoryaddRowCheck == true && e.RowIndex == addrowIndex)
                {

                    object cellValue = dataGridView2.Rows[e.RowIndex].Cells[1].Value;

                    string category = cellValue.ToString();

                    if (category == "" || category == null)
                    {
                        ErrorForm errorForm = new ErrorForm("Enter Value in new Row");
                        errorForm.Show();
                    }
                    else
                    {
                        bool check1 = InsertCategory(category);
                        if (check1 == true)
                        {
                            infoForm info = new infoForm("Inserted Successfully!");
                            info.Show();

                        }
                        else
                        {
                            infoForm info = new infoForm("Insertion Failed!");
                            info.Show();
                            dataGridView2.Rows.RemoveAt(addrowIndex);
                        }
                        categoryaddRowCheck = false;
                    }

                }
                else
                {

                    object cellValue = dataGridView2.Rows[e.RowIndex].Cells[1].Value;

                    string category = cellValue.ToString();
                    if (category1 == "")
                    {
                        infoForm info = new infoForm("Updated Successfully!");
                        info.Show();
                        getCategoryData();
                    }
                    else if (category == "")
                    {
                        ErrorForm errorForm = new ErrorForm("Enter Value in Row");
                        errorForm.Show();
                        categoryUpdateRowCheck = false;
                        getCategoryData();
                    }
                    else
                    {
                        Boolean check = UpdateCategory(category1, category);
                        if (check == true)
                        {
                            infoForm info = new infoForm("Updated Successfully!");
                            info.Show();
                            getCategoryData();
                            categoryUpdateRowCheck = false;
                        }
                        else if (check == false)
                        {
                            infoForm info = new infoForm("Updation Failed!");
                            info.Show();
                            getCategoryData();
                            categoryUpdateRowCheck = false;
                        }

                    }
                    category1 = "";
                }
            }

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
                            // Assuming Category_ID is an integer
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
                            // Assuming Category_ID is an integer
                            c = reader.GetInt32(0);
                        }
                    }
                }
            }

            return c;

        }
        private bool UpdateCategory(string oldCategoryName, string newCategoryName)
        {
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Category SET Category_Name = :newCategoryName WHERE Category_Name = :oldCategoryName";

                using (OracleCommand command = new OracleCommand(updateQuery, connection))
                {
                    command.Parameters.Add("newCategoryName", OracleDbType.Varchar2).Value = newCategoryName;
                    command.Parameters.Add("oldCategoryName", OracleDbType.Varchar2).Value = oldCategoryName;

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }


        private bool InsertCategory(string categoryName)
        {
            
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Category (Category_Name) VALUES (:categoryName)";

                using (OracleCommand command = new OracleCommand(insertQuery, connection))
                {
                    command.Parameters.Add("categoryName", OracleDbType.Varchar2).Value = categoryName;

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }

        public bool insertType(string type, string category)
        {

            if (categorytypecheck(category, type) == true)
            {

                return false;
            }
            else
            {

                int type_id = -1;
                int category_id = getCategoryId(category);
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    String selQuery = "Select Type_Id from Type WHERE Type_Name=:type";
                    using (OracleCommand command = new OracleCommand(selQuery, connection))
                    {
                        command.Parameters.Add("type", OracleDbType.Varchar2).Value = type;
                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                type_id = reader.GetInt32(0);
                            }
                        }
                        if (type_id == -1)
                        {
                            //insert in type
                            string insertQuery = "Insert into Type(Type_Name) Values(:type)";
                            using (OracleCommand command2 = new OracleCommand(insertQuery, connection))
                            {

                                command2.Parameters.Add("type", OracleDbType.Varchar2).Value = type;
                                command2.ExecuteNonQuery();

                            }
                            String selectQuery = "Select Type_Id from Type WHERE Type_Name=:type";
                            using (OracleCommand command3 = new OracleCommand(selectQuery, connection))
                            {
                                command3.Parameters.Add("type", OracleDbType.Varchar2).Value = type;
                                command3.ExecuteNonQuery();

                                using (OracleDataReader reader = command3.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        type_id = reader.GetInt32(0);
                                    }
                                }
                            }

                            String insertQuery2 = "Insert into CategoryType (TypeId,CategoryId) Values(:type_id,:catergory_id)";
                            using (OracleCommand command4 = new OracleCommand(insertQuery2, connection))
                            {
                                command4.Parameters.Add("type_id", OracleDbType.Int32).Value = type_id;
                                command4.Parameters.Add("category_id", OracleDbType.Int32).Value = category_id;

                                command4.ExecuteNonQuery();
                            }

                            return true;
                        }
                        else
                        {
                            String insertQuery2 = "Insert into CategoryType (TypeId,CategoryId) Values(:type_id,:catergory_id)";
                            using (OracleCommand command4 = new OracleCommand(insertQuery2, connection))
                            {
                                command4.Parameters.Add("type_id", OracleDbType.Int32).Value = type_id;
                                command4.Parameters.Add("category_id", OracleDbType.Int32).Value = category_id;

                                command4.ExecuteNonQuery();
                            }

                            return true;
                        }
                    }

                }


            }

        }

        private void addCategoryBtn_Click(object sender, EventArgs e)
        {
            if (categoryaddRowCheck == true)
            { categoryaddRowCheck = false; }
            else
            {
                categoryaddRowCheck = true;
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView2);
                newRow.Cells[0].Value = (dataGridView2.Rows.Count).ToString();
                dataGridView2.Rows.Add(newRow);
                addrowIndex = dataGridView2.Rows.Count - 2;
            }

        }

        private void addType_Click(object sender, EventArgs e)
        {
            if (typeaddRowCheck == true)
            {

            }
            else
            {
                typeaddRowCheck = true;
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView3);
                newRow.Cells[0].Value = (dataGridView3.Rows.Count).ToString();
                dataGridView3.Rows.Add(newRow);
                typeaddrowIndex = dataGridView3.Rows.Count - 2;
            }

        }

        private void ordersBtn_Click(object sender, EventArgs e)
        {
            DashPanel.Visible = false;
            ListOrders.Visible = false;
            roundPanel2.Visible = true;
            manageAdmin.Visible = true;
            dashBoardPanel.Visible = true;
            userLogsPanel.Visible = true;
            CategoryPanel.Visible = true;
            TypePanel.Visible = true;
            productPanel.Visible = false;
            OrderPanel.Visible = true;
            roundBtn3.Visible = true;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (listOders == true)
            {

                ListOrders.BringToFront();
                roundPanel2.BringToFront();
                ListOrders.Visible = false;
                listOders = false;
            }
            else
            {
                ListOrders.BringToFront();
                roundPanel2.BringToFront();
                ListOrders.Visible = true;
                listOders = true;
            }
        }

        private void AllOrder_Click(object sender, EventArgs e)
        {
            OrdersCBTxt.Texts = "All Orders";
            ListOrders.Visible = false;
            listOders = false;

            dataGridView4.Rows.Clear();
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[3].ReadOnly = true;
            dataGridView4.Columns[4].ReadOnly = true;
            dataGridView4.Columns[5].ReadOnly = true;
            int columnIndexToRemove = 6;

            // Check if the index is within the valid range before removing the column
            if (columnIndexToRemove >= 0 && columnIndexToRemove < dataGridView4.Columns.Count)
            {
                dataGridView4.Columns.RemoveAt(columnIndexToRemove);
            }
         
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT O.OrderID, C.Email, O.OrderDate, SUM(P.Price * OL.Quantity) AS TotalBill, C.Address, O.Status FROM Orders O JOIN Customer C ON O.CustomerID = C.ID JOIN OrderLineItems OL ON O.OrderID = OL.OrderID JOIN Product P ON OL.ProductID = P.Product_ID GROUP BY O.OrderID, C.Email, O.OrderDate, C.Address, O.Status ORDER BY O.OrderID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView4); // Change to dataGridView4
                            row.Height = 30;
                            row.Cells[0].Value = reader["OrderID"].ToString();
                            row.Cells[1].Value = reader["Email"].ToString();
                            row.Cells[2].Value = reader["OrderDate"].ToString();
                            row.Cells[3].Value = reader["TotalBill"].ToString();
                            row.Cells[4].Value = reader["Address"].ToString();
                            row.Cells[5].Value = reader["Status"].ToString();

                            dataGridView4.Rows.Add(row); // Change to dataGridView4
                        }
                    }
                }
            }
        }


        private void DeliveredOrderBtn_Click(object sender, EventArgs e)
        {
            OrdersCBTxt.Texts = "Delivered Orders";
            ListOrders.Visible = false;
            listOders = false;

            dataGridView4.Rows.Clear();
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[3].ReadOnly = true;
            dataGridView4.Columns[4].ReadOnly = true;
            dataGridView4.Columns[5].ReadOnly = true;
            int columnIndexToRemove = 6;

            // Check if the index is within the valid range before removing the column
            if (columnIndexToRemove >= 0 && columnIndexToRemove < dataGridView4.Columns.Count)
            {
                dataGridView4.Columns.RemoveAt(columnIndexToRemove);
            }
           
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT    O.OrderID,  C.Email,   O.OrderDate,    SUM(P.Price * OL.Quantity) AS TotalBill,   C.Address,   O.Status  FROM   Orders O  JOIN    Customer C ON O.CustomerID = C.ID  JOIN    OrderLineItems OL ON O.OrderID = OL.OrderID JOIN   Product P ON OL.ProductID = P.Product_ID  WHERE   O.Status = 'delivered'  GROUP BY    O.OrderID, C.Email, O.OrderDate, C.Address, O.Status  ORDER BY   O.OrderID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView4);
                            row.Height = 30;
                            row.Cells[0].Value = reader["OrderID"].ToString();
                            row.Cells[1].Value = reader["Email"].ToString();
                            row.Cells[2].Value = reader["OrderDate"].ToString();
                            row.Cells[3].Value = reader["TotalBill"].ToString();
                            row.Cells[4].Value = reader["Address"].ToString();
                            row.Cells[5].Value = reader["Status"].ToString();

                            dataGridView4.Rows.Add(row);
                        }
                    }
                }
            }

        }

        private void processingOrdersBtn_Click(object sender, EventArgs e)
        {

            OrdersCBTxt.Texts = "Processing Orders";
            ListOrders.Visible = false;
            listOders = false;

            dataGridView4.Rows.Clear();
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[3].ReadOnly = true;
            dataGridView4.Columns[4].ReadOnly = true;
            dataGridView4.Columns[5].ReadOnly = false;
            int columnIndex = 7;

            if (columnIndex > dataGridView4.Columns.Count)
            {
                dataGridView4.Columns.Add(Column11);
            }
            
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT   O.OrderID,  C.Email,   O.OrderDate,    SUM(P.Price * OL.Quantity) AS TotalBill,   C.Address,   O.Status  FROM   Orders O  JOIN    Customer C ON O.CustomerID = C.ID  JOIN    OrderLineItems OL ON O.OrderID = OL.OrderID JOIN   Product P ON OL.ProductID = P.Product_ID  WHERE   O.Status = 'processing'  GROUP BY    O.OrderID, C.Email, O.OrderDate, C.Address, O.Status  ORDER BY   O.OrderID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView4);
                            row.Height = 30;
                            row.Cells[0].Value = reader["OrderID"].ToString();
                            row.Cells[1].Value = reader["Email"].ToString();
                            row.Cells[2].Value = reader["OrderDate"].ToString();
                            row.Cells[3].Value = reader["TotalBill"].ToString();
                            row.Cells[4].Value = reader["Address"].ToString();
                            row.Cells[5].Value = reader["Status"].ToString();

                            dataGridView4.Rows.Add(row);
                        }
                    }
                }
            }
        }

        private void cancelledOrderBtn_Click(object sender, EventArgs e)
        {
            OrdersCBTxt.Texts = "Cancelled Orders";
            ListOrders.Visible = false;
            listOders = false;

            dataGridView4.Rows.Clear();
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[3].ReadOnly = true;
            dataGridView4.Columns[4].ReadOnly = true;
            dataGridView4.Columns[5].ReadOnly = true;
            int columnIndexToRemove = 6;

            // Check if the index is within the valid range before removing the column
            if (columnIndexToRemove >= 0 && columnIndexToRemove < dataGridView4.Columns.Count)
            {
                dataGridView4.Columns.RemoveAt(columnIndexToRemove);
            }
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT    O.OrderID,  C.Email,   O.OrderDate,    SUM(P.Price * OL.Quantity) AS TotalBill,   C.Address,   O.Status  FROM   Orders O  JOIN    Customer C ON O.CustomerID = C.ID  JOIN    OrderLineItems OL ON O.OrderID = OL.OrderID JOIN   Product P ON OL.ProductID = P.Product_ID  WHERE   O.Status = 'cancelled'  GROUP BY    O.OrderID, C.Email, O.OrderDate, C.Address, O.Status  ORDER BY   O.OrderID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView4);
                            row.Height = 30;
                            row.Cells[0].Value = reader["OrderID"].ToString();
                            row.Cells[1].Value = reader["Email"].ToString();
                            row.Cells[2].Value = reader["OrderDate"].ToString();
                            row.Cells[3].Value = reader["TotalBill"].ToString();
                            row.Cells[4].Value = reader["Address"].ToString();
                            row.Cells[5].Value = reader["Status"].ToString();

                            dataGridView4.Rows.Add(row);
                        }
                    }
                }
            }

        }

        private void WayOrderBtn_Click(object sender, EventArgs e)
        {
            OrdersCBTxt.Texts = "Orders on the way";
            ListOrders.Visible = false;
            listOders = false;
            dataGridView4.Rows.Clear();
            dataGridView4.Columns[0].ReadOnly = true;
            dataGridView4.Columns[1].ReadOnly = true;
            dataGridView4.Columns[2].ReadOnly = true;
            dataGridView4.Columns[3].ReadOnly = true;
            dataGridView4.Columns[4].ReadOnly = true;
            dataGridView4.Columns[5].ReadOnly = false;
            int columnIndex = 7;
            if (columnIndex > dataGridView4.Columns.Count)
            {
                dataGridView4.Columns.Add(Column11);
            }


            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT    O.OrderID,  C.Email,   O.OrderDate,    SUM(P.Price * OL.Quantity) AS TotalBill,   C.Address,   O.Status  FROM   Orders O  JOIN    Customer C ON O.CustomerID = C.ID  JOIN    OrderLineItems OL ON O.OrderID = OL.OrderID JOIN   Product P ON OL.ProductID = P.Product_ID  WHERE   O.Status = 'on the way'  GROUP BY    O.OrderID, C.Email, O.OrderDate, C.Address, O.Status  ORDER BY   O.OrderID";

                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView4);
                            row.Height = 30;
                            row.Cells[0].Value = reader["OrderID"].ToString();
                            row.Cells[1].Value = reader["Email"].ToString();
                            row.Cells[2].Value = reader["OrderDate"].ToString();
                            row.Cells[3].Value = reader["TotalBill"].ToString();
                            row.Cells[4].Value = reader["Address"].ToString();
                            row.Cells[5].Value = reader["Status"].ToString();

                            dataGridView4.Rows.Add(row);
                        }
                    }
                }
            }
        }
        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==0)
            {
                if (adminAddRowCheck == true && e.RowIndex != adminAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
              

            }
            if(e.ColumnIndex==1)
            {
                if (adminAddRowCheck == true && e.RowIndex != adminAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
                else if (adminAddRowCheck == true && e.RowIndex == adminAddRowIndex)
                {

                }
                else if(adminAddRowCheck == false && e.RowIndex != adminAddRowIndex)
                {
                    object cellValue = dataGridView7.Rows[e.RowIndex].Cells[1].Value;

                    adminemail = cellValue.ToString();
                }
                }
            if(e.ColumnIndex==2)
            {
                if (adminAddRowCheck == true && e.RowIndex != adminAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
            }
            if(e.ColumnIndex==3)
            {
                if (adminAddRowCheck == true && e.RowIndex != adminAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
                else
                {
                    getAdmindata();
                    object cellValue = dataGridView7.Rows[e.RowIndex].Cells[1].Value;
                    string email = cellValue.ToString();
                    deleteAdmin(email);
                    getAdmindata();
                }
            }
            if(e.ColumnIndex==4)
            {
                if (adminAddRowCheck == true && e.RowIndex != adminAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
                else if (adminAddRowCheck == true && e.RowIndex == adminAddRowIndex)
                {
                    object cellValue = dataGridView7.Rows[e.RowIndex].Cells[1].Value;
                    string email = cellValue.ToString();
                    object cellValue2= dataGridView7.Rows[e.RowIndex].Cells[0].Value;
                    string name = cellValue2.ToString();
                    object cellValue3 = dataGridView7.Rows[e.RowIndex].Cells[2].Value;
                    string a = cellValue3.ToString();
                    if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name)|| !a.Equals("1")&&!a.Equals("0"))
                    {
                        ErrorForm error = new ErrorForm("Add correct values in new row! ");
                        error.Show();
                    }
                    else
                    {
                        Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                        bool isValid = regex.IsMatch(email);
                        if (isValid == false)
                        {
                            ErrorForm errorForm = new ErrorForm("Email is not Valid ");
                            errorForm.ShowDialog();
                        }
                        else
                        {
                            String pass = "";

                            Random rand = new Random();
                            int num = rand.Next(9, 15);

                            int total = 0;
                            do
                            {
                                int ch = rand.Next(48, 132);
                                if ((ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122))
                                {
                                    pass = pass + (char)ch;
                                    total++;
                                    if (total == num)
                                        break;
                                }

                            } while (true);

                            if (sendPassword(email, pass) == true)
                            {
                                insertAdmin(name, email, pass, a);
                            }
                            getAdmindata();
                        }
                        getAdmindata();
                    }
                    adminAddRowCheck = false;

                }
                else
                {
                    if(string.IsNullOrEmpty(adminemail))
                    {
                        object cellValue = dataGridView7.Rows[e.RowIndex].Cells[1].Value;
                        string email = cellValue.ToString();
                        object cellValue2 = dataGridView7.Rows[e.RowIndex].Cells[0].Value;
                        string name = cellValue2.ToString();
                        object cellValue3 = dataGridView7.Rows[e.RowIndex].Cells[2].Value;
                        string a = cellValue3.ToString();
                        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) || !a.Equals("1") && !a.Equals("0"))
                        {
                            ErrorForm error = new ErrorForm("Add correct values in new row! ");
                            error.Show();
                        }
                        else
                        {
                            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                            bool isValid = regex.IsMatch(email);
                            if (isValid == false)
                            {
                                ErrorForm errorForm = new ErrorForm("Email is not Valid ");
                                errorForm.ShowDialog();
                            }
                            else
                            {
                                updateAdmin(email, email, name, a);
                            }
                        }
                    }
                    else
                    {
                        object cellValue = dataGridView7.Rows[e.RowIndex].Cells[1].Value;
                        string email = cellValue.ToString();
                        object cellValue2 = dataGridView7.Rows[e.RowIndex].Cells[0].Value;
                        string name = cellValue2.ToString();
                        object cellValue3 = dataGridView7.Rows[e.RowIndex].Cells[2].Value;
                        string a = cellValue3.ToString();
                        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) || !a.Equals("1") && !a.Equals("0"))
                        {
                            ErrorForm error = new ErrorForm("Add correct values in new row! ");
                            error.Show();
                        }
                        else
                        {
                            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                            bool isValid = regex.IsMatch(email);
                            if (isValid == false)
                            {
                                ErrorForm errorForm = new ErrorForm("Email is not Valid ");
                                errorForm.ShowDialog();
                            }
                            else
                            {
                                updateAdmin(adminemail, email, name, a);
                            }
                        }

                    }
                    getAdmindata();
                }

            }
        }
        private void updateAdmin(string currentEmail, string newEmail, string newName, string newAuthority)
        {
            try
            {
                
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                 
                    string updateQuery = "UPDATE Admin SET name = :newName, email = :newEmail, authority ="+newAuthority+" WHERE email = :currentEmail";

                    using (OracleCommand command = new OracleCommand(updateQuery, connection))
                    {
                        command.Parameters.Add(":newName", OracleDbType.Varchar2).Value = newName;
                        command.Parameters.Add(":newEmail", OracleDbType.Varchar2).Value = newEmail;
                       // command.Parameters.Add(":newAuthority", OracleDbType.Int32).Value = newAuthority;
                        command.Parameters.Add(":currentEmail", OracleDbType.Varchar2).Value = currentEmail;

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            infoForm infoForm = new infoForm("Updated Successfully!");
                            infoForm.Show();
                        }
                        else
                        {
                            infoForm infoForm = new infoForm("Updation Failed!");
                            infoForm.Show();
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"OracleException: Code={ex.Number}, Message={ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void insertAdmin(string name, string email, string pass, string authority)
        {
            try
            {
                if (int.TryParse(authority, out int parsedAuthority))
                {
                  
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        connection.Open();

                        string insertQuery = "INSERT INTO Admin (name, email, password, authority) VALUES (:name, :email, :pass, :authority)";

                        using (OracleCommand command = new OracleCommand(insertQuery, connection))
                        {
                            command.Parameters.Add(":name", OracleDbType.Varchar2).Value = name;
                            command.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;
                            command.Parameters.Add(":pass", OracleDbType.Varchar2).Value = pass;
                            command.Parameters.Add(":authority", OracleDbType.Int32).Value = parsedAuthority;

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                infoForm infoForm = new infoForm("Inserted Successfully!");
                                infoForm.Show();
                            }
                            else
                            {
                                infoForm infoForm = new infoForm("Insertion Failed!");
                                infoForm.Show();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid authority value. Please provide a valid integer.");
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"OracleException: Code={ex.Number}, Message={ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }


        private void deleteAdmin(string email)
        {
            try
            {
              
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                   
                    string deleteQuery = "DELETE FROM Admin WHERE email = :email";

                    using (OracleCommand command = new OracleCommand(deleteQuery, connection))
                    {
                        command.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;

                        
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            infoForm infoForm = new infoForm("Admin Deleted Successfully!");
                            infoForm.Show();
                            Console.WriteLine("Admin deleted successfully.");
                        }
                        else
                        {
                            infoForm infoForm = new infoForm("Deletion Failed!");
                            infoForm.Show();
                            Console.WriteLine("Admin not found or deletion failed.");
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                // Console.WriteLine($"OracleException: Code={ex.Number}, Message={ex.Message}");
            }
            catch (Exception ex)
            {
              
                //Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int delind = 0;
            if (e.ColumnIndex == 1)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
                else if (productAddRowCheck == true && e.RowIndex == productAddRowIndex)
                {
                    Cancel.BringToFront();
                    imageURLPanel.Visible = true;

                }
                else
                {
                    infoForm info = new infoForm("Product Image can not be updated!");
                    info.Show();
                }

            }
            if (e.ColumnIndex == 2)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
                else if (productAddRowCheck == true && e.RowIndex == productAddRowIndex)
                {

                }
                else
                {
                    object cellValue = dataGridView5.Rows[e.RowIndex].Cells[2].Value;

                    productName = cellValue.ToString();
                    delind = e.RowIndex;
                    Console.WriteLine(productName);

                }

            }
            if (e.ColumnIndex == 3)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
            }
            if (e.ColumnIndex == 4)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
            }
            if (e.ColumnIndex == 5)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
            }
            if (e.ColumnIndex == 6)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
            }
            if (e.ColumnIndex == 7)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
            }

            if (e.ColumnIndex == 8)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
                else
                {
                    if (productName == "" && e.RowIndex != delind)
                    {
                        object cellValue = dataGridView5.Rows[e.RowIndex].Cells[2].Value;
                        productName = cellValue.ToString();
                    }
                    bool check = deleteProduct(productName);
                    if (check == true)
                    {
                        infoForm info = new infoForm("Product deleted successfully !");
                        info.Show();
                    }
                    else
                    {
                        infoForm info = new infoForm("Deletion Failed!");
                        info.Show();
                    }

                    getProductData();
                }

            }
            if (e.ColumnIndex == 9)
            {
                if (productAddRowCheck == true && e.RowIndex != productAddRowIndex)
                {
                    ErrorForm error = new ErrorForm("Add values in new row! ");
                    error.Show();
                }
                else
                {
                    try
                    {
                        object cellValue = dataGridView5.Rows[e.RowIndex].Cells[6].Value;
                        pcategory = cellValue.ToString();
                        object cellValue2 = dataGridView5.Rows[e.RowIndex].Cells[7].Value;
                        ptype = cellValue2.ToString();

                    }
                    catch (NullReferenceException ex)
                    {
                        ErrorForm error = new ErrorForm("Invalid Cell Value");
                        error.Show();
                    }
                    int typeid = getTypeId(ptype);
                    int categoryid = getCategoryId(pcategory);

                    int categorytypeid = getCategoryTypeId(ptype, pcategory);
                    if (categorytypeid == -1)
                    {
                        infoForm info = new infoForm("Inavalid Type or Category");
                        info.Show();
                        if (productAddRowCheck == true && e.RowIndex == productAddRowIndex)
                        {
                            dataGridView5.Rows.RemoveAt(e.RowIndex);


                        }
                        getProductData();

                    }
                    else
                    {
                        string pName = "";
                        string description = "";
                        string stock = "";
                        string p = "";
                        int s = 0;
                        double pric = 0;
                        try
                        {
                            object cellValue3 = dataGridView5.Rows[e.RowIndex].Cells[2].Value;
                            pName = cellValue3.ToString();
                            object cellValue4 = dataGridView5.Rows[e.RowIndex].Cells[3].Value;
                            description = cellValue4.ToString();
                            object cellValue5 = dataGridView5.Rows[e.RowIndex].Cells[4].Value;
                            stock = cellValue5.ToString();
                            object cellValue6 = dataGridView5.Rows[e.RowIndex].Cells[5].Value;
                            p = cellValue6.ToString();

                        }
                        catch (NullReferenceException ex)
                        {

                        }
                        if (productAddRowCheck == true && e.RowIndex == productAddRowIndex)
                        {
                            if (description == null || string.IsNullOrEmpty(pName))
                            {
                                ErrorForm error = new ErrorForm("Invalid Cell Value");
                                error.Show();
                                if (int.TryParse(stock, out s) && double.TryParse(p, out pric))
                                {

                                }
                                else
                                {
                                    ErrorForm error2 = new ErrorForm("Price and Stock  must be integer");
                                    error2.Show();
                                }
                                dataGridView5.Rows.RemoveAt(e.RowIndex);
                                getProductData();

                            }
                            else
                            {
                                if (int.TryParse(stock, out s))
                                {
                                    if (double.TryParse(p, out pric))
                                    {

                                    }
                                    else
                                    {
                                        ErrorForm error2 = new ErrorForm("Price must be Number");
                                        error2.Show();
                                    }
                                }
                                else
                                {
                                    ErrorForm error2 = new ErrorForm("Price and Stock  must be Integer");
                                    error2.Show();
                                }
                                SaveFileToFolder(imageUrl, "C:\\Product");
                                bool check = InsertProduct(pName, description, categorytypeid, pric, s, Path.GetFileName(imageUrl));
                                if (check == true)
                                {
                                    infoForm infoForm = new infoForm("Inserted Successfully!");
                                    infoForm.Show();
                                    getProductData();
                                }
                                else
                                {
                                    infoForm infoForm = new infoForm("Insertion Failed!");
                                    infoForm.Show();
                                    getProductData();
                                }
                            }

                        }
                        else
                        {


                            if (description == null || string.IsNullOrEmpty(pName))
                            {
                                ErrorForm error = new ErrorForm("Invalid Cell Value");
                                error.Show();
                                if (int.TryParse(stock, out s) && double.TryParse(p, out pric))
                                {

                                }
                                else
                                {
                                    ErrorForm error2 = new ErrorForm("Price and Stock  must be integer");
                                    error2.Show();
                                }
                                dataGridView5.Rows.RemoveAt(e.RowIndex);
                                getProductData();

                            }
                            else
                            {

                                if (int.TryParse(stock, out s) && double.TryParse(p, out pric))
                                {
                                    if (string.IsNullOrEmpty(productName))
                                    {
                                        productName = pName;
                                    }
                                }
                                else
                                {
                                    ErrorForm error2 = new ErrorForm("Price and Stock  must be integer");
                                    error2.Show();
                                }
                                bool check = UpdateProduct(productName, pName, description, pric, s);
                                if (check == true)
                                {
                                    infoForm infoForm = new infoForm("Updated Successfully!");
                                    infoForm.Show();

                                }
                                else
                                {
                                    infoForm infoForm = new infoForm("Updation Failed!");
                                    infoForm.Show();
                                }
                                productName = "";
                                getProductData();
                            }









                        }
                    }

                }
                imageUrl = "";
                getProductData();
            }
        }
        public bool deleteProduct(String productName)
        {
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "DELETE from product where NAME =: productname";

                using (OracleCommand command = new OracleCommand(updateQuery, connection))
                {
                    command.Parameters.Add("productname", OracleDbType.Varchar2).Value = productName;


                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {

                        return false;
                    }
                }
            }
        }

        private bool InsertProduct(string name, string description, int categoryTypeId, double price, int stock, string imagePath)
        {
            try
            { using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DECLARE l_bfile BFILE; l_blob BLOB; BEGIN " +
                                              $"l_bfile := BFILENAME('PRODUCTDIRECTORY', '{Path.GetFileName(imagePath)}');" +
                                              "INSERT INTO PRODUCT (NAME, DESCRIPTION, IMAGE, CATEGORYTYPEID, PRICE, Stock) " +
                                              "VALUES (:name, :description, EMPTY_BLOB(), :categoryTypeId, :price, :stock) " +
                                              "RETURNING IMAGE INTO l_blob; " +
                                              "DBMS_LOB.FILEOPEN(l_bfile, DBMS_LOB.FILE_READONLY); " +
                                              "DBMS_LOB.LOADFROMFILE(l_blob, l_bfile, DBMS_LOB.GETLENGTH(l_bfile)); " +
                                              "DBMS_LOB.FILECLOSE(l_bfile); " +
                                              "DBMS_OUTPUT.PUT_LINE('Size of the Image is: ' || DBMS_LOB.GETLENGTH(l_blob)); " +
                                              "COMMIT; EXCEPTION WHEN OTHERS THEN DBMS_LOB.FILECLOSE(l_bfile); RAISE; END;";

                        command.Parameters.Add("name", OracleDbType.Varchar2).Value = name;
                        command.Parameters.Add("description", OracleDbType.Varchar2).Value = description;
                        command.Parameters.Add("categoryTypeId", OracleDbType.Int32).Value = categoryTypeId;
                        command.Parameters.Add("price", OracleDbType.Double).Value = price;
                        command.Parameters.Add("stock", OracleDbType.Int32).Value = stock;

                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        private bool UpdateProduct(string bpname, string name, string description, double price, int stock)
        {
            int productId = getProductId(bpname);

            try
            {
               
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE PRODUCT " +
                        "SET NAME = :name, DESCRIPTION = :description, " +
                        "PRICE = " + price + ", STOCK = " + stock +
                        "WHERE PRODUCT_ID = " + productId;

                    using (OracleCommand command = new OracleCommand(updateQuery, connection))
                    {
                        //command.Parameters.Add("productId", OracleDbType.Int32).Value = productId;
                        command.Parameters.Add("name", OracleDbType.Varchar2).Value = name;
                        command.Parameters.Add("description", OracleDbType.Varchar2).Value = description;
                        // command.Parameters.Add("price", OracleDbType.Decimal).Value = Convert.ToDecimal(price);
                        // command.Parameters.Add("stock", OracleDbType.Int32).Value = stock;

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





        private void addProductBtn_Click(object sender, EventArgs e)
        {
            if (productAddRowCheck == true)
            {
                productAddRowCheck = false;
            }
            else
            {
                productAddRowCheck = true;
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView5);

                dataGridView5.Rows.Add(newRow);
                productAddRowIndex = dataGridView5.Rows.Count - 1;
            }
        }

        private void addLink_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select File";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "All Files|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
               
                imageURLTxt.Text = openFileDialog1.FileName;
                imageUrl=openFileDialog1.FileName;
                imageURLPanel.Visible = false;

                string fileName = Path.GetFileName(openFileDialog1.FileName);
                string destinationPath = Path.Combine(@"C:\Product", fileName);

                File.Copy(openFileDialog1.FileName, destinationPath, true);

            }
            else
            {
                imageURLTxt.Text = "You didn't select a file!";
                imageURLPanel.Visible = false;
            }


        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            imageUrl = "";
            imageURLPanel.Visible = false;
        }

        static void SaveFileToFolder(string sourceFilePath, string destinationFolderPath)
        {
            try
            {
                if (!File.Exists(sourceFilePath))
                {
                    ErrorForm error = new ErrorForm("Source file does not exist.");
                    error.ShowDialog();
                    Console.WriteLine("Source file does not exist.");
                    return;
                }

                if (!Directory.Exists(destinationFolderPath))
                {
                    Console.WriteLine("Destination folder does not exist.");
                    return;
                }

                string fileName = Path.GetFileName(sourceFilePath);

                // Combine the destination folder path and the file name to get the full destination path
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                // Copy the file to the destination folder
                File.Copy(sourceFilePath, destinationFilePath);
                infoForm infoForm = new infoForm("File saved to destination! ");
                infoForm.ShowDialog();

                Console.WriteLine($"File '{fileName}' saved to: {destinationFolderPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void roundBtn3_Click(object sender, EventArgs e)//orderpdf
        {
            List<int> list = new List<int> { 0,1,2,3,4,5 };
            DownloadPdf(dataGridView4, "Orders"+ordersi+1, list);
            ordersi++;
        }

        private void roundBtn2_Click_1(object sender, EventArgs e)//products pdf

        {
            List<int> list = new List<int> { 2, 3, 4, 5, 6, 7 };
            DownloadPdf(dataGridView5, "Products"+prodi+1, list);
            prodi++;

        }
        private void DownloadPdf(DataGridView dataGridView1, string fileName, List<int> columnIndices)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = fileName + ".pdf";
                bool errorMessage = false;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            errorMessage = true;
                            MessageBox.Show("Unable to write data to disk: " + ex.Message);
                        }
                    }

                    if (!errorMessage)
                    {
                        try
                        {
                            Document pdfDocument = new Document(PageSize.A4);

                            PdfWriter writer = PdfWriter.GetInstance(pdfDocument, new FileStream(save.FileName, FileMode.Create));

                            pdfDocument.Open();

                            PdfPTable pdfTable = new PdfPTable(columnIndices.Count);
                            foreach (int columnIndex in columnIndices)
                            {
                                PdfPCell headerCell = new PdfPCell(new Phrase(dataGridView1.Columns[columnIndex].HeaderText));
                                pdfTable.AddCell(headerCell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (int columnIndex in columnIndices)
                                {
                                    if (row.Cells[columnIndex].Value != null)
                                        pdfTable.AddCell(row.Cells[columnIndex].Value.ToString());
                                    else
                                        pdfTable.AddCell("N/A");
                                }
                            }

                            pdfDocument.Add(pdfTable);
                            pdfDocument.Close();

                            infoForm info = new infoForm("Data Exported Successfully!");
                            info.Show();
                            //MessageBox.Show("Data Exported Successfully", "Info");
                        }
                        catch (Exception ex)
                        {
                            infoForm info = new infoForm("Error While Exporting Data!");
                            info.Show();

                           // MessageBox.Show("Error while exporting Data: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                infoForm info = new infoForm("No Record Found!");
                info.Show();
              //  MessageBox.Show("No Records Found", "Info");
            }
        }

        private void getSaleData()
        {
            int index = 1;

            dataGridView6.Rows.Clear();
            dataGridView6.Columns[0].ReadOnly = true;
            dataGridView6.Columns[1].ReadOnly = true;
            dataGridView6.Columns[2].ReadOnly = true;
            dataGridView6.Columns[3].ReadOnly = true;
          
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT p.NAME, SUM(o.quantity) AS ProductSold, SUM(p.price * o.quantity) AS Revenue, p.stock FROM Product p JOIN Orderlineitems o ON o.productid = p.product_id GROUP BY p.NAME, p.price, p.stock";
                using (OracleCommand command = new OracleCommand(selectQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView6);
                            row.Height = 30;
                            row.Cells[0].Value = index.ToString();
                            row.Cells[1].Value = reader["NAME"].ToString();
                            row.Cells[2].Value = reader["stock"].ToString();
                            row.Cells[3].Value = reader["ProductSold"].ToString();
                            row.Cells[4].Value = reader["Revenue"].ToString();

                            dataGridView6.Rows.Add(row);
                            index++;
                        }
                    }
                }
            }
        }


        private void reportsBtn_Click(object sender, EventArgs e)
        {
            DashPanel.Visible = false;
            ListOrders.Visible = false;
            roundPanel2.Visible = false;
            imageURLPanel.Visible = false;
            manageAdmin.Visible = true;
            dashBoardPanel.Visible = true;
            userLogsPanel.Visible = true;
            CategoryPanel.Visible = true;
            TypePanel.Visible = true;
            OrderPanel.Visible = true;
            productPanel.Visible = true;
            Salepanel1.Visible = true;
            getSaleData();

        }

        private void roundBtn4_Click(object sender, EventArgs e)
        {

            downloadpdf(dataGridView6, "ProductRevenue" + salei+1);
            salei++;
        }

        private void roundPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void addAdmin_Click(object sender, EventArgs e)
        {
            if (adminAddRowCheck == true)
            {
                adminAddRowCheck = false;
            }
            else
            {
                adminAddRowCheck = true;
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView7);

                dataGridView7.Rows.Add(newRow);
                adminAddRowIndex = dataGridView7.Rows.Count - 2;
                
            }
        }

        private void mAdminPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


