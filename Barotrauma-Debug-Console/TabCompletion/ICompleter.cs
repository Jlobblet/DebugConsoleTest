namespace Barotrauma_Debug_Console.TabCompletion
{
    public interface ICompleter
    {
        bool TryComplete(string input, out string output);
    }
}
