namespace SMA.Blockchain.Core.Tests.Encoder.Encoder85Test;
using System;
using System.IO;
using System.Threading.Tasks; 
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using Xunit;

public class Ascii85Test
{

    [Fact]
    public void Decode_InvalidShortcut_ThrowsArgumentException()
    {
        const string input = "9zjqo";
        Assert.Throws<ArgumentException>(() => Encoders.Ascii85.Decode(input));
    }

    [Fact]
    public void Decode_InvalidCharacter_ThrowsArgumentException()
    {
        const string input = "~!@#()(";
        Assert.Throws<ArgumentException>(() => Encoders.Ascii85.Decode(input));
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public void Decode_Whitespace_IsIgnored(byte[] expectedOutput, string input)
    {
        string actualInput = String.Empty;
        for (int i = 0; i < input.Length; i++)
        {
            actualInput += "  " + input[i];
        }
        actualInput += " ";
        var result = Encoders.Ascii85.Decode(actualInput);
        expectedOutput.Should().BeEquivalentTo(result.ToArray());
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public void Encode_TestVectorsOnStream_ShouldEncodeCorrectly(byte[] input, string expectedOutput)
    {
        using var inputStream = new MemoryStream(input);
        using var writer = new StringWriter();
        Encoders.Ascii85.Encode(inputStream, writer);
        writer.ToString().Should().BeEquivalentTo(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public async Task EncodeAsync_TestVectorsOnStream_ShouldEncodeCorrectly(byte[] input, string expectedOutput)
    {
        using var inputStream = new MemoryStream(input);
        using var writer = new StringWriter();
        await Encoders.Ascii85.EncodeAsync(inputStream, writer);
        writer.ToString().Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public void Encode_TestVectors_ShouldEncodeCorrectly(byte[] input, string expectedOutput)
    {
        var result = Encoders.Ascii85.Encode(input);
        result.Should().Be(expectedOutput);
    }
    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public void TryEncode_TestVectors_ShouldEncodeCorrectly(byte[] input, string expectedOutput)
    {
        var output = new char[Encoders.Ascii85.GetSafeCharCountForEncoding(input)];
        Assert.True(Encoders.Ascii85.TryEncode(input, output, out int numCharsWritten));
        new string(output[..numCharsWritten]).Should().Be(expectedOutput);
    }

    [Fact]
    public void Encode_UnevenBuffer_DoesNotThrowArgumentException()
    {
        var exception = Record.Exception(() => Encoders.Ascii85.Encode(new byte[3]));
        Assert.Null(exception);
    }

    [Fact]
    public void Encode_NullBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Ascii85.Encode(null));
    }

    [Fact]
    public void Decode_UnevenText_DoesNotThrowArgumentException()
    {
        var exception = Record.Exception(() => Encoders.Ascii85.Decode("hebe"));
        Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public void Decode_TestVectorsWithStream_ShouldDecodeCorrectly(byte[] expectedOutput, string input)
    {
        using var inputStream = new StringReader(input);
        using var writer = new MemoryStream();
        Encoders.Ascii85.Decode(inputStream, writer);
        expectedOutput.Should().BeEquivalentTo(writer.ToArray());
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public async Task DecodeAsync_TestVectorsWithStream_ShouldDecodeCorrectly(byte[] expectedOutput, string input)
    {
        using var inputStream = new StringReader(input);
        using var writer = new MemoryStream();
        await Encoders.Ascii85.DecodeAsync(inputStream, writer);
        expectedOutput.Should().BeEquivalentTo(writer.ToArray());

    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public void Decode_TestVectors_ShouldDecodeCorrectly(byte[] expectedOutput, string input)
    {
        var result = Encoders.Ascii85.Decode(input);
        expectedOutput.Should().BeEquivalentTo(result.ToArray());
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectors), MemberType = typeof(DemoTests))]
    public void TryDecode_TestVectors_ShouldDecodeCorrectly(byte[] expectedOutput, string input)
    {
        var buffer = new byte[Encoders.Ascii85.GetSafeByteCountForDecoding(input)];
        Assert.True(Encoders.Ascii85.TryDecode(input, buffer, out int numBytesWritten));
        expectedOutput.Should().BeEquivalentTo(buffer[..numBytesWritten]);
    }
}
