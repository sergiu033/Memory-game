using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using MemoryGame.Views;

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

        public SignInViewModel()
        {
            Users = LoadUsersFromJson("users.json");

            OpenAddUserWindowCommand = new RelayCommands(OpenAddUserWindow);
            DeleteUserCommand = new RelayCommands(DeleteUser, CanDeleteUser);
        }

        private ObservableCollection<User> LoadUsersFromJson(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

                if (File.Exists(fullPath))
                {
                    string json = File.ReadAllText(fullPath);
                    var users = JsonSerializer.Deserialize<ObservableCollection<User>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return users ?? new ObservableCollection<User>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading JSON: " + ex.Message);
            }

            return new ObservableCollection<User>();
        }

        private void SaveUsersToJson(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                string json = JsonSerializer.Serialize(Users, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(fullPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving JSON: " + ex.Message);
            }
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

        private bool CanDeleteUser(object parameter)
        {
            return SelectedUser != null;
        }

        private void OnSelectedUserChanged()
        {
            (DeleteUserCommand as RelayCommands)?.RaiseCanExecuteChanged();
        }

        private void OpenAddUserWindow(object parameter)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
