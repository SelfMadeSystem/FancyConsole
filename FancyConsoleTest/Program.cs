using System;
using System.Threading;
using FancyConsoleTest.New;

namespace FancyConsoleTest {
    class Program {
        static void Main(string[] args) {
            try {
                GuiApp.Start(args);
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}