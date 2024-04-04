using FinanceManager.Data;

namespace FinanceManager.Models;

public class TransactionCurrencyType : TransactionTextEntry
{
    public override EntryType EntryType { get; }
    
    public TransactionCurrencyType(bool nullable = true)
    {
        EntryType = new EntryType(EntryValueType.Currency, nullable);
    }

    private TransactionCurrencyType(string value, bool nullable = true) : this(nullable)
    {
        ValueString = value;
    }
    
}