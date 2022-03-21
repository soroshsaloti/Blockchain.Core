namespace SMA.Blockchain.Core.Encoder.Abstarction;

/// <summary>
/// Basic encoding functionality.
/// </summary>
public interface IBaseEncoder
{
    /// <summary>
    /// Encode a buffer to base-encoded representation.
    /// </summary>
    /// <param name="bytes">Bytes to encode.</param>
    /// <returns>Encode16 string.</returns>
    string Encode(ReadOnlySpan<byte> bytes);

    /// <summary>
    /// Decode base-encoded text into bytes.
    /// </summary>
    /// <param name="text">Encode16 text.</param>
    /// <returns>Decoded bytes.</returns>
    Span<byte> Decode(ReadOnlySpan<char> text);
}
