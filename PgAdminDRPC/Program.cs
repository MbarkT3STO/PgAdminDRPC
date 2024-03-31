using Microsoft.SqlServer.Server;

using System;
using System.Runtime.InteropServices;
using System.Threading;

using static System.Net.Mime.MediaTypeNames;

namespace PgAdminDRPC
{
    internal class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = -1;


        static void Main(string[] args)
        {
            // Hide the console window
            var handle = GetConsoleWindow();
            // To hide:
            ShowWindow(handle, SW_HIDE);

            var worker = new PresenceWorker();

            try
            {
                worker.Start();

                // sleep thread forever
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception)
            {
                // Dispose the timer
                worker.Timer.Dispose();

                // Kill the application
                Environment.Exit(0);
            }
        }
    }
}
