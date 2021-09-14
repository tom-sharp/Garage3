using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class CheckinInitViewModel
    {
        public int id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string PersonEmail { get; set; }

    }
}
