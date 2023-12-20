using EasySave.MVVM.ViewModels;

namespace EasySave
{
    class Program
    {
        static void Main(string[] args)
        {
            string version = "1.1.0";

            if (args.Length == 0)
            {
                _ = new MainViewModel("", version);
            }
            else
            {
                _ = new MainViewModel(args[0], version);
            }
        }
    }
}
