using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Dashboard.Common.Business.Component;
/// <summary>
/// Common class to generate all sorts of things, encryption, msgid, emails and etc
/// </summary>
/// 
public class Generate
{
    #region To generate a unique ID
    public static string GenerateUniqueID()
    {
        string ret = string.Empty;
        byte[] buffer = Guid.NewGuid().ToByteArray();
        ret = BitConverter.ToInt64(buffer, 0).ToString();
        return ret;
    }
    #endregion

    #region To Check if string is an integer
    public static bool isNo(string strNumber)
    {
        Regex objNotNumberPattern = new Regex("[^0-9.-]");
        Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
        Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
        string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
        string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
        Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
        return !objNotNumberPattern.IsMatch(strNumber) && !objTwoDotPattern.IsMatch(strNumber) && !objTwoMinusPattern.IsMatch(strNumber) && objNumberPattern.IsMatch(strNumber);
    }

    #endregion
    #region To send an email without attachment
    public static bool SendEmail(string SendTo, string CC, string From, string subject, string body, string password, string host, bool ssl, int port = 587)
    {
        bool ret = true;
        try
        {
            using (var mail = new MailMessage())
            {
                mail.To.Add(SendTo);
                if (!string.IsNullOrEmpty(CC))
                    mail.CC.Add(CC);
                mail.From = new MailAddress(From);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                using (var smtpClient = new SmtpClient(host, port))
                {
                    smtpClient.Credentials = new NetworkCredential(From, password);
                    smtpClient.EnableSsl = ssl;
                    smtpClient.Send(mail);
                    Console.WriteLine($"{DateTime.Now}: Mail sent");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile("SendEmailExecption.log", ex);
            ret = false;
        }
        return ret;
    }
    #endregion
    #region To send an email with attachment
    public static void SendEmailWithAttachment(string SendTo, string CC, string From, string subject, string body, string password, string host, string FullAttachmentPath)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(SendTo);
            mail.CC.Add(CC);
            mail.From = new MailAddress(From);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Attachments.Add(new Attachment(FullAttachmentPath));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = host;
            smtp.Credentials = new System.Net.NetworkCredential(From, password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
            mail.Dispose();
        }
        catch (Exception ex)
        {
            Logger.LogToFile("SendMailAttachmentException.log", ex);
        }
    }
    #endregion
    public static bool IsValidEmail(string email)
    {
        try
        {
            var mail = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}