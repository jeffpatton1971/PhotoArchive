using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PhotoArchive.Functions;

public class GetPhotos
{
    private readonly ILogger<GetPhotos> _logger;

    public GetPhotos(ILogger<GetPhotos> logger)
    {
        _logger = logger;
    }

    [Function("GetPhotos")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
