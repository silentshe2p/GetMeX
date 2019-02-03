using GetMeX.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GetMeX.Templates
{
    public partial class GoogleSearchTemplate
    {
        public void AutoComplete_TryStart(object sender, RoutedEventArgs e)
        {
            var queryBox = sender as TextBox;
            if (IsSuggestionAllowed(queryBox) == true)
            {
                var viewModel = queryBox.DataContext as GoogleSearchViewModel;
                if (viewModel.SuggestionCommand.CanExecute(null))
                    viewModel.SuggestionCommand.Execute(null);
            }
        }

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
