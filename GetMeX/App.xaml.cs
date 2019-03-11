using System;
using System.IO;
using System.Windows;
using GetMeX.Views;
using GetMeX.ViewModels.VMs;

namespace GetMeX
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        const string gcCredentialPath = "auth/credentials.json";
        const string gcTokenPath = "auth/token.json";

        protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
            SetDirectoryVariables();
            AppDomain.CurrentDomain.UnhandledException += AppDomainExceptionHandler;
			ComposeObject();
			Current.MainWindow.Show();
		}

		private static void ComposeObject()
		{
			var viewModel = new EmptyViewModel();
			Current.MainWindow = new GetMeXWindow(viewModel);
		}

        private void SetDirectoryVariables()
        {
            var currentDir = Directory.GetCurrentDirectory();
            AppDomain.CurrentDomain.SetData("GoogleCalendarCredentialPath", Path.Combine(currentDir, gcCredentialPath));
            AppDomain.CurrentDomain.SetData("GoogleCalendarTokenPath", Path.Combine(currentDir, gcTokenPath));
        }

		private void AppDomainExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			string errMsg = string.Format("Error occurred: {0}{1}Application will be closed", e.Message, Environment.NewLine);
			if (MessageBox.Show(errMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
			{
				Current.Shutdown();
			}
		}
	}
}
