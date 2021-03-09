using System;
// ReSharper disable UnusedMember.Global

namespace Barotrauma_Debug_Console
{
    public static class Commands
    {
        [Command]
        public static void Exit()
        {
            Environment.Exit(0);
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
    }
}
