using EasySave.MVVM.Models;
using EasySave.MVVM.Views;
using System;
using System.Collections.Generic;

namespace EasySave.MVVM.ViewModels
{
    public class _consoleViewModel
    {
        // All the objects needed to run the _consoleViewModel
        public ConsoleView _consoleView { get; set; }
        public LanguageConfigurationViewModel _languageConfigurationViewModel { get; set; }
        public DailyLogs _Logger { get; set; }

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
            this.SaveProfiles = saveProfiles;

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

            // Create a new daily logs
            _Logger = new DailyLogs(paths["EasySaveFileLogsDirectoryPath"], config["logformat"]);

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
                    DisplaySaveProfiles();
                    break;
                case "csp":
                    CreateSaveProfile();
                    break;
                case "msp":
                    ModifySaveProfile();
                    break;
                case "esp":
                    ExecuteSaveProfile();
                    break;
                case "dl":
                    DisplayLogs();
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
                        DisplaySaveProfiles();
                        break;
                    case "2":
                        CreateSaveProfile();
                        break;
                    case "3":
                        ModifySaveProfile();
                        break;
                    case "4":
                        ExecuteSaveProfile();
                        break;
                    case "5":
                        DisplayLogs();
                        break;
                    case "6":
                        _consoleView.Help();
                        break;
                    case "7":
                        Config();
                        SaveDailyLogs();
                        break;
                    case "8":
                        _consoleView.Clear();
                        break;
                    case "9":
                        Exit();
                        break;
                    default:
                        _consoleView.DisplayMenuError();
                        break;
                }
            }
        }

        public void DisplaySaveProfiles()
        {
            if (SaveProfiles == null)
            {
                _consoleView.DisplaySaveProfilesError();
            }
            else
            {
                {
                    foreach (SaveProfile profile in SaveProfiles)
                    {
                        List<string> list = new List<string>();
                        list.Add(profile.Name);
                        list.Add(profile.SourceFilePath);
                        list.Add(profile.TargetFilePath);
                        list.Add(profile.State);
                        list.Add(profile.TotalFilesToCopy.ToString());
                        list.Add(profile.TotalFilesSize.ToString());
                        list.Add(profile.NbFilesLeftToDo.ToString());
                        list.Add(profile.Progression.ToString());
                        list.Add(profile.TypeOfSave);
                        _consoleView.DisplaySaveProfiles(list.ToArray());
                    }
                }
            }

        }

        public void CreateSaveProfile()
        {
            _consoleView.NotImplementedYet();
        }

        public void ModifySaveProfile()
        {

            try
            {
                string profileName = string.Empty;
                int profileIndex = -1;

                profileIndex = GetProfileIndex();

                _consoleView.DisplaySelectedProfileName(SaveProfiles[profileIndex].Name);

                _consoleView.DisplayModifySaveProfileNewName();
                SaveProfiles[profileIndex].Name = _consoleView.Read();

                _consoleView.DisplayModifySaveProfileNewSourceFilePath();
                SaveProfiles[profileIndex].SourceFilePath = _consoleView.Read();

                _consoleView.DisplayModifySaveProfileNewTargetFilePath();
                SaveProfiles[profileIndex].TargetFilePath = _consoleView.Read();

                _consoleView.DisplayModifySaveProfileNewTypeOfSave();
                SaveProfiles[profileIndex].TypeOfSave = _consoleView.Read();

                List<long> sourcedirectoryinfo = SaveProfile.sourceDirectoryInfos(SaveProfiles[profileIndex].SourceFilePath);
                SaveProfiles[profileIndex].TotalFilesToCopy = (int)sourcedirectoryinfo[0];
                SaveProfiles[profileIndex].TotalFilesSize = sourcedirectoryinfo[1];
                SaveProfiles[profileIndex].NbFilesLeftToDo = (int)sourcedirectoryinfo[0];
                SaveProfiles[profileIndex].Progression = 0;

                SaveProfiles[profileIndex].State = "READY";

                SaveProfile.SaveProfiles(paths["StateFilePath"], SaveProfiles);
                _consoleView.DisplayModifySaveProfileSuccess();

            }
            catch (Exception ex)
            {
                _consoleView.Error(ex.Message);
            }

        }

        public void ExecuteSaveProfile()
        {
            try
            {
                int profileIndex = GetProfileIndex();
                _consoleView.DisplaySelectedProfileName(SaveProfiles[profileIndex].Name);

                if (SaveProfiles[profileIndex].State == "READY")
                {
                    SaveProfiles[profileIndex].State = "IN PROGRESS";
                    SaveProfile.SaveProfiles(paths["StateFilePath"], SaveProfiles);

                    if (SaveProfiles[profileIndex].TypeOfSave == "full" || SaveProfiles[profileIndex].TypeOfSave == "diff")
                    {
                        _consoleView.DisplayBackupInProgress(SaveProfiles[profileIndex].Name);
                        SaveProfile.ExecuteSaveProfile(SaveProfiles, SaveProfiles[profileIndex], SaveProfiles[profileIndex].TypeOfSave);

                        DateTime startTime = DateTime.Now;
                        TimeSpan elapsedTime = DateTime.Now - startTime;

                        _Logger.CreateLog(
                        SaveProfiles[profileIndex].Name,
                        SaveProfiles[profileIndex].SourceFilePath,
                        SaveProfiles[profileIndex].TargetFilePath,
                        SaveProfiles[profileIndex].TotalFilesSize,
                        elapsedTime.TotalMilliseconds
                        );
                    }
                    else
                    {
                        _consoleView.DisplayExecuteSaveProfileTypeOfSaveError();
                    }

                    SaveProfiles[profileIndex].State = "COMPLETED";
                    SaveProfile.SaveProfiles(paths["StateFilePath"], SaveProfiles);
                    _consoleView.DisplayExecuteSaveProfileSuccess();
                }
                else
                {
                    _consoleView.DisplayExecuteSaveProfileStateError();
                }
            }
            catch (Exception ex)
            {
                _consoleView.Error(ex.Message);
            }
        }
        public void DisplayLogs()
        {
            _consoleView.DisplayLogsHeader();

            foreach (var log in _Logger.GetLogs())
            {
                _consoleView.DisplayLog(log);
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
            //DailyLogs loadedLogs = new DailyLogs(EasySaveFileLogsDirectoryPath, logformat);
            //return loadedLogs;
            return null;
        }

        private void SaveDailyLogs()
        {
            _Logger.SaveLogs();
        }

        private void ChooseLanguage()
        {
            _consoleView.DisplayLanguageMenu();
            string languageChoice = _consoleView.Read();

            switch (languageChoice)
            {
                case "1":
                    config["language"] = "fr";
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"]);
                    printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);
                    _consoleView.SetprintStringDictionary(printStringDictionary);
                    fulllanguagename = "Français";
                    _consoleView.Clear();
                    _consoleView.DisplayLanguageSuccess(fulllanguagename);
                    break;
                case "2":
                    config["language"] = "en";
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"]);
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
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"]);
                    _Logger.SetLogFileFormat(LogFileFormat.Json);
                    _consoleView.Clear();
                    _consoleView.DisplayLogFileFormatSuccess(config["logformat"]);
                    break;
                case "2":
                    config["logformat"] = "xml";
                    Configuration.WriteConfig(paths["ConfigFilePath"], config["language"], config["logformat"]);
                    _Logger.SetLogFileFormat(LogFileFormat.Xml);
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
            Environment.Exit(0);
        }

        public int GetProfileIndex()
        {
            _consoleView.DisplayChooseSelectedProfile();

            // print all the profiles name
            for (int i = 0; i < SaveProfiles.Count; i++)
            {
                _consoleView.DisplayProfileIndexName(i + 1, SaveProfiles[i].Name);
            }

            string choice = _consoleView.Read();

            int profileIndex = -1;

            for (int i = 0; i < SaveProfiles.Count; i++)
            {
                if (int.Parse(choice) == i + 1)
                {
                    profileIndex = i;
                }
            }
            return profileIndex;
        }
    }
}
