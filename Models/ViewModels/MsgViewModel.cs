using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class MsgViewModel
	{
		public MsgViewModel() {	}
		
		public MsgViewModel(string msg, string error = "") {
			this.Msg = msg;
			this.Error = error;
		}

		public string Msg { get; set; }
		public string Error { get; set; }

	}
}
