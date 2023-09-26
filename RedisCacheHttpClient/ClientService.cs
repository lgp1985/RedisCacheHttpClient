using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedisCacheHttpClient;

public class ClientService : IClientService
{
    private readonly IHttpClientFactory httpClientFactory;

    public ClientService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> GetTaskAsync(string query)
    {
        var httpClient = httpClientFactory.CreateClientWithRedis();
        var query2 = new QueryBuilder
        {
            { "key", query }
        };
        var httpResponseMessage = await httpClient.GetAsync(query2.ToString());
        return httpResponseMessage;
    }
}
