using System.Windows;

namespace DictionaryQuiz.Validators
{
    public abstract class Validator<T>
    {
        public abstract bool Validate(T entity, out List<string> errors);

        public void ShowError(List<string> errors)
        {
            string fullError = string.Join("\n", errors);

            MessageBox.Show(fullError, "Attention!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
