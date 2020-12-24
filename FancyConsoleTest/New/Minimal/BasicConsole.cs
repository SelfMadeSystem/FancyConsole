using FancyConsoleTest.New.Utils;
using System;

namespace FancyConsoleTest.New.Minimal
{
    public class BasicConsole
    {
        public static readonly BasicConsole Instance = new BasicConsole();

        public void StartInputting(string[] args)
        {
            while (true) GuiApp.LineRed(Console.ReadLine());
        }

        public void Log(FancyText text)
        {
            text.PrintNext(GuiApp.ConsoleColors);
            Console.Write("\n");
        }
    }
}