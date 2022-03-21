namespace SMA.Blockchain.Core.Tests.Encoder.Encoder32Test;

using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using Xunit;

public class GeohashTest
{
    [Fact]
    public void Decode_SmokeTest()
    {
        const string input = "ezs42";
        var result = Encoders.Geohash.Decode(input);
        var expected = new byte[] { 0b01101111, 0b11110000, 0b01000001 };
        result.ToArray().Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Encode_SmokeTest()
    {
        const string expected = "ezs42";
        var input = new byte[] { 0b01101111, 0b11110000, 0b01000001 };
        var result = Encoders.Geohash.Encode(input);
        result.Should().Be(expected);
    }
}
