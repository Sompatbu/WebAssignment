using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Mime;
using System.Xml.Linq;
using WebAssignment.Server.Enums;
using WebAssignment.Server.Extension;
using WebAssignment.Server.Models.DTOs;
using WebAssignment.Server.Models.Request;
using WebAssignment.Server.Models.Response;
using WebAssignment.Server.Services;

namespace WebAssignment.Server.Controllers;

[ApiController]
[Route("transaction")]
public class TransactionController(TransactionService transactionService) : ControllerBase
{
    [HttpPost("upload")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<BaseResponse<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BaseResponse<TransactionDto>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadTransactionFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            List<TransactionDto> transactionDtos = [];

            if (file.FileName.EndsWith(".xml"))
            {
                using var stream = new StreamReader(file.OpenReadStream());
                var document = XDocument.Load(stream);
                var transactions = document.Descendants("Transaction");

                int recordIndex = 1;
                foreach (var transaction in transactions)
                {
                    var dto = new TransactionDto
                    {
                        TransactionId = transaction.Attribute("id")?.Value,
                        TransactionDate = DateTimeOffset.TryParseExact(transaction.Element("TransactionDate")?.Value, "yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset dateValue) ? dateValue : null,
                        AccountNumber = transaction.Element("PaymentDetails")?.Element("AccountNo")?.Value,
                        Amount = decimal.TryParse(transaction.Element("PaymentDetails")?.Element("Amount")?.Value, out decimal amountValue) ? amountValue : null,
                        CurrencyCode = transaction.Element("PaymentDetails")?.Element("CurrencyCode")?.Value,
                        Status = transaction.Element("Status")?.Value switch
                        {
                            "Approved" => TransactionStatus.Approved,
                            "Rejected" => TransactionStatus.Rejected,
                            "Done" => TransactionStatus.Done,
                            _ => null,
                        },
                    };

                    var errors = dto.ValidateData(recordIndex);
                    dto.Error = errors.Count > 0 ? [.. errors] : null;
                    recordIndex++;
                    transactionDtos.Add(dto);
                }
            }
            else if (file.FileName.EndsWith(".csv"))
            {
                using var stream = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true });

                await csv.ReadAsync();
                csv.ReadHeader();
                int recordIndex = 1;
                while (await csv.ReadAsync())
                {
                    var dto = new TransactionDto
                    {
                        TransactionId = csv.GetField(0),
                        AccountNumber = csv.GetField(1),
                        Amount = decimal.TryParse(csv.GetField(2)?.Replace(",", string.Empty), out decimal amount) ? amount : 0,
                        CurrencyCode = csv.GetField(3),
                        TransactionDate = DateTimeOffset.TryParseExact(csv.GetField(4), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset transactionDate) ? transactionDate : default,
                        Status = csv.GetField(5) switch
                        {
                            "Approved" => TransactionStatus.Approved,
                            "Failed" => TransactionStatus.Rejected,
                            "Finished" => TransactionStatus.Done,
                            _ => null,
                        },
                    };

                    var errors = dto.ValidateData(recordIndex);
                    dto.Error = errors.Count > 0 ? [.. errors] : null;
                    recordIndex++;
                    transactionDtos.Add(dto);
                }
            }
            else
            {
                return BadRequest("Unknown format.");
            }

            if (transactionDtos.Any(item => item.Error is not null))
            {
                return BadRequest(new BaseResponse<TransactionDto>(transactionDtos.Where(item => item.Error is not null).SelectMany(item => item.Error!).ToArray()));
            }

            var response = await transactionService.InsertTransactionsAsync(transactionDtos);
            return response ? Ok(new BaseResponse<bool>(true)) : StatusCode(500, "Internal server error");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<BaseResponse<TransactionResponseData>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BaseResponse<TransactionResponseData>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTransactionsAsync(
        [FromQuery] string? currencyCode, 
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        [FromQuery] TransactionStatus? status)
    {
        var filter = new TransactionFilterRequest()
        {
            CurrencyCode = currencyCode,
            From = from,
            To = to,
            Status = status
        };

        var response = await transactionService.FindTransactionsAsync(filter);

        return Ok(response);
    }
}
