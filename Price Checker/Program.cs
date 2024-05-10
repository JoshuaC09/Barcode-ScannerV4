using System;
using System.Windows.Forms;

namespace Price_Checker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create an instance of the main form
            mainForm mainFormInstance = new mainForm();

            // Run the application with the main form
            Application.Run(mainFormInstance);
        }
    }
}
