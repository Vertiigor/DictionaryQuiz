using DictionaryQuiz.Models;
using DictionaryQuiz.Models.Abstractions;
using DictionaryQuiz.Repositories;
using DictionaryQuiz.Repositories.Interfaces;
using DictionaryQuiz.Services;
using DictionaryQuiz.Services.Abstractions;

namespace DictionaryQuiz.Factories
{
    internal class ServicesFactory
    {
        private readonly ConfigurationRoot _configuration;

        public ServicesFactory(ConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public Service<T> CreateService<T>() where T : DataEntity
        {
            if (typeof(T) == typeof(LanguageEntity))
            {
                var repository = new LanguageEntityRepository(_configuration);
                return new LanguageEntitiesService((Repository<LanguageEntity>)repository, _configuration) as Service<T>;
            }
            if (typeof(T) == typeof(Quiz))
            {
                var repository = new QuizRepository(_configuration);
                return new QuizzesService((Repository<Quiz>)repository, _configuration) as Service<T>;
            }

            throw new NotSupportedException($"Service for type {typeof(T).Name} is not supported.");
        }
    }
}
