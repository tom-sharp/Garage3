using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class CheckInViewModel
    {
       
        //public IEnumerable<SelectListItem> Garages { get; set; }
        //public Garage? Gar { get; set; }
       
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public VehicleColor Color { get; set; }
        [Display(Name = "License Plate Number")]
        public string LicensePlate { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Check-in Time")]
        public DateTime CheckInTime { get; set; }
        public int SlotId { get; set; }
        public int PersonId { get; set; }
       

    }
}
