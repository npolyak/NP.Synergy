using System;
using System.Linq;
using System.Reflection;

namespace NP.Utilities.Attributes
{
    public static class SynergyHelper
    {
        public static MethodInfo GetActionMethodInfo
        (
            this Type objType, 
            string actionName)
        {
            MethodInfo methodInfo =
                       objType.GetMethods()
                              .Single(m => m.GetAttr<ActionAttribute>()?.Name == actionName);

            return methodInfo;
        }
    }
}
