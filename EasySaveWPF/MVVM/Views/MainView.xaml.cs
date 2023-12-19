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
        private Dictionary<string, string>? printStringDictionary;
        private List<SaveProfile> saveProfiles;
        private List<SaveProfile> profilesToExecute = new List<SaveProfile>();
        private ObservableCollection<SaveProfile> profiles = new ObservableCollection<SaveProfile>();

        private DispatcherTimer dispatcherTimer;

        private bool isSavingBigFiles = false;
        private string ActualPage;

        public MainWindow()
        {
            InitializeComponent();
            _pathViewModel = new PathViewModel();
            _configurationViewModel = new ConfigurationViewModel();
            _languageConfigurationViewModel = new LanguageConfigurationViewModel();
            _saveProfileViewModel = new SaveProfileViewModel();
            paths = _pathViewModel.LoadPaths();
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);
            Dictionary<string, string> printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);
            Setlanguage();
            SetThemeColors();
            HandlePageSelection("Home");

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void MainWindow_NavigationBar_PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            ListBoxItem selectedListBoxItem = (ListBoxItem)listBox.SelectedItem;

            if (selectedListBoxItem != null)
            {
                string selectedPage = selectedListBoxItem.Content.ToString();
                if (selectedPage == "Accueil") {selectedPage = "Home";}
                else if (selectedPage == "Journaux") {selectedPage = "Logs";}
                else if (selectedPage == "Paramètres") {selectedPage = "Settings";}
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
            string selectedLanguage = MainWindow_Settings_Language_ComboBox.Text;
            string selectedLogFormat = MainWindow_Settings_LogFormat_ComboBox.Text;
            string selectedTheme = MainWindow_Settings_Theme_ComboBox.Text;

            string newLanguage = "", newLogFormat = "", newTheme = "";

            if (string.IsNullOrWhiteSpace(selectedLanguage) || string.IsNullOrWhiteSpace(selectedLogFormat) || string.IsNullOrWhiteSpace(selectedTheme)) { MessageBox.Show("Please select a value for each field"); return;}

            if (selectedLanguage == "English" || selectedLanguage == "Anglais") { newLanguage = "en";}
            else if (selectedLanguage == "French" || selectedLanguage == "Français") { newLanguage = "fr";}

            if (selectedLogFormat == ".json") {newLogFormat = "json";}
            else if (selectedLogFormat == ".xml") {newLogFormat = "xml";}

            if (selectedTheme == "Light" || selectedTheme == "Clair") {newTheme = "light";}
            else if (selectedTheme == "Dark" || selectedTheme == "Sombre") {newTheme = "dark";}

            _configurationViewModel.SaveConfig(paths["ConfigFilePath"], newLanguage, newLogFormat, newTheme);

            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            Setlanguage();
            SetComboBoxes(config);
            SetThemeColors();
        }

        private void MainWindow_Settings_Buttons_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            string language = "en";
            string logFormat = "json";
            string theme = "light";

            _configurationViewModel.SaveConfig(paths["ConfigFilePath"], language, logFormat, theme);
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            Setlanguage();
            SetComboBoxes(config);
            SetThemeColors();
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
            foreach (SaveProfile profile in saveProfiles) {profiles.Add(profile);}
            MainWindow_Home_ExistingSaves_Grid.ItemsSource = profiles;
        }

        private void SelectionDot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse selectionDot = (Ellipse)sender;

            if (selectionDot.Fill == Brushes.White) {selectionDot.Fill = Brushes.Black; profilesToExecute.Add((SaveProfile)selectionDot.DataContext);}
            else {selectionDot.Fill = Brushes.White; profilesToExecute.Remove((SaveProfile)selectionDot.DataContext);}
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
            if (result == MessageBoxResult.Yes) {_saveProfileViewModel.DeleteSaveProfile(saveProfiles, profileToDelete, paths); DisplayProfiles();}
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

        private void MainWindow_Home_Header_ExecuteAll_Button_Click(object sender, RoutedEventArgs e) {foreach (SaveProfile profile in profilesToExecute) {MessageBox.Show($"Executing {profile.Name}");}}

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            DisplayProfiles();
        }

        private void Setlanguage()
        {
            printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);
            MainWindow_NavigationBar_EasySaveName_Label.Content = printStringDictionary["Application_MainWindow_Title"];
            MainWindow_NavigationBar_PagesList_Home.Content = printStringDictionary["Application_MainWindow_NavigationBar_PagesList_Home"];
            MainWindow_NavigationBar_PagesList_Logs.Content = printStringDictionary["Application_MainWindow_NavigationBar_PagesList_Logs"];
            MainWindow_NavigationBar_PagesList_Settings.Content = printStringDictionary["Application_MainWindow_NavigationBar_PagesList_Settings"];
            MainWindow_Home_Header_ExistingSaves_Label.Content = printStringDictionary["Application_MainWindow_MainContentHeader_ExistingSaves_Label"];
            MainWindow_Home_Header_ExecuteAll_Button.Content = printStringDictionary["Application_MainWindow_MainContentHeader_ExecuteAll_Button"];
            MainWindow_Home_Header_CreateSave_Button.Content = printStringDictionary["Application_MainWindow_MainContentHeader_CreateSave_Button"];
            MainWindow_Settings_Language_Label.Content = printStringDictionary["Application_MainWindow_Settings_Language_Label"];
            MainWindow_Settings_LogFormat_Label.Content = printStringDictionary["Application_MainWindow_Settings_LogFormat_Label"];
            MainWindow_Settings_Theme_Label.Content = printStringDictionary["Application_MainWindow_Settings_Theme_Label"];
            MainWindow_Settings_Buttons_Save_Button.Content = printStringDictionary["Application_MainWindow_Settings_Buttons_Save_Button"];
            MainWindow_Settings_Buttons_Reset_Button.Content = printStringDictionary["Application_MainWindow_Settings_Buttons_Reset_Button"];
        }

        private void SetComboBoxes(Dictionary<string, string> config)
        {
            List<string> logFormats = new List<string> {printStringDictionary["Application_MainWindow_Settings_LogFormat_Json"], printStringDictionary["Application_MainWindow_Settings_LogFormat_Xml"]};
            List<string> languages = new List<string> {printStringDictionary["Application_MainWindow_Settings_Language_English"], printStringDictionary["Application_MainWindow_Settings_Language_French"]};
            List<string> themes = new List<string> {printStringDictionary["Application_MainWindow_Settings_Theme_Light"], printStringDictionary["Application_MainWindow_Settings_Theme_Dark"]};

            MainWindow_Settings_LogFormat_ComboBox.Items.Clear();
            MainWindow_Settings_Language_ComboBox.Items.Clear();
            MainWindow_Settings_Theme_ComboBox.Items.Clear();

            foreach (string logFormat in logFormats){
                ComboBoxItem item = new ComboBoxItem();
                item.Content = logFormat;
                MainWindow_Settings_LogFormat_ComboBox.Items.Add(item);
                if ((logFormat == ".json") && (config["logformat"] == "json")) {MainWindow_Settings_LogFormat_ComboBox.SelectedItem = item;}
                else if ((logFormat == ".xml") && (config["logformat"] == "xml")) {MainWindow_Settings_LogFormat_ComboBox.SelectedItem = item;}}

            foreach (string language in languages) {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = language;
                MainWindow_Settings_Language_ComboBox.Items.Add(item);
                if ((language == "English") && (config["language"] == "en")) {MainWindow_Settings_Language_ComboBox.SelectedItem = item;}
                else if ((language == "French") && (config["language"] == "fr")) {MainWindow_Settings_Language_ComboBox.SelectedItem = item;}}

            foreach (string theme in themes) {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = theme;
                MainWindow_Settings_Theme_ComboBox.Items.Add(item);
                if ((theme == "Light") && (config["theme"] == "light")) {MainWindow_Settings_Theme_ComboBox.SelectedItem = item;}
                else if ((theme == "Dark") && (config["theme"] == "dark")) {MainWindow_Settings_Theme_ComboBox.SelectedItem = item;}}

        }

        private void SetThemeColors()
        {
            if (config["theme"] == "light") {
                Resources["BackgroundColor"] = Resources["LightBackgroundColor"];
                Resources["NavigationBarColor"] = Resources["LightNavigationBarColor"];
                Resources["SaveBackgroundColor"] = Resources["LightSaveBackgroundColor"];
                Resources["ButtonBackgroundColor"] = Resources["LightButtonBackgroundColor"];}
            else if (config["theme"] == "dark") {
                Resources["BackgroundColor"] = Resources["DarkBackgroundColor"];
                Resources["NavigationBarColor"] = Resources["DarkNavigationBarColor"];
                Resources["SaveBackgroundColor"] = Resources["DarkSaveBackgroundColor"];
                Resources["ButtonBackgroundColor"] = Resources["DarkButtonBackgroundColor"];}
        }
    }
}
