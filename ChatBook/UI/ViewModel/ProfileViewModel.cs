using ChatBook.Entities;

namespace ChatBook.ViewModels
{
    public class ProfileViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public ProfileViewModel(User user, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            CurrentUser = user;
        }

        public User CurrentUser { get; set; }

        internal bool UpdateProfile()
        {
            return _mainViewModel.UpdateProfile(CurrentUser);
        }

    }

}
