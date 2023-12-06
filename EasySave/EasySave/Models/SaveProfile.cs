using Newtonsoft.Json;

namespace EasySaveConsoleApp
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

        public static List<SaveProfile> LoadProfiles(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                List<SaveProfile> profiles = JsonConvert.DeserializeObject<List<SaveProfile>>(json);
                return profiles;
            }
            catch (Exception ex)
            {
                return null;
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
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        public static string AddProfile(string filePath, SaveProfile profile)
        {
            try
            {
                List<SaveProfile> profiles = LoadProfiles(filePath);
                profiles.Add(profile);
                SaveProfiles(filePath, profiles);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        public static List<long> sourceDirectoryInfos(string sourceDirectory)
        {
            List<long> sourcedirectoryInfos = new List<long>();
            sourcedirectoryInfos.Add(Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories).Length);
            sourcedirectoryInfos.Add(Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length)));
            return sourcedirectoryInfos;
        }
    }
}