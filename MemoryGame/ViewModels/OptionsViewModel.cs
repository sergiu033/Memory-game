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
using Microsoft.Win32;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.IO;

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
        private string _baseLoadingPath;
        private string _selectedLoadingJson;

        public OptionsViewModel(User selectedUser)
        {
            _picSet = PictureSet.Animals;
            _baseLoadingPath = "Saves/" + selectedUser.UserName;
            _selectedUser = selectedUser;
            _rows = 4;
            _cols = 4;
            OpenNewGameCommand = new RelayCommand(OpenNewGame);
            SetStandardGameCommand = new RelayCommand(SetStandardGame);
            OpenSavedGameCommand = new RelayCommand(OpenSavedGame);
            ToggleCategoryCommand = new RelayCommand(_ => ShowCategories = !ShowCategories);
            SelectCategoryCommand = new RelayCommand(param =>
            {
                if (param is string category && Enum.TryParse<PictureSet>(category, out var pic))
                {
                    PicSet = pic;
                    ShowCategories = false;
                }
            });
            ToggleStatisticsCommand = new RelayCommand(_ => ShowStatistics = !ShowStatistics);
        }

        public String SelectedLoadingJson
        {
            get { return _selectedLoadingJson; }
            set
            {
                if (_selectedLoadingJson != value )
                    {
                        _selectedLoadingJson = value;
                        OnPropertyChanged(nameof(SelectedLoadingJson));
                    }
            }
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
        public ICommand ToggleStatisticsCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand OpenSavedGameCommand { get; }

        private void OpenNewGame(object parameter)
        {
            _selectedUser.GamesPlayed++;
            int gamesPlayed = _selectedUser.GamesPlayed;
            IncreaseGamesPlayed(gamesPlayed);
            GameWindow gameWindow = new GameWindow(SelectedUser, Rows, Cols, PicSet);
            gameWindow.ShowDialog();
        }

        private void SetStandardGame(object parameter)
        {
            Rows = 4;
            Cols = 4;
        }

        private void OpenSavedGame(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select Save File",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = "Saves" 
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                GameWindow gameWindow = new GameWindow(SelectedUser, selectedFilePath);
                gameWindow.ShowDialog();
            }
        }

        private void IncreaseGamesPlayed(int gamesPlayed)
        {
            string filePath = "Data/users.json";

            string jsonString = File.ReadAllText(filePath);
            JsonNode? root = JsonNode.Parse(jsonString);

            if (root is JsonArray people)
            {
                foreach (var personNode in people)
                {
                    if (personNode is JsonObject person && person["userName"]?.ToString() == _selectedUser.UserName)
                    {
                        person["gamesPlayed"] = gamesPlayed;
                        break;
                    }
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(filePath, root.ToJsonString(options));
            }
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

        private bool _showStatistics;
        public bool ShowStatistics
        {
            get => _showStatistics;
            set
            {
                _showStatistics = value;
                OnPropertyChanged(nameof(ShowStatistics));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
