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
        private static Helper _helper = new Helper();
        private List<SearchResult> _results { get; set; }
        private List<int> _pageHistory { get; set; }
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

        private bool _hasNext;

        public bool HasNext
        {
            get { return _hasNext; }
            set
            {
                _hasNext = value;
                OnPropertyChanged();
            }
        }

        private bool _hasPrev;

        public bool HasPrev
        {
            get { return _hasPrev; }
            set
            {
                _hasPrev = value;
                OnPropertyChanged();
            }
        }

        public SearchResultViewModel()
        {
            _results = new List<SearchResult>();
            _pageHistory = new List<int>();
            _currentPage = -1;
            HasNext = true;
            HasPrev = false;
            Messenger.Base.Register<List<SearchResult>>(this, OnListResultsReceived);
            Messenger.Base.Register<NewQueryMsg>(this, OnNewQueryMsgReceived);
            Messenger.Base.Register<OldQueryMsg>(this, OnOldQueryMsgReceived);
            DoWorkCommand = AsyncCommand.Create(DoWork);
            PreviousPageCommand = AsyncCommand.Create(Rewind);
        }

        /// Query changed, flush _results
        private void OnNewQueryMsgReceived(NewQueryMsg m)
        {
            _currentPage = -1;
            _results.Clear();
            _pageHistory.Clear();
            HasNext = true;
            HasPrev = false;
            CurrentQuery = m.Query;
        }

        /// New request but query unchanged, reuse _results
        private void OnOldQueryMsgReceived(OldQueryMsg m)
        {
            _currentPage = 0;
            HasNext = true;
            HasPrev = false;
            CurrentPageResults = _results.GetRange(0, _pageHistory[_currentPage]);
        }

        /// Process received list of results
        private void OnListResultsReceived(List<SearchResult> list)
        {
            if (list.Count < 1)
            {
                HasNext = false;
            }
            else
            {
                var total = _results.Count;
                foreach (var res in list)
                {
                    res.SetIndex(total++);
                    _results.Add(res);
                }
                _pageHistory.Add(list.Count);
                Forward();
            }
        }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public Task DoWork()
        {
            var total = _helper.ListSum(_pageHistory);
            var prevSum = _pageHistory.Count + _currentPage;
            // Forward in _pageHistory if there are enough entries in _results
            if (total < _results.Count)
            {
                Forward();
            }
            // Signal service to send new entries as there is not enough for new page
            else
            {
                Messenger.Base.Send(new FetchResultsMsg(CurrentQuery, _results.Count));
            }
            HasPrev = true;
            return Task.CompletedTask;
        }

        // Reassign CurrentPageResults to next _results sublist according to _pageHistory
        private void Forward()
        {
            var latest = (_currentPage < 0) ? 0
                                : _helper.ListSum(_pageHistory.GetRange(0, _currentPage + 1 /*zero-indexed*/));
            CurrentPageResults = _results.GetRange(latest, _pageHistory[++_currentPage]);
        }

        public IAsyncCommand PreviousPageCommand { get; private set; }

        public Task Rewind()
        {
            _currentPage--;
            var start = (_currentPage == 0) ? 0
                                : _helper.ListSum(_pageHistory.GetRange(0, _currentPage));
            CurrentPageResults = _results.GetRange(start, _pageHistory[_currentPage]);
            HasNext = true;
            // First page
            if (_currentPage < 1)
            {
                HasPrev = false;
            }
            return Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
