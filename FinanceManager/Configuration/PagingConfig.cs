namespace FinanceManager.Configuration;


/// <summary>
/// Configuration for pagination component.
/// Page numbers starts from 1
/// </summary>
public class PagingConfig
{
    public const int DefaultPageSize = 8;
    
    private int _currentPage;
    private int _pageSize = DefaultPageSize;
    private int _dataLength;
    
    public PagingConfig(){}

    public PagingConfig(int dataLength)
    {
        DataLength = dataLength;
    }
    
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Page sizes
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 ? value : 1;
    }
    
    /// <summary>
    /// Total items count
    /// </summary>
    public int DataLength
    {
        get => _dataLength;
        set => _dataLength = value >= 0 ? value : 0;
    }
    
    /// <summary>
    /// Current page number, staring at page 1
    /// </summary>
    public int CurrentPage
    {
        get => _currentPage + 1;
        set 
        {
            if (value < 1)
            {
                _currentPage = 0;
            } else if (value > MaxPageNumber)
            {
                _currentPage = MaxPageNumber - 1;
            }
            else
            {
                _currentPage = value - 1;
            }
        }
    }
    
    /// <summary>
    /// Last page number
    /// </summary>
    public int MaxPageNumber
    {
        get 
        {
            if (DataLength < 1)
            {
                return 1;
            }
            return (int) Math.Ceiling((double)DataLength / PageSize);
        } 
    }

    
    /// <summary>
    /// </summary>
    /// <returns>Number of entries before the current page.</returns>
    public int NumEntriesToSkip()
    {
        if (!Enabled)
        {
            return 0;
        }
        return _currentPage  * PageSize; 
    }
    
    /// <summary>
    /// </summary>
    /// <returns>Number of entries on the current page.</returns>
    public int NumEntriesToTake()
    {
        return !Enabled ? DataLength : Math.Min(PageSize, Math.Max(DataLength - PageSize * _currentPage, 0));
    }
    
}