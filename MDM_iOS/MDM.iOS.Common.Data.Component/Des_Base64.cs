using System.Security.Cryptography;
using System.Text;
using Dashboard.Common.Business.Component;

namespace MDM.iOS.Common.Data.Component;

/// <summary>
/// This class is used to convert bytes to a base 64 string. 
/// </summary>
public class Des_Base64
{
    public static string ConvertByteToBase64String(string req)
    {
        string ret = string.Empty;
        try
        {
            Byte[] ByteData;
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                ByteData = hash.ComputeHash(enc.GetBytes(req));

                ret = Convert.ToBase64String(ByteData);

            }

        }
        catch (Exception ex)
        {
            ret = @"ERROR";
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
        }

        return ret;
    }
}
