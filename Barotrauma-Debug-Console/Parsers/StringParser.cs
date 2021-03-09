namespace Barotrauma_Debug_Console
{
    public class StringParser : IParser
    {
        public bool TryParse(string input, out object output)
        {
            output = input;
            return true;
        }
    }
}
