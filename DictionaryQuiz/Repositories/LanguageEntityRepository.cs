using CsvHelper;
using DictionaryQuiz.Models;
using DictionaryQuiz.Repositories.Interfaces;
using System.Globalization;
using System.IO;

namespace DictionaryQuiz.Repositories
{
    internal class LanguageEntityRepository : Repository<LanguageEntity>
    {
        private string languageName = string.Empty;
        public LanguageDefinition languageDefinition;

        public LanguageEntityRepository(ConfigurationRoot configuration) : base(configuration) { }

        public override IEnumerable<LanguageEntity> GetAll(string path)
        {
            var records = new List<LanguageEntity>();

            using (var reader = new StreamReader(path))
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
    }
}
