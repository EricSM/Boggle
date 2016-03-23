using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS8
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

            //Initialize the ViewManager
            var context = ViewManager.GetContext();
            //Run a single instance of ViewForm
            ViewManager.GetContext().RunNew();
            //Run the program
            Application.Run(context);
        }
    }
}
