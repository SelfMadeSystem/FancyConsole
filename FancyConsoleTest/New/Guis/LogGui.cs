using FancyConsoleTest.New.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace FancyConsoleTest.New.Guis {
    public class LogGui : Window {
        public static LogGui Instance = new LogGui("Terminal");
        public Logs logs = new Logs();
        public TextField input = new TextField();

        public LogGui(string name) : base(name) {
            Y = 1;
            Width = Dim.Fill();
            Height = Dim.Fill();
            logs.Width = Dim.Fill();
            logs.Height = Dim.Fill() - 3;
            logs.CanFocus = false;
            input.Width = Dim.Fill();
            input.Y = Pos.Bottom(this) - 4;
            input.KeyDown += (k) => {
                if (k.KeyEvent.Key == Key.Enter) {
                    GuiApp.LineRed(input.Text.ToString());
                    input.Text = "";
                }
            };
            Add(logs);
            Add(input);
        }

        public void Log(FancyText text) {
            logs.Text += text.GetConsoleString();
        }
    }

    public class Logs : TextView {

    }
}
