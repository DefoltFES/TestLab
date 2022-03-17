using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLab.Model;

namespace TestLab.Class
{
    class PassTestClass
    {
        private UserResult userResults = new UserResult();
        private Test test;
        private User user;
        private List<Question> _questions;
        private DateTime timeBegin;

        public string NameTest
        {
            get => test.Name;
        }

        public string DescriptionTest
        {
            get => test.Description;
        }

        public List<Question> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }

        public int? Time
        {
            get { return test.Time; }
        }

        public PassTestViewModel(Test test, User user)
        {
            this.user = user;
            this.test = test;
            Questions = new List<Question>();
            userResults.TimeBegin = DateTime.Now;
            foreach (var question in test.Questions)
            {
                List<Answer> answers = question.Answers.Select(x => new Answer()
                {
                    AnswerText = x.AnswerText,
                    IsCorrect = false,
                    Question = x.Question
                }).ToList();
                Questions.Add(new Question()
                {
                    Answers = new ObservableCollection<Answer>(answers),
                    QuestionText = question.QuestionText,
                    QuestionType = question.QuestionType,
                    QuestionNumber = question.QuestionNumber

                });
            }

            foreach (var question in Questions)
            {
                if (question.QuestionType == 1)
                {
                    question.Answers[0].AnswerText = "";
                }
            }

        }

        public async Task SaveChangesAsync()
        {
            await Task.Run(() => SaveChanges());
        }



        private void SaveChanges()
        {
            userResults.Test = test;
            userResults.User = user;
            userResults.count_total_questions = test.count_of_questions;
            userResults.test_title = test.name;
            userResults.time_end = DateTime.Now;
            userResults.count_right_questions = 0;
            foreach (var question in Questions)
            {
                var testAnswers = test.questions.Where(x => x.question_number == question.question_number)
                    .Select(x => x.answers).First().ToList();
                var userAnswers = question.answers.ToList();
                List<answer> result = new List<answer>();
                if (question.question_type == 0 || question.question_type == 2)
                {
                    for (int i = 0; i < testAnswers.Count; i++)
                    {
                        if (userAnswers[i].is_correct != testAnswers[i].is_correct)
                        {
                            result.Add(userAnswers[i]);
                        }
                    }
                }
                var userAnswer = new user_answers()
                {
                    is_answered = true,
                    question = question.question_text,
                    qnumber = question.question_number

                };
                if (testAnswers[0].answers_text.ToLower() != userAnswers[0].answers_text.ToLower() &&
                    question.question_type == 1)
                {
                    userAnswer.is_right = false;
                }
                else
                {
                    userAnswer.is_right = true;
                }

                if (question.question_type == 0 || question.question_type == 2)
                {
                    if (result.Count != 0)
                    {
                        userAnswer.is_right = false;
                    }
                    else
                    {
                        userAnswer.is_right = true;
                    }
                }


                if (userAnswer.is_right == true)
                {
                    userResults.count_right_questions += 1;
                }

                foreach (var item in userAnswers)
                {
                    if (item.is_correct == true || question.question_type == 1)
                    {
                        userAnswer.answear += $"{item.answers_text} ";

                    }

                }
                userResults.user_answers.Add(userAnswer);

            }

            userResults.percent_right_questions = (float)(userResults.count_right_questions * 100 / userResults.count_total_questions) / 100;
            if (userResults.percent_right_questions >= test.percent_right_questions)
            {
                userResults.is_passed = true;
            }
            else
            {
                userResults.is_passed = false;
            }
            if ((userResults.time_end - userResults.time_begin).Value.Minutes > test.time)
            {
                userResults.is_passed = false;
            }
            App.dbContext.user_results.Add(userResults);

        }
    }
}
