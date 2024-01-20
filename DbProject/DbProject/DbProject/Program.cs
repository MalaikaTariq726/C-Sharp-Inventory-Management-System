using DbProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbProject
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String connectionString = @"DATA SOURCE =localhost:1521/XE;USER ID =dbfinal; PASSWORD=dbfinal";
            About_us_page ab= new About_us_page(connectionString);
           //Admin obj=new Admin();
       // CustomerPage customerPage = new CustomerPage();
             Application.Run(ab);
            //Application.Run(obj);
        }
    }
}
