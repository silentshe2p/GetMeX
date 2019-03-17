using System;
using System.Windows;
using System.Windows.Controls;
using GetMeX.Models;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Views
{
    /// <summary>
    /// Interaction logic for EventWideViewWindow.xaml
    /// </summary>
    public partial class EventTreeViewWindow : Window
    {
        public EventTreeViewWindow()
        {
            InitializeComponent();
        }

        private void LoadEvent_OnExpanded(object sender, RoutedEventArgs e)
        {
            var toExpandItem = e.OriginalSource as TreeViewItem;
            var year = toExpandItem.DataContext as EventPeriod<EventPeriod<GXEvent>>;

            if (year != null)
            {
                // "Unexpand" previously expanded item
                var total = EventTree.Items.Count;
                var current = (int)year.Period - DateTime.Today.Year;
                for (int i = 0; i < total; i++)
                {
                    if (i != current)
                    {
                        var item = (TreeViewItem)(EventTree.ItemContainerGenerator.ContainerFromIndex(i));
                        item.IsExpanded = false;
                    }
                }

                // Load events if selected year's events are unloaded
                var viewModel = ((FrameworkElement)sender).DataContext as EventTreeViewModel;
                if (viewModel.LoadEventsCommand.CanExecute(year.Period))
                {
                    viewModel.LoadEventsCommand.Execute(year.Period);
                }
            }
        }

        private void EditEvent_OnClick(object sender, RoutedEventArgs e)
        {
            var eventToEdit = ((FrameworkElement)sender).DataContext;
            if (eventToEdit != null && eventToEdit is GXEvent && DataContext is EventTreeViewModel)
            {
                var viewModel = DataContext as EventTreeViewModel;
                if (viewModel.EditEventCommand.CanExecute(eventToEdit))
                {
                    viewModel.EditEventCommand.Execute(eventToEdit);
                }
            }
        }
    }
}
