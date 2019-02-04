using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GetMeX.Models;
using GetMeX.ViewModels.Services;
using GetMeX.ViewModels.Utilities;

namespace GetMeX.ViewModels.VMs
{
    public class PageImagesViewModel : INotifyPropertyChanged, IViewModel
    {
        private int _page { get; set; }
        private int _imgPerPage { get; set; }

        private string _link;
        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged();
            }
        }

        private OnlineImageResult _selectedImage;
        public OnlineImageResult SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<OnlineImageResult> _images;
        public ObservableCollection<OnlineImageResult> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }

        public PageImagesViewModel(string link)
        {
            _page = 0;
            _imgPerPage = 20;
            Link = link;
            SelectedImage = null;
            Images = null;
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public async Task DoWork()
        {
            var retriever = new ImageRetrieverService(limit: _imgPerPage);
            var results = await retriever.RetrieveImages(Link);
            Images = results.ToObservableCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
