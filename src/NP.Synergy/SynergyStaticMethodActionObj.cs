using NP.Utilities;
using NP.Utilities.Attributes;
using System.Reflection;

namespace NP.Synergy
{
    internal class SynergyStaticMethodActionObj : ISynergyActionObj
    {
        private MethodInfo ActionMethodInfo { get; }

        public Type MethodContainerType => ActionMethodInfo.DeclaringType!;

        internal SynergyStaticMethodActionObj(Type methodContainingClassType, string actionName)
        {
            ActionMethodInfo = methodContainingClassType.GetActionMethodInfo(actionName);

            ActionMethodInfo
                .IsStatic
                .ThrowIfFalse($"method {ActionMethodInfo.Name.Sq()} within type {methodContainingClassType.Name.Sq()} for action {actionName.Sq()} is not static.");

            
        }

        /// connects the property propName of the action object to the container cell
        void ISynergyActionObj.ConnectWithCell(Cell cell, string? actionObjDataPointName)
        {
            ParameterInfo paramInfo = 
                actionObjDataPointName == null ? ActionMethodInfo.ReturnParameter : ActionMethodInfo.GetParameters().Single(p => p.Name == actionObjDataPointName);

            DataPointAttribute dataPointAttr = 
                paramInfo.GetAttr<DataPointAttribute>();

            //paramInfo.
        }
    }
}
