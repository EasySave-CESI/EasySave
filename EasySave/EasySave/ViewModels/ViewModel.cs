using System.Resources;
using EasySave.Models;
using EasySave.Views;

namespace EasySave.ViewModels
{
    public class MainViewModel
    {
        // All the fills will be in a file called EasySave in AppData
        public string EasySaveFileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave"; // Get the path of the AppData folder and add the EasySave folder
        public string EasySaveFileConfigDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\Config"; // Get the path of the AppData folder and add the EasySave Config folder
        public string EasySaveFileProfilesDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\Profiles"; // Get the path of the AppData folder and add the EasySave Profiles folder
        public string EasySaveFileLogsDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\Logs"; // Get the path of the AppData folder and add the EasySave Logs folder

        public string configFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\Config\\config.xml"; // Get the path of the AppData folder and add the config 
        public string stateFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\Profiles\\state.json"; // Get the path of the AppData folder and add the profiles

        public Configuration configuration { get; set; }
        public ConsoleView consoleView { get; set; }
        public List<SaveProfile> SaveProfiles { get; set; }
        public LanguageConfiguration languageConfiguration { get; set; }

        public string argument { get; set; }
        public string language { get; set; }
        public string fulllanguagename { get; set; }
        public string logformat { get; set; }
        public Dictionary<string, string> printStrings { get; set; }
        public DailyLogs Logger { get; set; }


        public MainViewModel() // Constructor by default, will lead to the menu
        {
            Initialization(); // Initialize thec configuration, language, save profiles, logs and console view
            argument = "menu"; // Set the argument to menu
            Menu(); // Load the menu
        }

        public MainViewModel(string userargument) // Constructor with an argument, will lead to the function called
        {
            Initialization(); // Initialize thec configuration, language, save profiles, logs and console view
            argument = userargument; // Set the argument to the user argument
            LoadFunction(); // Load the function called
        }

        public void Initialization()
        {
            // Create the EasySave directories if it doesn't exist
            if (!System.IO.Directory.Exists(EasySaveFileDirectoryPath)) { System.IO.Directory.CreateDirectory(EasySaveFileDirectoryPath); }
            if (!System.IO.Directory.Exists(EasySaveFileConfigDirectoryPath)) { System.IO.Directory.CreateDirectory(EasySaveFileConfigDirectoryPath); }
            if (!System.IO.Directory.Exists(EasySaveFileProfilesDirectoryPath)) { System.IO.Directory.CreateDirectory(EasySaveFileProfilesDirectoryPath); }
            if (!System.IO.Directory.Exists(EasySaveFileLogsDirectoryPath)) { System.IO.Directory.CreateDirectory(EasySaveFileLogsDirectoryPath); }

            // Load the configuration parameters
            try
               {configuration = new Configuration(configFilePath);
                language = configuration.language;
                logformat = configuration.logFormat;}
            catch (Exception ex) // If an error occurs, the configuration parameters are set to default
               {language = "en";
                logformat = "json";}

            // Load the language strings
            languageConfiguration = new LanguageConfiguration();
            switch (language)
               {case "en":
                    fulllanguagename = "English";
                    printStrings = languageConfiguration.printStrings_en;
                    break;

                case "fr":
                    fulllanguagename = "Français";
                    printStrings = languageConfiguration.printStrings_fr;
                    break;

                default:
                    fulllanguagename = "English";
                    printStrings = languageConfiguration.printStrings_en;
                    break;}

            // If the file doesn't exist create it
            if (!File.Exists(stateFilePath) || new FileInfo(stateFilePath).Length == 0)
               {SaveProfile.CreateProfilesFile(stateFilePath);}

            // Load the save profiles
            SaveProfiles = SaveProfile.LoadProfiles(stateFilePath);

            // Load the logs and the console view
            Logger = LoadDailyLogs();
            consoleView = new ConsoleView(printStrings);
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
                    consoleView.Help();
                    break;
                case "config":
                    Config();
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
                consoleView.PrintSeparator();
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
                        Config();
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
            if (SaveProfiles == null)
            {
                consoleView.DisplaySaveProfilesError();
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
                        consoleView.DisplaySaveProfiles(list.ToArray());
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

                consoleView.DisplaySelectedProfileName(SaveProfiles[profileIndex].Name);

                consoleView.DisplayModifySaveProfileNewName();
                SaveProfiles[profileIndex].Name = consoleView.Read();

                consoleView.DisplayModifySaveProfileNewSourceFilePath();
                SaveProfiles[profileIndex].SourceFilePath = consoleView.Read();

                consoleView.DisplayModifySaveProfileNewTargetFilePath();
                SaveProfiles[profileIndex].TargetFilePath = consoleView.Read();

                consoleView.DisplayModifySaveProfileNewTypeOfSave();
                SaveProfiles[profileIndex].TypeOfSave = consoleView.Read();

                List<long> sourcedirectoryinfo = SaveProfile.sourceDirectoryInfos(SaveProfiles[profileIndex].SourceFilePath);
                SaveProfiles[profileIndex].TotalFilesToCopy = (int)sourcedirectoryinfo[0];
                SaveProfiles[profileIndex].TotalFilesSize = sourcedirectoryinfo[1];
                SaveProfiles[profileIndex].NbFilesLeftToDo = (int)sourcedirectoryinfo[0];
                SaveProfiles[profileIndex].Progression = 0;

                SaveProfiles[profileIndex].State = "READY";

                SaveProfile.SaveProfiles(stateFilePath, SaveProfiles);
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
                consoleView.DisplaySelectedProfileName(SaveProfiles[profileIndex].Name);

                if (SaveProfiles[profileIndex].State == "READY")
                {
                    SaveProfiles[profileIndex].State = "IN PROGRESS";
                    SaveProfile.SaveProfiles(stateFilePath, SaveProfiles);

                    if (SaveProfiles[profileIndex].TypeOfSave == "full" || SaveProfiles[profileIndex].TypeOfSave == "diff")
                    {
                        consoleView.DisplayBackupInProgress(SaveProfiles[profileIndex].Name);
                        SaveProfile.ExecuteSaveProfile(SaveProfiles, SaveProfiles[profileIndex], SaveProfiles[profileIndex].TypeOfSave);

                        DateTime startTime = DateTime.Now;
                        TimeSpan elapsedTime = DateTime.Now - startTime;

                        Logger.CreateLog(
                        SaveProfiles[profileIndex].Name,
                        SaveProfiles[profileIndex].SourceFilePath,
                        SaveProfiles[profileIndex].TargetFilePath,
                        SaveProfiles[profileIndex].TotalFilesSize,
                        elapsedTime.TotalMilliseconds
                        );
                    }
                    else
                    {
                        consoleView.DisplayExecuteSaveProfileTypeOfSaveError();
                    }

                    SaveProfiles[profileIndex].State = "COMPLETED";
                    SaveProfile.SaveProfiles(stateFilePath, SaveProfiles);
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
            }
        }

        public void Config()
        {
            consoleView.DisplayConfigurationMenu(fulllanguagename, logformat);
            string configChoice = consoleView.Read();

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
                    consoleView.Clear();
                    break;
                default:
                    consoleView.DisplayMenuError();
                    break;
            }
        }

        private DailyLogs LoadDailyLogs()
        {
            DailyLogs loadedLogs = new DailyLogs(EasySaveFileLogsDirectoryPath, logformat);
            return loadedLogs;
        }

        private void SaveDailyLogs()
        {
            Logger.SaveLogs();
        }

        private void ChooseLanguage()
        {
            consoleView.DisplayLanguageMenu();
            string languageChoice = consoleView.Read();

            switch (languageChoice)
            {
                case "1":
                    language = "fr";
                    Configuration.WriteConfig(configFilePath, language, logformat);
                    consoleView.SetprintStringDictionary(languageConfiguration.printStrings_fr);
                    fulllanguagename = "Français";
                    consoleView.Clear();
                    consoleView.DisplayLanguageSuccess(fulllanguagename);
                    break;
                case "2":
                    language = "en";
                    Configuration.WriteConfig(configFilePath, language, logformat);
                    consoleView.SetprintStringDictionary(languageConfiguration.printStrings_en);
                    fulllanguagename = "English";
                    consoleView.Clear();
                    consoleView.DisplayLanguageSuccess(fulllanguagename);
                    break;
                default:
                    consoleView.DisplayLanguageError();
                    break;
            }
        }

        private void ChooseLogFileFormat()
        {
            consoleView.DisplayLogFileFormatMenu();
            string logFormatChoice = consoleView.Read();

            switch (logFormatChoice)
            {
                case "1":
                    logformat = "json";
                    Configuration.WriteConfig(configFilePath, language, logformat);
                    Logger.SetLogFileFormat(LogFileFormat.Json);
                    consoleView.Clear();
                    consoleView.DisplayLogFileFormatSuccess(logformat);
                    break;
                case "2":
                    logformat = "xml";
                    Configuration.WriteConfig(configFilePath, language, logformat);
                    Logger.SetLogFileFormat(LogFileFormat.Xml);
                    consoleView.Clear();
                    consoleView.DisplayLogFileFormatSuccess(logformat);
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
            for (int i = 0; i < SaveProfiles.Count; i++)
            {
                consoleView.DisplayProfileIndexName(i+1, SaveProfiles[i].Name);
            }

            string choice = consoleView.Read();

            int profileIndex = -1;

            for (int i = 0; i < SaveProfiles.Count; i++)
            {
                if (int.Parse(choice) == i+1)
                {
                    profileIndex = i;
                }
            }
            return profileIndex;
        }
    }
}
