namespace SMA.Blockchain.Core.Encoder.Abstarction;
using System.Diagnostics;

/// <summary>
/// A single encoding algorithm can support many different providers.
/// EncodingProvider consists of a basis for implementing different
/// providers for different encodings. It's suitable if you want to
/// implement your own encoding based on the existing base classes.
/// </summary>
public abstract class EncodingProvider : IEncodingProvider
{
    /// <summary>
    /// Specifies the highest possible char value in an encoding provider
    /// Any char above with would raise an exception.
    /// </summary>
    private const int maxLength = 127;

    /// <summary>
    /// Holds a mapping from character to an actual byte value
    /// The values are held as "value + 1" so a zero would denote "not set"
    /// and would cause an exception.
    /// </summary>
    /// <remarks>byte[] has no discernible perf impact and saves memory.</remarks>
    private readonly byte[] reverseLookupTable = new byte[maxLength];

    /// <summary>
    /// Initializes a new instance of the <see cref="EncodingProvider"/> class.
    /// </summary>
    /// <param name="length">Length of the alphabe.</param>
    /// <param name="provider">Provider character.</param>
    public EncodingProvider(int length, string provider)
    {
        if (provider is null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        if (provider.Length != length)
        {
            throw new ArgumentException($"Required provider length is {length} but provided provider is "
                + $"{provider.Length} characters long");
        }

        this.Length = length;
        this.Value = provider;

        for (short i = 0; i < length; i++)
        {
            this.Map(provider[i], i);
        }
    }

    /// <summary>
    /// Gets the length of the provider.
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Gets the characters of the provider.
    /// </summary>
    public string Value { get; private set; }

    internal ReadOnlySpan<byte> ReverseLookupTable => reverseLookupTable;

    /// <summary>
    /// Generates a standard invalid character exception for providers.
    /// </summary>
    /// <remarks>
    /// The reason this is not a throwing method itself is
    /// that the compiler has no way of knowing whether the execution
    /// will end after the method call and can incorrectly assume
    /// reachable code.
    /// </remarks>
    /// <param name="c">Characters.</param>
    /// <returns>Exception to be thrown.</returns>
    public static Exception InvalidCharacter(char c)
    {
        return new ArgumentException($"Invalid character: {c}");
    }

    /// <summary>
    /// Get the string representation of the provider.
    /// </summary>
    /// <returns>The characters of the encoding provider.</returns>
    public override string ToString()
    {
        return this.Value;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.Ordinal);
    }

    /// <summary>
    /// Map a character to a value.
    /// </summary>
    /// <param name="c">Characters.</param>
    /// <param name="value">Corresponding value.</param>
    protected void Map(char c, int value)
    {
        //if(c<maxLength)
        //    throw new ArgumentException($"Provider contains character above {maxLength}");
        Debug.Assert(c < maxLength, $"Provider contains character above {maxLength}");
        this.reverseLookupTable[c] = (byte)(value + 1);
    }
}
