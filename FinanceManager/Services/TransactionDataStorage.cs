using System.Collections;
using FinanceManager.Data;

namespace FinanceManager.Services;

public class TransactionDataStorage : ITransactionDataStorage
{
    private static int _counter;
    private readonly Dictionary<string, ITransactionData> _storage = new ();

    public int Counter => _counter;
    public int Id { get; }

    public TransactionDataStorage()
    {
        _counter++;
        Id = new Random(45).Next();
    }
    
    public TransactionDataStorage(string id, ITransactionData data) : this()
    {
        _storage.Add(id, data);
    }

    public TransactionDataStorage(IEnumerable<Tuple<string, ITransactionData>> dataEnumerable)
    {
        foreach (var (id, data) in dataEnumerable)  
        {
            if (!_storage.TryAdd(id, data))
            {
                _storage.Clear();
                throw new InvalidDataException("Same id found for the second time. Each id has to be unique.");
            }
        }
    }
    
    
    public bool TryAdd(string id, ITransactionData data)
    {
        return _storage.TryAdd(id, data);
    }

    public void Remove(string id)
    {
        _storage.Remove(id);
    }

    public ITransactionData? Get(string id)
    {
        _storage.TryGetValue(id, out var result);
        return result;
    }

    public bool Contains(string id)
    {
        return _storage.ContainsKey(id);
    }

    public int Count => _storage.Count;

    public IEnumerator<KeyValuePair<string, ITransactionData>> GetEnumerator()
    {
        return _storage.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}