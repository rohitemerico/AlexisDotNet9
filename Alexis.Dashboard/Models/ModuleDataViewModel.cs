namespace Alexis.Dashboard.Models;

public class ModuleDataViewModel
{
    public string mID { get; set; }
    public string? child { get; set; }
    public string? parent { get; set; }
    public decimal mView { get; set; }
    public decimal mMaker { get; set; }
    public decimal mChecker { get; set; }
}
