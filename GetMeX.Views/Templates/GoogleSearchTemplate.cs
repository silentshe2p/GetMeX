using GetMeX.ViewModels.VMs;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GetMeX.Templates
{
    public partial class GoogleSearchTemplate
    {
        /// Get suggestions for current input if suggestion box is checked
        private void AutoComplete_TryStart(object sender, RoutedEventArgs e)
        {
            var queryBox = sender as TextBox;
            if (IsSuggestionAllowed(queryBox) == true)
            {
                var viewModel = queryBox.DataContext as GoogleSearchViewModel;
                if (viewModel.SuggestionCommand.CanExecute(null))
                {
                    viewModel.SuggestionCommand.Execute(null);
                }
            }
        }

        /// Copy suggestion to input box
        private void InputBox_UseSuggestion(object sender, RoutedEventArgs e)
        {
            // Make sure the control clicked on is TextBlock but not Border
            if (e.OriginalSource is TextBlock suggestion)
            {
                var suggestionList = sender as ListBox;
                var viewModel = suggestionList.DataContext as GoogleSearchViewModel;
                if (viewModel.SetQueryCommand.CanExecute(suggestion.Text))
                {
                    viewModel.SetQueryCommand.Execute(suggestion.Text);
                }
            }
        }

        private void InputBox_UseSuggestion_EnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var suggestionList = sender as ListBox;
                var suggestion = suggestionList.SelectedValue;
                var viewModel = suggestionList.DataContext as GoogleSearchViewModel;
                if (viewModel.SetQueryCommand.CanExecute(suggestion))
                {
                    viewModel.SetQueryCommand.Execute(suggestion);
                }
            }
        }

        /// Check whether suggestion check box is enabled and checked
        private bool? IsSuggestionAllowed(TextBox tb)
        {
            bool? value = null;
            StackPanel main = (StackPanel)tb.Parent;
            if (main.Parent is Grid grid)
            {
                var suggestionBox = grid.Children.OfType<CheckBox>().FirstOrDefault(x => x.Name == "SuggestionCheckBox");
                if (suggestionBox != null)
                {
                    value = suggestionBox.IsChecked;
                }
            }
            return value;
        }
    }
}
