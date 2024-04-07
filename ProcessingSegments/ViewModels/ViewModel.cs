using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProcessingSegments.ViewModels
{
    internal abstract class ViewModel : EventHandlerCreator, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanget([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanget(propertyName);
            return true;
        }
    }
}
