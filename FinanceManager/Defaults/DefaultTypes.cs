using System.Collections.ObjectModel;
using FinanceManager.Data;

namespace FinanceManager.Defaults;

public static class DefaultTypes
{
    private static readonly string[] DefNames = new[]
    {
        "Date",
        "Counterpart Name",
        "IBAN",
        "BIC",
        "Counterpart Account",
        "Counterpart Bank Code",
        "Amount",
        "Currency",
        "Message For Me",
        "Transaction ID",
        "Owner Name",
        "Owner Account",

    };

    public static ReadOnlyCollection<string> EntryNames => DefNames.AsReadOnly();


    private static readonly EntryType DateType = new EntryType(EntryValueType.Date, false);
    private static readonly EntryType TextType = new EntryType(EntryValueType.Text, false);
    private static readonly EntryType NullableTextType = new EntryType(EntryValueType.Text, true);
    private static readonly EntryType NumType = new EntryType(EntryValueType.Number, false);
    private static readonly EntryType NumNullableType = new EntryType(EntryValueType.Number, true);
    private static readonly EntryType CurrencyType = new EntryType(EntryValueType.Currency, false);

    private static readonly EntryType[] DefTypes = new[]
    {
        DateType,
        NullableTextType,
        NullableTextType,
        NullableTextType,
        NullableTextType,
        NumNullableType,
        NumType,
        CurrencyType,
        NullableTextType,
        TextType,
        TextType,
        TextType,
    };

    public static ReadOnlyCollection<EntryType> EntryTypes => DefTypes.AsReadOnly();

    public static TransactionType TransactionType { get; } = new TransactionType(DefNames, DefTypes);
}