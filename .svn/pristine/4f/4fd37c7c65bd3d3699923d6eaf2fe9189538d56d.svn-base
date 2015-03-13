// Elliot Hatch, u0790511
// Samuel Davidson, u0835059
// December, 2014
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BoggleClient
{
    public partial class BoggleClientForm : Form
    {

        /// <summary>
        /// Flag used to indicate to user if the client is attempting to connect to a server
        /// </summary>
        private bool connecting = false;

        /// <summary>
        /// The MVC Model of the Boggle Client
        /// </summary>
        private BoogleClient gameClient;

        /// <summary>
        /// Data table storing the opponent's words at the end of the game
        /// </summary>
        private DataTable opponentWords;

        /// <summary>
        /// Data table storing the player's words at the end of the game
        /// </summary>
        private DataTable myWords;

        /// <summary>
        /// Color of invalid rows in the post-game words table
        /// </summary>
        private Color invalidWordColor = Color.FromArgb(255, 166, 166);

        /// <summary>
        /// Color of words both players entered in the post-game words table
        /// </summary>
        private Color commonWordColor = Color.FromArgb(255, 255, 166);

        /// <summary>
        /// the player's score
        /// </summary>
        private int myScore;

        /// <summary>
        /// the opponent's score
        /// </summary>
        private int opponentScore;

        /// <summary>
        /// Name of player used for MySQL query and labels
        /// </summary>
        private string playerName;

        /// <summary>
        /// Name of opponent used for MySQL query and labels
        /// </summary>
        private string opponentName;

        /// <summary>
        /// The MySQL connection string.
        /// </summary>
        public const string MySQLconnectionString =
            "server=atr.eng.utah.edu;database=cs3500_ellioth;uid=cs3500_ellioth;password=444892533";

        /// <summary>
        /// Construct the form with a client
        /// </summary>
        /// <param name="client"></param>
        public BoggleClientForm(BoogleClient client)
        {
            InitializeComponent();

            gameClient = client;
            gameClient.connectingCallback = OnConnect;
            gameClient.gameStartCallback = OnGameStart;
            gameClient.disconnectedCallback = OnDisconnect;
            gameClient.timerCallback = OnTimer;
            gameClient.scoreCallback = OnScore;
            gameClient.gameFinishedCallback = OnGameFinished;

            tabControl.Appearance = TabAppearance.Buttons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.Multiline = true;
            tabControl.SizeMode = TabSizeMode.Fixed;
        }


        /// <summary>
        /// Callback function for when the client connects to a server
        /// </summary>
        /// <param name="e"></param>
        private void OnConnect(Exception e)
        {
            if (e == null)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    processLabel.Text = "Waiting for opponent...";
                });

            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    enableConnectGUI();
                    MessageBox.Show("Failed to connect: " + e.Message);
                });
            }
        }

        /// <summary>
        /// Callback function for when the game begins
        /// </summary>
        /// <param name="board"></param>
        /// <param name="time"></param>
        /// <param name="opponentName"></param>
        private void OnGameStart(string board, int time, string _opponentName)
        {
            this.Invoke((MethodInvoker)delegate
            {
                tabControl.SelectTab(1);
                boggleTextBox.Focus();
                processLabel.Text = "Playing...";
                connectButton.Text = "Disconnect";

                boggleBoardControl.startGame(board);

                myScore = 0;
                opponentScore = 0;
                myScoreLabel.Text = "0";
                opponentScoreLabel.Text = "0";
                opponentNameLabel.Text = _opponentName + "'s Score:";
                opponentName = _opponentName;
                timeLabel.Text = "" + time;
                wordListBox.Items.Clear();
            });
        }

        /// <summary>
        /// Callback function for when the timer ticks down
        /// </summary>
        /// <param name="time"></param>
        private void OnTimer(int time)
        {
            this.Invoke((MethodInvoker)delegate
            {
                timeLabel.Text = "" + time;
            });
        }

        /// <summary>
        /// Callback function for when the score is changed
        /// </summary>
        /// <param name="_myScore"></param>
        /// <param name="_opponentScore"></param>
        private void OnScore(int _myScore, int _opponentScore)
        {
            this.Invoke((MethodInvoker)delegate
            {
                myScore = _myScore;
                opponentScore = _opponentScore;
                myScoreLabel.Text = "" + myScore;
                opponentScoreLabel.Text = "" + opponentScore;
            });
        }

        /// <summary>
        /// Callback function for when the game completes
        /// </summary>
        /// <param name="myLegalWords"></param>
        /// <param name="opponentLegalWords"></param>
        /// <param name="commonLegalWords"></param>
        /// <param name="myIllegalWords"></param>
        /// <param name="opponentIllegalWords"></param>
        public void OnGameFinished(ISet<string> myLegalWords, ISet<string> opponentLegalWords,
            ISet<string> commonLegalWords, ISet<string> myIllegalWords, ISet<string> opponentIllegalWords)
        {
            this.Invoke((MethodInvoker)delegate
            {
                tabControl.SelectTab(2);
                enableConnectGUI();
                myEndScore.Text = myScoreLabel.Text;
                opponentsEndScore.Text = opponentScoreLabel.Text;
                opponentEndLabel.Text = opponentNameLabel.Text;
                myEndWords.Text = (myLegalWords.Count + myIllegalWords.Count + commonLegalWords.Count).ToString();
                opponentsEndWords.Text =
                    (opponentLegalWords.Count + opponentIllegalWords.Count + commonLegalWords.Count).ToString();
                if (myScore > opponentScore)
                    winLoseLabel.Text = "You Win!";
                else if (myScore < opponentScore)
                    winLoseLabel.Text = "You Lose :(";
                else
                    winLoseLabel.Text = "You Tied";

                recordLabel.Text = "";

                myWords = new DataTable("My Words");
                DataColumn column;
                //Word Column;
                column = new DataColumn();
                column.DataType = typeof(string);
                column.ColumnName = "Word";
                column.AutoIncrement = false;
                column.Unique = true;
                column.ReadOnly = true;
                myWords.Columns.Add(column);
                //Valid Column;
                column = new DataColumn();
                column.DataType = typeof(bool);
                column.ColumnName = "Valid";
                column.AutoIncrement = false;
                column.Unique = false;
                column.ReadOnly = true;
                myWords.Columns.Add(column);
                //Common Column;
                column = new DataColumn();
                column.DataType = typeof(bool);
                column.ColumnName = "Common";
                column.AutoIncrement = false;
                column.Unique = false;
                column.ReadOnly = true;
                myWords.Columns.Add(column);

                //Add words
                foreach (string word in myLegalWords)
                {
                    DataRow row = myWords.NewRow();
                    row["Word"] = word;
                    row["Valid"] = true;
                    row["Common"] = false;
                    myWords.Rows.Add(row);
                }
                foreach (string word in myIllegalWords)
                {
                    DataRow row = myWords.NewRow();
                    row["Word"] = word;
                    row["Valid"] = false;
                    row["Common"] = false;
                    myWords.Rows.Add(row);
                }
                foreach (string word in commonLegalWords)
                {
                    DataRow row = myWords.NewRow();
                    row["Word"] = word;
                    row["Valid"] = true;
                    row["Common"] = true;
                    myWords.Rows.Add(row);
                }

                myWordTable.DataSource = myWords;

                myWordTable.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                myWordTable.Columns[1].Visible = false;
                myWordTable.Columns[2].Visible = false;

                myWordTable.Sort(myWordTable.Columns[0], ListSortDirection.Ascending);
                myWordTable.ClearSelection();

                foreach (DataGridViewRow row in myWordTable.Rows)
                {
                    if ((bool)row.Cells[1].Value == false) //if invalid
                        row.DefaultCellStyle.BackColor = invalidWordColor;
                    else if ((bool)row.Cells[2].Value == true) //if common
                        row.DefaultCellStyle.BackColor = commonWordColor;
                }

                opponentWords = new DataTable("Opponents Words");
                //Word Column;
                column = new DataColumn();
                column.DataType = typeof(string);
                column.ColumnName = "Word";
                column.AutoIncrement = false;
                column.Unique = true;
                column.ReadOnly = true;
                opponentWords.Columns.Add(column);
                //Valid Column;
                column = new DataColumn();
                column.DataType = typeof(bool);
                column.ColumnName = "Valid";
                column.AutoIncrement = false;
                column.Unique = false;
                column.ReadOnly = true;
                opponentWords.Columns.Add(column);
                //Common Column;
                column = new DataColumn();
                column.DataType = typeof(bool);
                column.ColumnName = "Common";
                column.AutoIncrement = false;
                column.Unique = false;
                column.ReadOnly = true;
                opponentWords.Columns.Add(column);

                //Add words
                foreach (string word in opponentLegalWords)
                {
                    DataRow row = opponentWords.NewRow();
                    row["Word"] = word;
                    row["Valid"] = true;
                    row["Common"] = false;
                    opponentWords.Rows.Add(row);
                }
                foreach (string word in opponentIllegalWords)
                {
                    DataRow row = opponentWords.NewRow();
                    row["Word"] = word;
                    row["Valid"] = false;
                    row["Common"] = false;
                    opponentWords.Rows.Add(row);
                }
                foreach (string word in commonLegalWords)
                {
                    DataRow row = opponentWords.NewRow();
                    row["Word"] = word;
                    row["Valid"] = true;
                    row["Common"] = true;
                    opponentWords.Rows.Add(row);
                }

                opponentWordTable.DataSource = opponentWords;

                opponentWordTable.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                opponentWordTable.Columns[1].Visible = false;
                opponentWordTable.Columns[2].Visible = false;

                opponentWordTable.Sort(opponentWordTable.Columns[0], ListSortDirection.Ascending);
                opponentWordTable.ClearSelection();

                foreach (DataGridViewRow row in opponentWordTable.Rows)
                {
                    if ((bool)row.Cells[1].Value == false) //if invalid
                        row.DefaultCellStyle.BackColor = invalidWordColor;
                    else if ((bool)row.Cells[2].Value == true) //if common
                        row.DefaultCellStyle.BackColor = commonWordColor;
                }
                Task.Factory.StartNew(getRecord);
            });
        }

        /// <summary>
        /// Callback function for when the user is disconnected
        /// </summary>
        /// <param name="message"></param>
        private void OnDisconnect(string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                enableConnectGUI();
                if (tabControl.SelectedIndex == 1)
                {
                    MessageBox.Show(message);
                    tabControl.SelectTab(0);
                }

            });
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (!connecting)
            {
                //Check if the username and ip are valid.
                if (usernameBox.Text.Length == 0)
                {
                    MessageBox.Show("Please enter a username.");
                    return;
                }
                if (ipAddressBox.Text.Length == 0)
                {
                    MessageBox.Show("Please enter an IP address.");
                    return;
                }
                //Start Connecting
                //connectButton.Enabled = false;
                usernameBox.Enabled = false;
                ipAddressBox.Enabled = false;
                processLabel.Text = "Connecting...";
                connectButton.Text = "Cancel";
                connecting = true;
                gameClient.Connect(ipAddressBox.Text, usernameBox.Text);
                tabControl.SelectTab(0);
                playerName = usernameBox.Text;
            }
            else
            {
                gameClient.Disconnect();
                enableConnectGUI();
                tabControl.SelectTab(0);

            }


        }

        private void enableConnectGUI()
        {
            usernameBox.Enabled = true;
            ipAddressBox.Enabled = true;
            connecting = false;
            processLabel.Text = "Enter Connection Information";
            connectButton.Text = "Connect";
        }

        private void usernameBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ipAddressBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    if (!connectButton.Enabled)
                        return;
                    connectButton_Click(null, null);
                    e.Handled = true; // Removes "ding" sound
                    break;

            }
        }

        private void usernameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    if (!connectButton.Enabled)
                        return;
                    connectButton_Click(null, null);
                    e.Handled = true; // Removes "ding" sound
                    break;

            }
        }

        private void wordEnterButton_Click(object sender, EventArgs e)
        {
            gameClient.SendWord(boggleTextBox.Text);
            wordListBox.Items.Add((wordListBox.Items.Count + 1) + ": " + boggleTextBox.Text);
            boggleTextBox.Text = "";
        }

        private void boggleTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    if (!wordEnterButton.Enabled)
                        return;
                    wordEnterButton_Click(null, null);
                    e.Handled = true; // Removes "ding" sound
                    break;

            }
        }

        /// <summary>
        /// Updates the record label in the score screen. In its own thread so that it can sleep and not race the server for update.
        /// </summary>
        private void getRecord()
        {
            string labelText = "";
            Thread.Sleep(600);
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySQLconnectionString))
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = @"SELECT score," +
                                          "(SELECT score FROM Players_Games T1 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId AND T1.playerId = (SELECT id FROM Players P1 WHERE P1.name = '" +
                                          opponentName + "')) AS 'opponentScore'" +
                                          "FROM Players INNER JOIN Players_Games T2 ON Players.id=T2.playerId INNER JOIN Games ON Games.id=T2.gameId WHERE name='" +
                                          playerName + "'";
                    // Execute the command and cycle through the DataReader object
                    MySqlDataReader sqlData = command.ExecuteReader();
                    int playerRecord = 0;
                    int opponentRecord = 0;
                    while (sqlData.Read())
                    {
                        int playerScr = 0;
                        int opponentScr = 0;
                        if (Int32.TryParse(sqlData["score"].ToString(), out playerScr))
                        {
                            if (Int32.TryParse(sqlData["opponentScore"].ToString(), out opponentScr))
                            {
                                if (playerScr > opponentScr)
                                {
                                    playerRecord++;
                                }
                                else if (playerScr < opponentScr)
                                {
                                    opponentRecord++;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    conn.Close();

                    labelText = playerRecord + "/" + opponentRecord;



                }
            }

            catch (Exception e)
            {
                //We blew it
                labelText = "ERROR";
            }
            this.Invoke((MethodInvoker)delegate
            {
                recordLabel.Text = labelText;
            });
        }
    }
}
