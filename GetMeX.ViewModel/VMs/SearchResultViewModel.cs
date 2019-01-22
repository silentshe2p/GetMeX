using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GetMeX.Models;
using GetMeX.ViewModels.Utilities;
using GetMeX.ViewModels.Utilities.Messages;

namespace GetMeX.ViewModels.VMs
{
    public class SearchResultViewModel : INotifyPropertyChanged, IViewModel
    {
        private int _resultsPerPage = 10;
        private List<SearchResult> _results { get; set; }
        private int _largestDisplayResult { get; set; }

        private string _currentQuery;

        public string CurrentQuery
        {
            get { return _currentQuery; }
            set
            {
                _currentQuery = value;
                OnPropertyChanged();
            }
        }

        private List<SearchResult> _currentPageResults;

        public List<SearchResult> CurrentPageResults
        {
            get { return _currentPageResults; }
            set
            {
                _currentPageResults = value;
                OnPropertyChanged();
            }
        }

        public SearchResultViewModel()
        {
            _results = new List<SearchResult>();
            _largestDisplayResult = 0;
            Messenger.Base.Register<List<SearchResult>>(this, OnListResultsReceived);
            Messenger.Base.Register<NewQueryMsg>(this, OnNewQueryMsgReceived);
            Messenger.Base.Register<OldQueryMsg>(this, OnOldQueryMsgReceived);
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        /// Query changed, flush _results
        private void OnNewQueryMsgReceived(NewQueryMsg m)
        {
            _largestDisplayResult = 0;
            _results.Clear();
            CurrentQuery = m.Query;
        }

        /// New request but query unchanged, reuse _results
        private void OnOldQueryMsgReceived(OldQueryMsg m)
        {
            _largestDisplayResult = 0;
            CurrentPageResults = (_results.Count >= _resultsPerPage) ? _results.GetRange(0, _resultsPerPage) : _results;
        }

        /// Process received list of results
        private void OnListResultsReceived(List<SearchResult> list)
        {
            foreach(var res in list)
            {
                _results.Add(res);
            }
            var total = _results.Count;
            // Enough entries for full page
            if (total >= _largestDisplayResult + _resultsPerPage)
            {
                CurrentPageResults = _results.GetRange(_largestDisplayResult, _resultsPerPage);
                _largestDisplayResult += _resultsPerPage;
            }
            // Not enough entries for full page
            else if (total >= _largestDisplayResult)
            {
                CurrentPageResults = _results.GetRange(_largestDisplayResult, total - _largestDisplayResult);
                _largestDisplayResult += total - _largestDisplayResult;
            }
        }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public Task DoWork()
        {
            return Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
