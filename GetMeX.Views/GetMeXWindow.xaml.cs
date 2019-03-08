using System;
using System.Windows;
using System.Windows.Controls;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Views
{
	/// <summary>
	/// Interaction logic for GetMeXWindow.xaml
	/// </summary>
	public partial class GetMeXWindow : Window
	{
		private string _lastFeature = null;
		private static J1fmViewModel _j1fmVM = new J1fmViewModel();

		private static WeatherViewModel _weatherVM = new WeatherViewModel();

        // Google search function vms
		private static SearchResultViewModel _srVM = new SearchResultViewModel();
		private static SearchResultWindow _gsView = new SearchResultWindow(_srVM);
		private static GoogleSearchViewModel _gsVM = new GoogleSearchViewModel(_gsView);

        // Event function vms
        private static EventEditViewModel _eeVM = new EventEditViewModel();
        private static EventEditWindow _eeView = new EventEditWindow(_eeVM);
        private static Window[] eventWindows = new Window[] { _eeView, _eeView };
        private static EventsViewModel _eVM = new EventsViewModel(eventWindows);

        public GetMeXWindow(IViewModel viewModel)
		{
			InitializeComponent();
			MainSelection.DropDownClosed += OnDropDownOpened;
			MainSelection.SelectionChanged += OnSelectionChanged;
			DataContext = viewModel;
			Closing += ChildWindowCleanup;
		}

		private void OnDropDownOpened(object sender, EventArgs e)
		{
			_lastFeature = MainSelection.SelectedItem?.ToString();
		}

		/// Assign DataContext to corresponding view model based on user selection
		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var currentFeature = MainSelection.SelectedItem.ToString();
			if (MainSelection.SelectedItem != null && (_lastFeature == null || !_lastFeature.Equals(currentFeature)))
			{
				switch (currentFeature)
				{
					case "J1fm":
						DataContext = _j1fmVM;
						_lastFeature = currentFeature;
						break;
					case "Weather":
						DataContext = _weatherVM;
						_lastFeature = currentFeature;
						break;
					case "GoogleSearch":
						DataContext = _gsVM;
						_lastFeature = currentFeature;
						break;
                    case "Events":
                        DataContext = _eVM;
                        _lastFeature = currentFeature;
                        break;
					default:
						break;
				}
			}
		}

		/// Close open or hidden child windows
		private void ChildWindowCleanup(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_gsVM.CloseChildView(parentClosing:true);
            _eVM.CloseChildView(parentClosing:true);

        }
	}
}
