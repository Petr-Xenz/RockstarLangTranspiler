using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RockstarLangTranspiler
{
    public static class KeyWords
    {
        #region Literals

        public const string Null = "null";

        public const string Nothing = "nothing";

        public const string Nowhere = "nowhere";

        public const string Nobody = "nobody";

        public const string Empty = "empty";

        public const string Gone = "gone";

        public const string Undefined = "mysterious";

        public const string True = "true";

        public const string Ok = "ok";

        public const string Right = "right";

        public const string Yes = "yes";

        public const string False = "false";

        public const string Wrong = "wrong";

        public const string No = "no";

        public const string Lies = "lies";

        #endregion

        #region Mathematical operations

        public const string Plus = "plus";

        public const string With = "with";

        public const string Minus = "minus";

        public const string Without = "without";

        public const string Times = "times";

        public const string Of = "of";

        public const string Over = "over";

        #endregion

        public const string And = "and";

        #region Assigment

        public const string Let = "let";

        public const string Be = "be";

        public const string Is = "is";

        public const string Are = "are";

        public const string Was = "was";

        public const string Were = "were";

        public const string Put = "put";

        public const string Into = "into";
        #endregion

        #region Condition

        public const string If = "if";

        public const string Else = "else";

        #endregion

        #region Functions

        public const string Ampersand = "&";

        public const string N = "'n'";

        public const string Takes = "takes";

        public const string Taking = "taking";

        public const string Gives = "gives";

        public const string Back = "back";

        #endregion

        #region Output

        public const string Say = "say";

        public const string Whisper = "whisper";

        public const string Shout = "shout";

        public const string Scream = "scream";

        #endregion

        #region Cycle

        public const string While = "while";

        public const string Until = "until";

        #endregion

        #region Proper variable prefixes

        public const string A = "a";

        public const string An = "an";

        public const string The = "the";

        public const string My = "my";

        public const string Your = "your";

        #endregion

        public static IEnumerable<string> AllReservedWords { get; } =
            typeof(KeyWords).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.GetValue(null))
            .Cast<string>()
            .ToList();
    }
}