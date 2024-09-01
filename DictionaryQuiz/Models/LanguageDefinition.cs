using DictionaryQuiz.Models.Interfaces;
using Newtonsoft.Json;

namespace DictionaryQuiz.Models
{
    public class LanguageDefinition : IValidatable
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("requiredInput")]
        public string RequiredInput { get; set; }

        [JsonProperty("additionalFields")]
        public List<string> AdditionalFields { get; set; }
    }
}
