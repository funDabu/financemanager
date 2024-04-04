namespace FinanceManager.Data;

/// <summary>
/// Interface for transaction entries.
/// Implements <c>IComparable&lt;ITransactionEntry&gt;</c>:
/// only transaction entries representing the same value type can be compare,
/// in other cases the behavior is not defined.
/// </summary>
public interface ITransactionEntry : IComparable<ITransactionEntry>, ICloneable
{
    /// <summary>
    /// Type of give transaction entry
    /// </summary>
    public EntryType EntryType { get; }
    
    /// <summary>
    /// Value of given entry as a string
    /// </summary>
    public string ValueString { get; set; }
    
    /// <summary>
    /// Validates <c>value</c> whether can be storied in given entry
    /// </summary>
    /// <param name="value">Value to be validated</param>
    /// <returns><c>true</c> if <c>value</c> can be stored in given entry,
    /// <c>false</c> otherwise</returns>
    public bool ValidateValue(string value);

    /// <summary>
    /// Validates <c>value</c> whether can be storied in given entry
    /// and sets the value of the entry accordingly if the <c>value</c> is valid.
    /// </summary>
    /// <param name="value">Value to be validated</param>
    /// <param name="validationErrorMessage">Will be set to non-null string if validation failed,
    /// briefly defining the valid values.</param>
    /// <returns><c>true</c> if <c>value</c> is valid and has been saved into given entry,
    /// <c>false</c> otherwise</returns>
    public bool TrySetValue(string value, out string? validationErrorMessage);
    
}