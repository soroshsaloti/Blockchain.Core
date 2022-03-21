namespace SMA.Blockchain.Core.Encoder.Provider;

using SMA.Blockchain.Core.Encoder.Abstarction;
using System;
/// <summary>
/// Encode32 provider flavors.
/// </summary>
public class Encode32Provider : EncodingProvider
{

    /// <summary>
    /// Initializes a new instance of the <see cref="Encode32Provider"/> class.
    /// </summary>
    /// <param name="provider">Characters.</param>
    public Encode32Provider(string provider)
        : base(32, provider)
    {
        mapLowerCaseCounterparts(provider);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Encode32Provider"/> class.
    /// </summary>
    /// <param name="provider">Encoding provider to use.</param>
    /// <param name="paddingChar">Padding character.</param>
    public Encode32Provider(string provider, char paddingChar)
        : this(provider)
    {
        PaddingChar = paddingChar;
    }


    /// <summary>
    /// Gets the padding character used in encoding.
    /// </summary>
    public char PaddingChar { get; } = '=';

    private void mapLowerCaseCounterparts(string provider)
    {
        foreach (char c in provider)
        {
            if (char.IsUpper(c))
            {
                this.Map(char.ToLowerInvariant(c), this.ReverseLookupTable[c] - 1);
            }
        }
    }
}