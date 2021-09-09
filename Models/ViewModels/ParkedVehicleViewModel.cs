using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class ParkedVehicleViewModel
	{
		public ParkedVehicleViewModel(Vehicle v) {
			this.VehicleType = v.VehicleType.Name;
			this.LicensePlate = v.LicensePlate;
		}

		// Ägare, Medlemskap, Fordonstyp, RegNum och ParkTid som minimum

		[Display(Name = "Vehicle type")]
		public string VehicleType { get; set; }

		[Display(Name = "License plate")]
		public string LicensePlate { get; set; }

		[Display(Name = "Parked time")]
		public string ParkedTime { get; set; }

		[Display(Name = "Owner")]
		public string Owner { get; set; }

		[Display(Name = "Membership")]
		public string Membership { get; set; }


	}
}
