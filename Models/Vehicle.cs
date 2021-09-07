using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class Vehicle
	{
		public int Id { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }

		public VehicleColor Color { get; set; }
        public string LicensePlate { get; set; }


        public int VehicleTypeId { get; set; }
		public VehicleType VehicleType { get; set; }

		public int PersonId { get; set; }
		public Person Person { get; set; }


	}
}
