using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasySaveConsoleApp
{
    public class ConsoleView
    {
        /* Constructor */

        public ConsoleView() { }

        /* Print Methods */

        public void Print(string text)
        {
            Console.WriteLine(text);
        }

        public void PrintError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void PrintSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void PrintWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void PrintInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
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
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
        }

        /* All the methods below are used to display all the differents strings using the methods above */

        /* Initialisation */

        public void WelcomeMessage()
        {
            PrintInfo("Welcome to EasySave!");
            printSeparator();
        }

        public void ArgumentError()
        {
            PrintError("Error: Argument not recognized");
        }

        /* Common */

        public void DisplaySelectedProfileName(string saveProfileName)
        {
            PrintInfo("You have selected the save profile: " + saveProfileName);
        }

        public void DisplayChooseSelectedProfile()
        {
            Print("Select the save profile you want to modify by entering its index");
        }

        public void DisplayProfileIndexName(int index, string saveProfileName)
        {
            Print(index + ". " + saveProfileName);
        }

        public void Error(string error)
        {
            PrintError("Error: " + error);
        }

        /* Menu */

        public void DisplayMenu()
        {
            PrintInfo("Please choose an option:");
            Print("1. Display the save profiles");
            Print("2. Create a save profile");
            Print("3. Modify a save profile");
            Print("4. Execute a save profile");
            Print("5. Display the logs");
            Print("6. Help");
            Print("7. Configuration");
            Print("8. Clear the console");
            Print("9. Exit");
            Print("");
        }

        

        public void DisplayMenuError()
        {
            PrintError("Please enter a valid option.");
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
            PrintError("There is no save profile.");
        }

        /* Modify Save Profile */

        public void DisplayModifySaveProfileNewName()
        {
            PrintInfo("Please enter the new name of the save profile:");
        }

        public void DisplayModifySaveProfileNewSourceFilePath()
        {
            PrintInfo("Please enter the new source file path of the save profile:");
        }

        public void DisplayModifySaveProfileNewTargetFilePath()
        {
            PrintInfo("Please enter the new target file path of the save profile:");
        }

        public void DisplayModifySaveProfileNewTypeOfSave()
        {
            PrintInfo("Please enter the new type of save of the save profile (full or diff):");
        }


        public void DisplayModifySaveProfileSuccess()
        {
            PrintSuccess("The save profile has been modified successfully.");
        }

        /* Execute Save Profile */

        public void DisplayBackupInProgress(string name)
        {
            PrintInfo($"Backing up profile {name}...");
        }

        public void DisplayExecuteSaveProfileSuccess()
        {
            PrintSuccess("The save profile has been executed successfully.");
        }

        public void DisplayExecuteSaveProfileStateError()
        {
            PrintError("Error: The profile is not ready");
        }

        public void DisplayExecuteSaveProfileTypeOfSaveError()
        {
            PrintError("Error: The type of save is not valid");
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
            PrintInfo("--------------- HELP ---------------");
            PrintInfo("menu:     Display the menu");
            PrintInfo("dsp:      Display the save profiles");
            PrintInfo("csp:      Create a save profile");
            PrintInfo("msp:      Modify a save profile");
            PrintInfo("esp:      Execute a save profile");
            PrintInfo("dl:       Display the logs");
            PrintInfo("help:     Display the help");
            PrintInfo("config:   Display the configuration");
            PrintInfo("exit:     Exit the program");
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
            PrintInfo("Thank you for using EasySave");
        }

        /* Not implemented yet */

        public void NotImplementedYet()
        {
            PrintError("Not implemented yet.");
            printSeparator();
        }
    }
}
