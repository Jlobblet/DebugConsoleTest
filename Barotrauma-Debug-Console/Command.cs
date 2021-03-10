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
        private readonly MethodInfo info;
        public readonly string Name;
        public readonly ParameterInfo[] ParameterInfos;

        public string BriefHelpString;
        public string HelpString;

        public Command(MethodInfo info, CommandAttribute attr)
        {
            Name = attr.Name ?? info.Name;
            Aliases = attr.Aliases;
            this.info = info;
            ParameterInfos = info.GetParameters();
            var briefHelp = new StringBuilder();
            var help = new StringBuilder();
            briefHelp.Append($"{Name}");
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

        public bool TryRun(params string[] parameters)
        {
            ParameterInfo[] methodParameters = info.GetParameters();
            int min = methodParameters.Count(p => !p.IsOptional),
                max = methodParameters.Length;
            if (parameters.Length < min)
            {
                Console.WriteLine("Not enough parameters given");
                return false;
            }

            if (parameters.Length > max)
            {
                Console.WriteLine("Too many parameters given");
                return false;
            }

            var objects = new object[methodParameters.Length];

            var i = 0;

            // Required parameters
            for (; i < parameters.Length; i++)
            {
                ParameterInfo param = methodParameters[i];
                string input = parameters[i];
                if (!Parsers.TryGetParser(param.ParameterType, out IParser parser))
                {
                    Console.WriteLine($"Could not find parser for {param.ParameterType}");
                    return false;
                }

                if (!parser.TryParse(input, out object output))
                {
                    Console.WriteLine($"Could not parse \"{input}\" as {param.ParameterType}");
                    return false;
                }

                objects[i] = output;
            }

            // Optional parameters
            for (; i < methodParameters.Length; i++) objects[i] = methodParameters[i].DefaultValue;

            info.Invoke(null, objects);

            return true;
        }
    }
}
