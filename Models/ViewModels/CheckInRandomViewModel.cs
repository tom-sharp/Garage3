using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class CheckInRandomViewModel
	{

		[Remote("IsFreeSlots", "Vehicles", HttpMethod = "POST", ErrorMessage = "Garage has no free slots")]
		public int GarageId { get; set; }


		
//		[Range(1,100)]
		[Remote("IsRandomVehicles", "Vehicles", HttpMethod = "POST", ErrorMessage = "Vehicles must be 1-100")]
		public int NumVehicles { get; set; }

		public IEnumerable<SelectListItem> GarageList { get; set; }

		internal bool IsValid()
		{
			if ((this.NumVehicles <= 0) || (this.NumVehicles > 100)) return false;
			if (this.GarageId <= 0) return false;
			return true;
		}
	}
}
