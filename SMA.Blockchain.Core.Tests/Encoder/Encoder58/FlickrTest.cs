namespace BaseEncoder.Tests.Encoder58Test;
using System; 
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Tests;
using SMA.Blockchain.Core.Tests.Encoder;
using Xunit;
public class FlickrTest
{
    [Fact]
    public void Encode_NullBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Flickr.Encode(null));
    }

    [Theory]
    [MemberData(nameof(DemoTests.flickrTestData), MemberType = typeof(DemoTests))]
    public void Encode_Flickr_ReturnsExpectedResults(string input, string expectedOutput)
    {
        var buffer = Encoders.UpperCase.Decode(input);
        string result = Encoders.Flickr.Encode(buffer);
        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(DemoTests.flickrTestData), MemberType = typeof(DemoTests))]
    public void TryEncode_Flickr_ReturnsExpectedResults(string input, string expectedOutput)
    {
        var inputBuffer = Encoders.UpperCase.Decode(input);
        var outputBuffer = new char[Encoders.Flickr.GetSafeCharCountForEncoding(inputBuffer)];
        Assert.True(Encoders.Flickr.TryEncode(inputBuffer, outputBuffer, out int numWritten));
        outputBuffer[..numWritten].Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Encode_EmptyBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Flickr.Encode(new byte[0]));
    }

    [Fact]
    public void Decode_EmptyString_ReturnsEmptyBuffer()
    {
        var result = Encoders.Flickr.Decode(String.Empty);
        result.Length.Should().Be(0);
    }

    [Fact]
    public void Decode_InvalidCharacter_Throws()
    {
        Assert.Throws<ArgumentException>(() => Encoders.Flickr.Decode("?"));
    }

    [Theory]
    [MemberData(nameof(DemoTests.flickrTestData), MemberType = typeof(DemoTests))]
    public void Decode_Flickr_ReturnsExpectedResults(string expectedOutput, string input)
    {
        var buffer = Encoders.Flickr.Decode(input);
        string result = BitConverter.ToString(buffer.ToArray()).Replace("-", "",
            StringComparison.Ordinal);
        result.Should().Be(expectedOutput);
    }
}
