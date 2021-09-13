using Garage3.Data;
using Garage3.Models.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3
{
	public class Program
	{
		public static void Main(string[] args)
		{
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

				try
				{
					SeedData.InitAsync(services).Wait();
				}
				catch (Exception e)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(e.Message, "Seed Fail");
				}

				// Init Memory Slotmanager - Reading DB with Slots
				// for some reson both SeedData and MemorySlotManager init can not both init
				// exception it thrown about disposed db
				//try
				//{

				//	MemorySlotManager.Run(services).Wait();
				//}
				//catch (Exception ex) {
				//	var logger = services.GetRequiredService<ILogger<Program>>();
				//	logger.LogError(ex.Message, "Memory slotmanager init failed");
				//}

			}

			host.Run();

        }

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
