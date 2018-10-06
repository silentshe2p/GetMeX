using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMeX.Models
{
	class Weather
	{
		public string Location { get; set; }

		public string Status { get; set; }

		public int Temp { get; set; }

		public int Precipitation { get; set; }

		public int Humidity { get; set; }

		public int Wind { get; set; }
	}
}
