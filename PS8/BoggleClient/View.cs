//Meysam Hamel & Eric Miramontes
//PS8

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    public partial class View : Form
    {
        public Model model;

        private int switchcolors = 0;

        public View()
        {
            InitializeComponent();
            model = new Model();
            
        }

        // User tries to join a game
        private void buttonJoinGame_Click(object sender, EventArgs e)
        {            
            // Communicate with servers on different thread.
            Task task = new Task(() => 
            {
                model.CreateUser(textBoxPlayerName.Text, textBoxServer.Text);
                model.JoinGame(int.Parse(textBoxTime.Text), textBoxServer.Text);           
            });

            task.Start();
            timer1.Enabled = true;

            // Disable textboxes and join button
            buttonJoinGame.Enabled = false;
            buttonCancel.Enabled = true;
            buttonSubmit.Enabled = true;
            textBoxServer.ReadOnly = textBoxPlayerName.ReadOnly = textBoxTime.ReadOnly = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //check if the game is not pending you cant cancel

            model.CancelJoinRequest(textBoxServer.Text);

           // timer1.Enabled = false;
            if (model.GameState == "")
            {
                this.Close();
            }

            // Disable and enable relevant buttons, textboxes, etc.
            buttonCancel.Text = "Cancel";
            panel3.Visible = false;
            timer1.Enabled = false;
            buttonCancel.Enabled = false;
            buttonJoinGame.Enabled = true;
            buttonSubmit.Enabled = false;
            textBoxServer.ReadOnly = textBoxPlayerName.ReadOnly = textBoxTime.ReadOnly = false;
        }
        //create button for submit 
        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            model.PlayWordRequest(textBoxWord.Text,  textBoxServer.Text);
            textBoxWord.Text = "";
            textBoxWord.Focus();
            timer1.Enabled = true;
        }
        // Remove period
        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); 
            return path;
        }
        
        private void endgame()
        {
            this.Close();
        }

        private static readonly Random rand = new Random();

        // To color the dice :)
        private Color GetRandomColour()
        {
            return Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Get game status
            model.GameStatus(false, textBoxServer.Text);
            
            // Update scores, timer, etc.
            labelStatus.Text = model.GameState;
            labelTime.Text = model.TimeLeft.ToString();
            labelPlayer1.Text = model.Player1;
            labelPlayer2.Text = model.Player2;
            label2.Text = model.Player1;
            label3.Text = model.Player2;

            if (model.TimeLeft == 0)// When game is over
            {
                model.GameStatus(false, textBoxServer.Text);
                Console.WriteLine("Times up");
                timer1.Enabled = false;
                buttonCancel.Text = "New Game";

                
                // Display words used by both players the the scores of each word.
                
                panel3.Visible = true;

                dataGridView1.ColumnCount = 2;
                dataGridView1.Columns[0].Name = "Word Played";
                dataGridView1.Columns[1].Name = "Score";
                dataGridView2.ColumnCount = 2;
                dataGridView2.Columns[0].Name = "Word Played";
                dataGridView2.Columns[1].Name = "Score";

                foreach (var playedword in model.Player1WordsPlayed)
                {

                    string[] row = new string[] { playedword.Word, playedword.Score };
                    dataGridView1.Rows.Add(row);

                    Console.WriteLine(playedword.Word);
                    
                }

                foreach (var playedword in model.Player2WordsPlayed)
                {
                    string[] row = new string[] { playedword.Word, playedword.Score };
                    dataGridView2.Rows.Add(row);
                    Console.WriteLine(playedword.Word);
                }

            }
            try // update scores and color the board
            {
                textBoxPlayer1Score.Text = model.Player1Score.ToString();
                textBoxPlayer2Score.Text = model.Player2Score.ToString();

                if (model.Board.Length > 0)
                {
                    if(switchcolors == 0)
                    {
                        Dice1.ForeColor = GetRandomColour();
                        Dice2.ForeColor = GetRandomColour();
                        Dice3.ForeColor = GetRandomColour();
                        Dice4.ForeColor = GetRandomColour();
                        Dice5.ForeColor = GetRandomColour();
                        Dice6.ForeColor = GetRandomColour();
                        Dice7.ForeColor = GetRandomColour();
                        Dice8.ForeColor = GetRandomColour();
                        Dice9.ForeColor = GetRandomColour();
                        Dice10.ForeColor = GetRandomColour();
                        Dice11.ForeColor = GetRandomColour();
                        Dice12.ForeColor = GetRandomColour();
                        Dice13.ForeColor = GetRandomColour();
                        Dice14.ForeColor = GetRandomColour();
                        Dice15.ForeColor = GetRandomColour();
                        Dice16.ForeColor = GetRandomColour();
                        switchcolors = 1;
                    }

                    Dice1.Text = model.Board[0].ToString();
                    Dice2.Text = model.Board[1].ToString();
                    Dice3.Text = model.Board[2].ToString();
                    Dice4.Text = model.Board[3].ToString();
                    Dice5.Text = model.Board[4].ToString();
                    Dice6.Text = model.Board[5].ToString();
                    Dice7.Text = model.Board[6].ToString();
                    Dice8.Text = model.Board[7].ToString();
                    Dice9.Text = model.Board[8].ToString();
                    Dice10.Text = model.Board[9].ToString();
                    Dice11.Text = model.Board[10].ToString();
                    Dice12.Text = model.Board[11].ToString();
                    Dice13.Text = model.Board[12].ToString();
                    Dice14.Text = model.Board[13].ToString();
                    Dice15.Text = model.Board[14].ToString();
                    Dice16.Text = model.Board[15].ToString();

                }
            }
            catch
            {

            }
        }

        private void tutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To start a new game, enter the boggle server, your gamer-tag, and desired time\n" +
                            "into the respective textboxes on the left, and press Join Game.  To exit a game\n" + 
                            "or cancel a pending game, press Cancel.  To submit a word, enter it into the box\n" +
                            "marked \"Word\".  When your game is over, to play a new game, press \"New Game\"");
        }
    }
}
