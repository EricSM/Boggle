namespace BoggleClient
{
    partial class View
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelPlayer2 = new System.Windows.Forms.Label();
            this.textBoxPlayer2Score = new System.Windows.Forms.TextBox();
            this.textBoxPlayer1Score = new System.Windows.Forms.TextBox();
            this.labelPlayer1 = new System.Windows.Forms.Label();
            this.textBoxWord = new System.Windows.Forms.TextBox();
            this.labelWord = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonJoinGame = new System.Windows.Forms.Button();
            this.Dice1 = new System.Windows.Forms.Label();
            this.Dice2 = new System.Windows.Forms.Label();
            this.Dice3 = new System.Windows.Forms.Label();
            this.Dice4 = new System.Windows.Forms.Label();
            this.Dice5 = new System.Windows.Forms.Label();
            this.Dice6 = new System.Windows.Forms.Label();
            this.Dice7 = new System.Windows.Forms.Label();
            this.Dice8 = new System.Windows.Forms.Label();
            this.Dice9 = new System.Windows.Forms.Label();
            this.Dice10 = new System.Windows.Forms.Label();
            this.Dice11 = new System.Windows.Forms.Label();
            this.Dice12 = new System.Windows.Forms.Label();
            this.Dice13 = new System.Windows.Forms.Label();
            this.Dice14 = new System.Windows.Forms.Label();
            this.Dice15 = new System.Windows.Forms.Label();
            this.Dice16 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonSubmit);
            this.panel1.Controls.Add(this.labelTime);
            this.panel1.Controls.Add(this.labelPlayer2);
            this.panel1.Controls.Add(this.textBoxPlayer2Score);
            this.panel1.Controls.Add(this.textBoxPlayer1Score);
            this.panel1.Controls.Add(this.labelPlayer1);
            this.panel1.Controls.Add(this.textBoxWord);
            this.panel1.Controls.Add(this.labelWord);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 57);
            this.panel1.TabIndex = 0;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Enabled = false;
            this.buttonSubmit.Location = new System.Drawing.Point(160, 18);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonSubmit.TabIndex = 7;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(605, 23);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(33, 13);
            this.labelTime.TabIndex = 6;
            this.labelTime.Text = "Timer";
            // 
            // labelPlayer2
            // 
            this.labelPlayer2.AutoSize = true;
            this.labelPlayer2.Location = new System.Drawing.Point(412, 23);
            this.labelPlayer2.Name = "labelPlayer2";
            this.labelPlayer2.Size = new System.Drawing.Size(45, 13);
            this.labelPlayer2.TabIndex = 5;
            this.labelPlayer2.Text = "Player 2";
            // 
            // textBoxPlayer2Score
            // 
            this.textBoxPlayer2Score.Location = new System.Drawing.Point(463, 20);
            this.textBoxPlayer2Score.Name = "textBoxPlayer2Score";
            this.textBoxPlayer2Score.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayer2Score.TabIndex = 4;
            // 
            // textBoxPlayer1Score
            // 
            this.textBoxPlayer1Score.Location = new System.Drawing.Point(292, 21);
            this.textBoxPlayer1Score.Name = "textBoxPlayer1Score";
            this.textBoxPlayer1Score.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayer1Score.TabIndex = 3;
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Location = new System.Drawing.Point(241, 23);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(45, 13);
            this.labelPlayer1.TabIndex = 2;
            this.labelPlayer1.Text = "Player 1";
            // 
            // textBoxWord
            // 
            this.textBoxWord.Location = new System.Drawing.Point(55, 21);
            this.textBoxWord.Name = "textBoxWord";
            this.textBoxWord.Size = new System.Drawing.Size(100, 20);
            this.textBoxWord.TabIndex = 1;
            // 
            // labelWord
            // 
            this.labelWord.AutoSize = true;
            this.labelWord.Location = new System.Drawing.Point(13, 24);
            this.labelWord.Name = "labelWord";
            this.labelWord.Size = new System.Drawing.Size(33, 13);
            this.labelWord.TabIndex = 0;
            this.labelWord.Text = "Word";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelStatus);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.textBoxTime);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.textBoxPlayerName);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.textBoxServer);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.buttonJoinGame);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(187, 405);
            this.panel2.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(57, 365);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(64, 13);
            this.labelStatus.TabIndex = 8;
            this.labelStatus.Text = "                   ";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(97, 267);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(60, 150);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxTime.TabIndex = 6;
            this.textBoxTime.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Time";
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Location = new System.Drawing.Point(60, 104);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayerName.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Name";
            // 
            // textBoxServer
            // 
            this.textBoxServer.Location = new System.Drawing.Point(60, 61);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(100, 20);
            this.textBoxServer.TabIndex = 2;
            this.textBoxServer.Text = "http://bogglecs3500s16.azurewebsites.net/BoggleService.svc/";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Server";
            // 
            // buttonJoinGame
            // 
            this.buttonJoinGame.Location = new System.Drawing.Point(28, 268);
            this.buttonJoinGame.Name = "buttonJoinGame";
            this.buttonJoinGame.Size = new System.Drawing.Size(62, 23);
            this.buttonJoinGame.TabIndex = 0;
            this.buttonJoinGame.Text = "Join";
            this.buttonJoinGame.UseVisualStyleBackColor = true;
            this.buttonJoinGame.Click += new System.EventHandler(this.buttonJoinGame_Click);
            // 
            // Dice1
            // 
            this.Dice1.AutoSize = true;
            this.Dice1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice1.Location = new System.Drawing.Point(255, 134);
            this.Dice1.Name = "Dice1";
            this.Dice1.Size = new System.Drawing.Size(23, 31);
            this.Dice1.TabIndex = 2;
            this.Dice1.Text = "-";
            // 
            // Dice2
            // 
            this.Dice2.AutoSize = true;
            this.Dice2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice2.Location = new System.Drawing.Point(360, 134);
            this.Dice2.Name = "Dice2";
            this.Dice2.Size = new System.Drawing.Size(23, 31);
            this.Dice2.TabIndex = 3;
            this.Dice2.Text = "-";
            // 
            // Dice3
            // 
            this.Dice3.AutoSize = true;
            this.Dice3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice3.Location = new System.Drawing.Point(458, 134);
            this.Dice3.Name = "Dice3";
            this.Dice3.Size = new System.Drawing.Size(23, 31);
            this.Dice3.TabIndex = 4;
            this.Dice3.Text = "-";
            // 
            // Dice4
            // 
            this.Dice4.AutoSize = true;
            this.Dice4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice4.Location = new System.Drawing.Point(558, 134);
            this.Dice4.Name = "Dice4";
            this.Dice4.Size = new System.Drawing.Size(23, 31);
            this.Dice4.TabIndex = 5;
            this.Dice4.Text = "-";
            // 
            // Dice5
            // 
            this.Dice5.AutoSize = true;
            this.Dice5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice5.Location = new System.Drawing.Point(255, 208);
            this.Dice5.Name = "Dice5";
            this.Dice5.Size = new System.Drawing.Size(23, 31);
            this.Dice5.TabIndex = 6;
            this.Dice5.Text = "-";
            // 
            // Dice6
            // 
            this.Dice6.AutoSize = true;
            this.Dice6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice6.Location = new System.Drawing.Point(360, 208);
            this.Dice6.Name = "Dice6";
            this.Dice6.Size = new System.Drawing.Size(23, 31);
            this.Dice6.TabIndex = 7;
            this.Dice6.Text = "-";
            // 
            // Dice7
            // 
            this.Dice7.AutoSize = true;
            this.Dice7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice7.Location = new System.Drawing.Point(458, 208);
            this.Dice7.Name = "Dice7";
            this.Dice7.Size = new System.Drawing.Size(23, 31);
            this.Dice7.TabIndex = 8;
            this.Dice7.Text = "-";
            // 
            // Dice8
            // 
            this.Dice8.AutoSize = true;
            this.Dice8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice8.Location = new System.Drawing.Point(558, 208);
            this.Dice8.Name = "Dice8";
            this.Dice8.Size = new System.Drawing.Size(23, 31);
            this.Dice8.TabIndex = 9;
            this.Dice8.Text = "-";
            // 
            // Dice9
            // 
            this.Dice9.AutoSize = true;
            this.Dice9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice9.Location = new System.Drawing.Point(255, 279);
            this.Dice9.Name = "Dice9";
            this.Dice9.Size = new System.Drawing.Size(23, 31);
            this.Dice9.TabIndex = 10;
            this.Dice9.Text = "-";
            // 
            // Dice10
            // 
            this.Dice10.AutoSize = true;
            this.Dice10.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice10.Location = new System.Drawing.Point(360, 279);
            this.Dice10.Name = "Dice10";
            this.Dice10.Size = new System.Drawing.Size(23, 31);
            this.Dice10.TabIndex = 11;
            this.Dice10.Text = "-";
            // 
            // Dice11
            // 
            this.Dice11.AutoSize = true;
            this.Dice11.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice11.Location = new System.Drawing.Point(458, 279);
            this.Dice11.Name = "Dice11";
            this.Dice11.Size = new System.Drawing.Size(23, 31);
            this.Dice11.TabIndex = 12;
            this.Dice11.Text = "-";
            // 
            // Dice12
            // 
            this.Dice12.AutoSize = true;
            this.Dice12.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice12.Location = new System.Drawing.Point(558, 279);
            this.Dice12.Name = "Dice12";
            this.Dice12.Size = new System.Drawing.Size(23, 31);
            this.Dice12.TabIndex = 13;
            this.Dice12.Text = "-";
            // 
            // Dice13
            // 
            this.Dice13.AutoSize = true;
            this.Dice13.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice13.Location = new System.Drawing.Point(255, 350);
            this.Dice13.Name = "Dice13";
            this.Dice13.Size = new System.Drawing.Size(23, 31);
            this.Dice13.TabIndex = 14;
            this.Dice13.Text = "-";
            // 
            // Dice14
            // 
            this.Dice14.AutoSize = true;
            this.Dice14.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice14.Location = new System.Drawing.Point(360, 350);
            this.Dice14.Name = "Dice14";
            this.Dice14.Size = new System.Drawing.Size(23, 31);
            this.Dice14.TabIndex = 15;
            this.Dice14.Text = "-";
            // 
            // Dice15
            // 
            this.Dice15.AutoSize = true;
            this.Dice15.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice15.Location = new System.Drawing.Point(458, 350);
            this.Dice15.Name = "Dice15";
            this.Dice15.Size = new System.Drawing.Size(23, 31);
            this.Dice15.TabIndex = 16;
            this.Dice15.Text = "-";
            // 
            // Dice16
            // 
            this.Dice16.AutoSize = true;
            this.Dice16.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dice16.Location = new System.Drawing.Point(558, 350);
            this.Dice16.Name = "Dice16";
            this.Dice16.Size = new System.Drawing.Size(23, 31);
            this.Dice16.TabIndex = 17;
            this.Dice16.Text = "-";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.dataGridView2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Location = new System.Drawing.Point(193, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(479, 393);
            this.panel3.TabIndex = 18;
            this.panel3.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(320, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Player2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(76, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Player1";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(244, 71);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(230, 316);
            this.dataGridView2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(168, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Game Results";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 71);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(230, 316);
            this.dataGridView1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(185, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tutorialToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tutorialToolStripMenuItem
            // 
            this.tutorialToolStripMenuItem.Name = "tutorialToolStripMenuItem";
            this.tutorialToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tutorialToolStripMenuItem.Text = "Tutorial";
            this.tutorialToolStripMenuItem.Click += new System.EventHandler(this.tutorialToolStripMenuItem_Click);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.Dice16);
            this.Controls.Add(this.Dice15);
            this.Controls.Add(this.Dice14);
            this.Controls.Add(this.Dice13);
            this.Controls.Add(this.Dice12);
            this.Controls.Add(this.Dice11);
            this.Controls.Add(this.Dice10);
            this.Controls.Add(this.Dice9);
            this.Controls.Add(this.Dice8);
            this.Controls.Add(this.Dice7);
            this.Controls.Add(this.Dice6);
            this.Controls.Add(this.Dice5);
            this.Controls.Add(this.Dice4);
            this.Controls.Add(this.Dice3);
            this.Controls.Add(this.Dice2);
            this.Controls.Add(this.Dice1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "View";
            this.Text = "Boggle";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWord;
        private System.Windows.Forms.TextBox textBoxWord;
        private System.Windows.Forms.Label labelPlayer1;
        private System.Windows.Forms.TextBox textBoxPlayer1Score;
        private System.Windows.Forms.TextBox textBoxPlayer2Score;
        private System.Windows.Forms.Label labelPlayer2;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonJoinGame;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.Label Dice1;
        private System.Windows.Forms.Label Dice2;
        private System.Windows.Forms.Label Dice3;
        private System.Windows.Forms.Label Dice4;
        private System.Windows.Forms.Label Dice5;
        private System.Windows.Forms.Label Dice6;
        private System.Windows.Forms.Label Dice7;
        private System.Windows.Forms.Label Dice8;
        private System.Windows.Forms.Label Dice9;
        private System.Windows.Forms.Label Dice10;
        private System.Windows.Forms.Label Dice11;
        private System.Windows.Forms.Label Dice12;
        private System.Windows.Forms.Label Dice13;
        private System.Windows.Forms.Label Dice14;
        private System.Windows.Forms.Label Dice15;
        private System.Windows.Forms.Label Dice16;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tutorialToolStripMenuItem;
    }
}

