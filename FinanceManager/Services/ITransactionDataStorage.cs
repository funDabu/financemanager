using FinanceManager.Data;

namespace FinanceManager.Services;

public interface ITransactionDataStorage : IEnumerable<KeyValuePair<string, ITransactionData>>
{
    /// <summary>
    /// Adds transaction data to the storage if no data with same id as "id" is already stored.
    /// </summary>
    /// <param name="id">id string of the new transaction data</param>
    /// <param name="data">the new transaction data</param>
    /// <returns>true if data were added, false other wise</returns>
    public bool TryAdd(string id, ITransactionData data);
    
    /// <summary>
    /// Removes transaction data with given id from the storage
    /// </summary>
    /// <param name="id">id string of transaction data to be removed</param>
    public void Remove(string id);
    
    /// <summary>
    /// Gets transactions data specified by id from teh storage
    /// </summary>
    /// <param name="id">id string of the desired transaction data</param>
    /// <returns>transaction data with given id if stored in service, null otherwise</returns>
    public ITransactionData? Get(string id);

    /// <summary>
    /// Informs whether transaction data with given id is stored.
    /// </summary>
    /// <param name="id">id string of the questioned data</param>
    /// <returns>true if data with given id is stored, false otherwise</returns>
    public bool Contains(string id);
    
    /// <summary>
    /// Number of transaction data in the storage
    /// </summary>
    public int Count { get; }

}