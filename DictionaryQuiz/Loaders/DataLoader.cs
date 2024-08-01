using DictionaryQuiz.Models;

namespace DictionaryQuiz.Loaders
{
    public abstract class DataLoader
    {
        protected readonly ConfigurationRoot config;

        public DataLoader(ConfigurationRoot config)
        {
            this.config = config;
        }

        public abstract List<ILoadableData> LoadData(string filePath);
    }
}
