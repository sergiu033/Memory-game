using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MemoryGame.Models;

namespace MemoryGame.ViewModels
{
    public class AddUserViewModel : INotifyPropertyChanged
    {
        private Action refreshInSignUp;

        private string newUserName;
        private string newUserProfilePath;
        public String NewUserName
        {
            get { return newUserName; }
            set
            {
                if (newUserName != value)
                {
                    newUserName = value;
                    OnPropertyChanged(nameof(NewUserName));
                }
            }
        }

        public String NewUserProfilePath
        {
            get { return newUserProfilePath; }
            set
            {
                if (newUserProfilePath != value)
                {
                    newUserProfilePath = value;
                    OnPropertyChanged(nameof(NewUserProfilePath));
                }
            }
        }
        public ObservableCollection<string> AvailableProfilePictures { get; }

        private string selectedProfilePicture;
        private int profilePictureIndex = 0;
        public string SelectedProfilePicture
        {
            get
            {
                return selectedProfilePicture;
            }
            set
            {
                if (selectedProfilePicture != value)
                {
                    selectedProfilePicture = value;
                    OnPropertyChanged(nameof(SelectedProfilePicture)); 
                    OnPropertyChanged(nameof(SelectedImageSource));
                    NewUserProfilePath = SelectedProfilePicture;
                }
            }
        }

        public ImageSource SelectedImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(selectedProfilePicture))
                    return null;

                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedProfilePicture);

                if (File.Exists(fullPath))
                {
                    return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }

                return null;
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand MovePictureLeftCommand { get; }
        public ICommand MovePictureRightCommand { get; }

        public AddUserViewModel(Action refreshInSignUp)
        {
            NewUserName = "";
            NewUserProfilePath = "";

            AvailableProfilePictures = new ObservableCollection<string>
            {
                "Data/ProfilePictures/image1.jpg",
                "Data/ProfilePictures/image2.jpg",
                "Data/ProfilePictures/image3.jpg",
                "Data/ProfilePictures/image4.jpg",
                "Data/ProfilePictures/image5.jpg"
            };

            SelectedProfilePicture = AvailableProfilePictures[profilePictureIndex];

            AddUserCommand = new RelayCommand(AddUser);
            MovePictureLeftCommand = new RelayCommand(MovePictureLeft);
            MovePictureRightCommand = new RelayCommand(MovePictureRight);
            this.refreshInSignUp = refreshInSignUp;
        }

        private void MovePictureLeft(object parameter)
        {
            if (profilePictureIndex == 0)
                profilePictureIndex = AvailableProfilePictures.Count - 1;
            else
                profilePictureIndex--;

                SelectedProfilePicture = AvailableProfilePictures[profilePictureIndex];
        }

        private void MovePictureRight(object parameter)
        {
            if (profilePictureIndex == AvailableProfilePictures.Count - 1)
                profilePictureIndex = 0;
            else
                profilePictureIndex++;

            SelectedProfilePicture = AvailableProfilePictures[profilePictureIndex];
        }

        private static string UsersFilePath = "Data/users.json";

        private void AddUser(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewUserName))
            {
                return;
            }

            var users = LoadUsersFromJson(UsersFilePath);
            users.Add(new User(NewUserName, NewUserProfilePath, 0, 0));
            SaveUsersToJson(UsersFilePath, users);

            NewUserName = "";
            NewUserProfilePath = "";

            refreshInSignUp?.Invoke();
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

        private void SaveUsersToJson(string filePath, ObservableCollection<User> users)
        {
            string json = JsonSerializer.Serialize(users.ToList());

            File.WriteAllText(filePath, json);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
