using System;

namespace Barotrauma_Debug_Console
{
    internal class Program
    {
        public static readonly CommandHandler Handler = new(typeof(Commands));

        private static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                string input = ConsoleExtensions.ReadInput();
                Handler.Handle(input ?? "");
            }
        }
    }
}
