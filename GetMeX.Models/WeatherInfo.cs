namespace GetMeX.Models
{
	public class WeatherInfo
	{
		public string Location { get; set; }

		public string Status { get; set; }

		public int SunRise { get; set; }

		public int SunSet { get; set; }

		public float Temp { get; set; }

		public float TempMin { get; set; }

		public float TempMax { get; set; }

		public int Humidity { get; set; }

		public float Wind { get; set; }
	}
}
