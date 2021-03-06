namespace SMA.Blockchain.Core.Encoder;

using SMA.Blockchain.Core.Encoder.Abstarction;
using SMA.Blockchain.Core.Encoder.Provider;
using System;
using System.Diagnostics;

/// <summary>
/// Encode58 Encoding/Decoding implementation.
/// </summary>
/// <remarks>
/// Encode58 doesn't implement a Stream-based interface because it's not feasible to use
/// on large buffers.
/// </remarks>
public sealed class Encode58 : IBaseEncoder, INonAllocatingBaseEncoder
{
    private const int reductionFactor = 733; 
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Encode58"/> class
    /// using a custom provider.
    /// </summary>
    /// <param name="provider">Provider to use.</param>
    public Encode58(Encode58Provider provider)
    {
        this.Provider = provider;
        this.ZeroChar = provider.Value[0];
    }

    /// <summary>
    /// Gets the encoding provider.
    /// </summary>
    public Encode58Provider Provider { get; }

    /// <summary>
    /// Gets the character for zero.
    /// </summary>
    public char ZeroChar { get; }

    /// <inheritdoc/>
    public int GetSafeByteCountForDecoding(ReadOnlySpan<char> text)
    {
        int textLen = text.Length;
        return GetSafeByteCountForDecoding(textLen, getPrefixCount(text, textLen, ZeroChar));
    }

    /// <summary>
    /// Retrieve safe byte count while avoiding multiple counting operations.
    /// </summary>
    /// <param name="textLen">Length of text.</param>
    /// <param name="numZeroes">Number of prefix zeroes.</param>
    /// <returns>Length of safe allocation.</returns>
    public static int GetSafeByteCountForDecoding(int textLen, int numZeroes)
    {
        Debug.Assert(textLen >= numZeroes, "Number of zeroes cannot be longer than text length");
        return numZeroes + ((textLen - numZeroes + 1) * reductionFactor / 1000) + 1;
    }

    /// <inheritdoc/>
    public int GetSafeCharCountForEncoding(ReadOnlySpan<byte> bytes)
    {
        int bytesLen = bytes.Length;
        int numZeroes = getZeroCount(bytes, bytesLen);

        return getSafeCharCountForEncoding(bytesLen, numZeroes);
    }

    /// <summary>
    /// Encode to Encode58 representation.
    /// </summary>
    /// <param name="bytes">Bytes to encode.</param>
    /// <returns>Encoded string.</returns>
    public unsafe string Encode(ReadOnlySpan<byte> bytes)
    {
        int bytesLen = bytes.Length;
        if (bytesLen == 0)
        {
            return string.Empty;
        }

        int numZeroes = getZeroCount(bytes, bytesLen);
        int outputLen = getSafeCharCountForEncoding(bytesLen, numZeroes);
        string output = new string('\0', outputLen);

        // 29.70µs (64.9x slower)   | 31.63µs (40.8x slower)
        // 30.93µs (first tryencode impl)
        // 29.36µs (single pass translation/copy + shift over multiply)
        // 31.04µs (70x slower)     | 24.71µs (34.3x slower)
        fixed (byte* inputPtr = bytes)
        fixed (char* outputPtr = output)
        {
            return internalEncode(inputPtr, bytesLen, outputPtr, outputLen, numZeroes, out int length)
                ? output[..length]
                : throw new InvalidOperationException("Output buffer with insufficient size generated");
        }
    }

    /// <summary>
    /// Decode a Encode58 representation.
    /// </summary>
    /// <param name="text">Encode58 encoded text.</param>
    /// <returns>Array of decoded bytes.</returns>
    public unsafe Span<byte> Decode(ReadOnlySpan<char> text)
    {
        int textLen = text.Length;
        if (textLen == 0)
        {
            return Array.Empty<byte>();
        }

        char zeroChar = ZeroChar;
        int numZeroes = getPrefixCount(text, textLen, zeroChar);
        int outputLen = GetSafeByteCountForDecoding(textLen, numZeroes);
        byte[] output = new byte[outputLen];
        fixed (char* inputPtr = text)
        fixed (byte* outputPtr = output)
        {
#pragma warning disable IDE0046 // Convert to conditional expression - prefer clarity
            if (!internalDecode(
                inputPtr,
                textLen,
                outputPtr,
                outputLen,
                numZeroes,
                out int numBytesWritten))
            {
                throw new InvalidOperationException("Output buffer was too small while decoding Encode58");
            }

            return output.AsSpan()[..numBytesWritten];
#pragma warning restore IDE0046 // Convert to conditional expression
        }
    }

    /// <inheritdoc/>
    public unsafe bool TryEncode(ReadOnlySpan<byte> input, Span<char> output, out int numCharsWritten)
    {
        fixed (byte* inputPtr = input)
        fixed (char* outputPtr = output)
        {
            int inputLen = input.Length;
            int numZeroes = getZeroCount(input, inputLen);
            return internalEncode(inputPtr, inputLen, outputPtr, output.Length, numZeroes, out numCharsWritten);
        }
    }

    /// <inheritdoc/>
    public unsafe bool TryDecode(ReadOnlySpan<char> input, Span<byte> output, out int numBytesWritten)
    {
        int inputLen = input.Length;
        if (inputLen == 0)
        {
            numBytesWritten = 0;
            return true;
        }

        int zeroCount = getPrefixCount(input, inputLen, ZeroChar);
        fixed (char* inputPtr = input)
        fixed (byte* outputPtr = output)
        {
            return internalDecode(
                inputPtr,
                input.Length,
                outputPtr,
                output.Length,
                zeroCount,
                out numBytesWritten);
        }
    }

    private unsafe bool internalDecode(
        char* inputPtr,
        int inputLen,
        byte* outputPtr,
        int outputLen,
        int numZeroes,
        out int numBytesWritten)
    {
        char* pInputEnd = inputPtr + inputLen;
        char* pInput = inputPtr + numZeroes;
        if (pInput == pInputEnd)
        {
            if (numZeroes > outputLen)
            {
                numBytesWritten = 0;
                return false;
            }

            byte* pOutput = outputPtr;
            for (int i = 0; i < numZeroes; i++)
            {
                *pOutput++ = 0;
            }

            numBytesWritten = numZeroes;
            return true;
        }

        var table = Provider.ReverseLookupTable;
        byte* pOutputEnd = outputPtr + outputLen - 1;
        byte* pMinOutput = pOutputEnd;
        while (pInput != pInputEnd)
        {
            char c = *pInput;
            int carry = table[c] - 1;
            if (carry < 0)
            {
                throw EncodingProvider.InvalidCharacter(c);
            }

            for (byte* pOutput = pOutputEnd; pOutput >= outputPtr; pOutput--)
            {
                carry += 58 * (*pOutput);
                *pOutput = (byte)carry;
                if (pMinOutput > pOutput && carry != 0)
                {
                    pMinOutput = pOutput;
                }

                carry >>= 8;
            }

            pInput++;
        }

        int startIndex = (int)(pMinOutput - numZeroes - outputPtr);
        numBytesWritten = outputLen - startIndex;
        Buffer.MemoryCopy(outputPtr + startIndex, outputPtr, numBytesWritten, numBytesWritten);
        return true;
    }

    private unsafe bool internalEncode(
        byte* inputPtr,
        int inputLen,
        char* outputPtr,
        int outputLen,
        int numZeroes,
        out int numCharsWritten)
    {
        if (inputLen == 0)
        {
            numCharsWritten = 0;
            return true;
        }

        fixed (char* providerPtr = Provider.Value)
        {
            byte* pInput = inputPtr + numZeroes;
            byte* pInputEnd = inputPtr + inputLen;
            char zeroChar = providerPtr[0];

            // optimized path for an all zero buffer
            if (pInput == pInputEnd)
            {
                if (outputLen < numZeroes)
                {
                    numCharsWritten = 0;
                    return false;
                }

                for (int i = 0; i < numZeroes; i++)
                {
                    *outputPtr++ = zeroChar;
                }

                numCharsWritten = numZeroes;
                return true;
            }

            int length = 0;
            char* pOutput = outputPtr;
            char* pLastChar = pOutput + outputLen - 1;
            while (pInput != pInputEnd)
            {
                int carry = *pInput;
                int i = 0;
                for (char* pDigit = pLastChar; (carry != 0 || i < length)
                    && pDigit >= outputPtr; pDigit--, i++)
                {
                    carry += *pDigit << 8;
                    carry = Math.DivRem(carry, 58, out int remainder);
                    *pDigit = (char)remainder;
                }

                length = i;
                pInput++;
            }

            var pOutputEnd = pOutput + outputLen;

            // copy the characters to the beginning of the buffer
            // and translate them at the same time. if no copying
            // is needed, this only acts as the translation phase.
            for (char* a = outputPtr + numZeroes, b = pOutputEnd - length;
                b != pOutputEnd;
                a++, b++)
            {
                *a = providerPtr[*b];
            }

            // translate the zeroes at the start
            while (pOutput != pOutputEnd)
            {
                char c = *pOutput;
                if (c != '\0')
                {
                    break;
                }

                *pOutput = providerPtr[c];
                pOutput++;
            }

            int actualLen = numZeroes + length;

            numCharsWritten = actualLen;
            return true;
        }
    }

    private static unsafe int getZeroCount(ReadOnlySpan<byte> bytes, int bytesLen)
    {
        if (bytesLen == 0)
        {
            return 0;
        }

        int numZeroes = 0;
        fixed (byte* inputPtr = bytes)
        {
            var pInput = inputPtr;
            while (*pInput == 0 && numZeroes < bytesLen)
            {
                numZeroes++;
                pInput++;
            }
        }

        return numZeroes;
    }

    private static unsafe int getPrefixCount(ReadOnlySpan<char> input, int length, char value)
    {
        if (length == 0)
        {
            return 0;
        }

        int numZeroes = 0;
        fixed (char* inputPtr = input)
        {
            var pInput = inputPtr;
            while (*pInput == value && numZeroes < length)
            {
                numZeroes++;
                pInput++;
            }
        }

        return numZeroes;
    }

    private static int getSafeCharCountForEncoding(int bytesLen, int numZeroes)
    {
        const int growthPercentage = 138;

        return numZeroes + ((bytesLen - numZeroes) * growthPercentage / 100) + 1;
    }
}