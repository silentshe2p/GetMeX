using System;
using System.Windows;
using System.Windows.Controls;
using GetMeX.Models;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Views
{
    /// <summary>
    /// Interaction logic for PageImagesWindow.xaml
    /// </summary>
    public partial class PageImagesWindow : Window
    {
        public PageImagesWindow(string result)
        {
            InitializeComponent();
            DataContext = new PageImagesViewModel(result);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var viewModel = (PageImagesViewModel)DataContext;
            if (viewModel.DoWorkCommand.CanExecute(null))
                viewModel.DoWorkCommand.Execute(null);
        }

        private void ClickedOn_ToSelectedImage(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Image img)
            {
                var clickedOn = img.DataContext as OnlineImageResult;
                var imageList = sender as ListBox;
                var viewModel = imageList.DataContext as PageImagesViewModel;
                viewModel.SelectedImage = clickedOn;
            }
        }
    }
}
