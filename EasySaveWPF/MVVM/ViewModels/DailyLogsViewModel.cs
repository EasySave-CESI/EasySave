using EasySaveWPF.MVVM.Views;
using EasySaveWPF.MVVM.Models;


namespace EasySaveWPF.MVVM.ViewModels
{
    public class DailyLogsViewModel
    {
        public DailyLogs dailyLogs { get; set; }

        public DailyLogsViewModel(string logsDirectory, string format)
        {
            dailyLogs = new DailyLogs(logsDirectory, format);
        }

        public void CreateLog(string path, string logformat, string name, string sourcefilepath, string targetfilepath, long fileSize, double fileTransferTime)
        {
            dailyLogs.CreateLog(name, sourcefilepath, targetfilepath, fileSize, fileTransferTime);
        }

        public void DisplayLogs(ConsoleView consoleView, string path, string logformat)
        {
            foreach (DailyLog log in dailyLogs.LoadLogs())
            {
                consoleView.DisplayLog(log);
            }
        }

        public void SaveLogs(string path, string logformat)
        {
            dailyLogs.SaveLogs();
        }
    }
}
