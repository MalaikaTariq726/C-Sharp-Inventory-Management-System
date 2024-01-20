using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using System.Net.Mail;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
namespace DbProject
{
    public partial class SignUp : Form
    {
        String connectionString = @"DATA SOURCE =localhost:1521/XE;USER ID =dbfinal; PASSWORD=dbfinal";
        int rndmNum;
        public SignUp()
        {
            InitializeComponent();
            nameTxt.SetTextAlignment(HorizontalAlignment.Left);
            addressTxt.SetTextAlignment(HorizontalAlignment.Left);
            emailTxt.SetTextAlignment(HorizontalAlignment.Left);
            phoneTxt.SetTextAlignment(HorizontalAlignment.Left);
            passtext.SetTextAlignment(HorizontalAlignment.Left);
            cnfrmTxt.SetTextAlignment(HorizontalAlignment.Left);
            otpPanel.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignInPage signIn = new SignInPage();
            signIn.Show();
            this.Hide();
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void signUpBtn_Click(object sender, EventArgs e)
        {
            String name=nameTxt.Texts;
            String email=emailTxt.Texts;
            String pass = passtext.Texts;
            String phone=phoneTxt.Texts;
            String address=addressTxt.Texts;
            String cnfrm=cnfrmTxt.Texts;
            
            if (String.IsNullOrEmpty(name)|| String.IsNullOrEmpty(email)||String.IsNullOrEmpty(pass)||String.IsNullOrEmpty(phone)||String.IsNullOrEmpty(address))
            {
                WarningForm warningForm = new WarningForm("Text Field Is Empty ! ");
                warningForm.ShowDialog();
            }
            else {
                Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                bool isValid = regex.IsMatch(emailTxt.Texts.Trim());
                if (isValid == true)
                {
                    if (long.TryParse(phone, out _))
                    {
                       if(IsStrongPassword(pass))
                        {
                            if(pass.Equals(cnfrm))
                            {
                                otpPanel.Visible = true;
                                linkLabel1.Visible = false;
                                signUpPanel.Visible = false;
                                sendotp(email);

                            }
                            else
                            {
                                ErrorForm objMessageBox = new ErrorForm("Password does not match");
                                objMessageBox.Show();
                            }
                        }
                        else
                        {

                            WarningForm warningForm = new WarningForm("Enter a Strong Password");
                            warningForm.ShowDialog();

                        }
                    }
                    else
                    {
                        ErrorForm objMessageBox = new ErrorForm("Phone No contain only Numbers!");
                        objMessageBox.Show();
                    }
                }
                else
                {
                    ErrorForm objMessageBox = new ErrorForm("Email Address is Invalid !");
                    objMessageBox.Show();
                }

            }
        }
        static bool IsStrongPassword(string password)
        {
           
            if (password.Length < 8)
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }
            if (!password.Any(char.IsLower))
            {
                return false;
            }
            if (!password.Any(char.IsDigit))
            {
                return false;
            }
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return false;
            }
            return true;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void otpPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private bool sendotp(String email) 
        {
            Random rnd = new Random();
            rndmNum = rnd.Next(1000, 10000); 
            string otpmail_body = "To complete your Sign Up process \n " +
                "Your One-Time Passcode (OTP) is " + rndmNum.ToString() + ".\n\nPlease do not share to anyone." +
                "\n\nThanks!\n\n\nSent through authorized source.\n\n";
            try 
            {
                using (MailMessage mail = new MailMessage("mamshoes089158@gmail.com", email, "Sign-Up One Time Passcode", otpmail_body))
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
                otpPanel.Visible = false;
                signUpPanel.Visible = true;
                ErrorForm errorForm = new ErrorForm("Incorrect Email ");
                errorForm.Show();
                return false;
            }
        }

        private void verifyBtn_Click(object sender, EventArgs e)
        {
            String name = nameTxt.Texts;
            String email = emailTxt.Texts;
            String pass = passtext.Texts;
            String phone = phoneTxt.Texts;
            String address = addressTxt.Texts;
            String cnfrm = cnfrmTxt.Texts;
            int otp = Int32.Parse(optTxt.Texts.Trim());
            
                if (otp == rndmNum)
                {

                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {

                        string insertQuery = "INSERT INTO Customer (name, email, password, address, phone_number) VALUES (:Name, :Email, :Password, :Address, :PhoneNumber)";

                        using (OracleCommand command = new OracleCommand(insertQuery, connection))
                        {
                            // Add parameters to the OracleCommand to prevent SQL injection
                            command.Parameters.Add("Name", OracleDbType.Varchar2).Value = name;
                            command.Parameters.Add("Email", OracleDbType.Varchar2).Value = email;
                            command.Parameters.Add("Password", OracleDbType.Varchar2).Value = pass;
                            command.Parameters.Add("Address", OracleDbType.Varchar2).Value = address;
                            command.Parameters.Add("PhoneNumber", OracleDbType.Varchar2).Value = phone;

                            try
                            {
                                connection.Open();
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    SignInPage signInPage = new SignInPage();
                                    this.Close();
                                signInPage.Show();


                                }
                                else
                                {
                                    ErrorForm objMessageBox = new ErrorForm("Account Creation Failed!");
                                    objMessageBox.Show();
                                    otpPanel.Visible = false;
                                    signUpPanel.Visible = true;
                                    linkLabel1.Visible = true;



                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorForm objMessageBox = new ErrorForm("Account Creation Failed!");
                                objMessageBox.Show();
                                otpPanel.Visible = false;
                                signUpPanel.Visible = true;
                                linkLabel1.Visible = true;
                            }
                        }
                    }
                }
            
        }

        private void resendemailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String email = emailTxt.Texts.Trim();
            sendotp(email);
        }

        private void nameTxt__TextChanged(object sender, EventArgs e)
        {
            nameTxt.SetTextAlignment(HorizontalAlignment.Left);
        }

        private void signUpPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void emailTxt__TextChanged(object sender, EventArgs e)
        {
            emailTxt.SetTextAlignment(HorizontalAlignment.Left);    
        }

        private void addressTxt__TextChanged(object sender, EventArgs e)
        {
            addressTxt.SetTextAlignment(HorizontalAlignment.Left);
        }

        private void phoneTxt__TextChanged(object sender, EventArgs e)
        {
            phoneTxt.SetTextAlignment(HorizontalAlignment.Left);        
        }

        private void passtext__TextChanged(object sender, EventArgs e)
        {
            passtext.SetTextAlignment(HorizontalAlignment.Left);
        }

        private void cnfrmTxt__TextChanged(object sender, EventArgs e)
        {
            cnfrmTxt.SetTextAlignment(HorizontalAlignment.Left);
        }
    }
}
