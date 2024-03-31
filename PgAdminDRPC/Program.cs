using System;

namespace PgAdminDRPC
{
    internal class Program
	{
		static void Main(string[] args)
		{
			var worker = new PresenceWorker();

			worker.Start();

            Console.ReadKey();

			// Dispose the timer
            worker.Timer.Dispose();

			// Kill the application
            Environment.Exit(0);
        }
	}
}
