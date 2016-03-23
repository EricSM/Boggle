using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS8
{
    class ViewManager : ApplicationContext
    {
        // Singleton ApplicationContext
        private static ViewManager context;
        private ViewManager()
        {
        }

        public static ViewManager GetContext()
        {
            if (context == null)
            {
                context = new ViewManager();
            }
            return context;
        }

        /// <summary>
        /// Runs a form in this application context
        /// </summary>
        public void RunNew()
        {
            // Create the window and the controller
            ViewForm window = new ViewForm();
            new Controller(window);

            // Run the form
            window.Show();

        }
    }
}
