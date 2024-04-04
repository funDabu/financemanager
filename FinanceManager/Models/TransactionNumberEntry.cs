using System.Globalization;
using System.Text.RegularExpressions;
using FinanceManager.Data;

namespace FinanceManager.Models;

public class TransactionNumberEntry: TransactionEntry
{
    private static readonly IFormatProvider NumberFormat = CultureInfo.InvariantCulture.NumberFormat;

    public override EntryType EntryType { get; }
    
    public TransactionNumberEntry(bool nullable = true)
    {
        EntryType = new EntryType(EntryValueType.Number, nullable);
    }

    public TransactionNumberEntry(decimal? value, bool nullable = true) : this(nullable)
    {
        ValueNumberOrNull = value;
    }
    
    public override bool ValidateValue(string valueString)
    {
        if (Nullable && valueString == Constants.NullString)
        {
            return true;
        }
        return double.TryParse(FixValueString(valueString), NumberFormat, out _ );
    }

    protected override decimal StringValueToNumber(string stringValue)
    {
        return decimal.Parse(FixValueString(stringValue), NumberFormat);
    }

    protected override string NumberValueToString(double numberValue)
    {
        return numberValue.ToString(NumberFormat);
    }
    
    private static string FixValueString(string valueString)
    {
        
        return valueString.Replace('\u00A0',' ')
            .Replace(" ", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(",", ".");
    }

    protected override string ValidationErrorMessage =>
        "must be a number, i.e. must contain only digits, spaces, comma, or a dot for decimal part separation";

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
        return new TransactionNumberEntry(ValueNumberOrNull, EntryType.Nullable);
    }
}