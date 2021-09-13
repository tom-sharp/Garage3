using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class MemberDetailsViewModel
	{
	

		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		public string Email { get; set; }

		[Display(Name = "Social Security Number")]
		public string SSN { get; set; }

		public int MemberType { get; set; }
		
		public ICollection<Vehicle> VehicleList { get; set; }

	}
}
