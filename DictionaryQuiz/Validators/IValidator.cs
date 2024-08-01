using DictionaryQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryQuiz.Validators
{
    internal interface IValidator<T> where T : LanguageEntity
    {
        public bool Validate(T entity, string input);
    }
}
