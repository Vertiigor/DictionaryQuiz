using DictionaryQuiz.Models;
using Newtonsoft.Json;
using System.IO;

namespace DictionaryQuiz.Loaders
{
    internal class QuizzesLoader : DataLoader<Quiz>
    {
        public QuizzesLoader(ConfigurationRoot config) : base(config) { }

        public override List<Quiz> LoadData(string filePath)
        {
            List<Quiz> quizzes;

            if (File.Exists(filePath) && new FileInfo(filePath).Length != 0)
            {
                string jsonString = File.ReadAllText(filePath);

                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<Quiz>>>(jsonString);
                quizzes = jsonObject?["quizzes"] ?? new List<Quiz>();
            }
            else
            {
                throw new Exception($"There is no file {filePath}");
            }

            return quizzes;
        }
    }
}
