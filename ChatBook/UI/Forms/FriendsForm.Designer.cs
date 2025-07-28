namespace ChatBook.UI.Forms
{
    partial class FriendsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFriends;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnShowFriends;
        private System.Windows.Forms.Button btnShowFollowers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.flowLayoutPanelFriends = new System.Windows.Forms.FlowLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnShowFriends = new System.Windows.Forms.Button();
            this.btnShowFollowers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowLayoutPanelFriends
            // 
            this.flowLayoutPanelFriends.AutoScroll = true;
            this.flowLayoutPanelFriends.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.flowLayoutPanelFriends.Location = new System.Drawing.Point(20, 60);
            this.flowLayoutPanelFriends.Name = "flowLayoutPanelFriends";
            this.flowLayoutPanelFriends.Size = new System.Drawing.Size(550, 300);
            this.flowLayoutPanelFriends.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(20, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 22);
            this.txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSearch.Location = new System.Drawing.Point(230, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 26);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnShowFriends
            // 
            this.btnShowFriends.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnShowFriends.Location = new System.Drawing.Point(320, 18);
            this.btnShowFriends.Name = "btnShowFriends";
            this.btnShowFriends.Size = new System.Drawing.Size(120, 26);
            this.btnShowFriends.TabIndex = 3;
            this.btnShowFriends.Text = "Мои подписки";
            this.btnShowFriends.UseVisualStyleBackColor = false;
            this.btnShowFriends.Click += new System.EventHandler(this.btnShowFriends_Click);
            // 
            // btnShowFollowers
            // 
            this.btnShowFollowers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnShowFollowers.Location = new System.Drawing.Point(450, 18);
            this.btnShowFollowers.Name = "btnShowFollowers";
            this.btnShowFollowers.Size = new System.Drawing.Size(120, 26);
            this.btnShowFollowers.TabIndex = 4;
            this.btnShowFollowers.Text = "Подписчики";
            this.btnShowFollowers.UseVisualStyleBackColor = false;
            this.btnShowFollowers.Click += new System.EventHandler(this.btnShowFollowers_Click);
            // 
            // FriendsForm
            // 
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.flowLayoutPanelFriends);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnShowFriends);
            this.Controls.Add(this.btnShowFollowers);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(618, 447);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(618, 447);
            this.Name = "FriendsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подписки";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnSearch;
    }
}
