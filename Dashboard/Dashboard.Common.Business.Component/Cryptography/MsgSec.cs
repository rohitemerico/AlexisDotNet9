using System.Text;

namespace Dashboard.Common.Business.Component.Cryptography;

public static class MsgSec
{
    public static string MasterKey = "EjRWeBI0VniHZUMhh2VDIQ==";

    public static string DecryptString(string encString)
    {
        if (encString == null || encString == "")
        {
            return "";
        }
        else
        {
            encryptionKey.masterKey = Convert.FromBase64String(MasterKey);
            encryptionKey.sessionKey = encryptionKey.masterKey;
            byte[] bytedata = CryptorEngine.DecryptWithBase64(encString);
            return Encoding.UTF8.GetString(bytedata);
        }
    }

    public static string EncryptString(string clearString)
    {
        if (clearString == null || clearString == "")
        {
            return "";
        }
        else
        {
            encryptionKey.masterKey = Convert.FromBase64String(MasterKey);
            encryptionKey.sessionKey = encryptionKey.masterKey;

            return CryptorEngine.EncryptWithBase64(clearString);
        }
    }
}
