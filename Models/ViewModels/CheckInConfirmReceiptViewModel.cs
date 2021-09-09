using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class CheckInConfirmReceiptViewModel
    {

        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string VechileTypeName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public VehicleColor Color { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Check-in Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CheckInTime { get; set; }
        


    }
}
