using EasySaveWPF.MVVM.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace EasySaveWPF.MVVM.Models
{
    public class SaveProfile
    {
        public string Name { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string State { get; set; }
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public int Progression { get; set; }
        public string TypeOfSave { get; set; }
        public bool Encryption { get; set; }
        public string EncryptionKey { get; set; }

        public SaveProfile(string name, string sourceFilePath, string targetFilePath, string state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression, string typeOfSave, bool encryption, string encryptionKey)
        {
            Name = name;
            SourceFilePath = sourceFilePath;
            TargetFilePath = targetFilePath;
            State = state;
            TotalFilesToCopy = totalFilesToCopy;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            Progression = progression;
            TypeOfSave = typeOfSave;
            Encryption = encryption;
            EncryptionKey = encryptionKey;
        }

        public static List<SaveProfile> LoadSaveProfiles(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<SaveProfile> profiles = JsonConvert.DeserializeObject<List<SaveProfile>>(json);
            return profiles;
        }

        public static void CreateSaveProfilesFile(string filePath)
        {
            try
            {
                File.Create(filePath).Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void CreateEmptySaveProfiles(string filePath)
        {
            try
            {
                List<SaveProfile> profiles = new List<SaveProfile>();
                for (int i = 0; i < 5; i++)
                {
                    profiles.Add(new SaveProfile("Save" + (i + 1), "", "", "", 0, 0, 0, 0, "", false, ""));
                }
                SaveProfiles(filePath, profiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string SaveProfiles(string filePath, List<SaveProfile> profiles)
        {
            try
            {
                string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
                File.WriteAllText(filePath, json);
                return "OK";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        public static string AddProfile(string filePath, SaveProfile profile)
        {
            try
            {
                List<SaveProfile> profiles = LoadSaveProfiles(filePath);
                profiles.Add(profile);
                SaveProfiles(filePath, profiles);
                return "OK";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        public static void ExecuteSaveProfile(List<SaveProfile> profiles,DailyLogsViewModel dailyLogsViewModel ,SaveProfile saveProfile, string mode, Dictionary<string, string> paths, Dictionary<string, string> config)
        {
            try
            {
                if (Directory.Exists(saveProfile.TargetFilePath))
                {
                    Directory.Delete(saveProfile.TargetFilePath, true);
                }

                Directory.CreateDirectory(saveProfile.TargetFilePath);

                string[] files = Directory.GetFiles(saveProfile.SourceFilePath, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    DateTime startTime = DateTime.Now;
                    string relativePath = file.Substring(saveProfile.SourceFilePath.Length + 1);
                    string targetFilePath = Path.Combine(saveProfile.TargetFilePath, relativePath);

                    string targetDirectoryPath = Path.GetDirectoryName(targetFilePath);

                    if (!Directory.Exists(targetDirectoryPath))
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                    }

                    if (mode == "diff")
                    {
                        if (!File.Exists(targetFilePath) || File.GetLastWriteTime(file) > File.GetLastWriteTime(targetFilePath))
                        {
                            if (saveProfile.Encryption)
                            {
                                string encryptedText = CallCryptoSoft(File.ReadAllText(file), saveProfile.EncryptionKey);
                                File.WriteAllText(targetFilePath, encryptedText);
                            }
                            else
                            {
                                File.Copy(file, targetFilePath, true);
                            }
                        }
                    }
                    else
                    {
                        if (saveProfile.Encryption)
                        {
                            string encryptedText = CallCryptoSoft(File.ReadAllText(file), saveProfile.EncryptionKey);
                            File.WriteAllText(targetFilePath, encryptedText);
                        }
                        else
                        {
                            File.Copy(file, targetFilePath, true);
                        }
                    }
                    TimeSpan elapsedTime = DateTime.Now - startTime;
                    dailyLogsViewModel.CreateLog(paths["EasySaveFileLogsDirectoryPath"], config["logformat"], saveProfile.Name, file, targetFilePath, file.Length, elapsedTime.TotalSeconds);

                    saveProfile.NbFilesLeftToDo--;
                    saveProfile.Progression = (int)(((double)saveProfile.TotalFilesToCopy - saveProfile.NbFilesLeftToDo) / saveProfile.TotalFilesToCopy * 100);
                    SaveProfiles(paths["StateFilePath"], profiles);
                }
            }
            catch (Exception)
            {
                saveProfile.State = "ERROR";
                SaveProfiles(paths["StateFilePath"], profiles);
            }
        }

        public static List<long> sourceDirectoryInfos(string sourceDirectory)
        {
            List<long> sourcedirectoryInfos = new List<long>();
            sourcedirectoryInfos.Add(Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories).Length);
            sourcedirectoryInfos.Add(Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories).Sum(t => new FileInfo(t).Length));
            return sourcedirectoryInfos;
        }

        static string CallCryptoSoft(string textToEncrypt, string key)
        {
            // this function calls the CryptoSoft.exe program and returns the encrypted text
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "D:\\école\\CESI\\A3\\EasySave\\CryptoSoft\\CryptoSoft\\bin\\Release\\net8.0\\publish\\CryptoSoft.exe"; // A changer
            start.Arguments = textToEncrypt + " " + key;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}