using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Vbe.Interop;
using Oracle.ManagedDataAccess.Client;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
namespace DbProject
{
    public partial class About_us_page : Form
    {
        OracleConnection connection;

        public About_us_page(String connectionString)
        {
            
           // String connectionString = @"DATA SOURCE =localhost:1521/XE;USER ID =dbfinal; PASSWORD=dbfinal";
            connection= new OracleConnection(connectionString);
            try
            {
                connection.Open();
              if(  !TableExists(connection, "CUSTOMER"))
                {
                    createCustomerTable(connection);
                  
                }
                if (!ObjectExists(connection, "CUSTOMER_SEQUENCE", "SEQUENCE"))
                {
                    createCustomerSequence(connection);

                }
                createCustomerTrigger(connection);
                if(!TableExists(connection,"ADMIN"))
                {
                    createAdminTable(connection);
                   
                }
                if (!ObjectExists(connection, "ADMIN_SEQUENCE", "SEQUENCE"))
                {
                    createAdminSequence(connection);
                }
                createAdminTrigger(connection);

                if (!TableExists(connection, "CATEGORY"))
                {
                    createCategoryTable(connection);
                   
                }
                if (!ObjectExists(connection, "CATEGORY_SEQUENCE", "SEQUENCE"))
                {
                    createCategorySequence(connection);
                }
                createCategoryTrigger(connection);

                if (!TableExists(connection, "USERLOGS"))
                {
                    createUserLogsTable(connection);
                    if (!ObjectExists(connection, "USERLOGS_SEQUENCE", "SEQUENCE"))
                    {
                        createUserLogsSequence(connection);
                    }
                }
                if (!ObjectExists(connection, "USERLOGS_SEQUENCE", "SEQUENCE"))
                {
                    createUserLogsSequence(connection);
                }
                createUserLogsTrigger(connection);

               if (!TableExists(connection, "TYPE"))
                {
                  createTypeTable(connection);
                   
                }
                if (!ObjectExists(connection, "TYPE_SEQUENCE", "SEQUENCE"))
                {
                    createTypeSequence(connection);
                }
                createTypeTrigger(connection);

                if (!TableExists(connection, "CATEGORYTYPE"))
                {
                    createCategoryTypeTable(connection);
                  
               }
                if (!ObjectExists(connection, "CATEGORYTYPE_SEQUENCE", "SEQUENCE"))
                {
                    createCategoryTypeSequence(connection);
                }
                createCategoryTypeTrigger(connection);
               if (!TableExists(connection, "PRODUCT"))
                {
                   createProductTable(connection);
                   
                }
                if (!ObjectExists(connection, "PRODUCT_SEQUENCE", "SEQUENCE"))
                {
                    createProductSequence(connection);
                }
                createProductTrigger(connection);

                if (!TableExists(connection, "ORDERS"))
                {
                    createOrderTable(connection);
                   
                }
                if (!ObjectExists(connection, "ORDERS_SEQUENCE", "SEQUENCE"))
                {
                    createOrderSequence(connection);
                }
                createOrderTrigger(connection);

                if (!TableExists(connection, "ORDERLINEITEMS"))
                {
                    createOrderLineTable(connection);
                    
                }
                if (!ObjectExists(connection, "ORDERLINE_SEQUENCE", "SEQUENCE"))
                {
                    createOrderLineSequence(connection);
                }
                createOrderLineTrigger(connection);

                // createDirectory(connection);
                string folderPath = @"C:\Product";

                CreateFolder(folderPath);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            InitializeComponent();
            contactUsPanel.Visible = false;
            reviewPanel.Visible = false;    
        }
        static void CreateFolder(string folderPath)
        {
            try
            {
                // Check if the folder already exists
                if (!Directory.Exists(folderPath))
                {
                    // Create the folder
                    Directory.CreateDirectory(folderPath);
                }
                else
                {
                    Console.WriteLine("Folder already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating folder: {ex.Message}");
            }
        }
        private void createDirectory(OracleConnection connection)
        {
            string createQuery = "CREATE DIRECTORY PRODUCTDIRECTORY AS 'C:\\Product'";
            using (OracleCommand command = new OracleCommand(createQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        private void createOrderLineTable(OracleConnection connection)
        {
            string createTableQuery = "CREATE TABLE OrderLineItems ( OrderLineItemID NUMBER PRIMARY KEY,   OrderID NUMBER,   ProductID NUMBER,   Quantity NUMBER,   FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,    FOREIGN KEY (ProductID) REFERENCES Product(Product_ID) ON DELETE CASCADE)";
            using (OracleCommand command = new OracleCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createOrderLineTrigger(OracleConnection connection)
        {
            string sql = "CREATE OR REPLACE TRIGGER OrderLineItems_Trigger BEFORE INSERT ON OrderLineItems FOR EACH ROW BEGIN SELECT OrderLine_Sequence.NEXTVAL INTO :new.OrderLineItemID FROM DUAL; END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createOrderLineSequence(OracleConnection connection)
        {
            string sql = "CREATE SEQUENCE OrderLine_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createOrderTrigger(OracleConnection connection)
        {
            string sql = "CREATE OR REPLACE TRIGGER Orders_Trigger BEFORE INSERT ON Orders FOR EACH ROW BEGIN SELECT Orders_Sequence.NEXTVAL INTO :new.OrderID FROM DUAL; END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createOrderSequence(OracleConnection connection)
        {
            string sql = "CREATE SEQUENCE Orders_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createOrderTable(OracleConnection connection)
        {
            string createTableQuery = "CREATE TABLE Orders (   OrderID INT PRIMARY KEY,   OrderDate DATE,   Status VARCHAR(255),    CustomerID INT,    FOREIGN KEY (CustomerID) REFERENCES Customer(ID))";

            using (OracleCommand command = new OracleCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createProductTrigger(OracleConnection connection)
        {
            string sql = "CREATE OR REPLACE TRIGGER Product_Trigger BEFORE INSERT ON PRODUCT FOR EACH ROW BEGIN SELECT Product_Sequence.NEXTVAL INTO :new.PRODUCT_ID FROM DUAL; END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createProductSequence(OracleConnection connection)
        {
            String sql = "CREATE SEQUENCE Product_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createTypeSequence(OracleConnection connection)
        {
            string sql = "CREATE SEQUENCE Type_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createUserLogsTable(OracleConnection connection)
        {
            string createTableQuery = "CREATE TABLE UserLogs (" +
                                          "Id INT PRIMARY KEY," +
                                          "LoginTimeStamp TIMESTAMP," +
                                          "LogoutTimeStamp TIMESTAMP," +
                                          "CustomerID INT," +
                                          "Activity VARCHAR2(255)," + 
                               "FOREIGN KEY (CustomerID) REFERENCES Customer(id)" +
                              ")";

                using (OracleCommand command = new OracleCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
        }
        private void createCategoryTypeTable(OracleConnection connection)
        {
            string createTableQuery = "CREATE TABLE CategoryType (ID NUMBER PRIMARY KEY,CategoryID NUMBER,TypeID NUMBER,FOREIGN KEY(CategoryID) REFERENCES Category(CATEGORY_ID),FOREIGN KEY(TypeID) REFERENCES Type(TYPE_ID))";

            using (OracleCommand command = new OracleCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createCategoryTypeSequence(OracleConnection connection)
        {
            string sql = "CREATE SEQUENCE CategoryType_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

        }
        private void createUserLogsSequence(OracleConnection connection)
        {
            string sql = "CREATE SEQUENCE UserLogs_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

        }
        private void createTypeTrigger(OracleConnection connection)
        {
            string sql = "CREATE OR REPLACE TRIGGER type_trigger BEFORE INSERT ON Type FOR EACH ROW BEGIN SELECT Type_Sequence.NEXTVAL INTO :new.Type_ID FROM DUAL; END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

        }

        private void createCategoryTypeTrigger(OracleConnection connection)
        {
            string sql = "CREATE OR REPLACE TRIGGER Categorytype_trigger BEFORE INSERT ON CategoryType FOR EACH ROW BEGIN SELECT CategoryType_Sequence.NEXTVAL INTO :new.ID FROM DUAL; END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

        }
        private void createUserLogsTrigger(OracleConnection connection)
        {
            string sql = "CREATE OR REPLACE TRIGGER userlogs_trigger BEFORE INSERT ON UserLogs FOR EACH ROW BEGIN SELECT UserLogs_Sequence.NEXTVAL INTO :new.Id FROM DUAL; END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

        }

        private void createCategoryTrigger(OracleConnection connection)
        {

            String sql = "create or replace TRIGGER category_trigger BEFORE INSERT ON CATEGORY FOR EACH ROW BEGIN    SELECT category_sequence.NEXTVAL INTO :new.CATEGORY_ID FROM DUAL; END;​";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createCategorySequence(OracleConnection connection)
        {
            String sql = "CREATE SEQUENCE Category_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void createCategoryTable(OracleConnection connection)
        {
            String sql = "CREATE TABLE Category ( category_id INT PRIMARY KEY, category_name VARCHAR(255) UNIQUE NOT NULL)";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
            

        }


        private void getStartedBtn_Click(object sender, EventArgs e)
        {
            SignInPage signin = new SignInPage();
            signin.Show();
            this.Hide();
        }

        private void About_us_page_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        static bool TableExists(OracleConnection connection, string tableName)
        {
            using (OracleCommand command = new OracleCommand($"SELECT COUNT(*) FROM user_tables WHERE table_name = '{tableName}'", connection))
            {
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
        public static void createCustomerTable(OracleConnection connection)
        {
            String sql = "CREATE TABLE Customer (id NUMBER PRIMARY KEY,name VARCHAR2(255) NOT NULL, email VARCHAR2(255) UNIQUE NOT NULL, password VARCHAR2(255) NOT NULL,address VARCHAR2(255), phone_number VARCHAR2(15))";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
           

        }
        public static void createProductTable(OracleConnection connection)
        {
            String sql = "CREATE TABLE  PRODUCT  (PRODUCT_ID NUMBER PRIMARY KEY,NAME VARCHAR2(255) NOT NULL, DESCRIPTION VARCHAR2(1000), IMAGE BLOB, PRICE NUMBER, CATEGORYTYPEID NUMBER NOT NULL ENABLE,STOCK NUMBER NOT NULL ENABLE,FOREIGN KEY (CATEGORYTYPEID) REFERENCES CATEGORYTYPE(ID) )";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }


        }
        static bool ObjectExists(OracleConnection connection, string objectName, string objectType)
        {
            string query = $"SELECT COUNT(*) FROM all_objects WHERE object_name = '{objectName}' AND object_type = '{objectType}'";
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
        public static void createCustomerSequence(OracleConnection connection)
        {
            String sql = "CREATE SEQUENCE Customer_Sequence START WITH 1 INCREMENT BY 1 NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void createCustomerTrigger(OracleConnection connection)
        {
            String sql = " CREATE OR REPLACE TRIGGER customer_trigger  BEFORE INSERT ON CUSTOMER FOR EACH ROW  BEGIN  SELECT customer_sequence.NEXTVAL INTO :new.id FROM DUAL;              END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void createAdminSequence(OracleConnection connection)
        {
            String sql2 = "CREATE SEQUENCE admin_sequence START WITH 1 INCREMENT BY 1  NOMAXVALUE NOCYCLE";
            using (OracleCommand command = new OracleCommand(sql2, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void createAdminTrigger(OracleConnection connection)
        {
            String sql = " CREATE OR REPLACE TRIGGER admin_trigger  BEFORE INSERT ON ADMIN FOR EACH ROW  BEGIN  SELECT admin_sequence.NEXTVAL INTO :new.id FROM DUAL;              END;";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void createAdminTable(OracleConnection connection)
        {
            String sql = "CREATE TABLE ADMIN (ID  NUMBER PRIMARY KEY, NAME VARCHAR2(255) NOT NULL ENABLE, EMAIL  VARCHAR2(255)  UNIQUE NOT NULL, PASSWORD VARCHAR2(255) NOT NULL ENABLE, AUTHORITY  VARCHAR2(1) NOT NULL  ) ";
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
           

        }
        public static void createTypeTable(OracleConnection connection)
        {
            String sql = "CREATE TABLE  Type    (Type_ID NUMBER, Type_NAME VARCHAR2(255) UNIQUE NOT NULL,  PRIMARY KEY (Type_ID) ENABLE )";
                  using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }


        }
        private void homeLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           contactUsPanel.Visible = false;  
            reviewPanel.Visible = false;

        }

        private void contactLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            contactUsPanel.Visible=true;
            reviewPanel.Visible = false;
        }

     

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void shopLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignInPage signin = new SignInPage();
            signin.Show();
            this.Hide();
        }

        private void reviewLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            contactUsPanel.Visible = false;
            reviewPanel.Visible = true;
            
            

        }
    }
}
