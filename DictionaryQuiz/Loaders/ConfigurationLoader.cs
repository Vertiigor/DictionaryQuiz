using DictionaryQuiz.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DictionaryQuiz.Loaders
{
    internal class ConfigurationLoader
    {
        public ConfigurationRoot LoadConfiguration(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var jObject = JObject.Parse(json);

            return JsonConvert.DeserializeObject<ConfigurationRoot>(json) ?? throw new NullReferenceException();
        }
    }
}
