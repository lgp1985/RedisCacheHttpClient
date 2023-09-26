using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System;
using System.Net.Http;

namespace RedisCacheHttpClient;

public static class RedisHttpClientConfiguration
{
    public static string RedisHttpClient = nameof(RedisHttpClient);
    private static readonly string baseAddress = "http://www.boredapi.com/api/activity";
    public static IServiceCollection AddRedisHttpClient(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(setupAction =>
        {
            setupAction.Configuration = "localhost:6379";
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
                builder.AdditionalHandlers.Add(builder.Services.GetService<RedisCacheHandler>());
            });
        });
        return services;
    }
    public static HttpClient CreateClientWithRedis(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(RedisHttpClient);
    }
}
