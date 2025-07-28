using ChatBook.Domain.Factories;
using ChatBook.Domain.Services;
using ChatBook.Entities;
using ChatBook.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChatBook.UI.Windows
{
    public partial class AddBookWindow : Window
    {
        private readonly UserService _userService;
        private Book _book;
        private byte[] _coverBytes;
        private User _user;
        private bool _isReadOnly;
        private int _selectedRating = 0;
        private readonly AddBookViewModel _viewModel;
        private readonly IBookFactory _bookFactory;
        public AddBookWindow(AddBookViewModel viewModel, User user, IBookFactory bookFactory, Book book = null, bool isReadOnly = false)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _bookFactory = bookFactory;
            _user = user;
            _book = book ?? new Book();
            _isReadOnly = isReadOnly;

            btnSave.Click += BtnSave_Click;
            btnUploadCover.Click += BtnUploadCover_Click;
            btnDelete.Click += BtnDelete_Click;

            if (_book != null)
                LoadBookData();

            InitializeStars();

            if (_isReadOnly)
                DisableEditing();
            else
                ShowButtons();
        }

        public void Initialize(User user, Book book = null, bool isReadOnly = false)
        {
            _user = user;
            _book = book ?? new Book();
            _isReadOnly = isReadOnly;

            LoadBookData();
            InitializeStars();

            if (_isReadOnly)
                DisableEditing();
            else
                ShowButtons();
        }

        private void LoadBookData()
        {
            txtTitle.Text = _book.Title;
            txtAuthor.Text = _book.Author;
            foreach (ComboBoxItem item in cmbStatus.Items)
            {
                if ((item.Content as string) == _book.Status)
                {
                    cmbStatus.SelectedItem = item;
                    break;
                }
            }
            txtReview.Text = _book.Review;
            _selectedRating = _book.Rating;

            if (_book.CoverImage != null)
            {
                _coverBytes = _book.CoverImage;
                using (var ms = new MemoryStream(_book.CoverImage))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imgCover.Source = bitmap;
                }
            }

            UpdateStars();
        }


        private void DisableEditing()
        {
            txtTitle.IsReadOnly = true;
            txtAuthor.IsReadOnly = true;
            txtReview.IsReadOnly = true;
            cmbStatus.IsEnabled = false;
            btnSave.Visibility = Visibility.Collapsed;
            btnUploadCover.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            StarsPanel.IsEnabled = false;
        }

        private void ShowButtons()
        {
            btnSave.Visibility = Visibility.Visible;
            btnUploadCover.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
        }

        private void BtnUploadCover_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Images|*.png;*.jpg;*.jpeg" };
            if (dialog.ShowDialog() == true)
            {
                _coverBytes = File.ReadAllBytes(dialog.FileName);
                imgCover.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter the book title.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _book.Title = txtTitle.Text;
            _book.Author = txtAuthor.Text;
            _book.Status = cmbStatus.Text;
            _book.Review = txtReview.Text;
            _book.CoverImage = _coverBytes;
            _book.Rating = _selectedRating; 

            try
            {
                if (_book.Id == 0) 
                {
                    var newBook = _bookFactory.Create(
                        txtTitle.Text,
                        txtAuthor.Text,
                        genre: "", 
                        cmbStatus.Text,
                        _selectedRating,
                        txtReview.Text,
                        _coverBytes,
                        _user.Id
                    );

                    _viewModel.AddBook(newBook, _user.Nickname);
                }
                else
                {
                    _book.Title = txtTitle.Text;
                    _book.Author = txtAuthor.Text;
                    _book.Status = cmbStatus.Text;
                    _book.Review = txtReview.Text;
                    _book.CoverImage = _coverBytes;
                    _book.Rating = _selectedRating;

                    _viewModel.UpdateBook(_book);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string details = string.Join("\n", ex.EntityValidationErrors
                    .SelectMany(ve => ve.ValidationErrors)
                    .Select(ve => $"{ve.PropertyName}: {ve.ErrorMessage}"));

                MessageBox.Show($"Validation failed:\n{details}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_book.Id != 0)
                _viewModel.DeleteBook(_book.Id);

            this.DialogResult = true;
            this.Close();
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "Book Title")
            {
                txtTitle.Text = "";
                txtTitle.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                txtTitle.Text = "Book Title";
                txtTitle.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void RemoveAuthorPlaceholder(object sender, RoutedEventArgs e)
        {
            if (txtAuthor.Text == "Author")
            {
                txtAuthor.Text = "";
                txtAuthor.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void AddAuthorPlaceholder(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                txtAuthor.Text = "Author";
                txtAuthor.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void InitializeStars()
        {
            StarsPanel.Children.Clear();

            for (int i = 1; i <= 5; i++)
            {
                var star = new TextBlock
                {
                    Text = "☆",
                    FontSize = 16,
                    Margin = new Thickness(2),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = i
                };

                star.MouseLeftButtonUp += (s, e) =>
                {
                    if (s is TextBlock clickedStar && clickedStar.Tag is int rating)
                    {
                        _selectedRating = rating;
                        UpdateStars();
                    }
                };

                StarsPanel.Children.Add(star);
            }

            UpdateStars();
        }

        private void UpdateStars()
        {
            foreach (var child in StarsPanel.Children)
            {
                if (child is TextBlock star && star.Tag is int tag)
                {
                    star.Text = tag <= _selectedRating ? "★" : "☆";
                    star.FontSize = tag <= _selectedRating ? 20 : 16;
                }
            }
        }
    }
}
