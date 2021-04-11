using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Stocks.Application.Commands;
using Stocks.Domain.Extensions;
using Stocks.Infrastructure.Settings;

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
        public async Task AnalyzeSupportResistanceTrendLinesFunction([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer,
            [CosmosDB(CosmosDbSettings.DatabaseName, CosmosDbSettings.WatchListSymbols, ConnectionStringSetting = CosmosDbSettings.ConnectionStringName,
                SqlQuery ="SELECT * FROM c WHERE c.UserID='%default_user_id%' ORDER BY c._ts DESC")] IEnumerable<dynamic> watchListSymbols,
             ILogger log)
        {
            log.LogInformation($"AnalyzeSupportResistanceTrendLinesFunction Timer trigger function executed at: {DateTime.Now}");
            var watchList = Environment.GetEnvironmentVariable("WatchListSymbols").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var response = await _mediator.Send(new AnalyzeSupportResistanceTrendLinesCommand(watchList));

            foreach (var call in response.Calls)
            {
                //TODO: add mapper in command
                var data = new
                {
                    call.Symbol.Symbol,
                    call.StartDate,
                    call.EndDate,
                    Range = call.Range.CurrentRange.GetEnumDescription(),
                    CloseLevelClose = call.CloseLevel.Close,
                    SupportLevelLow = call.SupportLevel.Close,
                    ResistanceLevelHigh = call.ResistanceLevel.Close,
                    call.SupportToResistanceRange,
                    ChartAnalysis = "CALL"
                };

                log.LogInformation($"CALL: {call.Symbol.Symbol} - close: {call.CloseLevel.Close}, support level: {call.SupportLevel.Low}, resistance level: {call.ResistanceLevel.High}, range: {call.Range.CurrentRange.GetEnumDescription()}");
            }

            foreach (var put in response.Puts)
            {
                log.LogInformation($"PUT: {put.Symbol.Symbol} - close: {put.CloseLevel.Close}, support level: {put.SupportLevel.Low}, resistance level: {put.ResistanceLevel.High}, range: {put.Range.CurrentRange.GetEnumDescription()}");
            }
        }

        [FunctionName("AnalyzeCandleStickPatternsFunction")]
        public async Task AnalyzeCandleStickPatternsFunction([TimerTrigger("0 */5 * * * *", RunOnStartup = false)] TimerInfo myTimer,
            [CosmosDB(CosmosDbSettings.DatabaseName, CosmosDbSettings.WatchListSymbols, ConnectionStringSetting = CosmosDbSettings.ConnectionStringName,
                SqlQuery ="SELECT * FROM c WHERE c.UserID='%default_user_id%' ORDER BY c._ts DESC")] IEnumerable<dynamic> watchListSymbols, 
            ILogger log)
        {
            log.LogInformation($"AnalyzeCandleStickPatternsFunction Timer trigger function executed at: {DateTime.Now}");
            var watchList = watchListSymbols?.Select(wl => (string)wl.Symbol) ?? Environment.GetEnvironmentVariable("WatchListSymbols").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var response = await _mediator.Send(new AnalyzeCandleStickPatternsCommand(watchList));

            foreach (var pattern in response.Patterns)
            {
                log.LogInformation($"Pattern: {pattern.Symbol.Symbol} {pattern.Pattern.GetEnumDescription()}");
            }
        }
    }
}
