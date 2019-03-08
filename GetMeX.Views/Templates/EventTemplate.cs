using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GetMeX.Models;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Templates
{
    public partial class EventTemplate
    {
        private void DoubleClick_EditEvent(object sender, MouseButtonEventArgs e)
        {
            var selected = ((FrameworkElement)e.OriginalSource).DataContext as GXEvent;
            if (selected != null)
            {
                var viewModel = ((ListBox) sender).DataContext as EventsViewModel;
                if (viewModel.EditEventCommand.CanExecute(selected))
                {
                    viewModel.EditEventCommand.Execute(selected);
                }
            }
        }
    }
}
