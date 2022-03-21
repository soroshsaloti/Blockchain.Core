namespace SMA.Blockchain.Core.Encoder.Crypto;
using SMA.Blockchain.Core.Encoder.Provider;

public static class Providers
{
    public const string upperCase = "0123456789ABCDEF";
    public const string lowerCase = "0123456789abcdef";
    public const string modHex = "cbdefghijklnrtuv";

    public const string rfc4648 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
    public const string extendedHex = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
    public const string zEncode32 = "ybndrfg8ejkmcpqxot1uwisza345h769";
    public const string geohash = "0123456789bcdefghjkmnpqrstuvwxyz";
    public const string fileCoin = "abcdefghijklmnopqrstuvwxyz234567";

    public const string bitcoin = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    public const string ripple = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";
    public const string flickr = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";

    public const string z = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";

    public const string ascii = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstu";

    private static readonly Lazy<Encode16Provider> upperCaseProvider = new Lazy<Encode16Provider>(
            () => new Encode16Provider(upperCase));
    private static readonly Lazy<Encode16Provider> lowerCaseProvider = new Lazy<Encode16Provider>(
            () => new Encode16Provider(lowerCase));
    private static readonly Lazy<Encode16Provider> modHexProvider = new Lazy<Encode16Provider>(
            () => new Encode16Provider(modHex));

    private static readonly Lazy<CrockfordEncode32Provider> crockfordProvider = new Lazy<CrockfordEncode32Provider>(
            () => new CrockfordEncode32Provider());
    private static readonly Lazy<Encode32Provider> rfc4648Provider = new Lazy<Encode32Provider>(
            () => new Encode32Provider(rfc4648));
    private static readonly Lazy<Encode32Provider> extendedHexProvider = new Lazy<Encode32Provider>(
            () => new Encode32Provider(extendedHex));
    private static readonly Lazy<Encode32Provider> zEncode32Provider = new Lazy<Encode32Provider>(
            () => new Encode32Provider(zEncode32));
    private static readonly Lazy<Encode32Provider> geohashProvider = new Lazy<Encode32Provider>(
            () => new Encode32Provider(geohash));
    private static readonly Lazy<Encode32Provider> fileCoinProvider = new Lazy<Encode32Provider>(
            () => new Encode32Provider(fileCoin));

    private static readonly Lazy<Encode58Provider> bitcoinProvider = new Lazy<Encode58Provider>(()
            => new Encode58Provider(bitcoin));
    private static readonly Lazy<Encode58Provider> rippleProvider = new Lazy<Encode58Provider>(()
            => new Encode58Provider(ripple));
    private static readonly Lazy<Encode58Provider> flickrProvider = new Lazy<Encode58Provider>(()
            => new Encode58Provider(flickr));

    private static readonly Lazy<Encode85Provider> z85 = new Lazy<Encode85Provider>(() => new Encode85Provider(z));
    private static readonly Lazy<Encode85Provider> ascii85 = new Lazy<Encode85Provider>(() => new Encode85Provider(
            ascii,
            allZeroShortcut: 'z',
            allSpaceShortcut: 'y'));


    /// <summary>
    /// Gets upper case Encode16 provider.
    /// </summary>
    public static Encode16Provider UpperCase { get; } = upperCaseProvider.Value;

    /// <summary>
    /// Gets lower case Encode16 provider.
    /// </summary>
    public static Encode16Provider LowerCase { get; } = lowerCaseProvider.Value;

    /// <summary>
    /// Gets ModHex Encode16 provider, used by Yubico apps.
    /// </summary>
    public static Encode16Provider ModHex { get; } = modHexProvider.Value;

    /// <summary>
    /// Gets Crockford provider.
    /// </summary>gpg
    public static Encode32Provider Crockford => crockfordProvider.Value;

    /// <summary>
    /// Gets RFC4648 provider.
    /// </summary>
    public static Encode32Provider Rfc4648 => rfc4648Provider.Value;

    /// <summary>
    /// Gets Extended Hex provider.
    /// </summary>
    public static Encode32Provider ExtendedHex => extendedHexProvider.Value;

    /// <summary>
    /// Gets z-base-32 provider.
    /// </summary>
    public static Encode32Provider ZEncode32 => zEncode32Provider.Value;

    /// <summary>
    /// Gets Geohash provider.
    /// </summary>
    public static Encode32Provider Geohash => geohashProvider.Value;

    /// <summary>
    /// Gets FileCoin provider.
    /// </summary>
    public static Encode32Provider FileCoin => fileCoinProvider.Value;

    /// <summary>
    /// Gets Bitcoin provider.
    /// </summary>
    public static Encode58Provider Bitcoin => bitcoinProvider.Value;

    /// <summary>
    /// Gets Encode58 provider.
    /// </summary>
    public static Encode58Provider Ripple => rippleProvider.Value;

    /// <summary>
    /// Gets Flickr provider.
    /// </summary>
    public static Encode58Provider Flickr => flickrProvider.Value;

    /// <summary>
    /// Gets ZeroMQ Z85 Provider.
    /// </summary>
    public static Encode85Provider Z85 => z85.Value;

    /// <summary>
    /// Gets Adobe Ascii85 Provider (each character is directly produced by raw value + 33),
    /// also known as "btoa" encoding.
    /// </summary>
    public static Encode85Provider Ascii85 => ascii85.Value;
}