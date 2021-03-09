namespace Barotrauma_Debug_Console
{
    public interface IParser
    {
        public bool TryParse(string input, out object output);
    }
}
