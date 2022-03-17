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
using TestLab.Class;
using TestLab.Model;

namespace TestLab.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private User User { get; set; }
        public TestPage(User user)
        {
            InitializeComponent();
            User = user;
        }

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreateTestPage(new Test(), User));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var test = (sender as Button).DataContext as Test;
            if (test != null)
            {
                NavigationService.Navigate(new CreateTestPage(ListTests.SelectedItem as Test, User, true));
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            var listUsers=MessageBox.Show("","",MessageBoxButton.YesNo);
            var test = (sender as Button).DataContext as Test;
            if (listUsers==MessageBoxResult.Yes)
            {
                if (test != null)
                {
                    DataBaseContext.DbContext.Questions.RemoveRange(test.Questions);
                    DataBaseContext.DbContext.Tests.Remove(test);
                    DataBaseContext.DbContext.SaveChangesAsync();
                    ListTests.ItemsSource = DataBaseContext.DbContext.Tests.ToList();

                }
            }
        }

        private void TestPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            ListTests.ItemsSource = DataBaseContext.DbContext.Tests.ToList();
        }
    }
}
