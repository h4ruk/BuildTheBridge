using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Gameplay
{
    internal class Player
    {
        public int score;
        public string name;
        public Player(string info)
        {
            string[] infos = info.Split(',');
            name = infos[0];
            score = Int32.Parse(infos[1]);
        }
        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
        public override string ToString()
        {
            return name + ',' + score;
        }
    }

    internal class Rank
    {
        private frmMain contain;        
        private Player[] players = new Player[3];

        public Rank(frmMain contain, int yourScore)
        {
            this.contain = contain;            

            ReadPlayerInfo();
            GetPlayerInfo(yourScore, GetRank(yourScore));
        }                
        
        private void ReadPlayerInfo()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);            
            if (!File.Exists(path + @"\rank.csv"))
            {
                FileStream stream = File.Create(path + @"\rank.csv");
                stream.Flush();
                stream.Close();
            }
            using (StreamReader reader = new StreamReader(path + @"\rank.csv"))
            {
                string info;
                for (int i = 0; i < players.Length && (info = reader.ReadLine()) != null; i++)                    
                        players[i] = new Player(info);                    
            }
        }

        private int GetRank(int score) {
            //not ranked when score = 0
            if (score == 0) return -1;

            //determine rank
            int rank = 0;
            while(rank < players.Length && players[rank] != null)
            {
                if (score > players[rank].score)
                    break;
                else
                    rank++;
            }            
            return rank;                
        }

        private void GetPlayerInfo(int score, int rank)
        {

            //player not get ranked
            if (rank >= players.Length || rank < 0) Display();
            else
            {
                //notify when player get ranked
                MessageBox.Show($"Bạn đạt được top {rank + 1}\nHãy ghi tên mình vào trong textbox.", "Chúc mừng!", MessageBoxButtons.OK);

                //get player name
                TextBox nameEntry = new TextBox();
                nameEntry.Size = new Size(250, 100);
                nameEntry.Location = new Point((contain.Width - nameEntry.Width) / 2, (contain.Height - nameEntry.Height) / 2);
                nameEntry.KeyDown += (object sender, KeyEventArgs arg) => {
                    if (arg.KeyCode == Keys.Enter)
                    {
                        string name = (!string.IsNullOrEmpty(nameEntry.Text)) ? nameEntry.Text : "No name";
                        contain.Controls.Remove(nameEntry);
                        Update(name, score, rank);

                    }
                };
                contain.Controls.Add(nameEntry);
                contain.Invalidate();
            }
        }


        private void Update(string name, int score, int rank) {
            //order
            Player player = new Player(name, score);            
            for (int i = players.Length - 1; i > rank; i--) 
                players[i] = players[i - 1];            
            players[rank] = player;

            //update data file
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (StreamWriter writer = new StreamWriter(path + @"\rank.csv")) {
                for (int i = 0; i < players.Length && players[i] != null; i++)                                         
                    writer.WriteLine(players[i]);                
            }
            
            Display(true);
        }

        private void Display(bool isPlayerGetRanked = false)
        {
            string rankedList = "";
            for(int i = 0; i < players.Length && players[i] != null; i++)
                rankedList += (char)(65 + i) + " " + players[i].name + " - " + players[i].score + '\n';
            if (!isPlayerGetRanked)
                rankedList += "\nBẠN THUA RỒI!\n DO BẠN GÀ Á.";
            else
                rankedList += "\nCHÚC MỪNG BẠN ĐÃ ĐẠT TOP";

                //show game overoption
                onGameoverOption(rankedList);
            contain.Invalidate();            
        }

        private void onGameoverOption(string rankedList)
        {
            //label
            Label label = new Label();
            label.Text = rankedList;
            label.Size = new Size(500,500);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Lucida Handwriting", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = Color.Black;
            label.Location = new Point((contain.Width - label.Width) / 2, (contain.Height - label.Height) / 2 - 70);

            // btnReset
            Button btnReset = new Button();
            btnReset.BackColor = Color.Transparent;
            btnReset.BackgroundImageLayout = ImageLayout.Stretch;
            btnReset.FlatAppearance.BorderColor = Color.White;
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.FlatAppearance.MouseOverBackColor = Color.Tan;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Lucida Handwriting", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnReset.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            btnReset.Location = new Point(40, 435);
            btnReset.Margin = new Padding(2);
            btnReset.Size = new Size(96, 54);
            btnReset.Text = "Chơi lại";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += new EventHandler(this.btnReset_Click);

            // btnExit
            Button btnExit = new Button(); 
            btnExit.BackColor = Color.Transparent;
            btnExit.BackgroundImageLayout = ImageLayout.Stretch;            
            btnExit.FlatAppearance.BorderColor = Color.White;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.MouseOverBackColor = Color.Tan;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Lucida Handwriting", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnExit.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            btnExit.Location = new Point(267, 435);
            btnExit.Margin = new Padding(2);            
            btnExit.Size = new Size(96, 54);            
            btnExit.Text = "Thoát";
            btnExit.UseVisualStyleBackColor = false;            
            btnExit.Click += new EventHandler(this.btnExit_Click);

            //add to contain
            contain.Controls.AddRange(new Control[]{ btnReset, btnExit, label});
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            contain.Hide();
            frm.ShowDialog();
            contain.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            frmWelcome frm = new frmWelcome();
            contain.Hide();
            frm.ShowDialog();
            contain.Close();
        }
    }
}
