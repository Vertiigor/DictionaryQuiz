using System.Windows;

namespace DictionaryQuiz.Validators
{
    public abstract class Validator<T>
    {
        public abstract bool Validate(T entity, out List<string> errors);

        public void ShowError(List<string> errors)
        {
            string fullError = string.Empty;

            foreach (var error in errors)
            {
                fullError += error + "\n";
            }

            MessageBox.Show(fullError);
        }
    }
}
