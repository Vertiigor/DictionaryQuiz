using DictionaryQuiz.Models;

namespace DictionaryQuiz.Loaders
{
    public abstract class DataLoader<T> where T : ILoadableData
    {
        protected readonly ConfigurationRoot config;

        public DataLoader(ConfigurationRoot config)
        {
            this.config = config;
        }

        public abstract List<T> LoadData(string filePath);
    }
}
