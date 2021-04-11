using Stocks.Domain.Data;
using Stocks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Application.Common.Interfaces
{
    public interface IIEXCloudService
    {
        Task<List<StockPrice>> GetHistoricalPrices(string symbol, string range);
        Task<CandleStickChart> GetCandleStickChart(string symbol, string range);
    }
}
