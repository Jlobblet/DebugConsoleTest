using System;
using System.Collections.Generic;

namespace Barotrauma_Debug_Console.TabCompletion
{
    public class Completers
    {
        private static readonly Dictionary<Type, ICompleter> CompleterDictionary = new()
        {
            {typeof(bool), new BoolCompleter()}
        };
        
        public static bool TryGetCompleter(Type t, out ICompleter completer)
        {
            if (CompleterDictionary.TryGetValue(t, out ICompleter c))
            {
                completer = c;
                return true;
            }
            if (typeof(Enum).IsAssignableFrom(t))
            {
                completer = new EnumCompleter(t);
                // Cache this enum parser
                CompleterDictionary.Add(t, completer);
                return true;
            }
            completer = null;
            return false;
        }
    }
}
