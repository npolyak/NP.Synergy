using Avalonia;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;

namespace NP.Synergy
{
    public class SynergyComposerPropertyAccessorPlugin : IPropertyAccessorPlugin
    {
        public bool Match(object obj, string propertyName)
        {
            return obj is Container sadeh && sadeh.GetCell(propertyName) is Cell cell && cell.IsBindable;
        }

        public IPropertyAccessor? Start(WeakReference<object?> reference, string propertyName)
        {
            reference.TryGetTarget(out var target);

            Container sadeh = (Container)target!;

            return sadeh.GetCell(propertyName) as BindableCell;
        }
    }

    public static class SadehPropertyAccessorPluginHelper
    {
        public static AppBuilder InsertSynergyPropertyAccessorPluginHelper(this AppBuilder appBuilder)
        {
            ExpressionObserver.PropertyAccessors.Insert(0, new SynergyComposerPropertyAccessorPlugin());

            return appBuilder;
        }
    }
}
