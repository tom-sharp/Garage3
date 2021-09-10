using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class SearchMembersViewModel
	{
		public SearchMembersViewModel(Person p)
		{
			this.Id = p.Id;
			this.FirstName = p.FirstName;
			this.LastName = p.LastName;
			this.Email = p.Email;
			this.MemberType = p.MemberType;
			this.VehiclesOwned = p.Vehicles.Count;
		}

		public int Id { get; set; }

		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Membership level")]
		public int MemberType { get; set; }

		[Display(Name = "Vehicles owned")]
		public int VehiclesOwned { get; set; }

	}
}
