using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using GetMeX.Models;

namespace GetMeX.Views
{
    /// <summary>
    /// Interaction logic for ImageZoomedWindow.xaml
    /// </summary>
    public partial class OnlineVideoWindow : Window
    {
        public OnlineVideoWindow(VideoResult data)
        {
            InitializeComponent();
            var browser = new ChromiumWebBrowser();
            browser.Address = data.Link;
            browser.LifeSpanHandler = new LifeSpanHandler();
            DataContext = browser;
        }

        public class LifeSpanHandler : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

            public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

            public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser,
                                                        IFrame frame, string targetUrl, string targetFrameName, 
                                                        WindowOpenDisposition targetDisposition, bool userGesture, 
                                                        IPopupFeatures popupFeatures, IWindowInfo windowInfo, 
                                                        IBrowserSettings browserSettings, ref bool noJavascriptAccess, 
                                                        out IWebBrowser newBrowser)
            {
                // Prevent new tab/window
                newBrowser = null;
                // Only load embed video
                VideoConverter converters = new VideoConverter();
                var converter = converters.SearchKnownSite(targetUrl);
                if (converter != null)
                {
                    var embeded = converter(targetUrl);
                    chromiumWebBrowser.Load(embeded);
                }
                return true;
            }
        }
    }
}