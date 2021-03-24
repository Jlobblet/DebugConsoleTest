using System;

namespace Barotrauma_Debug_Console
{
    internal class Program
    {
        public static readonly Commands Commands = new ();

        private static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                string input = ConsoleExtensions.ReadInput();
                Commands.Handle(input);
            }
        }
    }
}
