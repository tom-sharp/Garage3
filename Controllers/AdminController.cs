using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models;
using Garage3.Models.ViewModels;


/*
		1. Create a new garage:
			give it a name
			define size
			define slotsize
			create all slotsavailable
		2. Edit Garage
			change name
			change slotsize
			change size
			commit changes to slots.
		3. Delete Garage
			Remove All Garage Slots from slots
			Remove Garage

 */


namespace Garage3.Controllers
{
	public class AdminController : Controller
	{
		private readonly Garage3Context _context;

		public AdminController(Garage3Context context)
		{
			_context = context;
		}

		// GET: Garages
		public IActionResult Index() { return View(); }


		// List Garages
		public async Task<IActionResult> GaragesList()
		{
			var model = _context.Garages.OrderBy(g=> g.Name).Select(g => new GaragesViewModel(g));
			return View(await model.ToListAsync());
		}



		// GET: Garages/Create
		public IActionResult GarageNew() { return View(); }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GarageNew([Bind("Size, SlotSize, Name")] GaragesViewModel g)
		{
			if ((ModelState.IsValid) && (g.Validate()))
			{
				var newgarage = new Garage() {Name = g.Name, Size = g.Size, SlotSize = g.SlotSize };
				_context.Garages.Add(newgarage);
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException ex)
				{
					return View("_Msg", new MsgViewModel("Failed to create garage", ex.Message));
				}

				if (newgarage.Id > 0)
				{
					int counter = 0;
					newgarage.Slots = new List<Slot>();
					while (counter++ < newgarage.Size)
					{
						newgarage.Slots.Add(new Slot() { GarageId = newgarage.Id, InUse = 0, No = counter });
					}
				}
				else {
					return View("_Msg", new MsgViewModel("Failed to create garage", "Add to garage table Failed "));
				}

				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException ex) { 
					return View("_Msg", new MsgViewModel("Failed to create garage", ex.Message));
				}
				return RedirectToAction("GaragesList");
			}
			return View(g);
		}

		// GET: Garages/Edit/5
		public async Task<IActionResult> GarageEdit(int? id)
		{
			if ((id == null) || (id <= 0)) RedirectToAction("GaragesList");
			var garage = await _context.Garages.FindAsync(id);
			if (garage == null) RedirectToAction("GaragesList");
			return View(new GaragesViewModel(garage));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GarageEdit(int id, [Bind("Id,Size,SlotSize,Name")] GaragesViewModel g)
		{
			if (id != g.Id) RedirectToAction("GaragesList");
			var garage = await _context.Garages.FindAsync(id);
			if (garage == null) RedirectToAction("GaragesList");

			if ((ModelState.IsValid) && (g.Validate()))
			{
				// Will Only Update Name
				// Updating Garage and Slot size is not yet supported
				garage.Name = g.Name;
				try
				{
					_context.Update(garage);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException ex)
				{
					return View("_Msg", new MsgViewModel("Failed to Update Database", ex.Message));
				}
				return RedirectToAction("GaragesList");
			}
			return View(new GaragesViewModel(garage));
		}

		// GET: Garages/Details/5
		public async Task<IActionResult> GarageSlots(int? id)
		{
			if (id == null) return RedirectToAction("GaragesList");
			
			var garage = await _context.Garages.FirstOrDefaultAsync(m => m.Id == id);
			if (garage == null) return RedirectToAction("GaragesList");
			garage.Slots = await _context.Slots.Where(s => s.GarageId == garage.Id).ToListAsync();
			return View(garage.Slots);
		}


		// GET: Garages/Delete/5
		public async Task<IActionResult> GarageDelete(int? id)
		{
			if ((id == null) || (id <= 0)) return RedirectToAction("GaragesList");
			var garage = await _context.Garages.FirstOrDefaultAsync(m => m.Id == id);
			if (garage == null) return RedirectToAction("GaragesList");
			return View(garage);
		}

		// POST: Garages/Delete/5
		[HttpPost, ActionName("GarageDeleteConfirmed")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GarageDeleteConfirmed(int id)
		{
			if (id <= 0) return RedirectToAction("GaragesList");
			var garage = await _context.Garages.FindAsync(id);
			if (garage == null) return RedirectToAction("GaragesList");

			// delete all slots (Not really needed as slots depend on garage and has cascade delete)
			var allslots = await _context.Slots.Where(s => s.GarageId == garage.Id).ToListAsync();
			if (allslots.Count > 0) _context.Slots.RemoveRange(allslots);
			_context.Garages.Remove(garage);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex) {
				return View("_Err", new MsgViewModel("Failed Deleting Garage", ex.Message));
			}
			return RedirectToAction("GaragesList");
		}

		private bool GarageExists(int id)
		{
			return _context.Garages.Any(e => e.Id == id);
		}



		/*	##############################################
			##											##
			##				VehicleType					##
			##											##
			############################################## */


		// List VehicleTypes
		public async Task<IActionResult> VehicleTypesList()
		{
			var model = _context.VehicleTypes.OrderBy(v => v.Name).Select(v => new VehicleTypesViewModel() { Size = v.Size, Name = v.Name, Id = v.Id });
			return View(await model.ToListAsync());

		}

		// Create VehicleType
		public IActionResult VehicleTypeCreate() { return View(); }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> VehicleTypeCreate([Bind("Size, Name")] VehicleTypesViewModel v)
		{
			if ((ModelState.IsValid) && (v.Validate()))
			{
				var newVehicleType = new VehicleType() { Name = v.Name, Size = v.Size};
				_context.Add(newVehicleType);

				await _context.SaveChangesAsync();

				return RedirectToAction("VehicleTypesList");
			}
			return View(v);
		}

		// GET: VehicleTypes/Edit/5
		public async Task<IActionResult> VehicleTypeEdit(int? id)
		{
			if ((id == null) || (id <= 0)) return View("VehicleTypesList");

			var v = await _context.VehicleTypes.FindAsync(id);
			if (v == null)
			{
				return NotFound();
			}
			return View(v);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> VehicleTypeEdit(int id, [Bind("Id,Size,Name")] VehicleType v)
		{
			if (id != v.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(v);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!VehicleTypeExists(v.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("VehicleTypesList");
			}
			return View(v);
		}

		// GET: VehicleTypes/Delete/5
		public async Task<IActionResult> VehicleTypeDelete(int? id)
		{
			if ((id == null) || (id <= 0)) return View("VehicleTypesList");

			var v = await _context.VehicleTypes
				.FirstOrDefaultAsync(m => m.Id == id);
			if (v == null)
			{
				return NotFound();
			}

			return View(v);
		}

		// POST: VehicleTypes/Delete/5
		[HttpPost, ActionName("VehicleTypeDeleteConfirmed")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> VehicleTypeDeleteConfirmed(int id)
		{
			if (id <= 0) return RedirectToAction("VehicleTypesList");
			var v = await _context.VehicleTypes.FindAsync(id);
			if (v == null) return RedirectToAction("VehicleTypesList");

			_context.VehicleTypes.Remove(v);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				return View("_Err", new MsgViewModel("Failed Deleting Garage", ex.Message));
			}
			return RedirectToAction("VehicleTypesList");
		}

		private bool VehicleTypeExists(int id)
		{
			return _context.VehicleTypes.Any(e => e.Id == id);
		}

		/*	##############################################
			##											##
			##					Person					##
			##											##
			############################################## */


		// List Persons
		public async Task<IActionResult> PersonsList()
		{
			var model = _context.Persons.OrderBy(v => v.FirstName).Select(v => new PersonsViewModel() { FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, SSN = v.SSN, MemberType = v.MemberType, Id = v.Id });
			return View(await model.ToListAsync());

		}

		// Create Person
		public IActionResult PersonCreate() { return View(); }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PersonCreate([Bind("FirstName, LastName, Email, SSN, BirthDate, MemberType")] PersonsViewModel v)
		{
			if ((ModelState.IsValid) && (v.Validate()))
			{
				var newPerson = new Person() { FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, SSN = v.SSN,  MemberType = v.MemberType };
				_context.Add(newPerson);

				await _context.SaveChangesAsync();

				return RedirectToAction("PersonsList");
			}
			return View(v);
		}

		// GET: Persons/Edit/5
		public async Task<IActionResult> PersonEdit(int? id)
		{
			if ((id == null) || (id <= 0)) return View("PersonsList");

			var v = await _context.Persons.FindAsync(id);
			if (v == null)
			{
				return NotFound();
			}
			return View(v);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PersonEdit(int id, [Bind("Id,FirstName,LastName,Email,SSN,BirthDate,MemberType")] Person v)
		{
			if (id != v.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(v);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PersonExists(v.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("PersonsList");
			}
			return View(v);
		}

		// GET: Persons/Delete/5
		public async Task<IActionResult> PersonDelete(int? id)
		{
			if ((id == null) || (id <= 0)) return View("PersonsList");

			var v = await _context.Persons
				.FirstOrDefaultAsync(m => m.Id == id);
			if (v == null)
			{
				return NotFound();
			}

			return View(v);
		}

		// POST: Persons/Delete/5
		[HttpPost, ActionName("PersonDeleteConfirmed")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PersonDeleteConfirmed(int id)
		{
			if (id <= 0) return RedirectToAction("PersonsList");
			var v = await _context.Persons.FindAsync(id);
			if (v == null) return RedirectToAction("PersonsList");

			_context.Persons.Remove(v);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				return View("_Err", new MsgViewModel("Failed Deleting Garage", ex.Message));
			}
			return RedirectToAction("PersonsList");
		}

		private bool PersonExists(int id)
		{
			return _context.Persons.Any(e => e.Id == id);
		}

		/*	##############################################
			##											##
			##				  Statistics				##
			##											##
			############################################## */

		/*
		public async Task<IActionResult> Statistics()
		{
			var stats = new StatisticsViewModel();

			//var parked = await _context.ParkedVehicle.Where(v => v.State == VehicleState.Parked).ToListAsync();
			
			TimeSpan accumulatedtime = TimeSpan.Zero;
			DateTime nowtime = DateTime.Now;

			stats.NoOfCars = 0;
			stats.NoOfMotorCycles = 0;
			stats.NoOfTrucks = 0;
			stats.NoOfBuses = 0;
			stats.NoOfBoats = 0;

			stats.NoOfBlack = 0;
			stats.NoOfBlue = 0;
			stats.NoOfGreen = 0;
			stats.NoOfRed = 0;
			stats.NoOfSilver = 0;
			stats.NoOfUnknown = 0;
			stats.NoOfWhite = 0;
			stats.NoOfYellow = 0;

			stats.NoTotalVehicles = 0;
			stats.NoOfWheels = 0;
			stats.GarageTurnOver = 0;

			
			foreach (var v in parked)
			{
				switch (v.Color)
				{
					case VehicleColor.Black: stats.NoOfBlack++; break;
					case VehicleColor.Blue: stats.NoOfBlue++; break;
					case VehicleColor.Green: stats.NoOfGreen++; break;
					case VehicleColor.Red: stats.NoOfRed++; break;
					case VehicleColor.Silver: stats.NoOfSilver++; break;
					case VehicleColor.Unknown: stats.NoOfUnknown++; break;
					case VehicleColor.White: stats.NoOfWhite++; break;
					case VehicleColor.Yellow: stats.NoOfYellow++; break;
				}
			

				switch (v.VehicleType)
				{

					
					stats.
					case VehicleType.Car: stats.NoOfCars++; break;
					case VehicleType.Motorcycle: stats.NoOfMotorCycles++; break;
					case VehicleType.Truck: stats.NoOfTrucks++; break;
					case VehicleType.Bus: stats.NoOfBuses++; break;
					case VehicleType.Boat: stats.NoOfBoats++; break;
					
				}

				stats.NoTotalVehicles++;
				stats.NoOfWheels += v.Wheels;
				accumulatedtime += nowtime - v.ArrivalTime;
			}
		
			stats.GarageTurnOver = (int)(accumulatedtime.TotalMinutes * Globals.ParkingPrice);

			if (stats.NoTotalVehicles > 0) accumulatedtime /= stats.NoTotalVehicles;
			else accumulatedtime = TimeSpan.Zero;

			stats.AvgParkingTime = (int)accumulatedtime.TotalHours;

			return View("Statistics", stats);

		
		}
		*/


	}
}
