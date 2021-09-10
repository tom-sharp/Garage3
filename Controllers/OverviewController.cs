using Garage3.Data;
using Garage3.Models;
using Garage3.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
			var model = await dbReadOnly.Vehicles.Include(v=> v.VehicleType).Include(v => v.Slots).Include(v => v.Person).Where(v=> v.State == Models.VehicleState.Parked).OrderBy(v => v.Person.LastName).Select(v => new ParkedVehicleViewModel(v)).ToListAsync();
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
		// fordon varje medlem har registrerade.
		// Från översiktsvyn ska vi kunna navigera till ägaren och se alla fordonen
		// Sortering av medlems översiktsvy
		//		Denna vy ska sorteras enligt följande regler.
		//	- Sorteras på medlemmens förnamn.
		//	- Sorteras endast på de två första tecknen i namnet
		//	- Om två namn har samma första två tecken så behåller vi ordningen på dessa (stable sort)
		//	- Sorteringen är case sensitive enligt ASCII (versaler sorteras före gemener)

		[HttpPost]
		public async Task<IActionResult> SearchMembers([Bind("Search")] SearchViewModel searchfor)
		{
			string mfilter = "";
			if (mfilter == null) return RedirectToAction("Search");
			if ((searchfor.Search == null) || (searchfor.Search.Length == 0)) mfilter = "";
			else mfilter = searchfor.Search.ToLower();
			IQueryable<SearchMembersViewModel> model;

			if (mfilter.Length == 0) {
				// retrive all members
				model =  dbReadOnly.Persons.Include(p => p.Vehicles).OrderBy(p => p.FirstName.Substring(0, 2)).Select(p => new SearchMembersViewModel(p));
			}
			else {
				// filter on name
				model = dbReadOnly.Persons.Include(p => p.Vehicles).Where(p => p.FirstName.ToLower().Contains(mfilter) || p.LastName.ToLower().Contains(mfilter)).OrderBy(p => p.FirstName.Substring(0, 2)).Select(p => new SearchMembersViewModel(p));
			}
			return View(await model.ToListAsync());
		}

		// Sökfunktion för fordonstyp och registreringsnummer i översiktsvyn.
		[HttpPost]
		public async Task<IActionResult> SearchVehicles([Bind("VehicleTypeId,Search")] SearchViewModel searchfor)
		{
			string lpfilter = "";
			if (searchfor == null) return RedirectToAction("Search");
			if ((searchfor.Search == null) || (searchfor.Search.Length == 0)) lpfilter = "";
			else lpfilter = searchfor.Search.ToLower();
			int vtfilter = searchfor.VehicleTypeId;
			IQueryable<SearchVehicleViewModel> model;
			// four scenarios;
			if ((vtfilter == 0) && (lpfilter.Length == 0)) {
				// do not filter
				model = dbReadOnly.Vehicles.Include(v => v.VehicleType).Include(v => v.Slots).Include(v => v.Person).OrderBy(v => v.LicensePlate).Select(v => new SearchVehicleViewModel(v));
			}
			else if ((vtfilter != 0) && (lpfilter.Length == 0)) {
				// do filter on vehicletype
				model = dbReadOnly.Vehicles.Include(v => v.VehicleType).Include(v => v.Slots).Include(v => v.Person).Where(v=> v.VehicleTypeId == vtfilter).OrderBy(v => v.LicensePlate).Select(v => new SearchVehicleViewModel(v));
			}
			else if ((vtfilter == 0) && (lpfilter.Length > 0)) {
				// do filter on licenseplate
				model = dbReadOnly.Vehicles.Include(v => v.VehicleType).Include(v => v.Slots).Include(v => v.Person).Where(v => v.LicensePlate.ToLower().Contains(lpfilter)).OrderBy(v => v.LicensePlate).Select(v => new SearchVehicleViewModel(v));
			}
			else {
				// filter on vehicle type and licence plate
				model = dbReadOnly.Vehicles.Include(v => v.VehicleType).Include(v => v.Slots).Include(v => v.Person).Where(v => v.VehicleTypeId == vtfilter && v.LicensePlate.ToLower().Contains(lpfilter)).OrderBy(v => v.LicensePlate).Select(v => new SearchVehicleViewModel(v));
			}

			return View(await model.ToListAsync());
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
