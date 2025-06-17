using System.Security.Cryptography;
using System.Text;

public static class Crypt
{
    // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
    // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
    // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
    private const string initVector = "tu89geji340t89u2";
    private const string initPassPhase = "Emerico";
    //private const string initPassPhase = "3M3r1C0p4@53";

    // This constant is used to determine the keysize of the encryption algorithm.
    private const int keysize = 256;

    public static string Encrypt(string plainText)
    {
        byte[] clearDataArray = UTF8Encoding.UTF8.GetBytes(plainText);
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


        using TripleDES tdes = TripleDES.Create();
        tdes.Key = Convert.FromBase64String("EjRWeBI0VniHZUMhh2VDIQ==");
        tdes.Mode = CipherMode.ECB;
        tdes.IV = UTF8Encoding.UTF8.GetBytes("12345678");
        //tdes.Padding = PaddingMode.None;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(clearDataArray, 0, clearDataArray.Length);
        tdes.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string cipherText)
    {
        byte[] encryptDataArray = Convert.FromBase64String(cipherText);
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

        using TripleDES tdes = TripleDES.Create();
        tdes.Key = Convert.FromBase64String("EjRWeBI0VniHZUMhh2VDIQ==");
        tdes.Mode = CipherMode.ECB;
        tdes.IV = UTF8Encoding.UTF8.GetBytes("12345678");
        //tdes.Padding = PaddingMode.None;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(encryptDataArray, 0, encryptDataArray.Length);

        tdes.Clear();
        //return resultArray;
        return Encoding.UTF8.GetString(resultArray);
    }

    public static string GenerateSesionKey()
    {
        return TDESEncrypt(Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16), initVector);
    }

    public static string TDESEncrypt(string toEncrypt, string key)
    {
        byte[] keyArray;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
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

        keyArray = UTF8Encoding.UTF8.GetBytes(key);

        using TripleDES tdes = TripleDES.Create();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.IV = initialVector;
        tdes.Padding = PaddingMode.Zeros;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        tdes.Clear();
        String keyss = Convert.ToBase64String(resultArray, 0, resultArray.Length);
        return keyss;
    }

    public static string TDESDecrypt(string cipherString, string key)
    {
        byte[] keyArray;
        byte[] toEncryptArray = Convert.FromBase64String(cipherString);
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


        keyArray = UTF8Encoding.UTF8.GetBytes(key);

        using TripleDES tdes = TripleDES.Create();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.IV = initialVector;
        tdes.Padding = PaddingMode.Zeros;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        tdes.Clear();
        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    //Console.WriteLine("Please enter a password to use:");
    //string password = Console.ReadLine();
    //Console.WriteLine("Please enter a string to encrypt:");
    //string plaintext = Console.ReadLine();
    //Console.WriteLine("");

    //Console.WriteLine("Your encrypted string is:");
    //string encryptedstring = StringCipher.Encrypt(plaintext, password);
    //Console.WriteLine(encryptedstring);
    //Console.WriteLine("");

    //Console.WriteLine("Your decrypted string is:");
    //string decryptedstring = StringCipher.Decrypt(encryptedstring, password);
    //Console.WriteLine(decryptedstring);
    //Console.WriteLine("");

    //Console.ReadLine();
}
