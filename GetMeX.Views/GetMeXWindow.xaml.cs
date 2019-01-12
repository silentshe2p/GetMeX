using System;
using System.Windows;
using System.Windows.Controls;
using GetMeX.ViewModels.VMs;

namespace GetMeX
{
	/// <summary>
	/// Interaction logic for GetMeXWindow.xaml
	/// </summary>
	public partial class GetMeXWindow : Window
	{
		private string _lastFeature = null;
		private static J1fmViewModel _j1fmVM = new J1fmViewModel();
		private static WeatherViewModel _weatherVM = new WeatherViewModel();
		private static DictionaryViewModel _dictVM = new DictionaryViewModel();

		public GetMeXWindow(IViewModel viewModel)
		{
			InitializeComponent();
			MainSelection.DropDownClosed += OnDropDownOpened;
			MainSelection.SelectionChanged += OnSelectionChanged;
			DataContext = viewModel;
		}

		private void OnDropDownOpened(object sender, EventArgs e)
		{
			_lastFeature = MainSelection.SelectedItem?.ToString();
		}

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
					default:
						break;
				}
			}
		}
	}
}
