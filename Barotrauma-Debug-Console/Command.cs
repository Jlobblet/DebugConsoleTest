#nullable enable
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Barotrauma_Debug_Console
{
    public class Command
    {
        public readonly string[] Aliases;
        public readonly MethodInfo Info;
        public readonly string Name;
        public readonly ParameterInfo[] ParameterInfos;

        public string BriefHelpString;
        public string HelpString;

        public Command(MethodInfo info, CommandAttribute attr)
        {
            Name = attr.Name ?? info.Name;
            Aliases = attr.Aliases;
            this.Info = info;
            ParameterInfos = info.GetParameters();
            var briefHelp = new StringBuilder();
            var help = new StringBuilder();
            briefHelp.Append($"{Name}");
            if (Aliases.Any())
                foreach (string alias in Aliases)
                    briefHelp.Append($"/{alias}");
            foreach (ParameterInfo parameterInfo in ParameterInfos)
            {
                char openingBracket = parameterInfo.IsOptional ? '[' : '(';
                char closingBracket = parameterInfo.IsOptional ? ']' : ')';
                briefHelp.Append($" {openingBracket}{parameterInfo.Name}{closingBracket}");
                var helpAttribute = (HelpAttribute?) parameterInfo.GetCustomAttribute(typeof(HelpAttribute));
                string paramDescription = "no help provided";
                if (helpAttribute is not null) paramDescription = helpAttribute.Description;
                help.AppendLine($"{parameterInfo.ParameterType.Name} {parameterInfo.Name}: {paramDescription}");
            }

            BriefHelpString = briefHelp.ToString();
            HelpString = help.ToString();
        }
    }
}
