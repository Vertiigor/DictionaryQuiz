using Newtonsoft.Json;

namespace DictionaryQuiz.Models
{
    public class Quiz : ILoadableData, ISavable
    {
        [JsonProperty("date")]
        public string Date { get; set; } = string.Empty;

        [JsonProperty("language")]
        public string Language { get; set; } = string.Empty;

        [JsonProperty("questionsCount")]
        public int QuestionsCount { get; set; }

        [JsonProperty("correctAnswers")]
        public List<string> CorrectAnswers { get; set; } = new List<string>();

        [JsonProperty("incorrectAnswers")]
        public List<string> IncorrectAnswers { get; set; } = new List<string>();

        [JsonProperty("conclusion")]
        public string Conclusion { get; set; } = string.Empty;
    }
}
