using System.Globalization;
using FinanceManager.Data;

namespace FinanceManager.Models;

public class TransactionTextEntry : TransactionEntry
{
    public override EntryType EntryType { get; }


    public TransactionTextEntry(bool nullable = true)
    {
        EntryType = new EntryType(EntryValueType.Text, nullable);
    }

    private TransactionTextEntry(string value, bool nullable = true) : this(nullable)
    {
        ValueString = value;
    }

    public override bool ValidateValue(string value)
    {
        return Nullable || value != Constants.NullString;
    }

    protected override decimal StringValueToNumber(string stringValue)
    {
        return 0;
    }

    protected override string NumberValueToString(double numberValue)
    {
        return ValueString;
    }

    protected override string ValidationErrorMessage => 
        Nullable ? "must be text" : $"must non-null text, i.g. different from {Constants.NullString}";

    protected override int CompareToSameType(TransactionEntry other)
    {
        return string.Compare(ValueString, other.ValueString, StringComparison.InvariantCulture);
    }

    public override object Clone()
    {
        return new TransactionTextEntry(ValueString, EntryType.Nullable);
    }
}