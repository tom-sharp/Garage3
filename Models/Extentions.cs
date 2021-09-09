using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
	static public class Extentions
	{
		public static string TimedDiffString(this DateTime DT, DateTime DT2)
		{
			int seconds = (int)(DT - DT2).TotalSeconds;
			int minutes = seconds / 60; seconds %= 60; ;
			int hours = minutes / 60; minutes %= 60;
			int days = hours / 24; hours %= 24;
			if (days > 0) return $"{days} Days {hours} Hours";
			else if (hours > 0) return $"{hours} Hours {minutes} Minutes";
			return $"{minutes} Minutes {seconds} Seconds";
		}

		public static string TimedString(this TimeSpan TS)
		{
			int seconds = (int)TS.TotalSeconds;
			int minutes = seconds / 60; seconds %= 60; ;
			int hours = minutes / 60; minutes %= 60;
			int days = hours / 24; hours %= 24;
			if (days > 0) return $"{days} Days {hours} Hours";
			else if (hours > 0) return $"{hours} Hours {minutes} Minutes";
			return $"{minutes} Minutes {seconds} Seconds";
		}
	}
}
