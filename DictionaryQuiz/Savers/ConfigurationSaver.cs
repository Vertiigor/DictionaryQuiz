using DictionaryQuiz.Models;
using Newtonsoft.Json;
using System.IO;

namespace DictionaryQuiz.Savers
{
    class ConfigurationSaver : ISaver<ConfigurationRoot>
    {
        public void Save(string filePath, ConfigurationRoot configuration)
        {
            string jsonString = JsonConvert.SerializeObject(configuration);

            File.WriteAllText(filePath, jsonString);
        }
    }
}
