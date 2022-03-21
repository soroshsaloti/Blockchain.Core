namespace SMA.Blockchain.Core.Encoder.Abstarction;

/// <summary>
/// Defines basic encoding provider.
/// </summary>
public interface IEncodingProvider
{
    /// <summary>
    /// Gets the characters in the provider.
    /// </summary>
    string Value { get; }

    /// <summary>
    /// Gets the length of the provider.
    /// </summary>
    int Length { get; }
}
