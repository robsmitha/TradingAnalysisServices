using Stocks.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Domain.Models
{
    public class CandleStickChart
    {
        public string Symbol { get; private set; }
        public Range Range { get; private set; }
        public List<Candle> Candles { get; private set; }
        
        public CandleStickChart(string symbol, string range, IEnumerable<StockPrice> stocks)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Symbol cannot be empty");
            }

            if (stocks == null)
            {
                throw new ArgumentNullException($"Stocks cannot be null for Symbol: {symbol}");
            }

            if (!stocks.Any())
            {
                throw new ArgumentException($"Stocks cannot be empty for Symbol: {symbol}");
            }

            Symbol = symbol.ToUpper();
            Range = new Range(range);
            Candles = stocks.Select(s => new Candle(s)).ToList();
        }

        public Candle ResistanceLevel => Candles
                .OrderByDescending(c => c.High)
                .ThenByDescending(c => c.Date)
                .FirstOrDefault();

        public Candle SupportLevel => Candles
                .OrderBy(c => c.Low)
                .ThenByDescending(c => c.Date)
                .FirstOrDefault();

        public Candle CloseLevel => Candles
            .OrderByDescending(c => c.Date)
            .FirstOrDefault();

        public decimal SupportToCloseDifference =>
            Math.Abs(SupportLevel.Low - CloseLevel.Close);
        public decimal ResistanceToCloseDifference =>
            Math.Abs(ResistanceLevel.High - CloseLevel.Close);

        public decimal SupportToResistanceRange => ResistanceLevel.High - SupportLevel.Low;
    }
}
