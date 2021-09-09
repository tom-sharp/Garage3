using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Garage3.Models.Validations.PersonIdentity;

using Microsoft.EntityFrameworkCore;
using Garage3.Data;

namespace Garage3.Models.Validations
{
    public class SSNValidate : ValidationAttribute
    {

        private  Garage3Context db;// = new Garage3Context();

        public SSNValidate(int minYearsOld)
        {
          
            MinYearsOld = minYearsOld;
        }

        //public SSNValidate(int minYearsOld)
        //{
        //   MinYearsOld = minYearsOld;
        //}
        

        private int MinYearsOld { get; set;  }

        protected override ValidationResult IsValid(object objectSSN, ValidationContext validationContext)
        {
            db = (Garage3Context)validationContext.GetService(typeof(Garage3Context));
            
            string SSN = objectSSN.ToString();

            if (SSN.Length != 13) return new ValidationResult($"Invalid SSN. Format SSN as yyyymmdd-xxxx");

            PersonIdentity pid = PersonIdentity.Parse(SSN);
            if (pid.Age < MinYearsOld)
            {
                return new ValidationResult($"Must be {MinYearsOld} years old to park.");
            }

            if (pid.IdentityType == PersonIdentityType.Personnummer)
            {
                if (IsUniqueSSN(SSN))
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
                // SSN is not a valid PersonNr, can be a SamordningsNr or ReservNr used at Hospitals = Not allowed for now in this Garage App
                return new ValidationResult($"Invalid SSN. Format SSN as yyyymmdd-xxxx");
            }
        }

        private Boolean IsUniqueSSN(string SSN)
        {
            var person = db.Persons.FirstOrDefaultAsync(p => p.SSN == SSN);
            if (person == null) return true;

            return false;   
        }

    }

}