using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using MemoryGame.Models;

namespace MemoryGame.ViewModels
{
    public enum PictureSet
    {
        Animals,
        Cars,
        Food
    }

    public class OptionsViewModel : INotifyPropertyChanged
    {
        private User _selectedUser;
        private PictureSet _picSet;
        public OptionsViewModel(User selectedUser)
        {
            _picSet = PictureSet.Animals;
            _selectedUser = selectedUser;
            _rows = 4;
            _cols = 4;
            OpenNewGameCommand = new RelayCommand(OpenNewGame);
            SetStandardGameCommand = new RelayCommand(SetStandardGame);
            ToggleCategoryCommand = new RelayCommand(_ => ShowCategories = !ShowCategories);
            SelectCategoryCommand = new RelayCommand(param =>
            {
                if (param is string category && Enum.TryParse<PictureSet>(category, out var pic))
                {
                    PicSet = pic;
                    ShowCategories = false;
                }
            });
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

        public PictureSet PicSet
        {
            get { return _picSet; }
            set
            {
                if (_picSet != value)
                {
                    _picSet = value;
                    OnPropertyChanged(nameof(PicSet));
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
        public ICommand ToggleCategoryCommand { get; }
        public ICommand SelectCategoryCommand { get; }

        private void OpenNewGame(object parameter)
        {
            GameWindow gameWindow = new GameWindow(SelectedUser, Rows, Cols, PicSet);
            gameWindow.ShowDialog();
        }

        private void SetStandardGame(object parameter)
        {
            Rows = 4;
            Cols = 4;
        }

        private bool _showCategories;
        public bool ShowCategories
        {
            get => _showCategories;
            set
            {
                _showCategories = value;
                OnPropertyChanged(nameof(ShowCategories));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
