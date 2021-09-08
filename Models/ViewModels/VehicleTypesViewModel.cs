using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class VehicleTypesViewModel
	{
		public int Id { get; set; }

		[MinLength(2), MaxLength(50)]
		public string Name { get; set; }    // Boat, car..

		[Range(0, 10)]
		public int Size { get; set; }		// size in slots
		public ICollection<Vehicle> Vehicle{ get; set; }

		public bool Validate()
		{
			if (this.Name == null) return false;
			if ((this.Name.Length < 2) || (this.Name.Length > 50)) return false;
			if ((this.Size < 0) || (this.Size > 10)) return false;
			return true;
		}
	}
}
