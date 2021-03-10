using System;
using System.Linq;
using System.Reflection;

namespace Barotrauma_Debug_Console
{
    public class Command
    {
        private readonly MethodInfo info;
        public readonly string Name;
        public readonly string[] Aliases;
        public readonly ParameterInfo[] ParameterInfos;

        public Command(MethodInfo info, CommandAttribute attr)
        {
            Name = attr.Name ?? info.Name;
            Aliases = attr.Aliases;
            this.info = info;
            ParameterInfos = info.GetParameters();
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
            for (; i < methodParameters.Length; i++)
            {
                objects[i] = methodParameters[i].DefaultValue;
            }

            info.Invoke(null, objects);

            return true;
        }
    }
}
