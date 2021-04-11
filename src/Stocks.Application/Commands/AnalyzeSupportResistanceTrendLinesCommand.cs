using MediatR;
using Microsoft.Extensions.Logging;
using Stocks.Application.Common.Interfaces;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Application.Commands
{
    public class AnalyzeSupportResistanceTrendLinesCommand : IRequest<AnalyzeSupportResistanceTrendLinesCommand.Response>
    {
        public HashSet<string> WatchList { get; set; }
        public string Range { get; set; }
        public AnalyzeSupportResistanceTrendLinesCommand(IEnumerable<string> watchList, string range = "5d")
        {
            if (watchList == null || !watchList.Any())
            {
                throw new ArgumentException($"Watchlist cannot be empty");
            }

            WatchList = watchList.ToHashSet();
            Range = range;
        }

        public class Handler : IRequestHandler<AnalyzeSupportResistanceTrendLinesCommand, Response>
        {
            private readonly ILogger<AnalyzeSupportResistanceTrendLinesCommand> _logger;
            private readonly IIEXCloudService _service;

            public Handler(ILogger<AnalyzeSupportResistanceTrendLinesCommand> logger, IIEXCloudService service)
            {
                _logger = logger;
                _service = service;
            }

            public async Task<Response> Handle(AnalyzeSupportResistanceTrendLinesCommand request, CancellationToken cancellationToken)
            {
                var possiblePuts = new List<CandleStickChart>();
                var possibleCalls = new List<CandleStickChart>();
                foreach (var symbol in request.WatchList)
                {
                    try
                    {
                        await ProcessCandleStickChart(symbol, request.Range);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        _logger.LogError($"{nameof(AnalyzeSupportResistanceTrendLinesCommand)}.{nameof(IndexOutOfRangeException)} Error while processing symbol: {symbol}", e);
                    }
                    catch (ArgumentException e)
                    {
                        _logger.LogError($"{nameof(AnalyzeSupportResistanceTrendLinesCommand)}.{nameof(ArgumentException)}: {e.Message} Error while processing symbol: {symbol}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{nameof(AnalyzeSupportResistanceTrendLinesCommand)}.{nameof(Exception)}: {e.Message} Error while processing symbol: {symbol}");
                    }
                }

                return new Response(possibleCalls, possiblePuts);

                async Task ProcessCandleStickChart(string symbol, string range)
                {
                    var chart = await _service.GetCandleStickChart(symbol, range);
                    _logger.LogInformation(@$"
chart.CloseLevel.Close={chart.CloseLevel.Close},
chart.SupportLevel.Low={chart.SupportLevel.Low},
chart.ResistanceLevel.High={chart.ResistanceLevel.High},
");

                    if(chart.Start.Close < chart.End.Close)
                    {
                        //gone up
                        //go look for resistance

                    }

                    if (chart.Start.Close > chart.End.Close)
                    {
                        //gone down
                        //go look for support

                    }

                    // edge case: Close level is less than support
                    if (chart.SupportToCloseDifference > 0)
                    {
                        await ProcessCandleStickChart(symbol, chart.Range.NextRange);
                        return;
                    }

                    // edge case: Close level is greater than resistance
                    if (chart.ResistanceToCloseDifference < 0)
                    {
                        await ProcessCandleStickChart(symbol, chart.Range.NextRange);
                        return;
                    }

                    //todo: determine threshold
                    var callThreshold = 2M;
                    var putThreshold = 2M;

                    //reaching resistance
                    if (Math.Abs(chart.SupportToCloseDifference) > chart.ResistanceToCloseDifference
                        && chart.ResistanceToCloseDifference / chart.SupportToResistanceRange < callThreshold)
                    {
                        possiblePuts.Add(chart);
                        return;
                    }

                    //falling back on support
                    if (chart.ResistanceToCloseDifference > Math.Abs(chart.SupportToCloseDifference)
                        && Math.Abs(chart.SupportToCloseDifference) / chart.SupportToResistanceRange < putThreshold)
                    {
                        possibleCalls.Add(chart);
                        return;
                    }

                    await ProcessCandleStickChart(symbol, chart.Range.NextRange);
                }
            }

        }

        public class Response
        {
            public List<CandleStickChart> Calls { get; set; }
            public List<CandleStickChart> Puts { get; set; }
            public Response(List<CandleStickChart> calls, List<CandleStickChart> puts)
            {
                Calls = calls;
                Puts = puts;
            }
        }
    }
}
