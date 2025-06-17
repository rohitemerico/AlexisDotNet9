//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================
namespace MDM.Windows.Entities;

public class PostWinProfile //: WinMdm.Library.POST.PostWinProfile
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool AllowManualMDMUnenrollment { get; set; }
}
