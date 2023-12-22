using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.Views;
using System.IO;
using System.Windows;
using System.Threading;

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

                    saveProfiles[profileIndex].State = "READY";
                    SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void PauseSaveProfile(SaveProfile saveProfile)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SaveProfile.PauseSaveProfile(saveProfile.Name);
                MessageBox.Show($"Pausing {saveProfile.Name}");
            });
        }

        public void ResumeSaveProfile(SaveProfile saveProfile)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SaveProfile.ResumeSaveProfile(saveProfile.Name);
                MessageBox.Show($"Resuming {saveProfile.Name}");
            });
        }


        public void DeleteSaveProfile(List<SaveProfile> saveProfiles, SaveProfile saveProfile, Dictionary<string, string> paths)
        {
            try
            {
                saveProfiles.Remove(saveProfile);
                SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
