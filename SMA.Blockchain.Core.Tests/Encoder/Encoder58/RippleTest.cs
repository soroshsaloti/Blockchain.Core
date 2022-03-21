namespace BaseEncoder.Tests.Encoder58Test;
using System; 
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Tests;
using SMA.Blockchain.Core.Tests.Encoder;
using Xunit;
public class RippleTest
{
    [Fact]
    public void Encode_NullBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Ripple.Encode(null));
    }

    [Theory]
    [MemberData(nameof(DemoTests.rippleTestData), MemberType = typeof(DemoTests))]
    public void Encode_Ripple_ReturnsExpectedResults(string input, string expectedOutput)
    {
        var buffer = Encoders.UpperCase.Decode(input);
        string result = Encoders.Ripple.Encode(buffer);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.rippleTestData), MemberType = typeof(DemoTests))]
    public void TryEncode_Ripple_ReturnsExpectedResults(string input, string expectedOutput)
    {
        var inputBuffer = Encoders.UpperCase.Decode(input);
        var outputBuffer = new char[Encoders.Ripple.GetSafeCharCountForEncoding(inputBuffer)];
        Assert.True(Encoders.Ripple.TryEncode(inputBuffer, outputBuffer, out int numWritten));
        outputBuffer[..numWritten].Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Encode_EmptyBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Ripple.Encode(new byte[0]));
    }

    [Fact]
    public void Decode_EmptyString_ReturnsEmptyBuffer()
    {
        var result = Encoders.Ripple.Decode(String.Empty);
        result.Length.Should().Be(0);
    }

    [Fact]
    public void Decode_InvalidCharacter_Throws()
    {
        Assert.Throws<ArgumentException>(() => Encoders.Ripple.Decode("?"));
    }

    [Theory]
    [MemberData(nameof(DemoTests.rippleTestData), MemberType = typeof(DemoTests))]
    public void Decode_Ripple_ReturnsExpectedResults(string expectedOutput, string input)
    {
        var buffer = Encoders.Ripple.Decode(input);
        string result = BitConverter.ToString(buffer.ToArray()).Replace("-", "",
            StringComparison.Ordinal);
        result.Should().Be(expectedOutput);
    }
}
