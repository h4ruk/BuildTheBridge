using System;
using System.Media;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Gameplay
{
    public partial class frmMain : Form
    {        
        private int Mount1_X;               //Tọa độ x của núi 1
        private int Mount2_X;               //Tọa độ x của núi 2
        private int Mount_Y;                //Tọa độ y của núi (chung cho núi 1 và 2)
        private int Mount1_Width;           //Độ rộng núi 1
        private int Mount2_Width;           //Độ rộng núi 2
        private int Mount_Distance;         //Khoảng cách giữa 2 núi
        private int Character_X;            //Tọa độ x của nhân vật
        private int Character_Y;            //Tọa độ y của nhân vật
        private int Score;                  //Điểm số
        private double BridgeLength;        //Độ dài của cầu
        private double BridgeAngle = 0;     //Góc của cầu khi rơi
        private bool PressSpace;            //Kiểm tra nhấn phím cách
        private bool CreateBridge;          //Kiểm tra khởi tạo cầu
        private bool CharacterMove;         //Kiểm tra di chuyển của nhân vật
        private bool MoveToNextMount;                  //Kiểm tra di chuyển toàn bộ (nhân vật, núi, cầu) khi qua thành công
        private bool Fail;                  //Kiểm tra di chuyển thất bại hay thành công
        private bool BridgeFalling;         //Kiểm tra cầu có rơi không
        private Point Start;                //Điểm bắt đầu của cầu
        private Point Finish;               //Điểm kết thúc của cầu
        private Random rd = new Random();   //Biến ngẫu nhiên
        private static SoundPlayer BridgeSound = new SoundPlayer(Gameplay.Properties.Resources.StickGrowLoop);
        private static SoundPlayer KickSound = new SoundPlayer(Gameplay.Properties.Resources.Kick);
        private static SoundPlayer BridgeFallingSound = new SoundPlayer(Gameplay.Properties.Resources.Fall);
        private static SoundPlayer DeathSound = new SoundPlayer(Gameplay.Properties.Resources.Death);
        private static SoundPlayer ScoreSound = new SoundPlayer(Gameplay.Properties.Resources.Score);
        private static SoundPlayer ScoreBonusSound = new SoundPlayer(Gameplay.Properties.Resources.ScoreMid);
        private PictureBox picCharacter;
        public Label lblScore;
        private Timer tmrGame;
        private IContainer components;

        public frmMain()
        {
            InitializeComponent();
        }

        //Xử lý khi load màn hình
        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;                      //Bật chế độ Double Buffered giảm giật
            picCharacter.Size = new Size(30, 30);       //Thiết lập kích thước cho nhân vật
            Mount1_X = 0;                               //Núi 1 nằm ở bìa trái
            Score = 0;                                  //Điểm số khi chưa chơi
            PressSpace = true;                          //Cho phép nhấn phím Space
            lblScore.Text = Score.ToString();           //Hiện điểm lên màn hình
            Mount1_Width = 60;                          //Độ rộng núi 1
            Mount2_X = Mount1_X + rd.Next(100, 200);    //Tạo ngẫu nhiên khoảng cách 2 núi
            Mount2_Width = rd.Next(40, 70);             //Tạo ngẫu nhiên độ rộng núi 2
            Mount_Y = 380;                              //Độ cao hai núi
            Character_X = Mount1_X + Mount1_Width -
                picCharacter.Width - 5;                 //Thiết lập tọa độ x cho nhân vật
            Character_Y = 350;                          //Thiết lập tọa độ y cho nhân vật(dưới)
            tmrGame.Interval = 5;                      //Thiết lập đoạn thời gian cho timer(tốc độ game)
            Start = new Point
                (Mount1_X + Mount1_Width, Mount_Y - 1); //Thiết lập tọa độ điểm bắt đầu của cầu -1 để nằm trên bật núi
            Finish = Start;                             //Thiết lập tọa độ điểm kết thúc của cầu
        }

        //Tính độ dài của cầu
        private double CalculateBridgeLength(Point Start, Point Finish)
        {
            return Math.Sqrt(Math.Pow(Start.X - Finish.X, 2) + 
                Math.Pow(Start.Y - Finish.Y, 2));//tổng bình phương khoảng cách theo trục X và khoảng cách theo trục Y giữa hai điểm đó.
        }

        //Xử lý vẽ trong trò chơi
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;             //Chế độ làm mượt khi vẽ
            e.Graphics.FillRectangle(new SolidBrush(Color.Black),
                Mount1_X, Mount_Y, Mount1_Width, this.Height - Mount_Y);    //Vẽ núi 1
            e.Graphics.FillRectangle(new SolidBrush(Color.Black),
                Mount2_X, Mount_Y, Mount2_Width, this.Height - Mount_Y);    //Vẽ núi 2
            e.Graphics.FillRectangle(new SolidBrush(Color.Red),
                Mount1_X + Mount1_Width / 2 - 4, Mount_Y, 8, 6);            //Vẽ điểm bonus núi 1(điểm giữa)
            e.Graphics.FillRectangle(new SolidBrush(Color.Red),
                Mount2_X + Mount2_Width / 2 - 4, Mount_Y, 8, 6);            //Vẽ điểm bonus núi 2
            e.Graphics.DrawLine(new Pen(Color.Black, 3), Start, Finish);    //Vẽ cầu
            picCharacter.Location = new Point
                (Character_X, Character_Y);                                 //Thiết lập tọa độ mới cho nhân vật
        }

        //Xử lý timer trong trò chơi
        private void tmrGame_Tick(object sender, EventArgs e)
        {
            if (CreateBridge)                           //Khi đang tạo cầu
            {
                Character_Y = 350;                      //Thiết lập tọa độ Y cho nhân vật
                Finish.Y -= 4;                          //Tọa độ điểm kết thúc giảm (cầu đang lên cao)
                this.Invalidate();                      //Vẽ lại màn hình
            } 
            if (BridgeFalling)                          //Khi cầu đang rơi
            {
                Finish.X = Convert.ToInt32
                    (Start.X + (BridgeLength * 
                    Math.Sin((BridgeAngle * Math.PI) / 180)));      //Thiết lập tọa độ x mới cho cầu khi rơi
                Finish.Y = Convert.ToInt32
                    (Start.Y - (BridgeLength * 
                    Math.Cos((BridgeAngle * Math.PI) / 180)));      //Thiết lập tọa độ y mới cho cầu khi rơi
                BridgeAngle += 5;                                   //Tăng góc độ cho cầu khi rơi
                this.Invalidate();                                  //Vẽ lại màn hình
                if (Start.Y == Finish.Y && Start.X + BridgeLength == Finish.X)          //Khi cầu nằm ngang
                {
                    BridgeFallingSound.Play();                                          //Phát âm thanh đặt cầu
                    if (Finish.X <= Mount2_X || Finish.X >= Mount2_X + Mount2_Width)    //Nếu cầu đặt được lên núi
                        BridgeAngle = 90;                                               //Góc của cầu là góc vuông
                    else                                                                //Nếu không đặt cầu được lên núi
                        BridgeAngle = 0;                                                //Góc của cầu không có (rơi xuống vực)                                             
                    BridgeFalling = false;                                              //Không cho phép cầu rơi
                    CharacterMove = true;                                               //Cho phép nhân vật di chuyển
                }
            }  
            if (CharacterMove)                                                          //Khi nhân vật di chuyển
            {
                if (Finish.X < Mount2_X || Finish.X > Mount2_X + Mount2_Width)        //Di chuyển không thành công
                {
                    Character_X += 5;
                    if (Character_X > Start.X + BridgeLength)
                        Character_Y = 350;
                    else 
                        Character_Y = 348;
                    if (Character_X >= Finish.X)
                    {
                        CharacterMove = false;                                          //Không cho di chuyển nữa
                        Fail = true;                                                    //Qua không thành công
                        System.Threading.Thread.Sleep(100);                              //Dừng để xem người chết
                        DeathSound.Play();                                              //Phát nhạc chết
                    }
                }
                else if (Finish.X > Mount2_X + Mount2_Width / 2 - 4 && 
                    Finish.X < Mount2_X + Mount2_Width / 2 + 4)                         //Di chuyển thành công vào điểm bonus                
                {
                    Character_X += 5;
                    if (Character_X >= Mount2_X + Mount2_Width - picCharacter.Width - 10)
                    {
                        CharacterMove = false;                                          //Không cho di chuyển nữa
                        Score += 3;                                                     //Tăng 3 điểm
                        ScoreBonusSound.Play();                                         //Phát âm thanh điểm thưởng
                        MoveToNextMount = true;                                                    //Di chuyển thành công  
                    }
                    if (Character_X > Start.X + BridgeLength)
                        Character_Y = 350;
                    else
                        Character_Y = 348;
                    lblScore.Text = Score.ToString();
                }  
                else                                                                    //Di chuyển thành công không vào điểm bonus               
                {
                    Character_X += 5;
                    if (Character_X >= Mount2_X + Mount2_Width - picCharacter.Width - 10)
                    {
                        CharacterMove = false;                                          //Không cho di chuyển nữa
                        Score += 1;                                                     //Tăng 1 điểm
                        ScoreSound.Play();                                              //Phát âm thanh điểm
                        MoveToNextMount = true;                                                    //Di chuyển thành công  
                    }
                    if (Character_X > Start.X + BridgeLength)                           //kiểm tra vị trí nhân vật = núi ? cập nhật vị trí nhân vậy ở đầu núi
                        Character_Y = 350;
                    else
                        Character_Y = 348;                                              // nv đứng trên cầu
                    lblScore.Text = Score.ToString();                                   //Cập nhật điểm vô mà hình
                }    
                this.Invalidate();
            }    
            if (Fail)                                                                   //Khi bị fail
            {
                Finish.X = Convert.ToInt32
                    (Start.X + (BridgeLength *
                    Math.Sin((BridgeAngle * Math.PI) / 180)));                          //Thiết lập tọa độ x mới cho cầu khi rơi
                Finish.Y = Convert.ToInt32
                    (Start.Y - (BridgeLength *
                    Math.Cos((BridgeAngle * Math.PI) / 180)));                          //Thiết lập tọa độ y mới cho cầu khi rơi
                BridgeAngle += 5;
                Character_Y += 10;
                if (Character_X + picCharacter.Width > Mount2_X)
                    picCharacter.Size = new Size(picCharacter.Width - 
                        (Character_X + picCharacter.Width - Mount2_X), 30);
                if (Character_X > Mount2_X + Mount2_Width)
                    picCharacter.Size = new Size(30, 30);
                this.Invalidate();
                if (Start.X == Finish.X && Start.Y + BridgeLength == Finish.Y)
                {
                    BridgeAngle = 0;
                    Fail = false;
                    tmrGame.Stop();
                    Rank rank = new Rank(this, Score);                    
                }                    
            }   
            if (MoveToNextMount)                                                                   //Qua cầu thành công (di chuyển toàn bộ màn hình)
            {
                Start = new Point
                (Mount1_X + Mount1_Width, Mount_Y - 1);                                 //Thiết lập tọa độ điểm bắt đầu mới cho cầu
                Finish = Start;
                Mount2_X -= 10;                                                         //Di chuyển núi 2
                Mount1_X -= 10;                                                         //Di chuyển núi 1
                Character_X -= 10;                                                      //Di chuyển nhân vật
                Start.X -= 10;                                                          //Di chuyển tọa độ x điểm bắt đầu của cầu
                Finish.X -= 10;                                                         //Di chuyển tọa độ x điểm kết thúc của cầu
                this.Invalidate();                                                      //Vẽ lại màn hình
                if (Character_X - 30 <= 0)                    //kiểm tra tọa độ nhân vật có sát mép mh không                          
                {                                             //nếu có thì di chuyển
                    Mount1_X += 10;
                    Start.X += 10;
                    Finish.X += 10;
                    Character_X += 10;
                    if (Mount2_X - (Mount1_X + Mount1_Width) <= Mount_Distance)//code cũng kiểm tra xem vị trí của ngọn núi phía sau
                    {                                                           //so với vị trí của ngọn núi phía trước có chênh lệch nhỏ hơn khoảng cách giữa hai ngọn núi không hay không.
                         MoveToNextMount = false;
                        PressSpace = true;                                                       //Nếu có, biến Move được đặt bằng false, đại diện cho việc di chuyển của ký tự sẽ dừng lại
                                                                                //biến PressSpace được đặt bằng true, đại diện cho việc cần nhấn phím Space để bắt đầu một hành động mới.
                    }                                                          
                    
                }
                if (Mount1_X + Mount1_Width <= 0)
                {
                    Mount_Distance = rd.Next(70, 280);
                    Mount1_X = Mount2_X;
                    Mount1_Width = Mount2_Width;
                    Mount2_X = this.Width;
                    Mount2_Width = rd.Next(22, 60);
                }
            }    
        }

        //Xử lý khi nhấn phím xuống
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)                //Nếu nhấn phím Space
            {
                if (PressSpace)                         //Cho phép nhấn phím Space
                {
                    BridgeSound.PlayLooping();          //Phát âm thanh tạo cầu lặp liên tục
                    CreateBridge = true;                //Cho phép tạo cầu
                    PressSpace = false;                 //Không cho phép nhấn phím Space
                    tmrGame.Start();                    //Cho timer chạy
                }    
            }    
        }

        //Xử lý khi thả phím ra
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)                //Nếu thả phím Space
            {
                if (CreateBridge)                       //Nếu đã tạo cầu
                {
                    BridgeSound.Stop();                 //Dừng âm thanh tạo cầu
                    KickSound.Play();                   //Phát âm thanh tạo xong cầu
                    CreateBridge = false;               //Không cho tạo cầu nữa
                    BridgeLength = CalculateBridgeLength(
                        Start, Finish);                 //Tính độ dài cầu
                    BridgeFalling = true;               //Cho cầu rơi
                    System.Threading.Thread.Sleep(300); //Dừng để xem cầu rơi
                }    
            }    
        }

        private void InitializeComponent()
        {            
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.picCharacter = new System.Windows.Forms.PictureBox();
            this.lblScore = new System.Windows.Forms.Label();
            this.tmrGame = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).BeginInit();
            this.SuspendLayout();
            // 
            // picCharacter
            // 
            this.picCharacter.BackColor = System.Drawing.Color.Transparent;
            this.picCharacter.Image = ((System.Drawing.Image)(resources.GetObject("picCharacter.Image")));
            this.picCharacter.Location = new System.Drawing.Point(30, 350);
            this.picCharacter.Margin = new System.Windows.Forms.Padding(2);
            this.picCharacter.Name = "picCharacter";
            this.picCharacter.Size = new System.Drawing.Size(30, 30);
            this.picCharacter.TabIndex = 1;
            this.picCharacter.TabStop = false;
            // 
            // lblScore
            // 
            this.lblScore.BackColor = System.Drawing.Color.Transparent;
            this.lblScore.Font = new System.Drawing.Font("Nirmala UI", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblScore.Location = new System.Drawing.Point(166, 7);
            this.lblScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(84, 58);
            this.lblScore.TabIndex = 3;
            this.lblScore.Text = "0";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrGame
            // 
            this.tmrGame.Tick += new System.EventHandler(this.tmrGame_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = global::Gameplay.Properties.Resources.Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(395, 541);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.picCharacter);
            this.DoubleBuffered = true;
            this.Icon = global::Gameplay.Properties.Resources.CharacterICO;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stick Hero";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
