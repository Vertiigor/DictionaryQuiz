using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;
using DictionaryQuiz.Savers;
using DictionaryQuiz.Validators;
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
        private ConfigurationRoot configuration;
        private ConfigurationLoader configurationLoader;
        private QuizPreferences preferences;
        private ConfigurationSaver configurationSaver;
        private QuizPreferencesValidator validator;

        public QuizPreferencesWindow()
        {
            InitializeComponent();

            configurationLoader = new ConfigurationLoader();
            configurationSaver = new ConfigurationSaver();
            configuration = configurationLoader.LoadConfiguration(configFilePath);
            preferences = new QuizPreferences();
            validator = new QuizPreferencesValidator();

            FillInput();
        }

        private void FillInput()
        {
            QuestionCountTextField.Text = $"{configuration.QuizPreferences.QuestionsCount}";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FillPreferencesByInput();

            if (validator.Validate(preferences, out var errors))
            {
                SaveNewPreferences();

                this.Close();
            }
            else
            {
                validator.ShowError(errors);
            }
        }

        private void SaveNewPreferences()
        {
            configuration.QuizPreferences = preferences;

            configurationSaver.SaveConfiguration(configFilePath, configuration);
        }

        private void FillPreferencesByInput()
        {
            preferences.QuestionsCount = Convert.ToInt32(QuestionCountTextField.Text);
        }
    }
}
