namespace Alexis.Dashboard.Models;

public class FaceAPISummaryViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string UName { get; set; }
    public string ApiCallType { get; set; }
    public int TokenUsed { get; set; }
    public DateTime DateUsed { get; set; }
    public string Description { get; set; }
}

public class OCRAPISummaryViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string UName { get; set; }
    public string ApiCallType { get; set; }
    public int TokenUsed { get; set; }
    public DateTime DateUsed { get; set; }
    public string Description { get; set; }
}

public class DocumentAPISummaryViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string UName { get; set; }
    public string ApiCallType { get; set; }
    public int TokenUsed { get; set; }
    public DateTime DateUsed { get; set; }
    public string Description { get; set; }
}