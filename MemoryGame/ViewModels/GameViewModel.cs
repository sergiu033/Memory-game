using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MemoryGame.Models;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private User _selectedUser;
        private int _rows;
        private int _cols;
        private PictureSet _pictureSet;
        private GameGrid _grid;

        public GameViewModel(User selectedUser, int rows, int cols, PictureSet pictureSet)
        {
            _selectedUser = selectedUser;
            _rows = rows;
            _cols = cols;
            _pictureSet = pictureSet;
            _grid = new GameGrid(_rows, _cols, _pictureSet);

            FlipCardCommand = new RelayCommand(FlipCard);
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

        public GameGrid Grid
        {
            get { return _grid; }
            set
            {
                if (value != _grid)
                {
                    _grid = value;
                    OnPropertyChanged(nameof(Grid));
                }
            }
        }

        public ICommand FlipCardCommand { get; }

        private GridCell _firstFlippedCell;
        private bool _isCheckingMatch;
        private User selectedUser;
        private PictureSet pictureSet;

        private async void FlipCard(object parameter)
        {
            if (_isCheckingMatch || parameter is not GridCell cell || cell.IsRevealed || cell.IsMatched)
                return;

            cell.IsRevealed = true;

            if (_firstFlippedCell == null)
            {
                _firstFlippedCell = cell;
            }
            else
            {
                _isCheckingMatch = true;

                await Task.Delay(1000);

                if (_firstFlippedCell.PictureFilePath == cell.PictureFilePath)
                {
                    _firstFlippedCell.IsMatched = true;
                    cell.IsMatched = true;

                    CheckForWin();
                }
                else
                {
                    _firstFlippedCell.IsRevealed = false;
                    cell.IsRevealed = false;
                }

                _firstFlippedCell = null;
                _isCheckingMatch = false;
            }
        }

        private void CheckForWin()
        {
            if (Grid.Cells.All(cell => cell.IsMatched))
            {
                MessageBox.Show("You win!", "Memory Game", MessageBoxButton.OK, MessageBoxImage.Information);

                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.DataContext == this)
                    ?.Close();
            }
        }

        public IEnumerable<GridCell> Cells => _grid.Cells;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}