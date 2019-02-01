using GetMeX.Models;
using GetMeX.ViewModels.Services;
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

		public WeatherViewModel(string inputLocation = null, string unit = null)
		{
			InputLocation = inputLocation;
			TempUnit = (unit == null) ? "C" : unit;
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
