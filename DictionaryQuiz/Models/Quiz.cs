namespace DictionaryQuiz.Models
{
    public class Quiz : ILoadableData, ISavable
    {
        public string Date { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int QuestionsCount { get; set; }
        public List<string> CorrectAnswers { get; set; } = new List<string>();
        public List<string> IncorrectAnswers { get; set; } = new List<string>();
        public string Conclusion { get; set; } = string.Empty;
    }
}
