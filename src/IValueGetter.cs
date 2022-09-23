namespace NP.Synergy
{
    internal interface IValueGetter
    {
        object? Get();

        public event Action<object?>? ValueChangedEvent;

        public string? ChangedByActionName { get; }

        internal void FireValueChanged();
    }
}
