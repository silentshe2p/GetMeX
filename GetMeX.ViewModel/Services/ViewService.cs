﻿using System.Windows;

namespace GetMeX.ViewModels.Services
{
    public class ViewService
    {
        // Whether to hide view instead of closing and creating new each time
        private bool _maintainView { get; set; }
        // Whether parent view initiated Close()
        private bool _parentClosing { get; set; }
        private Window _view { get; set; }

        public ViewService(Window w)
        {
            _view = w;
            _maintainView = false;
            _parentClosing = false;
            _view.Closing += HideView;
        }

        public void ShowDialog()
        {
            if (_view != null)
            {
                _view.ShowDialog();
            }
        }

        public void ShowView()
        {
            if (_view != null)
            {
                _maintainView = true;
                _view.Show();
            }
        }

        public void CloseView(bool parentClosing=false)
        {
            if (_view != null)
            {
                _parentClosing = parentClosing;
                _view.Close();
            }
        }

        private void HideView(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Hide the view instead of closing to be able to re-open later unless main program (parent) is closing
            if (_maintainView && !_parentClosing)
            {
                e.Cancel = true;
                _view.Visibility = Visibility.Hidden;
            }
        }
    }
}
