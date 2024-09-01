using DictionaryQuiz.Models;
using DictionaryQuiz.Services;
using DictionaryQuiz.Validators;
using System.IO;
using System.Windows;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for NewLanguageWindow.xaml
    /// </summary>
    public partial class NewLanguageWindow : Window
    {
        private string configFilePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "Configuration", "config.json");
        private ConfigurationRoot configuration;
        private ConfigurationService configurationService;
        private LanguageDefinition definition;
        private LanguageDefinitionValidator validator;
        public NewLanguageWindow()
        {
            InitializeComponent();

            configurationService = new ConfigurationService();
            configuration = configurationService.Load(configFilePath);
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
                validator.ShowError(errors);
            }
        }

        private void AddNewDefinition()
        {
            configuration.Languages.Add(definition);

            configurationService.Save(configFilePath, configuration);
        }

        private void FillDefinitionByInput()
        {
            definition.Name = NameTextField.Text;
            definition.RequiredInput = RequiredInputTextField.Text;
            definition.AdditionalFields = AdditionalFieldsTextField.Text.Split(',').ToList();
        }
    }
}
