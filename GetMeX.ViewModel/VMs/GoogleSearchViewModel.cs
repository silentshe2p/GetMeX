﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using GetMeX.Models;
using GetMeX.ViewModels.Commands;
using GetMeX.ViewModels.Exceptions;
using GetMeX.ViewModels.Services;
using GetMeX.ViewModels.Utilities;

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

        private string _language;
        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged();
            }
        }

        private List<string> _availableLanguages;
        public List<string> AvailableLanguages
        {
            get { return _availableLanguages; }
            set {
                _availableLanguages = value;
                OnPropertyChanged();
            }
        }

        private List<GoogleSuggestion> _suggestions;
        public List<GoogleSuggestion> Suggestions
        {
            get { return _suggestions; }
            set
            {
                _suggestions = value;
                OnPropertyChanged();
            }
        }

        private bool _suggestionAllowed;
        public bool SuggestionAllowed
        {
            get { return _suggestionAllowed; }
            set
            {
                _suggestionAllowed = value;
                OnPropertyChanged();
            }
        }

        private ViewService viewService { get; set; }
        private List<SearchResult> _results { get; set; }
        private bool _queryUnchanged = true;
        private LanguageCode _langCode = new LanguageCode();

        public GoogleSearchViewModel(Window view)
        {
            Query = "";
            Language = "Auto";
            SuggestionAllowed = true;
            AvailableLanguages = _langCode.GetLanguages();

            // Init commands and messenger
            SetQueryCommand = new RelayCommand(
                (object q) => { Query = q.ToString(); }, 
                (object q) => { return !string.IsNullOrEmpty(q.ToString()); }
            );
            DoWorkCommand = AsyncCommand.Create(DoWork);
            SuggestionCommand = AsyncCommand.Create(FetchSuggestions);
            Messenger.Base.Register<FetchResultsMsg>(this, OnFetchResultsMsgReceived);

            // View for search result displaying
            viewService = new ViewService(view);
        }

        public IAsyncCommand DoWorkCommand { get; private set; }
        public async Task DoWork()
        {
            if (_queryUnchanged)
            {
                // Reuse old results
                Messenger.Base.Send(new OldQueryMsg());
                viewService.ShowView();
            }
            else
            {
                // Flush old results waiting for new results
                Messenger.Base.Send(new NewQueryMsg(Query));

                GetGoogleService service = new GetGoogleService(Query, _langCode.LangToCode(Language));
                var results = await service.GetGoogleSearches();
                if (results == null || results.Count == 0)
                {
                    throw new NoResultException();
                }

                // Send new results and show results view
                Messenger.Base.Send(results);
                _queryUnchanged = true;
                viewService.ShowView();
            }
        }

        // New page of search result needed so fetch it and send to requester
        private async void OnFetchResultsMsgReceived(FetchResultsMsg m)
        {
            GetGoogleService service = new GetGoogleService(m.query, _langCode.LangToCode(Language), m.start);
            var results = await service.GetGoogleSearches();
            Messenger.Base.Send(results);
            _queryUnchanged = true;
        }

        public IAsyncCommand SuggestionCommand { get; private set; }
        private async Task FetchSuggestions() // Fetch suggestion for current inputted query
        {
            if (string.IsNullOrEmpty(Query))
            {
                Suggestions = null;
            }
            else
            {
                GoogleSuggestionService service = new GoogleSuggestionService(Query, _langCode.LangToCode(Language));
                var results = await service.GetSuggestions();
                Suggestions = results;
            }
        }

        public RelayCommand SetQueryCommand { get; private set; }

        public void CloseChildView(bool parentClosing)
        {
            viewService.CloseView(parentClosing);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Query" || propertyName == "Language")
            {
                _queryUnchanged = false;
            }
            else if (propertyName == "SuggestionAllowed")
            {
                if (!SuggestionAllowed)
                {
                    Suggestions = null;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
