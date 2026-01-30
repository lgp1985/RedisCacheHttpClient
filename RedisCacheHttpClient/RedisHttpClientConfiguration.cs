using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace RedisCacheHttpClient;

public static class RedisHttpClientConfiguration
{
    public static readonly string RedisHttpClient = nameof(RedisHttpClient);
    private static readonly string baseAddress = "https://pokeapi.co/api/v2/";
    public static IServiceCollection AddRedisHttpClient(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(setupAction =>
        {
            setupAction.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
            {
                EndPoints = { {"localhost", 6379 } }
            };
        });
        services.AddScoped<RedisCacheHandler>();
        services.AddHttpClient(RedisHttpClient, configureClient =>
        {
            configureClient.BaseAddress = new Uri(baseAddress);
        });
        services.Configure<HttpClientFactoryOptions>(RedisHttpClient, options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(builder =>
            {
                builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<RedisCacheHandler>());
            });
        });
        return services;
    }
    public static HttpClient CreateClientWithRedis(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(RedisHttpClient);
    }
}
