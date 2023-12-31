﻿using System;
using System.Collections.Generic;
using System.IO;
using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.Views;

namespace EasySaveWPF.MVVM.ViewModels
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
