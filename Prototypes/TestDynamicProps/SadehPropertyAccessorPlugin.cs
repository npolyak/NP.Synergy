using Avalonia;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System;

namespace TestDynamicProps
{
    public class SadehPropertyAccessorPlugin : IPropertyAccessorPlugin
    {
        public bool Match(object obj, string propertyName)
        {
            return obj is Sadeh sadeh && sadeh.GetCell(propertyName) is Cell cell && cell.IsBindable;
        }

        public IPropertyAccessor? Start(WeakReference<object?> reference, string propertyName)
        {
            reference.TryGetTarget(out var target);

            Sadeh sadeh = (Sadeh)target!;

            return sadeh.GetCell(propertyName) as BindableCell;
        }
    }

    public static class SadehPropertyAccessorPluginHelper
    {
        public static AppBuilder InsertSadehPropertyAccessorPluginHelper(this AppBuilder appBuilder)
        {
            ExpressionObserver.PropertyAccessors.Insert(0, new SadehPropertyAccessorPlugin());

            return appBuilder;
        }
    }
}
