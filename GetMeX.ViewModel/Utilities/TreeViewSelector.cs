using System.Windows;
using System.Windows.Controls;
using GetMeX.Models;

namespace GetMeX.ViewModels.Utilities
{
    public class TreeViewSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate BranchTemplate { get; set; }

        public DataTemplate LeafTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return (item is GXEvent) ? LeafTemplate : BranchTemplate;
        }

    }
}
