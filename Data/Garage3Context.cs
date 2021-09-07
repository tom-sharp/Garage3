using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage3.Models;
using Garage3.Models.ViewModels;

namespace Garage3.Data
{
    public class Garage3Context : DbContext
    {
        public Garage3Context (DbContextOptions<Garage3Context> options)
            : base(options)
        {
        }

		public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
		public DbSet<Slot> Slots { get; set; }
		public DbSet<Garage> Garages { get; set; }

	}
}
