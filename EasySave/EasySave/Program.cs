using System;
using System.Collections.Generic;

namespace EasySaveConsoleApp
{
    class Program
    {
        private const string StateFilePath = "..\\..\\..\\logs\\state.json";

        static void Main(string[] args)
        {
            if (!File.Exists(StateFilePath))
            {
                
            }

            List<Profile> profiles = Profile.LoadProfiles(StateFilePath);

            if (profiles.Count == 0)
            {
                Console.WriteLine("No backup profiles have been loaded.");
                return;
            }
            else
            {
                Console.WriteLine("Number of loaded profiles: " + profiles.Count);

                var viewModel = new MainViewModel(profiles);
                var consoleView = new ConsoleView(viewModel);

                consoleView.Run();
            }
        }
    }
}
