using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private User _selectedUser;
        private int _rows;
        private int _cols;

        public GameViewModel(User selectedUser, int rows, int cols)
        {
            _selectedUser = selectedUser;
            _rows = rows;
            _cols = cols;
        }

        public int Rows
        {
            get { return _rows; }
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    OnPropertyChanged(nameof(Rows));
                }
            }
        }

        public int Cols
        {
            get { return _cols; }
            set
            {
                if (_cols != value)
                {
                    _cols = value;
                    OnPropertyChanged(nameof(Cols));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


    
