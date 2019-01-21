using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using GetMeX.Models;
using GetMeX.ViewModels.Services;
using GetMeX.ViewModels.Utilities;
using GetMeX.ViewModels.Utilities.Messages;

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

        public ViewService viewService { get; set; }
        private string _cache { get; set; }
        private List<SearchResult> _results { get; set; }

        public GoogleSearchViewModel(Window view)
        {
            _cache = "";
            viewService = new ViewService(view);
            Query = "";
            DoWorkCommand = AsyncCommand.Create(DoWork);
            Messenger.Base.Register<FetchResultsMsg>(this, OnFetchResultsMsgReceived);
        }

        public IAsyncCommand DoWorkCommand { get; private set; }
        public async Task DoWork()
        {
            if (!_cache.Equals("") && _cache.Equals(Query))
            {
                // Reuse old results
                Messenger.Base.Send(new OldQueryMsg());
            }
            else
            {
                _cache = Query;
                // Flush old results waiting for new results
                Messenger.Base.Send(new NewQueryMsg(Query));

                GetGoogleService service = new GetGoogleService(Query);
                var results = await service.GetGoogleSearches();

                // Send new results and show results view
                Messenger.Base.Send(results);
                viewService.ShowView();
            }
        }

        private async void OnFetchResultsMsgReceived(FetchResultsMsg m)
        {
            GetGoogleService service = new GetGoogleService(m.query, m.start);
            var results = await service.GetGoogleSearches();
            Messenger.Base.Send(results);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
