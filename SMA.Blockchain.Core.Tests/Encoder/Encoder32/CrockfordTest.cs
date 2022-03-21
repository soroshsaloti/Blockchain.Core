namespace SMA.Blockchain.Core.Tests.Encoder.Encoder32Test;

using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class CrockfordTest
{
    private static readonly object[][] testData = {
            new object[] { "", "", false },
            new object[] { "f", "CR", false },
            new object[] { "f", "CR======", true },
            new object[] { "fo", "CSQG", false },
            new object[] { "fo", "CSQG====", true },
            new object[] { "foo", "CSQPY", false },
            new object[] { "foo", "CSQPY===", true },
            new object[] { "foob", "CSQPYRG", false },
            new object[] { "foob", "CSQPYRG=", true },
            new object[] { "fooba", "CSQPYRK1", false },
            new object[] { "fooba", "CSQPYRK1", true },
            new object[] { "foobar", "CSQPYRK1E8", false },
            new object[] { "foobar", "CSQPYRK1E8======", true },
            new object[] { "123456789012345678901234567890123456789", "64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE8", false },
            new object[] { "123456789012345678901234567890123456789", "64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE8=", true }
        };

     
    [Fact]
    public void Encode_SampleInterface_Compiles()
    {
        byte[] myBuffer = new byte[0];
        string result = Encoders.Crockford.Encode(myBuffer, padding: true);
        Assert.Empty(result);
    }

    [Fact]
    public void Decode_SampleInterface_Compiles()
    { 
        string myText = "CSQPYRK1E8"; // any buffer will do
        Span<byte> result = Encoders.Crockford.Decode(myText);
        Assert.True(result.Length > 0 );
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void Encode_Stream_ReturnsExpectedValues(string input, string expectedOutput, bool padded)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        using var inputStream = new MemoryStream(bytes);
        using var writer = new StringWriter();
        Encoders.Crockford.Encode(inputStream, writer, padded);
        writer.ToString().Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void Encode_SimpleStream_ReturnsExpectedValues(string input, string expectedOutput, bool padded)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        using var inputStream = new MemoryStream(bytes);
        using var writer = new StringWriter();
        Encoders.Crockford.Encode(inputStream, writer, padded);
        writer.ToString().Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public async Task EncodeAsync_SimpleStream_ReturnsExpectedValues(string input, string expectedOutput, bool padded)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        using var inputStream = new MemoryStream(bytes);
        using var writer = new StringWriter();
        await Encoders.Crockford.EncodeAsync(inputStream, writer, padded);
        writer.ToString().Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void Decode_Stream_ReturnsExpectedValues(string expectedOutput, string input, bool _)
    {
        // upper case
        using (var inputStream = new StringReader(input))
        using (var outputStream = new MemoryStream())
        {
            Encoders.Crockford.Decode(inputStream, outputStream);
            string result = Encoding.ASCII.GetString(outputStream.ToArray());
            result.Should().Be(expectedOutput);
        }

        // lower case
        using (var inputStream = new StringReader(input.ToLowerInvariant()))
        using (var outputStream = new MemoryStream())
        {
            Encoders.Crockford.Decode(inputStream, outputStream);
            string result = Encoding.ASCII.GetString(outputStream.ToArray());
            result.Should().Be(expectedOutput);
        }
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public async Task DecodeAsync_Stream_ReturnsExpectedValues(string expectedOutput, string input, bool _)
    {
        // upper case
        using (var inputStream = new StringReader(input))
        using (var outputStream = new MemoryStream())
        {
            await Encoders.Crockford.DecodeAsync(inputStream, outputStream);
            string result = Encoding.ASCII.GetString(outputStream.ToArray());
            result.Should().Be(expectedOutput);
        }

        // lower case
        using (var inputStream = new StringReader(input.ToLowerInvariant()))
        using (var outputStream = new MemoryStream())
        {
            await Encoders.Crockford.DecodeAsync(inputStream, outputStream);
            string result = Encoding.ASCII.GetString(outputStream.ToArray());
            result.Should().Be(expectedOutput);
        }
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void Encode_ReturnsExpectedValues(string input, string expectedOutput, bool padded)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        string result = Encoders.Crockford.Encode(bytes, padded);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void Encode_Simple_ReturnsExpectedValues(string input, string expectedOutput, bool padded)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        string result = Encoders.Crockford.Encode(bytes, padded);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void TryEncode_Simple_ReturnsExpectedValues(string input, string expectedOutput, bool padded)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        var output = new char[Encoders.Crockford.GetSafeCharCountForEncoding(bytes)];
        bool success = Encoders.Crockford.TryEncode(bytes, output, padded, out int numCharsWritten);
        Assert.True(success);
        output[..numCharsWritten].Should().BeEquivalentTo(expectedOutput); 
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void Decode_ReturnsExpectedValues(string expectedOutput, string input, bool _)
    {
        var bytes = Encoders.Crockford.Decode(input);
        string result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
        bytes = Encoders.Crockford.Decode(input.ToLowerInvariant());
        result = Encoding.ASCII.GetString(bytes.ToArray());
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.Crockford), MemberType = typeof(DemoTests))]
    public void TryDecode_ReturnsExpectedValues(string expectedOutput, string input, bool _)
    {
        var output = new byte[Encoders.Crockford.GetSafeByteCountForDecoding(input)];
        var success = Encoders.Crockford.TryDecode(input, output, out int numBytesWritten);
        Assert.True(success);
        string result = Encoding.ASCII.GetString(output[..numBytesWritten]);
        result.Should().Be(expectedOutput);
        
        success = Encoders.Crockford.TryDecode(input.ToLowerInvariant(), output, out numBytesWritten);
        Assert.True(success);
        result = Encoding.ASCII.GetString(output[..numBytesWritten]);
        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void TryDecode_ZeroBuffer_ReturnsFalse()
    {
        var success = Encoders.Crockford.TryDecode("test", new byte[0], out int numBytesWritten);
        Assert.False(success);
        numBytesWritten.Should().Be(0);
    }

    [Fact]
    public void Decode_InvalidInput_ThrowsArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => Encoders.Crockford.Decode("[];',m."));
    }

    [Theory]
    [InlineData("O0o", "000")]
    [InlineData("Ll1", "111")]
    [InlineData("I1i", "111")]
    public void Decode_CrockfordChars_DecodedCorrectly(string equivalent, string actual)
    {
        var expectedResult = Encoders.Crockford.Decode(actual);
        var result = Encoders.Crockford.Decode(equivalent);
        result.ToArray().Should().BeEquivalentTo(expectedResult.ToArray());
    }

    [Fact]
    public void Encode_NullBytes_ReturnsEmptyString()
    {
        Encoders.Crockford.Encode(null, true).Should().Be(String.Empty);
    }
}
