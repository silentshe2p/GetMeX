﻿using System.Collections.Generic;
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


        public ViewService viewService { get; set; }
        private List<SearchResult> _results { get; set; }
        private bool _queryUnchanged = true;
        private LanguageCode _langCode = new LanguageCode();

        public GoogleSearchViewModel(Window view)
        {
            viewService = new ViewService(view);
            Query = "";
            Language = "Auto";
            AvailableLanguages = _langCode.GetLanguages();
            DoWorkCommand = AsyncCommand.Create(DoWork);
            Messenger.Base.Register<FetchResultsMsg>(this, OnFetchResultsMsgReceived);
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

                // Send new results and show results view
                Messenger.Base.Send(results);
                _queryUnchanged = true;
                viewService.ShowView();
            }
        }

        private async void OnFetchResultsMsgReceived(FetchResultsMsg m)
        {
            GetGoogleService service = new GetGoogleService(m.query, _langCode.LangToCode(Language), m.start);
            var results = await service.GetGoogleSearches();
            Messenger.Base.Send(results);
            _queryUnchanged = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Query" || propertyName == "Language")
            {
                _queryUnchanged = false;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
