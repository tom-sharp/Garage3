using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{

	public class Pricing
	{
		public int GetPrice(Vehicle vehicle, int membershiplevel)
		{
			int minutes = (int)(vehicle.CheckOutTime - vehicle.CheckInTime).TotalMinutes;
			if (vehicle.VehicleType != null) { 
				return GetMemberShipPrice(vehicle.VehicleType.Size, minutes, membershiplevel);
			}
			// if vehicle type not provide - fall back to vehicle size 3
			return GetMemberShipPrice(3, minutes, membershiplevel);
		}

		// Pricemodels

		private int GetMemberShipPrice(int vehiclesize, int minutes, int membershiplevel)
		{
			double amount = 0;
			double minuterate = 0.33;	// per slotsize unit
			switch (membershiplevel)
			{
				case 0:                                     // Basic membership
					amount = minuterate * minutes * vehiclesize;
					break;
				case 1:                                     // Level 1
					amount = minuterate * minutes * vehiclesize;
					amount *= 0.95; // 5% discount
					break;
				default:                                    // Invalid membership - fall down to basic
					amount = minuterate * minutes * vehiclesize;
					break;
			}
			return (int)amount;
		}


	}
}
