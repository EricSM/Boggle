using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    public partial class View : Form
    {
        public Model model;

        public View()
        {
            InitializeComponent();
            model = new Model();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model.JoinGame(textBoxPlayerName.Text, int.Parse(textBoxTime.Text), textBoxServer.Text);
        }
    }
}
