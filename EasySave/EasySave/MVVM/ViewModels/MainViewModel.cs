using System;
using System.Collections.Generic;
using System.IO;
using EasySave.MVVM.Models;
using EasySave.MVVM.Views;

namespace EasySave.MVVM.ViewModels
{
    public class MainViewModel
    {
        private readonly PathViewModel _pathViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;
        private readonly SaveProfileViewModel _saveProfileViewModel;
        private readonly _consoleViewModel _consoleViewModel;


        public MainViewModel(string userargument, string version)
        {
            _pathViewModel = new PathViewModel();
            _configurationViewModel = new ConfigurationViewModel();
            _saveProfileViewModel = new SaveProfileViewModel();

            // Create a new dictionary to store the paths
            Dictionary<string, string> paths = new Dictionary<string, string>();
            paths = _pathViewModel.LoadPaths();

            // Create a new dictionary to store the config
            Dictionary<string, string> config = new Dictionary<string, string>();
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            // Create a new list to store the save profiles
            List<SaveProfile> saveProfiles = new List<SaveProfile>();
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);

            // Create a new console view model
            _consoleViewModel = new _consoleViewModel(userargument, version, paths, config, saveProfiles);
        }
    }
}
