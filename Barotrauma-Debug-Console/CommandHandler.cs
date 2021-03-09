#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Barotrauma_Debug_Console
{
    public class CommandHandler
    {
        public List<Command> Commands { get; }
        
        public CommandHandler(params Type[] types)
        {
            Commands = new List<Command>();
            foreach (Type t in types)
            {
                foreach (MethodInfo info in t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    if (!Attribute.IsDefined(info, typeof(CommandAttribute))) continue;
                    var attr = info.GetCustomAttribute<CommandAttribute>();
                    Commands.Add(new Command(info, attr));
                }
            }
        }

        /// <summary>
        /// Copied from the Barotrauma codebase
        /// </summary>
        /// <param name="command">The input string to split into parts</param>
        /// <returns>Array of string split on spaces, except when in speech marks</returns>
        private static string[] SplitCommand(string command)
        {
            command = command.Trim();
            List<string> commands = new();
            var escape = 0;
            var inQuotes = false;
            string piece = "";
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
            Command? command = Commands.Find(c => c.Name.Equals(name,
                                                                StringComparison.InvariantCultureIgnoreCase)) ??
                               Commands.Find(c => c.Aliases.Any(n => n.Equals(name,
                                                                              StringComparison
                                                                                  .InvariantCultureIgnoreCase)));
            if (command is null)
            {
                Console.WriteLine("Could not find command.");
                return;
            }

            if (!command.TryRun(split[1..]))
            {
                Console.WriteLine("Failed to run command");
            }
        }
    }
}
