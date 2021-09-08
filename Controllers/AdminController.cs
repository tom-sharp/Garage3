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

		// List VehicleTypes
		public async Task<IActionResult> VehicleTypesList()
		{
			var model = _context.VehicleTypes.OrderBy(v => v.Name).Select(v => new VehicleTypesViewModel());
			return View(await model.ToListAsync());

		}


	}

	
}
