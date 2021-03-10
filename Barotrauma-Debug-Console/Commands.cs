using System;
using System.Collections.Generic;
using System.Linq;
using Barotrauma_Debug_Console.TabCompletion;

// ReSharper disable UnusedMember.Global

namespace Barotrauma_Debug_Console
{
    public static class Commands
    {
        public enum BarotraumaDeveloper
        {
            Hex,
            Juan,
            Regalis
        }

        public enum MyEnum
        {
            One,
            Two
        }

        [Command]
        [Help("Display information about all commands")]
        public static void Help(
            [CustomCompleter(typeof(HelpCompleter))] [Help("The name of a command to retrieve help on")]
            string command = "")
        {
            if (string.IsNullOrEmpty(command))
            {
                IEnumerable<string> commands = Program.Handler.Commands.Select(c => c.BriefHelpString);
                Console.WriteLine(string.Join('\n', commands));
            }
            else
            {
                if (!Program.Handler.TryFindCommand(command, out Command com)) return;
                Console.WriteLine(com.BriefHelpString);
                Console.WriteLine(com.HelpString);
            }
        }

        [Command(aliases: "quit")]
        [Help("Exit the program with the specified exit code")]
        public static void Exit([Help("The exit code to exit with")] int exitCode = 0)
        {
            Environment.Exit(exitCode);
        }

        [Command]
        public static void Foobar(string input)
        {
            Console.WriteLine(input);
        }

        [Command]
        public static void Double(int input)
        {
            Console.WriteLine(input * 2);
        }

        [Command]
        public static void Add(int a, int b)
        {
            Console.WriteLine(a + b);
        }

        [Command]
        public static void EnumParse(MyEnum e)
        {
            Console.WriteLine(e);
        }

        [Command]
        public static void AddFloat(float l, float r)
        {
            Console.WriteLine(l + r);
        }

        [Command(aliases: "mdoub")]
        public static void MaybeDouble(int i, bool d = false)
        {
            Console.WriteLine(i * (d ? 2 : 1));
        }

        [Command]
        public static void BaroDev(BarotraumaDeveloper dev)
        {
            Console.WriteLine($"Hi {dev}!");
        }

        [Command]
        public static void BallastFlora([CustomCompleter(typeof(BallastFloraCompleter))]
                                        string species)
        {
            Console.WriteLine($"That ballast flora species is also known as {species}.");
        }
    }
}
