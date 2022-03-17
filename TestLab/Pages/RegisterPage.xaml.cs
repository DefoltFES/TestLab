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
using TestLab.Windows;

namespace TestLab.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private User RegisterUser { get; set; } 
        public RegisterPage(User user)
        {
            InitializeComponent();
            RegisterUser = user;
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = String.Empty;
            (sender as TextBox).GotFocus -= TextBox_OnGotFocus;
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            Login.Focus();
            Password.Focus();
            Name.Focus();
            Surname.Focus();
            Middlename.Focus();
            Password.Focus();
            if (Login.Text == "" || Password.Text == "" || Name.Text == "" || Surname.Text == "" ||
                Middlename.Text == "")
            {
                MessageBox.Show("Проверьте заполненность данных", "Предупреждение", MessageBoxButton.OK);
            }
            if (CheckMail())
            {
                var messageBox = MessageBox.Show("Вы точно хотите зарегестрироваться ?", "Предупреждение",
                    MessageBoxButton.YesNo);
                
                AppWindow window = new AppWindow(RegisterUser);
                
                if (Creator.IsChecked == true)
                {
                    RegisterUser.IdRole = 2;

                }
                if (User.IsChecked == true)
                {
                    RegisterUser.IdRole = 1;
                }

                if (messageBox == MessageBoxResult.Yes)
                {
                    RegisterUser.Name = Name.Text;
                    RegisterUser.Surname = Surname.Text;
                    RegisterUser.Middlename = Middlename.Text;
                    RegisterUser.Login = Login.Text;
                    RegisterUser.Password = Password.Text;
                    DataBaseContext.DbContext.Users.Add(RegisterUser);
                    DataBaseContext.DbContext.SaveChanges();
                    App.Current.MainWindow.Close();
                    window.Show();
                }

            }
            else
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "Предупреждение", MessageBoxButton.OK);
            }
        }

        private bool CheckMail()
        {
            if (DataBaseContext.DbContext.Users.Where(x => x.Login == Login.Text).FirstOrDefault() != null)
            {
                return false;
            }

            return true;
        }
    }
}
