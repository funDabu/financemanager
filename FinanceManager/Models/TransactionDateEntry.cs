using System.Globalization;
using FinanceManager.Data;

namespace FinanceManager.Models;

public class TransactionDateEntry: TransactionEntry
{
    /// <summary>
    /// Date format in standard .NET datetime format string.
    /// </summary>
    public string DateFormat { get; set; } = "dd.MM.yyyy";
    
    public override EntryType EntryType { get; }
    
    public TransactionDateEntry(bool nullable = true)
    {
        EntryType = new EntryType(EntryValueType.Date, nullable);
    }
    public TransactionDateEntry(string value, bool nullable = true) : this(nullable)
    {
        ValueString = value;
    }

    public sealed override bool ValidateValue(string value)
    {
        if (Nullable && value == Constants.NullString)
        {
            return true;
        }
        
        return DateTime.TryParseExact(value,DateFormat,null, DateTimeStyles.None, out _ );
    }

    protected override decimal StringValueToNumber(string stringValue)
    {
        return DateTime.ParseExact(stringValue, DateFormat, null).ToFileTime();
    }

    protected override string NumberValueToString(double numberValue)
    {
        return  DateTime.FromFileTime((long) numberValue).ToString(DateFormat);
    }

    protected override int CompareToSameType(TransactionEntry other)
    {
        return (ValueNumberOrNull, other.ValueNumberOrNull) switch
        {
            (null, null) => 0,
            (null, _) => -1,
            (_, null) => 1,
            _ => Math.Sign((decimal)(ValueNumberOrNull - other.ValueNumberOrNull))

        };
    }

    public override object Clone()
    {
        return new TransactionDateEntry(ValueString, EntryType.Nullable);
    }

    protected override string ValidationErrorMessage => $"date must be in format {DateFormat}";
}