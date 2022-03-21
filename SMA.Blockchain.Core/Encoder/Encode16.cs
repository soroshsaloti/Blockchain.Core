namespace SMA.Blockchain.Core.Encoder;

using SMA.Blockchain.Core.Encoder.Abstarction;
using SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Encoder.Provider;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Encode16 encoding/decoding.
/// </summary>
public sealed class Encode16 : IBaseEncoder, IBaseStreamEncoder, INonAllocatingBaseEncoder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Encode16"/> class.
    /// </summary>
    /// <param name="provider">Provider to use.</param>
    public Encode16(Encode16Provider provider)
    {
        Provider = provider;
    }

    /// <summary>
    /// Gets the provider used by the encoder.
    /// </summary>
    public Encode16Provider Provider { get; }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Provider.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(Encode16)}_{Provider}";
    }

    /// <inheritdoc/>
    public int GetSafeByteCountForDecoding(ReadOnlySpan<char> text)
    {
        int textLen = text.Length;

        if ((textLen & 1) != 0)
        {
            return 0;
        }

        return textLen / 2;
    }

    /// <inheritdoc/>
    public int GetSafeCharCountForEncoding(ReadOnlySpan<byte> buffer)
    {
        return buffer.Length * 2;
    }

    /// <summary>
    /// Decode Upper/Lowercase Encode16 text into bytes.
    /// </summary>
    /// <param name="text">Hex string.</param>
    /// <returns>Decoded bytes.</returns>
    public static Span<byte> Decode(string text)
    {
        return Encoders.UpperCase.Decode(text.AsSpan());
    }

    /// <summary>
    /// Decode Encode16 text through streams for generic use. Stream based variant tries to consume
    /// as little memory as possible, and relies of .NET's own underlying buffering mechanisms,
    /// contrary to their buffer-based versions.
    /// </summary>
    /// <param name="input">Stream that the encoded bytes would be read from.</param>
    /// <param name="output">Stream where decoded bytes will be written to.</param>
    /// <returns>Task that represents the async operation.</returns>
    public async Task DecodeAsync(TextReader input, Stream output)
    {
        await StreamHelper.DecodeAsync(input, output, buffer => this.Decode(buffer.Span).ToArray())
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Decode Encode16 text through streams for generic use. Stream based variant tries to consume
    /// as little memory as possible, and relies of .NET's own underlying buffering mechanisms,
    /// contrary to their buffer-based versions.
    /// </summary>
    /// <param name="input">Stream that the encoded bytes would be read from.</param>
    /// <param name="output">Stream where decoded bytes will be written to.</param>
    public void Decode(TextReader input, Stream output)
    {
        StreamHelper.Decode(input, output, buffer => this.Decode(buffer.Span).ToArray());
    }

    /// <summary>
    /// Encodes stream of bytes into a Encode16 text.
    /// </summary>
    /// <param name="input">Stream that provides bytes to be encoded.</param>
    /// <param name="output">Stream that the encoded text is written to.</param>
    public void Encode(Stream input, TextWriter output)
    {
        StreamHelper.Encode(input, output, (buffer, lastBlock) => Encode(buffer.Span));
    }

    /// <summary>
    /// Encodes stream of bytes into a Encode16 text.
    /// </summary>
    /// <param name="input">Stream that provides bytes to be encoded.</param>
    /// <param name="output">Stream that the encoded text is written to.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task EncodeAsync(Stream input, TextWriter output)
    {
        await StreamHelper.EncodeAsync(input, output, (buffer, lastBlock) =>
            Encode(buffer.Span)).ConfigureAwait(false);
    }

    /// <summary>
    /// Decode Encode16 text into bytes.
    /// </summary>
    /// <param name="text">Encode16 text.</param>
    /// <returns>Decoded bytes.</returns>
    public unsafe Span<byte> Decode(ReadOnlySpan<char> text)
    {
        int textLen = text.Length;
        if (textLen == 0)
        {
            return Array.Empty<byte>();
        }

        byte[] output = new byte[GetSafeByteCountForDecoding(text)];

        var decode = TryDecode(text, output, out _);
        if (decode)
            return output;
        Debug.Write("Invalid text "+ nameof(text));
        return Array.Empty<byte>(); 
    }

    /// <inheritdoc/>
    public unsafe bool TryDecode(ReadOnlySpan<char> text, Span<byte> output, out int numBytesWritten)
    {
        int textLen = text.Length;
        if (textLen == 0)
        {
            numBytesWritten = 0;
            return true;
        }

        if ((textLen & 1) != 0)
        {
            numBytesWritten = 0;
            Debug.WriteLine("Invalid input buffer length for Encode16 decoding");
            return false;
        }

        int outputLen = textLen / 2;
        if (output.Length < outputLen)
        {
            numBytesWritten = 0;
            Debug.WriteLine("Insufficient output buffer length for Encode16 decoding");
            return false;
        }

        var table = Provider.ReverseLookupTable;
        var valid = true;
        fixed (byte* outputPtr = output)
        fixed (char* textPtr = text)
        {
            byte* pOutput = outputPtr;
            char* pInput = textPtr;
            char* pEnd = pInput + textLen;
            while (pInput != pEnd && valid)
            {
                int b1 = table[pInput[0]] - 1;
                if (b1 < 0)
                {
                    Debug.WriteLine($"Invalid hex character: {pInput[0]}");
                    valid = false;
                }

                int b2 = table[pInput[1]] - 1;
                if (b2 < 0)
                {
                    Debug.WriteLine($"Invalid hex character: {pInput[1]}");
                    valid = false;
                }

                *pOutput++ = (byte)((b1 << 4) | b2);
                pInput += 2;
            }

        }

        if (!valid)
        {
            numBytesWritten = 0;
            return false;
        }

        numBytesWritten = outputLen;
        return true;
    }

    /// <summary>
    /// Encode to Encode16 representation.
    /// </summary>
    /// <param name="bytes">Bytes to encode.</param>
    /// <returns>Encode16 string.</returns>
    public unsafe string Encode(ReadOnlySpan<byte> bytes)
    {
        int bytesLen = bytes.Length;
        if (bytesLen == 0)
        {
            return string.Empty;
        }

        var output = new string('\0', GetSafeCharCountForEncoding(bytes));
        fixed (char* outputPtr = output)
        {
            internalEncode(bytes, bytesLen, Provider.Value, outputPtr);
        }

        return output;
    }

    /// <inheritdoc/>
    public unsafe bool TryEncode(ReadOnlySpan<byte> bytes, Span<char> output, out int numCharsWritten)
    {
        int bytesLen = bytes.Length;
        string provider = Provider.Value;

        int outputLen = bytesLen * 2;
        if (output.Length < outputLen)
        {
            numCharsWritten = 0;
            return false;
        }

        if (outputLen == 0)
        {
            numCharsWritten = 0;
            return true;
        }

        fixed (char* outputPtr = output)
        {
            internalEncode(bytes, bytesLen, provider, outputPtr);
        }

        numCharsWritten = outputLen;
        return true;
    }

    private static unsafe void internalEncode(
        ReadOnlySpan<byte> bytes,
        int bytesLen,
        string provider,
        char* outputPtr)
    {
        fixed (byte* bytesPtr = bytes)
        {
            char* pOutput = outputPtr;
            byte* pInput = bytesPtr;

            int octets = bytesLen / sizeof(ulong);
            for (int i = 0; i < octets; i++, pInput += sizeof(ulong))
            {
                // read bigger chunks
                ulong input = *(ulong*)pInput;
                for (int j = 0; j < sizeof(ulong) / 2; j++, input >>= 16)
                {
                    ushort pair = (ushort)input;

                    // use cpu pipeline to parallelize writes
                    pOutput[0] = provider[(pair >> 4) & 0x0F];
                    pOutput[1] = provider[pair & 0x0F];
                    pOutput[2] = provider[pair >> 12];
                    pOutput[3] = provider[(pair >> 8) & 0x0F];
                    pOutput += 4;
                }
            }

            for (int remaining = bytesLen % sizeof(ulong); remaining > 0; remaining--)
            {
                byte b = *pInput++;
                pOutput[0] = provider[b >> 4];
                pOutput[1] = provider[b & 0x0F];
                pOutput += 2;
            }
        }
    }
}
