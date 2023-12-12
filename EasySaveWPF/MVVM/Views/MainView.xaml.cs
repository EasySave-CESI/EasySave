using EasySaveWPF.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasySaveWPF.MVVM.Views;
using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.ViewModels;
using System;

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PathViewModel _pathViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;
        private readonly SaveProfileViewModel _saveProfileViewModel;

        private ObservableCollection<SaveProfile> profiles;
        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private List<SaveProfile> saveProfiles;

        public MainWindow()
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

            DisplayProfiles();
        }

        private void DisplayProfiles()
        {
            profiles = new ObservableCollection<SaveProfile> { };

            foreach (SaveProfile profile in saveProfiles)
            {
                profiles.Add(profile);
            }

            List_Profil.ItemsSource = profiles;
            NumberProfileLoaded_TextBox.Content = profiles.Count.ToString();
        }


        private void ManageProfile_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileView manageProfileWindow = new ManageProfileView();
            manageProfileWindow.Closing += (s, e) => ManageProfile_Button.IsEnabled = true; //Enable the button when ManageProfileView is closed
            manageProfileWindow.Show();                                                  //Open a new window
            ManageProfile_Button.IsEnabled = false;                                      //Disable the button


        }

        private void ManageProfileWindow_Closing(object? sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExecuteSave_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSaveView executeSaveWindow = new ExecuteSaveView();
            executeSaveWindow.Closing += (s, e) => ExecuteSave_Button.IsEnabled = true;
            executeSaveWindow.Show();
            ExecuteSave_Button.IsEnabled = false;
        }

        private void ViewLogs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Opening logs.");
        }

        private void Option_Button_Click(object sender, RoutedEventArgs e)
        {
            OptionView optionView = new OptionView();
            optionView.Closing += (s, e) => Option_Button.IsEnabled = true;
            optionView.Show();
            Option_Button.IsEnabled = false;
        }


    }
}