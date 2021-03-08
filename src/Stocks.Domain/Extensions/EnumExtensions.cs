using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Stocks.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription<TEnum>(this TEnum item) => item.GetType()
              .GetField(item.ToString())
              .GetCustomAttributes(typeof(DescriptionAttribute), false)
              .Cast<DescriptionAttribute>()
              .FirstOrDefault()?.Description ?? nameof(TEnum);
    }
}
