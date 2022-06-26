using NP.Utilities;
using NP.Utilities.Attributes;
using System.ComponentModel;

namespace TestIntMethodConverter
{
    public static class IntConverterUtils
    {
        [return: DataPoint(DataPointDirection.Target)]
        public static string IntToStr([DataPoint(DataPointDirection.Source)]this int i) => i.ToString();

        [return: DataPoint(DataPointDirection.Target, changedByActionName:nameof(StrToInt))]
        [Action(nameof(StrToInt))]
        public static int StrToInt([DataPoint(DataPointDirection.Source, triggersActionName:nameof(StrToInt))]this string str) => int.Parse(str);
    }
}
