using DictionaryQuiz.Models.Abstractions;
using DictionaryQuiz.Models.Interfaces;
using Newtonsoft.Json;

namespace DictionaryQuiz.Models
{
    public class ConfigurationRoot : DataEntity, ISavable
    {
        [JsonProperty("languages")]
        public List<LanguageDefinition> Languages { get; set; }

        [JsonProperty("quizPreferences")]
        public QuizPreferences QuizPreferences { get; set; }
    }
}
