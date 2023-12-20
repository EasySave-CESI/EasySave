using EasySave.MVVM.Models;
using System.Collections.Generic;

namespace EasySave.MVVM.ViewModels
{
    public class LanguageConfigurationViewModel
    {
        public LanguageConfiguration LanguageConfiguration { get; set; } = new LanguageConfiguration();
        public LanguageConfigurationViewModel() {}

        public Dictionary<string, string> LoadPrintStrings(string language)
        {
            if (language == "fr")
            {
                return LanguageConfiguration.GetPrintStringsFr();
            }
            else
            {
                return LanguageConfiguration.GetPrintStringsEn();
            }
        }
    }
}
