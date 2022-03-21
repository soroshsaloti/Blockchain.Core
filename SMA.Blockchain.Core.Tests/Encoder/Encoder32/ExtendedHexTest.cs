namespace SMA.Blockchain.Core.Tests.Encoder.Encoder32Test;
using System;
using System.Text;
using System.IO;
using Xunit;
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;

public class ExtendedHexTest
{
    [Theory]
    [MemberData(nameof(DemoTests.testDataExtendedHex), MemberType = typeof(DemoTests))]
    public void Encode_Stream_ReturnsExpectedValues(string input, string expectedOutput)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        using var inputStream = new MemoryStream(bytes);
        using var writer = new StringWriter();
        Encoders.ExtendedHex.Encode(inputStream, writer, padding: true);
        writer.ToString().Should().Be(expectedOutput);

    }

    [Theory]
    [MemberData(nameof(DemoTests.testDataExtendedHex), MemberType = typeof(DemoTests))]
    public void Decode_Stream_ReturnsExpectedValues(string expectedOutput, string input)
    {
        // upper case
        using (var inputStream = new StringReader(input))
        using (var outputStream = new MemoryStream())
        {
            Encoders.ExtendedHex.Decode(inputStream, outputStream);
            string result = Encoding.ASCII.GetString(outputStream.ToArray());
            result.Should().Be(expectedOutput);
        }

        // lower case
        using (var inputStream = new StringReader(input.ToLowerInvariant()))
        using (var outputStream = new MemoryStream())
        {
            Encoders.ExtendedHex.Decode(inputStream, outputStream);
            string result = Encoding.ASCII.GetString(outputStream.ToArray());
            result.Should().Be(expectedOutput);
        }
    }

    [Theory]
    [MemberData(nameof(DemoTests.testDataExtendedHex), MemberType = typeof(DemoTests))]
    public void Encode_ReturnsExpectedValues(string input, string expectedOutput)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        string result = Encoders.ExtendedHex.Encode(bytes, padding: true);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.testDataExtendedHex), MemberType = typeof(DemoTests))]
    public void Decode_ReturnsExpectedValues(string expectedOutput, string input)
    {
        var bytes = Encoders.ExtendedHex.Decode(input);
        string result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
        bytes = Encoders.ExtendedHex.Decode(input.ToLowerInvariant());
        result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void Encode_NullBytes_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.ExtendedHex.Encode(null, false));
    }

    [Theory]
    [InlineData("!@#!#@!#@#!@")]
    [InlineData("||||")]
    public void Decode_InvalidInput_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => Encoders.ExtendedHex.Decode(input));
    }
}
