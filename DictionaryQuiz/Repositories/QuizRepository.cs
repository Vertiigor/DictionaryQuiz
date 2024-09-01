using DictionaryQuiz.Models;
using DictionaryQuiz.Repositories.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace DictionaryQuiz.Repositories
{
    internal class QuizRepository : Repository<Quiz>
    {
        public QuizRepository(ConfigurationRoot configuration) : base(configuration)
        {
        }

        public override IEnumerable<Quiz> GetAll(string path)
        {
            List<Quiz> quizzes;

            if (File.Exists(path) && new FileInfo(path).Length != 0)
            {
                string jsonString = File.ReadAllText(path);

                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<Quiz>>>(jsonString);
                quizzes = jsonObject?["quizzes"] ?? new List<Quiz>();
            }
            else
            {
                throw new Exception($"There is no file {path}");
            }

            return quizzes;
        }

        public void Save(string path, Quiz quiz)
        {
            List<Quiz> quizzes;

            if (File.Exists(path) && new FileInfo(path).Length != 0)
            {
                string jsonString = File.ReadAllText(path);

                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<Quiz>>>(jsonString);
                quizzes = jsonObject?["quizzes"] ?? new List<Quiz>();
            }
            else
            {
                quizzes = new List<Quiz>();
            }

            quizzes.Add(quiz);

            string updatedJsonString = JsonConvert.SerializeObject(new { quizzes }, Formatting.Indented);

            File.WriteAllText(path, updatedJsonString);
        }
    }
}
