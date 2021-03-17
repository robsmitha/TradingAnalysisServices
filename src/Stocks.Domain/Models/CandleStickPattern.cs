using Stocks.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Domain.Models
{
    public class CandleStickPattern
    {
        public ChartSymbol Symbol { get; set; }
        public ChartRange Range { get; set; }
        public CandleStickPatterns Pattern { get; set; }
        public CandleStickPattern(string symbol, string range, CandleStickPatterns pattern)
        {
            Symbol = new ChartSymbol(symbol);
            Range = new ChartRange(range);
            Pattern = pattern;
        }
    }
}
