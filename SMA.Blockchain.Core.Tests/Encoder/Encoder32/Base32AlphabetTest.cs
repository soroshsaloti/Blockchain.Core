namespace SMA.Blockchain.Core.Tests.Encoder.Encoder32Test;

using FluentAssertions;
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Encoder.Provider;
using System;
using Xunit;

public class Encoder32ProviderTest
{
    [Fact]
    public void ctor_NullProvider_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Encode32Provider(null));
    }

    [Fact]
    public void ctorWithPaddingChar_Works()
    {
        var alpha = new Encode32Provider("0123456789abcdef0123456789abcdef", '!');

        alpha.PaddingChar.Should().Be('!');
    }

    [Theory]
    [InlineData("12345", 3)]
    [InlineData("", 0)]
    public void GetSafeByteCountForDecoding_Works(string param, int expect)
    {
        Encoders.Crockford.GetSafeByteCountForDecoding(param).Should().Be(expect);
    }
}
