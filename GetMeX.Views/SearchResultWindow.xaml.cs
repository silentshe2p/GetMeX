using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using GetMeX.Models;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Views
{
    /// <summary>
    /// Interaction logic for GetMeXWindow.xaml
    /// </summary>
    public partial class SearchResultWindow : Window
    {
        public SearchResultWindow(IViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            // The actual link is inside a TextBlock which is inside HyperLink
            var s = sender as TextBlock;
            Process.Start(new ProcessStartInfo(s?.Text));
            e.Handled = true;
        }

        public void Image_ZoomIn(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            var data = s.DataContext as OnlineImageResult;
            ImageZoomedWindow imageWindow = new ImageZoomedWindow(data);
            imageWindow.ShowDialog();
        }
    }
}
