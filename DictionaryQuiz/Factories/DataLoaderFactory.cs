using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;

namespace DictionaryQuiz.Factories
{
    internal abstract class DataLoaderFactory
    {
        protected readonly ConfigurationRoot config;

        public DataLoaderFactory(ConfigurationRoot config)
        {
            this.config = config;
        }

        protected abstract DataLoader InitializeDataLoader();

        public DataLoader CreateLoader()
        {
            return this.InitializeDataLoader();
        }
    }
}
