namespace SMA.Blockchain.Core.Tests.Encoder85Test;
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using Xunit;
public class Encoder85ProviderTest
{
    [Fact]
    public void GetSafeCharCountForEncoding_Buffer_Works()
    {
        var input = new byte[] { 0, 1, 2, 3 };
        Encoders.Ascii85.GetSafeCharCountForEncoding(input).Should().Be(8);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 5)]
    [InlineData(2, 6)]
    [InlineData(3, 7)]
    [InlineData(4, 8)]
    [InlineData(5, 10)]
    [InlineData(8, 13)]
    public void GetSafeCharCountForEncoding_Length_Works(int inputLen, int expectedSize)
    {
        var buffer = new byte[inputLen];
        Encoders.Ascii85.GetSafeCharCountForEncoding(buffer).Should().Be(expectedSize);
    }

    [Fact]
    public void HasShortcut()
    {
        Assert.True(Encoders.Ascii85.Provider.HasShortcut);
        Assert.False(Encoders.Z85.Provider.HasShortcut);
    }
}
