namespace SMA.Blockchain.Core.Encoder.Crypto;
using System.Text.Json.Serialization; 
public class KeyPair
{
    public struct Signature
    {
        public readonly byte[] PublicKey;
        public readonly string Value;

        [JsonConstructor]
        public Signature(byte[] PublicKey, string Value)
        {
            this.PublicKey = PublicKey;
            this.Value = Value;
        }

        public bool Verify(string Message)
        {
            return new KeyPair(PublicKey).Verify(this, Message);
        }
    }

    public readonly byte[] PublicKey;
    public readonly byte[] PrivateKey;

    public KeyPair(byte[] PublicKey = null, byte[] PrivateKey = null)
    {
        this.PublicKey = PublicKey;
        this.PrivateKey = PrivateKey;
    }

    // Delegate/Helper method
    public static KeyPair Create()
    {
        return CryptoECDsa.GenerateKeyPair();
    }

    public byte[] AsData()
    {
        byte[] data = new byte[96];

        Array.Copy(PublicKey, data, 64);
        Array.Copy(PrivateKey, 0, data, 64, 32);

        return data;
    }

    public bool HasPublicKey()
    {
        return PublicKey != null;
    }

    public (byte[] X, byte[] Y) GetPublicKey()
    {
        return (PublicKey.Take(PublicKey.Length / 2).ToArray(), PublicKey.TakeLast(PublicKey.Length / 2).ToArray());
    }

    public bool HasPrivateKey()
    {
        return PrivateKey != null;
    }

    public string GetAddress()
    {
        if (!HasPublicKey())
        {
            throw new MissingFieldException("Missing public key.");
        }
        return "mtl" + PublicKey.Sha1();
    }

    public bool Verify(Signature Signature, string Message)
    {
        return CryptoECDsa.Verify(this, Signature.Value, Message);
    }

    public Signature Sign(string Message)
    {
        return new Signature(PublicKey, CryptoECDsa.Sign(this, Message));
    }
}
