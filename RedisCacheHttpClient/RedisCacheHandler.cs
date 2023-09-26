using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RedisCacheHttpClient;

internal class RedisCacheHandler : DelegatingHandler
{
    private readonly IDistributedCache distributedCache;

    public RedisCacheHandler(IDistributedCache distributedCache)
    {
        this.distributedCache = distributedCache;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var key = request.RequestUri.ParseQueryString()["key"].ToLowerInvariant();
        var bytes = await distributedCache.GetAsync(key, cancellationToken);
        if (bytes == null)
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            _ = Task.Run(async () =>
            {
                await httpResponseMessage.Content.LoadIntoBufferAsync();
                var bytes1 = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken);
                await distributedCache.SetAsync(key, bytes1, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) }, cancellationToken);
            }, cancellationToken);
            return httpResponseMessage;
        }
        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StreamContent(new System.IO.MemoryStream(bytes)), RequestMessage = request };
    }
}