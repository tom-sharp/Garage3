﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models;
using Garage3.Models.ViewModels;

namespace Garage3.Controllers
{
	public class VehiclesController : Controller
	{
		private readonly Garage3Context _context;

		public VehiclesController(Garage3Context context)
		{
			_context = context;
		}

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var garage3Context = _context.Vehicles.Include(v => v.Person);
            return View(await garage3Context.ToListAsync());
        }

		// GET: Vehicles/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehicle = await _context.Vehicles
				.Include(v => v.Person)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (vehicle == null)
			{
				return NotFound();
			}

			return View(vehicle);
		}

        public IActionResult CheckIn()
        {
            return View();
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Id");
            return View();
        }

		// POST: Vehicles/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Make,Model,CheckInTime,CheckOuTime,State,Color,VehicleTypeId,PersonId")] Vehicle vehicle)
		{
			if (ModelState.IsValid)
			{
				_context.Add(vehicle);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Id", vehicle.PersonId);
			return View(vehicle);
		}

		// GET: Vehicles/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehicle = await _context.Vehicles.FindAsync(id);
			if (vehicle == null)
			{
				return NotFound();
			}
			ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Id", vehicle.PersonId);
			return View(vehicle);
		}

		// POST: Vehicles/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,CheckInTime,CheckOuTime,State,Color,VehicleTypeId,PersonId")] Vehicle vehicle)
		{
			if (id != vehicle.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(vehicle);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!VehicleExists(vehicle.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Id", vehicle.PersonId);
			return View(vehicle);
		}

		// GET: Vehicles/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehicle = await _context.Vehicles
				.Include(v => v.Person)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (vehicle == null)
			{
				return NotFound();
			}

			return View(vehicle);
		}

		// POST: Vehicles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var vehicle = await _context.Vehicles.FindAsync(id);
			_context.Vehicles.Remove(vehicle);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool VehicleExists(int id)
		{
			return _context.Vehicles.Any(e => e.Id == id);
		}

		public async Task<IActionResult> TestPark()
		{
			var garage = await _context.Garages.FirstOrDefaultAsync(g=> g.Size > 0);
			if (garage == null) return View("_Msg", new MsgViewModel("Could not find any garage to check in to"));

			var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.State == VehicleState.UnParked);
			if (vehicle == null) return View("_Msg", new MsgViewModel("Could not find any Vehicle to check in"));

			if (await ParkVehicle(garage.Id, vehicle.Id)) {
				return View("_Msg", new MsgViewModel("Success check-in (test)"));
			}
			return View("_Msg", new MsgViewModel("failed check-in (test)"));
		}


		private async Task<bool> ParkVehicle(int garageid, int vehicleid)
		{
			if ((garageid <= 0) || (vehicleid <= 0)) return false;
			var vehicle = _context.Vehicles.Include(v=> v.VehicleType).FirstOrDefault(v=> v.Id == vehicleid);
			if ((vehicle == null) || (vehicle.State == VehicleState.Parked)) return false;
			var garage = await _context.Garages.FindAsync(garageid);
			if (garage == null) return false;

			// get slots for garage
			int slotsize = garage.SlotSize;
			var garageslots = await _context.Slots.Include(s=> s.Vehicles).Where(s => s.GarageId == garage.Id).OrderBy(s=> s.No).ToListAsync();
			foreach (var slot in garageslots) {
				if ((slot.InUse < 0) || (slot.InUse > slotsize)) throw new ApplicationException($"DEBUG::ParkVehicle - InUse Integrigy test failed - (SlitId: {slot.Id} InUse={slot.InUse})");
			}
			int slotcount = garageslots.Count();
			int slotsizeRequired = vehicle.VehicleType.Size;    // size of vechicle
			int slotsizeAccumulated = 0;
			var SlotsAccumulated = new List<Slot>();
			int counter = 0;
			while (slotsizeAccumulated < slotsizeRequired) {
				if (counter == slotcount) break;
				if (slotsizeRequired >= slotsize) {
					// Vehicle need at least a full slot
					if (garageslots[counter].InUse != 0) {
						counter++;
						slotsizeAccumulated = 0;
						if (SlotsAccumulated.Count > 0) SlotsAccumulated.Clear();
					}
					else {
						slotsizeAccumulated += slotsize;
						SlotsAccumulated.Add(garageslots[counter]);
						counter++;
					}
				}
				else {
					// Vehicle do not need a full slot
					if ((garageslots[counter].InUse < slotsize) && (slotsizeRequired <= (slotsize - garageslots[counter].InUse))) {
						slotsizeAccumulated = slotsizeRequired;
						SlotsAccumulated.Add(garageslots[counter]);
						break;
					}
					counter++;
				}
			}

			if (slotsizeRequired <= slotsizeAccumulated) {
				// Managed to get enough slots to park
				vehicle.CheckInTime = DateTime.Now;
				foreach (var slot in SlotsAccumulated) {
					slot.Vehicles.Add(vehicle);
					if (slotsizeRequired <= slotsize)
					{
						slot.InUse += slotsizeRequired; // a fraction of the slot is used
						slotsizeRequired = 0;
					}
					else {
						slot.InUse = slotsize;          // a full slot is used
						slotsizeRequired -= slotsize;
					}
				}
				if (slotsizeRequired != 0) throw new ApplicationException("DEBUG::Park - Slot Integrity check failed - left over slots");
				_context.UpdateRange(SlotsAccumulated);
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException){
					return false;
				}
				return true;
			}
			return false;
		}

		private async Task<bool> UnParkVehicle(int vehicleid)
		{
			if (vehicleid <= 0) return false;
			var vehicle = _context.Vehicles.Include(v => v.Slots).FirstOrDefault(v => v.Id == vehicleid);
			if ((vehicle == null) || (vehicle.State != VehicleState.Parked)) return false;


			// ADD LOGIC TO FREE PARKING SLOTS

			vehicle.State = (int)VehicleState.UnParked;

			return true;
		}
	}
}

