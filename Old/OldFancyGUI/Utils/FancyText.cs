using System;
using System.Collections.Generic;
using System.Linq;

namespace FancyConsoleTest.Old.Utils
{
    public class FancyText
    {
        public string Text;

        public FancyColor[] Colors;

        // public FancyColor BgColor; // Don't use this
        public FancyText Next;

        public FancyText(string text, params FancyColor[] colors)
        {
            Text = text;
            Colors = colors;
        }

        public void SetConsoleColor()
        {
            Console.ResetColor();
            foreach (var color in Colors)
            {
                if (color.IsColor) Console.ForegroundColor = color.ConsoleColor;
            }
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
                    list.Add(new FancyText(str, Colors));
                    str = "";
                }
                else
                {
                    len++;
                    // Console.WriteLine("{" + Text + +len + "}");
                    if (len > ConsoleUtils.Width)
                    {
                        len = 0;
                        list.Add(new FancyText(str, Colors));
                        str = c.ToString();
                    }
                    else str += c;
                }
            }

            if (!string.IsNullOrEmpty(str)) list.Add(new FancyText(str, Colors));

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
            if (ConsoleUtils.GetColorSupport() == 1) SetConsoleColor();
            Console.Write(GetConsoleStringRaw());
        }

        public string GetConsoleStringRaw()
        {
            return (ConsoleUtils.GetColorSupport() == 2 ? Colors.Aggregate("", (current, item) => current + item.PrintFunc) : "") + Text;
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