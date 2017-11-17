using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleTrayIcon.Library;

namespace ConsoleTrayIcon
{
    internal class Program
    {     
        private static void Main()
        {
            Task.Factory.StartNew(Run);

            var windowsMenu = new WindowsMenu();
            windowsMenu.AddMenuItem("PoweShell", PowerShell_Click);

            windowsMenu.Create();

            Application.Run();
        }

        private static void Run()
        {
            Console.WriteLine(@"Emulating some event...");

            while (true)
            {
                Thread.Sleep(5000);
                Console.WriteLine(@"Emulating some event...");
            }
        }

        private static void PowerShell_Click(object sender, EventArgs e)
        {
            Console.WriteLine(@"PowerShell opened.");
            Process.Start("C:\\windows\\system32\\windowspowershell\\v1.0\\powershell.exe ");
        }
    }
}