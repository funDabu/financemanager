namespace FinanceManager.Data;

/// <summary>
/// Interface for single bank transaction.
/// Stores transaction entries, giving each of them a name. 
/// The oder in which the entries are stored in transaction is not defined.
/// </summary>
public interface ITransaction : ICloneable
{
    /// <summary>
    /// Unique transaction identifier.
    /// </summary>
    public long Id { get; }
    
    /// <summary>
    /// Returns transaction entry specified by its name.
    /// </summary>
    /// <param name="entryName">Name of the desired transaction entry</param>
    /// <returns><c>ITransactionEntry</c> if the transaction stores entry of given <c>entryName</c>,
    /// <c>null</c> otherwise.</returns>
    public ITransactionEntry? GetEntry(string entryName);
    
    /// <summary>
    /// Saves transaction entry specified by its name into <c>entry</c>.
    /// </summary>
    /// <param name="entryName">Name of the desired transaction entry</param>
    /// <param name="entry">Output variable for the desired transaction entry,
    /// if given transaction doesn't contain entry of given name,
    /// the value if <c>entry</c> is set to <c>null</c>.</param>
    /// <returns><c>true</c> if desired entry exists in given transaction,
    /// <c>false</c> otherwise.</returns>
    public bool TryGetEntry(string entryName, out ITransactionEntry? entry);
    
    /// <summary>
    /// Returns <c>IEnumerable</c> of entries specified by their names.
    /// </summary>
    /// <param name="entryNames">Specifies names of the desired transaction entries.
    /// Names which don't correspond to any transaction entry are ignored. </param>
    /// <returns><c>IEnumerable&lt;ITransactionEntry&gt;</c> of entries specified
    /// by <c>entryNames</c> parameter and in the same order.</returns>
    public IEnumerable<ITransactionEntry> GetEntries(IEnumerable<string> entryNames);
    
    /// <summary>
    /// Tries to add given transaction entry into the transaction.
    /// </summary>
    /// <param name="entryName">Name of the transaction entry.</param>
    /// <param name="entry">Transaction entry entry to be added.</param>
    /// <returns><c>true</c> if entry successfully added, <c>false</c> otherwise</returns>
    public bool TryAddEntry(string entryName, ITransactionEntry entry);
    
    /// <summary>
    /// Tries to create transaction entry from parameters and add it into the transaction.
    /// </summary>
    /// <param name="entryName">Name of the transaction entry.</param>
    /// <param name="valueString">Value of the transaction entry.</param>
    /// <param name="entryType">Type of the transaction entry.</param>
    /// <param name="validationErrorMessage">Will be set to non-null string if
    /// the validation of <c>valueString</c> fails, briefly defining the valid values.</param>
    /// <returns><c>true</c> if entry successfully added, <c>false</c> otherwise</returns>
    public bool TryAddEntry(string entryName, string valueString, EntryType entryType, out string? validationErrorMessage);
    
    /// <summary>
    /// Returns true if entry of given name is stored in the transaction.
    /// </summary>
    /// <param name="entryName">Name of the transaction entry.</param>
    /// <returns><c>true</c> if entry of given name is stored in the transaction,
    /// <c>false</c> otherwise.</returns>
    public bool ContainsEntry(string entryName);
    
    
    /// <summary>
    /// Returns the names of all stored transaction entries.
    /// </summary>
    public IEnumerable<string> EntryNames { get; }

    
}