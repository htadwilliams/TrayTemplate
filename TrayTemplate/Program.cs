using System;
using System.Windows.Forms;

namespace TrayTemplate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationContext applicationContext = new TrayApplicationContext<TrayTemplateMainForm>(); 
            Application.Run(applicationContext);
        }
    }
}
