using Garage3.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models
{
	public class PersonsViewModel // : IValidatableObject
	{
		public int Id { get; set; }

		[Required]
		[MinLength(2), MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MinLength(2), MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		//[SSNValidate(18)]
		public string SSN { get; set; }

		[Required]
		public string BirthDate { get; set; }

		[Required]
		[Range(1,100)]
		public int MemberType { get; set; }

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
			if (this.FirstName == this.LastName) return false;

			if ((this.Email.Length < 2) || (this.Email.Length > 50)) return false;
			if ((this.SSN.Length < 2) || (this.SSN.Length > 50)) return false;
			
			if ((this.MemberType < 0) || (this.MemberType > 10)) return false;
			return true;
		}

    //    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //    {
    //       if(FirstName == LastName)
    //        {
				//yield return new ValidationResult($"{FirstName} cant be same as lastname", new[] { nameof(FirstName) });
    //        }
    //    }
    }

}
