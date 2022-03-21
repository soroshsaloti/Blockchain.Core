namespace SMA.Blockchain.Core.Tests.Encoder.Encoder32Test;
using System;
using System.Text; 
using Xunit;
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;

public class Rfc4648Test
{ 
    [Theory]
    [MemberData(nameof(DemoTests.testDataRFC), MemberType = typeof(DemoTests))]
    public void Encode_ReturnsExpectedValues(string input, string expectedOutput)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        string result = Encoders.Rfc4648.Encode(bytes, padding: true);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.testDataRFC), MemberType = typeof(DemoTests))]
    public void Decode_ReturnsExpectedValues(string expectedOutput, string input)
    {
        var bytes = Encoders.Rfc4648.Decode(input);
        string result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
        bytes = Encoders.Rfc4648.Decode(input.ToLowerInvariant());
        result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void Encode_NullBytes_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Rfc4648.Encode(null, true));
    }

    [Fact]
    public void Decode_InvalidInput_ThrowsArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => Encoders.Rfc4648.Decode("[];',m."));
    }
}
