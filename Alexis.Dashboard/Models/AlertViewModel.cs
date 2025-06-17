namespace Alexis.Dashboard.Models;

public class AlertViewModel
{
    public string aID { get; set; }
    public string aDesc { get; set; }
    public decimal aMinCardBal { get; set; }
    public decimal aMinChequeBal { get; set; }
    public decimal aMinPaperBal { get; set; }
    public decimal aMinRejCardBal { get; set; }
    public DateTime aCreatedDate { get; set; }

    public string CreatedDateString => aCreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
    public string CreatedName { get; set; }
    public DateTime? aUpdatedDate { get; set; }
    public string UpdatedDateString => aUpdatedDate?.ToString("MM/dd/yyyy hh:mm:ss tt");
    public string? updatedby { get; set; }
    public string AlertStatus { get; set; }

    public bool Visible { get; set; } = true;
    public bool ErrorVisible { get; set; } = false;

    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowDecline { get; set; }
    public bool AllowView { get; set; }

    // Nested View – People In Charge
    public string aCardEmail { get; set; }
    public string aCardSms { get; set; }
    public decimal aCardTInterval { get; set; }

    public string aChequeEmail { get; set; }
    public string aChequeSms { get; set; }
    public decimal aChequeTInterval { get; set; }

    public string aMaintenanceEmail { get; set; }
    public string aMaintenanceSms { get; set; }
    public decimal aMaintenanceTInterval { get; set; }

    public string aSecurityEmail { get; set; }
    public string aSecuritySms { get; set; }
    public decimal aSecurityTInterval { get; set; }

    public string aTroubleshootEmail { get; set; }
    public string aTroubleshootSms { get; set; }
    public decimal aTroubleshootTInterval { get; set; }

    // Nested View – Low Cartridge Balance
    public decimal AMINRIBFRONTBAL { get; set; }
    public decimal AMINRIBREARBAL { get; set; }
    public decimal AMINRIBTIPBAL { get; set; }
    public decimal AMINCHEQUEPRINTCATRIDGE { get; set; }
    public decimal AMINCATRIDGEBAL { get; set; }
}
