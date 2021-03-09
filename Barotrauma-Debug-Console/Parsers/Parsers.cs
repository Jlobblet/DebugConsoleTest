using System;
using System.Collections.Generic;

namespace Barotrauma_Debug_Console
{
    public static class Parsers
    {
        private static readonly Dictionary<Type, IParser> ParserDictionary =
            new()
            {
                {typeof(string), new StringParser()},
                {typeof(int), new IntParser()},
                {typeof(float), new FloatParser()},
                {typeof(bool), new BoolParser()}
            };

        public static bool TryGetParser(Type t, out IParser parser)
        {
            if (ParserDictionary.TryGetValue(t, out IParser p))
            {
                parser = p;
                return true;
            }

            if (typeof(Enum).IsAssignableFrom(t))
            {
                parser = new EnumParser(t);
                // Cache this enum parser
                ParserDictionary.Add(t, parser);
                return true;
            }

            parser = null;
            return false;
        }
    }
}
