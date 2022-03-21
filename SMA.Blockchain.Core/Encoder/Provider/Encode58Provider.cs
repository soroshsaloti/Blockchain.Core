namespace SMA.Blockchain.Core.Encoder.Provider;

using SMA.Blockchain.Core.Encoder.Abstarction;
using System;

/// <summary>
/// Encode58 provider.
/// </summary>
public sealed class Encode58Provider : EncodingProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Encode58Provider"/> class
    /// using a custom provider.
    /// </summary>
    /// <param name="provider">Provider to use.</param>
    public Encode58Provider(string provider)
        : base(58, provider)
    {
    }
}