using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.ViewModels;

namespace EasySaveWPF.MVVM.Views
{
    public class ConsoleView
    {
        private Dictionary<string, string> printStringDictionary;
        /* Constructor */

        public ConsoleView(Dictionary<string, string> printStringDictionary)
        {
            this.printStringDictionary = printStringDictionary;
        }

        public void SetprintStringDictionary(Dictionary<string, string> printStringDictionary)
        {
            this.printStringDictionary = printStringDictionary;
        }

        /* Print Methods */

        public void Print(string text)
        {
            Console.WriteLine(text ?? string.Empty);
        }


        public void PrintError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Print(text);
            Console.ResetColor();
            PrintSeparator();
        }

        public void PrintSuccess(string text)
        {
            PrintSeparator();
            Console.ForegroundColor = ConsoleColor.Green;
            Print(text);
            Console.ResetColor();
            PrintSeparator();
        }

        public void PrintWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Print(text);
            Console.ResetColor();
        }

        public void PrintInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Print(text);
            Console.ResetColor();
        }

        /* Read Methods */

        public string Read()
        {
            string input = Console.ReadLine();
            return input ?? string.Empty; // or handle null in a way that makes sense for your application
        }

        /* Clarity Methods */

        public void Clear()
        {
            Console.Clear();
        }

        public void PrintSeparator()
        {
            Print("");
            Print(printStringDictionary["Separator"]);
            Print("");
        }

        /* All the methods below are used to display all the differents strings using the methods above */

        /* Initialisation */

        public void WelcomeMessage(string version)
        {
            PrintInfo(printStringDictionary["WelcomeMessage"] + version);
            PrintSeparator();
        }

        public void ArgumentError()
        {
            PrintError(printStringDictionary["ArgumentError"]);
        }

        /* Common */

        public void DisplaySelectedProfileName(string saveProfileName)
        {
            Print("");
            PrintInfo(printStringDictionary["DisplaySelectedProfileName"] + saveProfileName);
        }

        public void DisplayChooseSelectedProfile()
        {
            Print(printStringDictionary["DisplayChooseSelectedProfile"]);
        }

        public void DisplayProfileIndexName(int index, string saveProfileName)
        {
            Print(index + ". " + saveProfileName);
        }

        public void Error(string error)
        {
            Print("");
            PrintError(printStringDictionary["Error"] + error);
        }

        /* Menu */

        public void DisplayMenu()
        {
            PrintInfo(printStringDictionary["DisplayMenu_Header"]);
            Print(printStringDictionary["DisplayMenu_DislaySaveProfiles"]);
            // Print(printStringDictionary["DisplayMenu_CreateSaveProfile"]);
            Print(printStringDictionary["DisplayMenu_ModifySaveProfile"]);
            Print(printStringDictionary["DisplayMenu_ExecuteSaveProfile"]);
            Print(printStringDictionary["DisplayMenu_DisplayLogs"]);
            Print(printStringDictionary["DisplayMenu_Help"]);
            Print(printStringDictionary["DisplayMenu_Configuration"]);
            Print(printStringDictionary["DisplayMenu_Clear"]);
            Print(printStringDictionary["DisplayMenu_Exit"]);
            Print("");
        }

        public void DisplayMenuError()
        {
            PrintError(printStringDictionary["DisplayMenuError"]);
        }

        /* Display Save Profiles */

        public void DisplaySaveProfiles(List<string> saveProfilesInfos)
        {
            Print("");
            Print(printStringDictionary["DisplaySaveProfiles_Name"] + saveProfilesInfos[0]);
            Print(printStringDictionary["DisplaySaveProfiles_SourceFilePath"] + saveProfilesInfos[1]);
            Print(printStringDictionary["DisplaySaveProfiles_TargetFilePath"] + saveProfilesInfos[2]);
            Print(printStringDictionary["DisplaySaveProfiles_State"] + saveProfilesInfos[3]);
            Print(printStringDictionary["DisplaySaveProfiles_TotalFilesToCopy"] + saveProfilesInfos[4]);
            Print(printStringDictionary["DisplaySaveProfiles_TotalFilesSize"] + saveProfilesInfos[5]);
            Print(printStringDictionary["DisplaySaveProfiles_NbFilesLeftToDo"] + saveProfilesInfos[6]);
            Print(printStringDictionary["DisplaySaveProfiles_Progression"] + saveProfilesInfos[7]);
            Print(printStringDictionary["DisplaySaveProfiles_TypeOfSave"] + saveProfilesInfos[8]);
            PrintSeparator();
        }


        public void DisplaySaveProfilesError()
        {
            PrintError(printStringDictionary["DisplaySaveProfilesError"]);
        }

        /* Modify Save Profile */

        public void DisplayModifySaveProfileNewName()
        {
            Print("");
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewName"]);
        }

        public void DisplayModifySaveProfileNewSourceFilePath()
        {
            Print("");
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewSourceFilePath"]);
        }

        public void DisplayModifySaveProfileNewTargetFilePath()
        {
            Print("");
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewTargetFilePath"]);
        }

        public void DisplayModifySaveProfileNewTypeOfSave()
        {
            Print("");
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewTypeOfSave"]);
        }


        public void DisplayModifySaveProfileSuccess()
        {
            PrintSuccess(printStringDictionary["DisplayModifySaveProfileSuccess"]);
        }

        /* Execute Save Profile */

        public void DisplayBackupInProgress(string name)
        {
            PrintInfo(printStringDictionary["DisplayBackupInProgress"] + name + printStringDictionary["dots"]);
        }

        public void DisplayExecuteSaveProfileSuccess()
        {
            PrintSuccess(printStringDictionary["DisplayExecuteSaveProfileSuccess"]);
        }

        public void DisplayExecuteSaveProfileStateError()
        {
            PrintError(printStringDictionary["DisplayExecuteSaveProfileStateError"]);
        }

        public void DisplayExecuteSaveProfileTypeOfSaveError()
        {
            PrintError(printStringDictionary["DisplayExecuteSaveProfileTypeOfSaveError"]);
        }

        /* Display Logs */

        public void DisplayLogsHeader()
        {
            PrintInfo(printStringDictionary["DisplayLogsHeader"]);
        }

        public void DisplayLog(DailyLog log)
        {
            Print("");
            Print(printStringDictionary["DisplayLog_Name"] + log.Name);
            Print(printStringDictionary["DisplayLog_SourceFilePath"] + log.SourceFilePath);
            Print(printStringDictionary["DisplayLog_TargetFilePath"] + log.TargetFilePath);
            Print(printStringDictionary["DisplayLog_FileSize"] + log.FileSize);
            Print(printStringDictionary["DisplayLog_FileTransferTime"] + log.FileTransferTime);
            Print(printStringDictionary["DisplayLog_Time"] + log.Time);
            PrintSeparator();
        }

        public void DisplayLogError()
        {
            PrintError(printStringDictionary["DisplayLogError"]);
        }

        /* Help */

        public void Help()
        {
            PrintInfo(printStringDictionary["Help_Header"]);
            Print("");
            PrintInfo(printStringDictionary["Help_Menu"]);
            PrintInfo(printStringDictionary["Help_DislaySaveProfiles"]);
            // PrintInfo(printStringDictionary["Help_CreateSaveProfile"]);
            PrintInfo(printStringDictionary["Help_ModifySaveProfile"]);
            PrintInfo(printStringDictionary["Help_ExecuteSaveProfile"]);
            PrintInfo(printStringDictionary["Help_DisplayLogs"]);
            PrintInfo(printStringDictionary["Help_Help"]);
            PrintInfo(printStringDictionary["Help_Configuration"]);
            PrintInfo(printStringDictionary["Help_Exit"]);
            PrintSeparator();
        }

        public void DisplayConfigurationMenu(string language, string logformat)
        {
            Print(printStringDictionary["DisplayConfigurationMenu_DisplayLanguage"] + language);
            Print(printStringDictionary["DisplayConfigurationMenu_DisplayLogFormat"] + logformat);
            Print("");
            PrintInfo(printStringDictionary["DisplayConfigurationMenu_Header"]);
            Print(printStringDictionary["DisplayConfigurationMenu_ChangeLanguage"]);
            Print(printStringDictionary["DisplayConfigurationMenu_ChangeLogFormat"]);
            Print(printStringDictionary["DisplayConfigurationMenu_Back"]);
            Print("");
        }

        public void DisplayLanguageMenu()
        {
            PrintSeparator();
            PrintInfo(printStringDictionary["DisplayLanguageMenu_Header"]);
            Print("");
            Print(printStringDictionary["DisplayLanguageMenu_French"]);
            Print(printStringDictionary["DisplayLanguageMenu_English"]);
            PrintSeparator();
        }

        public void DisplayLanguageSuccess(string language)
        {
            PrintSuccess(printStringDictionary["DisplayLanguageSuccess"] + language);
        }

        public void DisplayLanguageError()
        {
            PrintError(printStringDictionary["DisplayLanguageError"]);
        }

        public void DisplayLogFileFormatMenu()
        {
            PrintSeparator();
            PrintInfo(printStringDictionary["DisplayLogFileFormatMenu_Header"]);
            Print("");
            Print(printStringDictionary["DisplayLogFileFormatMenu_Json"]);
            Print(printStringDictionary["DisplayLogFileFormatMenu_Xml"]);
            PrintSeparator();
        }

        public void DisplayLogFileFormatSuccess(string format)
        {
            PrintSuccess(printStringDictionary["DisplayLogFileFormatSuccess"] + format);
        }

        public void DisplayLogFileFormatError()
        {
            PrintError(printStringDictionary["DisplayLogFileFormatError"]);
        }

        public void Exit()
        {
            PrintInfo(printStringDictionary["Exit"]);
        }

        /* Not implemented yet */

        public void NotImplementedYet()
        {
            PrintError(printStringDictionary["NotImplementedYet"]);
        }
    }
}
