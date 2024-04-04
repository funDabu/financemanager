using System.Text.RegularExpressions;
using FinanceManager.Data;
using FinanceManager.Models;

namespace FinanceManager.Configuration;

/// <summary>
/// Class for filtering transaction entries.
/// Transaction entry satisfies the filter if it is in
/// specified relation with the reference entry.
/// </summary>
public class TransactionEntryFilter
{
    private static TransactionEntryFactory _entryFactory= new("yyyy-MM-dd");

    public const string DateFormat = "yyyy-MM-dd";

    /// <summary>
    /// Filter identifier
    /// </summary>
    public long Id { get; }
    
    /// <summary>
    /// <c>ITransactionEntry</c> object serving as a reference value. 
    /// </summary>
    public ITransactionEntry Reference { get; set; }
    
    /// <summary>
    /// Defines the type of relation which the compared object must have to the reference value.
    /// </summary>
    public FilterComparison Comparison { get; set; }
    
    /// <summary>
    /// To turn the filter on/off
    /// </summary>
    public bool Applied { get; set; }
    

    public TransactionEntryFilter(long id,
        ITransactionEntry reference,
        FilterComparison comparison,
        bool applied = false)
    {
        Id = id;
        Reference = reference;
        Comparison = comparison;
        Applied = applied;
    }
    

    /// <summary>
    /// Defines transaction entry value types, which can be filtered using this filter.
    /// Is equal to the transaction entry value types of the reference object.
    /// </summary>
    public EntryValueType ValueType => Reference.EntryType.ValueType;


    /// <summary>
    /// Decides whether the given transaction entry satisfies the defined filter.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns><c>true</c> if the given entry satisfies the filter, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public bool MatchFunc(ITransactionEntry entry)
    {
        if (!Applied)
        {
            return true;
        }
        switch (Comparison)
        {
            case FilterComparison.Greater:
                return entry.CompareTo(Reference) > 0;
            case FilterComparison.GreaterOrEqual:
                return entry.CompareTo(Reference) >= 0;
            case FilterComparison.Equal:
                return entry.CompareTo(Reference) >= 0;
            case FilterComparison.NotEqual:
                return entry.CompareTo(Reference) != 0;
            case FilterComparison.LessOrEqual:
                return entry.CompareTo(Reference) <= 0;
            case FilterComparison.Less:
                return entry.CompareTo(Reference) < 0;
            case FilterComparison.Regex:
                var regex = new Regex(Reference.ValueString);
                return regex.IsMatch(entry.ValueString);
            default:
                throw new ArgumentOutOfRangeException(nameof(Comparison), Comparison, null);
        }
    }

    public TransactionEntryFilter(long id, EntryValueType type)
    {
        FilterComparison comparison;
        string initVal;
        
        switch (type)
        {
            case EntryValueType.Number:
                comparison = FilterComparison.Equal;
                initVal = "0";
                break;
            case EntryValueType.Date:
                comparison = FilterComparison.Equal;
                initVal = DateTime.Now.ToString(DateFormat);
                break;
            case EntryValueType.Text:
            case EntryValueType.Currency:
                initVal = ".*";
                comparison = FilterComparison.Regex;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        Reference = _entryFactory.Create(new EntryType(type, true), initVal, out _)!;
        Comparison = comparison;
        Id = id;
        Applied = false;

    }
    
}