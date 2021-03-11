using Stocks.Domain.Data;
using Stocks.Domain.Enums;
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
        public void CandleStickChart_Constructor_EmptyStocksThrowsArugmentException()
        {
            Assert.Throws<ArgumentException>(() => 
            new CandleStickChart("spy", "5d", new List<StockPrice>()));
        }
    }
}
