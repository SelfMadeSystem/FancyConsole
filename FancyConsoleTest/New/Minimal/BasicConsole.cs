using FancyConsoleTest.New.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace FancyConsoleTest.New.Minimal {
    public class BasicConsole {
        public static BasicConsole Instance = new BasicConsole();
        public void StartInputting(string[] args) {
            while (true) {
                var line = Console.ReadLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', ConsoleUtils.Width));
                Console.SetCursorPosition(0, Console.CursorTop);
                GuiApp.LineRed(line);
            }
        }

        public void Log(FancyText text) {
            text.PrintNext();
        }
    }
}
