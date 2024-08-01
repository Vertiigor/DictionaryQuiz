﻿using DictionaryQuiz.Loaders;
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
        private ConfigurationRoot Configuration;
        private ConfigurationLoader configurationLoader;
        private QuizPreferences preferences;
        private ConfigurationSaver configurationSaver;
        private QuizPreferencesValidator validator;

        public QuizPreferencesWindow()
        {
            InitializeComponent();

            configurationLoader = new ConfigurationLoader();
            configurationSaver = new ConfigurationSaver();
            Configuration = configurationLoader.LoadConfiguration(configFilePath);
            preferences = new QuizPreferences();
            validator = new QuizPreferencesValidator();

            FillInput();
        }

        private void FillInput()
        {
            QuestionCountTextField.Text = $"{Configuration.QuizPreferences.QuestionsCount}";
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
                ShowError(errors);
            }
        }

        private void SaveNewPreferences()
        {
            Configuration.QuizPreferences = preferences;

            configurationSaver.SaveConfiguration(configFilePath, Configuration);
        }

        private static void ShowError(List<string> errors)
        {
            string fullError = string.Empty;

            foreach (var error in errors)
            {
                fullError += error + "\n";
            }

            MessageBox.Show(fullError);
        }

        private void FillPreferencesByInput()
        {
            preferences.QuestionsCount = Convert.ToInt32(QuestionCountTextField.Text);
        }
    }
}
