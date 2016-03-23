using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS8
{
    public partial class ViewForm : Form, Interface
    {
        //Declare events
        public event Action<string> TestEvent;

        //Example string definition
        public string Title
        {
            set { Text = value; }

        }


        public ViewForm()
        {
            InitializeComponent();
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            //Test loading a function from controller
            TestEvent("Test!");
        }
    }
}
