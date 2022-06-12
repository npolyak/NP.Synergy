using Avalonia;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;

namespace NP.Synergy
{
    public class SynergyAvaloniaContainerPropertyAccessorPlugin : IPropertyAccessorPlugin
    {
        public bool Match(object obj, string keyStr)
        {
            return obj is Container container && container.GetCellByKeyStr(keyStr) is Cell cell && cell.IsBindable;
        }

        public IPropertyAccessor? Start(WeakReference<object?> reference, string propertyName)
        {
            reference.TryGetTarget(out var target);

            Container sadeh = (Container)target!;

            return sadeh.GetCellByKeyStr(propertyName) as AvaloniaBindableCell;
        }
    }

    public static class SadehPropertyAccessorPluginHelper
    {
        public static AppBuilder InsertSynergyPropertyAccessorPluginHelper(this AppBuilder appBuilder)
        {
            ExpressionObserver.PropertyAccessors.Insert(0, new SynergyAvaloniaContainerPropertyAccessorPlugin());

            return appBuilder;
        }
    }
}
