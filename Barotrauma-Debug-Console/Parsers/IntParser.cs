namespace Barotrauma_Debug_Console
{
    public class IntParser : IParser
    {
        public bool TryParse(string input, out object output)
        {
            bool r = int.TryParse(input, out int i);
            output = i;
            return r;
        }
    }
}
