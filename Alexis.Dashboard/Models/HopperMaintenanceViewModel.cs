namespace Alexis.Dashboard.Models;

public class HopperMaintenanceViewModel
{
    public string hID { get; set; }
    public string hDesc { get; set; }
    public string hStatus { get; set; }
    public DateTime hCreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? hUpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public bool Visible { get; set; } = true;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }

    public string h1Temp { get; set; }
    public string h2Temp { get; set; }
    public string h3Temp { get; set; }
    public string h4Temp { get; set; }
    public string h5Temp { get; set; }
    public string h6Temp { get; set; }
    public string h7Temp { get; set; }
    public string h8Temp { get; set; }
    public string HC1 { get; set; }
    public string HC2 { get; set; }
    public string HC3 { get; set; }
    public string HC4 { get; set; }
    public string HC5 { get; set; }
    public string HC6 { get; set; }
    public string HC7 { get; set; }
    public string HC8 { get; set; }
    public string h1Range { get; set; }
    public string h2Range { get; set; }
    public string h3Range { get; set; }
    public string h4Range { get; set; }
    public string h5Range { get; set; }
    public string h6Range { get; set; }
    public string h7Range { get; set; }
    public string h8Range { get; set; }
    public string h1Mask { get; set; }
    public string h2Mask { get; set; }
    public string h3Mask { get; set; }
    public string h4Mask { get; set; }
    public string h5Mask { get; set; }
    public string h6Mask { get; set; }
    public string h7Mask { get; set; }
    public string h8Mask { get; set; }

    public string Hopper1 => "Hopper 1 - " + HC1 + " - " + h1Range + " - " + h1Mask;
    public string Hopper2 => "Hopper 2 - " + HC2 + " - " + h2Range + " - " + h2Mask;
    public string Hopper3 => "Hopper 3 - " + HC3 + " - " + h3Range + " - " + h3Mask;
    public string Hopper4 => "Hopper 4 - " + HC4 + " - " + h4Range + " - " + h4Mask;
    public string Hopper5 => "Hopper 5 - " + HC5 + " - " + h5Range + " - " + h5Mask;
    public string Hopper6 => "Hopper 6 - " + HC6 + " - " + h6Range + " - " + h6Mask;
    public string Hopper7 => "Hopper 7 - " + HC7 + " - " + h7Range + " - " + h7Mask;
    public string Hopper8 => "Hopper 8 - " + HC8 + " - " + h8Range + " - " + h8Mask;
}