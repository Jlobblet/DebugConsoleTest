using System;

namespace Barotrauma_Debug_Console.TabCompletion
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CustomCompleterAttribute : Attribute
    {
        public readonly ICompleter Completer;

        public CustomCompleterAttribute(Type t)
        {
            Completer = (ICompleter) Activator.CreateInstance(t);
        }
    }
}
