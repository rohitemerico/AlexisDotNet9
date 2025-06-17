using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using Dashboard.Common.Business.Component;


namespace Dashboard.Common.Business.Component.Export;

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
    public static bool SendEmail(string SendTo, string CC, string From, string subject, string body, string password, string host, bool ssl)
    {
        bool ret = true;
        try
        {
            using (MailMessage myMail = new MailMessage())
            {
                //  read this from - system.net > mailSettings > smtp > network  || web.config
                using (SmtpClient sClient = new SmtpClient())
                {
                    sClient.Credentials = sClient.Credentials;//new System.Net.NetworkCredential(From, password);
                    myMail.To.Add(SendTo);
                    myMail.From = new MailAddress(((System.Net.NetworkCredential)sClient.Credentials).UserName);
                    myMail.Subject = subject;
                    myMail.Body = body;
                    myMail.IsBodyHtml = true;

                    sClient.EnableSsl = sClient.EnableSsl;//ssl;

                    sClient.Send(myMail);
                    Console.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Mail sent");
                }
            }
            //MailMessage mail = new MailMessage();
            //mail.To.Add(SendTo);
            //if (!string.IsNullOrEmpty(CC))
            //    mail.CC.Add(CC);
            //mail.From = new MailAddress(From);
            //mail.Subject = subject;
            //mail.Body = body;
            //mail.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = host;
            //smtp.Credentials = new System.Net.NetworkCredential(From, password);
            //smtp.EnableSsl = ssl;
            //smtp.Send(mail);
            //mail.Dispose();
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
    #region Pop up message box
    public static System.Web.UI.WebControls.Label ShowMessageBox(string msg, string URL)
    {
        System.Web.UI.WebControls.Label lbl = new System.Web.UI.WebControls.Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "'); window.location = '" + URL + "'; </script>";
        //Page.Controls.Add(lbl);
        return lbl;
    }
    public static System.Web.UI.WebControls.Label ShowMessageBox(string msg)
    {
        System.Web.UI.WebControls.Label lbl = new System.Web.UI.WebControls.Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        return lbl;
        //Page.Controls.Add(lbl);
    }
    #endregion

    /// <summary>
    /// Saves the excel file base on the datasets name for sheet name
    /// Provide the full file path example, C:\Excelfile\myexcel.xlsx
    /// </summary>
    /// <param name="dataSets"></param>
    /// <param name="FullFileNamePath"></param>
    public static void DataSetsToExcel(List<DataSet> dataSets, string FullFileNamePath)
    {
        Microsoft.Office.Interop.Excel.Application xlApp =
                  new Microsoft.Office.Interop.Excel.Application();
        Workbook xlWorkbook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        Sheets xlSheets = null;
        Worksheet xlWorksheet = null;

        foreach (DataSet dataSet in dataSets)
        {
            System.Data.DataTable dataTable = dataSet.Tables[0];
            int rowNo = dataTable.Rows.Count;
            int columnNo = dataTable.Columns.Count;
            int colIndex = 0;

            //Create Excel Sheets
            xlSheets = xlWorkbook.Sheets;
            xlWorksheet = (Worksheet)xlSheets.Add(xlSheets[1],
                           Type.Missing, Type.Missing, Type.Missing);
            xlWorksheet.Name = dataSet.DataSetName;

            //Generate Field Names
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                colIndex++;
                xlApp.Cells[1, colIndex] = dataColumn.ColumnName;
            }

            object[,] objData = new object[rowNo, columnNo];

            //Convert DataSet to Cell Data
            for (int row = 0; row < rowNo; row++)
            {
                for (int col = 0; col < columnNo; col++)
                {
                    objData[row, col] = dataTable.Rows[row][col];
                }
            }

            //Add the Data
            Microsoft.Office.Interop.Excel.Range range = xlWorksheet.Range[xlApp.Cells[2, 1], xlApp.Cells[rowNo + 1, columnNo]];
            range.Value2 = objData;

            //Format Data Type of Columns 
            colIndex = 0;
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                colIndex++;
                string format = "@";
                switch (dataColumn.DataType.Name)
                {
                    case "Boolean":
                        break;
                    case "Byte":
                        break;
                    case "Char":
                        break;
                    case "DateTime":
                        format = "dd/mm/yyyy";
                        break;
                    case "Decimal":
                        format = "$* #,##0.00;[Red]-$* #,##0.00";
                        break;
                    case "Double":
                        break;
                    case "Int16":
                        format = "0";
                        break;
                    case "Int32":
                        format = "0";
                        break;
                    case "Int64":
                        format = "0";
                        break;
                    case "SByte":
                        break;
                    case "Single":
                        break;
                    case "TimeSpan":
                        break;
                    case "UInt16":
                        break;
                    case "UInt32":
                        break;
                    case "UInt64":
                        break;
                    default: //String
                        break;
                }
                //Format the Column accodring to Data Type
                xlWorksheet.Range[xlApp.Cells[2, colIndex],
                      xlApp.Cells[rowNo + 1, colIndex]].NumberFormat = format;
            }
        }

        //Remove the Default Worksheet
        ((Worksheet)xlApp.ActiveWorkbook.Sheets[xlApp.ActiveWorkbook.Sheets.Count]).Delete();

        //Save
        xlWorkbook.SaveAs(FullFileNamePath,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            XlSaveAsAccessMode.xlNoChange,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value);

        xlWorkbook.Close();
        xlApp.Quit();
        GC.Collect();
    }


}


