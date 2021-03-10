using System;
using System.Linq;

namespace Barotrauma_Debug_Console.TabCompletion
{
    public class HelpCompleter : ICompleter
    {
        private readonly string[] commands = Program.Handler.Commands.Select(c => c.Name).ToArray();
        public bool TryComplete(string input, out string output)
        {
            output = commands.FirstOrDefault(i => i.StartsWith(input, StringComparison.OrdinalIgnoreCase));
            return output is not null;
        }

    }
}
