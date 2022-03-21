namespace SMA.Blockchain.Core.Tests.Encoder;
 
using SMA.Blockchain.Core.Encoder;
using SMA.Blockchain.Core.Encoder.Crypto;
using System.Collections.Generic;
using System.Linq;

public static class DemoTests
{
    private static readonly object[][] testCasesEncoder16 = new[]
{                                                                                   // LowerCase        // UpperCase        // ModHex
            new object[] { new byte[] { },                                                  "",                 "",                 ""                  },
            new object[] { new byte[] { 0xAB },                                             "ab",               "AB",               "ln"                },
            new object[] { new byte[] { 0x00, 0x01, 0x02, 0x03 },                           "00010203",         "00010203",         "cccbcdce"          },
            new object[] { new byte[] { 0x10, 0x11, 0x12, 0x13 },                           "10111213",         "10111213",         "bcbbbdbe"          },
            new object[] { new byte[] { 0xAB, 0xCD, 0xEF, 0xBA },                           "abcdefba",         "ABCDEFBA",         "lnrtuvnl"          },
            new object[] { new byte[] { 0xAB, 0xCD, 0xEF, 0xBA, 0xAB, 0xCD, 0xEF, 0xBA },   "abcdefbaabcdefba", "ABCDEFBAABCDEFBA", "lnrtuvnllnrtuvnl"  },
        };

    private static readonly Encode16[] encoders = new[]
{
            Encoders.LowerCase,
            Encoders.UpperCase,
            Encoders.ModHex
        };


    public static IEnumerable<object[]> encodersTestData
    {
        get
        {
            foreach (var pair in encoders.Select((encoder, index) => (encoder, index)))
            {
                foreach (var testRow in testCasesEncoder16)
                {
                    var testValue = testRow[pair.index + 1];
                    yield return new object[] { pair.encoder, testRow[0], testValue };
                }
            }
        }
    }

    public static IEnumerable<object[]> encodersTestDataInvalidLength
    {
        get
        {
            var input = new string[] { "123", "12345" };
            foreach (var pair in encoders.Select((encoder, index) => (encoder, index)))
            {
                foreach (var testRow in input)
                { 
                    yield return new object[] { pair.encoder, testRow   };
                }
            }
        }
    }

    public static IEnumerable<object[]> encodersTypeTestData
    {
        get
        {
            foreach (var pair in encoders.Select((encoder, index) => (encoder, index)))
            {
                yield return new object[] { pair.encoder };
            }
        }
    }

    public static IEnumerable<object[]> Crockford
    {
        get
        {
            return new[]{
                new object[] { "", "", false },
            new object[] { "f", "CR", false },
            new object[] { "f", "CR======", true },
            new object[] { "fo", "CSQG", false },
            new object[] { "fo", "CSQG====", true },
            new object[] { "foo", "CSQPY", false },
            new object[] { "foo", "CSQPY===", true },
            new object[] { "foob", "CSQPYRG", false },
            new object[] { "foob", "CSQPYRG=", true },
            new object[] { "fooba", "CSQPYRK1", false },
            new object[] { "fooba", "CSQPYRK1", true },
            new object[] { "foobar", "CSQPYRK1E8", false },
            new object[] { "foobar", "CSQPYRK1E8======", true },
            new object[] { "123456789012345678901234567890123456789", "64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE8", false },
            new object[] { "123456789012345678901234567890123456789", "64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE8=", true }
        };
        }
    }

    public static readonly IEnumerable<string[]> testDataExtendedHex = new[]
    {
            new[] { "", "" },
            new[] { "f", "CO======" },
            new[] { "fo", "CPNG====" },
            new[] { "foo", "CPNMU===" },
            new[] { "foob", "CPNMUOG=" },
            new[] { "fooba", "CPNMUOJ1" },
            new[] { "foobar", "CPNMUOJ1E8======" },
            new[] { "1234567890123456789012345678901234567890", "64P36D1L6ORJGE9G64P36D1L6ORJGE9G64P36D1L6ORJGE9G64P36D1L6ORJGE9G" },
     };

    public static readonly IEnumerable<string[]> testDataFileCoin = new[]
        {
            new[] { "", "" },
            new[] {"f", "my======" },
            new[] {"fo", "mzxq====" },
            new[] {"foo", "mzxw6===" },
            new[] {"foob", "mzxw6yq=" },
            new[] {"fooba", "mzxw6ytb" },
            new[] {"foobar", "mzxw6ytboi======" },
            new[] {"foobar1", "mzxw6ytboiyq====" },
            new[] {"foobar12", "mzxw6ytboiyte===" },
            new[] {"foobar123", "mzxw6ytboiytemy=" },
            new[] {"1234567890123456789012345678901234567890", "gezdgnbvgy3tqojqgezdgnbvgy3tqojqgezdgnbvgy3tqojqgezdgnbvgy3tqojq" }
        };
    public static readonly object[] byteTestDataFileCoin = new object[]
   {
            new object[] { new byte[] { 245, 202, 80, 149, 94, 201, 222, 50, 17, 198, 138, 104, 32, 183, 131, 33, 139, 208, 203, 211, 197, 191, 92, 194 }, "6xffbfk6zhpdeeogrjucbn4degf5bs6tyw7vzqq", false },
   };

    public static readonly string[][] testDataRFC = new[]
    {
            new[] { "", "" },
            new[] {"f", "MY======" },
            new[] {"fo", "MZXQ====" },
            new[] {"foo", "MZXW6===" },
            new[] {"foob", "MZXW6YQ=" },
            new[] {"fooba", "MZXW6YTB" },
            new[] {"foobar", "MZXW6YTBOI======" },
            new[] {"foobar1", "MZXW6YTBOIYQ====" },
            new[] {"foobar12", "MZXW6YTBOIYTE===" },
            new[] {"foobar123", "MZXW6YTBOIYTEMY=" },
            new[] {"1234567890123456789012345678901234567890", "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ" },
        };

    public static readonly string[][] testDataZEncoder32 = new[]
        {
            new[] { "", "" },
            new[] { "dCode z-base-32", "ctbs63dfrb7n4aubqp114c31" },
            new[] { "Never did sun more beautifully steep",
                "j31zc3m1rb1g13byqp4shedpp73gkednciozk7djc34sa5d3rb3ze3mfqy" },
        };

    public static readonly IEnumerable<object[]> bitcoinTestData = new List<object[]>
        {
            new object[]{"0001", "12"},
            new object[]{"0000010203", "11Ldp"},
            new object[]{"009C1CA2CBA6422D3988C735BB82B5C880B0441856B9B0910F", "1FESiat4YpNeoYhW3Lp7sW1T6WydcW7vcE"},
            new object[]{"000860C220EBBAF591D40F51994C4E2D9C9D88168C33E761F6", "1mJKRNca45GU2JQuHZqZjHFNktaqAs7gh"},
            new object[]{"00313E1F905554E7AE2580CD36F86D0C8088382C9E1951C44D010203", "17f1hgANcLE5bQhAGRgnBaLTTs23rK4VGVKuFQ"},
            new object[]{"0000000000", "11111"},
            new object[]{"1111111111", "2vgLdhi"},
            new object[]{"FFEEDDCCBBAA", "3CSwN61PP"},
            new object[]{"00", "1"},
            new object[]{"21", "a"},
            new object[]{"000102030405060708090A0B0C0D0E0F000102030405060708090A0B0C0D0E0F", "1thX6LZfHDZZKUs92febWaf4WJZnsKRiVwJusXxB7L"},
            new object[]{"0000000000000000000000000000000000000000000000000000", "11111111111111111111111111"},
        };

    public static readonly IEnumerable<object[]> flickrTestData = new List<object[]>
      {
            new object[]{"0000010203", "11kCP"},
            new object[]{"009C1CA2CBA6422D3988C735BB82B5C880B0441856B9B0910F", "1ferHzT4xPnDNxGv3kP7Sv1s6vYCBv7VBe"},
            new object[]{"000860C220EBBAF591D40F51994C4E2D9C9D88168C33E761F6", "1LijqnBz45gt2ipUhyQyJhfnKTzQaS7FG"},
            new object[]{"00313E1F905554E7AE2580CD36F86D0C8088382C9E1951C44D010203", "17E1GFanBke5ApGagqFMbzkssS23Rj4ugujUfp"},
            new object[]{"0000000000", "11111"},
            new object[]{"1111111111", "2VFkCGH"},
            new object[]{"FFEEDDCCBBAA", "3crWn61oo"},
            new object[]{"00", "1"},
            new object[]{"21", "z"},
      };

    public static readonly IEnumerable<object[]> rippleTestData = new List<object[]>
      {
            new object[]{"0000010203", "rrLdF"},
            new object[]{"009C1CA2CBA6422D3988C735BB82B5C880B0441856B9B0910F", "rENS52thYF4eoY6WsLFf1WrTaWydcWfvcN"},
            new object[]{"000860C220EBBAF591D40F51994C4E2D9C9D88168C33E761F6", "rmJKR4c2hnG7pJQuHZqZjHE4kt2qw1fg6"},
            new object[]{"00313E1F905554E7AE2580CD36F86D0C8088382C9E1951C44D010203", "rfCr6gw4cLNnbQ6wGRg8B2LTT1psiKhVGVKuEQ"},
            new object[]{"0000000000", "rrrrr"},
            new object[]{"1111111111", "pvgLd65"},
            new object[]{"FFEEDDCCBBAA", "sUSA4arPP"},
            new object[]{"00", "r"},
            new object[]{"21", "2"},
      };

    public static readonly IEnumerable<object[]> testVectors = new List<object[]>
        {
            new object[] { new byte[] { }, "" },
            new object[] { new byte[] { 0, 0, 0, 0 }, "z" },
            new object[] { new byte[] { 0x20, 0x20, 0x20, 0x20 }, "y" },
            new object[] { new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45 }, "5sdq,70" },
            new object[] { new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B }, "L/669[9<6." },
            new object[] { new byte[] { 0x11, 0x22, 0x33 }, "&L'\"" },
            new object[] { new byte[] { 77, 97, 110, 32 }, "9jqo^" },
        };

    public static readonly IEnumerable<object[]> testVectorsZ85 = new[]
      {
            new object[]{new byte[] { }, ""},
            new object[]{new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B }, "HelloWorld"},
            new object[]{new byte[] { 0x11 }, "5D"},
            new object[]{new byte[] { 0x11, 0x22 }, "5H4"},
            new object[]{new byte[] { 0x11, 0x22, 0x33 }, "5H61"},
            new object[]{new byte[] { 0x11, 0x22, 0x33, 0x44 }, "5H620"},
            new object[]{new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55 }, "5H620rr"},
            new object[]{new byte[] { 0x00, 0x00, 0x00, 0x00 }, "00000"},
            new object[]{new byte[] { 0x20, 0x20, 0x20, 0x20 }, "arR^H"},
        };

}
