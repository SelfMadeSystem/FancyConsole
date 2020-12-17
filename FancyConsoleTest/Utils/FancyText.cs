using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyConsoleTest.Utils
{
    public class FancyText
    {
        public string Text;

        //public bool BoldStrikethrough;
        //public bool Underline;
        //public bool Italic; //\x1b[1m
        //public bool Reset; //\x1b[0m
        public FancyColor Color;

        // public FancyColor BgColor; // Don't use this
        public FancyText Next;

        public FancyText(string text, FancyText next = null) : this(text, FancyColor.Reset, next)
        {
        }

        public FancyText(string text, FancyColor color, FancyText next = null)
        {
            Text = text;
            Color = color;
            Next = next;
        }

        public void SetConsoleColor()
        {
            Console.ResetColor();
            if (Color.IsColor) Console.ForegroundColor = Color.ConsoleColor;
            // if (BgColor.IsColor) Console.BackgroundColor = BgColor.ConsoleColor;
        }

        public List<FancyText> Split(int len = 0)
        {
            var list = new List<FancyText>();
            var str = "";
            foreach (var c in Text)
            {
                if (c == '\n')
                {
                    len = 0;
                    list.Add(new FancyText(str, Color));
                    str = "";
                }
                else
                {
                    len++;
                    // Console.WriteLine("{" + Text + +len + "}");
                    if (len > ConsoleUtils.Width)
                    {
                        len = 0;
                        list.Add(new FancyText(str, Color));
                        str = c.ToString();
                    }
                    else str += c;
                }
            }

            if (!string.IsNullOrEmpty(str)) list.Add(new FancyText(str, Color));

            return list;
        }

        public List<FancyText> GetLines()
        {
            var lines = new List<FancyText>();
            var text = this;
            do
            {
                var split = text.Split(lines.Count > 0 ? lines[^1].GetWidthRaw(): 0);
                if (split.Count == 0) continue;
                // Console.WriteLine("{" + (lines.Count > 0 ? lines[^1].Text.Length : 0) + "}");
                var first = split[0];

                if (lines.Count > 0)
                {
                    lines[^1].SetNext(first);
                    split.Remove(first);
                    lines.AddRange(split);
                }
                else lines.AddRange(split);
            } while ((text = text.Next) != null);

            return lines;
        }

        public void SetNext(FancyText next)
        {
            var t = this;
            while (t.Next != null) t = t.Next;
            t.Next = next;
        }

        public int GetWidth()
        {
            var lines = GetLines();
            var maxWidth = lines.Select(fancyText => fancyText.GetWidthRaw()).Prepend(0).Max();
            return maxWidth;
        }
        public int GetWidthRaw()
        {
            var w = Text.Length;
            var t = this;
            while (t.Next != null)
            {
                t = t.Next;
                w += t.Text.Length;
            }

            return w;
        }

        public void PrintNext()
        {
            PrintText();
            Next?.PrintNext();
        }

        public void PrintText()
        {
            // SetConsoleColor();
            // Console.Write(Text);
            Console.Write(GetConsoleStringRaw());
        }

        public string GetConsoleStringRaw()
        {
            return Color.PrintFunc + Text;
        }

        public string GetConsoleString()
        {
            var s = "";
            var txt = this;
            do
            {
                s += txt.GetConsoleStringRaw();
            } while ((txt = txt.Next) != null);

            return s;
        }
    }
}