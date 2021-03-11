using Stocks.Domain.Enums;
using Stocks.Domain.Extensions;
using Stocks.Domain.Models;
using System;
using Xunit;

namespace Stocks.Domain.UnitTests
{
    public class ChartRangeTests
    {
        [Fact]
        public void ChartRange_Constructor_CurrentRangeIsSet()
        {
            var sut = new ChartRange("5d");
            Assert.True(sut.CurrentRange == ChartRanges.FiveDay);
        }

        [Fact]
        public void ChartRange_NextRange_CanGetNextRange()
        {
            var sut = new ChartRange("5d");
            Assert.Equal(ChartRanges.OneMonth.GetEnumDescription(), sut.NextRange);
        }

        [Fact]
        public void CandleStickChart_NextRange_MaxRangeThrowsIndexOutOfRangeException()
        {
            var sut = new ChartRange("5y");
            Assert.Throws<IndexOutOfRangeException>(() => sut.NextRange);
        }
    }
}
