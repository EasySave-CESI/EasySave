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
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
        private readonly DailyLogsViewModel _dailyLogsViewModel;

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
            // Initialize the window
            InitializeComponent();

            // Initialize the view models
            _pathViewModel = new PathViewModel();
            _configurationViewModel = new ConfigurationViewModel();
            _languageConfigurationViewModel = new LanguageConfigurationViewModel();
            _saveProfileViewModel = new SaveProfileViewModel();
            

            // Create a new dictionary to store the paths
            paths = _pathViewModel.LoadPaths();

            // Create a new dictionary to store the config
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);
            _dailyLogsViewModel = new DailyLogsViewModel(paths["EasySaveFileLogsDirectoryPath"], config["logformat"]); ;

            // Create a new language configuration
            Dictionary<string, string> printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);

            // Create a new list to store the save profiles
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);

            // Set the language
            Setlanguage();

            // Set the theme
            SetThemeColors();

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
            string selectedLanguage = MainWindow_Settings_Language_ComboBox.Text;
            string selectedLogFormat = MainWindow_Settings_LogFormat_ComboBox.Text;
            string selectedTheme = MainWindow_Settings_Theme_ComboBox.Text;

            string newLanguage = "";
            string newLogFormat = "";
            string newTheme = "";

            // Then check if the values are correct
            if (selectedLanguage == "" || selectedLogFormat == "" || selectedTheme == "")
            {
                MessageBox.Show("Please select a value for each field");
                return;
            }

            // Change the values to match the config file
            if (selectedLanguage == "English" || selectedLanguage == "Anglais")
            {
                newLanguage = "en";
            }
            else if (selectedLanguage == "French" || selectedLanguage == "Français")
            {
                newLanguage = "fr";
            }

            if (selectedLogFormat == ".json")
            {
                newLogFormat = "json";
            }
            else if (selectedLogFormat == ".xml")
            {
                newLogFormat = "xml";
            }

            if (selectedTheme == "Light" || selectedTheme == "Clair")
            {
                newTheme = "light";
            }
            else if (selectedTheme == "Dark" || selectedTheme == "Sombre")
            {
                newTheme = "dark";
            }

            // Then save the values in the config file
            _configurationViewModel.SaveConfig(paths["ConfigFilePath"], newLanguage, newLogFormat, newTheme);

            // Then reload the config file
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);



            // Then reload the language
            Setlanguage();

            // Then reload the comboboxes
            SetComboBoxes(config);

            // Then reload the theme
            SetThemeColors();

            // Then display a message to the user
            MessageBox.Show("Configuration saved");
        }

        private void MainWindow_Settings_Buttons_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            // First we set the default values
            string language = "en";
            string logFormat = "json";
            string theme = "light";

            // Then save the values in the config file
            _configurationViewModel.SaveConfig(paths["ConfigFilePath"], language, logFormat, theme);

            // Then reload the config file
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            // Then reload the language
            Setlanguage();

            // Then reload the comboboxes
            SetComboBoxes(config);

            // Then reload the theme
            SetThemeColors();

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
            MainWindow_Home_ExistingSaves_Grid.ItemsSource = profiles;
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

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Button startButton = (Button)sender;
            SaveProfile profileToStart = (SaveProfile)startButton.DataContext;

            MessageBox.Show($"Starting {profileToStart.Name}");

            await Task.Run(() =>
            {
                _saveProfileViewModel.ExecuteSaveProfile(_dailyLogsViewModel, saveProfiles, paths, config, profiles.IndexOf(profileToStart));
            });

            DisplayProfiles();
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

        private void MainWindow_Home_Header_CreateSave_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateSaveProfileView _createSaveProfileView = new CreateSaveProfileView(paths, config, saveProfiles);
            _createSaveProfileView.Show();
            MainWindow_Home_Header_CreateSave_Button.IsEnabled = false;
            MainWindow_Home_Header_CreateSave_Button.Background = Brushes.Black;
            _createSaveProfileView.Closing += CreateSaveProfileView_Closing;

        }

        private void CreateSaveProfileView_Closing(object? sender, CancelEventArgs e)
        {
            MainWindow_Home_Header_CreateSave_Button.IsEnabled = true;
            MainWindow_Home_Header_CreateSave_Button.Background = Brushes.LightGray;
            DisplayProfiles();
        }

        /*private async void MainWindow_Home_Header_ExecuteAll_Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.WhenAll(profilesToExecute.Select(profile =>
            _saveProfileViewModel.ExecuteSaveProfile(_dailyLogsViewModel, saveProfiles, paths, config, profiles.IndexOf(profile))));

            DisplayProfiles();
        }*/

        private async void MainWindow_Home_Header_ExecuteAll_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (SaveProfile profile in profiles)
            {
                await Task.Run(() =>
                {
                    _saveProfileViewModel.ExecuteSaveProfile(_dailyLogsViewModel, saveProfiles, paths, config, profiles.IndexOf(profile));
                });
            }

            DisplayProfiles();
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
            MainWindow_Home_Header_ExistingSaves_Label.Content = printStringDictionary["Application_MainWindow_MainContentHeader_ExistingSaves_Label"];

            // Execute all button
            MainWindow_Home_Header_ExecuteAll_Button.Content = printStringDictionary["Application_MainWindow_MainContentHeader_ExecuteAll_Button"];

            // Create button
            MainWindow_Home_Header_CreateSave_Button.Content = printStringDictionary["Application_MainWindow_MainContentHeader_CreateSave_Button"];

            // Save profiles
            /*
            MainWindow_Home_SaveProfileStart_Button.Content = printStringDictionary["Application_MainWindow_MainContent_SaveProfileStart_Button"];
            MainWindow_Home_SaveProfileStop_Button.Content = printStringDictionary["Application_MainWindow_MainContent_SaveProfileStop_Button"];
            MainWindow_Home_SaveProfileModify_Button.Content = printStringDictionary["Application_MainWindow_MainContent_SaveProfileModify_Button"];
            MainWindow_Home_SaveProfileDelete_Button.Content = printStringDictionary["Application_MainWindow_MainContent_SaveProfileDelete_Button"];
            */

            // Settings page
            MainWindow_Settings_Language_Label.Content = printStringDictionary["Application_MainWindow_Settings_Language_Label"];
            MainWindow_Settings_LogFormat_Label.Content = printStringDictionary["Application_MainWindow_Settings_LogFormat_Label"];
            MainWindow_Settings_Theme_Label.Content = printStringDictionary["Application_MainWindow_Settings_Theme_Label"];
            MainWindow_Settings_Buttons_Save_Button.Content = printStringDictionary["Application_MainWindow_Settings_Buttons_Save_Button"];
            MainWindow_Settings_Buttons_Reset_Button.Content = printStringDictionary["Application_MainWindow_Settings_Buttons_Reset_Button"];
        }

        private void SetComboBoxes(Dictionary<string, string> config)
        {
            // Create a new list of log formats
            List<string> logFormats = new List<string>();
            logFormats.Add(printStringDictionary["Application_MainWindow_Settings_LogFormat_Json"]);
            logFormats.Add(printStringDictionary["Application_MainWindow_Settings_LogFormat_Xml"]);

            // Create a new list of languages
            List<string> languages = new List<string>();
            languages.Add(printStringDictionary["Application_MainWindow_Settings_Language_English"]);
            languages.Add(printStringDictionary["Application_MainWindow_Settings_Language_French"]);

            // Create a new list of themes
            List<string> themes = new List<string>();
            themes.Add(printStringDictionary["Application_MainWindow_Settings_Theme_Light"]);
            themes.Add(printStringDictionary["Application_MainWindow_Settings_Theme_Dark"]);

            // Clear the comboboxes
            MainWindow_Settings_LogFormat_ComboBox.Items.Clear();
            MainWindow_Settings_Language_ComboBox.Items.Clear();
            MainWindow_Settings_Theme_ComboBox.Items.Clear();

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

            // Create a combobox item for each theme
            foreach (string theme in themes)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = theme;

                // Add the item to the combobox
                MainWindow_Settings_Theme_ComboBox.Items.Add(item);

                // Set the selected item
                if ((theme == "Light") && (config["theme"] == "light"))
                {
                    MainWindow_Settings_Theme_ComboBox.SelectedItem = item;
                }
                else if ((theme == "Dark") && (config["theme"] == "dark"))
                {
                    MainWindow_Settings_Theme_ComboBox.SelectedItem = item;
                }
            }

        }

        private void SetThemeColors()
        {
            if (config["theme"] == "light")
            {
                // Set light theme colors
                Resources["BackgroundColor"] = Resources["LightBackgroundColor"];
                Resources["NavigationBarColor"] = Resources["LightNavigationBarColor"];
                Resources["SaveBackgroundColor"] = Resources["LightSaveBackgroundColor"];
                Resources["ButtonBackgroundColor"] = Resources["LightButtonBackgroundColor"];
            }
            else if (config["theme"] == "dark")
            {
                // Set dark theme colors
                Resources["BackgroundColor"] = Resources["DarkBackgroundColor"];
                Resources["NavigationBarColor"] = Resources["DarkNavigationBarColor"];
                Resources["SaveBackgroundColor"] = Resources["DarkSaveBackgroundColor"];
                Resources["ButtonBackgroundColor"] = Resources["DarkButtonBackgroundColor"];
            }
        }

        private void Transfert_Limit_TextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
