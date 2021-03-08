using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Application;
using Stocks.Application.Common.Interfaces;
using Stocks.Infrastructure.Services;
using Stocks.Infrastructure.Settings;

[assembly: FunctionsStartup(typeof(Stocks.FunctionApp.Startup))]

namespace Stocks.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddApplication();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IIEXCloudService, IEXCloudService>();

            builder.Services.AddOptions<AppSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("AppSettings").Bind(settings);
            });
        }
    }
}
