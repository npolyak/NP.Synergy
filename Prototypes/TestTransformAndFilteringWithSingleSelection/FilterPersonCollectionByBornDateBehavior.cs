using NP.Concepts.Behaviors;
using NP.Utilities;
using NP.Utilities.Attributes;
using System;

namespace TestTransformAndFilteringWithSingleSelection
{
    public class FilterPersonCollectionByBornDateBehavior : FilterCollectionBehavior<Person>
    {

        #region FromYear Property
        private int? _fromYear;
        [DataPoint(DataPointDirection.Source)]
        public int? FromYear
        {
            get
            {
                return this._fromYear;
            }
            set
            {
                if (this._fromYear == value)
                {
                    return;
                }

                this._fromYear = value;

                CreateFilter();
            }
        }
        #endregion FromYear Property


        #region ToYear Property
        private int? _toYear;
        [DataPoint(DataPointDirection.Source)]
        public int? ToYear
        {
            get
            {
                return this._toYear;
            }
            set
            {
                if (this._toYear == value)
                {
                    return;
                }

                this._toYear = value;

                CreateFilter();
            }
        }
        #endregion ToYear Property

        private void CreateFilter()
        {
            Func<Person, bool> fromYearFilter = p => true;

            if (FromYear != null)
            {
                fromYearFilter = p => p.Born > FromYear;
            }

            Func<Person, bool> toYearFilter = p => true;

            if (ToYear != null)
            {
                toYearFilter = p => p.Born < ToYear;
            }

            Filter = p => fromYearFilter(p) && toYearFilter(p);
        }

        public FilterPersonCollectionByBornDateBehavior()
        {
            CreateFilter();
        }
    }
}
