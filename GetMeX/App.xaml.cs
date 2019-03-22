using System;
using System.IO;
using System.Windows;
using GetMeX.Views;
using GetMeX.ViewModels.VMs;
using System.Reflection;

namespace GetMeX
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        int eventYearRange = 3;
        const string gcCredentialPath = @"auth\credentials.json";
        const string gcTokenPath = @"auth\token.json";
        static string weatherApiKeyHash = "lop7pm<:?7;@k=ql=>k;A;:k?o@;nn??";


        protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
            SetGlobalVariables();
            AppDomain.CurrentDomain.UnhandledException += AppDomainExceptionHandler;
			ComposeObject();
			Current.MainWindow.Show();
		}

		private static void ComposeObject()
		{
			var viewModel = new EmptyViewModel();
			Current.MainWindow = new GetMeXWindow(viewModel);
		}

        private void SetGlobalVariables()
        {
            var key = Assembly.GetEntryAssembly().GetName().Name;
            var apiKey = Hash(weatherApiKeyHash, key);
            AppDomain.CurrentDomain.SetData("DefaultWeatherApiKey", apiKey);

            var currentDir = Directory.GetCurrentDirectory();
            AppDomain.CurrentDomain.SetData("GoogleCalendarCredentialPath", Path.Combine(currentDir, gcCredentialPath));
            AppDomain.CurrentDomain.SetData("GoogleCalendarTokenPath", Path.Combine(currentDir, gcTokenPath));
            AppDomain.CurrentDomain.SetData("EventViewableYearRange", eventYearRange);
        }

        private string Hash(string input, string key)
        {
            if (input == null || key == null)
            {
                return null;
            }
            var keyChars = key.ToCharArray();
            var inputChars = input.ToCharArray();
            char[] hashed = new char[inputChars.Length];

            for (int i = 0; i < inputChars.Length; i++)
            {
                hashed[i] = (char)(inputChars[i] - key[i % key.Length] / 10);
            }
            return new string(hashed);
        }

		private void AppDomainExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			string errMsg = string.Format("Error occurred: {0}{1}Application will be closed", e.InnerException.Message, Environment.NewLine);
			if (MessageBox.Show(errMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
			{
				Current.Shutdown();
			}
		}
	}
}
