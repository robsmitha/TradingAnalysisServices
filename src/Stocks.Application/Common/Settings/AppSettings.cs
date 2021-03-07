using Stocks.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Application.Common.Settings
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
    }
}
