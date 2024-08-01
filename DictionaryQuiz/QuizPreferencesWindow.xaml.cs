using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;
using DictionaryQuiz.Savers;
using System.IO;
using System.Windows;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for QuizPreferencesWindow.xaml
    /// </summary>
    public partial class QuizPreferencesWindow : Window
    {
        private string configFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Configuration", "config.json");
        private ConfigurationRoot Configuration;
        private ConfigurationLoader configurationLoader;
        private QuizPreferences preferences;
        private ConfigurationSaver configurationSaver;

        public QuizPreferencesWindow()
        {
            InitializeComponent();

            configurationLoader = new ConfigurationLoader();
            configurationSaver = new ConfigurationSaver();
            Configuration = configurationLoader.LoadConfiguration(configFilePath);
            preferences = new QuizPreferences();

            FillInput();
        }

        private void FillInput()
        {
            QuestionCountTextField.Text = $"{Configuration.QuizPreferences.QuestionsCount}";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            preferences.QuestionsCount = Convert.ToInt32(QuestionCountTextField.Text);
            Configuration.QuizPreferences = preferences;

            configurationSaver.SaveConfiguration(configFilePath, Configuration);

            this.Close();
        }
    }
}
