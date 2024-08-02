using DictionaryQuiz.Models;

namespace DictionaryQuiz.Savers
{
    public interface ISaver<T> where T : ISavable
    {
        public void Save(string filePath, T entity);
    }
}
