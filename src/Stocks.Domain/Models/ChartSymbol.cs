using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Domain.Models
{
    public class ChartSymbol
    {
        public string Symbol { get; set; }
        public ChartSymbol(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Symbol cannot be empty");
            }

            Symbol = symbol.ToUpper();
        }
    }
}
