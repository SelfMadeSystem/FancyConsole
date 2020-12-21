using System;

namespace FancyConsoleTest.Utils {
    public class ConsoleUtils {
        private static TermType _consoleColorSupportType = null;
        public static int Width => Console.BufferWidth;
        public static int Height => Console.BufferHeight;

        public static void SetConsoleLine(char c = ' ', int start = 0, int end = 0) {
            var currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(start, Console.CursorTop);
            Console.Write(new string(c, Console.WindowWidth - start - end));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static TermType GetTermType() {
            if (_consoleColorSupportType == null) SetTermType();
            return _consoleColorSupportType;
        }

        private static void SetTermType() { //todo IDE check, especially rider
            _consoleColorSupportType = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WT_SESSION")) ?
               string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TERM")) ?
               TermType.WIN_CMD : TermType.LINUX :
               TermType.WIN_WT;
        }

        public static sbyte OBugPotential = -1;
        public static sbyte OColorSupport = -1;
        public static sbyte OMinimal = -1;
        public static sbyte GetBugPotential() {
            return OBugPotential < 0 ? GetTermType().BugPotential : OBugPotential;
        }
        public static sbyte GetColorSupport() {
            return OColorSupport < 0 ? GetTermType().ColorSupport : OColorSupport;
        }
        public static bool GetMinimal() {
            return OMinimal < 0 ? GetTermType().Minimal : OMinimal > 0;
        }


        public class TermType {
            private static string GetTERM() {
                return Environment.GetEnvironmentVariable("TERM");
            }
            public static readonly TermType UNKNOWN = new TermType(3, 0, "Unknown");
            public static readonly TermType LINUX = new TermType(0, 2, () => GetTERM());
            public static readonly TermType WIN_CMD = new TermType(2, 1, "CMD", true);
            public static readonly TermType WIN_WT = new TermType(1, 2, "Windows Terminal");

            // 0 none
            // 1 minor visuals
            // 2 major visuals
            // 3 unknown
            public readonly sbyte BugPotential;
            // 0 none
            // 1 ConsoleColor only
            // 2 ANSI
            public readonly sbyte ColorSupport;
            public readonly bool Minimal;
            public readonly Func<string> Name;

            public TermType(sbyte bugPotential, sbyte colorSupport, string name, bool minimal=false) {
                BugPotential = bugPotential;
                ColorSupport = colorSupport;
                Name = () => name;
                Minimal = minimal;
            }

            public TermType(sbyte bugPotential, sbyte colorSupport, Func<string> name, bool minimal = false) {
                BugPotential = bugPotential;
                ColorSupport = colorSupport;
                Name = name;
                Minimal = minimal;
            }
        }
    }
}