using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class Slot
	{
		public int Id { get; set; }
		public int State { get; set; }			// 0= Free, 1= InUse, 2 = Reserved

		public int VehicleId { get; set; }		// this makes a vehicle mandatory on a slot
		public Vehicle Vehicle { get; set; }

		public int GarageId { get; set; }
		public Garage Garage { get; set; }
	}
}
