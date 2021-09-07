﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class Person
	{
		public int Id { get; set; }

		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		
		[Display(Name = "Social Security Number")]
		public string SSN { get; set; } // Social Security Number
		
		[DataType(DataType.Date)]
		public DateTime BirthDate  { get; set; }
		
		public int MemberType { get; set; }

		public ICollection<Vehicle> Vehicles { get; set; }
		public ICollection<ParkedVehicle> ParkedVehicles { get; set; }

	}
}
