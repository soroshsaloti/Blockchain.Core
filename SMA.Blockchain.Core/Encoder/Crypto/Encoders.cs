namespace SMA.Blockchain.Core.Encoder.Crypto;
public static class Encoders
{
    private static readonly Lazy<Encode16> upperCase = new Lazy<Encode16>(() => new Encode16(Providers.UpperCase));
    private static readonly Lazy<Encode16> lowerCase = new Lazy<Encode16>(() => new Encode16(Providers.LowerCase));
    private static readonly Lazy<Encode16> modHex = new Lazy<Encode16>(() => new Encode16(Providers.ModHex));

    private static readonly Lazy<Encode32> crockford = new Lazy<Encode32>(() => new Encode32(Providers.Crockford));
    private static readonly Lazy<Encode32> rfc4648 = new Lazy<Encode32>(() => new Encode32(Providers.Rfc4648));
    private static readonly Lazy<Encode32> extendedHex = new Lazy<Encode32>(() => new Encode32(Providers.ExtendedHex));
    private static readonly Lazy<Encode32> zEncode32 = new Lazy<Encode32>(() => new Encode32(Providers.ZEncode32));
    private static readonly Lazy<Encode32> geohash = new Lazy<Encode32>(() => new Encode32(Providers.Geohash));
    private static readonly Lazy<Encode32> filecoin = new Lazy<Encode32>(() => new Encode32(Providers.FileCoin));

    private static readonly Lazy<Encode58> bitcoin = new Lazy<Encode58>(() => new Encode58(Providers.Bitcoin));
    private static readonly Lazy<Encode58> ripple = new Lazy<Encode58>(() => new Encode58(Providers.Ripple));
    private static readonly Lazy<Encode58> flickr = new Lazy<Encode58>(() => new Encode58(Providers.Flickr));

    private static readonly Lazy<Encode85> z85 = new Lazy<Encode85>(() => new Encode85(Providers.Z85));
    private static readonly Lazy<Encode85> ascii85 = new Lazy<Encode85>(() => new Encode85(Providers.Ascii85));

    /// <summary>
    /// Gets upper case Encode16 encoder. Decoding is case-insensitive.
    /// </summary>
    public static Encode16 UpperCase => upperCase.Value;

    /// <summary>
    /// Gets lower case Encode16 encoder. Decoding is case-insensitive.
    /// </summary>
    public static Encode16 LowerCase => lowerCase.Value;

    /// <summary>
    /// Gets lower case Encode16 encoder. Decoding is case-insensitive.
    /// </summary>
    public static Encode16 ModHex => modHex.Value;


    /// <summary>
    /// Gets Douglas Crockford's Encode32 flavor with substitution characters.
    /// </summary>
    public static Encode32 Crockford => crockford.Value;

    /// <summary>
    /// Gets RFC 4648 variant of Encode32 coder.
    /// </summary>
    public static Encode32 Rfc4648 => rfc4648.Value;

    /// <summary>
    /// Gets Extended Hex variant of Encode32 coder.
    /// </summary>
    /// <remarks>Also from RFC 4648.</remarks>
    public static Encode32 ExtendedHex => extendedHex.Value;

    /// <summary>
    /// Gets z-base-32 variant of Encode32 coder.
    /// </summary>
    /// <remarks>This variant is used in Mnet, ZRTP and Tahoe-LAFS.</remarks>
    public static Encode32 ZEncode32 => zEncode32.Value;

    /// <summary>
    /// Gets Geohash variant of Encode32 coder.
    /// </summary>
    public static Encode32 Geohash => geohash.Value;

    /// <summary>
    /// Gets FileCoin variant of Encode32 coder.
    /// </summary>
    public static Encode32 FileCoin => filecoin.Value;

    /// <summary>
    /// Gets Bitcoin flavor.
    /// </summary>
    public static Encode58 Bitcoin => bitcoin.Value;

    /// <summary>
    /// Gets Ripple flavor.
    /// </summary>
    public static Encode58 Ripple => ripple.Value;

    /// <summary>
    /// Gets Flickr flavor.
    /// </summary>
    public static Encode58 Flickr => flickr.Value;

    /// <summary>
    /// Gets Z85 flavor of Encode85.
    /// </summary>
    public static Encode85 Z85 => z85.Value;

    /// <summary>
    /// Gets Ascii85 flavor of Encode85.
    /// </summary>
    public static Encode85 Ascii85 => ascii85.Value;

}

