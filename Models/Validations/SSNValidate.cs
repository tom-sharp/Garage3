using System;
using System.ComponentModel.DataAnnotations;
using static Garage3.Models.Validations.PersonIdentity;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Garage3.Models.Validations
{
    public class SSNValidate : ValidationAttribute
    {
        private  Garage3Context db;
        private int MinYearsOld { get; set; }
        private int Id { get; set; }

        public SSNValidate(int minYearsOld)
        {
            MinYearsOld = minYearsOld;
        }

      
        protected override ValidationResult IsValid(object objectSSN, ValidationContext validationContext)
        {
            // Get handle to the Database _context
            db = (Garage3Context)validationContext.GetService(typeof(Garage3Context));

//			var x = validationContext.ObjectInstance as Person;
//			if (x == null) { var x = validationContext.ObjectInstance as PersonsViewModel; }

			if (validationContext.ObjectInstance as Person != null)
			{
				Id = ((Person)validationContext.ObjectInstance).Id;
			}
			else { Id = ((PersonsViewModel)validationContext.ObjectInstance).Id; }


//			Id = ((Person)validationContext.ObjectInstance).Id;

            string SSN = objectSSN.ToString();

            if (SSN.Length != 13) return new ValidationResult($"Invalid SSN. Format SSN as yyyymmdd-xxxx");

            PersonIdentity pid = PersonIdentity.Parse(SSN);
            if (pid.Age < MinYearsOld)
            {
                return new ValidationResult($"Must be {MinYearsOld} years old to park.");
            }

            if (pid.IdentityType == PersonIdentityType.Personnummer)
            {
                var result = IsUniqueSSN(SSN);

                if (result)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"Person with SSN {SSN} is already registered.");
                }
            }
            else
            {
                // SSN is not a valid PersonNr, can be a SamordningsNr or ReservNr used at Hospitals
                // = Not allowed for now in this Garage App
                return new ValidationResult($"Invalid SSN. Format SSN as yyyymmdd-xxxx");
            }
        }

        // SSN should only be allowed to be registered once
        private bool IsUniqueSSN(string SSN)
        {
            return ! db.Persons.Any(p => p.SSN == SSN && p.Id != Id); // Dimitris Way in University / StundentsController
        }

    }

}