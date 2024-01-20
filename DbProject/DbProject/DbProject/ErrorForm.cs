using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbProject
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(String lbl1)
        {
            
            InitializeComponent();
            label2.Text = lbl1;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void OKbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void EmailInvalidForm_Load(object sender, EventArgs e)
        {

        }
    }
}
