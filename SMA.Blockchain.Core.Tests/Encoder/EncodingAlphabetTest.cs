namespace SMA.Blockchain.Core.Tests.Encoder;
using System;
using BaseEncoder;
using FluentAssertions;
using SMA.Blockchain.Core.Encoder.Provider;
using Xunit;
public class EncodingProviderTest
{
    [Fact]
    public void ctor_NullProvider_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Encode16Provider(null));
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var str = "0123456789abcdef";
        var alpha = new Encode16Provider(str);
        alpha.ToString().Should().Be("0123456789abcdef");
    }
}

