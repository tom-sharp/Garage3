using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class VehicleTypesViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }    // Boat, car..
		public int Size { get; set; }		// size in slots
		public ICollection<Vehicle> Vehicle{ get; set; }

	}
}
