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
			return GetMemberShipPrice(minutes, membershiplevel);
		}

		// Pricemodels

		private int GetMemberShipPrice(int minutes, int membershiplevel)
		{
			double amount = 0;
			double minuterate = 0.99;
			switch (membershiplevel)
			{
				case 0:                                     // Basic membership
					amount = minuterate * minutes;
					break;
				case 1:                                     // Level 1
					amount = minuterate * minutes;
					amount *= 0.95; // 5% discount
					break;
				default:                                    // Invalid membership - fall down to basic
					amount = minuterate * minutes;
					break;
			}
			return (int)amount;
		}


	}
}
