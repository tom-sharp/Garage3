using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class CheckInViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public VehicleColor Color { get; set; }
        public string LicensePlate { get; set; }

        public int GarageId { get; set; }
        public IEnumerable<SelectListItem> GarageList { get; set; }

      
        public int VehicleTypeId { get; set; }
        public IEnumerable<SelectListItem> VehicleTypes { get; set; }
      
        public int PersonId { get; set; }

        public DateTime CheckInTime { get; set; }
             
      

       
    }
}
