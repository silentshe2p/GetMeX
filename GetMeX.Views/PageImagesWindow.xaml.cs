using System;
using System.Windows;
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
    }
}
