using Microsoft.Extensions.Caching.Distributed;
using System.Web;

namespace RedisCacheHttpClient;

internal class RedisCacheHandler(IDistributedCache distributedCache) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var key = HttpUtility.ParseQueryString(request.RequestUri.Query)["key"]?.ToLowerInvariant();
        var bytes = await distributedCache.GetAsync(key, cancellationToken);
        if (bytes == null)
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            await httpResponseMessage.Content.LoadIntoBufferAsync(cancellationToken);
            var bytes1 = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken);
            await distributedCache.SetAsync(key, bytes1, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) }, cancellationToken);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StreamContent(new System.IO.MemoryStream(bytes1)), RequestMessage = request };

        }
        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StreamContent(new System.IO.MemoryStream(bytes)), RequestMessage = request };
    }
}