using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.Views;
using System.IO;

namespace EasySaveWPF.MVVM.ViewModels
{
    public class SaveProfileViewModel
    {
        public SaveProfileViewModel() { }

        public List<SaveProfile> LoadSaveProfiles(string directoryPath)
        {
            CheckSaveProfilesFile(directoryPath);
            return SaveProfile.LoadSaveProfiles(directoryPath);
        }

        public void CheckSaveProfilesFile(string directoryPath)
        {
            if (!File.Exists(directoryPath)) // If the save profiles file doesn't exist, create a new one
            {
                Console.WriteLine("The save profiles file doesn't exist. Creating a new one...");
                SaveProfile.CreateSaveProfilesFile(directoryPath);
                SaveProfile.CreateEmptySaveProfiles(directoryPath);
            }
            else if (new FileInfo(directoryPath).Length == 0) // If the save profiles file is empty, create a new one
            {
                Console.WriteLine("The save profiles file is empty. Creation of the empty profiles...");
                SaveProfile.CreateEmptySaveProfiles(directoryPath);
            }
        }
        public void DisplaySaveProfiles(ConsoleView consoleView, List<SaveProfile> saveProfiles)
        {
            if (saveProfiles == null)
            {
                consoleView.DisplaySaveProfilesError();
            }
            else
            {
                foreach (SaveProfile saveProfile in saveProfiles)
                {
                    List<string> saveProfileInfos = new List<string>();
                    saveProfileInfos.Add(saveProfile.Name);
                    saveProfileInfos.Add(saveProfile.SourceFilePath);
                    saveProfileInfos.Add(saveProfile.TargetFilePath);
                    saveProfileInfos.Add(saveProfile.TypeOfSave);
                    saveProfileInfos.Add(saveProfile.State);
                    saveProfileInfos.Add(saveProfile.TotalFilesToCopy.ToString());
                    saveProfileInfos.Add(saveProfile.TotalFilesSize.ToString());
                    saveProfileInfos.Add(saveProfile.NbFilesLeftToDo.ToString());
                    saveProfileInfos.Add(saveProfile.Progression.ToString());
                    consoleView.DisplaySaveProfiles(saveProfileInfos);
                }
            }

        }

        public void ModifySaveProfile(ConsoleView consoleView, List<SaveProfile> saveProfiles, Dictionary<string, string> paths, int id)
        {

            try
            {
                string profileName = string.Empty;
                int profileIndex = -1;

                profileIndex = id;

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

                SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
                consoleView.DisplayModifySaveProfileSuccess();

            }
            catch (Exception ex)
            {
                consoleView.Error(ex.Message);
            }

        }

        public void ExecuteSaveProfile(DailyLogsViewModel dailyLogsViewModel, List<SaveProfile> saveProfiles, Dictionary<string, string> paths, Dictionary<string, string> config, int index)
        {
            try
            {
                int profileIndex = index;

                if (saveProfiles[profileIndex].State == "READY")
                {
                    saveProfiles[profileIndex].State = "IN PROGRESS";
                    SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);

                    if (saveProfiles[profileIndex].TypeOfSave == "full" || saveProfiles[profileIndex].TypeOfSave == "diff")
                    {

                        SaveProfile.ExecuteSaveProfile(saveProfiles, dailyLogsViewModel, saveProfiles[profileIndex], saveProfiles[profileIndex].TypeOfSave, paths, config);
                    }
                    else
                    {
                        // If the type of save is not full or diff, display an error message
                    }

                    saveProfiles[profileIndex].State = "COMPLETED";
                    SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
                }
                else
                {
                    // If the state is not ready, display an error message
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, display an error message
            }
        }
    }
}
