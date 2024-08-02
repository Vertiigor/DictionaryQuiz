namespace DictionaryQuiz.Models
{
    internal class LanguageEntity : ILoadableData
    {
        public int ID { get; set; }
        public string Word { get; set; } = string.Empty;
        public Dictionary<string, string?> AdditionalFields { get; set; } = new Dictionary<string, string?>();
    }
}
