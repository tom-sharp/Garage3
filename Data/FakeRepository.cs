using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Data
{
	public class FakeRepository 
	{
		public int GetRndInt(int min, int max)
		{
			return rnd.Next(minValue: min, maxValue: max);
		}
		public string GetRndSSN() {
			return SSNRepo[rnd.Next(minValue: 0, maxValue: SSNRepo.Length)].ToString().Insert(8,"-");
		}
		public DateTime GetRndBirthDate(string SSN = null)
		{
			string ssn = SSN == null ? this.GetRndSSN() : SSN;
			if (ssn.Length <= 12) ssn = this.GetRndSSN();
			int y = Int32.Parse(ssn.Substring(0, 4));
			int m = Int32.Parse(ssn.Substring(4, 2));
			int d = Int32.Parse(ssn.Substring(6, 2));
			return new DateTime(y,m,d);
		}

		public string GetRndFirstName()
		{
			return FirstNameRepo[rnd.Next(minValue: 0, maxValue: FirstNameRepo.Length)];
		}
		public string GetRndLastName()
		{
			return LastNameRepo[rnd.Next(minValue: 0, maxValue: LastNameRepo.Length)];
		}

		public string GetRndEmailDomain() { 
			return MailDomainRepo[rnd.Next(minValue: 0, maxValue: MailDomainRepo.Length)];
		}
		public string GetRndEmail(string firstname = null, string lastname = null) {
			string ret = "";
			if ((firstname != null) || (lastname != null))
			{
				if (firstname != null) ret = firstname;
				if ((lastname != null) && (firstname != null)) ret += $".{lastname}";
				else ret = lastname;
			}
			else { 
				ret = $"{this.GetRndFirstName()}.{this.GetRndLastName()}";
			}
			ret += GetRndEmailDomain();
			return ret;
		}

		public string GetRndLicensePlateNumber() {
			string LicensePlate = "";
			LicensePlate = "";
			LicensePlate += (char)rnd.Next(65, 90);
			LicensePlate += (char)rnd.Next(65, 90);
			LicensePlate += (char)rnd.Next(65, 90);
			LicensePlate += rnd.Next(100, 999).ToString();
			return LicensePlate;
		}

		public string GetRndVehicleBrand()
		{
			return VehicleBrandRepo[rnd.Next(minValue: 0, maxValue: VehicleBrandRepo.Length)];
		}

		Random rnd = new Random();

		string[] VehicleBrandRepo = { "Saab","Volvo", "Lamborghini", "BMW", "Nissan", "Chrysler", "Toyota" };
		string[] FirstNameRepo = { "John","Anders", "Brad", "Alex", "Sten", "Stefan", "Camilla", "Linda", "Birgitte", "Julia", "Janique", "Malin", "Wolfgang","Lena" };
		string[] LastNameRepo = { "Doe","Andersson", "Svensson", "Pitt","Schulman", "Löfen", "Hjerten", "Lind", "Wolin","Hansson","Mellin" };

		string[] MailDomainRepo = {
			"@yahoo.com",
			"@gmail.com",
			"@outlook.com",
			"@hotmail.com",
		};


		Int64[] SSNRepo = {
			199605282392,
			199605292383,
			199605302398,
			199605312389,
			199606012392,
			199606022383,
			199606032390,
			199606042381,
			199606052398,
			199606062389,
			199606072396,
			199606082387,
			199606092394,
			199606102383,
			199606112390,
			199606122381,
			199606132398,
			199606142389,
			199606152396,
			199606162387,
			199606172394,
			199606182385,
			199606192392,
			199606202381,
			199606212398,
			199606222389,
			199606232396,
			199606242387,
			199606252394,
			199606262385,
			199606272392,
			199606282383,
			199606292390,
			199606302389,
			199607012391,
			199607022382,
			199607032399,
			199607042380,
			199607052397,
			199607062388,
			199607072395,
			199607082386,
			199607092393,
			199607102382,
			199607112399,
			199607122380,
			199607132397,
			199607142388,
			199607152395,
			199607162386,
			199607172393,
			199607182384,
			199607192391,
			199607202380,
			199607212397,
			199607222388,
			199607232395,
			199607242386,
			199607252393,
			199607262384,
			199607272391,
			199607282382,
			199607292399,
			199607302388,
			199607312395,
			199608012382,
			199608022399,
			199608032380,
			199608042397,
			199608052388,
			199608062395,
			199608072386,
			199608082393,
			199608092384,
			199608102399,
			199608112380,
			199608122397,
			199608132388,
			199608142395,
			199608152386,
			199608162393,
			199608172384,
			199608182391,
			199608192382,
			199608202397,
			199608212388,
			199608222395,
			199608232386,
			199608242393,
			199608252384,
			199608262391,
			199608272382,
			199608282399,
			199608292380,
			199608302395,
			199608312386,
			199609012399,
			199609022380,
			199609032397,
			199609042388 };
	}
}
