using System.Windows;
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
    }
}
