using System;

namespace FancyConsoleTest.Utils
{
    public class ConsoleUtils
    {
        public static int Width => Console.BufferWidth;
        public static int Height => Console.BufferHeight;

        public static void SetConsoleLine(char c=' ', int start=0, int end=0)
        {
            var currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(start, Console.CursorTop);
            Console.Write(new string(c, Console.WindowWidth-start-end));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}