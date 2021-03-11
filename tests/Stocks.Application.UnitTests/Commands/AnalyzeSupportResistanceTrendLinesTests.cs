using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Stocks.Application.Commands;
using Stocks.Application.Common.Interfaces;
using Stocks.Domain.Data;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Application.UnitTests.Commands
{
    public class AnalyzeSupportResistanceTrendLinesTests
    {
        [Fact]
        public async Task AnalyzeSupportResistanceTrendLinesCommand_Handler_ProcessCandleStickChart()
        {
            var iexCloudServiceMock = new Mock<IIEXCloudService>();
            iexCloudServiceMock.Setup(x => x.GetHistoricalPrices("spy", "5d"))
                .Returns(Task.FromResult(JsonConvert.DeserializeObject<List<StockPrice>>(
                File.ReadAllText("TestData" + Path.DirectorySeparatorChar + "spy-5d.json"))));
            var loggerMock = new Mock<ILogger<AnalyzeSupportResistanceTrendLinesCommand>>();         
            
            var handler = new AnalyzeSupportResistanceTrendLinesCommand.Handler(loggerMock.Object, iexCloudServiceMock.Object);
            var command = new AnalyzeSupportResistanceTrendLinesCommand(new[] { "spy" }, "5d"); 
            var response = await handler.Handle(command, new System.Threading.CancellationToken());
            
            Assert.NotNull(response.Calls);
            Assert.NotNull(response.Puts);
        }
    }
}
