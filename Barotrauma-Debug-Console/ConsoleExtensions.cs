using System;
using System.Linq;

namespace Barotrauma_Debug_Console
{
    public static class ConsoleExtensions
    {
        public static string ReadInput()
        {
            var input = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Backspace when (key.Modifiers & ConsoleModifiers.Control) != 0 || (key.Modifiers & ConsoleModifiers.Alt) != 0:
                        if (!string.IsNullOrEmpty(input))
                        {
                            int index = input.LastIndexOf(' ');
                            if (index == -1)
                            {
                                Console.Write(string.Concat(Enumerable.Repeat("\b \b", input.Length)));
                                input = "";
                            }
                            else
                            {
                                Console.Write(string.Concat(Enumerable.Repeat("\b \b", input.Length - index)));
                                input = input.Substring(0, index);   
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (!string.IsNullOrEmpty(input))
                        {
                            input = input.Substring(0, input.Length - 1);
                            Console.Write("\b \b");
                        }

                        break;
                    default:
                        input += key.KeyChar;
                        Console.Write(key.KeyChar);
                        break;
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return input;
        }
    }
}
