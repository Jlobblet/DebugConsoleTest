namespace Barotrauma_Debug_Console
{
    public class BoolParser : IParser
    {
        public bool TryParse(string input, out object output)
        {
            bool r = bool.TryParse(input, out bool b);
            output = b;
            return r;
        }
    }
}
