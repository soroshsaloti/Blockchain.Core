namespace SMA.Blockchain.Core.Tests.Encoder.Encoder32Test;
using System;
using System.Text;
using Xunit;
using FluentAssertions; 
using SMA.Blockchain.Core.Tests.Encoder;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;

public class FileCoinTest
{
    [Theory]
    [MemberData(nameof(DemoTests.testDataFileCoin), MemberType = typeof(DemoTests))]
    public void Encode_ReturnsExpectedValues(string input, string expectedOutput)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        string result = Encoders.FileCoin.Encode(bytes, padding: true);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.byteTestDataFileCoin), MemberType = typeof(DemoTests))]
    public void Encode_Bytes_ReturnsExpectedValues(byte[] bytes, string expectedOutput, bool padding)
    {
        string result = Encoders.FileCoin.Encode(bytes, padding: padding);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.testDataFileCoin), MemberType = typeof(DemoTests))]
    public void Decode_ReturnsExpectedValues(string expectedOutput, string input)
    {
        var bytes = Encoders.FileCoin.Decode(input);
        string result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
        bytes = Encoders.FileCoin.Decode(input.ToLowerInvariant());
        result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void Encode_NullBytes_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.FileCoin.Encode(null, true));
    }

    [Fact]
    public void Decode_InvalidInput_ThrowsArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => Encoders.FileCoin.Decode("[];',m."));
    }
}
