using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class SearchViewModel
	{
		public int VehicleTypeId { get; set; }
		public string Search { get; set; }
		public IEnumerable<VehicleType> VTypes	{ get; set; }
	}
}
