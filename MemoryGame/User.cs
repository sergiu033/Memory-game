using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Text.Json.Serialization;

namespace MemoryGame
{
    public class User : INotifyPropertyChanged
    {
        private string userName;
        private string profilePictureFilePath;

        public string UserName
        {
            get { return userName; }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string ProfilePictureFilePath
        {
            get { return profilePictureFilePath; }
            set
            {
                if (profilePictureFilePath != value)
                {
                    profilePictureFilePath = value;
                    OnPropertyChanged(nameof(ProfilePictureFilePath));
                }
            }
        }

        [JsonIgnore]
        public ImageSource ProfilePicture
        {
            get
            {
                if (string.IsNullOrEmpty(ProfilePictureFilePath))
                    return null;

                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProfilePictureFilePath);

                if (File.Exists(fullPath))
                {
                    return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }

                return null;
            }
        }

        public User() { }

        public User(string userName, string profilePictureFilePath)
        {
            UserName = userName;
            ProfilePictureFilePath = profilePictureFilePath;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
