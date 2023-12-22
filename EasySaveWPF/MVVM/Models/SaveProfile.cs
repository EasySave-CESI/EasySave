using EasySaveWPF.MVVM.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.ComponentModel;

namespace EasySaveWPF.MVVM.Models
{
    public class SaveProfile
    {
        public string Name { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string TypeOfSave { get; set; }
        public string State { get; set; }
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public int Progression { get; set; }
        private static readonly object StateFileLock = new object();


        public static Dictionary<string, ManualResetEvent> PauseResumeEvents = new Dictionary<string, ManualResetEvent>();

        public static void PauseSaveProfile(string saveProfileName)
        {
            lock (StateFileLock)
            {
                if (PauseResumeEvents.TryGetValue(saveProfileName, out var pauseResumeEvent))
                {
                    pauseResumeEvent.Reset();
                }
            }
        }

        public static void ResumeSaveProfile(string saveProfileName)
        {
            lock (StateFileLock)
            {
                if (PauseResumeEvents.TryGetValue(saveProfileName, out var pauseResumeEvent))
                {
                    pauseResumeEvent.Set();
                }
            }
        }

        public SaveProfile(string name, string sourceFilePath, string targetFilePath, string state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression, string typeOfSave)
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
        }

        public SaveProfile()
        {
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

        public static SaveProfile CreateSaveProfile(string Name, string SourceFilePath, string TargetFilePath, string TypeOfSave)
        {
            try
            {
                // First we calculate the number of files to copy
                int totalFilesToCopy = Directory.GetFiles(SourceFilePath, "*", SearchOption.AllDirectories).Length;

                // Then we calculate the total size of the files to copy
                long totalFilesSize = Directory.GetFiles(SourceFilePath, "*", SearchOption.AllDirectories).Sum(t => new FileInfo(t).Length);

                // We create the save profile
                SaveProfile saveProfile = new SaveProfile(Name, SourceFilePath, TargetFilePath, "READY", totalFilesToCopy, totalFilesSize, totalFilesToCopy, 0, TypeOfSave);

                return saveProfile;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string SaveProfiles(string filePath, List<SaveProfile> profiles)
        {
            lock (StateFileLock)
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

        public static void ExecuteSaveProfile(List<SaveProfile> profiles, DailyLogsViewModel dailyLogsViewModel, SaveProfile saveProfile, string mode, Dictionary<string, string> paths, Dictionary<string, string> config)
        {
            ManualResetEvent pauseResumeEvent = new ManualResetEvent(true);
            PauseResumeEvents[saveProfile.Name] = pauseResumeEvent;

            Thread thread = new Thread(() =>
            {
                lock (StateFileLock)
                {
                    try
                    {
                        saveProfile.State = "IN PROGRESS";
                        SaveProfiles(paths["StateFilePath"], profiles);
                        if (Directory.Exists(saveProfile.TargetFilePath))
                        {
                            Directory.Delete(saveProfile.TargetFilePath, true);
                        }
                        Directory.CreateDirectory(saveProfile.TargetFilePath);
                        string[] files = Directory.GetFiles(saveProfile.SourceFilePath, "*", SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            pauseResumeEvent.WaitOne();
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
                                    File.Copy(file, targetFilePath, true);
                                }
                            }
                            else
                            {
                                File.Copy(file, targetFilePath, true);
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
                    finally
                    {
                        saveProfile.State = "READY";
                        SaveProfiles(paths["StateFilePath"], profiles);
                        MessageBox.Show($"{saveProfile.Name} has just finished");
                    }
                }
            });
            thread.Start();
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
            start.Arguments = textToEncrypt;
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