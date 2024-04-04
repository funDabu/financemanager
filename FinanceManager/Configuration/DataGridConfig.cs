using FinanceManager.Data;

namespace FinanceManager.Configuration;

/// <summary>
/// Configuration for the data grid component
/// </summary>
public class DataGridConfig()
{
    private readonly Dictionary<string, ColumnDefinition> _columns = new();
    private readonly TransactionComparator _transactionComparator = new();
    private readonly Dictionary<string, List<TransactionEntryFilter>> _filters = new ();
    
    public ColumnDefinition GetColumn(string columnName)
    {
        _columns.TryGetValue(columnName, out var column);
        if (column == null)
        {
            throw new ArgumentException($"invalid {nameof(columnName)} value: {columnName}");
        }
        return column;
    }

    public bool GetColumnDisplayed(string columnName)
    {
        var column = GetColumn(columnName);
        return column.Displayed;
    }
    
    public void ToggleColumnDisplayed(string columnName)
    {
        var column = GetColumn(columnName);
        column.Displayed = !column.Displayed;
    }

    public string GetAlignmentClass(string columnName)
    {
        var column = GetColumn(columnName);

        return column.Alignment switch
        {
            Alignment.NotSet => "",
            Alignment.Left => "text-start",
            Alignment.Right => "text-end",
            Alignment.Center => "text-center",
            _ => throw new ArgumentOutOfRangeException($"invalid value of {nameof(column.Alignment)}: {column.Alignment}")
        };
    }
    public TransactionComparator Comparator => _transactionComparator;

    public SortDirection GetColumnDirection(string columnName)
    {
        return _transactionComparator.GetSortDirection(columnName);
    }

    public void ToggleSort(string columnName)
    {
        var column = GetColumn(columnName);

        var direction = _transactionComparator.GetSortDirection(column.ColumnName) switch
        {
            SortDirection.Ascending => SortDirection.NoSort,
            SortDirection.Descending => SortDirection.Ascending,
            SortDirection.NoSort => SortDirection.Descending,
            _ => throw new ArgumentOutOfRangeException()
        };
        _transactionComparator.AddSort(column.ColumnName, direction);
    }
    
    public PagingConfig Paging { get; } = new();


    public Predicate<ITransactionEntry> GetFilter(string column)
    {
        return _filters.TryGetValue(column, out var filters) ? 
            x => filters.All(f => f.MatchFunc(x)) :
            _ => true;
    }

    public bool AddFilter(string column, TransactionEntryFilter filter)
    {
        if (!_filters.TryGetValue(column, out List<TransactionEntryFilter>? value))
        {
            _filters[column] = new List<TransactionEntryFilter>();
        }
        else if (value.Select(f => f.Id).Contains(filter.Id))
        {
            return false;
        }
        
        _filters[column].Add(filter);
        return true;
    }
    
    public void RemoveFilter(string column, long id)
    {
        if (!_filters.TryGetValue(column, out var filters))
        {
            return;
        }

        var filter = filters.Find(f => f.Id == id);
        if (filter != null)
        {
            filters.Remove(filter);
        }
    }
    
    public void RemoveFilter(long id)
    {
        foreach (var filters in _filters.Values)
        {
            var filter = filters.Find(f => f.Id == id);
            if (filter != null)
            {
                filters.Remove(filter);
            }
        }
        
    }

    public bool TransactionFilter(ITransaction transaction)
    {
        return transaction.EntryNames.All(name => GetFilter(name)(transaction.GetEntry(name)));
    }

    /// <summary>
    /// Assumes column names of columns in "columns" are distinct
    /// </summary>
    /// <param name="transactionType"></param>
    public DataGridConfig(TransactionType transactionType) : this()
    {
        foreach(var column in ColumnDefinition.FromTransactionType(transactionType))
        {
            _columns.Add(column.ColumnName, column);
        }
    }

    
    

}