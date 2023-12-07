using System;

namespace EasySaveConsoleApp
{
    public class ConsoleView
    {
        public ConsoleView() { }
        public void print(string text)
        {
            Console.WriteLine(text);
        }

        public string read()
        {
            return Console.ReadLine();
        }

        public void clear()
        {
            Console.Clear();
        }

        public void printError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void printSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void printWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void printInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void printSeparator()
        {
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
