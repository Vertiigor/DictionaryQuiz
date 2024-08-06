using DictionaryQuiz.Factories;
using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using DictionaryQuiz.Savers;

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
        private DataLoaderFactory<LanguageEntity> languageEntitiesFactory;
        private DataLoader<LanguageEntity> languageEntitiesLoader;
        private ConfigurationLoader configurationLoader;
        private QuizSaver quizSaver;
        private Quiz currentQuiz;
        private LanguageEntity currentWord;
        private LanguageDefinition currentLanguage;
        private int QuestionsCounter = 0;

        public MainWindow()
        {
            InitializeComponent();

            dictionaryFilePath = string.Empty;
            configurationLoader = new ConfigurationLoader();
            configuration = configurationLoader.LoadConfiguration(configFilePath);
            languageEntitiesFactory = new LanguageEntitiesDataLoaderFactory(configuration, "English");
            languageEntitiesLoader = languageEntitiesFactory.CreateLoader();
            currentQuiz = new Quiz();
            currentWord = new LanguageEntity();
            currentLanguage = new LanguageDefinition();
            quizSaver = new QuizSaver();

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
                MessageBox.Show("Correct", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);
                currentQuiz.CorrectAnswers.Add(currentWord.Word);
            }
            else
            {
                MessageBox.Show("Incorrect", "Be vigilant!", MessageBoxButton.OK, MessageBoxImage.Error);
                currentQuiz.IncorrectAnswers.Add(currentWord.Word);
            }

            InputTextField.Text = string.Empty;

            if (QuestionsCounter == configuration.QuizPreferences.QuestionsCount)
            {
                MessageBox.Show("P L A C E H O L D E R");
                quizSaver.Save(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Data", "quizzesHistory.json"), currentQuiz);
                StartNew_Click(sender, e);
                return;
            }

            QuestionsCounter++;

            SetNextWord();
        }

        private void DontKnowButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"The correct answer is: {currentWord.AdditionalFields[currentLanguage.RequiredInput]}", "Attention!", MessageBoxButton.OK, MessageBoxImage.Information);
            SetNextWord();
            InputTextField.Text = string.Empty;
        }

        private void StartNew_Click(object sender, RoutedEventArgs e)
        {
            SetNewQuiz();

            MakeInputComponentsVisible();

            string? selectedLanguage = LanguagesListBox.SelectionBoxItem.ToString();
            SetNewLanguage(selectedLanguage);

            QuestionsCounter = 1;

            SetNextWord();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LanguagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsingLanguageLabel.Content = "You're using: ";
        }

        private LanguageEntity GetRandomWord()
        {
            var records = languageEntitiesLoader.LoadData(dictionaryFilePath);
            Random rand = new Random();
            var word = records[rand.Next(0, records.Count)];

            return word;
        }

        private void SetNewLanguage(string selectedLanguage)
        {
            configuration = configurationLoader.LoadConfiguration(configFilePath);
            languageEntitiesFactory = new LanguageEntitiesDataLoaderFactory(configuration, selectedLanguage);
            languageEntitiesLoader = languageEntitiesFactory.CreateLoader();
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
    }
}