﻿using MemoryGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private Action refreshInSignUp;
        public AddUserWindow(Action refreshAction)
        {
            InitializeComponent();
            refreshInSignUp = refreshAction;

            var viewModel = new AddUserViewModel(refreshInSignUp);
            this.DataContext = viewModel;
        }
    }
}
