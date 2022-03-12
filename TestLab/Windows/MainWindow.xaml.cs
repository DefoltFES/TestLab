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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestLab.Model;
using TestLab.Pages;

namespace TestLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.NavigationService.Navigate(new RegisterPage(new User()));
            Register.Content = "Назад";
            Register.Click -= Register_OnClick;
            Register.Click += Back_OnClick;
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
           MainWindowFrame.NavigationService.GoBack();
           Register.Click += Register_OnClick;
           Register.Click -= Back_OnClick;
           Register.Content = "Регистрация";
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content=new LoginPage();
        }
    }
}
