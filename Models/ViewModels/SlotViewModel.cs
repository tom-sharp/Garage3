using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class SlotViewModel
	{
		public SlotViewModel(){	}
		public SlotViewModel(Slot slot)
		{
			if (slot != null) {
				this.ParkingSlot = $"{slot.GarageId}:{slot.No}";
				this.InUse = slot.InUse;
				this.ParkedVehicles = "";
				if (slot.Vehicles != null) {
					foreach (var v in slot.Vehicles) {
						this.ParkedVehicles += $"{v.LicensePlate}, ";
					}
				}
			}
		}
		[Display (Name = "Garage:Slot")]
		public string ParkingSlot { get; set; }     // Garage+SlotNumber
		[Display(Name = "In Use")]
		public int InUse { get; set; }          // -1 = Reserved, 0= Free, 1..n = InUse(0..slotsize),
		[Display(Name = "Parked Vehicles")]
		public string ParkedVehicles { get; set; }	// list with regno

	}
}
