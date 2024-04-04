using FinanceManager.Data;
using FinanceManager.Models;

namespace FinanceManager.Configuration;

public class ColumnDefinition(
    string columnName,
    EntryValueType dataValueType = EntryValueType.Text,
    Alignment alignment = Alignment.NotSet,
    string caption = Constants.UninitializedString,
    bool displayed = true)
{
    public string ColumnName { get; set; } = columnName;
    public string Caption { get; set; } = caption;
    public EntryValueType DataValueType { get; set; } = dataValueType;
    public Alignment Alignment { get; set; } = alignment;
    public bool Displayed = displayed;

    public static List<ColumnDefinition> FromTransactionType(TransactionType transactionType)
    {
        return transactionType.Select(tuple => new ColumnDefinition(
                tuple.Item1, dataValueType: tuple.Item2.ValueType, caption: tuple.Item1)
            ).ToList();
    }
}