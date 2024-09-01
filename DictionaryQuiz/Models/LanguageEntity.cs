using DictionaryQuiz.Models.Abstractions;
using DictionaryQuiz.Models.Interfaces;

namespace DictionaryQuiz.Models
{
    internal class LanguageEntity : DataEntity, ILoadableData
    {
        public int ID { get; set; }
        public string Word { get; set; } = string.Empty;
        public Dictionary<string, string?> AdditionalFields { get; set; } = new Dictionary<string, string?>();
    }
}
