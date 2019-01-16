using System;
using System.Device.Location;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GetMeX.Models;
using GetMeX.ViewModels.Exceptions;

namespace GetMeX.ViewModels.Services
{
	public class GetWeatherService
	{
		private string openweathermapApiKey;
		private string location;
		private TempUnit unit;
		private static string defaultApiKey = "eee0fe504018d3fe36d1640c8e54df85";
		private static string openweathermapUrl = "http://api.openweathermap.org/data/2.5/weather?APPID=";
		private static string statusIconUrl = "http://openweathermap.org/img/w/{0}.png";
		private static string[] unitParam = { "metric", "imperial" };

		public GetWeatherService(string apiKey, string inputLocation, string unit = "Celsius")
		{
			openweathermapApiKey = ((apiKey == null || apiKey == "") ? defaultApiKey : apiKey);
			location = inputLocation;
			switch(unit)
			{
				case "Fahrenheit":
					this.unit = TempUnit.Fahrenheit;
					break;
				case "Kelvin":
					this.unit = TempUnit.Kelvin;
					break;
				case "Celsius":
				default:
					this.unit = TempUnit.Celsius;
					break;
			}
		}

		public async Task<WeatherInfo> GetWeatherInfo()
		{
			WeatherInfo result = new WeatherInfo();
			if (location == null)
			{
				var foundLocation = TryGetLocation();
				if (foundLocation == null)
				{
					throw new InsufficientDataException("Location information is incorrect or not provided and couldn't be retrieved");
				}
				else
				{
					location = foundLocation;
				}
			}
			else
			{
				location = "q=" + location;
			}

			var unitQuery = (unit == TempUnit.Kelvin) ? "" : string.Format("&units={0}", unitParam[(int)unit]);
			var query = openweathermapUrl + openweathermapApiKey + '&' + location + unitQuery;
			using (var client = new HttpClient())
			{
				var res = await client.GetAsync(query, HttpCompletionOption.ResponseHeadersRead);
				if (res.IsSuccessStatusCode)
				{
					var resAsByteArr = await res.Content.ReadAsByteArrayAsync();
					MemoryStream ms = new MemoryStream(resAsByteArr);
					var resWrapper = new OpenWeatherMapResult();
					DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(OpenWeatherMapResult));
					resWrapper = (OpenWeatherMapResult)deserializer.ReadObject(ms);
					result.Location = string.Format("{0} ({1})", resWrapper.name, resWrapper.sys.country);
					result.Status = string.Format(statusIconUrl, resWrapper.weather?[0].icon);
					result.Temp = resWrapper.main.temp;
					result.TempMin = resWrapper.main.temp_min;
					result.TempMax = resWrapper.main.temp_max;
					result.Wind = resWrapper.wind.speed;
					result.Humidity = resWrapper.main.humidity;
				}
				else
				{
					if (res.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new ArgumentException("Api Key is incorrect");
					}
					else if (res.StatusCode == HttpStatusCode.NotFound)
					{
						throw new ArgumentException("Provided location is not found");
					}
				}
			}
			return result;
		}

		public string TryGetLocation()
		{
			GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
			// Display permission dialog box and timeout after 1000 milliseconds
			watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
			GeoCoordinate coord = watcher.Position.Location;

			if (coord.IsUnknown != true)
			{
				return string.Format("lat={0}&lon={1}", coord.Latitude, coord.Longitude);
			}
			return null;
		}
	}
}
