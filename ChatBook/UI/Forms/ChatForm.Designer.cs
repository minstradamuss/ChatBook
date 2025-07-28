using System.Drawing;

namespace ChatBook.UI.Forms
{
    partial class ChatForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxMessages;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox listBoxChats;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.listBoxMessages = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.listBoxChats = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxMessages
            // 
            this.listBoxMessages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.listBoxMessages.ItemHeight = 16;
            this.listBoxMessages.Location = new System.Drawing.Point(251, 12);
            this.listBoxMessages.Name = "listBoxMessages";
            this.listBoxMessages.Size = new System.Drawing.Size(576, 340);
            this.listBoxMessages.TabIndex = 1;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(251, 370);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(475, 22);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSend.Location = new System.Drawing.Point(732, 368);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(95, 26);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // listBoxChats
            // 
            this.listBoxChats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.listBoxChats.ItemHeight = 16;
            this.listBoxChats.Location = new System.Drawing.Point(12, 12);
            this.listBoxChats.Name = "listBoxChats";
            this.listBoxChats.Size = new System.Drawing.Size(233, 340);
            this.listBoxChats.TabIndex = 0;
            this.listBoxChats.DoubleClick += new System.EventHandler(this.listBoxChats_DoubleClick);
            // 
            // ChatForm
            //
            this.listBoxMessages.Font = new Font("Arial", 11);
            this.txtMessage.Font = new Font("Arial", 11);
            this.listBoxChats.Font = new Font("Arial", 11);
           
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(839, 410);
            this.Controls.Add(this.listBoxChats);
            this.Controls.Add(this.listBoxMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(857, 457);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(857, 457);
            this.Name = "ChatForm";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
