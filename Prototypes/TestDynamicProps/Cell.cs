using Avalonia.Data;
using System;

namespace TestDynamicProps
{
    public class Cell 
    {
        public virtual bool IsBindable => false;

        public object? Key { get; }

        public Type? PropertyType { get; init; }

        object? _value;
        public object? Value 
        {
            get => _value;
            private set
            {
                if (_value == value)
                    return;

                _value = value;

                OnValueSet();
            }
        }

        protected virtual void OnValueSet()
        {

        }

        public Cell(object key, Type? propertyType)
        {
            Key = key;
            PropertyType = propertyType;
        }

        public bool SetValue(object? value, BindingPriority priority)
        {
            Value = value;

            return true;
        }
    }
}
