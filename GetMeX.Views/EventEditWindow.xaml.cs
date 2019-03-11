using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Views
{
    public partial class EventEditWindow : Window
    {
        public EventEditWindow(IViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += ClearFields;
        }

        private void ClearFields(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = DataContext as EventEditViewModel;
            viewModel.InitModel();
        }

        private void TextBox_CheckEventModified(object sender, KeyEventArgs e)
        {
            var viewModel = ((TextBox)sender).DataContext as EventEditViewModel;
            if (viewModel.CheckEventModifiedCommand.CanExecute(null))
            {
                viewModel.CheckEventModifiedCommand.Execute(null);
            }
        }

        private void DateTimeBox_CheckEventModified(object sender, KeyEventArgs e)
        {
            DateTime dt;
            var dtBox = sender as TextBox;
            var viewModel = dtBox.DataContext as EventEditViewModel;
            if (DateTime.TryParseExact(dtBox.Text, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                if (dtBox.Name == "StartTimeBox")
                {
                    viewModel.Event.StartDate = dt;
                }
                else
                {
                    viewModel.Event.EndDate = dt;
                }
            }

            if (viewModel.CheckEventModifiedCommand.CanExecute(null))
            {
                viewModel.CheckEventModifiedCommand.Execute(null);
            }
        }
    }
}
