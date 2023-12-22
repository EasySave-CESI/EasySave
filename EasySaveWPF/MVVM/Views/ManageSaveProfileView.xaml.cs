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
using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.ViewModels;
using Microsoft.Win32;

namespace EasySaveWPF.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ManageSaveProfile.xaml
    /// </summary>
    public partial class ManageSaveProfileView : Window
    {
        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private List<SaveProfile> saveProfiles;
        private SaveProfile profiletoedit;
        private string option;
        public List<string> TypeList { get; set; }
        public ManageSaveProfileView(Dictionary<string, string> paths, Dictionary<string, string> config, List<SaveProfile> saveProfiles, string option, SaveProfile profile)
        {
            InitializeComponent();
            this.paths = paths;
            this.config = config;
            this.saveProfiles = saveProfiles;
            this.option = option;
            this.profiletoedit = profile;
            TypeList = new List<string> { "Full", "Differential" };
            DataContext = this;

            if (option == "Edit")
            {
                DisplayProfileToEdit();
            }
        }

        private void ManageSaveProfileView_MainGrid1_Source_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog sourceFolderDialog = new OpenFolderDialog();
            sourceFolderDialog.ShowDialog();
            ManageSaveProfileView_MainGrid1_Source_TextBox.Text = sourceFolderDialog.FolderName;
        }

        private void ManageSaveProfileView_MainGrid1_Target_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog targetFolderDialog = new OpenFolderDialog();
            targetFolderDialog.ShowDialog();
            ManageSaveProfileView_MainGrid1_Target_TextBox.Text = targetFolderDialog.FolderName;
        }

        private void ManageSaveProfileView_MainGrid2_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if the save profile name is not empty
            if (ManageSaveProfileView_MainGrid1_Name_TextBox.Text == "")
            {
                MessageBox.Show("The save profile name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the save profile name is not already used and the option is Add
            else if (saveProfiles.Any(profile => profile.Name == ManageSaveProfileView_MainGrid1_Name_TextBox.Text) && option == "Add")
            {
                MessageBox.Show("The save profile name is already used.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the source path is not empty
            else if (ManageSaveProfileView_MainGrid1_Source_TextBox.Text == "")
            {
                MessageBox.Show("The source path cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the target path is not empty
            else if (ManageSaveProfileView_MainGrid1_Target_TextBox.Text == "")
            {
                MessageBox.Show("The target path cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the source path exists
            else if (!System.IO.Directory.Exists(ManageSaveProfileView_MainGrid1_Source_TextBox.Text))
            {
                MessageBox.Show("The source path does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the source path is not the same as the target path
            else if (ManageSaveProfileView_MainGrid1_Source_TextBox.Text == ManageSaveProfileView_MainGrid1_Target_TextBox.Text)
            {
                MessageBox.Show("The source path cannot be the same as the target path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (ManageSaveProfileView_MainGrid1_Type_ComboBox.Text == "")
            {
                MessageBox.Show("The type cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Check what the Type is
                string type = ManageSaveProfileView_MainGrid1_Type_ComboBox.Text;

                if (type == "Full" || type == "Complète")
                {
                    type = "full";
                }
                else if (type == "Differential" || type == "Différentielle")
                {
                    type = "diff";
                }

                switch (option)
                {
                    case "Add":
                        AddSaveProfile(type);
                        break;
                    case "Edit":
                        EditSaveProfile(type);
                        break;
                }
                this.Close();
            }
        }

        private void AddSaveProfile(string type)
        {
            SaveProfile profile = SaveProfile.CreateSaveProfile(ManageSaveProfileView_MainGrid1_Name_TextBox.Text, ManageSaveProfileView_MainGrid1_Source_TextBox.Text, ManageSaveProfileView_MainGrid1_Target_TextBox.Text, type);
            saveProfiles.Add(profile);
            SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
        }

        private void EditSaveProfile(string type)
        {
            profiletoedit.Name = ManageSaveProfileView_MainGrid1_Name_TextBox.Text;
            profiletoedit.SourceFilePath = ManageSaveProfileView_MainGrid1_Source_TextBox.Text;
            profiletoedit.TargetFilePath = ManageSaveProfileView_MainGrid1_Target_TextBox.Text;
            profiletoedit.TypeOfSave = type;
            SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
        }

        private void DisplayProfileToEdit()
        {
            // BAD FIX This MessageBox is only for debug the display of the profile to edit
            MessageBox.Show("Editing Save Profile: " + profiletoedit.Name, "Edit Save Profile", MessageBoxButton.OK, MessageBoxImage.Information);
            // END BAD FIX

            ManageSaveProfileView_MainGrid1_Name_TextBox.Text = profiletoedit.Name;
            ManageSaveProfileView_MainGrid1_Source_TextBox.Text = profiletoedit.SourceFilePath;
            ManageSaveProfileView_MainGrid1_Target_TextBox.Text = profiletoedit.TargetFilePath;
            ManageSaveProfileView_MainGrid1_Type_ComboBox.Text = profiletoedit.TypeOfSave;
        }

        private void ManageSaveProfileView_MainGrid2_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
