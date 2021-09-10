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


		// List Parked Vehicles
		// Ägare, Medlemskap, Fordonstyp, RegNum och ParkTid som minimum

		public async Task<IActionResult> ParkedVehicles()
		{
			var model = await dbReadOnly.Vehicles.Include(v=> v.VehicleType).Include(v => v.Slots).Include(v => v.Person).Where(v=> v.State == Models.VehicleState.Parked).OrderBy(v => v.Person).Select(v => new ParkedVehicleViewModel(v)).ToListAsync();
			return View(model);
		}


		// Search Menu
		public async Task<IActionResult> Search()
		{
			var model = new SearchViewModel();
			model.VTypes = await dbReadOnly.VehicleTypes.ToListAsync();
			return View("_SearchMenu",model);
		}

		// Search Members
		// Där vi även ska kunna se hur många
		// fordon varje medlem har registrerade.Från översiktsvyn ska vi kunna navigera till ägaren
		// och se alla fordonen

		[HttpPost]
		public async Task<IActionResult> SearchMembers([Bind("Search")] SearchViewModel searchtext)
		{
			var model = await dbReadOnly.Vehicles.Include(v => v.VehicleType).Include(v => v.Slots).Include(v => v.Person).Where(v => v.State == Models.VehicleState.Parked).OrderBy(v => v.Person).Select(v => new ParkedVehicleViewModel(v)).ToListAsync();
			return View();
		}

		// Sökfunktion för fordonstyp och registreringsnummer i översiktsvyn.
		[HttpPost]
		public async Task<IActionResult> SearchVehicles([Bind("VehicleTypeId,Search")] SearchViewModel searchtext)
		{
			var model = await dbReadOnly.Vehicles.Include(v => v.VehicleType).Include(v => v.Slots).Include(v => v.Person).Where(v => v.State == Models.VehicleState.Parked).OrderBy(v => v.Person).Select(v => new ParkedVehicleViewModel(v)).ToListAsync();
			return View();
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
