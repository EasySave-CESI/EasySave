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
        private string ActualPage;

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

            // Set the page
            HandlePageSelection("Home");
        }

        private void MainWindow_NavigationBar_PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem selectedListBoxItem = (ListBoxItem)listBox.SelectedItem;

            if (selectedListBoxItem != null)
            {
                string selectedPage = selectedListBoxItem.Content.ToString();
                if (selectedPage == "Accueil")
                {
                    selectedPage = "Home";
                }
                else if (selectedPage == "Journaux")
                {
                    selectedPage = "Logs";
                }
                else if (selectedPage == "Paramètres")
                {
                    selectedPage = "Settings";
                }
                HandlePageSelection(selectedPage);
            }
        }

        private void HandlePageSelection(string selectedPage)
        {
            switch (selectedPage)
            {
                case "Home":
                    if (ActualPage != "Home")
                    {
                        MainWindow_Home_Grid.Visibility = Visibility.Visible;
                        MainWindow_Settings_Grid.Visibility = Visibility.Hidden;
                        ActualPage = "Home";
                        DisplayProfiles();
                    }
                    break;
                case "Logs":
                    OpenLogsFolder();
                    break;
                case "Settings":
                    if (ActualPage != "Settings")
                    {
                        MainWindow_Home_Grid.Visibility = Visibility.Hidden;
                        MainWindow_Settings_Grid.Visibility = Visibility.Visible;
                        ActualPage = "Settings";
                        SetComboBoxes(config);
                    }
                    break;
                default:
                    break;
            }
        }

        private void MainWindow_Settings_Buttons_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            // First extract the values from the ComboBoxes
            string language = MainWindow_Settings_Language_ComboBox.Text;
            string logFormat = MainWindow_Settings_LogFormat_ComboBox.Text;

            // Then check if the values are correct
            if (language == "" || logFormat == "")
            {
                MessageBox.Show("Please select a language and a log format");
                return;
            }

            // Change the values to match the config file
            if (language == "English" || language == "Anglais")
            {
                language = "en";
            }
            else if (language == "French" || language == "Français")
            {
                language = "fr";
            }

            if (logFormat == ".json")
            {
                logFormat = "json";
            }
            else if (logFormat == ".xml")
            {
                logFormat = "xml";
            }

            // Then save the values in the config file
            _configurationViewModel.SaveConfig(paths["ConfigFilePath"], language, logFormat);

            // Then reload the config file
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            // Then reload the language
            Setlanguage();

            // Then reload the comboboxes
            SetComboBoxes(config);

            // Then display a message to the user
            MessageBox.Show("Configuration saved");
        }

        private void MainWindow_Settings_Buttons_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            // First we set the default values
            string language = "en";
            string logFormat = "json";

            // Then save the values in the config file
            _configurationViewModel.SaveConfig(paths["ConfigFilePath"], language, logFormat);

            // Then reload the config file
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            // Then reload the language
            Setlanguage();

            // Then reload the comboboxes
            SetComboBoxes(config);

            // Then display a message to the user
            MessageBox.Show("Configuration reset");
        }

        private void OpenLogsFolder()
        {
            Process process = new Process();
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.Arguments = paths["EasySaveFileLogsDirectoryPath"];
            process.Start();
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
            CreateSaveProfileView _createSaveProfileView = new CreateSaveProfileView(paths, config, saveProfiles);
            _createSaveProfileView.Show();
            MainWindow_MainContentHeader_CreateSave_Button.IsEnabled = false;
            MainWindow_MainContentHeader_CreateSave_Button.Background = Brushes.Black;
            _createSaveProfileView.Closing += CreateSaveProfileView_Closing;

        }

        private void CreateSaveProfileView_Closing(object? sender, CancelEventArgs e)
        {
            MainWindow_MainContentHeader_CreateSave_Button.IsEnabled = true;
            MainWindow_MainContentHeader_CreateSave_Button.Background = Brushes.LightGray;
            DisplayProfiles();
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

        private void SetComboBoxes(Dictionary<string, string> config)
        {
            // Create a new list of log formats
            List<string> logFormats = new List<string>();
            logFormats.Add(".json");
            logFormats.Add(".xml");

            // Create a new list of languages
            List<string> languages = new List<string>();
            languages.Add("English");
            languages.Add("French");

            // Clear the comboboxes
            MainWindow_Settings_LogFormat_ComboBox.Items.Clear();
            MainWindow_Settings_Language_ComboBox.Items.Clear();

            // Create a combobox item for each log format
            foreach (string logFormat in logFormats)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = logFormat;

                // Add the item to the combobox
                MainWindow_Settings_LogFormat_ComboBox.Items.Add(item);

                // Set the selected item
                if ((logFormat == ".json") && (config["logformat"] == "json"))
                {
                    MainWindow_Settings_LogFormat_ComboBox.SelectedItem = item;
                }
                else if ((logFormat == ".xml") && (config["logformat"] == "xml"))
                {
                    MainWindow_Settings_LogFormat_ComboBox.SelectedItem = item;
                }
            }

            // Create a combobox item for each language
            foreach (string language in languages)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = language;

                // Add the item to the combobox
                MainWindow_Settings_Language_ComboBox.Items.Add(item);

                // Set the selected item
                if ((language == "English") && (config["language"] == "en"))
                {
                    MainWindow_Settings_Language_ComboBox.SelectedItem = item;
                }
                else if ((language == "French") && (config["language"] == "fr"))
                {
                    MainWindow_Settings_Language_ComboBox.SelectedItem = item;
                }
            }

        }
    }
}
