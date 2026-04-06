using System;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Forms;

namespace Takeaway_Restaurant_Management_System
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}