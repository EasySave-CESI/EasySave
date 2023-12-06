using Newtonsoft.Json;
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