namespace DbProject
{
    partial class SignInPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignInPage));
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.welc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.roundPanel1 = new DbProject.Resources.RoundPanel();
            this.captchaTxt = new DbProject.Resources.RoundTxtBox();
            this.PassTxt = new DbProject.Resources.RoundTxtBox();
            this.emailTxt = new DbProject.Resources.RoundTxtBox();
            this.checkBoxPAss = new System.Windows.Forms.CheckBox();
            this.adminRBtn = new System.Windows.Forms.RadioButton();
            this.SignInBtn1 = new DbProject.Resources.RoundBtn();
            this.siginLbl = new System.Windows.Forms.Label();
            this.UserRBtn = new System.Windows.Forms.RadioButton();
            this.cptLbl = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.logo = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.roundPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(261, 518);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(139, 16);
            this.linkLabel1.TabIndex = 15;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Create a New Account";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // welc
            // 
            this.welc.AutoSize = true;
            this.welc.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F);
            this.welc.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.welc.Location = new System.Drawing.Point(12, 244);
            this.welc.Name = "welc";
            this.welc.Size = new System.Drawing.Size(416, 54);
            this.welc.TabIndex = 16;
            this.welc.Text = "Welcome to M.A.M";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label2.Location = new System.Drawing.Point(239, 305);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 18);
            this.label2.TabIndex = 17;
            this.label2.Text = "Sign in to continue access";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.roundPanel1);
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Location = new System.Drawing.Point(454, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(650, 600);
            this.panel1.TabIndex = 19;
            // 
            // roundPanel1
            // 
            this.roundPanel1.BackColor = System.Drawing.Color.Gray;
            this.roundPanel1.BorderRadius = 30;
            this.roundPanel1.Controls.Add(this.captchaTxt);
            this.roundPanel1.Controls.Add(this.PassTxt);
            this.roundPanel1.Controls.Add(this.emailTxt);
            this.roundPanel1.Controls.Add(this.checkBoxPAss);
            this.roundPanel1.Controls.Add(this.adminRBtn);
            this.roundPanel1.Controls.Add(this.SignInBtn1);
            this.roundPanel1.Controls.Add(this.siginLbl);
            this.roundPanel1.Controls.Add(this.UserRBtn);
            this.roundPanel1.Controls.Add(this.cptLbl);
            this.roundPanel1.ForeColor = System.Drawing.Color.White;
            this.roundPanel1.GradientAngle = 90F;
            this.roundPanel1.GradientBottomColor = System.Drawing.Color.DimGray;
            this.roundPanel1.GradientTopColor = System.Drawing.Color.DarkGray;
            this.roundPanel1.Location = new System.Drawing.Point(144, 86);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Size = new System.Drawing.Size(376, 429);
            this.roundPanel1.TabIndex = 16;
            // 
            // captchaTxt
            // 
            this.captchaTxt.BackColor = System.Drawing.Color.DarkGray;
            this.captchaTxt.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.captchaTxt.BorderFocusColor = System.Drawing.Color.DarkGray;
            this.captchaTxt.BorderRadius = 15;
            this.captchaTxt.BorderSize = 2;
            this.captchaTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captchaTxt.ForeColor = System.Drawing.Color.White;
            this.captchaTxt.IsPasswordChar = false;
            this.captchaTxt.IsPlaceHolder = true;
            this.captchaTxt.Location = new System.Drawing.Point(25, 261);
            this.captchaTxt.Multiline = false;
            this.captchaTxt.Name = "captchaTxt";
            this.captchaTxt.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.captchaTxt.PasswordChar = false;
            this.captchaTxt.PlaceholderText = "Captcha";
            this.captchaTxt.PlaceolderColor = System.Drawing.Color.DimGray;
            this.captchaTxt.Size = new System.Drawing.Size(318, 35);
            this.captchaTxt.TabIndex = 23;
            this.captchaTxt.Texts = "";
            this.captchaTxt.UnderLinedStyle = false;
            this.captchaTxt._TextChanged += new System.EventHandler(this.captchaTxt__TextChanged);
            // 
            // PassTxt
            // 
            this.PassTxt.BackColor = System.Drawing.Color.DarkGray;
            this.PassTxt.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.PassTxt.BorderFocusColor = System.Drawing.Color.DarkGray;
            this.PassTxt.BorderRadius = 15;
            this.PassTxt.BorderSize = 2;
            this.PassTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassTxt.IsPasswordChar = true;
            this.PassTxt.IsPlaceHolder = true;
            this.PassTxt.Location = new System.Drawing.Point(25, 121);
            this.PassTxt.Multiline = false;
            this.PassTxt.Name = "PassTxt";
            this.PassTxt.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.PassTxt.PasswordChar = true;
            this.PassTxt.PlaceholderText = "Password";
            this.PassTxt.PlaceolderColor = System.Drawing.Color.DimGray;
            this.PassTxt.Size = new System.Drawing.Size(318, 35);
            this.PassTxt.TabIndex = 22;
            this.PassTxt.Texts = "";
            this.PassTxt.UnderLinedStyle = false;
            this.PassTxt._TextChanged += new System.EventHandler(this.PassTxt__TextChanged);
            // 
            // emailTxt
            // 
            this.emailTxt.BackColor = System.Drawing.Color.DarkGray;
            this.emailTxt.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.emailTxt.BorderFocusColor = System.Drawing.Color.DarkGray;
            this.emailTxt.BorderRadius = 15;
            this.emailTxt.BorderSize = 2;
            this.emailTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailTxt.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.emailTxt.IsPasswordChar = false;
            this.emailTxt.IsPlaceHolder = true;
            this.emailTxt.Location = new System.Drawing.Point(25, 80);
            this.emailTxt.Multiline = false;
            this.emailTxt.Name = "emailTxt";
            this.emailTxt.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.emailTxt.PasswordChar = false;
            this.emailTxt.PlaceholderText = "EmailAddress";
            this.emailTxt.PlaceolderColor = System.Drawing.Color.DimGray;
            this.emailTxt.Size = new System.Drawing.Size(318, 35);
            this.emailTxt.TabIndex = 21;
            this.emailTxt.Texts = "";
            this.emailTxt.UnderLinedStyle = false;
            this.emailTxt._TextChanged += new System.EventHandler(this.emailTxt__TextChanged);
            // 
            // checkBoxPAss
            // 
            this.checkBoxPAss.AutoSize = true;
            this.checkBoxPAss.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxPAss.Location = new System.Drawing.Point(233, 171);
            this.checkBoxPAss.Name = "checkBoxPAss";
            this.checkBoxPAss.Size = new System.Drawing.Size(125, 20);
            this.checkBoxPAss.TabIndex = 20;
            this.checkBoxPAss.Text = "Show Password";
            this.checkBoxPAss.UseVisualStyleBackColor = false;
            this.checkBoxPAss.CheckedChanged += new System.EventHandler(this.checkBoxPAss_CheckedChanged);
            // 
            // adminRBtn
            // 
            this.adminRBtn.AutoSize = true;
            this.adminRBtn.BackColor = System.Drawing.Color.Transparent;
            this.adminRBtn.Font = new System.Drawing.Font("Arial Narrow", 7.2F);
            this.adminRBtn.ForeColor = System.Drawing.Color.White;
            this.adminRBtn.Location = new System.Drawing.Point(78, 325);
            this.adminRBtn.Name = "adminRBtn";
            this.adminRBtn.Size = new System.Drawing.Size(55, 20);
            this.adminRBtn.TabIndex = 7;
            this.adminRBtn.TabStop = true;
            this.adminRBtn.Text = "Admin";
            this.adminRBtn.UseVisualStyleBackColor = false;
            // 
            // SignInBtn1
            // 
            this.SignInBtn1.BackColor = System.Drawing.Color.Black;
            this.SignInBtn1.FlatAppearance.BorderSize = 0;
            this.SignInBtn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignInBtn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.26957F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignInBtn1.ForeColor = System.Drawing.Color.White;
            this.SignInBtn1.Location = new System.Drawing.Point(106, 365);
            this.SignInBtn1.Name = "SignInBtn1";
            this.SignInBtn1.Size = new System.Drawing.Size(150, 40);
            this.SignInBtn1.TabIndex = 19;
            this.SignInBtn1.Text = "Sign In";
            this.SignInBtn1.UseVisualStyleBackColor = false;
            this.SignInBtn1.Click += new System.EventHandler(this.SignInBtn1_Click);
            // 
            // siginLbl
            // 
            this.siginLbl.AutoSize = true;
            this.siginLbl.BackColor = System.Drawing.Color.Transparent;
            this.siginLbl.Font = new System.Drawing.Font("Tahoma", 16.27826F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siginLbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.siginLbl.Location = new System.Drawing.Point(122, 17);
            this.siginLbl.Name = "siginLbl";
            this.siginLbl.Size = new System.Drawing.Size(117, 34);
            this.siginLbl.TabIndex = 4;
            this.siginLbl.Text = "Sign In";
            // 
            // UserRBtn
            // 
            this.UserRBtn.AutoSize = true;
            this.UserRBtn.BackColor = System.Drawing.Color.Transparent;
            this.UserRBtn.Font = new System.Drawing.Font("Arial Narrow", 7.2F);
            this.UserRBtn.ForeColor = System.Drawing.Color.White;
            this.UserRBtn.Location = new System.Drawing.Point(259, 325);
            this.UserRBtn.Name = "UserRBtn";
            this.UserRBtn.Size = new System.Drawing.Size(49, 20);
            this.UserRBtn.TabIndex = 6;
            this.UserRBtn.TabStop = true;
            this.UserRBtn.Text = "User";
            this.UserRBtn.UseVisualStyleBackColor = false;
            // 
            // cptLbl
            // 
            this.cptLbl.AutoSize = true;
            this.cptLbl.BackColor = System.Drawing.Color.Transparent;
            this.cptLbl.Font = new System.Drawing.Font("Lucida Calligraphy", 18.15652F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cptLbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cptLbl.Location = new System.Drawing.Point(99, 206);
            this.cptLbl.Name = "cptLbl";
            this.cptLbl.Size = new System.Drawing.Size(204, 40);
            this.cptLbl.TabIndex = 13;
            this.cptLbl.Text = "CAPTCHA";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DbProject.Properties.Resources.design2;
            this.pictureBox2.Location = new System.Drawing.Point(177, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(271, 238);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 21;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DbProject.Properties.Resources.design1;
            this.pictureBox1.Location = new System.Drawing.Point(3, 346);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 253);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // logo
            // 
            this.logo.Image = ((System.Drawing.Image)(resources.GetObject("logo.Image")));
            this.logo.Location = new System.Drawing.Point(-35, -1);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(242, 201);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 0;
            this.logo.TabStop = false;
            // 
            // SignInPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1103, 595);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.welc);
            this.Controls.Add(this.logo);
            this.Name = "SignInPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign In";
            this.Load += new System.EventHandler(this.SignInPage_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.roundPanel1.ResumeLayout(false);
            this.roundPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label welc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Resources.RoundPanel roundPanel1;
        private Resources.RoundTxtBox captchaTxt;
        private Resources.RoundTxtBox PassTxt;
        private Resources.RoundTxtBox emailTxt;
        private System.Windows.Forms.CheckBox checkBoxPAss;
        private System.Windows.Forms.RadioButton adminRBtn;
        private Resources.RoundBtn SignInBtn1;
        private System.Windows.Forms.Label siginLbl;
        private System.Windows.Forms.RadioButton UserRBtn;
        private System.Windows.Forms.Label cptLbl;
    }
}