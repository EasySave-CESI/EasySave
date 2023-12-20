using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.ViewModels;
using EasySaveWPF.MVVM.Views;
using System;
using System.Collections.Generic;

namespace EasySaveWPF.MVVM.ViewModels
{
    public class _consoleViewModel
    {
        // All the objects needed to run the _consoleViewModel
        public ConsoleView _consoleView { get; set; }
        public LanguageConfigurationViewModel _languageConfigurationViewModel { get; set; }
        public DailyLogsViewModel _dailyLogsViewModel { get; set; }
        public SaveProfileViewModel _saveProfileViewModel { get; set; }

        // Lists and Dictionaries
        public Dictionary<string, string> paths { get; set; }
        public Dictionary<string, string> config { get; set; }
        public List<SaveProfile> SaveProfiles { get; set; }
        public Dictionary<string, string> printStringDictionary { get; set; }

        // Strings
        public string version { get; set; }
        public string argument { get; set; }
        public string fulllanguagename { get; set; }




        public _consoleViewModel(string argument, string version, Dictionary<string, string> paths, Dictionary<string, string> config, List<SaveProfile> saveProfiles)
        {
            this.version = version;
            this.argument = argument;
            this.paths = paths;
            this.config = config;
            SaveProfiles = saveProfiles;

            // Define the full language name
            switch (config["language"])
            {
                case "fr":
                    fulllanguagename = "Français";
                    break;
                case "en":
                    fulllanguagename = "English";
                    break;
                default:
                    fulllanguagename = "English";
                    break;
            }

            // Create a new language configuration
            _languageConfigurationViewModel = new LanguageConfigurationViewModel();
            printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);

            // Create a new console view
            _consoleView = new ConsoleView(printStringDictionary);

            // Create a new daily logs view model
            _dailyLogsViewModel = new DailyLogsViewModel(paths["EasySaveFileLogsDirectoryPath"], config["logformat"]);

            // Create a new save profile view model
            _saveProfileViewModel = new SaveProfileViewModel();

            // If the user argument is empty, load the menu, else load the function
            if (argument == "")
            {
                _consoleView.WelcomeMessage(version);
                Menu();
            }
            else
            {
                LoadFunction();
            }
        }

        public void LoadFunction()
        {
            // transform the function to lowercase
            argument = argument.ToLower();
            switch (argument)
            {
                case "menu":
                    Menu();
                    break;
                case "dsp":
                    _saveProfileViewModel.DisplaySaveProfiles(_consoleView, SaveProfiles);
                    break;
                case "msp":
                    //_saveProfileViewModel.ModifySaveProfile(_consoleView, SaveProfiles, paths);
                    break;
                case "esp":
                    //_saveProfileViewModel.ExecuteSaveProfile(_consoleView, _dailyLogsViewModel, SaveProfiles, paths, config);
                    break;
                case "dl":
                    _dailyLogsViewModel.DisplayLogs(_consoleView, paths["EasySaveFileLogsDirectoryPath"], config["logformat"]);
                    break;
                case "help":
                    _consoleView.Help();
                    break;
                case "config":
                    Config();
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    _consoleView.ArgumentError();
                    break;
            }
        }

        public void Menu()
        {
            while (argument != "exit")
            {
                _consoleView.DisplayMenu();
                string choice = _consoleView.Read();
                _consoleView.PrintSeparator();
                switch (choice)
                {
                    case "1":
                        _saveProfileViewModel.DisplaySaveProfiles(_consoleView, SaveProfiles);
                        break;
                    case "2":
                        //_saveProfileViewModel.ModifySaveProfile(_consoleView, SaveProfiles, paths);
                        break;
                    case "3":
                        //_saveProfileViewModel.ExecuteSaveProfile(_consoleView, _dailyLogsViewModel, SaveProfiles, paths, config);
                        break;
                    case "4":
                        _dailyLogsViewModel.DisplayLogs(_consoleView, paths["EasySaveFileLogsDirectoryPath"], config["logformat"]);
                        break;
                    case "5":
                        _consoleView.Help();
                        break;
                    case "6":
                        Config();
                        SaveDailyLogs();
                        break;
                    case "7":
                        _consoleView.Clear();
                        break;
                    case "8":
                        Exit();
                        break;
                    default:
                        _consoleView.DisplayMenuError();
                        break;
                }
            }
        }

        public void Config()
        {
            _consoleView.DisplayConfigurationMenu(fulllanguagename, config["logformat"]);
            string configChoice = _consoleView.Read();

            switch (configChoice)
            {
                case "1":
                    ChooseLanguage();
                    break;
                case "2":
                    ChooseLogFileFormat();
                    SaveDailyLogs();
                    break;
                case "3":
                    _consoleView.Clear();
                    break;
                default:
                    _consoleView.DisplayMenuError();
                    break;
            }
        }

        private DailyLogs LoadDailyLogs()
        {
            DailyLogs loadedLogs = new DailyLogs(paths["EasySaveFileLogsDirectoryPath"], config["logformat"]);
            return loadedLogs;
        }

        private void SaveDailyLogs()
        {
            _dailyLogsViewModel.SaveLogs(paths["EasySaveFileLogsDirectoryPath"], config["logformat"]);
        }

        private void ChooseLanguage()
        {
            _consoleView.DisplayLanguageMenu();
            string languageChoice = _consoleView.Read();

            switch (languageChoice)
            {
                case "1":
                    config["language"] = "fr";
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"], config["theme"], config["maxfilesize"]);
                    printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);
                    _consoleView.SetprintStringDictionary(printStringDictionary);
                    fulllanguagename = "Français";
                    _consoleView.Clear();
                    _consoleView.DisplayLanguageSuccess(fulllanguagename);
                    break;
                case "2":
                    config["language"] = "en";
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"], config["theme"], config["maxfilesize"]);
                    printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);
                    fulllanguagename = "English";
                    _consoleView.Clear();
                    _consoleView.DisplayLanguageSuccess(fulllanguagename);
                    break;
                default:
                    _consoleView.DisplayLanguageError();
                    break;
            }
        }

        private void ChooseLogFileFormat()
        {
            _consoleView.DisplayLogFileFormatMenu();
            string logFormatChoice = _consoleView.Read();

            switch (logFormatChoice)
            {
                case "1":
                    config["logformat"] = "json";
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"], config["theme"], config["maxfilesize"]);
                    _consoleView.Clear();
                    _consoleView.DisplayLogFileFormatSuccess(config["logformat"]);
                    break;
                case "2":
                    config["logformat"] = "xml";
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"], config["theme"], config["maxfilesize"]);
                    _consoleView.Clear();
                    _consoleView.DisplayLogFileFormatSuccess(config["logformat"]);
                    break;
                default:
                    _consoleView.DisplayLogFileFormatError();
                    break;
            }
        }

        public void Exit()
        {
            _consoleView.Exit();
            argument = "exit";
        }
    }
}
