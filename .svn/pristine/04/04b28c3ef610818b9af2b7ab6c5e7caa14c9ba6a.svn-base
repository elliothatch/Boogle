using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    public partial class BoggleBoardControl : UserControl
    {
        Label[] letters;
        public BoggleBoardControl()
        {
            InitializeComponent();
            letters = new Label[16];
            for(int row = 0; row < 4; row++)
            {
                for(int col = 0; col < 4; col++)
                {
                    Label letter = new Label();
                    letters[col + row * 4] = letter;
                    letter.Font = new Font("Impact", 16, FontStyle.Regular);
                    letter.Location = new Point(this.Size.Width / 4 * col, this.Size.Height / 4 * row);
                    letter.Size = new Size(this.Size.Width / 4, this.Size.Height / 4);
                    letter.AutoSize = false;
                    letter.TextAlign = ContentAlignment.MiddleCenter;
                    letter.Text = "W";
                    this.Controls.Add(letter);
                }
            }
        }

        public void startGame(string board)
        {
            for(int i = 0; i < letters.Length; i++)
            {
                string str  = "" + board[i];
                if (str == "Q")
                {
                    str += "u";
                }
                letters[i].Text = "" + str ;
            }
        }
    }
}
