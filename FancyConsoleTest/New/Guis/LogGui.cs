using FancyConsoleTest.New.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace FancyConsoleTest.New.Guis {
    public class LogGui : Window {
        public static LogGui Instance = new LogGui("Terminal");
        public TextField logs = new TextField();

        public LogGui(string name) : base(name) {
            Width = Dim.Fill();
            Height = Dim.Fill();
        }

        public void Log(FancyText text) {
        }
    }
}
