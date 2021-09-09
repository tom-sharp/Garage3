using Garage3.Data;
using Garage3.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Controllers
{
	public class OverviewController : Controller
	{
		private readonly Garage3contextNoTracking dbReadOnly;

		public OverviewController(Garage3contextNoTracking context) { dbReadOnly = context;	}
		public IActionResult Index() { return View(); }

		// List Garages
		public async Task<IActionResult> GaragesList()
		{
			var model = await dbReadOnly.Garages.OrderBy(g => g.Name).Select(g => new GaragesViewModel(g)).ToListAsync();
			foreach (var g in model) {
				g.FreeSlots = await FreeSlots(g.Id);
			}
			return View(model);
		}



		private async Task<int> FreeSlots(int garageid)
		{
			if (garageid <= 0) return 0;
			var garage = await dbReadOnly.Garages.FindAsync(garageid);
			if (garage == null) return 0;
			var garageslots = await dbReadOnly.Slots.Where(s => s.GarageId == garage.Id).ToListAsync();
			int slotsize = garage.SlotSize;
			int counter = 0;
			foreach (var slot in garageslots)
			{
				if (slot.InUse == 0) counter++;
				else if ((slot.InUse < 0) || (slot.InUse > slotsize)) throw new ApplicationException($"DEBUG::FreeSlots - InUse Integrigy test failed - (SlotId: {slot.Id} InUse={slot.InUse})");
			}
			return counter;
		}



	}
}
