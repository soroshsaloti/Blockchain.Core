namespace SMA.Blockchain.Core.Encoder.Provider;

using SMA.Blockchain.Core.Encoder.Abstarction;

/// <summary>
/// Encode85 Provider.
/// </summary>
public sealed class Encode85Provider : EncodingProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Encode85Provider"/> class
    /// using custom settings.
    /// </summary>
    /// <param name="provider">Provider to use.</param>
    /// <param name="allZeroShortcut">Character to substitute for all zero.</param>
    /// <param name="allSpaceShortcut">Character to substitute for all space.</param>
    public Encode85Provider(
        string provider,
        char? allZeroShortcut = null,
        char? allSpaceShortcut = null)
        : base(85, provider)
    {
        this.AllZeroShortcut = allZeroShortcut;
        this.AllSpaceShortcut = allSpaceShortcut;
    }


    /// <summary>
    /// Gets the character to be used for "all zeros".
    /// </summary>
    public char? AllZeroShortcut { get; }

    /// <summary>
    /// Gets the character to be used for "all spaces".
    /// </summary>
    public char? AllSpaceShortcut { get; }

    /// <summary>
    /// Gets a value indicating whether the provider uses one of shortcut characters for all spaces
    /// or all zeros.
    /// </summary>
    public bool HasShortcut => AllSpaceShortcut.HasValue || AllZeroShortcut.HasValue;
}