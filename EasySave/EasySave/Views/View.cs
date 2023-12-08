using System.Xml;
using System;

namespace EasySaveConsoleApp
{
    public class ConsoleView
    {
        private readonly Dictionary<string, string> printStringDictionary;

        /* Constructor */

        public ConsoleView(Dictionary<string, string> printStringDictionary)
        {
            this.printStringDictionary = printStringDictionary;
        }

        /* Print Methods */

        public void Print(string text)
        {
            Console.WriteLine(text);
        }

        public void PrintError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Print(text);
            Console.ResetColor();
        }

        public void PrintSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Print(text);
            Console.ResetColor();
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
            return Console.ReadLine();
        }

        /* Clarity Methods */

        public void Clear()
        {
            Console.Clear();
        }

        public void printSeparator()
        {
            Print("");
            Print(printStringDictionary["Separator"]);
            Print("");
        }

        /* All the methods below are used to display all the differents strings using the methods above */

        /* Initialisation */

        public void WelcomeMessage()
        {
            PrintInfo(printStringDictionary["WelcomeMessage"]);
            printSeparator();
        }

        public void ArgumentError()
        {
            PrintError(printStringDictionary["ArgumentError"]);
        }

        /* Common */

        public void DisplaySelectedProfileName(string saveProfileName)
        {
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
            PrintError(printStringDictionary["Error"] + error);
        }

        /* Menu */

        public void DisplayMenu()
        {
            PrintInfo(printStringDictionary["DisplayMenu_0"]);
            Print(printStringDictionary["DisplayMenu_1"]);
            Print(printStringDictionary["DisplayMenu_2"]);
            Print(printStringDictionary["DisplayMenu_3"]);
            Print(printStringDictionary["DisplayMenu_4"]);
            Print(printStringDictionary["DisplayMenu_5"]);
            Print(printStringDictionary["DisplayMenu_6"]);
            Print(printStringDictionary["DisplayMenu_7"]);
            Print(printStringDictionary["DisplayMenu_8"]);
            Print(printStringDictionary["DisplayMenu_9"]);
        }

        public void DisplayMenuError()
        {
            PrintError(printStringDictionary["DisplayMenuError"]);
        }

        /* Display Save Profiles */

        public void DisplaySaveProfiles(string[] saveProfiles)
        {
            Console.WriteLine($"Name:             {saveProfiles[0]}");
            Console.WriteLine($"SourceFilePath:   {saveProfiles[1]}");
            Console.WriteLine($"TargetFilePath:   {saveProfiles[2]}");
            Console.WriteLine($"State:            {saveProfiles[3]}");
            Console.WriteLine($"TotalFilesToCopy: {saveProfiles[4]}");
            Console.WriteLine($"TotalFilesSize:   {saveProfiles[5]}");
            Console.WriteLine($"NbFilesLeftToDo:  {saveProfiles[6]}");
            Console.WriteLine($"Progression:      {saveProfiles[7]}");
            Console.WriteLine($"TypeOfSave:       {saveProfiles[8]}");
        }


        public void DisplaySaveProfilesError()
        {
            PrintError(printStringDictionary["DisplaySaveProfilesError"]);
        }

        /* Modify Save Profile */

        public void DisplayModifySaveProfileNewName()
        {
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewName"]);
        }

        public void DisplayModifySaveProfileNewSourceFilePath()
        {
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewSourceFilePath"]);
        }

        public void DisplayModifySaveProfileNewTargetFilePath()
        {
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewTargetFilePath"]);
        }

        public void DisplayModifySaveProfileNewTypeOfSave()
        {
            PrintInfo(printStringDictionary["DisplayModifySaveProfileNewTypeOfSave"]);
        }


        public void DisplayModifySaveProfileSuccess()
        {
            PrintSuccess(printStringDictionary["DisplayModifySaveProfileSuccess"]);
        }

        /* Execute Save Profile */

        public void DisplayBackupInProgress(string name)
        {
            PrintInfo($"Backing up profile {name}...");
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
            PrintInfo("--------------- LOGS ---------------");
        }

        public void DisplayLog(DailyLog log)
        {
            Console.WriteLine($"Name:             {log.Name}");
            Console.WriteLine($"FileSource:       {log.FileSource}");
            Console.WriteLine($"FileTarget:       {log.FileTarget}");
            Console.WriteLine($"FileSize:         {log.FileSize}");
            Console.WriteLine($"FileTransferTime: {log.FileTransferTime}");
            Console.WriteLine($"Time:             {log.Time}");
        }

        /* Help */

        public void Help()
        {
            PrintInfo(printStringDictionary["Help_0"]);
            PrintInfo(printStringDictionary["Help_1"]);
            PrintInfo(printStringDictionary["Help_2"]);
            PrintInfo(printStringDictionary["Help_3"]);
            PrintInfo(printStringDictionary["Help_4"]);
            PrintInfo(printStringDictionary["Help_5"]);
            PrintInfo(printStringDictionary["Help_6"]);
            PrintInfo(printStringDictionary["Help_7"]);
            PrintInfo(printStringDictionary["Help_8"]);
            PrintInfo(printStringDictionary["Help_9"]);
            printSeparator();
        }

        public void DisplayConfigurationMenu()
        {
            PrintInfo("Please choose an option:");
            Print("1. Change language");
            Print("2. Change logs file format");
            Print("3. Exit");
            Print("");
        }

        public void DisplayLogFileFormatMenu()
        {
            PrintInfo("Please choose the log file format:");
            Print("1. JSON");
            Print("2. XML");
            Print("");
        }

        public void DisplayLogFileFormatSuccess(string format)
        {
            PrintSuccess($"Log file format set to {format}");
        }

        public void DisplayLogFileFormatError()
        {
            PrintError("Please enter a valid option for log file format.");
        }

        public void Exit()
        {
            PrintInfo(printStringDictionary["Exit"]);
        }

        /* Not implemented yet */

        public void NotImplementedYet()
        {
            PrintError(printStringDictionary["NotImplementedYet"]);
            printSeparator();
        }
    }
}
