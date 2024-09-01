using DictionaryQuiz.Models;
using DictionaryQuiz.Services;
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
        private ConfigurationService configurationService;
        private QuizPreferences preferences;
        private QuizPreferencesValidator validator;

        public QuizPreferencesWindow()
        {
            InitializeComponent();

            configurationService = new ConfigurationService();
            configuration = configurationService.Load(configFilePath);
            preferences = new QuizPreferences();
            validator = new QuizPreferencesValidator();

            FillInput();
        }

        private void FillInput()
        {
            QuestionCountTextField.Text = $"{configuration.QuizPreferences.QuestionsCount}";
            AllowDuplicatesCheckBox.IsChecked = configuration.QuizPreferences.AllowDuplicates;
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

            configurationService.Save(configFilePath, configuration);
        }

        private void FillPreferencesByInput()
        {
            preferences.QuestionsCount = Convert.ToInt32(QuestionCountTextField.Text);
            preferences.AllowDuplicates = (bool)AllowDuplicatesCheckBox.IsChecked;
        }
    }
}
