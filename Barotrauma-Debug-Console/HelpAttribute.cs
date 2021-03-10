using System;

namespace Barotrauma_Debug_Console
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public class HelpAttribute : Attribute
    {
        public readonly string Description;

        public HelpAttribute(string description)
        {
            Description = description;
        }
    }
}
