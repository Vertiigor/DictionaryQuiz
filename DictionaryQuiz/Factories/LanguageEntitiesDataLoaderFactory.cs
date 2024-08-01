using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;

namespace DictionaryQuiz.Factories
{
    internal class LanguageEntitiesDataLoaderFactory : DataLoaderFactory
    {
        public string Language { get; set; }

        public LanguageEntitiesDataLoaderFactory(ConfigurationRoot config, string language) : base(config)
        {
            Language = language;
        }

        protected override DataLoader InitializeDataLoader()
        {
            return new LanguageEntitiesDataLoader(config, Language);
        }
    }
}