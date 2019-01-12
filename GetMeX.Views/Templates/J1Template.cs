using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using Microsoft.Win32;
using GetMeX.ViewModels.VMs;

namespace GetMeX.Templates
{
    public partial class J1Template
    {
        public void J1BtnBrowseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (dialog.ShowDialog() == true)
            {
                Button b = sender as Button;
                J1fmViewModel m = b.DataContext as J1fmViewModel;
                m.FileToAppend = dialog.FileName;
            }
        }

        public void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            // The actual link is inside a TextBlock which is inside HyperLink
            var s = sender as TextBlock;
            Process.Start(new ProcessStartInfo(s?.Text));
            e.Handled = true;
        }
    }
}
