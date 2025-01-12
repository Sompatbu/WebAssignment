using WebAssignment.Server.Entities;
using WebAssignment.Server.Models.DTOs;
using WebAssignment.Server.Models.Response;

namespace WebAssignment.Server.Extension;

public static class TransactionExtensions
{
    public static TransactionEntity ToTransactionEntity(this TransactionDto source)
    {
        return new()
        {
            TransactionId = source.TransactionId!,
            AccountNumber = source.AccountNumber!,
            Amount = source.Amount!.Value,
            CurrencyCode = source.CurrencyCode!,
            TransactionDate = source.TransactionDate!.Value,
            Status = source.Status!.Value,
        };
    }

    public static TransactionResponseData ToTransactionResponseData(this TransactionEntity source)
    {
        return new()
        {
            TransactionId = source.TransactionId,
            Payment = $"{source.Amount} {source.CurrencyCode}",
            Status = nameof(source.Status)[..1],
        };
    }

    public static List<ErrorResponseData> ValidateData(this TransactionDto source, int index)
    {
        List<ErrorResponseData> errors = [];
        if (string.IsNullOrEmpty(source.TransactionId))
            errors.Add(new ErrorResponseData(400, $"Row No. {index} Transaction ID is missing."));
        if (string.IsNullOrEmpty(source.AccountNumber))
            errors.Add(new ErrorResponseData(400, $"Row No. {index} Account number is missing."));
        if (!source.Amount.HasValue)
            errors.Add(new ErrorResponseData(400, $"Row No. {index} Amount is missing or invalid."));
        if (string.IsNullOrEmpty(source.CurrencyCode))
            errors.Add(new ErrorResponseData(400, $"Row No. {index} Currency code is missing."));
        if (!source.TransactionDate.HasValue)
            errors.Add(new ErrorResponseData(400, $"Row No. {index} Transaction date is missing or invalid."));
        if (!source.Status.HasValue)
            errors.Add(new ErrorResponseData(400, $"Row No. {index} Status is missing or invalid."));

        return errors;
    }
}
