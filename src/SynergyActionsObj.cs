using NP.Utilities;
using NP.Utilities.Attributes;
using NP.Utilities.Expressions;
using System.Reflection;

namespace NP.Synergy
{
    /// <summary>
    /// wraps a C# object with various (source and target) endpoints and possibly some actions
    /// </summary>
    internal class SynergyActionsObj : ISynergyActionObj
    {
        /// C# object wrapped by the SynergyActionObj
        private object ActionObj { get; }

        /// <summary>
        /// C# obj Type 
        /// </summary>
        internal Type ActionObjType { get; }

        // dictionary that maps property names into the corresponding
        // Actions that set the property value for this action object
        // that contains the property
        private Dictionary<string, IValueSetter> SourceSetters { get; } =
            new Dictionary<string, IValueSetter>();

        // dictionary that maps property names into the corresponding
        // Funcs that return the property value for this action object
        // that contains this property
        private Dictionary<string, IValueGetter> TargetGetters { get; } =
            new Dictionary<string, IValueGetter>();

        private void SetSourceSetter
        (
            string propName, 
            DataPointAttribute dataPointAttribute)
        {
            var sourceSetter = 
                new ActionObjPropSetter
                (
                    ActionObj, 
                    propName, 
                    dataPointAttribute.TriggersActionName);

            SourceSetters[propName] = sourceSetter;

            if (sourceSetter.TriggersActionName != null)
            {
                sourceSetter.ActionFiredEvent +=
                    SourceSetter_ActionFiredEvent;
            }
        }

        /// <summary>
        /// it fires after the action of name actionName had completed (if the action is syncronous)
        /// </summary>
        /// <param name="actionName"></param>
        private void SourceSetter_ActionFiredEvent(string actionName)
        {
            TargetGetters
                .Values
                .Where(targetGetter => targetGetter.ChangedByActionName == actionName)
                .ToList()
                .DoForEach(targetGetter => targetGetter.FireValueChanged());
        }

        private void SetTargetGetter
        (
            string propName, 
            DataPointAttribute dataPointAttribute)
        {
            var targetGetter = 
                new ActionObjPropGetter
                (
                    ActionObj, 
                    propName, 
                    dataPointAttribute.ChangedByActionName);

            TargetGetters[propName] = targetGetter;
        }

        internal SynergyActionsObj(object actionObj)
        {
            ActionObj = actionObj;

            ActionObjType = ActionObj.GetType();

            PropertyInfo[] propInfos =
                ActionObjType.GetProperties();

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
                    SetSourceSetter(propName, dataPointAttribute);
                }

                if (dataPointAttribute.Direction.IsTarget())
                {
                    SetTargetGetter(propName, dataPointAttribute);
                }
            }
        }


        private IValueSetter? GetActionSourceSetter(string propName)
        {
            if (!SourceSetters.TryGetValue(propName, out var setter))
                return null;

            return setter;
        }

        private IValueGetter? GetActionTargetGetter(string propName)
        {
            if (!TargetGetters.TryGetValue(propName, out var getter))
                return null;

            return getter;
        }


        private bool HasProp(string propName)
        {
            return HasGetter(propName) || HasSetter(propName);
        }

        private bool HasSetter(string propName)
        {
            return SourceSetters.ContainsKey(propName);
        }

        private bool HasGetter(string propName)
        {
            return TargetGetters.ContainsKey(propName);
        }

        // connects the property propName of the action object to the synergy assembly cell
        void ISynergyActionObj.ConnectWithCell
        (
            Cell cell, 
            string actionObjDataPointName)
        {
            object key = cell.Key;
            string behaviorTypeStr = ActionObjType.Sq();

            if (!HasProp(actionObjDataPointName))
            {
                $"PropertyInfo for property {actionObjDataPointName.Sq()} is not found for synergy assembly behavior object of type {behaviorTypeStr}".ThrowProgError();
            }

            if (cell.Direction.IsSource())
            {
                HasSetter(actionObjDataPointName)
                    .ThrowIfFalse($"Property {actionObjDataPointName.Sq()} on synergy assembly behavior object of type {behaviorTypeStr} is not a source as required for the cell {key.Sq()}");
            }

            if (cell.Direction.IsTarget())
            {
                HasGetter(actionObjDataPointName)
                    .ThrowIfFalse($"Property {actionObjDataPointName.Sq()} on synergy assembly behavior object of type {behaviorTypeStr} is not a target as required for the cell {key.Sq()}");
            }

            IValueSetter? sourceSetter = GetActionSourceSetter(actionObjDataPointName);
            if (sourceSetter != null)
            {
                sourceSetter.Set(cell.Value);
                cell.ActionSourcesFromCellValueSetters.Add(sourceSetter);
            }

            IValueGetter? targetGetter = GetActionTargetGetter(actionObjDataPointName);
            if (targetGetter != null)
            {
                object? val = targetGetter.Get();
                cell.SetVal(val);

                targetGetter.ValueChangedEvent += cell.SetVal;

                cell.TargetActionsValueGetters.Add((ActionObjPropGetter) targetGetter);
            }
        }
    }
}
