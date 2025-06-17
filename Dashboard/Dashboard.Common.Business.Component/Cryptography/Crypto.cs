using System.Security.Cryptography;
using System.Text;

namespace Dashboard.Common.Business.Component.Cryptography;

public class encryptionKey
{
    public static byte[] masterKey;
    public static byte[] sessionKey;
    public static byte[] enSessionKey;
    public static byte[] tmsSessionKey;
    public static byte[] enTMSSessionKey;

    //public static string sKey;
}

public class CryptorEngine
{
    public static string EncryptWithBase64(string clearData)
    {
        byte[] clearDataArray = Encoding.UTF8.GetBytes(clearData);
        byte[] initialVector = new byte[8];

        //Initial value
        initialVector[0] = 0x00;
        initialVector[1] = 0x00;
        initialVector[2] = 0x00;
        initialVector[3] = 0x00;
        initialVector[4] = 0x00;
        initialVector[5] = 0x00;
        initialVector[6] = 0x00;
        initialVector[7] = 0x00;


        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = encryptionKey.sessionKey;
        tdes.Mode = CipherMode.ECB;
        tdes.IV = Encoding.UTF8.GetBytes("12345678");
        //tdes.Padding = PaddingMode.None;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(clearDataArray, 0, clearDataArray.Length);
        tdes.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    public static byte[] DecryptWithBase64(string encryptData)
    {
        byte[] encryptDataArray = Convert.FromBase64String(encryptData);
        byte[] initialVector = new byte[8];

        //Initial value
        initialVector[0] = 0x00;
        initialVector[1] = 0x00;
        initialVector[2] = 0x00;
        initialVector[3] = 0x00;
        initialVector[4] = 0x00;
        initialVector[5] = 0x00;
        initialVector[6] = 0x00;
        initialVector[7] = 0x00;

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = encryptionKey.sessionKey;
        tdes.Mode = CipherMode.ECB;
        tdes.IV = Encoding.UTF8.GetBytes("12345678");
        //tdes.Padding = PaddingMode.None;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(encryptDataArray, 0, encryptDataArray.Length);

        tdes.Clear();
        return resultArray;
    }


    public static string EncryptWithBase64inByte(byte[] clearDataArray)
    {
        byte[] initialVector = new byte[8];

        //Initial value
        initialVector[0] = 0x00;
        initialVector[1] = 0x00;
        initialVector[2] = 0x00;
        initialVector[3] = 0x00;
        initialVector[4] = 0x00;
        initialVector[5] = 0x00;
        initialVector[6] = 0x00;
        initialVector[7] = 0x00;


        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = encryptionKey.sessionKey;
        tdes.Mode = CipherMode.ECB;
        tdes.IV = Encoding.UTF8.GetBytes("12345678");
        //tdes.Padding = PaddingMode.Zeros;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(clearDataArray, 0, clearDataArray.Length);
        tdes.Clear();
        return Convert.ToBase64String(resultArray);
    }

    public static string EncryptWithBase64tms(string clearData)
    {
        byte[] clearDataArray = Encoding.UTF8.GetBytes(clearData);
        byte[] initialVector = new byte[8];

        //Initial value
        initialVector[0] = 0x00;
        initialVector[1] = 0x00;
        initialVector[2] = 0x00;
        initialVector[3] = 0x00;
        initialVector[4] = 0x00;
        initialVector[5] = 0x00;
        initialVector[6] = 0x00;
        initialVector[7] = 0x00;


        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = encryptionKey.tmsSessionKey;
        tdes.Mode = CipherMode.ECB;
        tdes.IV = Encoding.UTF8.GetBytes("12345678");
        //tdes.Padding = PaddingMode.Zeros;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(clearDataArray, 0, clearDataArray.Length);
        tdes.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static byte[] DecryptWithBase64tms(string encryptData)
    {
        byte[] encryptDataArray = Convert.FromBase64String(encryptData);
        byte[] initialVector = new byte[8];

        //Initial value
        initialVector[0] = 0x00;
        initialVector[1] = 0x00;
        initialVector[2] = 0x00;
        initialVector[3] = 0x00;
        initialVector[4] = 0x00;
        initialVector[5] = 0x00;
        initialVector[6] = 0x00;
        initialVector[7] = 0x00;

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = encryptionKey.tmsSessionKey;
        tdes.Mode = CipherMode.ECB;
        tdes.IV = Encoding.UTF8.GetBytes("12345678");
        //tdes.Padding = PaddingMode.Zeros;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(encryptDataArray, 0, encryptDataArray.Length);

        tdes.Clear();
        return resultArray;
    }

    /* pkcs? */

    public static string pkcs7EncryptData(string Message)
    {
        byte[] Results;
        UTF8Encoding UTF8 = new UTF8Encoding();
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(encryptionKey.sessionKey);
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        byte[] DataToEncrypt = UTF8.GetBytes(Message);
        try
        {
            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return Convert.ToBase64String(Results);
    }

    public static string pkcs7DecryptString(string Message)
    {
        byte[] Results;
        UTF8Encoding UTF8 = new UTF8Encoding();
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(encryptionKey.sessionKey);
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        byte[] DataToDecrypt = Convert.FromBase64String(Message);
        try
        {
            ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return UTF8.GetString(Results);
    }

    /* HSM */

    public static byte[] EncryptHSMWithBase64(string clearData)
    {
        byte[] clearDataArray = Encoding.UTF8.GetBytes(clearData);
        byte[] initialVector = new byte[8];

        //Initial value
        initialVector[0] = 0x00;
        initialVector[1] = 0x00;
        initialVector[2] = 0x00;
        initialVector[3] = 0x00;
        initialVector[4] = 0x00;
        initialVector[5] = 0x00;
        initialVector[6] = 0x00;
        initialVector[7] = 0x00;


        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = encryptionKey.sessionKey;
        tdes.Mode = CipherMode.ECB;
        //tdes.IV = UTF8Encoding.UTF8.GetBytes("12345678");
        tdes.Padding = PaddingMode.None;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(clearDataArray, 0, clearDataArray.Length);
        tdes.Clear();
        return resultArray;
    }

    public static byte[] DecryptHSMWithBase64(string encryptData)
    {
        byte[] encryptDataArray = Convert.FromBase64String(encryptData);
        byte[] initialVector = new byte[8];

        //Initial value
        initialVector[0] = 0x00;
        initialVector[1] = 0x00;
        initialVector[2] = 0x00;
        initialVector[3] = 0x00;
        initialVector[4] = 0x00;
        initialVector[5] = 0x00;
        initialVector[6] = 0x00;
        initialVector[7] = 0x00;

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = encryptionKey.sessionKey;
        tdes.Mode = CipherMode.ECB;
        //tdes.IV = UTF8Encoding.UTF8.GetBytes("12345678");
        tdes.Padding = PaddingMode.None;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(encryptDataArray, 0, encryptDataArray.Length);

        tdes.Clear();
        return resultArray;
    }


    /*TW*/
    public static byte[] zzzEncryption(byte[] Deskey, byte[] plainText)
    {
        SymmetricAlgorithm TdesAlg = new TripleDESCryptoServiceProvider();
        TdesAlg.Key = Deskey;
        TdesAlg.Mode = CipherMode.ECB;
        TdesAlg.Padding = PaddingMode.None;
        //TdesAlg.IV = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        ICryptoTransform ict = TdesAlg.CreateEncryptor(TdesAlg.Key, TdesAlg.IV);
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, ict, CryptoStreamMode.Write);
        cStream.Write(plainText, 0, plainText.Length);
        cStream.FlushFinalBlock();
        cStream.Close();
        return mStream.ToArray();
    }
}
