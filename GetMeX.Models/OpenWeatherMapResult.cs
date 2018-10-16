namespace GetMeX.Models
{
	public class Weather {
		public string icon { get; set; }
	}

	public class Main
	{
		public float temp { get; set; }
		public int humidity { get; set; }
		public float temp_min { get; set; }
		public float temp_max { get; set; }
	}

	public class Wind
	{
		public float speed { get; set; }
	}

	public class Sys
	{
		public string country { get; set; }
	}

	public class OpenWeatherMapResult
	{
		public Weather[] weather { get; set; }
		public Main main { get; set; }
		public Wind wind { get; set; }
		public Sys sys { get; set; }
		public string name { get; set; }
	}
}
