using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stocks.Domain.Enums
{
    public enum ChartRanges
    {
        [Description("5d")]
        FiveDay = 1,
        [Description("1m")]
        OneMonth = 2,
        [Description("3m")]
        ThreeMonth = 3,
        [Description("6m")]
        SixMonth = 4,
        [Description("1y")]
        OneYear = 5,
        [Description("2y")]
        TwoYear = 6,
        [Description("5y")]
        FiveYear = 7,

        [Description("max")]
        Max = 8,
        [Description("ytd")]
        YearToDate = 9,
        [Description("1mm")]
        OneMonthThirtyMinIntervals = 10,
        [Description("5dm")]
        FiveDayTenMinIntervals = 11,
        [Description("date")]
        Date = 12,
        [Description("dynamic")]
        Dynamic = 13
    }

}
