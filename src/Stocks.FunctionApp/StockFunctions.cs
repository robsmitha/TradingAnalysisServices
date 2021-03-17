using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Stocks.Application.Commands;
using Stocks.Domain.Extensions;

namespace Stocks.FunctionApp
{
    public class StockFunctions
    {
        private readonly IMediator _mediator;
        public StockFunctions(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("AnalyzeSupportResistanceTrendLinesFunction")]
        public async Task AnalyzeSupportResistanceTrendLinesFunction([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"AnalyzeSupportResistanceTrendLinesFunction Timer trigger function executed at: {DateTime.Now}");
            var watchList = Environment.GetEnvironmentVariable("WatchListSymbols").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var response = await _mediator.Send(new AnalyzeSupportResistanceTrendLinesCommand(watchList));

            foreach (var call in response.Calls)
            {
                log.LogInformation($"CALL: {call.Symbol.Symbol} - close: {call.CloseLevel.Close}, support level: {call.SupportLevel.Low}, resistance level: {call.ResistanceLevel.High}, range: {call.Range.CurrentRange.GetEnumDescription()}");
            }

            foreach (var put in response.Puts)
            {
                log.LogInformation($"PUT: {put.Symbol.Symbol} - close: {put.CloseLevel.Close}, support level: {put.SupportLevel.Low}, resistance level: {put.ResistanceLevel.High}, range: {put.Range.CurrentRange.GetEnumDescription()}");
            }
        }

        [FunctionName("AnalyzeCandleStickPatternsFunction")]
        public async Task AnalyzeCandleStickPatternsFunction([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"AnalyzeCandleStickPatternsFunction Timer trigger function executed at: {DateTime.Now}");
            var watchList = Environment.GetEnvironmentVariable("WatchListSymbols").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var response = await _mediator.Send(new AnalyzeCandleStickPatternsCommand(watchList));

            foreach (var pattern in response.Patterns)
            {
                log.LogInformation($"Pattern: {pattern.Symbol.Symbol} {pattern.Pattern.GetEnumDescription()}");
            }
        }
    }
}
