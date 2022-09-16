using NP.Utilities;
using NP.Utilities.Attributes;
using System.ComponentModel;

namespace TestIntMethodConverter
{
    public static class IntConverterUtils
    {
        public static string IntToStr([TriggerSource] this int i) => i.ToString();

        public static int StrToInt([TriggerSource] this string str) => int.Parse(str);
    }
}
