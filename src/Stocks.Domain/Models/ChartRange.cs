using Stocks.Domain.Enums;
using Stocks.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Stocks.Domain.Models
{
    public class ChartRange
    {
        public ChartRanges CurrentRange { get; set; }
        public ChartRange(string range)
        {
            if (!Ranges.ContainsKey(range))
            {
                throw new ArgumentException($"Invalid range: {range}");
            }

            CurrentRange = Ranges[range];
        }

        private Dictionary<string, ChartRanges> Ranges => new List<ChartRanges>
        {
            ChartRanges.Max,
            ChartRanges.FiveYear,
            ChartRanges.TwoYear,
            ChartRanges.OneYear,
            ChartRanges.YearToDate,
            ChartRanges.SixMonth,
            ChartRanges.ThreeMonth,
            ChartRanges.OneMonth,
            ChartRanges.FiveDay,
            ChartRanges.OneMonthThirtyMinIntervals,
            ChartRanges.FiveDayTenMinIntervals,
            ChartRanges.Date,
            ChartRanges.Dynamic
        }.ToDictionary(r => r.GetEnumDescription(), r => r);

        public string NextRange
        {
            get
            {
                var nextValue = (int)CurrentRange + 1;
                if (!Enum.IsDefined(typeof(ChartRanges), nextValue))
                {
                    throw new IndexOutOfRangeException($"{nextValue} is not a defined range");
                }

                var validRanges = new[] {
                    ChartRanges.FiveDay,
                    ChartRanges.OneMonth,
                    ChartRanges.ThreeMonth,
                    ChartRanges.SixMonth,
                    ChartRanges.OneYear,
                    ChartRanges.TwoYear,
                    ChartRanges.FiveYear
                };

                if (!validRanges.Contains((ChartRanges)nextValue))
                {
                    throw new IndexOutOfRangeException($"{nextValue} is not in the valid list of ranges");
                }

                var nextRange = (ChartRanges)nextValue;
                return nextRange.GetEnumDescription();
            }
        }

    }
}
