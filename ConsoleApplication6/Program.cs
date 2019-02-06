using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalQuartz.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Quartz;
using Quartz.Impl;
using Quartzmin;

namespace ConsoleApplication6
{
	class Program
	{
		static void Main(string[] args)
		{
			//IScheduler scheduler = SetupScheduler();
			//Action<IAppBuilder> startup = app =>
			//{
			//	app.UseCrystalQuartz(() => scheduler);
			//};

			//Console.WriteLine("Starting self-hosted server...");
			//using (WebApp.Start("http://localhost:9000/", startup))
			//{
			//	Console.WriteLine("Server is started");
			//	Console.WriteLine();
			//	Console.WriteLine("Check http://localhost:9000/quartz to see jobs information");
			//	Console.WriteLine();

			//	Console.WriteLine("Starting scheduler...");
			//	scheduler.Start();

			//	Console.WriteLine("Scheduler is started");
			//	Console.WriteLine();
			//	Console.WriteLine("Press [ENTER] to close");
			//	Console.ReadLine();
			//}

			//Console.WriteLine("Shutting down...");
			//scheduler.Shutdown(waitForJobsToComplete: true);
			Action<IAppBuilder> startup = app =>
			{
				app.UseQuartzmin(new QuartzminOptions()
				{
					Scheduler = StdSchedulerFactory.GetDefaultScheduler().Result
				});
			};

			Console.WriteLine("Starting self-hosted server...");
			using (WebApp.Start("http://localhost:9000/", startup))
			{
				Console.WriteLine("Server is started");
				Console.WriteLine();
				Console.WriteLine("Check http://localhost:9000/quartz to see jobs information");
				Console.WriteLine();

				Console.WriteLine("Starting scheduler...");

				Console.WriteLine("Scheduler is started");
				Console.WriteLine();
				Console.WriteLine("Press [ENTER] to close");
				Console.ReadLine();
			}


			Console.ReadLine();
		}


		private static IScheduler SetupScheduler()
		{

			NameValueCollection properties = new NameValueCollection();

			properties["quartz.scheduler.instanceName"] = "RemoteClient";

			properties["quartz.threadPool.type"] = "Quartz.Simpl.DedicatedThreadPool, Quartz";
			properties["quartz.threadPool.threadCount"] = "5";
			properties["quartz.threadPool.threadPriority"] = "Normal";
			properties["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz";

			// set remoting expoter
			properties["quartz.scheduler.proxy"] = "true";
			properties["quartz.scheduler.proxy.address"] = "tcp://localhost:555/QuartzScheduler";

			ISchedulerFactory sf = new StdSchedulerFactory(properties);
			var sched = sf.GetScheduler().Result;

			return sched;
		}
	}
}
