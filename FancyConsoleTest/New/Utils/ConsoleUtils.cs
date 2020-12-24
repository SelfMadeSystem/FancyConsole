using System;
using System.Collections.Generic;
using System.Text;

namespace FancyConsoleTest.New.Utils
{
    public static class ConsoleUtils
    {
        public static int Width => Console.BufferWidth;
        public static int Height => Console.BufferHeight;
    }
}