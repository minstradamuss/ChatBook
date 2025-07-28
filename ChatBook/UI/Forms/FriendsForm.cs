using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ChatBook.Entities;
using ChatBook.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBook.UI.Forms
{
    public partial class FriendsForm : Form
    {
        private readonly string _currentUserNickname;
        private readonly FriendsViewModel _viewModel;

        public FriendsForm(string currentUserNickname, FriendsViewModel viewModel)
        {
            InitializeComponent();
            _currentUserNickname = currentUserNickname;
            _viewModel = viewModel;
            LoadFriends();
        }

        private void LoadFriends()
        {
            flowLayoutPanelFriends.Controls.Clear();
            var friends = _viewModel.GetFriends(_currentUserNickname);

            foreach (var friend in friends)
            {
                Panel friendPanel = CreateFriendPanel(friend, isSearchResult: false);
                flowLayoutPanelFriends.Controls.Add(friendPanel);
            }
        }

        private Panel CreateFriendPanel(User user, bool isSearchResult)
        {
            Panel panel = new Panel
            {
                Size = new Size(200, 220),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                Tag = user
            };

            PictureBox avatar = new PictureBox
            {
                Size = new Size(120, 120),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(40, 10),
                Tag = user
            };

            if (user.Avatar != null && user.Avatar.Length > 0)
            {
                avatar.Image = ConvertByteArrayToImage(user.Avatar);
            }
            else
            {
                avatar.BackColor = Color.Gray;
            }

            Label nicknameLabel = new Label
            {
                Text = user.Nickname,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 140),
                Width = 200,
                Height = 20,
                Tag = user
            };

            Label fullNameLabel = new Label
            {
                Text = $"{user.FirstName} {user.LastName}",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 165),
                Width = 200,
                Height = 20,
                Tag = user
            };

            Button btnAddFriend = null;

            if (isSearchResult && !_viewModel.AreFriends(_currentUserNickname, user.Nickname))
            {
                btnAddFriend = new Button
                {
                    Text = "Подписаться",
                    Location = new Point(30, 190),
                    Width = 150,
                    Height = 20,
                    BackColor = Color.LightGreen
                };
                btnAddFriend.Click += (s, e) => AddFriend(user, panel, btnAddFriend);
                panel.Controls.Add(btnAddFriend);
            }

            panel.DoubleClick += OpenUserProfile;
            avatar.DoubleClick += OpenUserProfile;
            nicknameLabel.DoubleClick += OpenUserProfile;
            fullNameLabel.DoubleClick += OpenUserProfile;

            panel.Controls.Add(avatar);
            panel.Controls.Add(nicknameLabel);
            panel.Controls.Add(fullNameLabel);
            if (btnAddFriend != null)
                panel.Controls.Add(btnAddFriend);

            return panel;
        }

        private void OpenUserProfile(object sender, EventArgs e)
        {
            User user = null;

            if (sender is Panel panel && panel.Tag is User panelUser)
                user = panelUser;
            else if (sender is PictureBox pictureBox && pictureBox.Tag is User pictureUser)
                user = pictureUser;
            else if (sender is Label label && label.Tag is User labelUser)
                user = labelUser;

            if (user == null)
            {
                MessageBox.Show("Ошибка: не удалось получить данные пользователя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Hide();

            var mainForm = Program.ServiceProvider.GetRequiredService<MainForm>();
            mainForm.SetCurrentUser(user, isViewOnly: true);
            mainForm.FormClosed += (s, args) => this.Show();
            mainForm.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchNickname = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchNickname))
            {
                MessageBox.Show("Введите никнейм для поиска.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var foundUsers = _viewModel.SearchUsers(searchNickname);

            flowLayoutPanelFriends.Controls.Clear();

            if (foundUsers.Count == 0)
            {
                MessageBox.Show("Пользователь не найден.", "Результаты поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var user in foundUsers)
            {
                flowLayoutPanelFriends.Controls.Add(CreateFriendPanel(user, isSearchResult: true));
            }
        }

        private void btnShowFriends_Click(object sender, EventArgs e)
        {
            LoadFriends();
        }

        private void AddFriend(User user, Panel panel, Button btnAddFriend)
        {
            bool success = _viewModel.AddFriend(_currentUserNickname, user.Nickname);
            if (success)
            {
                btnAddFriend.Enabled = false;
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении в друзья.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image ConvertByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void btnShowFollowers_Click(object sender, EventArgs e)
        {
            flowLayoutPanelFriends.Controls.Clear();
            var followers = _viewModel.GetFollowers(_currentUserNickname);

            if (followers.Count == 0)
            {
                MessageBox.Show("У вас пока нет подписчиков.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var user in followers)
            {
                flowLayoutPanelFriends.Controls.Add(CreateFriendPanel(user, isSearchResult: false));
            }
        }

    }
}
