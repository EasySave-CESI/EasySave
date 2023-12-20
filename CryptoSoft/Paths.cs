using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSoft
{
    internal class Paths
    {
        public string CryptoSoftFolderPath;
        public string CryptoSoftCryptedFilesFolderPath;
        public string CryptoSoftConfigFolderPath;
        public string CryptoSoftConfigFilePath;

        public Paths()
        {
            CryptoSoftFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CryptoSoft");
            CryptoSoftCryptedFilesFolderPath = Path.Combine(CryptoSoftFolderPath, "CryptedFiles");
            CryptoSoftConfigFolderPath = Path.Combine(CryptoSoftFolderPath, "Config");
            CryptoSoftConfigFilePath = Path.Combine(CryptoSoftConfigFolderPath, "config.xml");
        }

        public string GetFileContent(string filepath) { return File.ReadAllText(filepath);}

        public void SaveFile(string filepath, string content) {File.WriteAllText(filepath, content);}
        public bool CheckFolfer(string path) {if (Directory.Exists(path)) {return true;} return false;}

        public void CreateFolder(string path) {Directory.CreateDirectory(path);}

        public bool CheckFile(string filepath) {if (File.Exists(filepath)) {return true;} return false;}

        public void CreateFile(string filepath) {File.Create(filepath);}
    }
}
