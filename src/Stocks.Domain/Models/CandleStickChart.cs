using Stocks.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Domain.Models
{
    public class CandleStickChart
    {
        public ChartSymbol Symbol { get; private set; }
        public ChartRange Range { get; private set; }
        public List<CandleStick> Candles { get; private set; }
        
        public CandleStickChart(string symbol, string range, IEnumerable<StockPrice> stocks)
        {
            if (stocks == null)
            {
                throw new ArgumentNullException($"Stocks cannot be null for Symbol: {symbol}");
            }

            if (!stocks.Any())
            {
                throw new ArgumentException($"Stocks cannot be empty for Symbol: {symbol}");
            }

            Symbol = new ChartSymbol(symbol);
            Range = new ChartRange(range);
            Candles = stocks.Select(s => new CandleStick(s)).ToList();
        }

        public CandleStick Start => Candles
                .OrderBy(c => c.Date)
                .FirstOrDefault();
        public CandleStick End => Candles
                .OrderByDescending(c => c.Date)
                .FirstOrDefault();

        public CandleStick ResistanceLevel => Candles
                .OrderByDescending(c => c.High)
                .ThenByDescending(c => c.Date)
                .FirstOrDefault();

        public CandleStick SupportLevel => Candles
                .OrderBy(c => c.Low)
                .ThenByDescending(c => c.Date)
                .FirstOrDefault();

        public CandleStick CloseLevel => Candles
            .OrderByDescending(c => c.Date)
            .FirstOrDefault();

        public decimal SupportToCloseDifference =>
            SupportLevel.Low - CloseLevel.Close;
        public decimal ResistanceToCloseDifference =>
            ResistanceLevel.High - CloseLevel.Close;

        public decimal SupportToResistanceRange => ResistanceLevel.High - SupportLevel.Low;

        public DateTime StartDate => Candles
            .OrderBy(c => c.Date)
            .FirstOrDefault()?.Date ?? DateTime.Now.AddDays(-1);

        public DateTime EndDate => Candles
            .OrderByDescending(c => c.Date)
            .FirstOrDefault()?.Date ?? DateTime.Now;
    }
}
