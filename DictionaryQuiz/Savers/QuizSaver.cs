using DictionaryQuiz.Models;
using Newtonsoft.Json;
using System.IO;

namespace DictionaryQuiz.Savers
{
    public class QuizSaver : ISaver<Quiz>
    {
        public void Save(string filePath, Quiz quiz)
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
                quizzes = new List<Quiz>();
            }

            quizzes.Add(quiz);

            string updatedJsonString = JsonConvert.SerializeObject(new { quizzes }, Formatting.Indented);

            File.WriteAllText(filePath, updatedJsonString);
        }
    }
}
