using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{

	// Garage size define how many slots a garage have. (ex 100)
	// Garage slotsize define how many fractions a slot can be diveded into (ex 3)
	// the same number of slots are created int slots table nombered from 1..size (property No)
	// InUse property indicate how many fractions of a slot is used (0 == free, 1 == 1/3, 3 == full, -1 == reserved)
	// GarageId, is the id of garage owning the slot and construct the unkik parking slotname: garageid:No (ex 1:34)
	// Vehicles is a list vehicles positioned at that slot

	public class Slot
	{
		//public int Id { get; set; }
		//public int State { get; set; }          // 0= Free, 1= InUse, 2 = Reserved

		//public int VehicleId { get; set; }      // this makes a vehicle mandatory on a slot
		//public Vehicle Vehicle { get; set; }

		//public int GarageId { get; set; }
		//public Garage Garage { get; set; }

		public int Id { get; set; }             // this is only used with DB reference and not guaranteed på be continous

		public int No { get; set; }         // Slot numbering for the garage (parking space number)
		public int InUse { get; set; }          // -1 = Reserved, 0= Free, 1..n = InUse(0..slotsize), 

		public ICollection<ParkedVehicle> ParkedVehicles { get; set; }

		public int GarageId { get; set; }
		public Garage Garage { get; set; }
	}
}
