using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace MemoryGame.Models
{
    public class GridCell : INotifyPropertyChanged
    {
        private string _pictureFilePath;
        private bool _hasPictureAssigned;
        private bool _isRevealed;

        public GridCell() { }

        public GridCell(string pictureFilePath)
        {
            _pictureFilePath = pictureFilePath;
            _hasPictureAssigned = true;
            _isRevealed = false;
            _isMatched = false;
        }

        public string PictureFilePath
        {
            get { return _pictureFilePath; }
            set
            {
                _pictureFilePath = value;
                if (value != null)
                {
                    _hasPictureAssigned = true;
                    OnPropertyChanged(nameof(PictureFilePath));
                }
            }
        }

        [JsonIgnore]
        public ImageSource DisplayImage
        {
            get
            {
                string path = IsRevealed ? PictureFilePath : "Data/hidden.jpg";
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                return File.Exists(fullPath) ? new BitmapImage(new Uri(fullPath, UriKind.Absolute)) : null;
            }
        }

        public bool HasPictureAssigned
        {
            get { return _hasPictureAssigned; }
            set
            {
                _hasPictureAssigned = value;
                OnPropertyChanged(nameof(HasPictureAssigned));
            }
        }

        public bool IsRevealed
        {
            get { return _isRevealed; }
            set
            {
                if (_isRevealed != value)
                {
                    _isRevealed = value;
                    OnPropertyChanged(nameof(IsRevealed));
                    OnPropertyChanged(nameof(DisplayImage));
                }
            }
        }

        private bool _isMatched;

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                if (_isMatched != value)
                {
                    _isMatched = value;
                    OnPropertyChanged(nameof(IsMatched));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
