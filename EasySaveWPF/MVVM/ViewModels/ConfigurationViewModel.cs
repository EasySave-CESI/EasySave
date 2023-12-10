using System;
using System.Collections.Generic;
using System.IO;
using EasySave.MVVM.Models;
using EasySave.MVVM.Views;

namespace EasySave.MVVM.ViewModels
{
    public class ConfigurationViewModel
    {
        public ConfigurationViewModel() {}

        public Dictionary<string, string> LoadConfig(string filePath)
        {
            return Configuration.LoadConfig(filePath);
        }
    }
}
