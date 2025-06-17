namespace Alexis.Dashboard.Models;

public class ModuleViewModel
{
    public string mID { get; set; }
    public string? child { get; set; }
    public string? parent { get; set; }
    public bool mView { get; set; }
    public bool mMaker { get; set; }
    public bool mChecker { get; set; }
}