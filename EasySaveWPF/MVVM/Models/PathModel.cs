using System;
using System.Collections.Generic;
using System.IO;

namespace EasySaveWPF.MVVM.Models
{
    public class PathModel
    {
        public string EasySaveFileDirectoryPath { get; private set; }
        public string EasySaveFileConfigDirectoryPath { get; private set; }
        public string EasySaveFileProfilesDirectoryPath { get; private set; }
        public string EasySaveFileLogsDirectoryPath { get; private set; }
        public string ConfigFilePath { get; private set; }
        public string StateFilePath { get; private set; }

        public PathModel()
        {
            InitializePaths();
            InitializeFolders();
        }

        private void InitializePaths()
        {
            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                                      ?? throw new InvalidOperationException("ApplicationData folder path is null.");

            EasySaveFileDirectoryPath = Path.Combine(appDataFolderPath, "EasySave");
            EasySaveFileConfigDirectoryPath = Path.Combine(EasySaveFileDirectoryPath, "Config");
            EasySaveFileProfilesDirectoryPath = Path.Combine(EasySaveFileDirectoryPath, "Profiles");
            EasySaveFileLogsDirectoryPath = Path.Combine(EasySaveFileDirectoryPath, "Logs");

            ConfigFilePath = Path.Combine(EasySaveFileConfigDirectoryPath, "config.xml");
            StateFilePath = Path.Combine(EasySaveFileProfilesDirectoryPath, "state.json");
        }

        private void InitializeFolders()
        {
            // Create the EasySave directories if they don't exist
            if (!Directory.Exists(EasySaveFileDirectoryPath)) { Directory.CreateDirectory(EasySaveFileDirectoryPath); }
            if (!Directory.Exists(EasySaveFileConfigDirectoryPath)) { Directory.CreateDirectory(EasySaveFileConfigDirectoryPath); }
            if (!Directory.Exists(EasySaveFileProfilesDirectoryPath)) { Directory.CreateDirectory(EasySaveFileProfilesDirectoryPath); }
            if (!Directory.Exists(EasySaveFileLogsDirectoryPath)) { Directory.CreateDirectory(EasySaveFileLogsDirectoryPath); }
        }

        public Dictionary<string, string> LoadPaths()
        {
            Dictionary<string, string> paths = new Dictionary<string, string>();

            paths.Add("EasySaveFileDirectoryPath", EasySaveFileDirectoryPath);
            paths.Add("EasySaveFileConfigDirectoryPath", EasySaveFileConfigDirectoryPath);
            paths.Add("EasySaveFileProfilesDirectoryPath", EasySaveFileProfilesDirectoryPath);
            paths.Add("EasySaveFileLogsDirectoryPath", EasySaveFileLogsDirectoryPath);
            paths.Add("ConfigFilePath", ConfigFilePath);
            paths.Add("StateFilePath", StateFilePath);

            return paths;
        }
    }
}
