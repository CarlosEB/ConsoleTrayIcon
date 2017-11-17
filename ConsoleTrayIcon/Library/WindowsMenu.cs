using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsoleTrayIcon.Library
{
    public class WindowsMenu
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        static NotifyIcon _notifyIcon;
        static IntPtr _processHandle;
        static IntPtr _winShell;
        static IntPtr _winDesktop;
        static MenuItem _hideMenu;
        static MenuItem _restoreMenu;

        private readonly ContextMenu _menu;

        private readonly Dictionary<string, EventHandler> _menuOptions;

        public WindowsMenu()
        {

            _menuOptions = new Dictionary<string, EventHandler>();

            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon("icon.ico"),
                Text = @"Monitor",
                Visible = true
            };

            _menu = new ContextMenu();

            _hideMenu = new MenuItem("Hide", Minimize_Click);
            _restoreMenu = new MenuItem("Restore", Maximize_Click);
        }

        public void AddMenuItem(string name, EventHandler action)
        {
            _menuOptions.Add(name, action);
        }

        public void Create()
        {
            _menu.MenuItems.Add(_restoreMenu);
            _menu.MenuItems.Add(_hideMenu);
            _menu.MenuItems.Add("-");

            foreach (var item in _menuOptions)
                _menu.MenuItems.Add(new MenuItem(item.Key, item.Value));

            _menu.MenuItems.Add("-");
            _menu.MenuItems.Add(new MenuItem("Exit", CleanExit));

            _notifyIcon.ContextMenu = _menu;

            _processHandle = Process.GetCurrentProcess().MainWindowHandle;

            _winShell = GetShellWindow();
            _winDesktop = GetDesktopWindow();

            ResizeWindow(false);
        }


        private static void CleanExit(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
            Environment.Exit(1);
        }

        private static void Minimize_Click(object sender, EventArgs e)
        {
            ResizeWindow(false);
        }

        private static void Maximize_Click(object sender, EventArgs e)
        {
            ResizeWindow();
        }

        private static void ResizeWindow(bool restore = true)
        {
            if (restore)
            {
                _restoreMenu.Enabled = false;
                _hideMenu.Enabled = true;
                SetParent(_processHandle, _winDesktop);
            }
            else
            {
                _restoreMenu.Enabled = true;
                _hideMenu.Enabled = false;
                SetParent(_processHandle, _winShell);
            }
        }
    }
}
