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
using Microsoft.Win32;
using TestLab.Class;
using TestLab.Model;

namespace TestLab.Pages
{
    /// <summary>
    /// Interaction logic for CreateTestPage.xaml
    /// </summary>
    public partial class CreateTestPage : Page
    {
        private User Creator { get; set; }
        private CreateTestClass Context { get; set; }


        public CreateTestPage(Test test,User user,bool isEdit=false)
        {
            InitializeComponent();
            Context = new CreateTestClass(test, user, isEdit);
            DataContext = Context;
            if (isEdit == true)
            {
                CountQuestions.Text = Convert.ToString(Context.GetProccent());
                if (Context.TimeTest != null)
                {
                    RadioButtonTimeTest.IsChecked = true;
                }
            }
        }

        private async void DeleteQuestions(object sender, RoutedEventArgs e)
        {
            var question = (sender as Button).DataContext as Question;
            if (question != null)
            {
                Context.Questions.Remove(ListQuestions.SelectedItem as Question);
                Context.UpdateNumbersAsync();
                if (Context.CountOfQuestions != 0)
                {
                    Context.CountOfQuestions -= 1;
                    if (Convert.ToInt32(CountQuestions.Text) - 1 >= 0)
                    {
                        CountQuestions.Text = (Convert.ToInt32(CountQuestions.Text) - 1).ToString();
                        Context.GetProccent(Convert.ToInt32(CountQuestions.Text));
                    }

                }
            }
        }

        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            Context.Questions.Add(new Question()
            {
                QuestionNumber = Context.Questions.Count() + 1
            });
            Context.CountOfQuestions = Context.Questions.Count;
            Context.GetProccent(Convert.ToInt32(CountQuestions.Text));
        }

        private void TypeQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Question question = (Question)ListQuestions.SelectedItem;
            if (Context.SelectedQuestion == null)
            {
                return;
            }
            if (Context.SelectedQuestion.QuestionType == null)
            {
                return;
            }

            if (Context.SelectedQuestion.QuestionType == 0)
            {
                AddAnswer.Visibility = Visibility.Visible;
                if (OneAnswear.Visibility == Visibility.Visible)
                {
                    OneAnswear.Visibility = Visibility.Collapsed;
                }
                ListManyAnswear.Visibility = Visibility.Collapsed;
                ListOneAnswear.Visibility = Visibility.Visible;
            }
            if (Context.SelectedQuestion.QuestionType == 1)
            {
                ListManyAnswear.Visibility = Visibility.Collapsed;
                ListOneAnswear.Visibility = Visibility.Collapsed;
                OneAnswear.Visibility = Visibility.Visible;
                AddAnswer.Visibility = Visibility.Collapsed;
                if (question != null)
                {
                    if (Context.SelectedQuestion.QuestionType == question.QuestionType)
                    {
                        Context.SelectedQuestion.Answers.Clear();
                        Context.SelectedQuestion.Answers.Add(new Answer()
                        {
                            IsCorrect = true
                        });
                    }
                }
            }
            if (Context.SelectedQuestion.QuestionType == 2)
            {
                AddAnswer.Visibility = Visibility.Visible;
                if (OneAnswear.Visibility == Visibility.Visible)
                {

                    OneAnswear.Visibility = Visibility.Collapsed;
                }
                ListOneAnswear.Visibility = Visibility.Collapsed;
                ListManyAnswear.Visibility = Visibility.Visible;
            }
        }

        private void DeleteAnswer_Click(object sender, RoutedEventArgs e)
        {

            if (ListManyAnswear.SelectedItem != null)
            {
                Context.SelectedQuestion.Answers.Remove(ListManyAnswear.SelectedItem as Answer);

            }
            if (ListOneAnswear.SelectedItem != null)
            {
                Context.SelectedQuestion.Answers.Remove(ListOneAnswear.SelectedItem as Answer);
            }
        }

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (ListQuestions.SelectedItem != null)
            {
                Context.SelectedQuestion.Answers.Add(new Answer()
                {
                });
            }
        }

        private void ListQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListQuestions.SelectedItem != null)
            {
                AddAnswer.IsEnabled = true;
                TypeQuestion.IsEnabled = true;
                QuestionText.IsEnabled = true;
                ChangeImage.IsEnabled = true;
            }
        }

        private void NoTimeLimit_Checked(object sender, RoutedEventArgs e)
        {
            if (Context != null)
            {
                Context.TimeTest = null;
            }
        }

        private void TimeTest_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Context.TimeTest == null)
            {
                Context.TimeTest = 1;
            }
        }

        private void TimeTest_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TimeTest_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (TimeTest.Text != "")
            {
                if (Convert.ToInt32(TimeTest.Text) > 180)
                {
                    Context.TimeTest = 180;
                }
            }
        }
        private void TimeTest_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TimeTest.Text == "" || TimeTest.Text == "0")
            {
                Context.TimeTest = 1;
            }
        }

        private void MinusOne_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(CountQuestions.Text);

            if (x - 1 < 0)
            {
                return;
            }
            else
            {
                CountQuestions.Text = (x - 1).ToString();
                Context.GetProccent(x - 1);
            }
        }

        private void PlusOne_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(CountQuestions.Text);
            if (x + 1 > Context.Questions.Count)
            {
                return;
            }
            else
            {
                CountQuestions.Text = (x + 1).ToString();
                Context.GetProccent(x + 1);
            }
        }



        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string warning = "Вы уверены что хотите закончить создание теста";
            if (Context.IsEdit)
            {
                warning = "Вы уверены что хотите закончить редактирования теста";
            }
            var result = MessageBox.Show(warning, "Предупреждение", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (Context.NameTest == null)
            {
                Name.BorderBrush = (SolidColorBrush)TryFindResource("ErrorButtonText");
                Name.Focus();
                return;
            }
            if (Context.Questions.Count(x => x.QuestionText == null || x.QuestionText == "" || x.Answers.Count == 0
                                             || x.Answers.Count(a => a.AnswerText == null || a.AnswerText == "") > 0) > 0)
            {
                MessageBox.Show("Проверьте заполнение вопросов", "Предупреждение");
                return;
            }
            if (Context.PercentRightQuestions != null && Context.PercentRightQuestions != 0)
            {
                await Context.SaveChangesAsync();
                this.NavigationService.GoBack();
            }
            else
            {
                CountQuestions.BorderBrush = (SolidColorBrush)TryFindResource("ErrorButtonText");
                CountQuestions.Focus();
            }
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Context.SelectedQuestion.QuestionType == 0)
            {
                foreach (var item in Context.SelectedAnswers)
                {
                    if (item != (sender as RadioButton).DataContext as Answer)
                    {
                        item.IsCorrect = false;
                    }
                }
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void ChangeImage_OnClick(object sender, RoutedEventArgs e)
        {
            Context.ChangeImage();

        }

        private void ResetImage_OnClick(object sender, RoutedEventArgs e)
        {
            Context.SelectedQuestion.Image=null;
        }
    }
}
