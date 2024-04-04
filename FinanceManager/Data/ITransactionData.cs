namespace FinanceManager.Data;

/// <summary>
/// Interface for a "table" of transactions
/// All stored transactions have a single type
/// that defines names and types of entries which each transaction contains.
/// </summary>
public interface ITransactionData: IEnumerable<ITransaction>, ICloneable
{
    /// <summary>
    /// Type of stored transactions.
    /// </summary>
    public TransactionType TransactionType { get; }
    
    /// <summary>
    /// The count of stored transactions.
    /// </summary>
    public int Length { get; }
    
    /// <summary>
    /// Tries to add given transaction into the table.
    /// </summary>
    /// <param name="transaction">The new transaction.
    /// Entries stored in the transaction and their names has to comply to
    /// the <c>TransactionType</c> of the table.</param>
    /// <returns><c>true</c> if the transaction is compatible with the <c>TransactionType</c> of the table
    /// and was successfully added, <c>false</c> otherwise.</returns>
    public bool TryAddTransaction(ITransaction transaction);
    
    /// <summary>
    /// Filter out transactions which are false to given predicate.
    /// </summary>
    /// <param name="predicate">Predicate, those transactions on which the predicates is false
    /// will be removed from the table.</param>
    public void Filter(Predicate<ITransaction> predicate);
}