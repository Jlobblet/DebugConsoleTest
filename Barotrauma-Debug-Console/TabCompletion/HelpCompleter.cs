using System;
using System.Linq;

namespace Barotrauma_Debug_Console.TabCompletion
{
    public class HelpCompleter : ICompleter
    {
        private readonly string[] aliases = Program.Commands.CommandList.SelectMany(c => c.Aliases).ToArray();
        private readonly string[] commands = Program.Commands.CommandList.Select(c => c.Name).ToArray();

        public bool TryComplete(string input, out string output)
        {
            output = commands.FirstOrDefault(i => i.StartsWith(input, StringComparison.OrdinalIgnoreCase)) ??
                     aliases.FirstOrDefault(i => i.StartsWith(input, StringComparison.OrdinalIgnoreCase));
            return output is not null;
        }
    }
}
