using Avalonia.Data;
using NP.Utilities;

namespace NP.Synergy
{
    internal class Cell
    {
        public DataPointDirection Direction { get; }

        public virtual bool IsBindable => false;

        public object Key { get; }

        public Type? PropertyType { get; init; }

        // fire when the cell value changes. set the sources of the actions
        // that start at the cell
        internal List<ActionObjPropSetter> ActionSourcesFromCellValueSetters { get; } = 
            new List<ActionObjPropSetter>();

        // each one of the target getters set the Value
        // of the cell with the result of the action
        // actions ending at the cell after they fire. 
        internal List<ActionObjPropGetter> TargetActionsValueGetters { get; } = 
            new List<ActionObjPropGetter>();

        object? _value;
        public object? Value 
        {
            get => _value;
            private set
            {
                if (_value == value)
                    return;

                _value = value;

                FireAllSetActions();

                OnValueSet();
            }
        }

        internal void SetVal(object? value)
        {
            Value = value; 
        }

        protected virtual void OnValueSet()
        {

        }

        public Cell(object key, Type? propertyType, DataPointDirection direction)
        {
            Key = key;
            PropertyType = propertyType;
            Direction = direction;
        }

        public bool SetValue(object? value, BindingPriority priority)
        {
            Value = value;

            return true;
        }

        protected void FireAllSetActions()
        {
            ActionSourcesFromCellValueSetters.DoForEach(source => source.Set(Value));
        }
    }
}
