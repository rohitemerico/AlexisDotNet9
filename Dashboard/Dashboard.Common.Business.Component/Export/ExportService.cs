using System.Collections;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Dashboard.Common.Business.Component.Export;

public class ExportService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExportService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ExportPdfAsync(string fileName, List<DataTable> myDataTableList, List<Paragraph> gridHeader)
    {
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                if (myDataTableList.Count != gridHeader.Count)
                {
                    // Log mismatch error
                }

                for (int a = 0; a < myDataTableList.Count; a++)
                {
                    pdfDoc.Add(gridHeader[a]);
                    pdfDoc.Add(new Chunk("\n"));
                    DataTable dt = myDataTableList[a];

                    if (dt != null)
                    {
                        PdfPTable pdfTable = new PdfPTable(dt.Columns.Count);

                        foreach (DataColumn column in dt.Columns)
                        {
                            pdfTable.AddCell(new PdfPCell(new Phrase(column.ColumnName, FontFactory.GetFont("Verdana", 10))));
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            foreach (var item in row.ItemArray)
                            {
                                pdfTable.AddCell(new PdfPCell(new Phrase(item.ToString(), FontFactory.GetFont("Verdana", 8))));
                            }
                        }

                        pdfDoc.Add(pdfTable);
                        pdfDoc.Add(new Paragraph("\n\n"));
                    }
                }

                pdfDoc.Close();

                var response = _httpContextAccessor.HttpContext.Response;
                response.ContentType = "application/pdf";
                response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
                response.Body.Write(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
        catch (Exception ex)
        {
            // Log exception
        }
    }

    public async Task ExportExcelAsync(string fileName, List<ArrayList> headers, List<DataTable> tables)
    {
        try
        {
            using (var package = new ExcelPackage())
            {
                for (int x = 0; x < tables.Count; x++)
                {
                    DataTable table = tables[x];
                    ArrayList header = headers[x];
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(table.TableName);

                    int rowSpan = 1;
                    for (int i = 0; i < header.Count; i++)
                    {
                        worksheet.Cells[i + 1, 1].Value = header[i];
                        rowSpan++;
                    }

                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            worksheet.Cells[row + rowSpan, col + 1].Value = table.Rows[row][col].ToString();
                        }
                    }
                }

                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    stream.Position = 0;
                    var response = _httpContextAccessor.HttpContext.Response;
                    response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
                    await response.Body.WriteAsync(stream.ToArray(), 0, (int)stream.Length);
                }
            }
        }
        catch (Exception ex)
        {
            // Log exception
        }
    }
}
