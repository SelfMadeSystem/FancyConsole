using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace FancyConsoleTest.New.Guis {
    public class GuiHndlr {
        public static GuiHndlr Instance = new GuiHndlr();
        public void Start(string[] args) {
            Application.Init();
            var top = Application.Top;
            var win = LogGui.Instance;

            top.Add(win);

            Application.Run();
        }
    }

    internal class TopBar : MenuBar {
        // todo: make me
    }
}
