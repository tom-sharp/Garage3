using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class VehicleType
	{
		public int Id { get; set; }

		[Range(1, 6)]
		public int Size { get; set; }       // size in slots

		[MinLength(2), MaxLength(50)]
		public string Name { get; set; }    // Boat, car..

		public ICollection<Vehicle> Vehicle{ get; set; }

	}
}
