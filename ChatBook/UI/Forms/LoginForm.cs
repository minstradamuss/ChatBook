using ChatBook.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using ChatBook.Models;
using ChatBook.Domain.Services;

namespace ChatBook.UI.Forms
{
    public partial class LoginForm : Form
    {
        private readonly LoginViewModel _viewModel;


        public LoginForm(UserService userService)
        {
            InitializeComponent();
            _viewModel = new LoginViewModel(userService);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string nickname = txtNickname.Text.Trim();
            string password = txtPassword.Text.Trim();

            var user = await _viewModel.LoginAsync(nickname, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль, либо пользователь не зарегистрирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var userEntity = UserMapper.ToEntity(user);

            AppSession.SetLoggedUser(userEntity);

            var mainForm = Program.ServiceProvider.GetRequiredService<MainForm>();
            mainForm.SetCurrentUser(userEntity);

            this.Hide();
            mainForm.Show();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string nickname = txtNickname.Text.Trim();
            string password = txtPassword.Text.Trim();

            bool success = await _viewModel.RegisterAsync(nickname, password);

            if (success)
            {
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка регистрации. Такой пользователь уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
