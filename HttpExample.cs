using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BasicHttpTrigger
{
    public class HttpExample
    {
        private readonly ILogger<HttpExample> _logger;

        public HttpExample(ILogger<HttpExample> logger)
        {
            _logger = logger;
        }

        [Function("HttpExample")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Get name from query string or request body
            string name = req.Query["name"];

            if (string.IsNullOrEmpty(name))
            {
                string requestBody = await new System.IO.StreamReader(req.Body).ReadToEndAsync();
                if (!string.IsNullOrEmpty(requestBody))
                {
                    dynamic data = System.Text.Json.JsonSerializer.Deserialize<dynamic>(requestBody);
                    name = data?.GetProperty("name").GetString();
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                return new OkObjectResult("Welcome to Azure Functions! Please enter your name.");
            }

            return new OkObjectResult($"Hello, {name}! Your Azure Function executed successfully.");
        }
    }
}