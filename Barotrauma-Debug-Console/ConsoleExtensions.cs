#nullable enable
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
            var lengthToClear = 0;
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Backspace when (key.Modifiers & ConsoleModifiers.Control) != 0 ||
                                                   (key.Modifiers & ConsoleModifiers.Alt) != 0:
                        if (!string.IsNullOrEmpty(input))
                        {
                            int index = input.LastIndexOf(' ');
                            if (index == -1)
                            {
                                Backspace(input.Length);
                                lengthToClear += input.Length;
                                input = "";
                            }
                            else
                            {
                                Backspace(input.Length - index);
                                lengthToClear += input.Length - index;
                                input = input.Substring(0, index);
                            }
                        }

                        break;
                    case ConsoleKey.Backspace:
                        if (!string.IsNullOrEmpty(input))
                        {
                            input = input.Substring(0, input.Length - 1);
                            Backspace();
                            lengthToClear += 2;
                        }

                        break;
                    case ConsoleKey.Tab:
                        if (!string.IsNullOrEmpty(input) && !input.Contains(' '))
                        {
                            if (LookupCommand(input, out string output))
                            {
                                Console.Write(output[input.Length..]);
                                input = output;
                            }
                        }
                        else if (!string.IsNullOrEmpty(input))
                        {
                            string[]? split = CommandHandler.SplitCommand(input);
                        }

                        break;
                    default:
                        input += key.KeyChar;
                        Console.Write(key.KeyChar);
                        break;
                }

                ForwardBackspace(Math.Max(0, lengthToClear - 1));
                lengthToClear = 0;

                if (!string.IsNullOrEmpty(input) && !input.Contains(' '))
                    if (LookupCommand(input, out string output))
                    {
                        ConsoleColor original = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(output[input.Length..]);
                        Console.Write(new string('\b', output.Length - input.Length));
                        Console.ForegroundColor = original;
                        lengthToClear = output.Length - input.Length;
                    }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine("");
            return input;
        }

        public static void Backspace(int n = 1)
        {
            if (n < 1) return;
            Console.Write(string.Concat(Enumerable.Repeat("\b \b", n)));
        }

        public static void ForwardBackspace(int n = 1)
        {
            if (n < 1) return;
            Console.Write(new string(' ', n));
            Console.Write(new string('\b', n));
        }

        private static bool LookupCommand(string input, out string output)
        {
            if (Program.Handler.SearchCommand(input, out Command command))
            {
                output = command.Name.StartsWith(input, StringComparison.InvariantCultureIgnoreCase)
                             ? command.Name
                             : command.Aliases.First(a => a.StartsWith(input,
                                                                       StringComparison
                                                                           .InvariantCultureIgnoreCase))!;
                return true;
            }

            output = "";
            return false;
        }
    }
}
