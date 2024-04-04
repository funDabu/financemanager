using System.Collections;

namespace FinanceManager.Data;

/// <summary>
/// Class for objects representing transaction types.
/// Transaction type is defined by transaction entry names, types and their order.
/// </summary>
public sealed class TransactionType : IEquatable<TransactionType>, IEnumerable<Tuple<string, EntryType>>, ICloneable
{
    private readonly string[] _entryNames;
    private readonly EntryType[] _entryTypes;

    /// <summary>
    /// Transaction entry names.
    /// </summary>
    public IEnumerable<string> EntryNames => _entryNames.AsReadOnly();
    
    /// <summary>
    /// Transaction entry types.
    /// </summary>
    public IEnumerable<EntryType> EntryTypes => _entryTypes.AsReadOnly();
    
    /// <summary>
    /// Transaction entries count.
    /// </summary>
    public int Length => _entryNames.Length;
    
    
    /// <summary>
    /// Creates new transaction type from given names and types.
    /// Given entry types have to be given in the same order as entry names,
    /// in other words i-th type has to correspond to the i-th name.
    /// </summary>
    /// <param name="entryNames">Entry names, must be mutually distinct and non-empty.</param>
    /// <param name="entryTypes">Entry types corresponding to given names</param>
    /// <exception cref="ArgumentException">Thrown when entry names are not unique,
    /// or the count of entry names is different from the count of entry types,
    /// or the count of any if the method's parameter is zero.</exception>
    public TransactionType(IEnumerable<string> entryNames, IEnumerable<EntryType> entryTypes)
    {
        var names = entryNames as string[] ?? entryNames.ToArray();
        if (names.Length == 0)
        {
            throw new ArgumentException("Parameter has to be IEnumerable with Count greater then zero", nameof(entryNames));
        }
        if (names.Length != names.Distinct().Count())
        {
            throw new ArgumentException("Parameter values has to be distinct", nameof(entryNames));
        }

        var types = entryTypes as EntryType[] ?? entryTypes.ToArray();
        if (types.Length == 0)
        {
            throw new ArgumentException("Parameter has to be IEnumerable with Count greater then zero", nameof(entryNames));
        }

        if (types.Length != names.Length)
        {
            throw new ArgumentException("Length of both parameters, i.g. entry names and entry types, has to be equal");
        }
        _entryTypes = types;
        _entryNames = names;
    }


    /// <summary>
    /// Two transaction types are equal when both have the same transaction entry names, types in the same order.
    /// </summary>
    /// <param name="other">The other transaction type.</param>
    /// <returns><c>true</c> if types are equal, <c>false</c> otherwise.</returns>
    public bool Equals(TransactionType? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _entryNames.Equals(other._entryNames) && _entryTypes.Equals(other._entryTypes);
    }

    public IEnumerator<Tuple<string, EntryType>> GetEnumerator()
    {
        return _entryNames.Zip(_entryTypes)
            .Select(x => new Tuple<string,EntryType>(x.First, x.Second))
            .GetEnumerator();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TransactionType)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_entryNames.GetHashCode(), _entryTypes.GetHashCode());
    }

    public object Clone()
    {
        return new TransactionType((string[])_entryNames.Clone(),
            _entryTypes.Select(t => new EntryType(t.ValueType, t.Nullable)));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}