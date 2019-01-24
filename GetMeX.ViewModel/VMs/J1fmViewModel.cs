using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GetMeX.Models;
using GetMeX.ViewModels.Services;

namespace GetMeX.ViewModels.VMs
{
	public class J1fmViewModel : INotifyPropertyChanged, IViewModel
	{
		private string _channel;
		public string Channel
		{
			get { return _channel; }
			set
			{
				_channel = value;
				OnPropertyChanged();
			}
		}

		private string _fileToAppend;
		public string FileToAppend
		{
			get { return _fileToAppend; }
			set
			{
				_fileToAppend = value;
				OnPropertyChanged();
			}
		}

		private SongInfo _info;
		public SongInfo Info
		{
			get { return _info; }
			set
			{
				_info = value;
				OnPropertyChanged();
			}
		}

		public J1fmViewModel()
		{
			Channel = "Hits";
			FileToAppend = null;
			DoWorkCommand = AsyncCommand.Create(DoWork);
		}

		public IAsyncCommand DoWorkCommand { get; private set; }
		public async Task DoWork()
		{
			GetSongService service = new GetSongService(Channel);
			Info = await service.GetSongInfo();
			if (FileToAppend != null && File.Exists(FileToAppend))
			{
                var appendFormat = "{0} - {1} @ {2}";
                var appendContent = string.Format(appendFormat, Info.Artist, Info.Title, Info.StoreUrl);
				File.AppendAllText(FileToAppend, appendContent + Environment.NewLine, Encoding.Unicode);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
