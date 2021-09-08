using Garage3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Data
{
    public class SeedData
    {
        internal static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<Garage3Context>())
            {
                if (await db.Persons.AnyAsync()) return;

               

                var person = GetPersons();
                await db.AddRangeAsync(person);

              

                await db.SaveChangesAsync();
            }
        }

        private static List<Person> GetPersons()
        {
            var persons = new List<Person>();
            var person = new Person
            {
                FirstName = "john",
                LastName ="johansson",
                Email = "john@abc.com",
                SSN="19881123-1111",
                BirthDate= Convert.ToDateTime("1988-11-23"),
                MemberType=1
            };
            var person1 = new Person
            {
                FirstName = "ron",
                LastName = "johansson",
                Email = "ron@abc.com",
                SSN = "19821123-1101",
                BirthDate = Convert.ToDateTime("1982-11-23"),
                MemberType = 1
            };


            persons.Add(person);
            persons.Add(person1);

            return persons;

        }

    }
}
