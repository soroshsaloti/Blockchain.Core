namespace SMA.Blockchain.Core.Tests.Encoder85Test; 
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Tests.Encoder;
using Xunit;

public class Z85Test
{
    [Theory]
    [MemberData(nameof(DemoTests.testVectorsZ85), MemberType = typeof(DemoTests))]
    public void Encode_TestVectors_ShouldEncodeCorrectly(byte[] input, string expectedOutput)
    {
        var result = Encoders.Z85.Encode(input);
        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void Encode_NullBuffer_ReturnsEmptyString()
    {
        Assert.Empty(Encoders.Z85.Encode(null));
    }

    [Theory]
    [MemberData(nameof(DemoTests.testVectorsZ85), MemberType = typeof(DemoTests))]
    public void Decode_TestVectors_ShouldDecodeCorrectly(byte[] expectedOutput, string input)
    {
        var result = Encoders.Z85.Decode(input);
        result.ToArray().Should().BeEquivalentTo(expectedOutput);

    }
}
