using System;

namespace Barotrauma_Debug_Console.TabCompletion
{
    public class BoolCompleter : ICompleter
    {
        public bool TryComplete(string input, out string output)
        {
            if ("true".StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
            {
                output = "true";
                return true;
            }

            if ("false".StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
            {
                output = "false";
                return true;
            }

            output = "";
            return false;
        }
    }
}
