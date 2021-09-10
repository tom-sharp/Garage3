using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
		}

		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<Person> Persons { get; set; }
		public DbSet<VehicleType> VehicleTypes { get; set; }
		public DbSet<Slot> Slots { get; set; }
		public DbSet<Garage> Garages { get; set; }
		public DbSet<Garage3.Models.ViewModels.CheckInConfirmReceiptViewModel> CheckInConfirmReceiptViewModel { get; set; }
		

	}
}
