using FancyConsoleTest.New.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FancyConsoleTest.New.Minimal {
    public class TabConsole {
        private static readonly char[] Whitespace = new[] { ' ', '\t', '\n', '\u200b', '|'};
        public static TabConsole Instance = new TabConsole();
        public List<string> history = new List<string>();
        public int historyIndex;
        public string savedLine = "";
        public bool hinting;
        public List<string> hints;
        public string currentArg = "";
        public string line = "";
        public int cursor;
        public ConsoleKeyInfo Key;

        public void StartInputting(string[] args) {
            while (true) {
                Key = Console.ReadKey(true);
                switch (Key.Key) {
                    case ConsoleKey.Tab:
                        GuiApp.Log(new FancyText("UwU", FancyColor.Aqua));
                        break;
                    case ConsoleKey.Enter:
                        Console.Write("\n");
                        history.Insert(0, line);
                        GuiApp.LineRed(line);
                        Reset();
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Key.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                            AddCursorPos(-GetSkipAmount(-1, true));
                        } else {
                            AddCursorPos(-1);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Key.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                            AddCursorPos(GetSkipAmount(1, false));
                        } else {
                            AddCursorPos(1);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        History(1);
                        break;
                    case ConsoleKey.DownArrow:
                        History(-1);
                        break;
                    case ConsoleKey.Backspace:
                        if (Key.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                            var amount = GetSkipAmount(-1, true);
                            if (amount > 0) line = line.Remove(cursor - amount, amount);
                            AddCursorPos(-amount);
                        } else {
                            if (cursor > 0) line = line.Remove(cursor - 1, 1);
                            AddCursorPos(-1);
                        }
                        break;
                    case ConsoleKey.Delete:
                        if (Key.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                            var amount = GetSkipAmount(1, false);
                            if (amount > 0 && cursor < line.Length) line = line.Remove(cursor, amount);
                        } else {
                            if (cursor < line.Length) line = line.Remove(cursor, 1);
                        }
                        break;
                    default:
                        HandleKey();
                        break;
                }
                SetCursorPos();
            }
        }

        private void HandleKey() {
            if (Key.KeyChar <= 31) return;
            line = line.Insert(cursor, Key.KeyChar.ToString());
            AddCursorPos(1);
        }

        private void Reset() {
            hinting = false;
            currentArg = line = savedLine = "";
            cursor = 0;
            historyIndex = -1;
        }

        private void SetCursorPos() {
            Console.CursorVisible = false;
            AddCursorPos();
            Console.SetCursorPosition(0, Console.CursorTop);
            var printLine = line;
            Console.Write(printLine + new string(' ', ConsoleUtils.Width - printLine.Length - 2));
            Console.SetCursorPosition(cursor, Console.CursorTop);
            Console.CursorVisible = true;
        }

        private void AddCursorPos(int i = 0) {
            cursor += i;
            if (cursor < 0) cursor = 0;
            else if (cursor > line.Length) cursor = line.Length;
        }

        private int GetSkipAmount(int dir, bool skipLastWhitespace) {
            var skipped = 0;
            var index = cursor;
            var inWhitespace = cursor > line.Length || cursor <= 0 || Whitespace.Contains(line[cursor - 1]);
            while ((index += dir) >= 0 && (index < line.Length)) {
                skipped++;
                if (inWhitespace) inWhitespace = Whitespace.Contains(line[index]);
                if (!inWhitespace)
                    if (Whitespace.Contains(line[index])) {
                        if (skipLastWhitespace) skipped--;
                        break;
                    }
            }
            if (index >= line.Length) skipped++;
            return skipped;
        }

        private void History(int i) {
            if (historyIndex < 0) savedLine = line;
            historyIndex += i;
            if (historyIndex < -1) historyIndex = -1;
            else if (historyIndex >= history.Count) historyIndex = history.Count - 1;
            if (historyIndex < 0) line = savedLine;
            else line = history[historyIndex];
            cursor = line.Length;
        }

        public void Log(FancyText text) {
            Console.CursorVisible = false;
            var top = Console.CursorTop;
            Console.SetCursorPosition(0, top);
            Console.Write(new string(' ', ConsoleUtils.Width));
            Console.SetCursorPosition(0, top);
            text.SetNext(new FancyText("\n", FancyColor.Reset));
            text.PrintNext(GuiApp.ConsoleColors);
            SetCursorPos();
        }
    }
}
