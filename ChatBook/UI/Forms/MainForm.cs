using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ChatBook.Domain.Factories;
using ChatBook.Domain.Services;
using ChatBook.Entities;
using ChatBook.UI.Windows;
using ChatBook.ViewModels;
using ChatService.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBook.UI.Forms
{
    public partial class MainForm : Form
    {
        private User _currentUser;
        private Dictionary<Book, Panel> _userBooks = new Dictionary<Book, Panel>();
        private FlowLayoutPanel flowLayoutPanelBooks;
        private readonly IChatService _chatService;
        private User _logged;
        private Button btnSendMessage;
        private readonly MainViewModel _viewModel;
        private readonly ChatViewModel _chatviewModel;
        public string CurrentUserNickname { get; private set; }
        private bool _isProfileViewOnly = false;

        public MainForm(UserService userService, IChatService chatService)

        {
            InitializeComponent();
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _viewModel = new MainViewModel(userService);
            _logged = AppSession.LoggedUser;

            InitializeFlowLayoutPanel();
        }

        public void SetCurrentUser(User user, bool isViewOnly = false)
        {
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _logged = AppSession.LoggedUser;
            _isProfileViewOnly = isViewOnly;

            CurrentUserNickname = _currentUser.Nickname;

            if (_currentUser == null)
            {
                MessageBox.Show("Ошибка: пользователь не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            UpdateProfileInfo(_currentUser);
            lblNickname.Text = _currentUser.Nickname;

            LoadUserBooks();

            if (_isProfileViewOnly)
            {
                btnAddBook.Visible = false;
                btnEditProfile.Visible = false;
                buttonChats.Visible = false;
                btnSearchBooks.Visible = false;
                buttonSearchFriends.Visible = false;
                btnRemoveFriend.Visible = true;
                
                InitializeSendMessageButton();
            }
        }


        private void btnRefreshBooks_Click(object sender, EventArgs e)
        {
            LoadUserBooks();
        }

        private void InitializeFlowLayoutPanel()
        {
            flowLayoutPanelBooks = new FlowLayoutPanel
            {
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Location = new Point(20, 200),
                Size = new Size(650, 300)
            };
            Controls.Add(flowLayoutPanelBooks);
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            var freshUser = _viewModel.GetUser(_currentUser.Nickname);

            var editProfileWindow = new EditProfileWindow(freshUser, _viewModel);

            editProfileWindow.ProfileUpdated += (updatedUser) =>
            {
                _currentUser = updatedUser;
                UpdateProfileInfo(_currentUser);
            };

            editProfileWindow.ShowDialog();
        }

        private void UpdateProfileInfo(User updatedUser)
        {
            if (updatedUser == null) return;

            _currentUser = updatedUser;
            lblFullName.Text = $"{_currentUser.FirstName} {_currentUser.LastName}";
            if (_currentUser.Avatar != null && _currentUser.Avatar.Length > 0)
            {
                using (var ms = new MemoryStream(_currentUser.Avatar))
                {
                    var img = Image.FromStream(ms);
                    pictureBoxAvatar.Image?.Dispose();
                    pictureBoxAvatar.Image = new Bitmap(img);
                }
            }
            else
            {
                pictureBoxAvatar.Image?.Dispose();
                pictureBoxAvatar.Image = null;
            }
        }

        private void AddBookForm_BookAdded(Book newBook)
        {
            Panel bookPanel = new Panel
            {
                Size = new Size(120, 180),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            PictureBox bookCover = new PictureBox
            {
                Size = new Size(100, 140),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(10, 10),
                Tag = newBook
            };

            Label bookTitle = new Label
            {
                Text = newBook.Title,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 30
            };

            if (newBook.CoverImage != null && newBook.CoverImage.Length > 0)
            {
                try
                {
                    bookCover.Image = ConvertByteArrayToImage(newBook.CoverImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                    bookCover.BackColor = Color.Gray;
                }
            }
            else
            {
                bookCover.BackColor = Color.Gray;
            }

            bookCover.Click += BookCover_Click;
            bookPanel.Controls.Add(bookCover);
            bookPanel.Controls.Add(bookTitle);

            _userBooks[newBook] = bookPanel;
            flowLayoutPanelBooks.Controls.Add(bookPanel);
        }

        private void BookCover_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox bookCover && bookCover.Tag is Book book)
            {
                bool isReadOnly = (_logged.Nickname != _currentUser.Nickname);

                var thread = new System.Threading.Thread(() =>
                {
                    var window = new AddBookWindow(
                            Program.ServiceProvider.GetRequiredService<AddBookViewModel>(),
                            _currentUser,
                            Program.ServiceProvider.GetRequiredService<IBookFactory>(),
                            book,
                            isReadOnly
                        );

                    var result = window.ShowDialog();

                    if (result == true)
                    {
                        this.Invoke(new Action(() =>
                        {
                            LoadUserBooks();
                        }));
                    }
                });

                thread.SetApartmentState(System.Threading.ApartmentState.STA);
                thread.Start();
            }
        }

        private void btnSearchBooks_Click(object sender, EventArgs e)
        {
            var thread = new System.Threading.Thread(() =>
            {
                var window = Program.ServiceProvider.GetRequiredService<BookSearchWindow>();
                window.ShowDialog();
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }


        private ChatForm _chatForm;

        private void buttonChats_Click(object sender, EventArgs e)
        {
            var chatViewModel = Program.ServiceProvider.GetRequiredService<ChatViewModel>();
            ChatForm chatForm = new ChatForm(CurrentUserNickname, chatViewModel);
            chatForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var friendsForm = new FriendsForm(CurrentUserNickname, Program.ServiceProvider.GetRequiredService<FriendsViewModel>());
            friendsForm.Show();
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

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            var thread = new System.Threading.Thread(() =>
            {
                var window = new AddBookWindow(
                    Program.ServiceProvider.GetRequiredService<AddBookViewModel>(),
                    _currentUser,
                    Program.ServiceProvider.GetRequiredService<IBookFactory>(), // 3-й аргумент
                    null,
                    false
                );

                var result = window.ShowDialog();

                if (result == true)
                {
                    this.Invoke(new Action(() => LoadUserBooks()));
                }
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }

        private void LoadUserBooks()
        {
            flowLayoutPanelBooks.Controls.Clear();
            _userBooks.Clear();

            var books = _viewModel.GetUserBooks(_currentUser.Nickname);

            foreach (var book in books)
            {
                AddBookForm_BookAdded(book);
            }

            txtSearchBooks_TextChanged(null, null);
        }

        private void txtSearchBooks_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchBooks.Text == "🔍 Искать книгу...") return;

            string searchText = txtSearchBooks.Text.ToLower().Trim();

            foreach (var bookPanel in _userBooks)
            {
                bool match = bookPanel.Key.Title.ToLower().Contains(searchText);
                bookPanel.Value.Visible = match;
            }
        }

        private void btnRemoveFriend_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Ошибка: _currentUser == null");
                MessageBox.Show("Ошибка: пользователь не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirmResult = MessageBox.Show($"Вы уверены, что хотите удалить {_currentUser.Nickname} из подписок?",
                                                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                bool success = _viewModel.RemoveFriend(_logged.Nickname, _currentUser.Nickname);

                if (success)
                {
                    btnRemoveFriend.Visible = false;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении подписки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtSearchBooks_Enter(object sender, EventArgs e)
        {
            if (txtSearchBooks.Text == "🔍 Искать книгу...")
            {
                txtSearchBooks.Text = "";
                txtSearchBooks.ForeColor = Color.Black;
            }
        }

        private void txtSearchBooks_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchBooks.Text))
            {
                txtSearchBooks.Text = "🔍 Искать книгу...";
                txtSearchBooks.ForeColor = Color.Gray;
            }
        }

        private void InitializeSendMessageButton()
        {
            btnSendMessage = new Button
            {
                Text = "Написать",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 30),
                Location = new Point(600, 60),
                BackColor = Color.LightBlue,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnSendMessage.FlatAppearance.BorderSize = 0;
            btnSendMessage.Click += btnSendMessage_Click;
            Controls.Add(btnSendMessage);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            var chatViewModel = Program.ServiceProvider.GetRequiredService<ChatViewModel>();
            ChatForm chatForm = new ChatForm(_logged.Nickname, chatViewModel, _currentUser.Nickname);
            chatForm.Show();
        }
    }
}
