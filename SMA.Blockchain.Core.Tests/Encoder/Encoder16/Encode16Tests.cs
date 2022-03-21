namespace SMA.Blockchain.Core.Tests.Encoder.Encoder16Tests;

using Xunit; 
using System.IO;
using FluentAssertions;
using System.Threading.Tasks;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Provider;
using System;

public class Encoder16Tests
{

    [Theory]
    [InlineData("ABCDEFF0")]
    [InlineData("abcdeff0")]
    public void Decode_DecodesBothLowerAndUpperCase(string code)
    {
        var expectedResult = new byte[] { 0xAB, 0xCD, 0xEF, 0xF0 };
        var result = Encode16.Decode(code).ToArray();
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public void Decode_Stream(Encode16 encoder, byte[] expectedOutput, string input)
    {
        using var memoryStream = new MemoryStream();
        using var reader = new StringReader(input);
        encoder.Decode(reader, memoryStream);
        Assert.Equal(expectedOutput, memoryStream.ToArray());
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public void Encode_Stream(Encode16 encoder, byte[] input, string expectedOutput)
    {
        using var inputStream = new MemoryStream(input);
        using var writer = new StringWriter();
        encoder.Encode(inputStream, writer);
        writer.ToString().Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public async Task DecodeAsync_Stream(Encode16 encoder, byte[] expectedOutput, string input)
    {
        using var memoryStream = new MemoryStream();
        using var reader = new StringReader(input);
        await encoder.DecodeAsync(reader, memoryStream);
        Assert.Equal(expectedOutput, memoryStream.ToArray());
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public async Task EncodeAsync_StreamAsync(Encode16 encoder, byte[] input, string expectedOutput)
    {
        using var inputStream = new MemoryStream(input);
        using var writer = new StringWriter();
        await encoder.EncodeAsync(inputStream, writer);
        writer.ToString().Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public void Encode(Encode16 encoder, byte[] input, string expectedOutput)
    {
        var result = encoder.Encode(input);
        result.ToString().Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public void TryEncode_RegularInput_Succeeds(Encode16 encoder, byte[] input, string expectedOutput)
    {
        var output = new char[input.Length * 2];
        var result = encoder.TryEncode(input, output, out int numCharsWritten);
        Assert.True(result);
        numCharsWritten.Should().Be(output.Length);
        new string(output).Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void TryEncode_SmallerOutput_Fails(Encode16 encoder)
    {
        var input = new byte[4];
        var output = new char[0];
        var result = encoder.TryEncode(input, output, out int numCharsWritten);
        Assert.False(result);
        numCharsWritten.Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public void Decode(Encode16 encoder, byte[] expectedOutput, string input)
    {
        var result = encoder.Decode(input);
        expectedOutput.Should().BeEquivalentTo(result.ToArray());
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public void TryDecode_RegularInput_Succeeds(Encode16 encoder, byte[] expectedOutput, string input)
    {
        var output = new byte[expectedOutput.Length];
        var result = encoder.TryDecode(input, output, out int numBytesWritten);
        Assert.True(result);
        numBytesWritten.Should().Be(output.Length);
        expectedOutput.Should().BeEquivalentTo(output);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void TryDecode_SmallOutputBuffer_Fails(Encode16 encoder)
    {
        var input = "1234";
        var output = new byte[1];
        var result = encoder.TryDecode(input, output, out int numBytesWritten);
        Assert.False(result);
        numBytesWritten.Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void TryDecode_UnevenInputBuffer_Fails(Encode16 encoder)
    {
        var input = "123";
        var output = new byte[1];
        var result = encoder.TryDecode(input, output, out int numBytesWritten);
        Assert.False(result);
        numBytesWritten.Should().Be(0);
        //Assert.That(encoder.TryDecode(input, output, out int numBytesWritten), Is.False);
        //Assert.That(numBytesWritten, Is.EqualTo(0));
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTestData), MemberType = typeof(DemoTests))]
    public void Decode_OtherCase_StillPasses(Encode16 encoder, byte[] expectedOutput, string input)
    {
        var result = encoder.Decode(input.ToUpperInvariant());
         expectedOutput.Should().BeEquivalentTo( result.ToArray());
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void GetSafeCharCountForEncoding_ReturnsCorrectValue(Encode16 encoder)
    {
        var input = new byte[5];
        encoder.GetSafeCharCountForEncoding(input).Should().Be(10);
        
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void GetSafeByteCountForDecoding_ReturnsCorrectValues(Encode16 encoder)
    {
        var input = new char[10];
        encoder.GetSafeByteCountForDecoding(input).Should().Be(5);  
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void GetSafeByteCountForDecoding_InvalidBufferSize_ReturnsZero(Encode16 encoder)
    {
        var input = new char[11];
         encoder.GetSafeByteCountForDecoding(input).Should().Be(0);
    }

    [Fact]
    public void CustomCtor()
    {
        var encoder = new Encode16(new Encode16Provider("abcdefghijklmnop"));
        var result = encoder.Encode(new byte[] { 0, 1, 16, 128, 255 });
        result.Should().Be("aaabbaiapp");
    }

    [Theory] 
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void ToString_ReturnsNameWithProvider( Encode16 encoder)
    {
        encoder.ToString().Should().Be($"Encode16_{encoder.Provider}"); 
    }

    [Theory]
    [MemberData(nameof(DemoTests.encodersTypeTestData), MemberType = typeof(DemoTests))]
    public void GetHashCode_ReturnsProviderHashCode(Encode16 encoder)
    {
        encoder.Provider.GetHashCode().Should().Be(encoder.GetHashCode()); 
    }
}

