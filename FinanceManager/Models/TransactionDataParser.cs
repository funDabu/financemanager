using FinanceManager.Data;
using FinanceManager.Defaults;
using Microsoft.VisualBasic.FileIO;

namespace FinanceManager.Models;

/// <summary>
/// Implementation of <c>ITransactionDataParser</c>.
/// The expected format of the CSV is:
///     - first line is expected to be a header and is skipped
///     - each other line contains a single transaction
///     - transaction is given as transaction entries enclosed in '"' and separated by ','
///     - the order, types, and formats of entries is defined by default transaction type <see cref="DefaultTypes.TransactionType"/>
/// </summary>
public class TransactionDataParser : ITransactionDataParser
{
    private TransactionEntryFactory _transactionEntryFactory = new ();
    private string _dateFormat;

    public ITransactionData ParseCsv(Stream stream, out string? validationErrorMessage)
    {
        using var parser = new TextFieldParser(stream);
        
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        parser.ReadLine(); // Skip the header row

        var result = new TransactionData(DefaultTypes.TransactionType);
        
        int line = 1;
        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (fields != null && !result.AddTransaction(out var errorMessage, fields))
            {
                validationErrorMessage = $"line {line}, " + errorMessage;
                return result;
            }
            line++;
        }

        validationErrorMessage = null;
        return result;
    }

    public Stream ToCsvStream(ITransactionData transactionData)
    {
        var header = String.Join(",", DefaultTypes.EntryNames.Select(n => "\"" + n + "\""));
        var stringRepresentation = header + Environment.NewLine + String.Join(
            Environment.NewLine,
            transactionData.Select(
                t => String.Join(
                    ",", 
                    t.GetEntries(transactionData.TransactionType.EntryNames)
                        .Select(e => "\"" + e.ValueString + "\"")
                    )
                )
            );
        
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(stringRepresentation);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public ITransactionEntry? TryParseEntry(string entry, EntryType entryType, out string? validationErrorMessage)
    {
        return _transactionEntryFactory.Create(entryType, entry, out validationErrorMessage);
    }

    public string DateFormat
    {
        get => _dateFormat;
        set
        {
            _dateFormat = value;
            _transactionEntryFactory.DateFormat = value;
        }
    }
}