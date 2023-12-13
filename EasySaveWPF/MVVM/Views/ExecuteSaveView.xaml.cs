using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasySaveWPF.Views
{
    /// <summary>
    /// Logique d'interaction pour ExecuteSaveView.xaml
    /// </summary>
    public partial class ExecuteSaveView : Window
    {
        public ExecuteSaveView()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Start_ExecuteSaveView_Click(object sender, RoutedEventArgs e)
        {
 
            Close();
        }

        private void Cancel_ExecuteSaveView_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Textbox_preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
