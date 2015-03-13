namespace BoggleClient
{
    partial class BoggleClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoggleClientForm));
            this.label1 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.ipAddressBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.ipLabel = new System.Windows.Forms.Label();
            this.usernameLable = new System.Windows.Forms.Label();
            this.processLabel = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.boggleBoardControl = new BoggleClient.BoggleBoardControl();
            this.timeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.opponentScoreLabel = new System.Windows.Forms.Label();
            this.opponentNameLabel = new System.Windows.Forms.Label();
            this.myScoreLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.wordEnterButton = new System.Windows.Forms.Button();
            this.wordListBox = new System.Windows.Forms.ListBox();
            this.boggleTextBox = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.opponentWordTable = new System.Windows.Forms.DataGridView();
            this.myWordTable = new System.Windows.Forms.DataGridView();
            this.opponentsEndScore = new System.Windows.Forms.Label();
            this.opponentsEndWords = new System.Windows.Forms.Label();
            this.myEndWords = new System.Windows.Forms.Label();
            this.myEndScore = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.opponentEndLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.winLoseLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.recordLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opponentWordTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myWordTable)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Impact", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-8, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Boggle";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(304, 50);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ipAddressBox
            // 
            this.ipAddressBox.Location = new System.Drawing.Point(169, 24);
            this.ipAddressBox.Name = "ipAddressBox";
            this.ipAddressBox.Size = new System.Drawing.Size(100, 20);
            this.ipAddressBox.TabIndex = 2;
            this.ipAddressBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ipAddressBox_KeyPress);
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(279, 24);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(100, 20);
            this.usernameBox.TabIndex = 3;
            this.usernameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.usernameBox_KeyDown);
            this.usernameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.usernameBox_KeyPress);
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(166, 8);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(61, 13);
            this.ipLabel.TabIndex = 4;
            this.ipLabel.Text = "IP Address:";
            // 
            // usernameLable
            // 
            this.usernameLable.AutoSize = true;
            this.usernameLable.Location = new System.Drawing.Point(276, 8);
            this.usernameLable.Name = "usernameLable";
            this.usernameLable.Size = new System.Drawing.Size(58, 13);
            this.usernameLable.TabIndex = 5;
            this.usernameLable.Text = "Username:";
            // 
            // processLabel
            // 
            this.processLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.processLabel.Font = new System.Drawing.Font("Impact", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processLabel.Location = new System.Drawing.Point(16, 114);
            this.processLabel.Name = "processLabel";
            this.processLabel.Size = new System.Drawing.Size(341, 34);
            this.processLabel.TabIndex = 6;
            this.processLabel.Text = "Enter Connection Information";
            this.processLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Location = new System.Drawing.Point(2, 79);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(377, 284);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.processLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(369, 258);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connecting";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.boggleBoardControl);
            this.tabPage2.Controls.Add(this.timeLabel);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.opponentScoreLabel);
            this.tabPage2.Controls.Add(this.opponentNameLabel);
            this.tabPage2.Controls.Add(this.myScoreLabel);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.wordEnterButton);
            this.tabPage2.Controls.Add(this.wordListBox);
            this.tabPage2.Controls.Add(this.boggleTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(369, 258);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Playing";
            // 
            // boggleBoardControl
            // 
            this.boggleBoardControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boggleBoardControl.Location = new System.Drawing.Point(25, 46);
            this.boggleBoardControl.Name = "boggleBoardControl";
            this.boggleBoardControl.Size = new System.Drawing.Size(148, 148);
            this.boggleBoardControl.TabIndex = 19;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Font = new System.Drawing.Font("Impact", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLabel.Location = new System.Drawing.Point(70, 3);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(76, 43);
            this.timeLabel.TabIndex = 18;
            this.timeLabel.Text = "999";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Time:";
            // 
            // opponentScoreLabel
            // 
            this.opponentScoreLabel.AutoSize = true;
            this.opponentScoreLabel.Font = new System.Drawing.Font("Impact", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opponentScoreLabel.Location = new System.Drawing.Point(307, 3);
            this.opponentScoreLabel.Name = "opponentScoreLabel";
            this.opponentScoreLabel.Size = new System.Drawing.Size(52, 29);
            this.opponentScoreLabel.TabIndex = 16;
            this.opponentScoreLabel.Text = "999";
            this.opponentScoreLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // opponentNameLabel
            // 
            this.opponentNameLabel.AutoSize = true;
            this.opponentNameLabel.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opponentNameLabel.Location = new System.Drawing.Point(166, 0);
            this.opponentNameLabel.Name = "opponentNameLabel";
            this.opponentNameLabel.Size = new System.Drawing.Size(126, 20);
            this.opponentNameLabel.TabIndex = 15;
            this.opponentNameLabel.Text = "Opponent\'s Score:";
            this.opponentNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // myScoreLabel
            // 
            this.myScoreLabel.AutoSize = true;
            this.myScoreLabel.Font = new System.Drawing.Font("Impact", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myScoreLabel.Location = new System.Drawing.Point(298, 32);
            this.myScoreLabel.Name = "myScoreLabel";
            this.myScoreLabel.Size = new System.Drawing.Size(68, 39);
            this.myScoreLabel.TabIndex = 14;
            this.myScoreLabel.Text = "999";
            this.myScoreLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(221, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "My Score:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // wordEnterButton
            // 
            this.wordEnterButton.Location = new System.Drawing.Point(123, 210);
            this.wordEnterButton.Name = "wordEnterButton";
            this.wordEnterButton.Size = new System.Drawing.Size(50, 23);
            this.wordEnterButton.TabIndex = 12;
            this.wordEnterButton.Text = "Enter";
            this.wordEnterButton.UseVisualStyleBackColor = true;
            this.wordEnterButton.Click += new System.EventHandler(this.wordEnterButton_Click);
            // 
            // wordListBox
            // 
            this.wordListBox.ColumnWidth = 68;
            this.wordListBox.FormattingEnabled = true;
            this.wordListBox.Location = new System.Drawing.Point(200, 73);
            this.wordListBox.MultiColumn = true;
            this.wordListBox.Name = "wordListBox";
            this.wordListBox.Size = new System.Drawing.Size(159, 160);
            this.wordListBox.TabIndex = 11;
            // 
            // boggleTextBox
            // 
            this.boggleTextBox.Location = new System.Drawing.Point(25, 213);
            this.boggleTextBox.Name = "boggleTextBox";
            this.boggleTextBox.Size = new System.Drawing.Size(92, 20);
            this.boggleTextBox.TabIndex = 10;
            this.boggleTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.boggleTextBox_KeyPress);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.opponentWordTable);
            this.tabPage3.Controls.Add(this.myWordTable);
            this.tabPage3.Controls.Add(this.opponentsEndScore);
            this.tabPage3.Controls.Add(this.opponentsEndWords);
            this.tabPage3.Controls.Add(this.myEndWords);
            this.tabPage3.Controls.Add(this.myEndScore);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.recordLabel);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.opponentEndLabel);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.winLoseLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(369, 258);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Game Over";
            // 
            // opponentWordTable
            // 
            this.opponentWordTable.AllowUserToAddRows = false;
            this.opponentWordTable.AllowUserToDeleteRows = false;
            this.opponentWordTable.AllowUserToResizeRows = false;
            this.opponentWordTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.opponentWordTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.opponentWordTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.opponentWordTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.opponentWordTable.GridColor = System.Drawing.SystemColors.Control;
            this.opponentWordTable.Location = new System.Drawing.Point(205, 114);
            this.opponentWordTable.MultiSelect = false;
            this.opponentWordTable.Name = "opponentWordTable";
            this.opponentWordTable.RowHeadersVisible = false;
            this.opponentWordTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.opponentWordTable.ShowEditingIcon = false;
            this.opponentWordTable.Size = new System.Drawing.Size(164, 141);
            this.opponentWordTable.TabIndex = 0;
            // 
            // myWordTable
            // 
            this.myWordTable.AllowUserToAddRows = false;
            this.myWordTable.AllowUserToDeleteRows = false;
            this.myWordTable.AllowUserToResizeRows = false;
            this.myWordTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.myWordTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.myWordTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.myWordTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.myWordTable.GridColor = System.Drawing.SystemColors.Control;
            this.myWordTable.Location = new System.Drawing.Point(6, 114);
            this.myWordTable.MultiSelect = false;
            this.myWordTable.Name = "myWordTable";
            this.myWordTable.RowHeadersVisible = false;
            this.myWordTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.myWordTable.ShowEditingIcon = false;
            this.myWordTable.Size = new System.Drawing.Size(164, 141);
            this.myWordTable.TabIndex = 0;
            // 
            // opponentsEndScore
            // 
            this.opponentsEndScore.AutoSize = true;
            this.opponentsEndScore.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opponentsEndScore.Location = new System.Drawing.Point(255, 60);
            this.opponentsEndScore.Name = "opponentsEndScore";
            this.opponentsEndScore.Size = new System.Drawing.Size(45, 26);
            this.opponentsEndScore.TabIndex = 17;
            this.opponentsEndScore.Text = "999";
            // 
            // opponentsEndWords
            // 
            this.opponentsEndWords.AutoSize = true;
            this.opponentsEndWords.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opponentsEndWords.Location = new System.Drawing.Point(269, 91);
            this.opponentsEndWords.Name = "opponentsEndWords";
            this.opponentsEndWords.Size = new System.Drawing.Size(36, 20);
            this.opponentsEndWords.TabIndex = 17;
            this.opponentsEndWords.Text = "999";
            // 
            // myEndWords
            // 
            this.myEndWords.AutoSize = true;
            this.myEndWords.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myEndWords.Location = new System.Drawing.Point(75, 91);
            this.myEndWords.Name = "myEndWords";
            this.myEndWords.Size = new System.Drawing.Size(36, 20);
            this.myEndWords.TabIndex = 17;
            this.myEndWords.Text = "999";
            // 
            // myEndScore
            // 
            this.myEndScore.AutoSize = true;
            this.myEndScore.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myEndScore.Location = new System.Drawing.Point(63, 60);
            this.myEndScore.Name = "myEndScore";
            this.myEndScore.Size = new System.Drawing.Size(45, 26);
            this.myEndScore.TabIndex = 17;
            this.myEndScore.Text = "999";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(218, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 20);
            this.label9.TabIndex = 14;
            this.label9.Text = "Words:";
            // 
            // opponentEndLabel
            // 
            this.opponentEndLabel.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opponentEndLabel.Location = new System.Drawing.Point(205, 40);
            this.opponentEndLabel.Name = "opponentEndLabel";
            this.opponentEndLabel.Size = new System.Drawing.Size(164, 20);
            this.opponentEndLabel.TabIndex = 14;
            this.opponentEndLabel.Text = "Opponent\'s Score:";
            this.opponentEndLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Words:";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "My Score:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // winLoseLabel
            // 
            this.winLoseLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.winLoseLabel.Font = new System.Drawing.Font("Impact", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.winLoseLabel.Location = new System.Drawing.Point(0, 0);
            this.winLoseLabel.Name = "winLoseLabel";
            this.winLoseLabel.Size = new System.Drawing.Size(369, 36);
            this.winLoseLabel.TabIndex = 7;
            this.winLoseLabel.Text = "Winner/Loser";
            this.winLoseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(156, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Record";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recordLabel
            // 
            this.recordLabel.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordLabel.Location = new System.Drawing.Point(156, 80);
            this.recordLabel.Name = "recordLabel";
            this.recordLabel.Size = new System.Drawing.Size(62, 20);
            this.recordLabel.TabIndex = 14;
            this.recordLabel.Text = "xxx/xxx";
            this.recordLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // BoggleClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 362);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.usernameLable);
            this.Controls.Add(this.ipAddressBox);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.usernameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BoggleClientForm";
            this.Text = "Boggle Champions 2015";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opponentWordTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myWordTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox ipAddressBox;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.Label usernameLable;
        private System.Windows.Forms.Label processLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button wordEnterButton;
        private System.Windows.Forms.ListBox wordListBox;
        private System.Windows.Forms.TextBox boggleTextBox;
        private System.Windows.Forms.Label opponentScoreLabel;
        private System.Windows.Forms.Label opponentNameLabel;
        private System.Windows.Forms.Label myScoreLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label label2;
        private BoggleBoardControl boggleBoardControl;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView myWordTable;
        private System.Windows.Forms.Label opponentsEndScore;
        private System.Windows.Forms.Label opponentsEndWords;
        private System.Windows.Forms.Label myEndWords;
        private System.Windows.Forms.Label myEndScore;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label opponentEndLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label winLoseLabel;
        private System.Windows.Forms.DataGridView opponentWordTable;
        private System.Windows.Forms.Label recordLabel;
        private System.Windows.Forms.Label label4;
    }
}

