using DictionaryQuiz.Models;
using Newtonsoft.Json;
using System.IO;

namespace DictionaryQuiz.Savers
{
    class ConfigurationSaver
    {
        public void SaveConfiguration(string filePath, ConfigurationRoot configuration)
        {
            string jsonString = JsonConvert.SerializeObject(configuration);

            File.WriteAllText(filePath, jsonString);
        }
    }
}
