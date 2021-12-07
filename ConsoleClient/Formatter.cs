using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaConsole {
    public static class Formatter {

        /// <summary> Aligns the text within the body width </summary>
        /// <returns> Aligned text </returns>
        public static string AlignText(this string input, int bodyWidth, AlignmentType alignType) {
            int Spaces;
            if (alignType == AlignmentType.Centre) Spaces = (int)Math.Floor((double)(bodyWidth - input.Length) / 2);
            else Spaces = bodyWidth - input.Length;
            return $"{RepeatString(" ", Spaces)}{input}";
        }

        public static string GenerateTitle(this string title, int bodyWidth, TitleFanciness fanciness, AlignmentType alignment) {
            string arg = RepeatString("=", bodyWidth);
            if (fanciness == TitleFanciness.Fancy) arg += $"\n{RepeatString("=", bodyWidth)}";
            arg += $"\n{title.AlignText(bodyWidth, alignment)}\n";
            arg += RepeatString("=", bodyWidth);
            if (fanciness == TitleFanciness.Fancy) arg += $"\n{RepeatString("=", bodyWidth)}";
            arg += "\n\n";
            return arg;
        }

        public static string RepeatString(this string op, int count) {
            string arg = "";
            for (int i = 0; i < count; i++) {
                arg += op;
            }
            return arg;
        }

        public static string FillWithWhitespace(this string text) {
            string arg = text;
            arg += RepeatString(" ", Console.WindowWidth - text.Length);
            return arg;
        }
    }
}
