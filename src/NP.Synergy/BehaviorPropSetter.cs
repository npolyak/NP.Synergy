namespace NP.Synergy
{
    internal class BehaviorPropSetter
    {
        private readonly object _behavior;

        private readonly Action<object, object?> _setter;

        internal BehaviorPropSetter(object behavior, Action<object, object?> setter)
        {
            _behavior = behavior;
            _setter = setter;
        }

        internal void Set(object? value)
        {
            _setter.Invoke(_behavior, value);
        }
    }
}
