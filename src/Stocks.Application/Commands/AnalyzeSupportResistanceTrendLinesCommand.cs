﻿using MediatR;
using Microsoft.Extensions.Logging;
using Stocks.Application.Common.Interfaces;
using Stocks.Application.Common.Services;
using Stocks.Domain.Data;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Application.Commands
{
    public class AnalyzeSupportResistanceTrendLinesCommand : IRequest<AnalyzeSupportResistanceTrendLinesCommand.Response>
    {
        //TODO: Combine Symbol/Timespan model
        public List<string> WatchList { get; set; }
        public string Range { get; set; }
        public AnalyzeSupportResistanceTrendLinesCommand(IEnumerable<string> watchList, string range = "5d")
        {
            WatchList = watchList?.ToList() ?? new List<string>();
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
                var puts = new List<CandleStickChart>();
                var calls = new List<CandleStickChart>();
                foreach (var symbol in request.WatchList)
                {
                    try
                    {
                        await ProcessCandleStickChart(symbol, request.Range);
                    }
                    catch (ArgumentException e)
                    {
                        _logger.LogError($"{nameof(AnalyzeSupportResistanceTrendLinesCommand)}.{nameof(Handler)}.{nameof(Handle)}.{nameof(ArgumentException)}: {e.Message}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{nameof(AnalyzeSupportResistanceTrendLinesCommand)}.{nameof(Handler)}.{nameof(Handle)}.{nameof(Exception)}: {e.Message}");
                    }
                }

                return new Response(calls, puts);

                async Task ProcessCandleStickChart(string symbol, string range)
                {
                    var prices = await _service.GetHistoricalPrices(symbol, range);
                    var chart = new CandleStickChart(symbol, range, prices);

                    if (chart.SupportLevel.Low == chart.CloseLevel.Close)
                    {
                        //call process candle
                        await ProcessCandleStickChart(symbol, range);
                    }

                    if (chart.SupportToCloseDifference == chart.ResistanceToCloseDifference)
                    {
                        //edge case: stock has not moved..
                        return;
                    }

                    if (chart.SupportToCloseDifference > chart.ResistanceToCloseDifference)
                    {
                        if (chart.ResistanceToCloseDifference / chart.SupportToResistanceRange < .2M)
                        {
                            puts.Add(chart);
                        }
                    }
                    else
                    {
                        if (chart.SupportToCloseDifference / chart.SupportToResistanceRange < .2M)
                        {
                            calls.Add(chart);
                        }
                    }
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
