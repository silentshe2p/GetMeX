using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Templates
{
    public partial class WeatherTemplate
    {
        public void Help_Show(object sender, RoutedEventArgs e)
        {
            var icon = sender as FontAwesome.WPF.ImageAwesome;

            icon.Height += 6;
            icon.Width += 6;
            icon.Foreground = new SolidColorBrush(Colors.Orange);
            ToggleTextVisibility(icon);
        }

        public void Help_Hide(object sender, RoutedEventArgs e)
        {
            var icon = sender as FontAwesome.WPF.ImageAwesome;
            icon.Height -= 6;
            icon.Width -= 6;
            icon.Foreground = new SolidColorBrush(Colors.Black);
            ToggleTextVisibility(icon);
        }

        private void ToggleTextVisibility(FontAwesome.WPF.ImageAwesome icon)
        {
            if (icon.Parent is Grid grid)
            {
                var helpBlock = grid.Children.OfType<TextBlock>().FirstOrDefault(x => x.Name == "UsageHelp");
                if (helpBlock != null)
                {
                    helpBlock.Visibility = (helpBlock.Visibility == Visibility.Visible)
                                                    ? Visibility.Hidden : Visibility.Visible;
                }
            }
        }
    }
}
