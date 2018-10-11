using System.Threading.Tasks;

namespace GetMeX.ViewModels.VMs
{
	public interface IViewModel
	{
		IAsyncCommand DoWorkCommand { get; }
		Task DoWork();
	}
}
