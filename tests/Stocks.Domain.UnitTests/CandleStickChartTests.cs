using Stocks.Domain.Data;
using Stocks.Domain.Extensions;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stocks.Domain.UnitTests
{
    public class CandleStickChartTests
    {
        [Fact]
        public void CandleStickChart_Constructor_SymbolIsCapitalized()
        {
            var sut = new CandleStickChart("spy", "5d", Enumerable.Range(0, 1).Select(x => new StockPrice { Date = DateTime.Now }));
            Assert.Equal("SPY", sut.Symbol);
        }

        [Fact]
        public void CandleStickChart_Constructor_RangeValueIsSet()
        {
            var sut = new CandleStickChart("spy", "5d", Enumerable.Range(0,1).Select(x => new StockPrice { Date = DateTime.Now }));
            Assert.Equal(Ranges.FiveDay, sut.Range.Value);
        }

        [Fact]
        public void CandleStickChart_Constructor_EmptyStocksThrowsArugmentException()
        {
            Assert.Throws<ArgumentException>(() => 
            new CandleStickChart("spy", "5d", new List<StockPrice>()));
        }

        [Fact]
        public void CandleStickChart_NextRange_CanGetNextRange()
        {
            var sut = new CandleStickChart("spy", "5d", Enumerable.Range(0, 1).Select(x => new StockPrice { Date = DateTime.Now }));
            Assert.Equal(Ranges.OneMonth.GetEnumDescription(), sut.Range.NextRange);
        }

        [Fact]
        public void CandleStickChart_NextRange_MaxRangeThrowsIndexOutOfRangeException()
        {
            var sut = new CandleStickChart("spy", "5y", Enumerable.Range(0, 1).Select(x => new StockPrice { Date = DateTime.Now }));
            Assert.Throws<IndexOutOfRangeException>(() => sut.Range.NextRange);
        }
    }
}
