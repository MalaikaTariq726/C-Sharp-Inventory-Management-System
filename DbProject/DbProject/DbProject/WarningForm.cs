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
    public partial class WarningForm : Form
    {
        public WarningForm(String msg)
        {
            InitializeComponent();
            label2.Text = msg;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
           
            this.Close();

        }

        private void WarningForm_Load(object sender, EventArgs e)
        {

        }
    }
}
