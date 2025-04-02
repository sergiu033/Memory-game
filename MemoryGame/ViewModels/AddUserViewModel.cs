using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using System;

namespace MemoryGame.ViewModels
{
    public class AddUserViewModel : INotifyPropertyChanged
    {
        private User newUser;
        public User NewUser
        {
            get { return newUser; }
            set
            {
                if (newUser != value)
                {
                    newUser = value;
                    OnPropertyChanged(nameof(NewUser));
                }
            }
        }

        public ICommand SelectProfilePictureCommand { get; }
        public ICommand AddUserCommand { get; }

        public AddUserViewModel()
        {
            NewUser = new User(string.Empty, string.Empty);
            SelectProfilePictureCommand = new RelayCommands(SelectProfilePicture);
            AddUserCommand = new RelayCommands(AddUser);
        }

        private void SelectProfilePicture(object parameter)
        {
            // Open a file dialog to allow the user to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select Profile Picture"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Set the selected image file path to the NewUser
                NewUser.ProfilePictureFilePath = openFileDialog.FileName;
            }
        }

        private void AddUser(object parameter)
        {
            // Logic to add the user (e.g., adding to a collection or saving to a file)
            // Example: 
            // Users.Add(NewUser);
            Console.WriteLine($"User added: {NewUser.UserName}, Profile Picture: {NewUser.ProfilePictureFilePath}");

            // Reset the form after adding
            NewUser = new User(string.Empty, string.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
