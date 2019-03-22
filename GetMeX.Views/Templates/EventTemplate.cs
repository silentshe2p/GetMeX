using System;
using System.Linq;
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

        private void SlideEvent_OnMouseHover(object sender, MouseEventArgs e)
        {
            var eventBox = sender as ListBox;
            if (eventBox.Items.Count > 2)
            {
                var mousePos = e.GetPosition(Application.Current.MainWindow);
                var center = Application.Current.MainWindow.ActualWidth / 2;
                var diff = Math.Abs(mousePos.X - center);
                var itemsWidth = 370 /*each item has max with of 350 and margin of 20 */ * (eventBox.Items.Count-1) - 20;

                if (diff > (center * 0.7))
                {
                    Thickness margin = eventBox.Margin;
                    if (mousePos.X > center && -margin.Left < itemsWidth)
                    {
                        margin.Left -= 0.0015 * diff;
                    }
                    else if (mousePos.X < center && margin.Left < itemsWidth)
                    {
                        margin.Left += 0.0015 * diff;
                    }

                    eventBox.Margin = margin;
                }
            }
        }

        private void ResetMargin(object sender, RoutedEventArgs e)
        {
            var current = sender as FrameworkElement;
            while (current.Parent != null)
            {
                current = current.Parent as FrameworkElement;
            }
            
            if (current is Grid)
            {
                var outmostGrid = current as Grid;
                var eventBox = outmostGrid.Children.OfType<ListBox>();
                if (eventBox != null && eventBox.Count() > 0)
                {
                    eventBox.ElementAt(0).Margin = new Thickness(30, 0, 30, 0);
                }
            }
        }
    }
}
