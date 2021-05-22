using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingStore.Extensions
{
    public class YearRangeDateAttribute : RangeAttribute
    {
        public YearRangeDateAttribute(int StartYear):base(StartYear,DateTime.Today.Year)
        {

        }

    }
}
