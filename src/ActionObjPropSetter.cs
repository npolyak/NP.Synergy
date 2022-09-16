using NP.Utilities;
using NP.Utilities.Attributes;
using NP.Utilities.Expressions;
using System.Reflection;

namespace NP.Synergy
{
    internal class ActionObjPropSetter : IValueSetter
    {
        private readonly object _actionObj;

        private readonly Action<object, object?> _setter;

        private readonly Action? _actionToTrigger = null;

        public event Action<string>? ActionFiredEvent;

        public string PropName { get; }    

        internal string? TriggersActionName { get; }

        internal ActionObjPropSetter(object actionObj, string propName, string? triggersActionName)
        {
            _actionObj = actionObj;

            // for debugging purpose
            PropName = propName;

            Type actionObjType = actionObj.GetType();   

            _setter =
                actionObjType.GetUntypedCSPropertySetterByObjType(propName);

            TriggersActionName = triggersActionName;

            if (triggersActionName != null)
            {
                MethodInfo triggeredMethodInfo = actionObjType.GetActionMethodInfo(triggersActionName);

                _actionToTrigger =
                    (Action) triggeredMethodInfo.GetCompiledMethodCallLambda(actionObj);
            }
        }

        public void Set(object? value)
        {
            _setter.Invoke(_actionObj, value);

            _actionToTrigger?.Invoke();

            if (TriggersActionName != null)
            {
                ActionFiredEvent?.Invoke(TriggersActionName);
            }
        }
    }
}
