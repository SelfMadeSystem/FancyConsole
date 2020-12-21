using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FancyConsoleTest.Old.Utils;

namespace FancyConsoleTest.Old.FancyConsole {
    public class FancyConsole {
        public static object LockObj = new object();
        public static bool Debug = false;
        internal static PrintManager Pm;
        internal static InputManager Im;
        internal static LogManager Lm;

        private static void ParseArgs(string[] args) {
            var possibleArgs = new[] { "obug", "ocolor", "ominimal" };
            foreach (var arg in args) {
                try {
                    var split = arg.Split("=");
                    if (!possibleArgs.Contains(split[0])) continue;
                    var i = sbyte.Parse(split[1]);
                    if (split.Length == 2) {
                        switch (split[0]) {
                            case "obug":
                                ConsoleUtils.OBugPotential = i;
                                Log(new FancyText("[Info] ", FancyColor.Gray) {
                                    Next = new FancyText($"Overrode bug potential: {i}", FancyColor.Reset)
                                });
                                break;
                            case "ocolor":
                                ConsoleUtils.OColorSupport = i;
                                Log(new FancyText("[Info] ", FancyColor.Gray) {
                                    Next = new FancyText($"Overrode color support: {i}", FancyColor.Reset)
                                });
                                break;
                            case "ominimal":
                                ConsoleUtils.OMinimal = i;
                                Log(new FancyText("[Info] ", FancyColor.Gray) {
                                    Next = new FancyText($"Overrode minimal console: {i > 0}", FancyColor.Reset)
                                });
                                break;
                        }
                    } else throw new ArgumentException($"Invalid argument: {arg}");
                } catch (Exception e) {
                    Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                        Next = new FancyText($"Exception: {e.Message}", FancyColor.Reset)
                    });
                }
            }
        }


        // do this on the main thread
        public static void Init(string[] args) {
            ParseArgs(args);
            Pm = new PrintManager();
            Im = new InputManager();
            Lm = new LogManager();
            Pm.Start();

            ConsoleUtils.TermType type = ConsoleUtils.GetTermType();
            if (type.Minimal) {
                Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                    Next = new FancyText($"You are using {type.Name()}. There is minimal support for {type.Name()}. " +
                    $"A lot of features will be missing. If you are on windows, try using windows terminal instead.", FancyColor.Reset)
                });
                return;
            }

            Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                Next = new FancyText("Resizing of window will cause issues. Press Ctrl-R to refresh screen.", FancyColor.Reset)
            });

            if (type.ColorSupport == 0) {
                Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                    Next = new FancyText($"You are using {type.Name()}. Color is not supported for this.", FancyColor.Reset)
                });
            } else if (type.ColorSupport == 1) {
                Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                    Next = new FancyText($"You are using {type.Name()}. Color is partially supported for this.", FancyColor.Reset)
                });
            }
            switch (type.BugPotential) {
                case 1:
                    Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                        Next = new FancyText($"You are using {type.Name()}. There may be minor visual bugs.", FancyColor.Reset)
                    });
                    break;
                case 2:
                    Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                        Next = new FancyText($"You are using {type.Name()}. There will be bugs.", FancyColor.Reset)
                    });
                    break;
                case 3:
                    Log(new FancyText("[Warning] ", FancyColor.Yellow) {
                        Next = new FancyText($"You are using {type.Name()}. There might be bugs.", FancyColor.Reset)
                    });
                    break;
            }
        }

        public static void StartScreen() {
            if (ConsoleUtils.IsMinimal()) return;
            while (true) {
                Task.Delay(1000).Wait();
                Pm.DrawLogs();
            }
        }

        // Do this async
        public static void StartInput() {
            Im.Start();
        }

        public static void Log(FancyText text) {
            Lm.Log(text);
            Pm.DrawLogs();
        }

        public static void RefreshScreen() {
            Lm.RefreshLines();
            Pm.RefreshScreen();
        }
    }

    public class PrintManager {
        private static InputManager Im => FancyConsole.Im;
        private static LogManager Lm => FancyConsole.Lm;

        public void Start() {
            RefreshScreen();
        }

        public void Reset() {
            if (ConsoleUtils.GetColorSupport() == 2) Console.Write("\x001bc");
            else Console.Clear();
        }

        public void RefreshScreen() {
            Console.CursorVisible = false;
            Reset();
            if (ConsoleUtils.GetBugPotential() == 0) {
                Console.SetCursorPosition(0, MaxLines() - 1);
                Console.ResetColor();
                ConsoleUtils.SetConsoleLine('═');
            }
            DrawLogs();
            DrawInput();
            ResetCursor();
        }

        public void DrawLogs() {
            Console.CursorVisible = false;
            var s = Lm.VisibleLines().Aggregate("", (current, line) => current + line.GetConsoleString() + "\n");
            var i = s.Count(a => a == '\n');
            Console.SetCursorPosition(0, 0);
            do {
                Console.Write("\x1b[2K");
                if (i-- > 0) Console.Write("\n");
            } while (i > 0);

            Console.SetCursorPosition(0, 0);
            Console.Write(s);
            ResetCursor();
        }

        public static int InputWidth => ConsoleUtils.Width * 5 / 6;
        private bool _longInput = false;

        public void DrawInput() {
            Console.CursorVisible = false;
            var b = string.IsNullOrEmpty(Im.CurrentHint) || (Im.CurrentHint.Length < Im.CurrentArg.Length)
                ? ""
                : Im.CurrentHint.Substring(Im.CurrentArg.Length) + " ";
            var a = Im.LeftInput + b + Im.RightInput;

            var s = Im.LeftInput + FancyColor.Gray.PrintFunc + b + FancyColor.Reset.PrintFunc + Im.RightInput;
            _longInput = false;
            if (a.Length >= InputWidth) {
                if (Im.Cursor >= InputWidth) {
                    s = a.Substring(Im.Cursor - InputWidth,
                        Math.Min(a.Length - (Im.Cursor - InputWidth), ConsoleUtils.Width));
                    if (!string.IsNullOrEmpty(Im.CurrentHint)) {
                        s = s.Insert(InputWidth, FancyColor.Gray.PrintFunc)
                            .Insert(InputWidth + FancyColor.Gray.PrintFunc.Length +
                                    (Im.CurrentHint == null ? 0 : Im.CurrentHint.Length - Im.CurrentArg.Length),
                                FancyColor.Reset.PrintFunc);
                    }

                    _longInput = true;
                } else
                    s = a.Substring(0, Math.Min(a.Length, ConsoleUtils.Width))
                        .Insert(Im.LeftInput.Length, FancyColor.Gray.PrintFunc)
                        .Insert(Im.LeftInput.Length + FancyColor.Gray.PrintFunc.Length + Im.CurrentHint?.Length ?? 0,
                            FancyColor.Reset.PrintFunc);
            }

            Console.SetCursorPosition(0, ConsoleUtils.Height - 1);
            Console.Write("\x1b[2K");
            Console.Write(FancyColor.Reset.PrintFunc + s);
            ResetCursor();
        }

        public void DrawHints() {
            Console.CursorVisible = false;
            var selB = 0; // beginning
            var selE = 0; // ending
            var a = "";
            var s = "";
            for (var i = 0; i < Im.VisibleHints.Length; i++) {
                var vh = Im.VisibleHints[i];
                if (i == Im.HintsIndex) {
                    s += FancyColor.Gray.PrintFunc;
                    selB = a.Length;
                    selE = vh.Length;
                } else s += FancyColor.Reset.PrintFunc;

                s += vh + " ";
                a += vh + " ";
            }

            s += FancyColor.Reset.PrintFunc;
            if (a.Length >= ConsoleUtils.Width) {
                var v = selB - ConsoleUtils.Width / 2;
                var b = v > 0;
                s = a.Substring(b ? v : 0, Math.Min(a.Length - v, ConsoleUtils.Width));
                s = s.Insert(b ? ConsoleUtils.Width / 2 : selB, FancyColor.Gray.PrintFunc)
                    .Insert((b ? ConsoleUtils.Width / 2 : selB) + selE + FancyColor.Gray.PrintFunc.Length,
                        FancyColor.Reset.PrintFunc);
            }

            Console.SetCursorPosition(0, ConsoleUtils.Height - 2);
            Console.Write("\x1b[2K");
            Console.Write(s);
            ResetCursor();
        }

        public void ResetCursor() {
            Console.CursorVisible = true;
            Console.SetCursorPosition(_longInput ? InputWidth : Im.Cursor, ConsoleUtils.Height - 1);
        }

        public static int MaxLines() {
            return ConsoleUtils.Height - 2;
        }
    }

    public class InputManager {
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

        public void Start() {
            while (true) {
                Key = Console.ReadKey(true);
                ParseInput();
            }
        }

        public void ParseInput() {
            if (FancyConsole.Debug)
                FancyConsole.Log(new FancyText("[Key] ", FancyColor.Green) {
                    Next = new FancyText(
                        Key.Key + ":" + Key.Modifiers + ":" + Key.Modifiers.HasFlag(ConsoleModifiers.Shift) + ":" +
                        ((int) Key.KeyChar), FancyColor.Reset)
                });
            switch (Key.Key) {
                case ConsoleKey.Tab:
                    if (Hints == null || Hints.Length == 0) GetHints();
                    else if (Key.Modifiers.HasFlag(ConsoleModifiers.Shift)) DecrHint();
                    else IncrHint();

                    break;
                case ConsoleKey.Backspace:
                    if (Cursor > 0) {
                        var reset = LeftInput.Length > 1 && LeftInput[^1] == ' ' || LeftInput.Length == 1;
                        LeftInput = LeftInput.Substring(0, LeftInput.Length - 1);
                        MoveCursor(-1);
                        UpdateInputs();
                        UpdateVisibleHints();
                        if (reset) SpaceReset();
                    }

                    break;
                case ConsoleKey.Delete:
                    if (Cursor < CurrentInput.Length) {
                        RightInput = RightInput.Substring(1, RightInput.Length - 1);
                        MoveCursor(0);
                        UpdateInputs();
                        UpdateVisibleHints();
                    }

                    break;
                case ConsoleKey.LeftArrow:
                    MoveCursor(-1);
                    UpdateLeftRight();
                    UpdateInputs();
                    if (LeftInput.Length > 0 && LeftInput[^1] == ' ') SpaceReset();
                    else UpdateVisibleHints();
                    break;
                case ConsoleKey.RightArrow:
                    MoveCursor(1);
                    UpdateLeftRight();
                    UpdateInputs();
                    if (LeftInput.Length > 0 && LeftInput[^1] == ' ') SpaceReset();
                    else UpdateVisibleHints();
                    break;
                case ConsoleKey.Enter:
                    if (string.IsNullOrEmpty(CurrentHint) || Key.Modifiers.HasFlag(ConsoleModifiers.Shift)) Entered();
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
                case ConsoleKey.Escape:
                    Hints = new string[0];
                    VisibleHints = new string[0];
                    CurrentHint = "";
                    HintsIndex = -1;
                    break;
                case ConsoleKey.R:
                    if (Key.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                        FancyConsole.RefreshScreen();
                        return;
                    } else AddInput(Key.KeyChar);

                    break;
                default:
                    AddInput(Key.KeyChar);
                    break;
            }

            Pm.DrawInput();
            Pm.DrawHints();
        }

        public void Entered() {
            switch (CurrentInput.ToLower()) {
                case "exit":
                    FancyConsole.Log(new FancyText("[Exit] ", FancyColor.Red) {
                        Next = new FancyText("Shutting down...", FancyColor.Reset)
                    });
                    Environment.Exit(0);
                    break;
                case "debug":
                    FancyConsole.Debug = !FancyConsole.Debug;
                    FancyConsole.Log(new FancyText("[Debug] ", FancyColor.Aqua) {
                        Next = FancyConsole.Debug ? new FancyText("Enabled", FancyColor.Green) :
                            new FancyText("Disabled", FancyColor.Red)
                    });
                    break;
                default:
                    FancyConsole.Log(new FancyText("[Entered] ", FancyColor.Green) {
                        Next = new FancyText(CurrentInput, FancyColor.Reset)
                    });
                    break;
            }

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

        public void SpaceReset() {
            Hints = new string[0];
            VisibleHints = new string[0];
            CurrentHint = "";
            HintsIndex = -1;
            GetHintsR();
        }

        public void UpdateVisibleHints() {
            var lower = CurrentArg.ToLower();


            VisibleHints = Hints.Where(hint => hint.ToLower().StartsWith(lower)).ToArray();
            if (HintsIndex >= VisibleHints.Length) HintsIndex = 0;
            CurrentHint = VisibleHints.Length > HintsIndex && HintsIndex > 0 ? VisibleHints[HintsIndex] : "";
        }

        public void IncrHint() {
            HintsIndex++;
            if (HintsIndex >= VisibleHints.Length) HintsIndex = 0;
            CurrentHint = VisibleHints.Length > HintsIndex ? VisibleHints[HintsIndex] : "";
        }

        public void DecrHint() {
            HintsIndex--;
            if (HintsIndex < 0) HintsIndex = VisibleHints.Length - 1;
            CurrentHint = VisibleHints.Length > HintsIndex ? VisibleHints[HintsIndex] : "";
        }

        public void SelectHint() {
            var a = CurrentHint.Substring(CurrentArg.Length) + " ";
            LeftInput += a;
            UpdateInputs();
            MoveCursor(a.Length);
            SpaceReset();
        }

        public void GetHintsR() {
            Hints = GetTabbyThingy.GetHints(LeftInput).ToArray();
            if (Hints.Length == 1) IncrHint();
            UpdateVisibleHints();
        }

        public void GetHints() {
            GetHintsR();
            UpdateVisibleHints();
        }

        public void OffsetHistory(int offset) {
            if (HistoryIndex < 0) SavedInput = CurrentInput;
            HistoryIndex += offset;
            if (HistoryIndex < -1) HistoryIndex = -1;
            else if (HistoryIndex >= History.Count) HistoryIndex = History.Count - 1;
            CurrentInput = HistoryIndex < 0 ? SavedInput : History[HistoryIndex];
            MoveCursor(CurrentInput.Length);
            UpdateLeftRight();
            UpdateInputs();
        }

        public void AddInput(char c) {
            if (c <= '\u001F') return;
            LeftInput += c;
            UpdateInputs();
            MoveCursor(1);
            if (c == ' ') SpaceReset();
            else UpdateVisibleHints();
        }

        public void MoveCursor(int offset) {
            Cursor += offset;
            if (Cursor < 0) Cursor = 0;
            else if (Cursor > CurrentInput.Length) Cursor = CurrentInput.Length;
        }

        public void UpdateLeftRight() {
            LeftInput = CurrentInput.Substring(0, Cursor);
            RightInput = CurrentInput.Substring(Cursor, CurrentInput.Length - Cursor);
        }

        public void UpdateInputs() {
            CurrentInput = LeftInput + RightInput;
            CurrentArg = LeftInput.Substring(LeftInput.LastIndexOf(' ') + 1);
            InputStrip = LeftInput.Substring(0, LeftInput.LastIndexOf(' ') + 1);
        }
    }

    public class LogManager {
        private static PrintManager Pm => FancyConsole.Pm;
        private static InputManager Im => FancyConsole.Im;

        public int Scroll;
        public List<FancyText> Logs = new List<FancyText>();
        public List<FancyText> Lines = new List<FancyText>();

        public List<FancyText> VisibleLines() {
            var max = Math.Min(Lines.Count, PrintManager.MaxLines() - 1);
            var list = new List<FancyText>();
            for (var i = 0; i < max; i++) {
                list.Add(Lines[i + Scroll]);
            }

            return list;
        }

        public void OffsetScroll(int amount) {
            Scroll += amount;
            var m = Math.Max(Lines.Count - PrintManager.MaxLines() + 1, 0);
            if (Scroll < 0) Scroll = 0;
            else if (Scroll > m) Scroll = m;
        }

        public void Log(FancyText text) {
            Logs.Insert(0, text);
            var lines = text.GetLines();
            Lines.InsertRange(0, lines);
            if (Scroll > 0) Scroll += lines.Count;
        }

        public void RefreshLines() {
            Lines.Clear();
            Logs.Reverse();
            foreach (var text in Logs) {
                Lines.InsertRange(0, text.GetLines());
            }

            Logs.Reverse();
        }
    }
}