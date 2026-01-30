using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace RedisCacheHttpClient;

public class Function1(ILogger<Function1> logger, IClientService clientService)
{
    [Function("Function1")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }

    [Function(nameof(Boredapi))]
    public async Task<IActionResult> Boredapi([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "bored")] HttpRequest httpRequest, CancellationToken cancellationToken)
    {
        if (httpRequest is null)
        {
            throw new ArgumentNullException(nameof(httpRequest));
        }
        var key = httpRequest.Query["key"];
        if (key.Count == 0)
        {
            return new BadRequestObjectResult($"Missing query paramenter: {nameof(key)}");
        }
        var httpResponseMessage = await clientService.GetTaskAsync(key!);
        var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);
        return new FileStreamResult(stream, Application.Json);
    }
}