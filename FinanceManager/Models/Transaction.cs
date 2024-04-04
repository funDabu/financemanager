using System.Collections;
using FinanceManager.Data;

namespace FinanceManager.Models;


/// <summary>
/// Implementation of <c>ITransaction</c>.
/// </summary>
public class Transaction : ITransaction
{
    private static long _counter;
    public long Id { get; }
    private readonly Dictionary<string, ITransactionEntry> _transactions = new ();

    public Transaction()
    {
        Id = _counter++;
    }
    
    private Transaction(long id, Dictionary<string, ITransactionEntry> transactions)
    {
        Id = id;
        foreach (var (name, entry) in transactions)
        {
            _transactions[name] = entry;
        }
    }

    public ITransactionEntry? GetEntry(string entryName)
    {
        _transactions.TryGetValue(entryName, out var result);
        return result;
    }

    public bool TryGetEntry(string entryName, out ITransactionEntry? entry)
    {
        return _transactions.TryGetValue(entryName, out entry);
    }

    public bool TryAddEntry(string entryName, string valueString, EntryType entryType, out string? validationErrorMessage)
    {
        if (_transactions.ContainsKey(entryName))
        {
            validationErrorMessage = "entry with name'{entryName}' already exists in given transaction";
            return false;
        }
        
        var entry = new TransactionEntryFactory().Create( entryType, valueString, out validationErrorMessage);
        if (entry == null)
        {
            return false;
        }
        _transactions.Add(entryName, entry);
        return true;

    }

    public bool ContainsEntry(string entryName)
    {
        return _transactions.ContainsKey(entryName);
    }

    public bool TryAddEntry(string entryName, ITransactionEntry entry)
    {
        return _transactions.TryAdd(entryName, entry);
    }

    public IEnumerable<string> EntryNames => _transactions.Keys;

    public IEnumerable<ITransactionEntry> GetEntries(IEnumerable<string> entryNames)
    {
        var result = new List<ITransactionEntry>();
        foreach (var name in entryNames)
        {
            if (_transactions.TryGetValue(name, out var entry))
            {
                result.Add(entry);
            }
        }
        return result;
    }

    public object Clone()
    {
        return new Transaction(Id, _transactions);
    }
}