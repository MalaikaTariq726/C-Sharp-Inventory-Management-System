using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
namespace DbProject
{
    public partial class SignInPage : Form
    {
        string Captcha = "";
        string authorities;
        public string getAuthority()
        {
            return this.authorities;
        }
        public SignInPage()
        {
            InitializeComponent();
            emailTxt.SetTextAlignment(HorizontalAlignment.Left);
            PassTxt.SetTextAlignment(HorizontalAlignment.Left);
            captchaTxt.SetTextAlignment(HorizontalAlignment.Left);  

        }

        private void SignInPage_Load(object sender, EventArgs e)
        {
            setCaptchTxt();
        }
        public void setCaptchTxt()
        {
            cptLbl.Text = "";
            Captcha = "";
            Random rand = new Random();
            int num = rand.Next(4,7);
            int total = 0;
            do
            {
                int ch = rand.Next(48, 132);
                if ((ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122))
                {
                    Captcha = Captcha + (char)ch;
                    total++;
                    if (total == num)
                        break;
                }

            } while (true);
            
            cptLbl.Text = Captcha;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Close();
        }



        public void findAuthority(string userName, string password)
        {
            string connectionString = @"DATA SOURCE=localhost:1521/XE;USER ID=dbfinal;PASSWORD=dbfinal";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT Authority FROM ADMIN WHERE email = :username AND password = :password";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        // Add parameters before executing the command
                        command.Parameters.Add("username", OracleDbType.Varchar2).Value = userName;
                        command.Parameters.Add("password", OracleDbType.Varchar2).Value = password;

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                authorities = reader["Authority"].ToString();
                            }
                            else
                            {
                                // Handle the case when no records are found
                                authorities = "No authority found for the given credentials";
                            }
                        }
                    }
                }
                catch (OracleException ex)
                {
                    ErrorForm errorForm = new ErrorForm(ex.Message);
                    errorForm.Show();
                }
                catch (Exception ex)
                {
                    

                    ErrorForm errorForm = new ErrorForm(ex.Message);
                    errorForm.Show();
                }
            }
        }



        private void SignInBtn1_Click(object sender, EventArgs e)
        {
            if (adminRBtn.Checked)
            {

                String username = emailTxt.Texts.Trim();
                String password = PassTxt.Texts.Trim();
                String capt = captchaTxt.Texts.Trim();
                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(capt))
                {

                    WarningForm objemptyform = new WarningForm("Text Field Is Empty ! ");
                    objemptyform.Show();
                }
                else
                {
                    if (capt == Captcha)
                    {
                        String connectionString = @"DATA SOURCE =localhost:1521/XE;USER ID =dbfinal; PASSWORD=dbfinal";
                        using (OracleConnection connection = new OracleConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();

                                string query = $"SELECT COUNT(*) FROM ADMIN WHERE email = :username AND password = :password";

                                using (OracleCommand command = new OracleCommand(query, connection))
                                {
                                    command.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                                    command.Parameters.Add("password", OracleDbType.Varchar2).Value = password;

                                    int count = Convert.ToInt32(command.ExecuteScalar());

                                    if (count > 0)
                                    {
                                        findAuthority(username, password);
                                        this.Hide();
                                        Admin admin = new Admin(this);
                                        admin.Show();

                                    }
                                    else
                                    {
                                        setCaptchTxt();
                                        ErrorForm errorForm = new ErrorForm("Invalid username or password!");
                                        errorForm.Show();


                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                setCaptchTxt();
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        setCaptchTxt();

                        ErrorForm errorForm = new ErrorForm("Incorrect Re-Captcha !");
                        errorForm.Show();

                    }
                }




            }
            else if (UserRBtn.Checked)
            {


                String username = emailTxt.Texts.Trim();
                String password = PassTxt.Texts.Trim();
                String capt = captchaTxt.Texts.Trim();
                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(capt))
                {
                    setCaptchTxt();
                    WarningForm objemptyform = new WarningForm("Text Field Is Empty ! ");
                    objemptyform.Show();
                    setCaptchTxt();
                }
                else
                {
                    if (capt == Captcha)
                    {
                        String connectionString = @"DATA SOURCE =localhost:1521/XE;USER ID =dbfinal; PASSWORD=dbfinal";
                        using (OracleConnection connection = new OracleConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();

                                string query = $"SELECT COUNT(*) FROM CUSTOMER WHERE email = :username AND password = :password";

                                using (OracleCommand command = new OracleCommand(query, connection))
                                {
                                    command.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                                    command.Parameters.Add("password", OracleDbType.Varchar2).Value = password;

                                    int count = Convert.ToInt32(command.ExecuteScalar());

                                    if (count > 0)
                                    {
                                        this.Hide();
                                        DateTime currentTimestamp = DateTime.Now;
                                        CustomerPage customerPage = new CustomerPage(username,currentTimestamp);
                                        customerPage.Show();

                                    }
                                    else
                                    {
                                        setCaptchTxt();
                                        ErrorForm errorForm = new ErrorForm("Invalid username or password!");
                                        errorForm.Show();


                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                setCaptchTxt();
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        setCaptchTxt();

                        ErrorForm errorForm = new ErrorForm("Incorrect Re-Captcha !");
                        errorForm.Show();
                        setCaptchTxt();
                    }
                }

            }
        }

        private void emailTxt__TextChanged(object sender, EventArgs e)
        {
            emailTxt.SetTextAlignment(HorizontalAlignment.Left);
            
        }

        private void PassTxt__TextChanged(object sender, EventArgs e)
        {
            PassTxt.SetTextAlignment(HorizontalAlignment.Left);
        }

        private void captchaTxt__TextChanged(object sender, EventArgs e)
        {
            captchaTxt.SetTextAlignment(HorizontalAlignment.Left);
        }

        private void checkBoxPAss_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPAss.Checked == true)
            {
                PassTxt.PasswordChar = false;
                PassTxt.IsPasswordChar = false;

            }
            else
            {
                PassTxt.PasswordChar = true;
                PassTxt.IsPasswordChar = true;
            }
        }
    }
}

