using System.Threading.Tasks;

namespace GetMeX.ViewModels.VMs
{
	public class EmptyViewModel : IViewModel
	{
		public EmptyViewModel()
		{
			DoWorkCommand = AsyncCommand.Create(DoWork);
		}

		public IAsyncCommand DoWorkCommand { get; set; }

		public Task DoWork()
		{
			return Task.CompletedTask;
		}
	}
}
