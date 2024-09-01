using DictionaryQuiz.Factories;
using DictionaryQuiz.Models;
using DictionaryQuiz.Services;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
        private ServicesFactory servicesFactory;
        private ConfigurationService configurationService;
        private QuizzesService quizzesService;
        private List<Quiz> quizzes;

        public QuizzesHistoryWindow()
        {
            InitializeComponent();

            configurationService = new ConfigurationService();
            configuration = configurationService.Load(configFilePath);
            servicesFactory = new ServicesFactory(configuration);
            quizzesService = servicesFactory.CreateService<Quiz>() as QuizzesService;

            quizzes = quizzesService.GetAll(historyFilePath).ToList();

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
