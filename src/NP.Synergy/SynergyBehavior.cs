using NP.Utilities;
using NP.Utilities.Attributes;
using NP.Utilities.Expressions;
using System.Reflection;

namespace NP.Synergy
{
    internal class SynergyBehavior
    {
        internal object Behavior { get; }

        internal Type BehaviorType => Behavior.GetType();

        internal SynergyBehavior(object behavior)
        {
            Behavior = behavior;
            Type behaviorType = behavior.GetType();

            PropertyInfo[] propInfos = 
                behaviorType.GetProperties();

            foreach (PropertyInfo propInfo in propInfos)
            {
                string propName = propInfo.Name;

                DataPointAttribute dataPointAttribute =
                    propInfo.GetAttr<DataPointAttribute>();

                if (dataPointAttribute == null)
                {
                    continue;
                }

                if (dataPointAttribute.Direction.IsSource())
                {
                    Action<object, object?> setterAction =
                        behaviorType.GetUntypedCSPropertySetterByObjType(propName);

                    SourceSetters[propName] = setterAction;
                }

                if (dataPointAttribute.Direction.IsTarget())
                {
                    Func<object, object?> getterFunc =
                        behaviorType.GetUntypedCSPropertyGetterByObjType(propName);

                    TargetGetters[propName] = getterFunc;
                }
            }
        }

        internal Dictionary<string, Action<object, object?>> SourceSetters { get; } = 
            new Dictionary<string, Action<object, object?>>();

        internal Dictionary<string, Func<object, object?>> TargetGetters { get; } = 
            new Dictionary<string, Func<object, object?>>();

        internal BehaviorPropSetter? GetSetter(string propName)
        {
            if (!SourceSetters.TryGetValue(propName, out var setter))
                return null;

            return new BehaviorPropSetter(Behavior, setter);    
        }

        internal BehaviorPropGetter? GetGetter(string propName)
        {
            if (!TargetGetters.TryGetValue(propName, out var getter))
                return null;

            return new BehaviorPropGetter(Behavior, getter, propName);
        }


        internal bool HasProp(string propName)
        {
            return HasGetter(propName) || HasSetter(propName);
        }

        internal bool HasSetter(string propName)
        {
            return SourceSetters.ContainsKey(propName);
        }

        internal bool HasGetter(string propName)
        {
            return TargetGetters.ContainsKey(propName);
        }
    }
}
