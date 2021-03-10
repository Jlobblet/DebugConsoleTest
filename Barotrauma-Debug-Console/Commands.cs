using System;
using Barotrauma_Debug_Console.TabCompletion;

// ReSharper disable UnusedMember.Global

namespace Barotrauma_Debug_Console
{
    public static class Commands
    {
        [Command(aliases: "quit")]
        public static void Exit(int exitCode = 0)
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

        public enum MyEnum
        {
            One,
            Two,
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

        public enum BarotraumaDeveloper
        {
            Hex,
            Juan,
            Regalis,
        }

        [Command]
        public static void BaroDev(BarotraumaDeveloper dev)
        {
            Console.WriteLine($"Hi {dev}!");
        }

        [Command]
        public static void BallastFlora([CustomCompleter(typeof(BallastFloraCompleter))] string species)
        {
            Console.WriteLine($"That ballast flora species is also known as {species}.");
        }
    }
}
