using DictionaryQuiz.Models;
using DictionaryQuiz.Models.Abstractions;
using DictionaryQuiz.Repositories.Interfaces;

namespace DictionaryQuiz.Services.Abstractions
{
    internal abstract class Service<T> where T : DataEntity
    {
        protected readonly ConfigurationRoot _configuration;
        protected Repository<T> repository;

        public Service(Repository<T> repository, ConfigurationRoot configuration)
        {
            this.repository = repository;
            _configuration = configuration;
        }

        public abstract IEnumerable<T> GetAll(string path);
    }
}
