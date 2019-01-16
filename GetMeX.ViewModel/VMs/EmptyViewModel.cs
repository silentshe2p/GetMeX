using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GetMeX.ViewModels.VMs
{
	public class EmptyViewModel : INotifyPropertyChanged, IViewModel
	{
		private static string greeting = "Choose a feature to get started!";
		private static string hint = "Hint: Select a feature from the dropdown box above!";
		private string _usage;

		public string Usage
		{
			get { return _usage; }
			set {
				_usage = value;
				OnPropertyChanged();
			}
		}


		public EmptyViewModel()
		{
			Usage = greeting;
			DoWorkCommand = AsyncCommand.Create(DoWork);
		}

		public IAsyncCommand DoWorkCommand { get; private set; }

		public Task DoWork()
		{
			Usage = hint;
			return Task.CompletedTask;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
