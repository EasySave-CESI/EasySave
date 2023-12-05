using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace EasySaveConsoleApp
{
    public class MainViewModel
    {
        private readonly List<Profile> _profiles;

        public MainViewModel(List<Profile> profiles)
        {
            _profiles = profiles;
        }

        public void DisplayProfiles()
        {
            Console.WriteLine("\nList of backup profiles:");

            Console.WriteLine("Name - State - Source path - Target path");

            foreach (var profile in _profiles)
            {
                Console.WriteLine((int.Parse(profile.Name.Substring(profile.Name.Length - 1))) + ". " + profile.Name + " - " + profile.State + " - " + profile.SourceFilePath + " - " + profile.TargetFilePath + " - " + profile.TypeOfSave);
            }
        }

        public void ModifyProfile()
        {
            Console.WriteLine("\nModifying a backup profile:");

            Console.Write("Enter the profile number to modify (1-5): ");
            int profileNumber;
            if (int.TryParse(Console.ReadLine(), out profileNumber) && profileNumber >= 1 && profileNumber <= 5)
            {
                int index = profileNumber -1;

                Console.Write("New source path: ");

                _profiles[index].SourceFilePath = Console.ReadLine();
                Console.WriteLine("New source path: " + _profiles[index].SourceFilePath);
     

                Console.Write("New target path: ");
                _profiles[index].TargetFilePath = Console.ReadLine();
                Console.WriteLine("New target path: " + _profiles[index].TargetFilePath);

                Console.Write("New type of save (full of diff): ");
                if (Console.ReadLine() == "full")
                {
                    _profiles[index].TypeOfSave = "full";
                }
                else
                {
                    _profiles[index].TypeOfSave = "diff";
                }

                string[] files = Directory.GetFiles(_profiles[index].SourceFilePath, "*", SearchOption.AllDirectories);

                _profiles[index].TotalFilesToCopy = files.Length;

                foreach (string file in files)
                {
                    /* Add the size of the file to the total size of the profile in long */
                    _profiles[index].TotalFilesSize += new FileInfo(file).Length;
                    Console.WriteLine("Total size of the profile: " + _profiles[index].TotalFilesSize);
                }

                _profiles[index].State = "READY";
                /* Then save the profiles to the state file */
                Profile.SaveProfiles("..\\..\\..\\logs\\state.json", _profiles);
            }
            else
            {
                Console.WriteLine("Invalid profile number. Please enter a number between 1 and 5.");
            }
        }

        public void ExecuteProfile()
        {
            List<int> index = new List<int>();

            string pattern_1 = @"^[0-9]+$";
            string pattern_2 = @"^[0-9]+[;][0-9]+$";
            string pattern_3 = @"^[0-9]+[-][0-9]+$";

            Console.WriteLine("Choose the profile(s) to execute: ");

            string answer = Console.ReadLine();

            if (Regex.IsMatch(answer, pattern_1))
            {
                index.Add(int.Parse(answer) - 1);
            }

            if (Regex.IsMatch(answer, pattern_2))
            {
                string[] split = answer.Split(';');

                index.Add(int.Parse(split[0]) - 1);
                index.Add(int.Parse(split[1]) - 1);
            }

            if (Regex.IsMatch(answer, pattern_3))
            {
                int start = int.Parse(answer.Split('-')[0]);
                int end = int.Parse(answer.Split('-')[1]);

                for (int i = start; i <= end; i++)
                {
                    index.Add(i - 1);
                }
            }

            for (int i = 0; i < index.Count; i++)
            {
                Console.WriteLine("Backing up profile " + _profiles[index[i]].Name + " in progress...");

                try
                {
                    if (Directory.Exists(_profiles[index[i]].TargetFilePath))
                    {
                        Directory.Delete(_profiles[index[i]].TargetFilePath, true);
                    }

                    Directory.CreateDirectory(_profiles[index[i]].TargetFilePath);

                    string[] files = Directory.GetFiles(_profiles[index[i]].SourceFilePath, "*", SearchOption.AllDirectories);

                    _profiles[index[i]].NbFilesLeftToDo = _profiles[index[i]].TotalFilesToCopy;

                    foreach (string file in files)
                    {
                        string relativePath = file.Substring(_profiles[index[i]].SourceFilePath.Length + 1);

                        string targetFilePath = Path.Combine(_profiles[index[i]].TargetFilePath, relativePath);

                        Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                        File.Copy(file, targetFilePath, true);

                        _profiles[index[i]].NbFilesLeftToDo--;
                        _profiles[index[i]].Progression = (int)((_profiles[index[i]].TotalFilesToCopy - _profiles[index[i]].NbFilesLeftToDo) * 100 / _profiles[index[i]].TotalFilesToCopy);
                        _profiles[index[i]].State = "RUNNING";

                        Profile.SaveProfiles("..\\..\\..\\logs\\state.json", _profiles);
                    }

                    _profiles[index[i]].State = "COMPLETED";
                    Profile.SaveProfiles("..\\..\\..\\logs\\state.json", _profiles);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error backing up profile {_profiles[index[i]].Name}: {ex.Message}");
                    _profiles[index[i]].State = "FAILED";
                    Profile.SaveProfiles("..\\..\\..\\logs\\state.json", _profiles);
                }
            }
        }
    }
}
