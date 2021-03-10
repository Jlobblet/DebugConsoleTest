#nullable enable
using System;

namespace Barotrauma_Debug_Console
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public readonly string? Name = null;
        public readonly string[] Aliases = Array.Empty<string>();

        public CommandAttribute()
        {
            
        }

        public CommandAttribute(string name)
        {
            Name = name;
        }

        public CommandAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }

        public CommandAttribute(string name, params string[] aliases)
        {
            Name = name;
            Aliases = aliases;
        }
    }
}
