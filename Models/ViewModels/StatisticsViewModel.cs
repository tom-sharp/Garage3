using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
	public class StatisticsViewModel
	{

		/*
			● Visa översiktlig statistik för fordonen som just nu är parkerade i garaget.
			● Hur många fordon finns av varje typ?
			● Hur många hjul finns det totalt i garaget just nu?
			● Hur mycket har fordonen som står i garaget just nu genererat i intäkt
			● Annan intressant statistik ni kan komma på.
		
			Visa en översikt av vilka platser som är lediga/upptagna just nu.
			Ni väljer själva var ni vill presentera den här informationen.
			Eller om ni väljer att visa upp den på flera ställen.
		 */
		public int NoOfCars { get; set; }
        public int NoOfMotorCycles { get; set; }
        public int NoOfTrucks { get; set; }
		public int NoOfBuses { get; set; }
		public int NoOfBoats { get; set; }
		public int NoTotalVehicles { get; set; }
		public int NoOfWheels { get; set; }

        public int NoOfBlack { get; set; }
        public int NoOfBlue { get; set; }
        public int NoOfGreen { get; set; }
        public int NoOfRed { get; set; }
        public int NoOfSilver { get; set; }
        public int NoOfUnknown { get; set; }
        public int NoOfWhite { get; set; }
        public int NoOfYellow { get; set; }

        public int GarageTurnOver { get; set; }
		public int AvgParkingTime { get; set; }

    }
}
