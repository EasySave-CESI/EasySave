using System.Configuration;
using System.Data;
using System.Windows;

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string MutexEasySave = "MutexEasySave";

        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            mutex = new Mutex(true, MutexEasySave, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("L'application est déjà en cours d'exécution.", "EasySave", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                Current.Shutdown();
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex.ReleaseMutex();
            mutex.Dispose();

            base.OnExit(e);
        }
    }

}
