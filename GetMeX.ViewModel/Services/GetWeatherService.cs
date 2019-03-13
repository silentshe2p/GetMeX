using System;
using System.Device.Location;
using System.IO;
using System.Linq;
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
		private string _apiKey;
		private TempUnit _unit;
		private static string openweathermapUrl = "http://api.openweathermap.org/data/2.5/weather?APPID=";
		private static string statusIconUrl = "http://openweathermap.org/img/w/{0}.png";
		private static string[] unitParam = { "metric", "imperial" };
        private static GeoCoordinateWatcher _geoWatcher;
        private int _geoWatcherTimeout = 1000; // milliseconds
        private string _location { get; set; }

        public GetWeatherService(string apiKey, string inputLocation, string unit = "Celsius")
		{
            var defaultApiKey = AppDomain.CurrentDomain.GetData("DefaultWeatherApiKey").ToString();
            _apiKey = ((apiKey == null || apiKey == "") ? defaultApiKey : apiKey);
            _location = inputLocation;
			switch(unit)
			{
				case "F":
					this._unit = TempUnit.F;
					break;
				case "K":
					this._unit = TempUnit.K;
					break;
				case "C":
				default:
					this._unit = TempUnit.C;
					break;
			}
		}

		public async Task<WeatherInfo> GetWeatherInfo()
		{
			WeatherInfo result = new WeatherInfo();
			if (string.IsNullOrEmpty(_location))
			{
                // Try to figure out location using GeoCoordinateWatcher with timeout
                InitiateGeoWatcher();
                await Task.Delay(TimeSpan.FromMilliseconds(_geoWatcherTimeout));
                if (string.IsNullOrEmpty(_location))
                {
                    throw new InsufficientDataException("Unable to retrieve location information");
                }
            }

            var locationQuery = LocationToQuery(_location);
			var unitQuery = (_unit == TempUnit.K) ? "" : string.Format("&units={0}", unitParam[(int)_unit]);
			var query = openweathermapUrl + _apiKey + '&' + locationQuery + unitQuery;

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

        private void InitiateGeoWatcher()
		{
            _geoWatcher = new GeoCoordinateWatcher();
            _geoWatcher.PositionChanged += 
                new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Watcher_PositionChanged);
            _geoWatcher.Start();
		}

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var foundLocation = e.Position.Location;
            _location = string.Format("lat={0}&lon={1}", foundLocation.Latitude, foundLocation.Longitude);
            _geoWatcher.Stop();
        }

        private string LocationToQuery(string location)
        {
            var query = "";
            if (!string.IsNullOrEmpty(location))
            {
                var locationParts = location.Split(',');
                var main = locationParts[0];
                var countryCode = (locationParts.Length > 1) ? ("," + locationParts[1]) : "";
                if (main.Contains("&")) // coordinate format
                {
                    query = main;
                }
                else if (main.Any(char.IsDigit)) // zip code format
                {
                    query = "zip=" + main + countryCode;
                }
                else // city name format
                {
                    query = "q=" + main + countryCode;
                }
            }
            return query;
        }
    }
}
