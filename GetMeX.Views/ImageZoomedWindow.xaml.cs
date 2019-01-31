using System.Windows;
using GetMeX.Models;

namespace GetMeX.Views
{
    /// <summary>
    /// Interaction logic for ImageZoomedWindow.xaml
    /// </summary>
    public partial class ImageZoomedWindow : Window
    {
        public ImageZoomedWindow(OnlineImageResult data)
        {
            InitializeComponent();
            DataContext = data;
        }
    }
}
