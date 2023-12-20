using System.Collections.Generic;
using EasySave.MVVM.Models;

namespace EasySave.MVVM.ViewModels
{
    public class PathViewModel
    {
        private readonly PathModel _pathModel;

        public string EasySaveFileDirectoryPath => _pathModel.EasySaveFileDirectoryPath;
        public string EasySaveFileConfigDirectoryPath => _pathModel.EasySaveFileConfigDirectoryPath;
        public string EasySaveFileProfilesDirectoryPath => _pathModel.EasySaveFileProfilesDirectoryPath;
        public string EasySaveFileLogsDirectoryPath => _pathModel.EasySaveFileLogsDirectoryPath;
        public string ConfigFilePath => _pathModel.ConfigFilePath;
        public string StateFilePath => _pathModel.StateFilePath;

        public PathViewModel()
        {
            _pathModel = new PathModel();
        }

        public Dictionary<string, string> LoadPaths()
        {
            return _pathModel.LoadPaths();
        }
    }
}
