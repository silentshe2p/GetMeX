using GetMeX.ViewModels.VMs;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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

		private void J1BtnBrowseFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			};
			if (dialog.ShowDialog() == true)
			{
				_j1fmVM.FileToAppend = dialog.FileName;
			}
		}

		private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
		{
			var s = sender as TextBlock;
			Process.Start(new ProcessStartInfo(s?.Text));
			e.Handled = true;
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
					case "Dictionary":
						DataContext = _dictVM;
						_lastFeature = currentFeature;
						break;
					default:
						break;
				}
			}
		}
	}
}
