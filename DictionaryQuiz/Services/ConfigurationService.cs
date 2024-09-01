using DictionaryQuiz.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DictionaryQuiz.Services
{
    internal class ConfigurationService
    {
        public ConfigurationRoot Load(string path)
        {
            var json = File.ReadAllText(path);
            var jObject = JObject.Parse(json);

            return JsonConvert.DeserializeObject<ConfigurationRoot>(json) ?? throw new NullReferenceException();
        }

        public void Save(string path, ConfigurationRoot configuration)
        {
            string jsonString = JsonConvert.SerializeObject(configuration);

            File.WriteAllText(path, jsonString);
        }
    }
}
