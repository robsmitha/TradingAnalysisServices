using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Domain.Models
{
    //public enum Ranges
    //{
    //    Max = 1,
    //    FiveYear = 2,
    //    TwoYear = 3,
    //    OneYear = 4,
    //    YearToDate = 5,
    //    SixMonth = 6,
    //    ThreeMonth = 7,
    //    OneMonth = 8,
    //    OneMonthThirtyMinIntervals = 9,
    //    FiveDay = 10,
    //    FiveDayTenMinIntervals = 11,
    //    Date = 12,
    //    Dynamic = 13
    //}
    //public enum Intervals
    //{
    //    Day = 1,
    //    TenMinute = 2,
    //    ThirtyMinute = 3
    //}
    
    public class Range
    {
        private readonly Dictionary<string, DateTime> ranges = new Dictionary<string, DateTime>{
            { "max", DateTime.MaxValue.Date },
            { "5y", DateTime.Now.AddYears(5).Date },
            { "2y", DateTime.Now.AddYears(2).Date },
            { "1y", DateTime.Now.AddYears(1).Date },
            { "ytd", DateTime.MinValue.Date },
            { "6m", DateTime.Now.AddMonths(5).Date },
            { "3m", DateTime.Now.AddMonths(3).Date },
            { "1m", DateTime.Now.AddMonths(1).Date },
            { "5d", DateTime.Now.AddDays(5).Date },

            { "1mm", DateTime.Now.AddMonths(1).Date },
            { "5dm", DateTime.Now.AddDays(5).Date },

            { "date", DateTime.MinValue.Date },
            { "dynamic", DateTime.MinValue.Date },
        };
        public string RangeValue { get; set; }
        public DateTime DateValue { get; set; }
        public Range(string range)
        {
            if (!ranges.ContainsKey(range))
            {
                throw new ArgumentException($"Invalid range: {range}");
            }
            DateValue = ranges[range];
            RangeValue = range;
            //defaults
            //Interval = Intervals.Day;
            //CurrentRange = Ranges.OneMonth;

            //switch (range.ToLower())
            //{
            //    case "max":
            //        CurrentRange = Ranges.Max;
            //        break;
            //    case "5y":
            //        CurrentRange = Ranges.FiveYear;
            //        break;
            //    case "2y":
            //        CurrentRange = Ranges.TwoYear;
            //        break;
            //    case "1y":
            //        CurrentRange = Ranges.OneYear;
            //        break;
            //    case "ytd":
            //        CurrentRange = Ranges.YearToDate;
            //        break;
            //    case "6m":
            //        CurrentRange = Ranges.SixMonth;
            //        break;
            //    case "3m":
            //        CurrentRange = Ranges.ThreeMonth;
            //        break;
            //    case "1mm":
            //        CurrentRange = Ranges.OneMonthThirtyMinIntervals;
            //        Interval = Intervals.ThirtyMinute;
            //        break;
            //    case "5d":
            //        CurrentRange = Ranges.FiveDay;
            //        break;
            //    case "5dm":
            //        CurrentRange = Ranges.FiveDayTenMinIntervals;
            //        Interval = Intervals.TenMinute;
            //        break;
            //    case "date":
            //        CurrentRange = Ranges.Date;
            //        break;
            //    case "dynamic":
            //        CurrentRange = Ranges.Dynamic;
            //        break;
            //}
        }
    }
}
