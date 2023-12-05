using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace EasySaveConsoleApp
{
    public class Profile
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

        public Profile(string name, string sourceFilePath, string targetFilePath, string state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression, string typeOfSave)
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

        public override string ToString()
        {
            return $"Name: {Name}, SourceFilePath: {SourceFilePath}, TargetFilePath: {TargetFilePath}, State: {State}";
        }

        public static List<Profile> LoadProfiles(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                List<Profile> profiles = JsonConvert.DeserializeObject<List<Profile>>(json);
                return profiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profiles: {ex.Message}");
                return new List<Profile>();
            }
        }

        public static void SaveProfiles(string filePath, List<Profile> profiles)
        {
            try
            {
                string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving profiles: {ex.Message}");
            }
        }

        public void Execute()
        {
            Console.WriteLine($"Backing up profile {Name} in progress...");

            try
            {
                if (Directory.Exists(TargetFilePath))
                {
                    Directory.Delete(TargetFilePath, true);
                }

                Directory.CreateDirectory(TargetFilePath);

                string[] files = Directory.GetFiles(SourceFilePath, "*", SearchOption.AllDirectories);


                NbFilesLeftToDo = TotalFilesToCopy;

                foreach (string file in files)
                {
                    string relativePath = file.Substring(SourceFilePath.Length + 1);
                    string targetFilePath = Path.Combine(TargetFilePath, relativePath);

                    string targetDirectoryPath = Path.GetDirectoryName(targetFilePath);

                    if (!Directory.Exists(targetDirectoryPath))
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                    }

                    if (TypeOfSave == "differential")
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

                    LogEntry logEntry = new LogEntry();
                    logEntry.Name = Name;
                    logEntry.FileSource = file;
                    logEntry.FileTarget = targetFilePath;
                    logEntry.FileSize = new FileInfo(file).Length;
                    logEntry.FileTransferTime = 0;
                    logEntry.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    DailyLogs dailyLogs = new DailyLogs();
                    dailyLogs.AddLogEntry(logEntry.Name, logEntry.FileSource, logEntry.FileTarget, logEntry.FileSize, logEntry.FileTransferTime);
                    dailyLogs.ExportToJson();

                    NbFilesLeftToDo--;
                    Progression = (int)(((double)TotalFilesToCopy - NbFilesLeftToDo) / TotalFilesToCopy * 100);
                    Console.WriteLine($"Progression of profile {Name}: {Progression}%");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during backup of profile {Name}: {ex.Message}");
            }

            State = "DONE";
            Console.WriteLine($"Backing up profile {Name} done.");
        }
    }

    // Classe principale pour la gestion des journaux quotidiens
    public class DailyLogs
    {
        private List<LogEntry> logEntries;  // Liste pour stocker les entrées de logs

        // Constructeur de la classe DailyLogs
        public DailyLogs()
        {
            logEntries = new List<LogEntry>();  // Initialisation de la liste
        }

        // Méthode pour ajouter une entrée de log
        public void AddLogEntry(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime)
        {
            LogEntry logEntry = new LogEntry
            {
                Name = name,
                FileSource = fileSource,
                FileTarget = fileTarget,
                FileSize = fileSize,
                FileTransferTime = fileTransferTime,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")  // Horodatage actuel
            };

            logEntries.Add(logEntry);  // Ajout de l'entrée à la liste
        }

        // Méthode pour exporter les logs au format JSON dans un fichier
        public void ExportToJson()
        {
            string filePath = "logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";  // Chemin du fichier de logs
            string jsonContent = JsonConvert.SerializeObject(logEntries, Formatting.Indented);  // Sérialisation de la liste en format JSON
            if (File.Exists(filePath))  // Si le fichier existe déjà ajoutes les logs à la fin
            {
                File.AppendAllText(filePath, jsonContent);
            }
            else  // Sinon crée le fichier et ajoutes les logs
            {
                File.WriteAllText(filePath, jsonContent);
            }

        }
    }

    // Classe représentant une entrée de log
    public class LogEntry
    {
        public string Name { get; set; }  // Nom de la sauvegarde
        public string FileSource { get; set; }  // Adresse complète du fichier source
        public string FileTarget { get; set; }  // Adresse complète du fichier de destination
        public long FileSize { get; set; }  // Taille du fichier
        public double FileTransferTime { get; set; }  // Temps de transfert du fichier en ms
        public string Time { get; set; }  // Horodatage de l'entrée de log
    }
}
