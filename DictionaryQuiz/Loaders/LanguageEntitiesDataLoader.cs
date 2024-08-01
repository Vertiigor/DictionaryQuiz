using CsvHelper;
using DictionaryQuiz.Models;
using System.Globalization;
using System.IO;

namespace DictionaryQuiz.Loaders
{
    internal class LanguageEntitiesDataLoader : DataLoader
    {
        public readonly LanguageDefinition languageDefinition;

        public LanguageEntitiesDataLoader(ConfigurationRoot config, string language) : base(config)
        {
            languageDefinition = GetLanguageDefinition(language);
        }

        public override List<ILoadableData> LoadData(string filePath)
        {
            var records = new List<ILoadableData>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var dictionaryRecords = csv.GetRecords<dynamic>().ToList();
                foreach (var record in dictionaryRecords)
                {
                    var recordDictionary = (IDictionary<string, object>)record;
                    var entity = new LanguageEntity
                    {
                        Word = record.Word,
                        AdditionalFields = languageDefinition.AdditionalFields.ToDictionary(
                            field => field,
                            field => recordDictionary.ContainsKey(field) ? recordDictionary[field]?.ToString() : null
                            )
                    };
                    // Dear God...
                    records.Add(entity);
                }
            }
            return records;
        }

        private LanguageDefinition GetLanguageDefinition(string language)
        {
            var definition = config.Languages.FirstOrDefault(x => x.Name == language);
            if (definition == null)
            {
                throw new ArgumentException($"No loader found for the language: {language}");
            }

            return definition;
        }
    }
}
