using Moq;
using Newtonsoft.Json;
using Stocks.Application.Common.Interfaces;
using Stocks.Domain.Data;
using Stocks.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Application.UnitTests
{
    public class IEXCloudServiceTests
    {
        [Fact]
        public async Task IEXCloudService_GetHistoricalPrices_CanGetHistoricalPrices()
        {
            var mock = new Mock<IIEXCloudService>();
            mock.Setup(x => x.GetHistoricalPrices("spy", "5d"))
                .Returns(Task.FromResult(JsonConvert.DeserializeObject<List<StockPrice>>(
                File.ReadAllText("TestData" + Path.DirectorySeparatorChar + "spy-5d.json"))));
            Assert.NotNull(await mock.Object.GetHistoricalPrices("spy", "5d"));
        }

        [Fact]
        public async Task IEXCloudService_GetCandleStickChart_CanGetCandleStickChart()
        {
            var mock = new Mock<IIEXCloudService>();
            mock.Setup(x => x.GetCandleStickChart("spy", "5d"))
                .Returns(Task.FromResult(new CandleStickChart("spy", "5d", new[] { new StockPrice { } })));

            Assert.NotNull(await mock.Object.GetCandleStickChart("spy", "5d"));
        }
    }
}
