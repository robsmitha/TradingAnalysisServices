using Stocks.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Stocks.Domain.Models
{
    public enum Ranges
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

    public class Range
    {
        public Ranges Value { get; set; }
        public Range(string range)
        {
            if (!ranges.ContainsKey(range))
            {
                throw new ArgumentException($"Invalid range: {range}");
            }

            Value = ranges[range];
        }

        private Dictionary<string, Ranges> ranges => new List<Ranges>
        {
            Ranges.Max,
            Ranges.FiveYear,
            Ranges.TwoYear,
            Ranges.OneYear,
            Ranges.YearToDate,
            Ranges.SixMonth,
            Ranges.ThreeMonth,
            Ranges.OneMonth,
            Ranges.FiveDay,
            Ranges.OneMonthThirtyMinIntervals,
            Ranges.FiveDayTenMinIntervals,
            Ranges.Date,
            Ranges.Dynamic
        }.ToDictionary(r => r.GetEnumDescription(), r => r);

        public string NextRange
        {
            get
            {
                var nextValue = (int)Value + 1;
                if (!Enum.IsDefined(typeof(Ranges), nextValue))
                {
                    throw new IndexOutOfRangeException($"{nextValue} is not a defined range");
                }

                var validRanges = new[] {
                    Ranges.FiveDay,
                    Ranges.OneMonth,
                    Ranges.ThreeMonth,
                    Ranges.SixMonth,
                    Ranges.OneYear,
                    Ranges.TwoYear,
                    Ranges.FiveYear
                };

                if (!validRanges.Contains((Ranges)nextValue))
                {
                    throw new IndexOutOfRangeException($"{nextValue} is not in the valid list of ranges");
                }

                var nextRange = (Ranges)nextValue;
                return nextRange.GetEnumDescription();
            }
        }

    }
}
