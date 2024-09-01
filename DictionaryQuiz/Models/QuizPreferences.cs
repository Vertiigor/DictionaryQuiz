using DictionaryQuiz.Models.Interfaces;
using Newtonsoft.Json;

namespace DictionaryQuiz.Models
{
    public class QuizPreferences : IValidatable
    {
        [JsonProperty("questionsCount")]
        public int QuestionsCount { get; set; }

        [JsonProperty("allowDuplicates")]
        public bool AllowDuplicates { get; set; }
    }
}
