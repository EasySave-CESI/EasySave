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
        private List<SaveProfile> saveProfiles;

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
            SetLanguage(printStringDictionary);

            // Display the profiles
            DisplayProfiles();
        }

        private void DisplayProfiles()
        {
            // Epty the list
            MainWindow_List_Profil.ItemsSource = null;
            MainWindow_List_Profil.Items.Clear();

            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);
            ObservableCollection<SaveProfile> profiles = new ObservableCollection<SaveProfile> { };

            foreach (SaveProfile profile in saveProfiles)
            {
                profile.Index = profiles.Count + 1;
                profiles.Add(profile);
            }
            // Ajouter les éléments à la liste
            MainWindow_List_Profil.ItemsSource = profiles;

            MainWindow_NumberProfileLoaded_TextBox.Content = profiles.Count.ToString();
        }

        private void ManageProfile_Click(object sender, RoutedEventArgs e)
        {
            ManageProfileView manageProfileWindow = new ManageProfileView();
            manageProfileWindow.Show();
            MainWindow_ManageProfile_Button.IsEnabled = false;
            manageProfileWindow.Closing += ManageProfileWindow_Closing;
        }

        private void ManageProfileWindow_Starting(object? sender, CancelEventArgs e)
        {
            //
        }

        private void ManageProfileWindow_Closing(object? sender, CancelEventArgs e)
        {
            MainWindow_ManageProfile_Button.IsEnabled = true;
            DisplayProfiles();
        }

        private void ExecuteSave_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSaveView executeSaveWindow = new ExecuteSaveView();
            executeSaveWindow.Show();
            MainWindow_ExecuteSave_Button.IsEnabled = false;
            executeSaveWindow.Closing += ExecuteSaveWindow_Closing;
        }

        private void ExecuteSaveWindow_Starting(object? sender, CancelEventArgs e)
        {
            //
        }

        private void ExecuteSaveWindow_Closing(object? sender, CancelEventArgs e)
        {
            MainWindow_ExecuteSave_Button.IsEnabled = true;
            DisplayProfiles();
        }

        private void ViewLogs_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Opening logs.");
            string appDataRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string logsPath = System.IO.Path.Combine(appDataRoaming, "EasySave", "Logs");
            Process.Start("explorer.exe", logsPath);
        }

        private void Option_Button_Click(object sender, RoutedEventArgs e)
        {
            OptionView optionView = new OptionView();
            optionView.Closing += (s, e) => Option_Button.IsEnabled = true;
            optionView.Show();
            Option_Button.IsEnabled = false;
        }

        private void SetLanguage(Dictionary<string, string> printStringDictionary)
        {
            // Set the language

            // Set the title
            Title = printStringDictionary["Application_MainWindow_Title"];

            // Set the buttons
            MainWindow_ManageProfile_Button.Content = printStringDictionary["Application_MainWindow_ManageProfile_Button"];
            MainWindow_ExecuteSave_Button.Content = printStringDictionary["Application_MainWindow_ExecuteSave_Button"];
            MainWindow_ViewLogs_Button.Content = printStringDictionary["Application_MainWindow_ViewLogs_Button"];

            // Set the labels
            MainWindow_ListOfProfiles_Label.Content = printStringDictionary["Application_MainWindow_ListOfProfiles_Label"];
            MainWindow_NumberOfprofiles_Label.Content = printStringDictionary["Application_MainWindow_NumberOfprofiles_Label"];
            MainWindow_State_Label.Content = printStringDictionary["Application_MainWindow_State_Label"];

            // Set the columns
            MainWindow_Index_Header.Header = printStringDictionary["Application_MainWindow_Index_Header"];
            MainWindow_ProfileName_Header.Header = printStringDictionary["Application_MainWindow_ProfileName_Header"];
            MainWindow_SourceFilePath_Header.Header = printStringDictionary["Application_MainWindow_SourceFilePath_Header"];
            MainWindow_DestinationFilePath_Header.Header = printStringDictionary["Application_MainWindow_TargetFilePath_Header"];
            MainWindow_Type_Header.Header = printStringDictionary["Application_MainWindow_TypeOfSave_Header"];
            MainWindow_State_Header.Header = printStringDictionary["Application_MainWindow_State_Header"];
        }
    }
}