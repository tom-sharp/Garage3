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
		[DataType(DataType.DateTime)]
		[Display(Name = "Check-in Time")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
		public DateTime CheckInTime { get; set; }
		[DataType(DataType.DateTime)]
		[Display(Name = "Check-out Time")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
		public DateTime CheckOuTime { get; set; }
		public int State { get; set; }
		public VehicleColor Color { get; set; }
        public string LicensePlate { get; set; }

        public int VehicleTypeId { get; set; }

		public VehicleType VehicleType { get; set; }

		public int PersonId { get; set; }
		public Person Person { get; set; }

		public ICollection<Slot> Slots { get; set; }

	}
}
