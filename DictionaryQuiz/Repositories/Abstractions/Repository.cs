using DictionaryQuiz.Models;
using DictionaryQuiz.Models.Abstractions;

namespace DictionaryQuiz.Repositories.Interfaces
{
    internal abstract class Repository<T> where T : DataEntity
    {
        protected readonly ConfigurationRoot _configuration;

        public Repository(ConfigurationRoot config)
        {
            _configuration = config;
        }

        public abstract IEnumerable<T> GetAll(string path);
    }
}
