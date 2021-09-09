using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Data
{
	public class Garage3contextNoTracking : Garage3Context
	{

		public Garage3contextNoTracking(DbContextOptions<Garage3Context> options) : base(options)
		{
			base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

	}
}
