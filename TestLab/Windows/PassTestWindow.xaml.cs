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

namespace TestLab.Windows
{
    /// <summary>
    /// Interaction logic for PassTestWindow.xaml
    /// </summary>
    public partial class PassTestWindow : Window
    {


        public PassTestWindow()
        {
            InitializeComponent();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите закончить тестирование ?", "Предупреждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }

            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                await Context.SaveChangesAsync();

            }


        }

        private async void PassClick(object sender, RoutedEventArgs e)
        {

            DialogResult = true;

        }

        private void RadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            var button = (sender as RadioButton).DataContext as Answer;
            if (button.question.question_type == 0)
            {
                foreach (var item in Context.Questions.Where(x => x.question_number == button.question.question_number).First().answers)
                {
                    if (item != button)
                    {
                        item.is_correct = false;
                    }
                }
            }
        }
    }
}
