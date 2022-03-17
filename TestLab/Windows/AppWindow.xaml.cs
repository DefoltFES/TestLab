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
using TestLab.Model;
using TestLab.Pages;

namespace TestLab.Windows
{
    /// <summary>
    /// Interaction logic for AppWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        private User user;

        public AppWindow(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void AppWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
           FramePage.Content=new ProfilePage(user);
        }

        private void TestsPage_OnClick(object sender, RoutedEventArgs e)
        {
            FramePage.Content = new TestPage(user);
        }

        private void Profile_OnClick(object sender, RoutedEventArgs e)
        {
            FramePage.Content = new ProfilePage(user);
        }

        private void Result_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
