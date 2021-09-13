using Garage3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.Services
{
	public class MemorySlotManager : ISlotManager
	{
		public bool IsRunning { get { return IsDBRead; } }

		public static async Task Run(IServiceProvider services)
		{

			//			var db = services.GetRequiredService<Garage3Context>();

			//var logger = services.GetRequiredService<ILogger<Program>>();
			//logger.LogError(ex.Message, "Memory slotmanager init failed");

			using (var db = services.GetRequiredService<Garage3Context>())
			{

				// Read Slots
				var dbslots = await db.Slots.ToListAsync();
				foreach (var slot in dbslots) {
					slots.Add(slot);
				}

			}



		}


		static ConcurrentBag<Slot> slots = new ConcurrentBag<Slot>();
		static bool IsDBRead = false;

	}
}
