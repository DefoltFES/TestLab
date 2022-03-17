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
                AppWindow window = new AppWindow(User);
                App.Current.MainWindow.Close();
                MessageBox.Show("Вы успешно вошли", "Потверждение", MessageBoxButton.OK);
                window.Show();

            }
            else
            {
                MessageBox.Show("Неправильно введенные данные ", "Ошибка",MessageBoxButton.OK);
            }
            
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text=String.Empty;
            (sender as TextBox).GotFocus -= TextBox_OnGotFocus;
        }


        /// <summary>
        /// Метод IsExist() осуществляет проверку введенных данных пользователем
        /// </summary> 
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
