namespace Barotrauma_Debug_Console
{
    public class FloatParser : IParser
    {
        public bool TryParse(string input, out object output)
        {
            bool r = float.TryParse(input, out float f);
            output = f;
            return r;
        }
    }
}
