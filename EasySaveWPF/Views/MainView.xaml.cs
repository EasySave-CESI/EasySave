using EasySaveWPF.Views;
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

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ManageProfile_Click(object sender, RoutedEventArgs e)
        {
          ManageProfileView manageProfileWindow = new ManageProfileView();
            manageProfileWindow.Closing += (s,e) => ManageProfile_Button.IsEnabled=true; //Enable the button when ManageProfileView is closed
            manageProfileWindow.Show();                                                  //Open a new window
            ManageProfile_Button.IsEnabled = false;                                      //Disable the button
            
            
        }


        private void ExecuteSave_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSaveView executeSaveWindow = new ExecuteSaveView();
            executeSaveWindow.Closing += (s, e) => ExecuteSave_Button.IsEnabled = true;
            executeSaveWindow.Show();
            ExecuteSave_Button.IsEnabled = false;
        }

        private void ViewLogs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Opening logs.");
        }

        private void Option_Button_Click(object sender, RoutedEventArgs e)
        {
            OptionView optionView = new OptionView();
            optionView.Closing += (s, e) => Option_Button.IsEnabled = true;
            optionView.Show();
            Option_Button.IsEnabled = false;
        }


    }
}