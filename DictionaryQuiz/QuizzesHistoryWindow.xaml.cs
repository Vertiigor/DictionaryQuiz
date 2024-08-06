using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;
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
using System.IO;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for QuizzesHistoryWindow.xaml
    /// </summary>
    public partial class QuizzesHistoryWindow : Window
    {
        private string configFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Configuration", "config.json");
        private string historyFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Data", "quizzesHistory.json");
        private ConfigurationRoot configuration;
        private ConfigurationLoader configurationLoader;
        private QuizzesLoader quizzesLoader;
        private List<Quiz> quizzes;

        public QuizzesHistoryWindow()
        {
            InitializeComponent();

            configurationLoader = new ConfigurationLoader();
            configuration = configurationLoader.LoadConfiguration(configFilePath);
            quizzesLoader = new QuizzesLoader(configuration);

            quizzes = quizzesLoader.LoadData(historyFilePath);

            foreach (var quiz in quizzes)
            {
                var newItem = new ListBoxItem();
                newItem.Content = $"{quiz.Date}";

                QuizzesListBox.Items.Add(newItem);
            }
        }

        private void QuizzesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var quiz = quizzes[QuizzesListBox.SelectedIndex];
            FillDetailsComponents(quiz);
        }

        private void FillDetailsComponents(Quiz quiz)
        {
            DateLabel.Content = $"Date: {quiz.Date}";
            LanguageLabel.Content = $"Language: {quiz.Language}";
            QuestionsCountLabel.Content = $"Questions count: {quiz.QuestionsCount}";
            CorrectAnswersCountLabel.Content = $"Correct answers count: {quiz.CorrectAnswers.Count}";
            IncorrectAnswersCountLabel.Content = $"Incorrect answers count: {quiz.IncorrectAnswers.Count}";
            ConclusionLabel.Content = $"Conclusion: {quiz.Conclusion}";

            CorrectAnswersExpander.Content = string.Join("\n", quiz.CorrectAnswers);
            IncorrectAnswersExpander.Content = string.Join("\n", quiz.IncorrectAnswers);
        }
    }
}
