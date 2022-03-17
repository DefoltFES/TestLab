using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using TestLab.Annotations;
using TestLab.Model;

namespace TestLab.Class
{
    class CreateTestClass:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Test test;
        private User user;
        private ObservableCollection<Question> _questions;
        private Question _selectedQuestion;
        private Test editTest;

        public bool IsEdit
        {
            get;
            set;
        }

        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Answer> SelectedAnswers
        {
            get { return _selectedQuestion.Answers; }
            set
            {
                _selectedQuestion.Answers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Question> Questions
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
                OnPropertyChanged();
            }
        }

        

        public string NameTest
        {
            get { return test.Name; }
            set
            {
                test.Name = value;
                OnPropertyChanged();
            }
        }

        public string DescriptionTest
        {
            get { return test.Description; }
            set
            {
                test.Description = value;
                OnPropertyChanged();
            }
        }
        public int? TimeTest
        {
            get { return test.Time; }
            set
            {
                test.Time = value;
                OnPropertyChanged();
            }
        }
        public int? CountOfQuestions
        {
            get { return test.CountOfQuestions; }
            set
            {
                test.CountOfQuestions = value;
                OnPropertyChanged();
            }
        }

        public double? PercentRightQuestions
        {
            get { return test.PercentRightQuestions; }
            set
            {
                test.PercentRightQuestions = value;
                OnPropertyChanged();
            }
        }
        public int IdCreator { get; set; }

        public CreateTestClass(Test test, User user, bool edit = false)
        {

            this.user = user;
            IdCreator = user.Id;
            if (edit == false)
            {
                this.test = test;
                Questions = new ObservableCollection<Question>(test.Questions);
            }
            if (edit == true)
            {
                var Test = new Test()
                {
                    Time = test.Time,
                    Name = test.Name,
                    CountOfQuestions = test.CountOfQuestions,
                    Description = test.Description,
                    PercentRightQuestions = test.PercentRightQuestions
                };
                foreach (var question in test.Questions)
                {
                    List<Answer> answers = question.Answers.Select(x => new Answer()
                    {
                        AnswerText = x.AnswerText,
                        IsCorrect = x.IsCorrect
                    }).ToList();
                    Test.Questions.Add(new Question()
                    {
                        Answers = new ObservableCollection<Answer>(answers),
                        QuestionText = question.QuestionText,
                        QuestionType = question.QuestionType,
                        QuestionNumber = question.QuestionNumber

                    });
                }
                editTest = test;
                this.test = Test;
                Questions = new ObservableCollection<Question>(this.test.Questions.ToList());
            }
            IsEdit = edit;
            

        }

        public async Task UpdateNumbersAsync()
        {
            await Task.Run(() => UpdateNumbers());
        }

        private void UpdateNumbers()
        {
            foreach (var question in Questions)
            {
                if (question.QuestionNumber - 1 != 1 && question.QuestionNumber - 1 != 0)
                {
                    question.QuestionNumber -= 1;
                }

            }
        }

        public async Task SaveChangesAsync()
        {
            await Task.Run(() => SaveChanges());
        }

        private void SaveChanges()
        {
            if (IsEdit == true)
            {
                editTest.Name = NameTest;
                editTest.Description = DescriptionTest;
                editTest.CountOfQuestions = CountOfQuestions;
                editTest.PercentRightQuestions = PercentRightQuestions;
                editTest.Time = TimeTest;
                editTest.Questions = Questions.ToList();
                DataBaseContext.DbContext.Tests.Attach(editTest);
                DataBaseContext.DbContext.Entry(editTest).State = EntityState.Modified;
                DataBaseContext.DbContext.SaveChanges();
            }
            if (IsEdit == false)
            {
                test.Questions = Questions.ToList();
                test.IdCreator = IdCreator;
                DataBaseContext.DbContext.Tests.Add(test);
                DataBaseContext.DbContext.SaveChanges();
            }

        }

        public void GetProccent(int number)
        {
            if (number != 0)
            {
                PercentRightQuestions = (float)(number * 100 / CountOfQuestions) / 100;
            }
            else
            {
                PercentRightQuestions = 0;
            }
        }

        public int GetProccent()
        {
            if (PercentRightQuestions == 1)
            {
                return (int)CountOfQuestions;
            }
            return (int)Math.Round((double)(CountOfQuestions * PercentRightQuestions), 1);
        }


        public BitmapImage GetBitmapImage(byte[] byteArr)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(byteArr);
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

        public void ChangeImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image File (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg; *.jpg;*.bmp;*.png",
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (SelectedQuestion == null)
                {
                    return;
                }
                SelectedQuestion.Image  = ImageToByte(new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute)));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
