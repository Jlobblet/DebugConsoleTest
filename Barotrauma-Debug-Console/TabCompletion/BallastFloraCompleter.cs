using System;
using System.Linq;

namespace Barotrauma_Debug_Console.TabCompletion
{
    internal static class BallastFloraPrefab
    {
        public static readonly string[] Prefabs = {"ballastflora", "hubismal", "clown"};
    }

    public class BallastFloraCompleter : ICompleter
    {
        private readonly string[] identifiers = BallastFloraPrefab.Prefabs.Select(bfp => bfp).Distinct().ToArray();

        public bool TryComplete(string input, out string output)
        {
            output = identifiers.FirstOrDefault(i => i.StartsWith(input, StringComparison.OrdinalIgnoreCase));
            return output is not null;
        }
    }
}
