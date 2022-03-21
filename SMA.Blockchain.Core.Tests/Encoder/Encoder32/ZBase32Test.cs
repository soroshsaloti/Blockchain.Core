namespace SMA.Blockchain.Core.Tests.Encoder.Encoder32Test;
 
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using System;
using System.Text;
using Xunit;

public class ZEncoder32Test
{
    [Theory]
    [MemberData(nameof(DemoTests.testDataZEncoder32), MemberType = typeof(DemoTests))]
    public void Encode_ReturnsExpectedValues(string input, string expectedOutput)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        string result = Encoders.ZEncode32.Encode(bytes, padding: false);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.testDataZEncoder32), MemberType = typeof(DemoTests))]
    public void Decode_ReturnsExpectedValues(string expectedOutput, string input)
    {
        var bytes = Encoders.ZEncode32.Decode(input);
        string result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
        bytes = Encoders.ZEncode32.Decode(input.ToLowerInvariant());
        result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void Encode_NullBytes_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.ZEncode32.Encode(null, padding: false));
    }

    [Fact]
    public void Decode_InvalidInput_ThrowsArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => Encoders.ZEncode32.Decode("[];',m."));
    }
}

