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
        
        #region ++ / --

        public const string Build = "build";

        public const string Up = "up";

        public const string Knock = "knock";

        public const string Down = "down";

        #endregion

        public const string And = "and";

        public const string Not = "not";

        #region Comparsion

        public const string Isnt = "isn't";

        public const string Aint = "ain't";

        public const string Higher = "higher";

        public const string Greater = "greater";

        public const string Bigger = "bigger";

        public const string Stronger = "stronger";

        public const string Lower = "lower";

        public const string Less = "less";

        public const string Smaller = "smaller";

        public const string Weaker = "weaker";

        public const string As = "as";

        public const string Than = "than";

        public const string High = "high";

        public const string Great = "great";

        public const string Big = "big";

        public const string Strong = "strong";

        public const string Low = "low";

        public const string Little = "little";

        public const string Small = "small";

        public const string Weak = "weak";

        #endregion

        public const string Is = "is";

        #region Assigment

        public const string Let = "let";

        public const string Be = "be";


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

        public const string Give = "give";

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

        public const string Break = "break";

        public const string Continue = "continue";

        public const string Take = "take";

        #endregion

        public const string It = "it";

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