using System.Collections.Generic;
using System.Linq;

namespace GetMeX.Models
{
    public class LanguageCode
    {
        private static Dictionary<string, string> LangDict = new Dictionary<string, string>()
        {
            ["Auto"] = "auto",
            ["Chinese"] = "zh-CN",
            ["English"] = "en",
            ["France"] = "fr",
            ["Japanese"] = "jp",
            ["Korean"] = "ko",
            ["Spanish"] = "es",
            ["Russian"] = "ru"
        };

        public string LangToCode(string lang)
        {
            return LangDict.ContainsKey(lang) ? LangDict[lang] : "auto";
        }

        public List<string> GetLanguages()
        {
            return LangDict.Keys.ToList();
        }
    }
}
