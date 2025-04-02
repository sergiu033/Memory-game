using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MemoryGame.ViewModels
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }

        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        public SignInViewModel()
        {
            Users = new ObservableCollection<User>
            {
                new User("Alice", "ProfileImages/image1.jpg"),
                new User("Bob", "ProfileImages/image2.jpg"),
                new User("Charlie", "ProfileImages/image3.jpg")
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
