using MediatR;
using Microsoft.Extensions.Logging;
using Stocks.Application.Common.Interfaces;
using Stocks.Domain.Enums;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Application.Commands
{
    public class AnalyzeCandleStickPatternsCommand : IRequest<AnalyzeCandleStickPatternsCommand.Response>
    {
        public HashSet<string> WatchList { get; set; }
        public string Range { get; set; }
        public AnalyzeCandleStickPatternsCommand(IEnumerable<string> watchList, string range = "5d")
        {
            if (watchList == null || !watchList.Any())
            {
                throw new ArgumentException($"Watchlist cannot be empty");
            }

            WatchList = watchList.ToHashSet();
            Range = range;
        }

        public class Handler : IRequestHandler<AnalyzeCandleStickPatternsCommand, Response>
        {
            private readonly ILogger<AnalyzeCandleStickPatternsCommand> _logger;
            private readonly IIEXCloudService _service;

            public Handler(ILogger<AnalyzeCandleStickPatternsCommand> logger, IIEXCloudService service)
            {
                _logger = logger;
                _service = service;
            }

            public async Task<Response> Handle(AnalyzeCandleStickPatternsCommand request, CancellationToken cancellationToken)
            {
                var patterns = new List<CandleStickPattern>();
                foreach (var symbol in request.WatchList)
                {
                    try
                    {
                        await ProcessCandleStickPatterns(symbol, request.Range);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        _logger.LogError($"{nameof(AnalyzeCandleStickPatternsCommand)}.{nameof(IndexOutOfRangeException)} Error while processing symbol: {symbol}", e);
                    }
                    catch (ArgumentException e)
                    {
                        _logger.LogError($"{nameof(AnalyzeCandleStickPatternsCommand)}.{nameof(ArgumentException)}: {e.Message} Error while processing symbol: {symbol}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{nameof(AnalyzeCandleStickPatternsCommand)}.{nameof(Exception)}: {e.Message} Error while processing symbol: {symbol}");
                    }
                }

                return new Response(patterns);

                async Task ProcessCandleStickPatterns(string symbol, string range)
                {
                    var chart = await _service.GetCandleStickChart(symbol, range);
                    var candles = new LinkedList<CandleStick>(chart.Candles);
                    for (var current = candles.First; current != null; current = current.Next)
                    {
                        if (current.IsHammer())
                        {
                            patterns.Add(new CandleStickPattern(symbol, range, CandleStickPatterns.Hammer));
                        }

                        if (current.IsInvertedHammer())
                        {
                            patterns.Add(new CandleStickPattern(symbol, range, CandleStickPatterns.InvertedHammer));
                        }

                        if (current.IsMorningStar())
                        {
                            patterns.Add(new CandleStickPattern(symbol, range, CandleStickPatterns.MorningStar));
                        }

                        if (current.IsPiercingLine())
                        {
                            patterns.Add(new CandleStickPattern(symbol, range, CandleStickPatterns.PiercingLine));
                        }

                        if (current.IsBullishEngulfing())
                        {
                            patterns.Add(new CandleStickPattern(symbol, range, CandleStickPatterns.BullishEngulfing));
                        }
                    }
                }
            }

        }

        public class Response
        {
            public List<CandleStickPattern> Patterns { get; set; }
            public Response(List<CandleStickPattern> patterns)
            {
                Patterns = patterns;
            }
        }
    }
}
