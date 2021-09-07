using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class VehicleType
	{
		public int Id { get; set; }
		public int Size { get; set; }		// size in slots
		public string Name { get; set; }    // Boat, car..

		public ICollection<Vehicle> Vehicle{ get; set; }

	}
}
