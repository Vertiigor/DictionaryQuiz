using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;
using DictionaryQuiz.Savers;
using DictionaryQuiz.Validators;
using System.Windows;
using System.IO;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for NewLanguageWindow.xaml
    /// </summary>
    public partial class NewLanguageWindow : Window
    {
        private string configFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Configuration", "config.json");
        private ConfigurationRoot configuration;
        private ConfigurationLoader configurationLoader;
        private LanguageDefinition definition;
        private ConfigurationSaver configurationSaver;
        private LanguageDefinitionValidator validator;
        public NewLanguageWindow()
        {
            InitializeComponent();

            configurationLoader = new ConfigurationLoader();
            configurationSaver = new ConfigurationSaver();
            configuration = configurationLoader.LoadConfiguration(configFilePath);
            definition = new LanguageDefinition();
            validator = new LanguageDefinitionValidator();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            FillDefinitionByInput();

            if (validator.Validate(definition, out var errors))
            {
                AddNewDefinition();

                this.Close();
            }
            else
            {
                ShowError(errors);
            }
        }

        private void AddNewDefinition()
        {
            configuration.Languages.Add(definition);

            configurationSaver.SaveConfiguration(configFilePath, configuration);
        }

        private void FillDefinitionByInput()
        {
            definition.Name = NameTextField.Text;
            definition.RequiredInput = RequiredInputTextField.Text;
            definition.AdditionalFields = AdditionalFieldsTextField.Text.Split(',').ToList();
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
    }
}
