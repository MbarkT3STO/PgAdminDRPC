using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MBDRPC.Core;
using MBDRPC.Helpers;

namespace PgAdminDRPC
{
	public class PresenceWorker
	{
		private Presence presence          = new Presence();
        private bool     isPgAdminFirstRun = true;
		private DateTime startTime;
        private string   currentPgAdminProcessName;
		private string   currentPgAdminAppVersion;
		    

        private string[] pgAdminProcessNames = { "pgAdmin", "pgAdmin1", "pgAdmin2", "pgAdmin3", "pgAdmin4" };

        private Dictionary<string , string> pgAdminAppNames = new Dictionary<string , string>
                                                              {
                                                                  {"pgAdmin" , "pg Admin"} ,
                                                                  {"pgAdmin1" , "pg Admin 1"} ,
                                                                  {"pgAdmin2" , "pg Admin 2"} ,
                                                                  {"pgAdmin3" , "pg Admin 3"} ,
                                                                  {"pgAdmin4" , "pg Admin 4"} ,
                                                              };

        public Timer Timer;


        public PresenceWorker()
        {
            Timer = new Timer(_ => CheckPgAdmin(), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Starts the presence
        /// </summary>
        public void Start()
		{
			// Create and start a timer to keep the presence up to date
			var timer = new Timer(_ => CheckPgAdmin(), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            timer.Dispose();
        }



		private void CheckPgAdmin()
		{
			var isPgAdminOpen = RunningAppChecker.IsOneAppRunning(pgAdminProcessNames);
			if (isPgAdminOpen)
			{
				if (isPgAdminFirstRun)
                {
                    currentPgAdminProcessName = GetWhichProcessNameIsRunning();
                    currentPgAdminAppVersion  = GetWhichAppVersionIsRunning();
                    
					presence.InitializePresence("1223760050753896548");

					presence.UpdateLargeImage("logo", currentPgAdminAppVersion);
					presence.UpdateDetails(currentPgAdminAppVersion);


					startTime         = RunningAppChecker.GetProcessStartTime(currentPgAdminProcessName);
					isPgAdminFirstRun = false;
				}

				UpdatePresence();
			}
			else
			{
				presence.ShutDown();
				isPgAdminFirstRun = true;
			}
		}


		/// <summary>
		/// Updates the presence
		/// </summary>
		private void UpdatePresence()
		{
			UpdatePresenceTime();
			presence.UpdatePresence();
		}

		/// <summary>
		/// Updates the presence time
		/// </summary>
		private void UpdatePresenceTime()
		{
			var elapsedTime = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");
			presence.UpdateState(elapsedTime);
		}


        private string GetWhichProcessNameIsRunning()
        {
            var processName = Process.GetProcesses()
                                     .FirstOrDefault( p => pgAdminProcessNames.Contains( p.ProcessName ,
                                                          StringComparer.OrdinalIgnoreCase ) )?.ProcessName ??
                              string.Empty;

            return processName;
        }


        private string GetWhichAppVersionIsRunning()
        {
            return pgAdminAppNames[currentPgAdminProcessName];
        }

	}
}