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
            Print(printStringDictionary["Console_Separator"]);
            Print("");
        }

        /* All the methods below are used to display all the differents strings using the methods above */

        /* Initialisation */

        public void WelcomeMessage(string version)
        {
            PrintInfo(printStringDictionary["Console_WelcomeMessage"] + version);
            PrintSeparator();
        }

        public void ArgumentError()
        {
            PrintError(printStringDictionary["Console_ArgumentError"]);
        }

        /* Common */

        public void DisplaySelectedProfileName(string saveProfileName)
        {
            Print("");
            PrintInfo(printStringDictionary["Console_DisplaySelectedProfileName"] + saveProfileName);
        }

        public void DisplayChooseSelectedProfile()
        {
            Print(printStringDictionary["Console_DisplayChooseSelectedProfile"]);
        }

        public void DisplayProfileIndexName(int index, string saveProfileName)
        {
            Print(index + ". " + saveProfileName);
        }

        public void Error(string error)
        {
            Print("");
            PrintError(printStringDictionary["Console_Error"] + error);
        }

        /* Menu */

        public void DisplayMenu()
        {
            PrintInfo(printStringDictionary["Console_DisplayMenu_Header"]);
            Print(printStringDictionary["Console_DisplayMenu_DislaySaveProfiles"]);
            //Print(printStringDictionary["Console_DisplayMenu_CreateSaveProfile"]);
            //Print(printStringDictionary["Console_DisplayMenu_DeleteSaveProfile"]);
            Print(printStringDictionary["Console_DisplayMenu_ModifySaveProfile"]);
            Print(printStringDictionary["Console_DisplayMenu_ExecuteSaveProfile"]);
            Print(printStringDictionary["Console_DisplayMenu_DisplayLogs"]);
            Print(printStringDictionary["Console_DisplayMenu_Help"]);
            Print(printStringDictionary["Console_DisplayMenu_Configuration"]);
            Print(printStringDictionary["Console_DisplayMenu_Clear"]);
            Print(printStringDictionary["Console_DisplayMenu_Exit"]);
            Print("");
        }

        public void DisplayMenuError()
        {
            PrintError(printStringDictionary["Console_DisplayMenuError"]);
        }

        /* Display Save Profiles */

        public void DisplaySaveProfiles(List<string> saveProfilesInfos)
        {
            Print("");
            Print(printStringDictionary["Console_DisplaySaveProfiles_Name"] + saveProfilesInfos[0]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_SourceFilePath"] + saveProfilesInfos[1]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_TargetFilePath"] + saveProfilesInfos[2]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_State"] + saveProfilesInfos[3]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_TotalFilesToCopy"] + saveProfilesInfos[4]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_TotalFilesSize"] + saveProfilesInfos[5]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_NbFilesLeftToDo"] + saveProfilesInfos[6]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_Progression"] + saveProfilesInfos[7]);
            Print(printStringDictionary["Console_DisplaySaveProfiles_TypeOfSave"] + saveProfilesInfos[8]);
            PrintSeparator();
        }


        public void DisplaySaveProfilesError()
        {
            PrintError(printStringDictionary["Console_DisplaySaveProfilesError"]);
        }

        /* Modify Save Profile */

        public void DisplayModifySaveProfileNewName()
        {
            Print("");
            PrintInfo(printStringDictionary["Console_DisplayModifySaveProfileNewName"]);
        }

        public void DisplayModifySaveProfileNewSourceFilePath()
        {
            Print("");
            PrintInfo(printStringDictionary["Console_DisplayModifySaveProfileNewSourceFilePath"]);
        }

        public void DisplayModifySaveProfileNewTargetFilePath()
        {
            Print("");
            PrintInfo(printStringDictionary["Console_DisplayModifySaveProfileNewTargetFilePath"]);
        }

        public void DisplayModifySaveProfileNewTypeOfSave()
        {
            Print("");
            PrintInfo(printStringDictionary["Console_DisplayModifySaveProfileNewTypeOfSave"]);
        }


        public void DisplayModifySaveProfileSuccess()
        {
            PrintSuccess(printStringDictionary["Console_DisplayModifySaveProfileSuccess"]);
        }

        /* Execute Save Profile */

        public void DisplayBackupInProgress(string name)
        {
            PrintInfo(printStringDictionary["Console_DisplayBackupInProgress"] + name + printStringDictionary["dots"]);
        }

        public void DisplayExecuteSaveProfileSuccess()
        {
            PrintSuccess(printStringDictionary["Console_DisplayExecuteSaveProfileSuccess"]);
        }

        public void DisplayExecuteSaveProfileStateError()
        {
            PrintError(printStringDictionary["Console_DisplayExecuteSaveProfileStateError"]);
        }

        public void DisplayExecuteSaveProfileTypeOfSaveError()
        {
            PrintError(printStringDictionary["Console_DisplayExecuteSaveProfileTypeOfSaveError"]);
        }

        /* Display Logs */

        public void DisplayLogsHeader()
        {
            PrintInfo(printStringDictionary["Console_DisplayLogsHeader"]);
        }

        public void DisplayLog(DailyLog log)
        {
            Print("");
            Print(printStringDictionary["Console_DisplayLog_Name"] + log.Name);
            Print(printStringDictionary["Console_DisplayLog_SourceFilePath"] + log.SourceFilePath);
            Print(printStringDictionary["Console_DisplayLog_TargetFilePath"] + log.TargetFilePath);
            Print(printStringDictionary["Console_DisplayLog_FileSize"] + log.FileSize);
            Print(printStringDictionary["Console_DisplayLog_FileTransferTime"] + log.FileTransferTime);
            Print(printStringDictionary["Console_DisplayLog_Time"] + log.Time);
            PrintSeparator();
        }

        public void DisplayLogError()
        {
            PrintError(printStringDictionary["Console_DisplayLogError"]);
        }

        /* Help */

        public void Help()
        {
            PrintInfo(printStringDictionary["Console_Help_Header"]);
            Print("");
            PrintInfo(printStringDictionary["Console_Help_Menu"]);
            PrintInfo(printStringDictionary["Console_Help_DislaySaveProfiles"]);
            // PrintInfo(printStringDictionary["Console_Help_CreateSaveProfile"]);
            PrintInfo(printStringDictionary["Console_Help_ModifySaveProfile"]);
            PrintInfo(printStringDictionary["Console_Help_ExecuteSaveProfile"]);
            PrintInfo(printStringDictionary["Console_Help_DisplayLogs"]);
            PrintInfo(printStringDictionary["Console_Help_Help"]);
            PrintInfo(printStringDictionary["Console_Help_Configuration"]);
            PrintInfo(printStringDictionary["Console_Help_Exit"]);
            PrintSeparator();
        }

        public void DisplayConfigurationMenu(string language, string logformat)
        {
            Print(printStringDictionary["Console_DisplayConfigurationMenu_DisplayLanguage"] + language);
            Print(printStringDictionary["Console_DisplayConfigurationMenu_DisplayLogFormat"] + logformat);
            Print("");
            PrintInfo(printStringDictionary["Console_DisplayConfigurationMenu_Header"]);
            Print(printStringDictionary["Console_DisplayConfigurationMenu_ChangeLanguage"]);
            Print(printStringDictionary["Console_DisplayConfigurationMenu_ChangeLogFormat"]);
            Print(printStringDictionary["Console_DisplayConfigurationMenu_Back"]);
            Print("");
        }

        public void DisplayLanguageMenu()
        {
            PrintSeparator();
            PrintInfo(printStringDictionary["Console_DisplayLanguageMenu_Header"]);
            Print("");
            Print(printStringDictionary["Console_DisplayLanguageMenu_French"]);
            Print(printStringDictionary["Console_DisplayLanguageMenu_English"]);
            PrintSeparator();
        }

        public void DisplayLanguageSuccess(string language)
        {
            PrintSuccess(printStringDictionary["Console_DisplayLanguageSuccess"] + language);
        }

        public void DisplayLanguageError()
        {
            PrintError(printStringDictionary["Console_DisplayLanguageError"]);
        }

        public void DisplayLogFileFormatMenu()
        {
            PrintSeparator();
            PrintInfo(printStringDictionary["Console_DisplayLogFileFormatMenu_Header"]);
            Print("");
            Print(printStringDictionary["Console_DisplayLogFileFormatMenu_Json"]);
            Print(printStringDictionary["Console_DisplayLogFileFormatMenu_Xml"]);
            PrintSeparator();
        }

        public void DisplayLogFileFormatSuccess(string format)
        {
            PrintSuccess(printStringDictionary["Console_DisplayLogFileFormatSuccess"] + format);
        }

        public void DisplayLogFileFormatError()
        {
            PrintError(printStringDictionary["Console_DisplayLogFileFormatError"]);
        }

        public void Exit()
        {
            PrintInfo(printStringDictionary["Console_Exit"]);
        }

        /* Not implemented yet */

        public void NotImplementedYet()
        {
            PrintError(printStringDictionary["Console_NotImplementedYet"]);
        }
    }
}
