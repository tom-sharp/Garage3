using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class SlotViewModel
	{
		public SlotViewModel(){	}
		public SlotViewModel(Slot slot)
		{
			this.GarageId = slot.GarageId;
			throw new ApplicationException("Automapper in ctor is not yet ready");
		}

		public int Id { get; set; }             // this is only used with DB reference and not guaranteed på be continous

		public int GarageId { get; set; }
		public int No { get; set; }         // Slot numbering for the garage (parking space number)

		string Name { get; set; }			// parking slot name ()
		public int InUse { get; set; }          // -1 = Reserved, 0= Free, 1..n = InUse(0..slotsize), 

		public ICollection<Vehicle> Vehicles { get; set; }

		public Garage Garage { get; set; }

	}
}
