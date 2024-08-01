using DictionaryQuiz.Factories;
using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataLoaderFactory dataLoaderFactory;
        private string dictionaryFilePath;
        private string configFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Configuration", "config.json");
        private ConfigurationRoot Configuration;
        private DataLoader dataLoader;
        private ConfigurationLoader configurationLoader;
        private Quiz currentQuiz;
        private LanguageEntity currentWord;
        private LanguageDefinition currentLanguage;

        public MainWindow()
        {
            InitializeComponent();

            dictionaryFilePath = string.Empty;
            configurationLoader = new ConfigurationLoader();
            Configuration = configurationLoader.LoadConfiguration(configFilePath);
            dataLoaderFactory = new LanguageEntitiesDataLoaderFactory(Configuration, "English");
            dataLoader = dataLoaderFactory.CreateLoader();
            currentQuiz = new Quiz();
            currentWord = new LanguageEntity();
            currentLanguage = new LanguageDefinition();

            foreach (var language in Configuration.Languages)
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

        private LanguageEntity GetRandomWord()
        {
            var records = dataLoader.LoadData(dictionaryFilePath);
            Random rand = new Random();
            var word = records[rand.Next(0, records.Count)];

            return (LanguageEntity)word;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string? selectedLanguage = LanguagesListBox.SelectionBoxItem.ToString();

            if (selectedLanguage == string.Empty || selectedLanguage == null)
            {
                MessageBox.Show("Choose the language!");
                return;
            }

            ShowOpenFileDialog("Comma delimited (*.csv)|*.csv", SetNewDictionaryFilePath);

            dataLoaderFactory = new LanguageEntitiesDataLoaderFactory(Configuration, selectedLanguage);
            dataLoader = dataLoaderFactory.CreateLoader();
            currentLanguage = Configuration.Languages.First(x => x.Name == selectedLanguage);
        }

        private void MakeInputComponentsVisible()
        {
            NextButton.Visibility = Visibility.Visible;
            InputTextField.Visibility = Visibility.Visible;
            CheckButton.Visibility = Visibility.Visible;
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

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (InputTextField.Text == currentWord.AdditionalFields[currentLanguage.RequiredInput])
            {
                MessageBox.Show("Correct");
            }
            else
            {
                MessageBox.Show("Incorrect");
            }

            InputTextField.Text = string.Empty;
        }

        private void LanguagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsingLanguageLabel.Content = "You're using: ";
        }

        private void StartNew_Click(object sender, RoutedEventArgs e)
        {
            MakeInputComponentsVisible();
            Configuration = configurationLoader.LoadConfiguration(configFilePath);
            QuestionsCountLabel.Content = $"1 / {Configuration.QuizPreferences.QuestionsCount}";
        }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            QuizPreferencesWindow preferencesWindow = new QuizPreferencesWindow();
            preferencesWindow.Show();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentWord = GetRandomWord();
            WordContent.Content = $"{currentWord.Word}";
        }
    }
}