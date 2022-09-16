using NP.Utilities.Expressions;
using System.ComponentModel;

namespace NP.Synergy
{
    internal class ActionObjPropGetter : IValueGetter
    {
        private readonly object _actionObj;

        private readonly Func<object, object?> _getter;

        public string PropName { get; }

        public event Action<object?>? ValueChangedEvent;

        public string? ChangedByActionName { get; }

        void IValueGetter.FireValueChanged()
        {
            ValueChangedEvent?.Invoke(Get());
        }

        public ActionObjPropGetter
        (
            object actionObj, 
            string propName, 
            string? changedByActionName)
        {
            _actionObj = actionObj;
            Type actionObjType = actionObj.GetType();
            _getter = actionObjType.GetUntypedCSPropertyGetterByObjType(propName);

            PropName = propName;
            ChangedByActionName = changedByActionName;

            if ( (ChangedByActionName == null) && 
                 (_actionObj is INotifyPropertyChanged notifiable) )
            {
                notifiable.PropertyChanged += Notifiable_PropertyChanged;
            }
        }

        private void Notifiable_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropName)
                ((IValueGetter)this).FireValueChanged();
        }

        public object? Get()
        {
            return _getter(_actionObj);
        }
    }
}
