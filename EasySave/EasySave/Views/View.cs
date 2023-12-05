using System;

namespace EasySaveConsoleApp
{
    public class ConsoleView
    {
        private readonly MainViewModel _viewModel;

        public ConsoleView(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Display backup profiles");
                Console.WriteLine("2. Modify a backup profile");
                Console.WriteLine("3. Execute a backup");
                Console.WriteLine("4. Quit");

                Console.Write("Choose an option (1-4): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _viewModel.DisplayProfiles();
                        break;
                    case "2":
                        _viewModel.ModifyProfile();
                        break;
                    case "3":
                        _viewModel.ExecuteProfile();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
