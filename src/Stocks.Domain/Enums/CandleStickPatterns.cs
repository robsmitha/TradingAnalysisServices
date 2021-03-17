using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stocks.Domain.Enums
{
    public enum CandleStickPatterns
    {
        [Description("Hammer")]
        Hammer,
        [Description("Inverted Hammer")]
        InvertedHammer,
        [Description("Bullish Engulfing")]
        BullishEngulfing,
        [Description("Piercing Line")]
        PiercingLine,
        [Description("Morning Star")]
        MorningStar
    }
}
