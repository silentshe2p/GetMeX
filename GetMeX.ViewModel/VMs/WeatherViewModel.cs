using GetMeX.Models;
using GetMeX.ViewModels.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GetMeX.ViewModels.VMs
{
	public class WeatherViewModel : INotifyPropertyChanged, IViewModel
	{
		private string _apiKey;
		public string ApiKey
		{
			get { return _apiKey; }
			set
			{
				_apiKey = value;
				OnPropertyChanged();
			}
		}

		private string _inputLocation;
		public string InputLocation
		{
			get { return _inputLocation; }
			set
			{
				_inputLocation = value;
				OnPropertyChanged();
			}
		}

		private string _tempUnit;
		public string TempUnit
		{
			get { return _tempUnit; }
			set
			{
				_tempUnit = value;
				OnPropertyChanged();
			}
		}

		private string _location;
		public string Location
		{
			get { return _location; }
			set
			{
				_location = value;
				OnPropertyChanged();
			}
		}

		private string _status;
		public string Status
		{
			get { return _status; }
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		}

		private float _temp;
		public float Temp
		{
			get { return _temp; }
			set
			{
				_temp = value;
				OnPropertyChanged();
			}
		}

		private float _tempMax;
		public float TempMax
		{
			get { return _tempMax; }
			set
			{
				_tempMax = value;
				OnPropertyChanged();
			}
		}

		private float _tempMin;
		public float TempMin
		{
			get { return _tempMin; }
			set
			{
				_tempMin = value;
				OnPropertyChanged();
			}
		}

		private int _humidity;
		public int Humidity
		{
			get { return _humidity; }
			set
			{
				_humidity = value;
				OnPropertyChanged();
			}
		}

		private float _wind;
		public float Wind
		{
			get { return _wind; }
			set {
				_wind = value;
				OnPropertyChanged();
			}
		}

		private WeatherInfo _info;
		public WeatherInfo Info
		{
			get { return _info; }
			set
			{
				_info = value;
				OnPropertyChanged();
			}
		}

		public WeatherViewModel()
		{
			TempUnit = "Celsius";
			DoWorkCommand = AsyncCommand.Create(DoWork);
		}

		public IAsyncCommand DoWorkCommand { get; private set; }
		public async Task DoWork()
		{
			GetWeatherService service = new GetWeatherService(ApiKey, InputLocation, TempUnit);
			Info = await service.GetWeatherInfo();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
