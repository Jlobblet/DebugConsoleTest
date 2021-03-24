#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Barotrauma_Debug_Console.TabCompletion;

// ReSharper disable UnusedMember.Global

namespace Barotrauma_Debug_Console
{
    public class Commands
    {
        public enum BarotraumaDeveloper
        {
            Hex,
            Juan,
            Regalis
        }

        public enum MyEnum
        {
            One,
            Two
        }

        public Commands()
        {
            CommandList = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                   .Where(m => Attribute.IsDefined(m, typeof(CommandAttribute)))
                                   .Select(m => new Command(m, m.GetCustomAttribute<CommandAttribute>()!))
                                   .ToList();
        }

        public List<Command> CommandList { get; }

        /// <summary>
        ///     Copied from the Barotrauma codebase
        /// </summary>
        /// <param name="command">The input string to split into parts</param>
        /// <returns>Array of string split on spaces, except when in speech marks</returns>
        public static string[] SplitCommand(string command)
        {
            command = command.Trim();
            List<string> commands = new();
            var escape = 0;
            var inQuotes = false;
            var piece = "";
            foreach (char t in command)
            {
                switch (t)
                {
                    case '\\' when escape == 0:
                        escape = 2;
                        break;
                    case '\\':
                        piece += '\\';
                        break;
                    case '"' when escape == 0:
                        inQuotes = !inQuotes;
                        break;
                    case '"':
                        piece += '"';
                        break;
                    case ' ' when !inQuotes:
                    {
                        if (!string.IsNullOrWhiteSpace(piece)) commands.Add(piece);
                        piece = "";
                        break;
                    }
                    default:
                    {
                        if (escape == 0) piece += t;
                        break;
                    }
                }

                if (escape > 0) escape--;
            }

            if (!string.IsNullOrWhiteSpace(piece)) commands.Add(piece); //add final piece
            return commands.ToArray();
        }

        public void Handle(string input)
        {
            string[] split = SplitCommand(input);
            string name = split[0];
            if (!TryFindCommand(name, out Command command))
            {
                Console.WriteLine("Could not find command.");
                return;
            }

            if (!TryRunCommand(command, split[1..])) Console.WriteLine("Failed to run command");
        }

        public bool TryRunCommand(Command command, params string[] parameters)
        {
            ParameterInfo[] methodParameters = command.Info.GetParameters();
            int min = methodParameters.Count(p => !p.IsOptional),
                max = methodParameters.Length;
            if (parameters.Length < min)
            {
                Console.WriteLine("Not enough parameters given");
                return false;
            }

            if (parameters.Length > max)
            {
                Console.WriteLine("Too many parameters given");
                return false;
            }

            var objects = new object[methodParameters.Length];

            var i = 0;

            // Required parameters
            for (; i < parameters.Length; i++)
            {
                ParameterInfo param = methodParameters[i];
                string input = parameters[i];
                if (!Parsers.TryGetParser(param.ParameterType, out IParser parser))
                {
                    Console.WriteLine($"Could not find parser for {param.ParameterType}");
                    return false;
                }

                if (!parser.TryParse(input, out object output))
                {
                    Console.WriteLine($"Could not parse \"{input}\" as {param.ParameterType}");
                    return false;
                }

                objects[i] = output;
            }

            // Optional parameters
            for (; i < methodParameters.Length; i++) objects[i] = methodParameters[i].DefaultValue!;

            command.Info.Invoke(this, objects);

            return true;
        }

        public bool TryFindCommand(string name, out Command output)
        {
            output = CommandList.Find(c => c.Name.Equals(name,
                                                         StringComparison.OrdinalIgnoreCase)) ??
                     CommandList.Find(c => c.Aliases.Any(n => n.Equals(name,
                                                                       StringComparison
                                                                           .OrdinalIgnoreCase)))!;
            return output is not null!;
        }

        public bool SearchCommand(string search, out Command output)
        {
            output = CommandList.Find(c => c.Name.StartsWith(search, StringComparison.OrdinalIgnoreCase)) ??
                     CommandList.Find(c => c.Aliases.Any(n => n.StartsWith(search, StringComparison.OrdinalIgnoreCase)))
                         !;
            return output is not null!;
        }

        [Command(aliases: "?")]
        [Help("Display information about all commands")]
        public void Help(
            [CustomCompleter(typeof(HelpCompleter))] [Help("The name of a command to retrieve help on")]
            string command = "")
        {
            if (string.IsNullOrEmpty(command))
            {
                IEnumerable<string>? helpStrings = CommandList.Select(c => c.BriefHelpString);
                Console.WriteLine(string.Join('\n', helpStrings));
            }
            else
            {
                if (!TryFindCommand(command, out Command com)) return;
                Console.WriteLine(com.BriefHelpString);
                Console.WriteLine(com.HelpString);
            }
        }

        [Command(aliases: "quit")]
        [Help("Exit the program with the specified exit code")]
        public void Exit([Help("The exit code to exit with")] int exitCode = 0)
        {
            Environment.Exit(exitCode);
        }

        [Command]
        public void Foobar(string input)
        {
            Console.WriteLine(input);
        }

        [Command]
        public void Double(int input)
        {
            Console.WriteLine(input * 2);
        }

        [Command]
        public void Add(int a, int b)
        {
            Console.WriteLine(a + b);
        }

        [Command]
        public void EnumParse(MyEnum e)
        {
            Console.WriteLine(e);
        }

        [Command]
        public void AddFloat(float l, float r)
        {
            Console.WriteLine(l + r);
        }

        [Command(aliases: "mdoub")]
        public void MaybeDouble(int i, bool d = false)
        {
            Console.WriteLine(i * (d ? 2 : 1));
        }

        [Command]
        public void BaroDev(BarotraumaDeveloper dev)
        {
            Console.WriteLine($"Hi {dev}!");
        }

        [Command]
        public void BallastFlora([CustomCompleter(typeof(BallastFloraCompleter))]
                                 string species)
        {
            Console.WriteLine($"That ballast flora species is also known as {species}.");
        }
    }
}
