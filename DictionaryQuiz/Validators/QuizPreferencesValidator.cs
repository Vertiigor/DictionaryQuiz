﻿using DictionaryQuiz.Models;

namespace DictionaryQuiz.Validators
{
    internal class QuizPreferencesValidator : Validator<QuizPreferences>
    {
        public override bool Validate(QuizPreferences preferences, out List<string> errors)
        {
            errors = new List<string>();

            if (preferences.QuestionsCount < 1)
            {
                errors.Add("Questions count must be a positive integer.");
            }

            return errors.Count == 0;
        }
    }
}
