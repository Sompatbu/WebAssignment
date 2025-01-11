using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;

namespace WebAssignment.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadTransactionFileAsync()
        {
            //if (!Request.Content.IsMimeMultipartContent())
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            //var provider = new MultipartMemoryStreamProvider();
            //await Request.Content.ReadAsMultipartAsync(provider);
            //foreach (var file in provider.Contents)
            //{
            //    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            //    var buffer = await file.ReadAsByteArrayAsync();
            //    //Do whatever you want with filename and its binary data.
            //}

            return Ok();
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //var something = XElement.Load();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
