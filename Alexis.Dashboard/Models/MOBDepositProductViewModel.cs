namespace Alexis.Dashboard.Models;

public class MOBDepositProductViewModel
{
    public DateTime TransDate { get; set; }
    public string TransDesc { get; set; }
    public int TransStatus { get; set; }
    public string UserName { get; set; }
    public string bDesc { get; set; }
    public string TblMachineMachineSerial { get; set; }
    public string TransDateString => TransDate.ToString("MM/dd/yyyy hh:mm:ss tt");
}

public class MSFDepositProductViewModel
{
    public DateTime TransDate { get; set; }
    public string TransDesc { get; set; }
    public int TransStatus { get; set; }
    public string UserName { get; set; }
    public string bDesc { get; set; }
    public string TblMachineMachineSerial { get; set; }
    public int TotalTrans { get; set; }
    public int TotalFailed { get; set; }
    public int TotalPassed { get; set; }
    public string TransDateString => TransDate.ToString("MM/dd/yyyy hh:mm:ss tt");
}