using DictionaryQuiz.Models;
using DictionaryQuiz.Repositories;
using DictionaryQuiz.Repositories.Interfaces;
using DictionaryQuiz.Services.Abstractions;

namespace DictionaryQuiz.Services
{
    internal class QuizzesService : Service<Quiz>
    {
        public QuizzesService(Repository<Quiz> repository, ConfigurationRoot configuration) : base(repository, configuration) { }

        public override IEnumerable<Quiz> GetAll(string path)
        {
            return repository.GetAll(path);
        }

        public void SaveQuiz(string path, Quiz quiz)
        {
            (repository as QuizRepository).Save(path, quiz);
        }
    }
}
