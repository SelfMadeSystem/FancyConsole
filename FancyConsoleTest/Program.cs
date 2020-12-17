using System;
using System.Threading;
using FancyConsoleTest.Utils;

namespace FancyConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var text = new FancyText("Red", FancyColor.Red)
            {
                Next = new FancyText("Blue", FancyColor.Blue)
                {
                    Next = new FancyText("Green", FancyColor.Green)
                    {
                        Next = new FancyText("Yellow", FancyColor.Yellow)
                        {
                            Next = new FancyText("Cyan", FancyColor.Aqua)
                        }
                    }
                }
            };

            var lines = text.GetLines();
            Console.WriteLine("=============" + ConsoleUtils.Width + "========" + Console.BufferWidth);
            foreach (var fancyText in lines)
            {
                fancyText.PrintNext();

                Console.Write("\n");
            } 

            /*for (var i = 0; i < 255; i++)
            {
                Console.Write("\n\x1b[" + i + "m" + "Num: " + i + "\x1b[0m");
            }
            for (var r = 0; r < 255; r+=4)
            {
                for (var g = 0; g < 255; g+=2)
                {
                    Console.Write("\x1b[48;2;" + r + ";" + g + ";0" +"m" + " " + "\x1b[0m");
                }

                Console.Write("\n");
            }

            //text.PrintNext();*/
            try
            {
                FancyConsole.FancyConsole.Init();
                new Thread(FancyConsole.FancyConsole.StartInput).Start();
                FancyConsole.FancyConsole.Log(new FancyText("[Warning] ", FancyColor.Yellow)
                {
                    Next = new FancyText("Resizing of window will cause issues. Press ESC to refresh screen.")
                });
                FancyConsole.FancyConsole.Log(new FancyText("[Warning] ", FancyColor.Yellow)
                {
                    Next = new FancyText("If you are running this in Rider, there will be bugs!!!")
                });
                for (var i = 0; i < 200; i++)
                {
                    FancyConsole.FancyConsole.Log(new FancyText("[Spam] ", FancyColor.Gold)
                    {
                        Next = new FancyText(i.ToString())
                    });
                }
                FancyConsole.FancyConsole.StartScreen();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}