using FinanceManager.Data;
using FinanceManager.Defaults;

namespace FinanceManager.Models;

/// <summary>
/// Factory class for <c>ITransactionEntry</c> objects.
/// </summary>
public class TransactionEntryFactory
{

    public TransactionEntryFactory()
    {
    }
    
    public TransactionEntryFactory(string dateFormat)
    {
        DateFormat = dateFormat;
    }

    /// <summary>
    /// Date format in standard .NET datetime format string
    /// that will be used for transaction entries of date value type.
    /// </summary>
    public string DateFormat { get; set; } = DefaultFormats.DateFormat;

    public ITransactionEntry? Create(EntryType entryType, string stringValue, out string? validationErrorMessages)
    {
        var entry = CreateUninitialized(entryType);
        return entry.TrySetValue(stringValue, out validationErrorMessages) ? entry : null;
    }
    
    private ITransactionEntry CreateUninitialized(EntryType entryType)
    {
        switch (entryType.ValueType)
        {
            case EntryValueType.Number:
                return new TransactionNumberEntry(entryType.Nullable);
            case EntryValueType.Text:
                return new TransactionTextEntry(entryType.Nullable);
            case EntryValueType.Currency:
                return new TransactionCurrencyType(entryType.Nullable);
            case EntryValueType.Date:
                var result = new TransactionDateEntry(entryType.Nullable)
                {
                    DateFormat = DateFormat
                };
                return result;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public ITransactionEntry Duplicate(ITransactionEntry entry)
    {
        var result = CreateUninitialized(entry.EntryType);
        result.ValueString = entry.ValueString;
        return result;
    }
}