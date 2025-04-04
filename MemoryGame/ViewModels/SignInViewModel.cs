using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using MemoryGame.Views;
using static System.Net.Mime.MediaTypeNames;

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
                    OnSelectedUserChanged();
                }
            }
        }

        public ICommand DeleteUserCommand { get; }
        public ICommand OpenAddUserWindowCommand {  get; }
        public ICommand OpenOptionsWindowCommand { get; }

        public SignInViewModel()
        {
            Users = LoadUsersFromJson("users.json");

            OpenAddUserWindowCommand = new RelayCommand(OpenAddUserWindow);
            DeleteUserCommand = new RelayCommand(DeleteUser, IsUserSelected);
            OpenOptionsWindowCommand = new RelayCommand(OpenOptionsWindow, IsUserSelected);
        }

        private ObservableCollection<User> LoadUsersFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var users = JsonSerializer.Deserialize<ObservableCollection<User>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return users ?? new ObservableCollection<User>();
        }

        private void SaveUsersToJson(string filePath)
        {
            string json = JsonSerializer.Serialize(Users.ToList());

            File.WriteAllText(filePath, json);
        }

        private void DeleteUser(object parameter)
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                SaveUsersToJson("users.json");
                SelectedUser = null;
            }
        }

        private bool IsUserSelected(object parameter)
        {
            return SelectedUser != null;
        }

        private void OnSelectedUserChanged()
        {
            (DeleteUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (OpenOptionsWindowCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void OpenAddUserWindow(object parameter)
        {
            AddUserWindow addUserWindow = new AddUserWindow(RefreshUserList);
            addUserWindow.ShowDialog();
        }

        private void OpenOptionsWindow(object parameter)
        {
            OptionsWindow optionsWindow = new OptionsWindow(SelectedUser);
            optionsWindow.ShowDialog();
        }

        public void RefreshUserList()
        {
            Users = LoadUsersFromJson("users.json");
            OnPropertyChanged(nameof(Users));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
