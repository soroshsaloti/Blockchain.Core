namespace SMA.Blockchain.Core.Encoder.Provider;

using SMA.Blockchain.Core.Encoder.Abstarction;
using System;

/// <summary>
/// Provider representation for Encode16 encodings.
/// </summary>
public class Encode16Provider : EncodingProvider
{ 

    /// <summary>
    /// Initializes a new instance of the <see cref="Encode16Provider"/> class with
    /// case insensitive semantics.
    /// </summary>
    /// <param name="provider">Encoding provider.</param>
    public Encode16Provider(string provider)
        : this(provider, caseSensitive: false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Encode16Provider"/> class.
    /// </summary>
    /// <param name="provider">Encoding provider.</param>
    /// <param name="caseSensitive">If the decoding should be performed case sensitive.</param>
    public Encode16Provider(string provider, bool caseSensitive)
        : base(16, provider)
    {
        if (!caseSensitive)
        {
            mapCounterparts();
        }
    } 

    /// <summary>
    /// Gets a value indicating whether the decoding should be performed in a case sensitive fashion.
    /// The default is false.
    /// </summary>
    public bool CaseSensitive { get; }

    private void mapCounterparts()
    {
        int alphaLen = Value.Length;
        for (int i = 0; i < alphaLen; i++)
        {
            char c = Value[i];
            if (char.IsLetter(c))
            {
                if (char.IsUpper(c))
                {
                    Map(char.ToLowerInvariant(c), i);
                }

                if (char.IsLower(c))
                {
                    Map(char.ToUpperInvariant(c), i);
                }
            }
        }
    }
}