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
using System.Diagnostics;

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PathViewModel _pathViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;
        private readonly LanguageConfigurationViewModel _languageConfigurationViewModel;
        private readonly SaveProfileViewModel _saveProfileViewModel;

        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private Dictionary<string, string> printStringDictionary;
        private List<SaveProfile> saveProfiles;
        private List<SaveProfile> profilesToExecute = new List<SaveProfile>();
        private ObservableCollection<SaveProfile> profiles = new ObservableCollection<SaveProfile>();

        private bool isSelectionDotPressed = false;

        public MainWindow()
        {
            InitializeComponent();

            _pathViewModel = new PathViewModel();
            _configurationViewModel = new ConfigurationViewModel();
            _languageConfigurationViewModel = new LanguageConfigurationViewModel();
            _saveProfileViewModel = new SaveProfileViewModel();

            // Create a new dictionary to store the paths
            paths = _pathViewModel.LoadPaths();

            // Create a new dictionary to store the config
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            // Create a new language configuration
            Dictionary<string, string> printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);

            // Create a new list to store the save profiles
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);

            // Set the language
            Setlanguage();

            // Display the profiles
            DisplayProfiles();
        }

        private void DisplayProfiles() 
        {
            profiles.Clear();
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);
            foreach (SaveProfile profile in saveProfiles)
            {
                profiles.Add(profile);
            }
            MainWindow_Content_ExistingSaves_Grid.ItemsSource = profiles;
        }

        private void SelectionDot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse selectionDot = (Ellipse)sender;

            if (isSelectionDotPressed)
            {
                selectionDot.Fill = Brushes.White;
                profilesToExecute.Remove((SaveProfile)selectionDot.DataContext);
            }
            else
            {
                selectionDot.Fill = Brushes.Black;
                profilesToExecute.Add((SaveProfile)selectionDot.DataContext);
            }

            isSelectionDotPressed = !isSelectionDotPressed;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Button startButton = (Button)sender;
            SaveProfile profileToStart = (SaveProfile)startButton.DataContext;
            MessageBox.Show($"Starting {profileToStart.Name}");
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Button stopButton = (Button)sender;
            SaveProfile profileToStop = (SaveProfile)stopButton.DataContext;
            MessageBox.Show($"Stopping {profileToStop.Name}");
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            Button modifyButton = (Button)sender;
            SaveProfile profileToModify = (SaveProfile)modifyButton.DataContext;
            MessageBox.Show($"Modifying {profileToModify.Name}");
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            SaveProfile profileToDelete = (SaveProfile)deleteButton.DataContext;
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {profileToDelete.Name} ?", "Delete profile", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) 
            {
                _saveProfileViewModel.DeleteSaveProfile(saveProfiles, profileToDelete, paths);
                DisplayProfiles();
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Creating a new save profile");
        }

        private void ExecuteAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (SaveProfile profile in profilesToExecute)
            {
                MessageBox.Show($"Executing {profile.Name}");
            }
        }

        private void Setlanguage()
        {
            printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);

            // Title
            MainWindow_NavigationBar_EasySaveName_Label.Content = printStringDictionary["Application_MainWindow_Title"];

            // Navigation bar
            MainWindow_NavigationBar_PagesList_Home.Content = printStringDictionary["Application_MainWindow_NavigationBar_PagesList_Home"];
            MainWindow_NavigationBar_PagesList_Logs.Content = printStringDictionary["Application_MainWindow_NavigationBar_PagesList_Logs"];
            MainWindow_NavigationBar_PagesList_Settings.Content = printStringDictionary["Application_MainWindow_NavigationBar_PagesList_Settings"];

            // Home page

            // Existing saves
            MainWindow_MainContentHeader_ExistingSaves_Label.Content = printStringDictionary["Application_MainWindow_MainContentHeader_ExistingSaves_Label"];

            // Execute all button
            MainWindow_MainContentHeader_ExecuteAll_Button.Content = printStringDictionary["Application_MainWindow_MainContentHeader_ExecuteAll_Button"];

            // Create button
            MainWindow_MainContentHeader_CreateSave_Button.Content = printStringDictionary["Application_MainWindow_MainContentHeader_CreateSave_Button"];
        }
    }
}
