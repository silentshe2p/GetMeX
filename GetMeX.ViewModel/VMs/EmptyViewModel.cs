using System.Threading.Tasks;

namespace GetMeX.ViewModels.VMs
{
	public class EmptyViewModel : IViewModel
	{
		private class EmptyViewModelCommand : AsyncCommandBase
		{
			public override bool CanExecute(object parameter)
			{
				return false;
			}

			public override Task ExecuteAsync(object parameter)
			{
				return Task.CompletedTask;
			}
		}

		public EmptyViewModel()
		{
			DoWorkCommand = new EmptyViewModelCommand();
		}

		public IAsyncCommand DoWorkCommand { get; set; }

		public Task DoWork()
		{
			return Task.CompletedTask;
		}
	}
}
