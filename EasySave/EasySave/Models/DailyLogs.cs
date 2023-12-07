using Newtonsoft.Json;
using static EasySaveConsoleApp.DailyLogs;
using System.IO;
using System.Xml.Serialization;
using EasySaveConsoleApp;
/*
namespace EasySaveConsoleApp
{
    public class DailyLogs
    {
        public string Name { get; set; }  // Nom de la sauvegarde
        public string FileSource { get; set; }  // Adresse complète du fichier source
        public string FileTarget { get; set; }  // Adresse complète du fichier de destination
        public long FileSize { get; set; }  // Taille du fichier
        public double FileTransferTime { get; set; }  // Temps de transfert du fichier en ms
        public string Time { get; set; }  // Horodatage de l'entrée de log

        // Constructeur de la classe DailyLogs
        public DailyLogs(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime)
        {
            Name = name;
            FileSource = fileSource;
            FileTarget = fileTarget;
            FileSize = fileSize;
            FileTransferTime = fileTransferTime;
            Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");  // Horodatage actuel
        }


        // Méthode pour exporter les logs au format JSON dans un fichier
        public void ExportToJson()
        {
            string filePath = "..\\..\\..\\logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";  // Chemin du fichier de logs
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
}
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace EasySaveConsoleApp
{
    public class DailyLog
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public double FileTransferTime { get; set; }
        public string Time { get; set; }
        
    }

    public enum LogFileFormat
    {
        Json,
        Xml
    }

    public class DailyLogs
    {
        private List<DailyLog> logs;
        private LogFileFormat logFileFormat = LogFileFormat.Json;
        private string logsDirectory = "..\\..\\..\\logs";

        public DailyLogs()
        {
            logs = LoadLogs();
        }

        public void SetLogFileFormat(LogFileFormat format)
        {
            logFileFormat = format;
        }

        public void CreateLog(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime)
        {
            var log = new DailyLog
            {
                Name = name,
                FileSource = fileSource,
                FileTarget = fileTarget,
                FileSize = fileSize,
                FileTransferTime = fileTransferTime,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };

            logs.Add(log);
            SaveLogs();
        }

        public void SaveLogs()
        {
            try
            {
                string filePath = GetLogsFilePath();

                if (logFileFormat == LogFileFormat.Json)
                {
                    string jsonContent = JsonConvert.SerializeObject(logs, Formatting.Indented);
                    File.WriteAllText(filePath, jsonContent);
                }
                else if (logFileFormat == LogFileFormat.Xml)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<DailyLog>));
                    using (StringWriter writer = new StringWriter())
                    {
                        serializer.Serialize(writer, logs);
                        File.WriteAllText(filePath, writer.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving logs: {ex.Message}");
            }
        }

        private string GetLogsFilePath()
        {
            string extension = (logFileFormat == LogFileFormat.Json) ? "json" : "xml";
            return Path.Combine(logsDirectory, DateTime.Now.ToString("yyyy-MM-dd") + "." + extension);
        }

        private void SaveLogsToJson()
        {
            string jsonLogs = Newtonsoft.Json.JsonConvert.SerializeObject(logs, Newtonsoft.Json.Formatting.Indented);

            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            string filePath = Path.Combine(logsDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".json");
            File.WriteAllText(filePath, jsonLogs);  
        }

        private void SaveLogsToXml()
        {
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            string filePath = Path.Combine(logsDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<DailyLog>));

            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, logs);
            }
        }

        private List<DailyLog> LoadLogs()
        {
            try
            {
                string filePath = Path.Combine(logsDirectory, DateTime.Now.ToString("yyyy-MM-dd") + "." + GetFileExtension());

                if (File.Exists(filePath))
                {
                    if (logFileFormat == LogFileFormat.Json)
                    {
                        string jsonContent = File.ReadAllText(filePath);
                        return JsonConvert.DeserializeObject<List<DailyLog>>(jsonContent);
                    }
                    else if (logFileFormat == LogFileFormat.Xml)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<DailyLog>));

                        using (TextReader reader = new StreamReader(filePath))
                        {
                            return (List<DailyLog>)serializer.Deserialize(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading logs: " + ex.Message);
            }

            return new List<DailyLog>();
        }

        private string GetFileExtension()
        {
            switch (logFileFormat)
            {
                case LogFileFormat.Json:
                    return "json";
                case LogFileFormat.Xml:
                    return "xml";
                default:
                    throw new NotSupportedException("Unsupported log file format.");
            }
        }

        public List<DailyLog> GetLogs()
        {
            return logs;
        }
    }
}

