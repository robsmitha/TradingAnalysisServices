using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stocks.Domain.UnitTests
{
    public class ChartSymbolTests
    {
        [Fact]
        public void ChartSymbol_Constructor_SymbolIsCapitalized()
        {
            var sut = new ChartSymbol("spy");
            Assert.Equal("SPY", sut.Symbol);
        }
    }
}
