using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace EasySaveConsoleApp
{
    public class MainViewModel
    {
        public ConsoleView consoleView { get; init; }
        public List<SaveProfile> saveProfiles { get; set; }
        public string argument { get; set; }
        public DailyLogs Logger { get; set; } = new DailyLogs();


        public MainViewModel()
        {
            consoleView = new ConsoleView();
            saveProfiles = SaveProfile.LoadProfiles("..\\..\\..\\logs\\state.json");
            Logger = LoadDailyLogs();
            argument = "menu";

            consoleView.WelcomeMessage();
            Menu();
        }

        public MainViewModel(string userargument)
        {
            consoleView = new ConsoleView();
            saveProfiles = SaveProfile.LoadProfiles("..\\..\\..\\logs\\state.json");
            Logger = LoadDailyLogs();
            argument = userargument;

            consoleView.WelcomeMessage();
            Main();
        }

        public void Main()
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
                    consoleView.Help();
                    break;
                case "config":
                    Configuration();
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    consoleView.ArgumentError();
                    break;
            }   
        }

        public void Menu()
        {
            while (argument != "exit")
            {
                consoleView.DisplayMenu();
                string choice = consoleView.Read();
                consoleView.printSeparator();
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
                        consoleView.Help();
                        break;
                    case "7":
                        Configuration();
                        SaveDailyLogs();
                        break;
                    case "8":
                        consoleView.Clear();
                        break;
                    case "9":
                        Exit();
                        break;
                    default:
                        consoleView.DisplayMenuError();
                        break;
                }
            }
        }

        public void DisplaySaveProfiles() 
        {
            if (saveProfiles == null)
            {
                consoleView.DisplaySaveProfilesError();
            }
            else
            {
                {
                    foreach (SaveProfile profile in saveProfiles)
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
                        consoleView.DisplaySaveProfiles(list.ToArray());
                        consoleView.printSeparator();
                    }
                }
            }

        }
        public void CreateSaveProfile() 
        {
            consoleView.NotImplementedYet();
        }
        public void ModifySaveProfile() 
        {

            try
            {
                string profileName = string.Empty;
                int profileIndex = -1;

                profileIndex = GetProfileIndex();

                consoleView.DisplaySelectedProfileName(saveProfiles[profileIndex].Name);

                consoleView.DisplayModifySaveProfileNewName();
                saveProfiles[profileIndex].Name = consoleView.Read();

                consoleView.DisplayModifySaveProfileNewSourceFilePath();
                saveProfiles[profileIndex].SourceFilePath = consoleView.Read();

                consoleView.DisplayModifySaveProfileNewTargetFilePath();
                saveProfiles[profileIndex].TargetFilePath = consoleView.Read();

                consoleView.DisplayModifySaveProfileNewTypeOfSave();
                saveProfiles[profileIndex].TypeOfSave = consoleView.Read();

                List<long> sourcedirectoryinfo = SaveProfile.sourceDirectoryInfos(saveProfiles[profileIndex].SourceFilePath);
                saveProfiles[profileIndex].TotalFilesToCopy = (int)sourcedirectoryinfo[0];
                saveProfiles[profileIndex].TotalFilesSize = sourcedirectoryinfo[1];
                saveProfiles[profileIndex].NbFilesLeftToDo = (int)sourcedirectoryinfo[0];
                saveProfiles[profileIndex].Progression = 0;

                saveProfiles[profileIndex].State = "READY";

                SaveProfile.SaveProfiles("..\\..\\..\\logs\\state.json", saveProfiles);
                consoleView.DisplayModifySaveProfileSuccess();

            }
            catch (Exception ex)
            {
                consoleView.Error(ex.Message);
            }
            
        }
        public void ExecuteSaveProfile()
        {
            try
            {
                int profileIndex = GetProfileIndex();
                consoleView.DisplaySelectedProfileName(saveProfiles[profileIndex].Name);

                if (saveProfiles[profileIndex].State == "READY")
                {
                    saveProfiles[profileIndex].State = "IN PROGRESS";
                    SaveProfile.SaveProfiles("..\\..\\..\\logs\\state.json", saveProfiles);

                    if (saveProfiles[profileIndex].TypeOfSave == "full" || saveProfiles[profileIndex].TypeOfSave == "diff")
                    {
                        consoleView.DisplayBackupInProgress(saveProfiles[profileIndex].Name);
                        DateTime startTime = DateTime.Now;
                        SaveProfile.ExecuteSaveProfile(saveProfiles, saveProfiles[profileIndex], saveProfiles[profileIndex].TypeOfSave);
                        TimeSpan elapsedTime = DateTime.Now - startTime;

                        Logger.CreateLog(
                        saveProfiles[profileIndex].Name,
                        saveProfiles[profileIndex].SourceFilePath,
                        saveProfiles[profileIndex].TargetFilePath,
                        saveProfiles[profileIndex].TotalFilesSize,
                        elapsedTime.TotalMilliseconds
                        );

                    }
                    else
                    {
                        consoleView.DisplayExecuteSaveProfileTypeOfSaveError();
                    }

                    saveProfiles[profileIndex].State = "COMPLETED";
                    SaveProfile.SaveProfiles("..\\..\\..\\logs\\state.json", saveProfiles);
                    consoleView.DisplayExecuteSaveProfileSuccess();
                }
                else
                {
                    consoleView.DisplayExecuteSaveProfileStateError();
                }
            }
            catch (Exception ex)
            {
                consoleView.Error(ex.Message);
            }
        }
        public void DisplayLogs()
        {
            consoleView.DisplayLogsHeader();

            foreach (var log in Logger.GetLogs())
            {
                consoleView.DisplayLog(log);
                consoleView.printSeparator();
            }
        }

        public void Configuration()
        {
            consoleView.DisplayConfigurationMenu();
            string configChoice = consoleView.Read();

            switch (configChoice)
            {
                case "1":
                    consoleView.NotImplementedYet();
                    break;
                case "2":
                    ChooseLogFileFormat();
                    SaveDailyLogs();
                    break;
                case "3":
                    Exit();
                    break;
                default:
                    consoleView.DisplayMenuError();
                    break;
            }
        }

        private DailyLogs LoadDailyLogs()
        {
            DailyLogs loadedLogs = new DailyLogs();
            return loadedLogs;
        }

        private void SaveDailyLogs()
        {
            Logger.SaveLogs();
        }

        private void ChooseLogFileFormat()
        {
            consoleView.DisplayLogFileFormatMenu();
            string logFormatChoice = consoleView.Read();

            switch (logFormatChoice)
            {
                case "1":
                    Logger.SetLogFileFormat(LogFileFormat.Json);
                    consoleView.DisplayLogFileFormatSuccess("JSON");
                    break;
                case "2":
                    Logger.SetLogFileFormat(LogFileFormat.Xml);
                    consoleView.DisplayLogFileFormatSuccess("XML");
                    break;
                default:
                    consoleView.DisplayLogFileFormatError();
                    break;
            }
        }

        public void Exit() 
        {
            consoleView.Exit();
            argument = "exit";
            Environment.Exit(0);
        }

        public int GetProfileIndex()
        {
            consoleView.DisplayChooseSelectedProfile();

            // print all the profiles name
            for (int i = 0; i < saveProfiles.Count; i++)
            {
                consoleView.DisplayProfileIndexName(i, saveProfiles[i].Name);
            }

            string choice = consoleView.Read();
            int profileIndex = -1;

            for (int i = 0; i < saveProfiles.Count; i++)
            {
                if (int.Parse(choice) == i)
                {
                    profileIndex = i;
                }
            }
            return profileIndex;
        }
    }
}
