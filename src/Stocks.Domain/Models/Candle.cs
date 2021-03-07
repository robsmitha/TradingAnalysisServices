using Stocks.Domain.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Domain.Models
{
    public class Candle
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public decimal ChangeOverTime { get; set; }

        public decimal LowerShadow => Close - Low;
        public decimal UpperShadow => High - Open;
        public bool HasShortBody => Math.Abs(ChangePercent) < 1 && Math.Abs(ChangePercent) > 0;

        public Candle(StockPrice stockPrice)
        {
            Date = stockPrice.Date.Value.UtcDateTime;

            Open = stockPrice.Open ?? 0;
            Close = stockPrice.Close ?? 0;
            High = stockPrice.High ?? 0;
            Low = stockPrice.Low ?? 0;
            Volume = stockPrice.Volume ?? 0;

            Change = stockPrice.Change ?? 0;
            ChangePercent = stockPrice.ChangePercent ?? 0;
            ChangeOverTime = stockPrice.ChangeOverTime ?? 0;
        }
    }
}
