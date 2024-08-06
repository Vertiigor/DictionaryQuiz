using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;

namespace DictionaryQuiz.Factories
{
    internal class LanguageEntitiesDataLoaderFactory : DataLoaderFactory<LanguageEntity>
    {
        public string Language { get; set; }

        public LanguageEntitiesDataLoaderFactory(ConfigurationRoot config, string language) : base(config)
        {
            Language = language;
        }

        protected override DataLoader<LanguageEntity> InitializeDataLoader()
        {
            return new LanguageEntitiesDataLoader(config, Language);
        }
    }
}