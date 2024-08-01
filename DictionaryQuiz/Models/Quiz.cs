namespace DictionaryQuiz.Models
{
    public class Quiz : ILoadableData
    {
        public string Date { get; set; }
        public string Language { get; set; }
        public int QuestionsCount { get; set; }
        public List<string> CorrectAnswers { get; set; }
        public List<string> IncorrectAnswers { get; set; }
        public string Conclusion { get; set; }
    }
}
