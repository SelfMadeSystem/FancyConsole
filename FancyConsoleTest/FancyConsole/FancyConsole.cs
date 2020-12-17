using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FancyConsoleTest.Utils;

namespace FancyConsoleTest.FancyConsole
{
    public class FancyConsole
    {
        public static bool Debug = false;
        internal static PrintManager Pm;
        internal static InputManager Im;
        internal static LogManager Lm;

        public static void Init()
        {
            Pm = new PrintManager();
            Im = new InputManager();
            Lm = new LogManager();
            Pm.Start();
        }

        public static void StartScreen()
        {
            while (true)
            {
                Task.Delay(1000).Wait();
                Pm.DrawLogs();
            }
        }

        // Do this async
        public static void StartInput()
        {
            Im.Start();
        }

        public static void Log(FancyText text)
        {
            Lm.Log(text);
            Pm.DrawLogs();
        }

        public static void RefreshScreen()
        {
            Lm.RefreshLines();
            Pm.RefreshScreen();
        }
    }

    internal class PrintManager
    {
        private static InputManager Im => FancyConsole.Im;
        private static LogManager Lm => FancyConsole.Lm;

        public void Start()
        {
            RefreshScreen();
        }

        public void Reset()
        {
            Console.Write("\x001bc");
        }

        public void RefreshScreen()
        {
            Reset();
            Console.SetCursorPosition(0, MaxLines() - 1);
            Console.ResetColor();
            ConsoleUtils.SetConsoleLine('═');
            DrawLogs();
            DrawInput();
        }

        public void DrawLogs()
        {
            var s = Lm.VisibleLines().Aggregate("", (current, line) => current + line.GetConsoleString() + "\n");
            var i = s.Count(a => a == '\n');
            Console.SetCursorPosition(0, 0);
            do
            {
                Console.Write("\x1b[2K");
                if (i-- > 0) Console.Write("\n");
            } while (i > 0);

            Console.SetCursorPosition(0, 0);
            Console.Write(s);
            Console.SetCursorPosition(Im.Cursor, ConsoleUtils.Height - 1);
        }

        public void DrawInput()
        {
            var tooLong = Im.CurrentInput.Length + Im.CurrentHint.Length - Im.CurrentArg.Length - 1 >
                          ConsoleUtils.Width;
            // var longBy = rawText.Length - ConsoleUtils.Width;
            if (tooLong)
            {
                Console.SetCursorPosition(0, ConsoleUtils.Height - 1);
                Console.Write("~~ Text too long, placeholder, please finish me thx ~~");
            }
            else
            {
                var s = "";
                s += FancyColor.Blue.PrintFunc + Im.InputStrip +
                     FancyColor.Red.PrintFunc + Im.CurrentArg + FancyColor.Gray.PrintFunc +
                     (string.IsNullOrEmpty(Im.CurrentHint) ? "" : Im.CurrentHint.Substring(Im.CurrentArg.Length)) +
                     FancyColor.Reset.PrintFunc +
                     Im.RightInput;
                // s += new string(' ',
                //     ConsoleUtils.Width - Im.CurrentInput.Length + Im.CurrentHint.Length - Im.CurrentArg.Length);
                Console.SetCursorPosition(0, ConsoleUtils.Height - 1);
                Console.Write("\x1b[2K");
                Console.Write(s);
            }

            Console.SetCursorPosition(Im.Cursor, ConsoleUtils.Height - 1);
        }

        public void DrawHints()
        {
            var selB = 0; // beginning
            var selE = 0; // ending
            var a = "";
            var s = "";
            for (var i = 0; i < Im.VisibleHints.Length; i++)
            {
                var vh = Im.VisibleHints[i];
                if (i == Im.HintsIndex)
                {
                    s += FancyColor.Gray.PrintFunc;
                    selB = a.Length;
                    selE = vh.Length;
                }
                else s += FancyColor.Reset.PrintFunc;

                s += vh + " ";
                a += vh + " ";
            }

            s += FancyColor.Reset.PrintFunc;
            if (a.Length >= ConsoleUtils.Width)
            {
                var v = selB - ConsoleUtils.Width / 2;
                var b = v > 0;
                s = a.Substring(b ? v : 0, Math.Min(a.Length - v, ConsoleUtils.Width));
                s = s.Insert(b ? ConsoleUtils.Width / 2 : selB, FancyColor.Gray.PrintFunc);
                s = s.Insert((b ? ConsoleUtils.Width / 2 : selB) + selE + FancyColor.Gray.PrintFunc.Length,
                    FancyColor.Reset.PrintFunc);
            }

            Console.SetCursorPosition(0, ConsoleUtils.Height - 2);
            Console.Write("\x1b[2K");
            Console.Write(s);
            Console.SetCursorPosition(Im.Cursor, ConsoleUtils.Height - 1);
        }

        public static int MaxLines()
        {
            return ConsoleUtils.Height - 2;
        }
    }

    internal class InputManager
    {
        private static PrintManager Pm => FancyConsole.Pm;
        private static LogManager Lm => FancyConsole.Lm;

        public string CurrentInput = "";
        public string LeftInput = "";
        public string RightInput = "";
        public string CurrentArg = "";
        public string InputStrip = "";
        public int Cursor;
        public List<string> History = new List<string>();
        public int HistoryIndex = -1;
        public string SavedInput = "";
        public string[] Hints = new string[0];
        public string[] VisibleHints = new string[0];
        public int HintsIndex;
        public string CurrentHint = "";
        public ConsoleKeyInfo Key;

        public void Start()
        {
            while (true)
            {
                Key = Console.ReadKey(true);
                ParseInput();
            }
        }

        public void ParseInput()
        {
            /*FancyConsole.Log(new FancyText("[Key] ", FancyColor.Green)
            {
                Next = new FancyText(Key.Key + ":" + Key.Modifiers + ":" + Key.Modifiers.HasFlag(ConsoleModifiers.Shift) + ":" + ((int) Key.KeyChar))
            });*/
            switch (Key.Key)
            {
                case ConsoleKey.Tab:
                    if (Hints == null || Hints.Length == 0) GetHints();
                    else if (Key.Modifiers.HasFlag(ConsoleModifiers.Shift)) DecrHint();
                    else IncrHint();

                    break;
                case ConsoleKey.Backspace:
                    if (Cursor > 0)
                    {
                        var reset = LeftInput.Length > 1 && LeftInput[^1] == ' ' || LeftInput.Length == 1;
                        LeftInput = LeftInput.Substring(0, LeftInput.Length - 1);
                        MoveCursor(-1);
                        UpdateInputs();
                        UpdateVisibleHints();
                        if (reset) SpaceReset();
                    }

                    break;
                case ConsoleKey.Delete:
                    if (Cursor < CurrentInput.Length)
                    {
                        RightInput = RightInput.Substring(1, RightInput.Length - 1);
                        MoveCursor(0);
                        UpdateInputs();
                        UpdateVisibleHints();
                    }

                    break;
                case ConsoleKey.LeftArrow:
                    MoveCursor(-1);
                    UpdateLeftRight();
                    if (LeftInput.Length > 0 && LeftInput[^1] == ' ') SpaceReset();
                    else UpdateVisibleHints();
                    UpdateInputs();
                    break;
                case ConsoleKey.RightArrow:
                    MoveCursor(1);
                    UpdateLeftRight();
                    if (LeftInput.Length > 0 && LeftInput[^1] == ' ') SpaceReset();
                    else UpdateVisibleHints();
                    UpdateInputs();
                    break;
                case ConsoleKey.Enter:
                    if (string.IsNullOrEmpty(CurrentHint)) Entered();
                    else SelectHint();
                    break;
                case ConsoleKey.PageUp: //For some reason, shift + arrow = Page
                    Lm.OffsetScroll(Key.Modifiers.HasFlag(ConsoleModifiers.Shift) ? -1 : -PrintManager.MaxLines() + 2);
                    Pm.DrawLogs();
                    break;
                case ConsoleKey.PageDown:
                    Lm.OffsetScroll(Key.Modifiers.HasFlag(ConsoleModifiers.Shift) ? 1 : PrintManager.MaxLines() - 2);
                    Pm.DrawLogs();
                    break;
                case ConsoleKey.UpArrow:
                    OffsetHistory(1);
                    break;
                case ConsoleKey.DownArrow:
                    OffsetHistory(-1);
                    break;
                case ConsoleKey.Home:
                    Lm.Scroll = 0;
                    Pm.DrawLogs();
                    break;
                case ConsoleKey.End:
                    Lm.Scroll = Math.Max(Lm.Lines.Count - PrintManager.MaxLines() + 1, 0);
                    Pm.DrawLogs();
                    break;
                case ConsoleKey.R:
                    if (Key.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        FancyConsole.RefreshScreen();
                        return;
                    }
                    else AddInput(Key.KeyChar);

                    break;
                default:
                    AddInput(Key.KeyChar);
                    break;
            }

            Pm.DrawInput();
            Pm.DrawHints();
        }

        public void Entered()
        {
            if (CurrentInput.ToLower().Equals("exit"))
            {
                FancyConsole.Log(new FancyText("[Exit] ", FancyColor.Red)
                {
                    Next = new FancyText("Shutting down...", FancyColor.Reset)
                });
                Environment.Exit(0);
            }
            else
                FancyConsole.Log(new FancyText("[Entered] ", FancyColor.Green)
                {
                    Next = new FancyText(CurrentInput, FancyColor.Reset)
                });
            History.Insert(0, CurrentInput);
            CurrentInput = "";
            LeftInput = "";
            RightInput = "";
            CurrentArg = "";
            InputStrip = "";
            SavedInput = "";
            Cursor = 0;
            HistoryIndex = -1;
            Hints = new string[0];
            VisibleHints = new string[0];
            HintsIndex = 0;
        }

        public void SpaceReset()
        {
            Hints = new string[0];
            VisibleHints = new string[0];
            CurrentHint = "";
            HintsIndex = 0;
            GetHints();
        }

        public void UpdateVisibleHints()
        {
            var lower = CurrentArg.ToLower();

            if (VisibleHints.Length <= HintsIndex) HintsIndex = 0;

            VisibleHints = Hints.Where(hint => hint.ToLower().StartsWith(lower)).ToArray();
            CurrentHint = VisibleHints.Length > HintsIndex ? VisibleHints[HintsIndex] : "";
        }

        public void IncrHint()
        {
            HintsIndex++;
            if (HintsIndex >= VisibleHints.Length) HintsIndex = 0;
            CurrentHint = VisibleHints.Length > HintsIndex ? VisibleHints[HintsIndex] : "";
        }

        public void DecrHint()
        {
            HintsIndex--;
            if (HintsIndex < 0) HintsIndex = VisibleHints.Length - 1;
            CurrentHint = VisibleHints.Length > HintsIndex ? VisibleHints[HintsIndex] : "";
        }

        public void SelectHint()
        {
            var a = CurrentHint.Substring(CurrentArg.Length) + " ";
            LeftInput += a;
            UpdateInputs();
            MoveCursor(a.Length);
            SpaceReset();
        }

        public void GetHints()
        {
            Hints = GetTabbyThingy.GetHints(LeftInput).ToArray();
            if (Hints.Length == 1) IncrHint();
            UpdateVisibleHints();
        }

        public void OffsetHistory(int offset)
        {
            if (HistoryIndex < 0) SavedInput = CurrentInput;
            HistoryIndex += offset;
            if (HistoryIndex < -1) HistoryIndex = -1;
            else if (HistoryIndex >= History.Count) HistoryIndex = History.Count - 1;
            CurrentInput = HistoryIndex < 0 ? SavedInput : History[HistoryIndex];
            MoveCursor(CurrentInput.Length);
            UpdateLeftRight();
            UpdateInputs();
        }

        public void AddInput(char c)
        {
            if (c <= '\u001F') return;
            LeftInput += c;
            UpdateInputs();
            MoveCursor(1);
            if (c == ' ') SpaceReset();
            else UpdateVisibleHints();
        }

        public void MoveCursor(int offset)
        {
            Cursor += offset;
            if (Cursor < 0) Cursor = 0;
            else if (Cursor > CurrentInput.Length) Cursor = CurrentInput.Length;
        }

        public void UpdateLeftRight()
        {
            LeftInput = CurrentInput.Substring(0, Cursor);
            RightInput = CurrentInput.Substring(Cursor, CurrentInput.Length - Cursor);
        }

        public void UpdateInputs()
        {
            CurrentInput = LeftInput + RightInput;
            CurrentArg = LeftInput.Substring(LeftInput.LastIndexOf(' ') + 1);
            InputStrip = LeftInput.Substring(0, LeftInput.LastIndexOf(' ') + 1);
        }
    }

    internal class LogManager
    {
        private static PrintManager Pm => FancyConsole.Pm;
        private static InputManager Im => FancyConsole.Im;

        public int Scroll;
        public List<FancyText> Logs = new List<FancyText>();
        public List<FancyText> Lines = new List<FancyText>();

        public List<FancyText> VisibleLines()
        {
            var max = Math.Min(Lines.Count, PrintManager.MaxLines() - 1);
            var list = new List<FancyText>();
            for (var i = 0; i < max; i++)
            {
                list.Add(Lines[i + Scroll]);
            }

            return list;
        }

        public void OffsetScroll(int amount)
        {
            Scroll += amount;
            var m = Math.Max(Lines.Count - PrintManager.MaxLines() + 1, 0);
            if (Scroll < 0) Scroll = 0;
            else if (Scroll > m) Scroll = m;
        }

        public void Log(FancyText text)
        {
            Logs.Insert(0, text);
            var lines = text.GetLines();
            Lines.InsertRange(0, lines);
            if (Scroll > 0) Scroll += lines.Count;
        }

        public void RefreshLines()
        {
            Lines.Clear();
            Logs.Reverse();
            foreach (var text in Logs)
            {
                Lines.InsertRange(0, text.GetLines());
            }

            Logs.Reverse();
        }
    }
}