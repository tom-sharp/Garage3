using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class Person
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string SSN { get; set; } // Social Security Number
		public DateTime BirthDate  { get; set; }
		public int MemberType { get; set; }

		public ICollection<Vehicle> Vehicles { get; set; }

	}
}
