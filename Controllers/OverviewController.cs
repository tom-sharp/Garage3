using Garage3.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Controllers
{
	public class OverviewController : Controller
	{
		private readonly Garage3contextNoTracking dbReadOnly;

		public OverviewController(Garage3contextNoTracking context)
		{
			dbReadOnly = context;
		}


		public IActionResult Index()
		{
			return View();
		}
	}
}
