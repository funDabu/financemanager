using System.Runtime.InteropServices;
using FinanceManager.Data;

namespace FinanceManager.Models;

/// <summary>
/// Abstract class serving as a basis for implementation of <c>ITransactionEntry</c>
/// </summary>
public abstract class TransactionEntry : ITransactionEntry

{
    private decimal? _valueNumberOrNull = 0;
    private string _valueString = Constants.UninitializedString;
    
    public abstract EntryType EntryType { get; }
    /// <summary>
    /// Describes valid string value for the transaction entry.
    /// </summary>
    protected abstract string ValidationErrorMessage { get; }
    public abstract bool ValidateValue(string value);
    
    /// <summary>
    /// Converts string value of the entry to a decimal representation.
    /// </summary>
    /// <param name="stringValue">entry value as string</param>
    /// <returns>decimal representation of given value</returns>
    protected abstract decimal StringValueToNumber(string stringValue);
    /// <summary>
    /// Converts decimal value of the entry to a string value.
    /// </summary>
    /// <param name="numberValue">entry value as a decimal</param>
    /// <returns>string representation of given value</returns>
    protected abstract string NumberValueToString(double numberValue);

    public bool Nullable => EntryType.Nullable;

    public string ValueString
    {
        get => _valueString;
        set
        {
            if (value == Constants.NullString)
            {
                if (Nullable)
                {
                    _valueNumberOrNull = null;
                }
                else
                {
                    throw new FormatException("Null representing value provided to non-nullable type");
                }
            }
            else
            {
                _valueNumberOrNull = StringValueToNumber(value);
            }
            _valueString = value;
        }
    }

    /// <summary>
    /// Helper property, representing the entry value as numerical value.
    /// </summary>
    /// <exception cref="FormatException">Thrown when null value is set to non-nullable transaction entry.</exception>
    public decimal? ValueNumberOrNull
    {
        get => _valueNumberOrNull;
        set
        {
            if (value == null)
            {
                if (Nullable)
                {
                    _valueString = Constants.NullString;
                }
                else
                {
                    throw new FormatException("Null value provided to non-nullable type");
                }
            }
            else
            {
                _valueString = NumberValueToString((double)value);
            }
            _valueNumberOrNull = value;
        }
    }


    public bool TrySetValue(string value, out string? validationErrorMessage)
    {
        if (ValidateValue(value))
        {
            ValueString = value;
            validationErrorMessage = null;
            return true;
        }

        validationErrorMessage = ValidationErrorMessage;
        return false;
    }

    /// <summary>
    /// Classic Compare function.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="InvalidComObjectException">Thrown when entries of different value types are compared</exception>
    public int CompareTo(ITransactionEntry? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (other is not TransactionEntry entry)
        {
            throw new InvalidComObjectException(
                $"Cannot compare transaction entries of different implementations");
        }

        if (other.EntryType.ValueType != EntryType.ValueType)
        {
            throw new InvalidComObjectException(
                $"Cannot compare transaction entries with different types: {EntryType.ValueType}, {other.EntryType.ValueType}");
        }

        return CompareToSameType(entry);
    }

    protected abstract int CompareToSameType(TransactionEntry other);
    public abstract object Clone();
}