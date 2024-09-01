using DictionaryQuiz.Models;
using DictionaryQuiz.Repositories;
using DictionaryQuiz.Repositories.Interfaces;
using DictionaryQuiz.Services.Abstractions;

namespace DictionaryQuiz.Services
{
    internal class LanguageEntitiesService : Service<LanguageEntity>
    {
        private string languageName = string.Empty;

        public string Language
        {
            get => languageName;
            set
            {
                (repository as LanguageEntityRepository).languageDefinition = GetLanguageDefinition(value);
            }
        }

        public LanguageEntitiesService(Repository<LanguageEntity> repository, ConfigurationRoot configuration) : base(repository, configuration) { }

        public override IEnumerable<LanguageEntity> GetAll(string path)
        {
            return repository.GetAll(path);
        }

        private LanguageDefinition GetLanguageDefinition(string language)
        {
            var definition = _configuration.Languages.FirstOrDefault(x => x.Name == language);
            if (definition == null)
            {
                throw new ArgumentException($"No loader found for the language: {language}");
            }

            return definition;
        }
    }
}
