using NP.Utilities;
using NP.Utilities.Attributes;
using System;

namespace TestIntClassConverter
{
    public class StrToIntConverter
    {
        [DataPoint(DataPointDirection.Target, changedByActionName:"Convert")]
        public int TheVal { get; set; }

        [DataPoint(DataPointDirection.Source, triggersActionName:"Convert")]
        public string? TheStr { get; set; }

        [Action("Convert")]
        public void ConvertImpl()
        {
            TheVal = Convert.ToInt32(TheStr);
        }
    }
}
