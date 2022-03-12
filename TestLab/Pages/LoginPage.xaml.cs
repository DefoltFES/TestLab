using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TestLab.Windows;

namespace TestLab.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        private User User { get; set; }
        
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Enter_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsExist())
            {
                if (User.IdRole == 1)
                {
                    App.Current.MainWindow.Close();
                    AppWindow window = new AppWindow();
                    window.Show();
                }

                if (User.IdRole == 2)
                {
                    AppWindow window = new AppWindow();
                    window.Show();
                }
            }
            else
            {
                MessageBox.Show("Ошибка","Неправильно введенные данные",MessageBoxButton.OK);
            }
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text=String.Empty;
            (sender as TextBox).GotFocus -= TextBox_OnGotFocus;
        }


        public bool IsExist()
        {
            var user = DataBaseContext.DbContext.Users.Where(x => x.Login == Login.Text).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == Password.Text)
                {
                    User = user;
                    return true;
                }
            }
            return false;
        }




    }
}
