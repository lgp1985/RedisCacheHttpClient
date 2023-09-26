using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;

[assembly: FunctionsStartup(typeof(RedisCacheHttpClient.Startup))]

namespace RedisCacheHttpClient;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddRedisHttpClient();
        builder.Services.AddScoped<IClientService,ClientService>();
    }
}
