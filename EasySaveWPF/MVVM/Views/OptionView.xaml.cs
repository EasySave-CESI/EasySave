using EasySaveWPF.MVVM.Models;
using EasySaveWPF.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace EasySaveWPF.Views
{
    /// <summary>
    /// Logique d'interaction pour OptionView.xaml
    /// </summary>
    public partial class OptionView : Window
    {
        private readonly MVVM.Models.Configuration _configuration;
        private readonly PathViewModel _pathViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;
        private readonly LanguageConfigurationViewModel _languageConfigurationViewModel;

        private Dictionary<string, string> paths;
        private Dictionary<string, string> config;
        private List<MVVM.Models.Configuration> Configurations;

        public OptionView()
        {
            InitializeComponent();

            _pathViewModel = new PathViewModel();
            _configuration = new MVVM.Models.Configuration();
            _configurationViewModel = new ConfigurationViewModel();
            _languageConfigurationViewModel = new LanguageConfigurationViewModel();

            // Create a new dictionary to store the paths
            paths = _pathViewModel.LoadPaths();

            // Create a new dictionary to store the config
            config = _configurationViewModel.LoadConfig(paths["ConfigFilePath"]);

            // Create a new language configuration
            Dictionary<string, string> printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);

            // Set the language
            SetLanguage(printStringDictionary);

            // Set comboboxes values
            SetComboBoxes(config);
        }

        private void OptionOK_Button_Click(object sender, RoutedEventArgs e)
        {
            /* LOG CONFIGURATION */
            var itemLog = OptionView_LogFormat_ComboBox.SelectedItem as ComboBoxItem;
            MVVM.Models.Configuration configuration = new MVVM.Models.Configuration();
            string newLogFormat = itemLog.Content.ToString();

            if (newLogFormat == ".json")
            {
                configuration.LogFormat = "json";
            }
            else if (newLogFormat == ".xml")
            {
                configuration.LogFormat = "xml";
            }


            /* LANGUAGE CONFIGURATION */
            var itemLanguage = OptionView_Language_ComboBox.SelectedItem as ComboBoxItem;
            string newLanguage = itemLanguage.Content.ToString();

            if (newLanguage == "Englsh")
            {

                configuration.Language = "en";

            }
            else if (newLanguage == "French")
            {
                configuration.Language = "fr";
            }

            MVVM.Models.Configuration.WriteConfig(paths["ConfigFilePath"], configuration.Language, configuration.LogFormat);
            SetLanguage(_languageConfigurationViewModel.LoadPrintStrings(configuration.Language));
            MessageBox.Show("Configuration saved");
            Close();
        }

        private void OptionView_LogFormat_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OptionView_Language_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SetLanguage(Dictionary<string, string> printStringDictionary)
        {
            printStringDictionary = _languageConfigurationViewModel.LoadPrintStrings(config["language"]);

            OptionView_Validate_Button.Content = printStringDictionary["Application_OptionView_Validate_Button"];
            OptionView_LogFormat_Label.Content = printStringDictionary["Application_OptionView_LogFormat_Label"];
            OptionView_Language_Label.Content = printStringDictionary["Application_OptionView_Language_Label"];
        }

        private void SetComboBoxes(Dictionary<string, string> config)
        {
            // Create a new list of log formats
            List<string> logFormats = new List<string>();

            // Add the log formats to the list
            logFormats.Add(".json");
            logFormats.Add(".xml");

            // Create a combobox item for each log format
            foreach (string logFormat in logFormats)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = logFormat;

                // Add the item to the combobox
                OptionView_LogFormat_ComboBox.Items.Add(item);

                // Set the selected item
                if (logFormat == config["logformat"])
                {
                    OptionView_LogFormat_ComboBox.SelectedItem = item;
                }
            }

            // Create a new list of languages
            List<string> languages = new List<string>();

            // Add the languages to the list
            languages.Add("English");
            languages.Add("French");

            // Create a combobox item for each language
            foreach (string language in languages)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = language;

                // Add the item to the combobox
                OptionView_Language_ComboBox.Items.Add(item);

                // Set the selected item
                if (language == config["language"])
                {
                    OptionView_Language_ComboBox.SelectedItem = item;
                }
            }
        }
    }
}
