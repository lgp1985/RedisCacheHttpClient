using System.Net.Http;
using System.Threading.Tasks;

namespace RedisCacheHttpClient;

public interface IClientService
{
    public Task<HttpResponseMessage> GetTaskAsync(string query);
}