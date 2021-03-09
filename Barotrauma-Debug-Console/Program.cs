using System;

namespace Barotrauma_Debug_Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var handler = new CommandHandler(typeof(Commands));
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                handler.Handle(input ?? "");
            }
        }
    }
}
