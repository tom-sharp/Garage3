using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class SearchVehicleViewModel
	{
		public SearchVehicleViewModel(Vehicle v)
		{
			this.Id = v.Id;
			this.VehicleType = v.VehicleType.Name;
			this.LicensePlate = v.LicensePlate;
			this.Checkout = false;
			if (v.State == VehicleState.Parked) { this.ParkedTime = (DateTime.Now - v.CheckInTime).TimedString(); this.Checkout = true; }
			else if (v.State == VehicleState.UnParked) this.ParkedTime = (v.CheckOutTime - v.CheckInTime).TimedString();
			else this.ParkedTime = "";
			this.Owner = $"{v.Person.FirstName} {v.Person.LastName}";
			this.MembershipLevel = v.Person.MemberType;
			this.ParkedAt = v.ParkedAt();
		}

		// Ägare, Medlemskap, Fordonstyp, RegNum och ParkTid som minimum

		public int Id { get; set; }

		public bool Checkout { get; set; }

		[Display(Name = "Vehicle type")]
		public string VehicleType { get; set; }

		[Display(Name = "License plate")]
		public string LicensePlate { get; set; }

		[Display(Name = "Parked time")]
		public string ParkedTime { get; set; }

		[Display(Name = "Parked at")]
		public string ParkedAt { get; set; }

		[Display(Name = "Owner")]
		public string Owner { get; set; }

		[Display(Name = "Membership level")]
		public int MembershipLevel { get; set; }
	}
}
