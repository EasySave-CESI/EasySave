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
        public string Function { get; set; }


        public MainViewModel()
        {
            consoleView = new ConsoleView();
            saveProfiles = SaveProfile.LoadProfiles("..\\..\\..\\logs\\state.json");

            consoleView.print("Welcome to EasySave");
            Menu();
        }

        public MainViewModel(string function)
        {
            consoleView = new ConsoleView();
            saveProfiles = SaveProfile.LoadProfiles("..\\..\\..\\logs\\state.json");
            Function = function;

            consoleView.print("Welcome to EasySave");
            Main();
        }

        public void Main()
        {
            switch (Function)
            {
                case "menu":
                    Menu();
                    break;
                case "printSaveProfiles":
                    PrintSaveProfiles();
                    break;
                case "createSaveProfile":
                    CreateSaveProfile();
                    break;
                case "modifySaveProfile":
                    ModifySaveProfile();
                    break;
                case "executeSaveProfile":
                    ExecuteSaveProfile();
                    break;
                case "printLogs":
                    PrintLogs();
                    break;
                case "help":
                    Help();
                    break;
                case "configuration":
                    Configuration();
                    break;
                default:
                    consoleView.printError("Error: No function found");
                    break;
            }   
        }

        public void Menu()
        {
            consoleView.print("Please choose an option:");
            consoleView.print("1. Display the save profiles");
            consoleView.print("2. Create a save profile");
            consoleView.print("3. Modify a save profile");
            consoleView.print("4. Execute a save profile");
            consoleView.print("5. Display the logs");
            consoleView.print("6. Help");
            consoleView.print("7. Configuration");
            consoleView.print("8. Exit");
            string choice = consoleView.read();
            switch (choice)
            {
                case "1":
                    PrintSaveProfiles();
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
                    PrintLogs();
                    break;
                case "6":
                    Help();
                    break;
                case "7":
                    Configuration();
                    break;
                case "8":
                    Exit();
                    break;
                default:
                    consoleView.printError("Error: No function found");
                    break;
            }
        }

        public void PrintSaveProfiles() 
        {
            if (saveProfiles == null)
            {
                consoleView.print("There is no save profile");
            }
            else
            {
                {
                    foreach (SaveProfile profile in saveProfiles)
                    {
                        consoleView.print("Name: " + profile.Name);
                        consoleView.print("SourceFilePath: " + profile.SourceFilePath);
                        consoleView.print("TargetFilePath: " + profile.TargetFilePath);
                        consoleView.print("State: " + profile.State);
                        consoleView.print("TotalFilesToCopy: " + profile.TotalFilesToCopy);
                        consoleView.print("TotalFilesSize: " + profile.TotalFilesSize);
                        consoleView.print("NbFilesLeftToDo: " + profile.NbFilesLeftToDo);
                        consoleView.print("Progression: " + profile.Progression);
                        consoleView.print("TypeOfSave: " + profile.TypeOfSave);
                        consoleView.print("");
                    }
                }
            }

        }
        public void CreateSaveProfile() 
        {
            consoleView.print("This function is not available yet");
        }
        public void ModifySaveProfile() 
        {
            consoleView.print("Select the save profile you want to modify by entering its index");
            // print all the profiles name
            for (int i = 0; i < saveProfiles.Count; i++)
            {
                consoleView.print((i) + ". " + saveProfiles[i].Name);
            }

            try
            {
                string choice = consoleView.read();
                int choiceInt = int.Parse(choice);
                string profileName = string.Empty;
                int profileIndex = -1;


                for (int i = 0; i < saveProfiles.Count; i++)
                {
                    string tempProfileName = saveProfiles[i].Name;
                    if (choice == tempProfileName || choiceInt == i)
                    {
                        profileName = tempProfileName;
                        profileIndex = i;
                    }
                }

                consoleView.print("The profile you selected is " + profileName);

                consoleView.print("Please enter the new values for the Name:");
                saveProfiles[profileIndex].Name = consoleView.read();

                consoleView.print("Please enter the new values for the Source File Path:");
                saveProfiles[profileIndex].SourceFilePath = consoleView.read();

                consoleView.print("Please enter the new values for the Target File Path:");
                saveProfiles[profileIndex].TargetFilePath = consoleView.read();

                consoleView.print("Please enter the new values for the Type Of Save (full or diff)");
                saveProfiles[profileIndex].TypeOfSave = consoleView.read();

                List<long> sourcedirectoryinfo = SaveProfile.sourceDirectoryInfos(saveProfiles[profileIndex].SourceFilePath);
                saveProfiles[profileIndex].TotalFilesToCopy = (int)sourcedirectoryinfo[0];
                saveProfiles[profileIndex].TotalFilesSize = sourcedirectoryinfo[1];
                saveProfiles[profileIndex].NbFilesLeftToDo = (int)sourcedirectoryinfo[0];
                saveProfiles[profileIndex].Progression = 0;

                saveProfiles[profileIndex].State = "READY";

                SaveProfile.SaveProfiles("..\\..\\..\\logs\\state.json", saveProfiles);
                consoleView.printSuccess("The profile has been modified");

            }
            catch (Exception ex)
            {
                consoleView.printError("Error: " + ex.Message);
            }
            
        }
        public void ExecuteSaveProfile()
        {
            consoleView.print("This function is not available yet");
        }
        public void PrintLogs()
        {
            consoleView.print("This function is not available yet");
        }
        public void Help() 
        {
            consoleView.printInfo("menu: Display the menu");
            consoleView.printInfo("printSaveProfiles: Display the save profiles");
            consoleView.printInfo("createSaveProfile: Create a save profile");
            consoleView.printInfo("modifySaveProfile: Modify a save profile");
            consoleView.printInfo("executeSaveProfile: Execute a save profile");
            consoleView.printInfo("printLogs: Display the logs");
            consoleView.printInfo("help: Display the help");
        }

        public void Configuration()
        {
            consoleView.print("This function is not available yet");
        }
        public void Exit() 
        {
            consoleView.print("Thank you for using EasySave");
            Environment.Exit(0);
        }
    }
}
