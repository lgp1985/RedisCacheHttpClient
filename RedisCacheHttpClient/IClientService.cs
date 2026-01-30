namespace RedisCacheHttpClient;

public interface IClientService
{
    public Task<HttpResponseMessage> GetTaskAsync(string query);
}