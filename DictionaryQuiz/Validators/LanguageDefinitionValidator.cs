using DictionaryQuiz.Models;

namespace DictionaryQuiz.Validators
{
    internal class LanguageDefinitionValidator : IValidator<LanguageDefinition>
    {
        public bool Validate(LanguageDefinition definition, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                errors.Add("Language definition must have a name.");
            }
            if (string.IsNullOrWhiteSpace(definition.RequiredInput))
            {
                errors.Add("Language definition must have a requiered input.");
            }
            if (definition.AdditionalFields.Count == 0)
            {
                errors.Add("Language definition must have at least one additional field.");
            }
            if (!(definition.AdditionalFields.Contains(definition.RequiredInput)))
            {
                errors.Add("There is no requierment filed among additional fields.");
            }

            return errors.Count == 0;
        }
    }
}
