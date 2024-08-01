using Newtonsoft.Json;

namespace DictionaryQuiz.Models
{
    public class ConfigurationRoot
    {
        [JsonProperty("languages")]
        public List<LanguageDefinition> Languages { get; set; }

        [JsonProperty("quizPreferences")]
        public QuizPreferences QuizPreferences { get; set; }
    }
}
