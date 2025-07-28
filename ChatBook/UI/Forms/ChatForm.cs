using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChatBook.ViewModels;

namespace ChatBook.UI.Forms
{
    public partial class ChatForm : Form
    {
        private readonly string _currentUserNickname;
        private string selectedChat = null;
        private readonly ChatViewModel _viewModel;

        public ChatForm(string currentUserNickname, ChatViewModel viewModel, string chatPartnerNickname = null)
        {
            InitializeComponent();

            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _currentUserNickname = currentUserNickname ?? throw new ArgumentNullException(nameof(currentUserNickname));

            LoadFriendsAndChats();

            if (!string.IsNullOrEmpty(chatPartnerNickname))
            {
                selectedChat = chatPartnerNickname;
                LoadChatMessages(chatPartnerNickname);
            }

            listBoxChats.Click += listBoxChats_Click;
            listBoxChats.DoubleClick += listBoxChats_DoubleClick;
            listBoxChats.KeyDown += listBoxChats_KeyDown;
        }

        private void LoadFriendsAndChats()
        {
            listBoxChats.Items.Clear();
            var friends = _viewModel.GetFriends(_currentUserNickname);
            var chatPartners = _viewModel.GetAllChatPartners(_currentUserNickname);
            var seen = new HashSet<string>();

            foreach (var u in friends.Concat(chatPartners))
            {
                if (seen.Add(u.Nickname))
                {
                    listBoxChats.Items.Add($"{u.Nickname} - {u.FirstName} {u.LastName}");
                }
            }
        }

        private void LoadChatMessages(string chatPartnerNickname)
        {
            listBoxMessages.Items.Clear();
            var messages = _viewModel.GetChatMessages(_currentUserNickname, chatPartnerNickname);

            foreach (var msg in messages)
            {
                string senderLabel = msg.FromUser == _currentUserNickname ? "Вы" : msg.FromUser;
                var wrapped = WrapMessage(msg.Text, 60);
                for (int i = 0; i < wrapped.Count; i++)
                {
                    string prefix = i == 0 ? $"{senderLabel}: " : "        ";
                    listBoxMessages.Items.Add(prefix + wrapped[i]);
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text) && selectedChat != null)
            {
                string receiverNickname = selectedChat.Split('-')[0].Trim();

                _viewModel.SendMessage(_currentUserNickname, receiverNickname, txtMessage.Text);

                LoadChatMessages(receiverNickname);
                txtMessage.Clear();
            }
        }

        private List<string> WrapMessage(string message, int maxLineLength)
        {
            var words = message.Split(' ');
            var lines = new List<string>();
            string current = "";

            foreach (var word in words)
            {
                if ((current + word).Length > maxLineLength)
                {
                    lines.Add(current.TrimEnd());
                    current = "";
                }
                current += word + " ";
            }

            if (!string.IsNullOrWhiteSpace(current))
                lines.Add(current.TrimEnd());

            return lines;
        }

        private void listBoxChats_Click(object sender, EventArgs e)
        {
            if (listBoxChats.SelectedItem != null)
                selectedChat = listBoxChats.SelectedItem.ToString().Split('-')[0].Trim();
        }

        private void listBoxChats_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxChats.SelectedItem != null)
            {
                selectedChat = listBoxChats.SelectedItem.ToString().Split('-')[0].Trim();
                LoadChatMessages(selectedChat);
            }
        }

        private void listBoxChats_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && selectedChat != null)
            {
                listBoxMessages.Items.Clear();
                listBoxChats.Items.Remove(listBoxChats.SelectedItem);
                selectedChat = null;
            }
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            const int maxHeight = 100;
            const int minHeight = 22;

            using (Graphics g = txtMessage.CreateGraphics())
            {
                SizeF size = g.MeasureString(txtMessage.Text, txtMessage.Font, txtMessage.Width);
                int newHeight = Math.Min(maxHeight, Math.Max(minHeight, (int)size.Height + 10));
                txtMessage.Height = newHeight;
            }

            txtMessage.ScrollBars = txtMessage.Height >= maxHeight ? ScrollBars.Vertical : ScrollBars.None;
        }
    }
}
