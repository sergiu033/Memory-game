using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace MemoryGame.ViewModels
{
    public class OptionsViewModel : INotifyPropertyChanged
    {
        private User _selectedUser;
        public OptionsViewModel(User selectedUser)
        {
            _selectedUser = selectedUser;
            _rows = 4;
            _cols = 4;
            OpenNewGameCommand = new RelayCommand(OpenNewGame);
            SetStandardGameCommand = new RelayCommand(SetStandardGame);
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        private int _rows;
        private int _cols;

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

        public ICommand OpenNewGameCommand { get; }
        public ICommand SetStandardGameCommand { get; }

        private void OpenNewGame(object parameter)
        {
            GameWindow gameWindow = new GameWindow(SelectedUser, Rows, Cols);
            gameWindow.ShowDialog();
        }

        private void SetStandardGame(object parameter)
        {
            Rows = 4;
            Cols = 4;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
