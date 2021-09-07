using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class ParkedVehicle
	{
		public int Id { get; set; }


		public int VehicleId { get; set; }
		public Vehicle Vehicle { get; set; }

		[DataType(DataType.DateTime)]
		[Display(Name = "Check-in Time")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]

		public DateTime CheckInTime { get; set; }
		[DataType(DataType.DateTime)]
		[Display(Name = "Check-in Time")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]

		public DateTime CheckOutTime { get; set; }

		public int PersonId { get; set; }
		public Person Person { get; set; }

		public int ChargeAmount { get; set; }

		public VehicleState State { get; set; }

		public ICollection<Slot> Slots { get; set; }


	}
}
