using Avalonia.Data;
using NP.Utilities;

namespace NP.Synergy
{
    internal class Cell
    {
        public DataPointDirection Direction { get; }

        public virtual bool IsBindable => false;

        public object? Key { get; }

        public Type? PropertyType { get; init; }

        internal List<BehaviorPropSetter> SourceSetters { get; } = new List<BehaviorPropSetter>();

        internal List<BehaviorPropGetter> TargetGetters { get; } = new List<BehaviorPropGetter>();

        object? _value;
        public object? Value 
        {
            get => _value;
            private set
            {
                if (_value == value)
                    return;

                _value = value;

                SetBehaviors();

                OnValueSet();
            }
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

        internal void AddBehavior(SynergyBehavior behavior, string propName)
        {
            string behaviorTypeStr = behavior.BehaviorType.Sq();

            if (!behavior.HasProp(propName))
            {
                $"PropertyInfo for property {propName.Sq()} is not found for synergy container behavior object of type {behaviorTypeStr}".ThrowProgError();
            }

            if (Direction == DataPointDirection.Source)
            {
                behavior.HasSetter(propName)
                    .ThrowIfFalse($"Property {propName.Sq()} on synergy container behavior object of type {behaviorTypeStr} is not a source as required for the cell {Key}");
            }

            if (Direction == DataPointDirection.Target)
            {
                behavior.HasGetter(propName)
                    .ThrowIfFalse($"Property {propName.Sq()} on synergy container behavior object of type {behaviorTypeStr} is not a target as required for the cell {Key}");
            }

            var sourceSetter = behavior.GetSetter(propName);  
            if (sourceSetter != null)
            {
                sourceSetter.Set(Value);
                SourceSetters.Add(sourceSetter);
            }

            var targetGetter = behavior.GetGetter(propName);
            if (targetGetter != null)
            {
                object? val = targetGetter.Get();
                Value = val;

                targetGetter.ValueChangedEvent += TargetGetter_ValueChangedEvent;

                TargetGetters.Add(targetGetter);
            }
        }

        private void TargetGetter_ValueChangedEvent(object? val)
        {
            Value = val;
        }

        protected void SetBehaviors()
        {
            SourceSetters.DoForEach(source => source.Set(Value));
        }
    }
}
