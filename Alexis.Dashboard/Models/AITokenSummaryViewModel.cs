namespace Alexis.Dashboard.Models;

public class AITokenSummaryViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string UNAME { get; set; }
    public string TokenType { get; set; }
    public int TokenCount { get; set; }
    public DateTime DateUsed { get; set; }
    public string Description { get; set; }
}
public class AITokenDetailsViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string UNAME { get; set; }
    public string TokenType { get; set; }
    public int TokenCount { get; set; }
    public DateTime DateUsed { get; set; }
    public string Description { get; set; }
}
