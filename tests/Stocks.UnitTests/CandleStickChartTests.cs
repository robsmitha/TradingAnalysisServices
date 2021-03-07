using Newtonsoft.Json;
using Stocks.Domain.Data;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Stocks.UnitTests
{
    public class CandleStickChartTests
    {
        [Fact]
        public void Chart_Constructor_SymbolIsCapitalized()
        {
            var data = File.ReadAllText("Samples" + Path.DirectorySeparatorChar + "spy-5d.json");
            var stocks = JsonConvert.DeserializeObject<List<StockPrice>>(data);
            var chart = new CandleStickChart("spy", "5d", stocks);
            Assert.Equal("SPY", chart.Symbol);
        }

        [Fact]
        public void Chart_Constructor_CurrentRangeIsSet()
        {
            var data = File.ReadAllText("Samples" + Path.DirectorySeparatorChar + "spy-5d.json");
            var stocks = JsonConvert.DeserializeObject<List<StockPrice>>(data);
            var chart = new CandleStickChart("spy", "5d", stocks);
            Assert.Equal("5d", chart.Range.RangeValue);
            Assert.Equal(DateTime.Now.AddDays(5).Date, chart.Range.DateValue);
        }
    }
}
