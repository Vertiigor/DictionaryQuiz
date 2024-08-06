using DictionaryQuiz.Loaders;
using DictionaryQuiz.Models;

namespace DictionaryQuiz.Factories
{
    internal abstract class DataLoaderFactory<T> where T : ILoadableData
    {
        protected readonly ConfigurationRoot config;

        public DataLoaderFactory(ConfigurationRoot config)
        {
            this.config = config;
        }

        protected abstract DataLoader<T> InitializeDataLoader();

        public DataLoader<T> CreateLoader()
        {
            return this.InitializeDataLoader();
        }
    }
}
