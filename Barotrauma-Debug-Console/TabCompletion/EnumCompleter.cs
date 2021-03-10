using System;
using System.Linq;

namespace Barotrauma_Debug_Console.TabCompletion
{
    public class EnumCompleter : ICompleter
    {
        private readonly string[] names;
        public EnumCompleter(Type e)
        {
            names = Enum.GetNames(e);
        }

        public bool TryComplete(string input, out string output)
        {
            output = names.FirstOrDefault(n => n.StartsWith(input, StringComparison.InvariantCultureIgnoreCase));
            return output is not null;
        }
    }
}
