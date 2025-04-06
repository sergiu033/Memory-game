using MemoryGame.ViewModels;
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
        public GameWindow(User selectedUser, int rows, int cols)
        {
            InitializeComponent();

            _selectedUser = selectedUser;
            _rows = rows;
            _cols = cols;

            var viewModel = new GameViewModel(_selectedUser, _rows, _cols);
            this.DataContext = viewModel;
        }
    }
}
