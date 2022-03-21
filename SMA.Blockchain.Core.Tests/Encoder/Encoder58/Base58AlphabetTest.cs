namespace SMA.Blockchain.Core.Tests.Encoder58Test;
using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Encoder.Provider;
using System;
using Xunit;

public class Encoder58ProviderTest
{
    [Fact]
    public void Ctor_InvalidLength_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Encode58Provider("123"));
    }

    [Fact]
    public void GetSafeCharCountForEncoding_Works()
    {
        var input = new byte[] { 0, 0, 0, 0, 1, 2, 3, 4 };
        Encoders.Bitcoin.GetSafeCharCountForEncoding(input).Should().Be(10);
    }
}
