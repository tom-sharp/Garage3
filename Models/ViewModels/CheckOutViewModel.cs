using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewModels
{
	public class CheckOutViewModel
	{
		public string LicensePlate { get; set; }
		public string CheckoutTime { get; set; }
		public string CheckinTime { get; set; }
		public string ParkTime { get; set; }
		public string Amount { get; set; }
	}
}
