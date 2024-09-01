using DictionaryQuiz.Models;
using DictionaryQuiz.Services;
using DictionaryQuiz.Validators;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for LanguagePreferencesWindow.xaml
    /// </summary>
    public partial class LanguagePreferencesWindow : Window
    {
        private string configFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Configuration", "config.json");
        private ConfigurationRoot configuration;
        private ConfigurationService configurationService;
        private LanguageDefinition definition;
        private LanguageDefinitionValidator validator;

        public LanguagePreferencesWindow()
        {
            InitializeComponent();

            configurationService = new ConfigurationService();
            configuration = configurationService.Load(configFilePath);
            definition = new LanguageDefinition();
            validator = new LanguageDefinitionValidator();

            foreach (var language in configuration.Languages)
            {
                var newItem = new ListBoxItem();
                newItem.Content = $"{language.Name}";

                LanguagesListBox.Items.Add(newItem);
            }
        }

        private void MakeInputComponentsEnabled()
        {
            RequiredInputTextField.IsEnabled = true;
            AdditionalFieldsTextField.IsEnabled = true;
            SaveButton.IsEnabled = true;
        }

        private void LanguagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.definition = configuration.Languages[LanguagesListBox.SelectedIndex];

            MakeInputComponentsEnabled();
            FillInputComponents();
        }

        private void FillInputComponents()
        {
            RequiredInputTextField.Text = definition.RequiredInput;

            AdditionalFieldsTextField.Text = string.Join(',', definition.AdditionalFields);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FillDefinitionByInput();

            if (validator.Validate(definition, out var errors))
            {
                SaveNewDefinition();

                this.Close();
            }
            else
            {
                validator.ShowError(errors);
            }
        }

        private void SaveNewDefinition()
        {
            configuration.Languages.Remove(configuration.Languages.First(x => x.Name == definition.Name));
            configuration.Languages.Add(definition);

            configurationService.Save(configFilePath, configuration);
        }

        private void FillDefinitionByInput()
        {
            definition.RequiredInput = RequiredInputTextField.Text;
            definition.AdditionalFields = AdditionalFieldsTextField.Text.Split(',').ToList();
        }

        private void AddNewLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            NewLanguageWindow newLanguage = new NewLanguageWindow();
            newLanguage.Show();
            this.Close();
        }
    }
}
