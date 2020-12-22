using FancyConsoleTest.New.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace FancyConsoleTest.New.Guis {
    public class LogGui : Window {
        public static LogGui Instance = new LogGui("Terminal");
        public LogsView logs = new LogsView();
        public TextField input = new TextField();

        public LogGui(string name) : base(name) {
            ColorScheme.Normal = new Terminal.Gui.Attribute(Color.White, Color.Black);
            Y = 1;
            Width = Dim.Fill();
            Height = Dim.Fill();
            logs.Width = Dim.Fill();
            logs.Height = Dim.Fill() - 3;
            logs.ReadOnly = true;
            input.Width = Dim.Fill();
            input.Y = Pos.Bottom(this) - 4;
            input.KeyDown += (k) => {
                if (k.KeyEvent.Key == Key.Enter) {
                    GuiApp.LineRed(input.Text.ToString());
                    input.Text = "";
                }
            };
            Add(input);
            Add(logs);
        }

        public void Log(FancyText text) {
            logs.Log(text);
        }
    }

    public class LogsView : TextView {
        public int Scroll;
        public List<FancyText> Logs = new List<FancyText>();
        public List<FancyText> Lines = new List<FancyText>();

        public List<FancyText> VisibleLines() {
            var max = Math.Min(Lines.Count, Bounds.Width - 1);
            var list = new List<FancyText>();
            for (var i = 0; i < max; i++) {
                list.Add(Lines[i + Scroll]);
            }

            return list;
        }

        public void OffsetScroll(int amount) {
            Scroll += amount;
            var m = Math.Max(Lines.Count - Bounds.Width + 1, 0);
            if (Scroll < 0) Scroll = 0;
            else if (Scroll > m) Scroll = m;
        }

        public void Log(FancyText text) {
            Logs.Insert(0, text);
            var lines = text.GetLines(Bounds.Width);
            Lines.InsertRange(0, lines);
            if (Scroll > 0) Scroll += lines.Count;
        }

        public void RefreshLines() {
            Lines.Clear();
            Logs.Reverse();
            foreach (var text in Logs) {
                Lines.InsertRange(0, text.GetLines(Bounds.Width));
            }

            Logs.Reverse();
        }
        void ColorNormal() {
            Driver.SetAttribute(ColorScheme.Normal);
        }
        void ClearRegion(int left, int top, int right, int bottom) {
            for (int row = top; row < bottom; row++) {
                Move(left, row);
                for (int col = left; col < right; col++)
                    AddRune(col, row, ' ');
            }
        }

        void ColorText(FancyText text) {
            FancyColor color = null;
            foreach (var v in text.Colors) { if (v.IsColor) color = v; else if (v.Equals(FancyColor.Reset)) ColorNormal(); }
            if (color != null) Driver.SetAttribute(new Terminal.Gui.Attribute(color.Color, Color.Black));
        }

        // https://github.com/migueldeicaza/gui.cs/blob/master/Terminal.Gui/Views/TextView.cs
        // todo: make scrolling
        public override void Redraw(Rect bounds) {
            ColorNormal();
            var lines = VisibleLines();

            int bottom = bounds.Bottom;
            int right = bounds.Right;
            for (int row = bounds.Top; row < bottom; row++) {
                int textLine = row; //topRow + row
                if (textLine >= lines.Count) {
                    ColorNormal();
                    ClearRegion(bounds.Left, row, bounds.Right, row + 1);
                    continue;
                }
                var line = lines[textLine];
                Move(bounds.Left, row);
                int prevCol = bounds.Left;
                int col = bounds.Left;
                do {
                    int lineRuneCount = line.Text.Length;
                    ColorText(line);

                    for (; col < prevCol + lineRuneCount; col++) {
                        var lineCol = col - prevCol;
                        var rune = lineCol >= lineRuneCount ? ' ' : line.Text[lineCol];
                        AddRune(col, row, rune);
                    }
                    prevCol = col;
                } while ((line = line.Next) != null);
                for (; col < right; col++) AddRune(col, row, ' ');
            }
            ColorNormal();
            PositionCursor();
        }
    }
}
