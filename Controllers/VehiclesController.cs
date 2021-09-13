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

		public  IActionResult CheckIn(int pid)
		{
			ViewBag.pid = pid;
			var model = new CheckInViewModel
            {
				VehicleTypes = GetVehicleTypeAsync().Result,
				GarageList=GetGarageListAsync().Result
            };
          
            return View(model);
			
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CheckIn(CheckInViewModel viewModel)
		{
			var veh = await _context.Vehicles.Where(v => v.LicensePlate == viewModel.LicensePlate && v.State == VehicleState.Parked).ToListAsync();
			
			if(veh.Count()>0)
			{
				TempData["Message1"] = "Car with License Plate Already Parked";
				return RedirectToAction(nameof(CheckInInitial));
			}
			var chkowner = await _context.Vehicles.Where(v => v.LicensePlate == viewModel.LicensePlate && v.PersonId != viewModel.PersonId).ToListAsync();
			if (chkowner.Count() > 0)
			{
				TempData["Message1"] = "Car with License Plate Is Registered With Another Owner";
				return RedirectToAction(nameof(CheckInInitial));
			}
			var vehicle = new Vehicle
			{	
				LicensePlate=viewModel.LicensePlate,
				Make=viewModel.Make,
				Color=viewModel.Color,
				Model=viewModel.Model,
				VehicleTypeId=viewModel.VehicleTypeId,
				PersonId= viewModel.PersonId,
				State= VehicleState.TryPark
			};

			_context.Add(vehicle);
			await _context.SaveChangesAsync();

			var vehicle1 = _context.Vehicles.Select(p => new CheckInViewModel
			{
				Id = p.Id,
				LicensePlate=p.LicensePlate			});

			var model1 = vehicle1.Where(p => p.LicensePlate==viewModel.LicensePlate).
									OrderBy(m=>m.Id).Last();
			int garageId = viewModel.GarageId;
			int vehicleId = model1.Id;
			bool allowPark = await ParkVehicle(garageId, vehicleId);
			
			if (allowPark) 
			{

				TempData["Message1"] = "Is Parked";
				return RedirectToAction(nameof(CheckInReciept), new { id = vehicleId });

			}
            else
            {
				TempData["Message1"] = "Cannot Park.. Not Enough Space";
				return RedirectToAction(nameof(CheckInInitial));
			}


				

		}

		
		public async Task<IActionResult> CheckInReciept(int id)
		{
			var receiptData = _context.Vehicles.Select(p => new CheckInConfirmReceiptViewModel
            {
				Id=p.Id,
                LicensePlate = p.LicensePlate,
                CheckInTime = p.CheckInTime,
                Color = p.Color,
                Make = p.Make,
				VechileTypeName=p.VehicleType.Name

            });

			var model1 = receiptData.Where(p => p.Id == id).FirstOrDefault();
            return View(model1);
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


		//		public async Task<IActionResult> CheckInRandom(int? garageid, int? numvehicles)
		public async Task<IActionResult> CheckInRandom(CheckInRandomViewModel ch) {

			int garageid = ch.GarageId;
			int numvehicles = ch.NumVehicles;

			if ((!ModelState.IsValid) || (!ch.IsValid()))
			{
				var model = new CheckInRandomViewModel
				{
					GarageList = GetGarageListAsync().Result
				};
				return View(model);
			}


//			if ((garageid == null) || (garageid <= 0)) return View(await model.ToListAsync());
//			if ((num == null) || (num <= 0)) return View(await model.ToListAsync());

			var garage = await _context.Garages.FirstOrDefaultAsync(g => g.Id == garageid);
			if (garage == null) return View("_Msg", new MsgViewModel($"Could not find garage (id = {garageid}) to check in to"));

			var fake = new FakeRepository();
			List <VehicleType> VT = await _context.VehicleTypes.ToListAsync();
			int checkinNum = numvehicles > 100 ? 100 : (int)numvehicles;
			int counter = 0;
			int personcounter = 0;
			Person newperson;
			Vehicle newvehicle;
			while (counter < checkinNum) {

				// first try create person
				newperson = new Person() { LastName = fake.GetRndLastName(), FirstName = fake.GetRndFirstName(), SSN = fake.GetRndSSN() };
				newperson.BirthDate = fake.GetRndBirthDate(newperson.SSN);
				newperson.Email = fake.GetRndEmail(newperson.FirstName, newperson.LastName);

				var res = await _context.Persons.FirstOrDefaultAsync(p => p.SSN == newperson.SSN || p.Email == newperson.Email);
				if (res == null)
				{
					// user dont exist - create it
					_context.Persons.Add(newperson);
					await _context.SaveChangesAsync();
					personcounter++;
				}
				else {
					// user exist - use that one 
					newperson = res;
				}

				while (true) {
					// create the vehicle
					newvehicle = new Vehicle() { LicensePlate = fake.GetRndLicensePlateNumber(), PersonId = newperson.Id, Make = fake.GetRndVehicleBrand(), State = VehicleState.TryPark, Model = "fake"  };
					newvehicle.VehicleTypeId = VT[fake.GetRndInt(0, VT.Count)].Id;
					var v = await _context.Vehicles.FirstOrDefaultAsync(v=> v.LicensePlate.ToLower() == newvehicle.LicensePlate.ToLower());

					if (v == null) {
						// does not exist - save it
						_context.Vehicles.Add(newvehicle);
						await _context.SaveChangesAsync();

						// Try To Park
						if (await ParkVehicle(garage.Id, newvehicle.Id))
						{
							// success
							counter++;
							break;
						}
						else {
							// failed - expected garage is full
							return View("_Msg", new MsgViewModel($" {personcounter} People was created. Was not able to Park more than {counter} Vehicles - possible garage is full"));
						}
					}
				}
			}
			return View("_Msg", new MsgViewModel($" {personcounter} People was created. {counter} Vehicles was parked"));
		}

		public IActionResult IsFreeSlots(int GarageId)
		{
			if (FreeSlots(GarageId).Result > 0) return Json(true);
			return Json(false);
		}

		public IActionResult IsRandomVehicles(int NumVehicles)
		{
			if ((NumVehicles > 0) && (NumVehicles <= 100)) return Json(true);
			return Json(false);
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

		public async Task<IActionResult> TestUnPark()
		{
			var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.State == VehicleState.Parked);
			if (vehicle == null) return View("_Msg", new MsgViewModel("Could not find any Vehicle to check out"));

			if (await UnParkVehicle(vehicle.Id))
			{
				return View("_Msg", new MsgViewModel("Success check-out (test)"));
			}
			return View("_Msg", new MsgViewModel("failed check-out (test)"));
		}


		public async Task<IActionResult> CheckOutTemp(int? vehicleid)
		{
			if (vehicleid == null) return RedirectToAction("CheckInInitial");
			var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleid);
			if (vehicle == null) return RedirectToAction("CheckInInitial");

			if (await UnParkVehicle(vehicle.Id))
			{
				return View("_Msg", new MsgViewModel($"{vehicle.LicensePlate} checked out, ParkingTime {(vehicle.CheckOutTime - vehicle.CheckInTime).TimedString()}, Cost SEK {vehicle.ChargeAmount} Kr"));
			}
			switch (vehicle.State) {
				case VehicleState.Parked: return View("_Msg", new MsgViewModel($"check out failed - vehicle still parked"));
				case VehicleState.UnParked: return View("_Msg", new MsgViewModel($"checkout failed vehicle already unparked"));
				case VehicleState.TryPark: return View("_Msg", new MsgViewModel($"checkout failed vehicle is not parked"));
			}
			return View("_Msg", new MsgViewModel($"Failed checkout, unhadeld stat property"));
		}


		private async Task<int> FreeSlots(int garageid) {
			if (garageid <= 0) return 0;
			var garage = await _context.Garages.FindAsync(garageid);
			if (garage == null) return 0;
			var garageslots = await _context.Slots.Where(s => s.GarageId == garage.Id).ToListAsync();
			int slotsize = garage.SlotSize;
			int counter = 0;
			foreach (var slot in garageslots)
			{
				if (slot.InUse == 0) counter++;
				else if ((slot.InUse < 0) || (slot.InUse > slotsize)) throw new ApplicationException($"DEBUG::FreeSlots - InUse Integrigy test failed - (SlotId: {slot.Id} InUse={slot.InUse})");
			}
			return counter;
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
				vehicle.State = VehicleState.Parked;
				foreach (var slot in SlotsAccumulated) {
					if ((slot.Vehicles != null) && (!slot.Vehicles.Contains(vehicle))) slot.Vehicles.Add(vehicle);		// Expect some DB inconsistence while in development state
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
			var vehicle = _context.Vehicles.Include(v => v.VehicleType).Include(v=> v.Person).Include(v => v.Slots).ThenInclude(s=> s.Garage).FirstOrDefault(v => v.Id == vehicleid);
			if ((vehicle == null) || (vehicle.State != VehicleState.Parked)) return false;
			vehicle.Slots = vehicle.Slots.OrderBy(s=> s.No).ToList();
			int parksize = vehicle.VehicleType.Size;
			foreach (var slot in vehicle.Slots) {
				if (parksize < slot.Garage.SlotSize)
				{
					// vehicle occupy smaller size than a full slot
					slot.InUse -= parksize;
					if ((slot.Vehicles != null) && (slot.Vehicles.Contains(vehicle))) slot.Vehicles.Remove(vehicle);	// Expect some DB inconsistence while in development state
					parksize = 0;
				}
				else {
					// vehicle occupy a full slot
					slot.InUse = 0;
					if ((slot.Vehicles != null) && (slot.Vehicles.Contains(vehicle))) slot.Vehicles.Remove(vehicle);    // Expect some DB inconsistence while in development state
					parksize -= slot.Garage.SlotSize;
				}
			}

			vehicle.CheckOutTime = DateTime.Now;
			vehicle.State = VehicleState.UnParked;
			var pricing = new Pricing();
			vehicle.ChargeAmount = pricing.GetPrice(vehicle, vehicle.Person.MemberType);

			_context.Update(vehicle);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return false;
			}
			return true;
		}


		public async Task<IActionResult> CheckLicensePlate(string LicensePlate)
		{
			// 

			var vehicleModel = await _context.Vehicles.Where(e => e.LicensePlate == LicensePlate).ToListAsync();

			if (vehicleModel.Count()>0)
			{
				return Json(true);
			}
			else
			{
				var vehicleModel1 = vehicleModel.Where(v => v.State == VehicleState.Parked);
				if (vehicleModel1.Count()>0)
				{
					return Json("Vehicle with license plate " + LicensePlate + " is already parked.");
				}
				else
				{
					return Json(true);
				}
			}

		}

		public async Task<IActionResult> CheckGarageSlots(int GarageId)
		{
			// 
			if (GarageId == 0)
			{
				return Json("please select garage");
			}
			else 
			{
				int freeSlots = await FreeSlots(GarageId);
				if (freeSlots! > 0)
				{
					return Json("Garage is full please choose another one");
				}
			
			}
			

			return Json(true);
						
		}
		}
}

