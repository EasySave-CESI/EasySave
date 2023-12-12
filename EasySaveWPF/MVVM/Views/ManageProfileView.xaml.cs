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

namespace EasySaveWPF.Views
{
   
    public partial class ManageProfileView : Window
    {
        private readonly PathViewModel _pathViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;
        private readonly SaveProfileViewModel _saveProfileViewModel;

        private ObservableCollection<SaveProfile> profiles;
        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private List<SaveProfile> saveProfiles;

        public ManageProfileView()
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


private void Validate_ManageProfileView_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_ManageProfileView_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private void Source_Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog sourceFolderDialog = new OpenFolderDialog();
            sourceFolderDialog.ShowDialog();                              // Open Folder Dialog
            string sourcepathText = sourceFolderDialog.FolderName;        
            Source_Textbox.Text = sourcepathText;
        }

        private void SourceTextbox(object sender, TextChangedEventArgs e)
        {
         
        }

        private void Destination_Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog destinationFolderDialog = new OpenFolderDialog();
            destinationFolderDialog.ShowDialog();
            string destinationpathText = destinationFolderDialog.FolderName;
            Destination_Textbox.Text = destinationpathText;
        }

        private void DestinationTextBox(object sender, TextChangedEventArgs e)
        {

        }

        private void enter(object sender, TextCompositionEventArgs e)
        {

        }

        private void AddProfile_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
