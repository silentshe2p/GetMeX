using System.Windows;

namespace GetMeX.ViewModels.Services
{
    public class ViewService
    {
        // Whether parent view initiated Close()
        private bool _parentClosing { get; set; }
        private Window _view { get; set; }

        public ViewService(Window w)
        {
            _view = w;
            _parentClosing = false;
            _view.Closing += HideView;
        }

        public void ShowDialog()
        {
            if (_view != null)
            {
                SetOwner();
                _view.ShowDialog();
            }
        }

        public void ShowView()
        {
            if (_view != null)
            {
                SetOwner();
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
            if (!_parentClosing)
            {
                e.Cancel = true;
                _view.Visibility = Visibility.Hidden;
            }
        }

        private void SetOwner() // Alt-tab will show both main and child window
        {
            if (_view.Owner == null)
            {
                _view.Owner = Application.Current.MainWindow;
            }
        }
    }
}
