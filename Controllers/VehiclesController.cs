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

        public IActionResult CheckInInitial()
        {
            return View();
        }
		[HttpPost]
		public IActionResult CheckInInitial(string email)
        {
			var model = _context.Persons.Select(p => new CheckinInitViewModel
			{
				id=p.Id,
				PersonEmail=p.Email
			});



			var model1 = model.Where(p => p.PersonEmail == email).FirstOrDefault();

			if (model1 == null)
			{
				ViewBag.Message = "notexits";
				return View();
			}
			else
            {
				ViewBag.Message = "Email exits..";
				return RedirectToAction(nameof(CheckIn),new { pid=model1.id});
			}
				
        }

		public  IActionResult CheckIn(int? pid)
		{
            var model = new CheckInViewModel
            {
				VehicleTypes = GetVehicleTypeAsync().Result,
				GarageList=GetGarageListAsync().Result
            };
          
            return View(model);
			
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CheckIn(CheckInViewModel model)
		{

			// 1. retirve infoormation about the vehicle 
			// user input data about vehicle ?   ||  retived from db ?


			

			// FreeSlots(garageid) - Returns number of free slots at that garage

			// 2 create a new vehicle and save it to DB as STATE UnParked
			// 

			// 3. CAll Park (garageid, vehicleid)
			// retrives the vehicle from DB, chec for slots
			// if its' abble to park, update state to PARKED and set checkin time
			// return TRUE if not slots avaiable DO NOTHING STE will still be UNPARKED return FALSE

			// 4. send message yto user about some success or failure

			// other choice to choose other garage ?


			//model.CheckInTime = DateTime.Now;
			//if(ParkVehicle(model.GarageId,))
			
			//var vehicle= new Vehicle

			//if (ModelState.IsValid)
			//{
			//	_context.Add(vehicle);
			//	await _context.SaveChangesAsync();
			//	return RedirectToAction(nameof(Index));
			//}
			//ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Id", vehicle.PersonId);
			//return View(vehicle);

			return RedirectToAction(nameof(Index));
		}


		//TODO: place this in its own Service-class
		private async Task<IEnumerable<SelectListItem>> GetVehicleTypeAsync()
		{
			return await _context.VehicleTypes
						.Select(g => new SelectListItem
						{
							Text = g.Name.ToString(),
                            Value = g.Id.ToString()
						})
						.ToListAsync();
		}

		private async Task<IEnumerable<SelectListItem>> GetGarageListAsync()
		{
			return await _context.Garages
						.Select(g => new SelectListItem
						{
							Text = g.Name.ToString(),
							Value = g.Id.ToString()
						})
						.ToListAsync();
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



		private async Task<bool> ParkVehicle(int garageid, int vehicleid)
		{
			if ((garageid <= 0) || (vehicleid <= 0)) return false;
			var vehicle = await _context.Vehicles.FindAsync(vehicleid);
			if (vehicle == null) return false;
			var garage = await _context.Garages.FindAsync(garageid);
			if (garage == null) return false;

			// succesful get garage & vehicle
			//			if (pv == null) return false;
			//			if (pv.State != (int)VehicleState.UnParked) return false;

			// ADD LOGIC TO GET PARKING SLOTS

			//vehicle.slots.Add(slots)
			//slot.add(vehicle)

			//VehicleSlot


			//			pv.State = (int)VehicleState.Parked;
			//			_context.Vehicles.Update(pv);

			return true;
		}

		private async Task<bool> UnParkVehicle(int vehicleid)
		{
			if (vehicleid <= 0) return false;
			var vehicle = await _context.Vehicles.FindAsync(vehicleid);
			if (vehicle == null) return false;

			//			if (vehicle.State != (int)VehicleState.Parked) return;

			// ADD LOGIC TO FREE PARKING SLOTS

			//			vehicle.State = (int)VehicleState.UnParked;
			return true;
		}
	}
}

