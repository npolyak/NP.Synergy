using NP.Utilities;
using NP.Utilities.Attributes;
using NP.Utilities.Expressions;
using System.Reflection;

namespace NP.Synergy
{
    internal class SynergyStaticMethodActionObj : ISynergyActionObj
    {
        private MethodInfo ActionMethodInfo { get; }

        public Type MethodContainingType => ActionMethodInfo.DeclaringType!;

        // dictionary that maps property names into the corresponding
        // Actions that set the property value for this action object
        // that contains the property
        private IValueSetter[] _sourceSetters;

        // dictionary that maps property names into the corresponding
        // Funcs that return the property value for this action object
        // that contains this property
        private ActionObjPropGetter[] _targetGetters;

        private Func<object[], object> _methodWrap;

        internal SynergyStaticMethodActionObj(Type methodContainingClassType, string actionName)
        {
            ActionMethodInfo = methodContainingClassType.GetActionMethodInfo(actionName);

            ActionMethodInfo
                .IsStatic
                .ThrowIfFalse
                (
                    $"method {ActionMethodInfo.Name.Sq()} within type {methodContainingClassType.Name.Sq()} for action {actionName.Sq()} is not static.");

            _methodWrap = ActionMethodInfo.GetParamArrayLambdaForReturningMethod();

            foreach (var paramInfo in ActionMethodInfo.GetParameters())
            {
                bool isSource = !paramInfo.IsOut;

                if (isSource)
                {
                    SetSourceSetter(paramInfo);
                }

                bool isTarget = paramInfo.IsOut || paramInfo.ParameterType.IsByRef;
            }
        }

        private void SetSourceSetter(ParameterInfo paramInfo)
        {
            
        }

        /// connects the property propName of the action object to the synergy assembly cell
        void ISynergyActionObj.ConnectWithCell(Cell cell, string? actionObjDataPointName)
        {
            ParameterInfo paramInfo = 
                actionObjDataPointName == null 
                    ? 
                    ActionMethodInfo.ReturnParameter : 
                        ActionMethodInfo.GetParameters().Single(p => p.Name == actionObjDataPointName);

            DataPointAttribute dataPointAttr = 
                paramInfo.GetAttr<DataPointAttribute>();

            //paramInfo.
        }
    }
}
