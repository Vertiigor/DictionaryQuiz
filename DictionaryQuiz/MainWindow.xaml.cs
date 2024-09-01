using DictionaryQuiz.Factories;
using DictionaryQuiz.Models;
using DictionaryQuiz.Services;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dictionaryFilePath;
        private string configFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Configuration", "config.json");
        private ConfigurationRoot configuration;
        private ServicesFactory servicesFactory;
        private LanguageEntitiesService languageEntitiesService;
        private ConfigurationService configurationService;
        private QuizzesService quizzesService;
        private Quiz currentQuiz;
        private LanguageEntity currentWord;
        private LanguageDefinition currentLanguage;
        private int QuestionsCounter = 0;
        private List<LanguageEntity> completedWords;

        public MainWindow()
        {
            InitializeComponent();

            dictionaryFilePath = string.Empty;
            configurationService = new ConfigurationService();
            configuration = configurationService.Load(configFilePath);
            servicesFactory = new ServicesFactory(configuration);
            languageEntitiesService = servicesFactory.CreateService<LanguageEntity>() as LanguageEntitiesService;
            currentQuiz = new Quiz();
            currentWord = new LanguageEntity();
            currentLanguage = new LanguageDefinition();
            quizzesService = servicesFactory.CreateService<Quiz>() as QuizzesService;

            foreach (var language in configuration.Languages)
            {
                var newItem = new ListBoxItem();
                newItem.Content = $"{language.Name}";

                LanguagesListBox.Items.Add(newItem);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string? selectedLanguage = LanguagesListBox.SelectionBoxItem.ToString();

            if (selectedLanguage == string.Empty || selectedLanguage == null)
            {
                MessageBox.Show("Choose the language!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ShowOpenFileDialog("Comma delimited (*.csv)|*.csv", SetNewDictionaryFilePath);

            StartNew.IsEnabled = true;
        }

        private void QuizPreferences_Click(object sender, RoutedEventArgs e)
        {
            QuizPreferencesWindow preferencesWindow = new QuizPreferencesWindow();
            preferencesWindow.Show();
        }

        private void LanguagePreferences_Click(object sender, RoutedEventArgs e)
        {
            LanguagePreferencesWindow preferencesWindow = new LanguagePreferencesWindow();
            preferencesWindow.Show();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (InputTextField.Text.ToUpper() == currentWord.AdditionalFields[currentLanguage.RequiredInput].ToUpper())
            {
                MessageBox.Show("Correct answer", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);
                currentQuiz.CorrectAnswers.Add($"{currentWord.Word} - {currentWord.AdditionalFields[currentLanguage.RequiredInput]}");
            }
            else
            {
                MessageBox.Show($"Incorrect answer\nThe correct answer is \"{currentWord.AdditionalFields[currentLanguage.RequiredInput]}\"", "Be vigilant!", MessageBoxButton.OK, MessageBoxImage.Error);
                currentQuiz.IncorrectAnswers.Add($"{currentWord.Word} - {InputTextField.Text}");
            }

            completedWords.Add(currentWord);

            InputTextField.Text = string.Empty;

            if (QuestionsCounter == configuration.QuizPreferences.QuestionsCount)
            {
                MessageBox.Show("You've completed this quiz");

                EvaluateQuiz();

                quizzesService.SaveQuiz(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Data", "quizzesHistory.json"), currentQuiz);
                StartNew_Click(sender, e);

                return;
            }

            QuestionsCounter++;

            SetNextWord();
        }

        private void EvaluateQuiz()
        {
            var percent = currentQuiz.CorrectAnswers.Count * 100 / (currentQuiz.IncorrectAnswers.Count + currentQuiz.CorrectAnswers.Count);

            if (Enumerable.Range(0, 40).Contains(percent))
            {
                currentQuiz.Conclusion = "Bad";
            }
            if (Enumerable.Range(40, 65).Contains(percent))
            {
                currentQuiz.Conclusion = "Satisfactorily";
            }
            if (Enumerable.Range(65, 85).Contains(percent))
            {
                currentQuiz.Conclusion = "Good";
            }
            if (Enumerable.Range(85, 100).Contains(percent))
            {
                currentQuiz.Conclusion = "Perfect!";
            }
        }

        private void DontKnowButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"The correct answer is: {currentWord.AdditionalFields[currentLanguage.RequiredInput]}", "Attention!", MessageBoxButton.OK, MessageBoxImage.Information);
            SetNextWord();
            InputTextField.Text = string.Empty;
        }

        private void StartNew_Click(object sender, RoutedEventArgs e)
        {
            MakeInputComponentsVisible();

            string? selectedLanguage = LanguagesListBox.SelectionBoxItem.ToString();
            SetNewLanguage(selectedLanguage);

            QuestionsCounter = 1;
            completedWords = new List<LanguageEntity>();

            SetNewQuiz();

            SetNextWord();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            QuizzesHistoryWindow historyWindow = new QuizzesHistoryWindow();
            historyWindow.Show();
        }

        private void LanguagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsingLanguageLabel.Content = "You're using: ";
        }

        private LanguageEntity GetRandomWord()
        {
            var records = languageEntitiesService.GetAll(dictionaryFilePath).ToList();

            if (configuration.QuizPreferences.AllowDuplicates == false)
            {
                foreach (var completed in completedWords)
                {
                    records = records.Where(x => x.Word != completed.Word).ToList();
                }
            }

            Random rand = new Random();
            var word = records[rand.Next(0, records.Count)];

            return word;
        }

        private void SetNewLanguage(string selectedLanguage)
        {
            configuration = configurationService.Load(configFilePath);
            languageEntitiesService.Language = selectedLanguage;
            currentLanguage = configuration.Languages.First(x => x.Name == selectedLanguage);
        }

        private void MakeInputComponentsVisible()
        {
            HintLabel.Visibility = Visibility.Visible;
            CheckButton.Visibility = Visibility.Visible;
            DontKnowButton.Visibility = Visibility.Visible;
            InputTextField.Visibility = Visibility.Visible;
            QuestionsCountLabel.Visibility = Visibility.Visible;
        }

        private void SetNewDictionaryFilePath(string fileName)
        {
            dictionaryFilePath = fileName;
            DictionaryFilePathLabel.Content = $"You're currently using this dictionary file: {dictionaryFilePath}";
        }

        private void ShowOpenFileDialog(string filter, Action<string> callback)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;

            if (openFileDialog.ShowDialog() == true)
            {
                callback(openFileDialog.FileName);
            }
        }

        private void SetNewQuiz()
        {
            currentQuiz = new Quiz();
            currentQuiz.Date = DateTime.Now.ToString();
            currentQuiz.Language = currentLanguage.Name;
            currentQuiz.QuestionsCount = configuration.QuizPreferences.QuestionsCount;
        }

        private void SetNextWord()
        {
            currentWord = GetRandomWord();
            WordContent.Content = $"{currentWord.Word}";
            QuestionsCountLabel.Content = $"{QuestionsCounter} / {configuration.QuizPreferences.QuestionsCount}";
            HintLabel.Content = $"Type a {currentLanguage.RequiredInput}";
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void DeveloperInfo_Click(object sender, RoutedEventArgs e)
        {
            DeveloperInfoWindow developerInfoWindow = new DeveloperInfoWindow();
            developerInfoWindow.Show();
        }
    }
}