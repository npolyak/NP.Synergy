using System.ComponentModel;

namespace NP.Synergy
{
    internal class BehaviorPropGetter
    {
        private readonly object _behavior;

        private readonly Func<object, object?> _getter;

        private string _propName;

        public event Action<object?>? ValueChangedEvent;

        public BehaviorPropGetter(object behavior, Func<object, object?> getter, string propName)
        {
            _behavior = behavior;
            _getter = getter;
            _propName = propName;

            if (_behavior is INotifyPropertyChanged notifiable)
            {
                notifiable.PropertyChanged += Notifiable_PropertyChanged;
            }
        }

        private void Notifiable_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _propName)
                ValueChangedEvent?.Invoke(Get());
        }

        public object? Get()
        {
            return _getter(_behavior);
        }
    }
}
