using System;
using System.Linq;

namespace Barotrauma_Debug_Console.TabCompletion
{
    public class HelpCompleter : ICompleter
    {
        private readonly string[] commands = Program.Handler.Commands.Select(c => c.Name).ToArray();
        private readonly string[] aliases = Program.Handler.Commands.SelectMany(c => c.Aliases).ToArray();

        public bool TryComplete(string input, out string output)
        {
            output = commands.FirstOrDefault(i => i.StartsWith(input, StringComparison.OrdinalIgnoreCase)) ??
                     aliases.FirstOrDefault(i => i.StartsWith(input, StringComparison.OrdinalIgnoreCase));
            return output is not null;
        }
    }
}
