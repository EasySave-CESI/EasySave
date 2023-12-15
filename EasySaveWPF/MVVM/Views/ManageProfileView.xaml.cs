using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EasySaveWPF.MVVM.Views;
using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.ViewModels;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;


namespace EasySaveWPF.Views
{
   
    public partial class ManageProfileView : Window
    {
        private readonly PathViewModel _pathViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;
        private readonly LanguageConfigurationViewModel _languageConfigurationViewModel;
        private readonly SaveProfileViewModel _saveProfileViewModel;

        private ObservableCollection<SaveProfile> profiles;
        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private Dictionary<string, string> printStringDictionary;
        private List<SaveProfile> saveProfiles;

        public ManageProfileView()
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

            // Create a new list to store the save profiles
            saveProfiles = _saveProfileViewModel.LoadSaveProfiles(paths["StateFilePath"]);

            // Set the language
            SetLanguage(printStringDictionary);

            DisplayProfiles();
        }

        private void DisplayProfiles()
        {
            profiles = new ObservableCollection<SaveProfile> { };

            foreach (SaveProfile profile in saveProfiles)
            {
                profile.Index = profiles.Count + 1;
                profiles.Add(profile);
            }

            ManageProfileView_ListProfil_ListView.ItemsSource = profiles;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }



        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void ManageProfileView_Validate_Button_Click(object sender, RoutedEventArgs e)
        {
            var item = ManageProfileView_ListProfil_ListView.SelectedItem as SaveProfile;
            if (item == null)
            {
                MessageBox.Show("Please select a profile to edit");
                return;
            }
            item.SourceFilePath = ManageProfileView_Source_Textbox.Text;
            item.TargetFilePath = ManageProfileView_Destination_Textbox.Text;
            item.TypeOfSave = ManageProfileView_TypeFull_RadioButton.IsChecked == true ? "full" : "diff";
            ManageProfileView_ListProfil_ListView.Items.Refresh();

            saveProfiles = profiles.ToList();
            SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
        }

        private void ManageProfileView_Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Source_Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog sourceFolderDialog = new OpenFolderDialog();
            sourceFolderDialog.ShowDialog();                              // Open Folder Dialog
            string sourcepathText = sourceFolderDialog.FolderName;
            ManageProfileView_Source_Textbox.Text = sourcepathText;
        }

        private void SourceTextbox(object sender, TextChangedEventArgs e)
        {
         
        }

        private void Destination_Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog destinationFolderDialog = new OpenFolderDialog();
            destinationFolderDialog.ShowDialog();
            string destinationpathText = destinationFolderDialog.FolderName;
            ManageProfileView_Destination_Textbox.Text = destinationpathText;
        }

        private void DestinationTextBox(object sender, TextChangedEventArgs e)
        {

        }

        private void enter(object sender, TextCompositionEventArgs e)
        {

        }

        private void AddProfile_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ManageProfileView_Source_Textbox.Text == "" || ManageProfileView_Destination_Textbox.Text == "" || (ManageProfileView_TypeFull_RadioButton.IsChecked == false && ManageProfileView_TypeDiff_RadioButton.IsChecked == false) || (ManageProfileView_EncryptionNo_RadioButton.IsChecked == false && ManageProfileView_EncryptionYes_RadioButton.IsChecked == false))
            {
                MessageBox.Show("Please enter a source, a destination, a type of save and an encryption option");
                return;
            }
            SaveProfile newProfile = new SaveProfile();

            newProfile.Name = Interaction.InputBox("Enter the name of the profile", "Add a new profile", "Save", 100, 100);
            if (newProfile.Name == "")
            {
                return;
            }

            newProfile.SourceFilePath = ManageProfileView_Source_Textbox.Text;
            newProfile.TargetFilePath = ManageProfileView_Destination_Textbox.Text;
            newProfile.TypeOfSave = ManageProfileView_TypeFull_RadioButton.IsChecked == true ? "full" : "diff";

            List<long> sourcedirectoryinfo = SaveProfile.sourceDirectoryInfos(newProfile.SourceFilePath);

            newProfile.TotalFilesToCopy = (int)sourcedirectoryinfo[0];
            newProfile.TotalFilesSize = sourcedirectoryinfo[1];
            newProfile.NbFilesLeftToDo = (int)sourcedirectoryinfo[0];
            newProfile.Progression = 0;

            newProfile.Encryption = ManageProfileView_EncryptionYes_RadioButton.IsChecked == true ? true : false;
            if (newProfile.Encryption == true)
            {
                newProfile.EncryptionKey = Interaction.InputBox("Enter the encryption key", "Add a new profile", "Save", 100, 100);
            }
            else
            {
                newProfile.EncryptionKey = "";
            }

            newProfile.State = "READY";

            SaveProfile.AddProfile(paths["StateFilePath"], newProfile);
            newProfile.Index = profiles.Count + 1;
            profiles.Add(newProfile);
            ManageProfileView_ListProfil_ListView.Items.Refresh();





        }

        private void List_Profil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ManageProfileView_ListProfil_ListView.SelectedItem as SaveProfile;
            
            if (item == null)
            {
                return;
            }
            ManageProfileView_Source_Textbox.Text = (item.SourceFilePath);
            ManageProfileView_Destination_Textbox.Text = (item.TargetFilePath);

            if (item.TypeOfSave == "full")
            {
                ManageProfileView_TypeFull_RadioButton.IsChecked = true;
            }
            else if (item.TypeOfSave == "diff")
            {
                ManageProfileView_TypeDiff_RadioButton.IsChecked = true;
            }
            else
            {
                ManageProfileView_TypeDiff_RadioButton.IsChecked = false;
                ManageProfileView_TypeFull_RadioButton.IsChecked = false;
            }
           
           

        }

        private void DeleteProfile_Button_Click(object sender, RoutedEventArgs e)
        {
            var item = ManageProfileView_ListProfil_ListView.SelectedItem as SaveProfile;

            if (item == null)
            {
                MessageBox.Show("Please select a profile to delete");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this profile ?" + item.Name, "Delete a profile", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            foreach (SaveProfile profile in saveProfiles)
            {
                if (profile.Name == item.Name && profile.SourceFilePath == item.SourceFilePath && profile.TargetFilePath == item.TargetFilePath)
                {
                    item = profile;
                }
            }

            profiles.Remove(item);
            SaveProfile.SaveProfiles(paths["StateFilePath"], profiles.ToList());
            ManageProfileView_ListProfil_ListView.Items.Refresh();
        }

        private void SetLanguage(Dictionary<string, string> printStringDictionary)
        {
            printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);

            ManageProfileView_Validate_Button.Content = printStringDictionary["Application_ManageProfileView_Validate_Button"];
            ManageProfileView_Exit_Button.Content = printStringDictionary["Application_ManageProfileView_Exit_Button"];

            ManageProfileView_Source_Label.Content = printStringDictionary["Application_ManageProfileView_Source_Label"];
            ManageProfileView_Destination_Label.Content = printStringDictionary["Application_ManageProfileView_Destination_Label"];

            ManageProfileView_Type_Label.Content = printStringDictionary["Application_ManageProfileView_Type_Label"];
            ManageProfileView_TypeFull_RadioButton.Content = printStringDictionary["Application_ManageProfileView_TypeFull_RadioButton"];
            ManageProfileView_TypeDiff_RadioButton.Content = printStringDictionary["Application_ManageProfileView_TypeDiff_RadioButton"];

            ManageProfileView_Index_Header.Header = printStringDictionary["Application_ManageProfileView_Index_Header"];
            ManageProfileView_ProfileName_Header.Header = printStringDictionary["Application_ManageProfileView_ProfileName_Header"];
            ManageProfileView_SourceFilePath_Header.Header = printStringDictionary["Application_ManageProfileView_SourceFilePath_Header"];
            ManageProfileView_DestinationFilePath_Header.Header = printStringDictionary["Application_ManageProfileView_DestinationFilePath_Header"];
            ManageProfileView_Type_Header.Header = printStringDictionary["Application_ManageProfileView_TypeOfSave_Header"];
            ManageProfileView_State_Header.Header = printStringDictionary["Application_ManageProfileView_State_Header"];
        }
    }
}
