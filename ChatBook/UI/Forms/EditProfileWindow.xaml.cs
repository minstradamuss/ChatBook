using ChatBook.Entities;
using ChatBook.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChatBook.UI.Windows
{
    public partial class EditProfileWindow : Window
    {
        private readonly ProfileViewModel _viewModel;
        private byte[] _avatarBytes;

        public event Action<User> ProfileUpdated;

        public EditProfileWindow(User user, MainViewModel userService)

        {
            InitializeComponent();
            _viewModel = new ProfileViewModel(user, userService);

            this.DataContext = _viewModel;

            if (user.Avatar != null)
            {
                _avatarBytes = user.Avatar;
                using (var ms = new MemoryStream(_avatarBytes))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    imgAvatar.Source = bitmap;
                }
            }
        }

        private void btnUploadAvatar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp" };
            if (dialog.ShowDialog() == true)
            {
                _avatarBytes = File.ReadAllBytes(dialog.FileName);
                imgAvatar.Source = new BitmapImage(new Uri(dialog.FileName));
                _viewModel.CurrentUser.Avatar = _avatarBytes;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.UpdateProfile())
            {
                ProfileUpdated?.Invoke(_viewModel.CurrentUser);
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при обновлении профиля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


}
