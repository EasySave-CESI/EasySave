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
    /// Interaction logic for CreateSaveProfile.xaml
    /// </summary>
    public partial class CreateSaveProfileView : Window
    {
        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private List<SaveProfile> saveProfiles;
        public List<string> TypeList { get; set; }
        public CreateSaveProfileView(Dictionary<string, string> paths, Dictionary<string, string> config, List<SaveProfile> saveProfiles)
        {
            InitializeComponent();
            this.paths = paths;
            this.config = config;
            this.saveProfiles = saveProfiles;
            TypeList = new List<string> { "Full", "Differential" };
            DataContext = this;
        }

        private void CreateSaveProfileView_MainGrid1_Source_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog sourceFolderDialog = new OpenFolderDialog();
            sourceFolderDialog.ShowDialog();
            CreateSaveProfileView_MainGrid1_Source_TextBox.Text = sourceFolderDialog.FolderName;
        }

        private void CreateSaveProfileView_MainGrid1_Target_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog targetFolderDialog = new OpenFolderDialog();
            targetFolderDialog.ShowDialog();
            CreateSaveProfileView_MainGrid1_Target_TextBox.Text = targetFolderDialog.FolderName;
        }

        private void CreateSaveProfileView_MainGrid2_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if the save profile name is not empty
            if (CreateSaveProfileView_MainGrid1_Name_TextBox.Text == "")
            {
                MessageBox.Show("The save profile name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the save profile name is not already used
            else if (saveProfiles.Any(profile => profile.Name == CreateSaveProfileView_MainGrid1_Name_TextBox.Text))
            {
                MessageBox.Show("The save profile name is already used.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the source path is not empty
            else if (CreateSaveProfileView_MainGrid1_Source_TextBox.Text == "")
            {
                MessageBox.Show("The source path cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the target path is not empty
            else if (CreateSaveProfileView_MainGrid1_Target_TextBox.Text == "")
            {
                MessageBox.Show("The target path cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the source path exists
            else if (!System.IO.Directory.Exists(CreateSaveProfileView_MainGrid1_Source_TextBox.Text))
            {
                MessageBox.Show("The source path does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Check if the source path is not the same as the target path
            else if (CreateSaveProfileView_MainGrid1_Source_TextBox.Text == CreateSaveProfileView_MainGrid1_Target_TextBox.Text)
            {
                MessageBox.Show("The source path cannot be the same as the target path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (CreateSaveProfileView_MainGrid1_Type_ComboBox.Text == "")
            {
                MessageBox.Show("The type cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Check what the Type is
                string type = CreateSaveProfileView_MainGrid1_Type_ComboBox.Text;

                if (type == "Full" || type == "Complète")
                {
                    type = "full";
                }
                else if (type == "Differential" || type == "Différentielle")
                {
                    type = "diff";
                }

                SaveProfile profile = SaveProfile.CreateSaveProfile(CreateSaveProfileView_MainGrid1_Name_TextBox.Text, CreateSaveProfileView_MainGrid1_Source_TextBox.Text, CreateSaveProfileView_MainGrid1_Target_TextBox.Text, type);
                saveProfiles.Add(profile);
                SaveProfile.SaveProfiles(paths["StateFilePath"], saveProfiles);
                this.Close();
            }
        }

        private void CreateSaveProfileView_MainGrid2_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
