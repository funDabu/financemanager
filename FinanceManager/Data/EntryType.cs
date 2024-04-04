namespace FinanceManager.Data;

/// <summary>
/// Class defining the type of transaction entries.
/// </summary>
/// <param name="ValueType">Defines the value type of the entry</param>
/// <param name="Nullable">Defines whether the value of hte entry can be null</param>
public record EntryType(EntryValueType ValueType, bool Nullable);