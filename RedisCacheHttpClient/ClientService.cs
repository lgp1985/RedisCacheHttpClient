using Microsoft.AspNetCore.Http.Extensions;

namespace RedisCacheHttpClient;

public class ClientService(IHttpClientFactory httpClientFactory) : IClientService
{
    public async Task<HttpResponseMessage> GetTaskAsync(string query)
    {
        var httpClient = httpClientFactory.CreateClientWithRedis();
        var query2 = new QueryBuilder
        {
            { "key", query }
        };
        var httpResponseMessage = await httpClient.GetAsync(query2.ToString());

        // TODO: here you can do other processing with the response

        return httpResponseMessage;
    }
}
