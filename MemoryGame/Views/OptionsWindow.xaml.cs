using MemoryGame.ViewModels;
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
using MemoryGame.Models;

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private User SelectedUser;
        public OptionsWindow(User selectedUser)
        {
            InitializeComponent();
            SelectedUser = selectedUser;

            var viewModel = new OptionsViewModel(SelectedUser);
            this.DataContext = viewModel;

            Rows.Text = "4";
            Cols.Text = "4";

        }

        private void File_Click(object sender, RoutedEventArgs e)
        {
            File.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Collapsed;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help.Visibility = Visibility.Visible;
            Options.Visibility = Visibility.Collapsed;
            File.Visibility = Visibility.Collapsed;
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            Options.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Collapsed;
            File.Visibility = Visibility.Collapsed;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
