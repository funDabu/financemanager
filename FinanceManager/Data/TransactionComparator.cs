namespace FinanceManager.Data;

/// <summary>
/// Class for defining sorting priorities to sort transactions.
/// Implements <c>IComparer</c> for <c>ITransaction</c>,
/// the comparison is based on the sort priorities.
/// </summary>
public class TransactionComparator : IComparer<ITransaction>
{
    private readonly Dictionary<string, SortDirection> _sortDirections = new();
    private readonly List<string> _sortedOn = new();

    /// <summary>
    /// Names of transaction entries which are considered when two transactions are compared.
    /// The order of the names reflects the priority of those entry names during comparison if transactions,
    /// each entry names has a higher priority than all the consequent entry names.
    /// </summary>
    public IEnumerable<string> SortedOn => _sortedOn.Where(col => GetSortDirection(col) != SortDirection.NoSort);
    
    /// <summary>
    /// Returns the <c>SortDirection</c> given to the entry.
    /// </summary>
    /// <param name="entryName">Name of the entry.</param>
    /// <returns><c>SortDirection</c> of given entry;
    /// if no direction is defined for given entry name, returns <c>SortDirection.NoSort</c>.</returns>
    public SortDirection GetSortDirection(string entryName) =>
        _sortDirections.GetValueOrDefault(entryName, SortDirection.NoSort);

    /// <summary>
    /// Adds sorting direction for given entry name.
    /// Replaces the old soring direction value,
    /// if one is already defined for given entry name.
    /// Sorting direction added earlier have higher priority during comparison
    /// then direction on entry names which were added later.
    /// Note that adding direction for entry name with already assigned direction
    /// will remove the old sorting direction and adds new one,
    /// and thus changing (lowering) the priority for this entry name during comparison.
    /// </summary>
    /// <param name="entryName">Entry name</param>
    /// <param name="direction">Sorting direction</param>
    public void AddSort(string entryName, SortDirection direction)
    {
        _sortedOn.Remove(entryName);
        _sortedOn.Add(entryName);
        _sortDirections[entryName] = direction;
    }

    
    /// <summary>
    /// Delete all sorting priorities (directions) for all entry names.
    /// </summary>
    public void Clear()
    {
        _sortedOn.Clear();
        _sortDirections.Clear();
    }

    private int Comparator(ITransaction t1, ITransaction t2)
    {
        foreach (var entryName in _sortedOn)
        {
            int direction;
            switch (_sortDirections.GetValueOrDefault(entryName, SortDirection.NoSort))
            {
                case SortDirection.Ascending:
                    direction = 1;
                    break;
                case SortDirection.Descending:
                    direction = -1;
                    break;
                case SortDirection.NoSort:
                    direction = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var entry1 = t1.GetEntry(entryName);
            var entry2 = t2.GetEntry(entryName);
            
            int compare = (entry1, entry2) switch
            {
                (null, null) => 0,
                (null, _) => -1,
                _ => entry1.CompareTo(entry2)
            };
            compare = direction * compare;
            
            if (compare != 0)
            { 
                return compare;
            }
        }

        return 0;
    }

    /// <summary>
    /// Compares two transaction based on the sorting priorities,
    /// i.g. first compares them on the entry for which the first sorting direction was added,
    /// when the entries are same it compares them on the entry for which the second sorting direction was added etc.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int Compare(ITransaction? x, ITransaction? y)
    {
        return (x, y) switch
        {
            (null, null) => 0,
            (null, _) => -1,
            (_, null) => 1,
            _ => Comparator(x, y)
        };
    }
}