using Newtonsoft.Json;

namespace DictionaryQuiz.Models
{
    public class QuizPreferences
    {
        [JsonProperty("questionsCount")]
        public int QuestionsCount { get; set; }
    }
}
