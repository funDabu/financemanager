namespace FinanceManager.Data;

/// <summary>
/// Interface for transaction parser.
/// </summary>
public interface ITransactionDataParser
{
    /// <summary>
    /// Parses transaction from SCV.
    /// The exact format of the CSV file is not defined by the interface
    /// so it depends solely on the interface implementation.
    /// </summary>
    /// <param name="stream">Stream of the CSV file.</param>
    /// <param name="validationErrorMessage"><c>null</c> if no error during parsing occured,
    /// otherwise string with information about parse error</param>
    /// <returns><c>ITransactionData</c> table containing parsed transaction from given csv stream.</returns>
    public ITransactionData ParseCsv(Stream stream, out string? validationErrorMessage);
    
    /// <summary>
    /// Converts <c>ITransactionData</c> table into CSV stream.
    /// The exact format of the CSV file is not defined by the interface
    /// so it depends solely on the interface implementation.
    /// </summary>
    /// <param name="transactionData">Contains transactions to be converted.</param>
    /// <returns>CSV file as a stream.</returns>
    public Stream ToCsvStream(ITransactionData transactionData);

    /// <summary>
    /// Tries parse single transaction entry into <c>ITransactionEntry</c>.
    /// Basically provides a factory method for transaction entries.
    /// </summary>
    /// <param name="entry">String value of transaction entry.</param>
    /// <param name="entryType">Type of transaction entry.</param>
    /// <param name="validationErrorMessage"><c>null</c> if <c>entry</c> is a valid string value for given <c>entryType</c>,
    /// string describing the validation error otherwise.</param>
    /// <returns><c>ITransactionEntry</c> of type <c>entryType</c> with value determined from <c>entry</c>
    /// if the <c>entry</c> is a valid, <c>null</c> otherwise</returns>
    public ITransactionEntry? TryParseEntry(string entry, EntryType entryType, out string? validationErrorMessage);
    
    /// <summary>
    /// Format of date entries in the CSV file.
    /// Use the standard .NET date format <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings">(link)</see>
    /// </summary>
    public string DateFormat { get; set; }
    
}