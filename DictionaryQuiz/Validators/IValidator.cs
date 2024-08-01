namespace DictionaryQuiz.Validators
{
    public interface IValidator<T>
    {
        public bool Validate(T entity, out List<string> errors);
    }
}
