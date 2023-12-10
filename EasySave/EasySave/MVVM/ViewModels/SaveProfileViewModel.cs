using EasySave.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace EasySave.MVVM.ViewModels
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
    }
}
