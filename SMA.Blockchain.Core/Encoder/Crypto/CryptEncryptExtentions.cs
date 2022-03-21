namespace SMA.Blockchain.Core.Encoder.Crypto;
using System; 
using System.Security.Cryptography;
using System.Text; 

public static class CryptEncryptExtentions
{
    private const string defaultKey = "/&!~Ss58*";
    private static string funcObfuscation(string strInput)
    {
        strInput = strInput + "789$@958";
        strInput = strInput + strInput.Substring(3);
        strInput = "+%Hh&1" + strInput;
        return strInput;
    }
    /// <summary>
    /// Encrypt message on way as MD5
    /// </summary>
    /// <param name="message">clear text</param>
    /// <returns>base64 hash</returns>
    public static string MD5GenratorAsBase64(this string message)
    {

        UnicodeEncoding Ue = new UnicodeEncoding();
        byte[] ByteSourceText = Ue.GetBytes(funcObfuscation(message));
        byte[] ByteHash = MD5.Create().ComputeHash(ByteSourceText);
        return Convert.ToBase64String(ByteHash);

    }
    /// <summary>
    /// Encrypt message tow way as TripleDES as CBC
    /// </summary>
    /// <param name="message">clear text</param>
    /// <returns>base64 hash</returns>
    public static string EncryptCBC(this string message, string key = defaultKey)
    {
        string encryptedText = "";
        MD5 md5 = MD5.Create();
        TripleDES des = TripleDES.Create();
        des.KeySize = 128;
        des.Mode = CipherMode.CBC;
        des.Padding = PaddingMode.PKCS7;

        byte[] md5Bytes = MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(key));
        byte[] ivBytes = new byte[8];

        des.Key = md5Bytes;
        des.IV = ivBytes;

        byte[] clearBytes = Encoding.Unicode.GetBytes(message);

        ICryptoTransform ct = des.CreateEncryptor();
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
            }
            encryptedText = Convert.ToBase64String(ms.ToArray());
        }
        return encryptedText;
    }
    /// <summary>
    /// Decrypt message tow way as TripleDES as CBC
    /// </summary>
    /// <param name="message">clear text</param>
    /// <returns>base64 hash</returns>
    public static string DecryptCBC(this string message, string key = defaultKey)
    {
        byte[] clearBytes = Convert.FromBase64String(message);
        byte[] md5Bytes = MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(key));
        string encryptedText = "";
        TripleDES des = TripleDES.Create();
        des.KeySize = 128;
        des.Mode = CipherMode.CBC;
        des.Padding = PaddingMode.PKCS7;
        byte[] ivBytes = new byte[8];
        des.Key = md5Bytes;
        des.IV = ivBytes;
        ICryptoTransform ct = des.CreateDecryptor();
        byte[] resultArray = ct.TransformFinalBlock(clearBytes, 0, clearBytes.Length);
        encryptedText = Encoding.Unicode.GetString(resultArray);
        return encryptedText;
    }
    /// <summary>
    /// Encrypt message tow way as TripleDES as ECB
    /// </summary>
    /// <param name="message">clear text</param>
    /// <returns>base64 hash</returns>
    public static string EncryptECB(this string message, string key = defaultKey)
    {
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(message);
        byte[] keyArray = MD5.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

        TripleDES tdes = TripleDES.Create();

        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        tdes.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    /// <summary>
    /// Encrypt message tow way as TripleDES as ECB
    /// </summary>
    /// <param name="message">clear text</param>
    /// <returns>base64 hash</returns>
    public static string DecryptECB(this string message, string key = defaultKey)
    {  
        byte[] toEncryptArray = Convert.FromBase64String(message); 
        byte[] keyArray = MD5.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
         

        TripleDES  tdes = TripleDES.Create();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        tdes.Clear();
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    /// <summary>
    /// /]
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ToHex(this byte[] data)
    {
        return BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] FromHex(this string data)
    {
        int NumberChars = data.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
        return bytes;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static byte[] HexToBytes(this string hex)
    { 
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GenHash(this string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] hash = SHA256.Create().ComputeHash(bytes);
        return Convert.ToHexString(hash).ToLower();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string Sha1(this byte[] bytes)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            // ComputeHash - returns byte array  
            byte[] computedBytes = sha1.ComputeHash(bytes);

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();

            Array.ForEach(computedBytes, (Byte Byte) => builder.Append(Byte.ToString("x2")));

            return builder.ToString();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Sha1(this string data)
    {
        return Encoding.UTF8.GetBytes(data).Sha1();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string Sha256(this byte[] bytes)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] computedBytes = sha256Hash.ComputeHash(bytes);

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();

            Array.ForEach(computedBytes, (Byte Byte) => builder.Append(Byte.ToString("x2")));

            return builder.ToString();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Sha256(this string data)
    {
        return Encoding.UTF8.GetBytes(data).Sha256();
    }
}

