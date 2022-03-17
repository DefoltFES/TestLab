using System;
using System.Collections.Generic;
using System.IO;
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
using System.Drawing;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using TestLab.Class;
using TestLab.Model;

namespace TestLab.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public User User { get; set; }
        public ProfilePage(User user)
        {
            InitializeComponent();
            this.User = user;
            Name.Text = User.Name;
            Surname.Text = User.Surname;
            Middlename.Text = User.Middlename;
            if (User.Image == null)
            {
                ProfileImage.Source = (BitmapImage)TryFindResource("DefoltImage");
            }
            else
            {
                ProfileImage.Source = GetBitmapImage(User.Image);
            }

        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            Name.IsReadOnly = false;
            Surname.IsReadOnly = false;
            Middlename.IsReadOnly=false;
            (sender as Button).Content = "Сохранить";
            ChangeImage.Click += ChangeImage_OnClick;
            (sender as Button).Click += Save_OnClick;

        }
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var messageBox = MessageBox.Show("Вы хотите сохранить данные", "Предупреждение", MessageBoxButton.YesNo);
            if (messageBox == MessageBoxResult.Yes)
            {
                User.Name = Name.Text;
                User.Surname = Surname.Text;
                User.Middlename = Middlename.Text;
                User.Image = ImageToByte(ProfileImage.Source as BitmapImage);
            }
            else
            {
                if (User.Image == null)
                {
                    ProfileImage.Source = (BitmapImage)TryFindResource("DefoltImage");
                }
                else
                {
                    ProfileImage.Source = GetBitmapImage(User.Image);
                }
            }
            (sender as Button).Content = "Изменить";
            ChangeImage.Click -= ChangeImage_OnClick;
            DataBaseContext.DbContext.SaveChanges();

        }

        public  BitmapImage GetBitmapImage(byte[] byteArr)
        {
            var bitmapImage=new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource= new MemoryStream(byteArr);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public byte[] ImageToByte(BitmapImage image)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        public void ChangeImage_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image File (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg; *.jpg;*.bmp;*.png",
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProfileImage.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
            }

        }

        
    }
}
