using Avalonia.Data.Core.Plugins;
using NP.Utilities;

namespace NP.Synergy
{
    internal class AvaloniaBindableCell : Cell, IPropertyAccessor
    {
        public override bool IsBindable => true;

        private Action<object?>? _listener;

        public AvaloniaBindableCell(object key, Type? propertyType, DataPointDirection direction) 
            : base(key, propertyType, direction)
        {
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                Unsubscribe();
            }
        }

        public void Subscribe(Action<object?> listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            if (_listener != null)
            {
                throw new InvalidOperationException(
                    "A member accessor can be subscribed to only once.");
            }

            _listener = listener;

            SendCurrentValue();
        }

        public void Unsubscribe()
        {
            if (_listener == null)
            {
                throw new InvalidOperationException(
                    "The member accessor was not subscribed.");
            }

            _listener = null;
        }

        private void SendCurrentValue()
        {
            object? val = this.Value;

            _listener?.Invoke(val);
        }

        protected override void OnValueSet()
        {
            SendCurrentValue();
        }
    }
}
