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

        public void ModifySaveProfile(ConsoleView consoleView, List<SaveProfile> saveProfiles, Dictionary<string, string> paths)
        {

            try
            {
                string profileName = string.Empty;
                int profileIndex = -1;

                profileIndex = GetProfileIndex(consoleView, saveProfiles);

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

        public void ExecuteSaveProfile(ConsoleView consoleView, DailyLogsViewModel dailyLogsViewModel, List<SaveProfile> saveProfiles, Dictionary<string, string> paths, Dictionary<string, string> config)
        {
            try
            {
                int profileIndex = GetProfileIndex(consoleView, saveProfiles);
                consoleView.DisplaySelectedProfileName(saveProfiles[profileIndex].Name);

                if (saveProfiles[profileIndex].State == "READY")
                {
                    saveProfiles[profileIndex].State = "IN PROGRESS";
                    SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);

                    if (saveProfiles[profileIndex].TypeOfSave == "full" || saveProfiles[profileIndex].TypeOfSave == "diff")
                    {
                        consoleView.DisplayBackupInProgress(saveProfiles[profileIndex].Name);

                        SaveProfile.ExecuteSaveProfile(saveProfiles, dailyLogsViewModel, saveProfiles[profileIndex], saveProfiles[profileIndex].TypeOfSave, paths, config);
                    }
                    else
                    {
                        consoleView.DisplayExecuteSaveProfileTypeOfSaveError();
                    }

                    saveProfiles[profileIndex].State = "COMPLETED";
                    SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
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

        public int GetProfileIndex(ConsoleView consoleView, List<SaveProfile> saveProfiles)
        {
            consoleView.DisplayChooseSelectedProfile();

            // print all the profiles name
            for (int i = 0; i < saveProfiles.Count; i++)
            {
                consoleView.DisplayProfileIndexName(i + 1, saveProfiles[i].Name);
            }

            string choice = consoleView.Read();

            int profileIndex = -1;

            for (int i = 0; i < saveProfiles.Count; i++)
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
