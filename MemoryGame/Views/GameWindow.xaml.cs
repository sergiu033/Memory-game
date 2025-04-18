﻿using MemoryGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MemoryGame.Models;

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private User _selectedUser;
        private int _rows;
        private int _cols;
        private PictureSet _pictureSet;
        private string _jsonFile;
        public GameWindow(User selectedUser, int rows, int cols, PictureSet set)
        {
            InitializeComponent();

            _selectedUser = selectedUser;
            _rows = rows;
            _cols = cols;
            _pictureSet = set;

            var viewModel = new GameViewModel(_selectedUser, _rows, _cols, _pictureSet);
            this.DataContext = viewModel;
        }

        public GameWindow(User selecteduser, string jsonFile)
        {
            InitializeComponent();

            _selectedUser = selecteduser;
            _jsonFile = jsonFile;

            var viewModel = new GameViewModel(_jsonFile, _selectedUser);
            this.DataContext = viewModel;
        }
    }
}
