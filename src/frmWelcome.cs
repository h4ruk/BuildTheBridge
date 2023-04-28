using System;
using System.Windows.Forms;

namespace Gameplay
{
    public partial class frmWelcome : Form
    {
        public frmWelcome()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            frmMain Main = new frmMain();
            this.Hide();
            Main.ShowDialog();
            this.Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Game Stick Hero\r\n" +
                "Người viết: Nhóm TTHP\r\n","Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nhấn giữ phím Space để tạo cầu\r\n" +
                "Giữ càng lâu cầu càng dài\r\n" +
                "Cầu chỉ dài chứ không ngắn lại\r\n" +
                "Cầu đặt được lên núi và qua cầu được 1 điểm\r\n" +
                "Cầu đặt được lên núi ngay điểm màu đỏ và qua cầu được 3 điểm\r\n" +
                "Cầu quá ngắn không đặt được lên núi hoặc đặt được lên núi " +
                "nhưng cầu quá dài bạn sẽ rớt xuống vực và thua cuộc",
                "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InitializeComponent()
        {
            this.btnPlay = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlay.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tan;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Lucida Handwriting", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnPlay.Location = new System.Drawing.Point(184, 236);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(151, 58);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "BẮT ĐẦU";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Nirmala UI", 45F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(148, 46);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(199, 162);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "STICK\r\nHERO";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.Transparent;
            this.btnAbout.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnAbout.FlatAppearance.BorderSize = 0;
            this.btnAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tan;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Lucida Handwriting", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnAbout.Location = new System.Drawing.Point(170, 308);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(172, 58);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "THÔNG TIN";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.Transparent;
            this.btnHelp.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tan;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Lucida Handwriting", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnHelp.Location = new System.Drawing.Point(170, 370);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(182, 58);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "HƯỚNG DẪN";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // frmWelcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Gameplay.Properties.Resources.Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(481, 484);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.lblTitle);
            this.Icon = global::Gameplay.Properties.Resources.CharacterICO;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "frmWelcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stick hero";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnHelp;
    }
}
