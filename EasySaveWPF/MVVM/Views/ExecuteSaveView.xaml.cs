using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.ViewModels;

namespace EasySaveWPF.Views
{
    /// <summary>
    /// Logique d'interaction pour ExecuteSaveView.xaml
    /// </summary>
    public partial class ExecuteSaveView : Window
    {
        private readonly PathViewModel _pathViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;
        private readonly SaveProfileViewModel _saveProfileViewModel;
        private readonly DailyLogsViewModel _dailyLogsViewModel;

        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private List<SaveProfile> saveProfiles;
        private List<int> listIdsSavesToExecute = new List<int>();
        private string RegexPattern = @"^(\d+)(?:-(\d+))?(?:;(\d+))?(?:;(\d+))?(?:;(\d+))?(?:;(\d+))?$";
        private string Regexpatternto = @"^(\d+)(?:-(\d+))?$";

        public ExecuteSaveView()
        {
            InitializeComponent();

            _pathViewModel = new PathViewModel();
            _configurationViewModel = new ConfigurationViewModel();
            _saveProfileViewModel = new SaveProfileViewModel();

            // Create a new dictionary to store the paths
            paths = _pathViewModel.LoadPaths();

            // Create a new dictionary to store the config
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            // Create a new list to store the save profiles
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);

            // Create a new list to store the daily logs
            _dailyLogsViewModel = new DailyLogsViewModel(paths["EasySaveFileLogsDirectoryPath"], config["logformat"]);
        }

        private void Start_ExecuteSaveView_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInput())
            {
                foreach (int id in listIdsSavesToExecute)
                {
                    MessageBox.Show("The save profile " + saveProfiles[id - 1].Name + " will be executed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    _saveProfileViewModel.ExecuteSaveProfile(_dailyLogsViewModel, saveProfiles, paths, config, id-1);
                }
                Close();
            }
        }

        private void Cancel_ExecuteSaveView_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool CheckInput()
        {
            // Empty the list
            listIdsSavesToExecute.Clear();

            // Check if the input is empty
            if (UserSelectionTextBox.Text == "")
            {
                MessageBox.Show("Please enter a name for the save profile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
            // Check if the regex is invalid
            if (!Regex.IsMatch(UserSelectionTextBox.Text, RegexPattern))
            {
                MessageBox.Show("Please enter a valid pattern", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // If the regex is valid then we check the numbers of groups
            Match match = Regex.Match(UserSelectionTextBox.Text, RegexPattern, RegexOptions.IgnoreCase);
            Match matchto = Regex.Match(UserSelectionTextBox.Text, Regexpatternto, RegexOptions.IgnoreCase);

            // Create a list of string to store the groups
            List<string> groups = new List<string>();
            foreach (Group group in match.Groups)
            {
                groups.Add(group.Value);
            }

            // Remove the first element of the list because it's the whole string
            groups.RemoveAt(0);

            // Remove the empty groups
            groups.RemoveAll(item => item == "");

            // Check if one of the group is lower than 1
            foreach (string group in groups)
            {
                if (int.Parse(group) < 1)
                {
                    MessageBox.Show("The save profile " + group + " doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            // First check if the user only want to execute one save profile
            if (groups.Count == 1)
            {
                if (int.Parse(groups[0]) > saveProfiles.Count)
                {
                    MessageBox.Show("The save profile " + groups[0] + " doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    listIdsSavesToExecute.Add(int.Parse(groups[0]));
                }
            }

            // Then check if the user want to execute a range of save profiles
            if (Regex.IsMatch(UserSelectionTextBox.Text, Regexpatternto) && listIdsSavesToExecute.Count == 0)
            {
                // First get the highest number
                int lowestNumber = int.Parse(groups[0]);
                int highestNumber = int.Parse(groups[1]);

                if (highestNumber > saveProfiles.Count || lowestNumber > saveProfiles.Count)
                {
                    MessageBox.Show("The save profile " + highestNumber + " or " + lowestNumber + " doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (highestNumber < lowestNumber)
                {
                    MessageBox.Show("The first number must be lower than the second number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                for (int i = lowestNumber; i <= highestNumber; i++)
                {
                    listIdsSavesToExecute.Add(i);
                }
            }

            // Then check if the user want to execute a list of save profiles
            if (groups.Count > 1 && listIdsSavesToExecute.Count == 0)
            {
                foreach (string group in groups)
                {
                    if (int.Parse(group) > saveProfiles.Count)
                    {
                        MessageBox.Show("The save profile " + group + " doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        listIdsSavesToExecute.Add(int.Parse(group));
                    }
                }
            }

            return true;
        }

        private void ListOfProfilesToSave(object sender, RoutedEventArgs e)
        {
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);

            // Get a 
        }
    }
}
