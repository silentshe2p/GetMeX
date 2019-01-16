using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GetMeX.Models;
using GetMeX.ViewModels.Services;

namespace GetMeX.ViewModels.VMs
{
    public class GoogleSearchViewModel : INotifyPropertyChanged, IViewModel
    {
        private string _query;

        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                OnPropertyChanged();
            }
        }

        private List<SearchResult> _results;

        public List<SearchResult>  Results
        {
            get { return _results; }
            set
            {
                _results = value;
                OnPropertyChanged();
            }
        }

        public GoogleSearchViewModel()
        {
            _query = "";
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        public IAsyncCommand DoWorkCommand { get; private set; }
        public async Task DoWork()
        {
            GetGoogleService service = new GetGoogleService(Query);
            Results = await service.GetGoogleSearches();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
