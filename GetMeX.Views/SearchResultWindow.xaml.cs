using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public void Video_PlayEmbeded(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            var result = s.DataContext as SearchResult;
            var video = result.Video;
            OnlineVideoWindow videoWindow = new OnlineVideoWindow(video);
            videoWindow.ShowDialog();
        }

        public void PlayCircle_FadeIn(object sender, RoutedEventArgs e)
        {
            var s = sender as MaterialDesignThemes.Wpf.PackIcon;
            Random rand = new Random();
            s.Foreground = new SolidColorBrush(
                    Color.FromRgb(
                        (byte)rand.Next(255),
                        (byte)rand.Next(255),
                        (byte)rand.Next(255)
                    )
            );
            s.Opacity += 0.2;
        }

        public void PlayCircle_FadeOut(object sender, RoutedEventArgs e) {
            var s = sender as MaterialDesignThemes.Wpf.PackIcon;
            s.Foreground = new SolidColorBrush(Colors.Black);
            s.Opacity -= 0.2;
        }
    }
}
