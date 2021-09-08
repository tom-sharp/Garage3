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
				_context.Add(newgarage);

				// ADD SOME CODE HERE TO HANDLE FAILURE
				await _context.SaveChangesAsync();

				// HERE CREATE SLOTS
				//if (newgarage.Id > 0) {
				//	int counter = 0, totalslots = newgarage.Size * newgarage.SlotSize;
				//	newgarage.Slots = new List<Slot>();
				//	while (counter++ < newgarage.Size) {
				//		newgarage.Slots.Add(new Slot() { GarageId = newgarage.Id, State = 0, VehicleId = 0 });
				//	}
				//	// ADD SOME CODE HERE TO HANDLE FAILURE
				//	await _context.SaveChangesAsync();
				//	// ADD SOME CODE HERE TO SHOW SUCCESS
				//	var slot = new Slot();
				//	slot.
				//}
				return RedirectToAction(nameof(Index));
			}
			return View(g);
		}

		// GET: Garages/Edit/5
		public async Task<IActionResult> GarageEdit(int? id)
		{
			if ((id == null) || (id <= 0)) return View("GarageList");

			var garage = await _context.Garages.FindAsync(id);
			if (garage == null)
			{
				return NotFound();
			}
			return View(garage);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GarageEdit(int id, [Bind("Id,Size,SlotSize,Name")] Garage garage)
		{
			if (id != garage.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(garage);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!GarageExists(garage.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(GaragesList));
			}
			return View(garage);
		}

		// GET: Garages/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var garage = await _context.Garages
				.FirstOrDefaultAsync(m => m.Id == id);
			if (garage == null)
			{
				return NotFound();
			}

			return View(garage);
		}


		// GET: Garages/Delete/5
		public async Task<IActionResult> GarageDelete(int? id)
		{
			if ((id == null) || (id <= 0)) return View("GarageList");

			var garage = await _context.Garages
				.FirstOrDefaultAsync(m => m.Id == id);
			if (garage == null)
			{
				return NotFound();
			}

			return View(garage);
		}

		// POST: Garages/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (id <= 0) return View("GarageList");
			var garage = await _context.Garages.FindAsync(id);
			_context.Garages.Remove(garage);
			await _context.SaveChangesAsync();
			return View("GarageList");
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
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteVehicleTypeConfirmed(int id)
		{
			if (id <= 0) return View("VehicleTypesList");
			var v = await _context.VehicleTypes.FindAsync(id);
			_context.VehicleTypes.Remove(v);
			await _context.SaveChangesAsync();
			return View("VehicleTypesList");
		}

		private bool VehicleTypeExists(int id)
		{
			return _context.VehicleTypes.Any(e => e.Id == id);
		}

	}

	
}
