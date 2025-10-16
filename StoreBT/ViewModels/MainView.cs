using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoreBT.ViewModels
{
    public class MainView : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainView()
        {
            // Mặc định hiển thị HomeView
            CurrentView = new Views.HomeView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
