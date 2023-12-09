using Newtonsoft.Json;

namespace EasySave.Models
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
            string json = File.ReadAllText(filePath);
            List<SaveProfile> profiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaveProfile>>(json);
            return profiles;
        }

        public static void CreateProfilesFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.Create(filePath).Close();

                List<SaveProfile> profiles = new List<SaveProfile>();
                for (int i = 0; i < 5; i++)
                {
                    profiles.Add(new SaveProfile("Save" + (i + 1), "", "", "", 0, 0, 0, 0, ""));
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
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(profiles, Newtonsoft.Json.Formatting.Indented);
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

        public static void ExecuteSaveProfile(List<SaveProfile> profiles, SaveProfile saveProfile, string mode)
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

                    saveProfile.NbFilesLeftToDo--;
                    saveProfile.Progression = (int)(((double)saveProfile.TotalFilesToCopy - saveProfile.NbFilesLeftToDo) / saveProfile.TotalFilesToCopy * 100);
                    SaveProfiles("..\\..\\..\\Profiles\\state.json", profiles);
                }
            }
            catch (Exception ex)
            {
                saveProfile.State = "ERROR";
                SaveProfiles("state.json", profiles);
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