using Garage3.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	public class PersonsViewModel
	{
		public int Id { get; set; }

		[MinLength(2), MaxLength(50)]
		public string FirstName { get; set; }    // Boat, car..

		[MinLength(2), MaxLength(50)]
		public string LastName { get; set; }    // Boat, car..

		[EmailAddress]
		public string Email { get; set; }    // Boat, car..

		[SSNValidate(18)]
		public string SSN { get; set; }    // Boat, car..

		public string BirthDate { get; set; }    // Boat, car..

		[Range(1,100)]
		public int MemberType { get; set; }    // Boat, car..

		public ICollection<Vehicle> Vehicles{ get; set; }

		public bool Validate()
		{
			if (this.FirstName == null) return false;
			if (this.LastName == null) return false;
			if (this.Email == null) return false;
			if (this.SSN == null) return false;
			if (this.BirthDate == null) return false;
			
			if ((this.FirstName.Length < 2) || (this.FirstName.Length > 50)) return false;
			if ((this.LastName.Length < 2) || (this.LastName.Length > 50)) return false;
			if ((this.Email.Length < 2) || (this.Email.Length > 50)) return false;
			if ((this.SSN.Length < 2) || (this.SSN.Length > 50)) return false;
			//if (!IsValidSwedishPersonIdentificationNumber()) return false;
			if ((this.MemberType < 0) || (this.MemberType > 10)) return false;
			return true;
		}


		private bool IsValidSwedishPersonIdentificationNumber()
        {


			return true;
        }
	}
}
