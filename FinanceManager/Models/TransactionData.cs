using System.Collections;
using FinanceManager.Data;

namespace FinanceManager.Models;

/// <summary>
/// Implementation of <c>ITransactionData</c>
/// </summary>
public class TransactionData : ITransactionData
{
    private List<ITransaction> _data = new();
    public TransactionType TransactionType { get; }
    public int Length => _data.Count;

    
    /// <summary>
    /// Creates an empty transaction data table for transaction of given type.
    /// </summary>
    /// <param name="transactionType">Type of transaction which will be stored in the created object.</param>
    public TransactionData(TransactionType transactionType)
    {
        TransactionType = transactionType;
    }

    /// <summary>
    /// Creates a transaction data table for transaction of given type and adds given transactions.
    /// </summary>
    /// <param name="transactionType">Type of transaction which will be stored in the created object.</param>
    /// <param name="transactions">Transaction to be added to the table.</param>
    /// <exception cref="InvalidDataException">Thrown when a transaction of different type is given.</exception>
    public TransactionData(TransactionType transactionType, IEnumerable<ITransaction> transactions) : this(transactionType)
    {
        if (transactions.Any(transaction => !TryAddTransaction(transaction)))
        {
            throw new InvalidDataException("Transaction couldn't be added, invalid transaction type");
        }
    }
    
    public bool TryAddTransaction(ITransaction transaction)
    {
        foreach (var (name, type) in TransactionType.EntryNames.Zip(TransactionType.EntryTypes))
        {
            if (!transaction.TryGetEntry(name, out var entry))
            {
                return false;
            }
            
            if (entry!.EntryType != type)
            {
                return false;
            }
        }

        _data.Add(transaction);
        return true;
    }

    public void Filter(Predicate<ITransaction> predicate)
    {
        _data = _data.Where(t => predicate(t)).ToList();
    }

    public bool AddTransaction(out string? validationErrorMessage, params string[] entryValues)
    {
        if (entryValues.Length != TransactionType.Length)
        {
            validationErrorMessage = $"wrong number of values: expected {TransactionType.Length}, got {entryValues.Length}";
            return false;
        }
        
        var entryNames = TransactionType.EntryNames as string[] ?? TransactionType.EntryNames.ToArray();
        var entryTypes = TransactionType.EntryTypes as EntryType[] ?? TransactionType.EntryTypes.ToArray();
        var transaction = new Transaction();
        
        for (var i = 0; i < entryValues.Length; i++ )
        {
            if (!transaction.TryAddEntry(entryNames[i], entryValues[i], entryTypes[i], out var errorMessage))
            {
                validationErrorMessage = $"position {i+1}, value '{entryValues[i]}': " + errorMessage;
                return false;
            }
        }
        _data.Add(transaction);
        validationErrorMessage = null;
        return true;
    }

    public IEnumerator<ITransaction> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    public object Clone()
    {
        return new TransactionData(TransactionType, _data.Select(t => (ITransaction) t.Clone()));
    }
}