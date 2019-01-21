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
        private int _currentResultsCount { get; set; }
        private List<SearchResult> _results { get; set; }
        private int _currentPage { get; set; }

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
            _currentPage = 0;
            Messenger.Base.Register<List<SearchResult>>(this, OnListResultsReceived);
            Messenger.Base.Register<NewQueryMsg>(this, OnNewQueryMsgReceived);
            Messenger.Base.Register<OldQueryMsg>(this, OnOldQueryMsgReceived);
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        /// Query changed, flush _results
        private void OnNewQueryMsgReceived(NewQueryMsg m)
        {
            _currentPage = 0;
            _results.Clear();
            CurrentQuery = m.Query;
        }

        /// New request but query unchanged, reuse _results
        private void OnOldQueryMsgReceived(OldQueryMsg m)
        {
            _currentPage = 0;
            var pageStart = _currentPage * _resultsPerPage;
            CurrentPageResults = (_results.Count >= pageStart) ? _results.GetRange(pageStart, _resultsPerPage) : _results;
        }

        /// Process received list of results
        private void OnListResultsReceived(List<SearchResult> list)
        {
            foreach(var res in list)
            {
                _results.Add(res);
            }
            var total = _results.Count;
            var pageStart = _currentPage * _resultsPerPage;
            // Enough entries for current page
            if (total >= (_currentPage+1)*_resultsPerPage)
            {
                CurrentPageResults = _results.GetRange(pageStart, _resultsPerPage);
            }
            // Not enough entries for current page
            else if (total >= _currentPage*_resultsPerPage)
            {
                CurrentPageResults = _results.GetRange(pageStart, total - pageStart);
            }
            // Only previous page
            else
            {
                CurrentPageResults = _results;
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
