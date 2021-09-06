using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class Garage
	{
		public int Id { get; set; }
		public int Size { get; set; }
		public int SlotSize { get; set; }
		public string Name { get; set; }

		public ICollection<Slot> Slots { get; set; }

	}
}

