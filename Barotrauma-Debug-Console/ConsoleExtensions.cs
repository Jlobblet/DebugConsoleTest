#nullable enable
using System;
using System.Linq;
using Barotrauma_Debug_Console.TabCompletion;

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
                        if (string.IsNullOrEmpty(input)) break;
                        int index = input.LastIndexOf(' ');
                        if (index == -1)
                        {
                            // Only one word - delete everything
                            Backspace(input.Length);
                            lengthToClear += input.Length;
                            input = "";
                        }
                        else
                        {
                            // Delete last word
                            Backspace(input.Length - index);
                            lengthToClear += input.Length - index;
                            input = input.Substring(0, index);
                        }

                        break;
                    case ConsoleKey.Backspace:
                        // Delete one character
                        if (string.IsNullOrEmpty(input)) break;
                        input = input.Substring(0, input.Length - 1);
                        Backspace();
                        lengthToClear += 1;
                        break;
                    case ConsoleKey.Delete:
                        break;
                    case ConsoleKey.Tab:
                        if (string.IsNullOrEmpty(input)) break;
                        if (!input.Contains(' '))
                        {
                            // We have part of a command name
                            if (!LookupCommand(input, out string output)) break;
                            Console.Write(output[input.Length..]);
                            input = output;
                        }
                        else
                        {
                            // We need to find the next argument
                            string[] split = CommandHandler.SplitCommand(input);
                            if (!Program.Handler.TryFindCommand(split[0], out Command command)) break;

                            Type currentType = command.ParameterTypes[split.Length - 2];
                            if (!Completers.TryGetCompleter(currentType, out ICompleter c)) break;
                            if (!c.TryComplete(split[^1], out string completion)) break;

                            string rest = completion[split[^1].Length..];
                            input += rest;
                            Console.Write(rest);
                        }

                        break;
                    default:
                        input += key.KeyChar;
                        Console.Write(key.KeyChar);
                        break;
                }

                ForwardBackspace(Math.Max(0, lengthToClear));
                lengthToClear = 0;

                if (string.IsNullOrEmpty(input)) continue;
                if (!input.Contains(' '))
                {
                    if (!LookupCommand(input, out string output)) continue;
                    ConsoleColor original = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(output[input.Length..]);
                    Console.Write(new string('\b', output.Length - input.Length));
                    Console.ForegroundColor = original;
                    lengthToClear += output.Length - input.Length;
                }
                else
                {
                    string[] split = CommandHandler.SplitCommand(input);
                    if (!Program.Handler.TryFindCommand(split[0], out Command command)) continue;
                    int index = split.Length - 2;
                    if (index < 0 || index >= command.ParameterTypes.Length) continue;

                    Type currentType = command.ParameterTypes[split.Length - 2];
                    if (!Completers.TryGetCompleter(currentType, out ICompleter c)) continue;
                    if (!c.TryComplete(split[^1], out string completion)) continue;

                    string rest = completion[split[^1].Length..];
                    ConsoleColor original = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(rest);
                    Console.Write(new string('\b', rest.Length));
                    Console.ForegroundColor = original;
                    lengthToClear += rest.Length;
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
                output = command.Name.StartsWith(input, StringComparison.OrdinalIgnoreCase)
                             ? command.Name
                             : command.Aliases.FirstOrDefault(a => a.StartsWith(input,
                                                                                    StringComparison
                                                                                        .OrdinalIgnoreCase))!;
                return output is not null;
            }

            output = "";
            return false;
        }
    }
}
