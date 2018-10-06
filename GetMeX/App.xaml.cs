using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GetMeX
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ComposeObject();
			Application.Current.MainWindow.Show();
		}

		private static void ComposeObject()
		{
			Application.Current.MainWindow = new GetMeXWindow();
		}
	}
}
