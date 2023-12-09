using EasySave.ViewModels;

namespace EasySave
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                _ = new MainViewModel();
            }
            else
            {
                _ = new MainViewModel(args[0]);
            }
        }
    }
}
