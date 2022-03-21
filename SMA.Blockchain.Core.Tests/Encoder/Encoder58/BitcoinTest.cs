namespace SMA.Blockchain.Core.Tests.Encoder58Test;
using System; 
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Tests.Encoder;
using Xunit;

public class BitcoinTest
{
    [Fact]
    public void Encode_NullBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Bitcoin.Encode(null));
    }

    [Theory]
    [MemberData(nameof(DemoTests.bitcoinTestData), MemberType = typeof(DemoTests))]
    public void Encode_Bitcoin_ReturnsExpectedResults(string input, string expectedOutput)
    {
        var buffer = Encoders.UpperCase.Decode(input);
        string result = Encoders.Bitcoin.Encode(buffer);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.bitcoinTestData), MemberType = typeof(DemoTests))]
    public void TryEncode_Bitcoin_ReturnsExpectedResults(string input, string expectedOutput)
    {
        var inputBuffer = Encoders.UpperCase.Decode(input);
        var outputBuffer = new char[Encoders.Bitcoin.GetSafeCharCountForEncoding(inputBuffer)];
        Assert.True(Encoders.Bitcoin.TryEncode(inputBuffer, outputBuffer, out int numWritten));
        outputBuffer[..numWritten].Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Encode_EmptyBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Bitcoin.Encode(new byte[0]));
    }

    [Fact]
    public void Decode_EmptyString_ReturnsEmptyBuffer()
    {
        var result = Encoders.Bitcoin.Decode(String.Empty);
        result.Length.Should().Be(0);
    }

    [Fact]
    public void TryDecode_EmptyString_ReturnsEmptyBuffer()
    {
        var result = Encoders.Bitcoin.TryDecode(String.Empty, new byte[1], out int numBytesWritten);
        Assert.True(result);
        numBytesWritten.Should().Be(0);
    }

    [Fact]
    public void Decode_InvalidCharacter_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => Encoders.Bitcoin.Decode("?"));
    }

    [Fact]
    public void TryDecode_InvalidCharacter_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => Encoders.Bitcoin.TryDecode("?", new byte[10], out _));
    }

    [Theory]
    [MemberData(nameof(DemoTests.bitcoinTestData), MemberType = typeof(DemoTests))]
    public void Decode_Bitcoin_ReturnsExpectedResults(string expectedOutput, string input)
    {
        var buffer = Encoders.Bitcoin.Decode(input);
        string result = BitConverter.ToString(buffer.ToArray()).Replace("-", "",
            StringComparison.Ordinal);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.bitcoinTestData), MemberType = typeof(DemoTests))]
    public void TryDecode_Bitcoin_ReturnsExpectedResults(string expectedOutput, string input)
    {
        var output = new byte[Encoders.Bitcoin.GetSafeByteCountForDecoding(input)];
        var success = Encoders.Bitcoin.TryDecode(input, output, out int numBytesWritten);
        Assert.True(success);
        string result = BitConverter.ToString(output[..numBytesWritten]).Replace("-", "",
            StringComparison.Ordinal);
        result.Should().Be(expectedOutput);
    }
}
