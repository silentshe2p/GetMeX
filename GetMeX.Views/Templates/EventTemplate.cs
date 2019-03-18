using System;
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
            var mousePos = e.GetPosition(Application.Current.MainWindow);
            var center = Application.Current.MainWindow.ActualWidth / 2;
            var diff = Math.Abs(mousePos.X - center);

            if (diff > (center * 0.7))
            {
                var eventBox = sender as ListBox;
                Thickness margin = eventBox.Margin;

                if (mousePos.X > center)
                {
                    margin.Left -= 0.001 * diff;
                }
                else
                {
                    margin.Left += 0.001 * diff;
                }

                eventBox.Margin = margin;
            }
        }
    }
}
