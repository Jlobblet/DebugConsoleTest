using System;

namespace Barotrauma_Debug_Console
{
    public class EnumParser : IParser
    {
        private readonly Type e;
        public EnumParser(Type e)
        {
            this.e = e;
        }
        public bool TryParse(string input, out object output)
        {
            return Enum.TryParse(e, input, true, out output);
        }
    }
}
