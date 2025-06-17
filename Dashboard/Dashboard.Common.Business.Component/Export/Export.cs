using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using Dashboard.Common.Business.Component;

namespace Dashboard.Common.Business.Component.Export;
public class Export : Page
{
    /// <summary>
    /// Exports the data to excel format using open office method.
    /// Please ensure to delete the file that is being passed in
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="filename"></param>
    /// <param name="MyTables"></param>
    /// 


    public void ExPdfFile(string filePath, string fileName, List<DataTable> myDataTableList, List<Paragraph> GridHeader)
    {
        try
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
            pdfDoc.Open();

            Paragraph p = new Paragraph();
            p.Alignment = Element.ALIGN_LEFT;

            if (myDataTableList.Count != GridHeader.Count)
            {
                Logger.LogToFile("Export_ExPdfFile.log", ',', "Failed to export the pdf,the datatable and paragraph is not match!");
            }

            for (int a = 0; a < myDataTableList.Count; a++)
            {
                pdfDoc.Add(GridHeader[a]);
                pdfDoc.Add(new Chunk("\n"));
                DataTable dt = myDataTableList[a];

                if (dt != null)
                {
                    //Craete instance of the pdf table and set the number of column in that table  
                    PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                    PdfPCell PdfPCell = null;
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(column.ColumnName, FontFactory.GetFont("Verdana", 10))));
                        PdfTable.AddCell(PdfPCell);
                    }


                    for (int rows = 0; rows < dt.Rows.Count; rows++)
                    {
                        for (int column = 0; column < dt.Columns.Count; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), FontFactory.GetFont("Verdana", 8))));
                            PdfTable.AddCell(PdfPCell);
                        }
                    }
                    //PdfTable.SpacingBefore = 15f; // Give some space after the text or it may overlap the table            

                    Paragraph newpara = new Paragraph();
                    newpara.Add(new Chunk("\n\n"));
                    // add pdf table to the document

                    pdfDoc.Add(PdfTable);
                    pdfDoc.Add(newpara);

                }

            }
            pdfDoc.Close();
            System.Web.HttpContext.Current.Response.ContentType = "application/pdf";


            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "");
            System.Web.HttpContext.Current.Response.Write(pdfDoc);
            System.Web.HttpContext.Current.Response.Flush();
            //Response.End(); 
            System.Web.HttpContext.Current.Response.OutputStream.Close();


            //System.Web.HttpContext.Current.Response.Clear();
            //System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "");
            //System.Web.HttpContext.Current.Response.TransmitFile(filePath + fileName);
            //System.Web.HttpContext.Current.Response.OutputStream.Close();

            //HttpContext.Current.ApplicationInstance.CompleteRequest();  
        }


        catch (Exception ex)
        {
            if (!ex.Message.Contains("Thread was being aborted."))
                Logger.LogToFile("RptRegional_ExportPDF.log", ex);

        }
    }


    public void OpenOfficeExcel(string filepath, string filename, List<ArrayList> Header, List<System.Data.DataTable> MyTables)
    {
        try
        {
            System.IO.FileInfo newFile = new System.IO.FileInfo("D:\\" + filename);



            ExcelPackage pck = new ExcelPackage(newFile);



            for (int x = 0; x < MyTables.Count; x++)
            {
                System.Data.DataTable MyTable = MyTables[x];
                ArrayList MyHeader = Header[x];

                int RowSpan = 1;
                OfficeOpenXml.ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add(MyTable.TableName);

                for (int Num = 1; Num < MyHeader.Count + 1; Num++)
                {
                    worksheet.Cells[Num, 1].Value = MyHeader[Num - 1];
                    RowSpan++;
                }


                for (int a = RowSpan; a < MyTable.Rows.Count + RowSpan; a++)
                {
                    if (a == RowSpan)
                    {
                        //set the columnnames
                        for (int i = 0; i < MyTable.Columns.Count; i++)
                        {
                            worksheet.Cells[a + 1, i + 1].Value = MyTable.Columns[i].ColumnName;

                        }
                    }
                    else
                    {
                        for (int b = 0; b < MyTable.Columns.Count; b++)
                        {
                            worksheet.Cells[a + 1, b + 1].Value = MyTable.Rows[a - RowSpan][b].ToString();
                        }
                    }
                }
            }

            pck.Save();
            pck.Dispose();
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + "");
            System.Web.HttpContext.Current.Response.TransmitFile("D:\\" + filename);
            System.Web.HttpContext.Current.Response.OutputStream.Close();
            //File.Delete();
        }
        catch (Exception ex)
        {
            Logger.LogToFile("OpenOfficeExcel.log", ex);
        }
    }
}
