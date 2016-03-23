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

        private void buttonJoinGame_Click(object sender, EventArgs e)
        {            
            Task task = new Task(() => 
            {
                model.CreateUser(textBoxPlayerName.Text, textBoxServer.Text);
                model.JoinGame(int.Parse(textBoxTime.Text), textBoxServer.Text);
            });

            task.Start();

            buttonJoinGame.Enabled = false;
            buttonCancel.Enabled = true;
            textBoxServer.ReadOnly = textBoxPlayerName.ReadOnly = textBoxTime.ReadOnly = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;

            model.CancelJoinRequest(textBoxServer.Text);

        }
    }
}
