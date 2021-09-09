using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class GaragesViewModel
	{
		public GaragesViewModel() { }
		public GaragesViewModel(Garage garage)
		{
			this.Id = garage.Id;
			this.Size = garage.Size;
			this.SlotSize = garage.SlotSize;
			this.Name = garage.Name;
			this.FreeSlots = 0;
		}
		public bool Validate() {
			if (this.Name == null) return false;
			if ((this.Name.Length  < 4) || (this.Name.Length > 50)) return false;
			if ((this.Size < 0) || (this.Size > 10000)) return false;
			if ((this.SlotSize < 1) || (this.SlotSize > 10)) return false;
			return true;
		}
		public int Id { get; set; }

		[Display(Name = "Number of parking slots (Size)")]
		[Range(0,10000)]
		public int Size { get; set; }

		[Display(Name = "Size of a parking slot (SlotSize)")]
		[Range(1,10)]
		public int SlotSize { get; set; }

		[Display(Name = "Garage name")]
		[MinLength(4),MaxLength(50)]
		public string Name { get; set; }

		[Display(Name = "Free slots")]
		public int FreeSlots { get; set; }

	}
}
