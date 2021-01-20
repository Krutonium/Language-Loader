using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LanguageLoader
{
    public abstract class LLoader
    {
        private gameLanguage _language = new gameLanguage();
        private gameLanguage _englishfalback = new gameLanguage();
        private string reloadPath = "";
        
        public string languagePath = "./languages/";
        public string languageExt = ".json";
        
        public bool LoadLanguage(string Language)
        {
            reloadPath = languagePath + Language + languageExt;
            return ReloadLanguages();
            //THIS MUST BE CALLED FIRST TO USE THE LANGUAGE SEEKING
            //CAPABILITES
        }

        public Task<string> GetString(string Key)
        {
            if (_language.language.ContainsKey(Key))
            {
                return Task.FromResult(_language.language[Key]);
            }
            else if(_englishfalback.language.ContainsKey(Key))
            {
                return Task.FromResult(_englishfalback.language[Key]);
            }
            else
            {
                //This string is unknown!
                return Task.FromResult("UNKNOWN STRING");
            }
        }

        public void addNewText(string Key, string Value, string Language)
        {
            try
            {
                string Json = File.ReadAllText("./languages/" + Language + languageExt);
                gameLanguage L = JsonSerializer.Deserialize<gameLanguage>(Json);
                L.language.Add(Key, Value);
                Json = JsonSerializer.Serialize(L);
                File.WriteAllText(languagePath + Language + languageExt, Json);
            }
            catch
            {  
                //GD.Print("Failed to load Language!");
            }
        }

        public void deleteText(string Key, string Language)
        {
            try
            {
                string Json = File.ReadAllText("./languages/" + Language + languageExt);
                gameLanguage L = JsonSerializer.Deserialize<gameLanguage>(Json);
                L.language.Remove(Key);
                Json = JsonSerializer.Serialize(L);
                File.WriteAllText(languagePath + Language + languageExt, Json);
            }
            catch
            {  
                //GD.Print("Failed to load Language!");
            }
        }

        public bool ReloadLanguages()
        {
            bool success1 = false;
            bool success2 = false;
            try
            {
                string Json = File.ReadAllText(reloadPath);
                _language = JsonSerializer.Deserialize<gameLanguage>(Json);
                success1 = true;
            }
            catch
            {  
                //GD.Print("Failed to load Language!");
            }
            try
            {
                string Json = File.ReadAllText(languagePath + "/english" + languageExt);
                _englishfalback = JsonSerializer.Deserialize<gameLanguage>(Json);
                success2 = true;
            }
            catch
            {
                //GD.Print("Failed to load English Fallback!");
            }

            if (success1 & success2)
            {
                return true;
            }
            return false;
        }
        
        internal class gameLanguage
        {
            public Dictionary<string, string> language;
        }
    }
    
}