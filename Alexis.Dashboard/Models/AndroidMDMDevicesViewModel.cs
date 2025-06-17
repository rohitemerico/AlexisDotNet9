namespace Alexis.Dashboard.Models;

public class AndroidMDMDevicesViewModel
{
    public int ID { get; set; }
    public string deviceMACAdd { get; set; }
    public string DEVICENAME { get; set; }
    public bool DEVICESTATUS { get; set; }
    public int CONNECTIONSTATUS { get; set; }
    public int TOUCHSCREENSTATUS { get; set; }
    public decimal CARDREADERSPERDAY { get; set; }
    public Decimal latitude { get; set; }
    public Decimal longitude { get; set; }
    public int BATTERYLEVEL { get; set; }
    public DateTime enrollDatetime { get; set; }
    public DateTime lastSyncDatetime { get; set; }
    public string GROUPNAME { get; set; }
    public decimal Restriction_LATITUDE { get; set; }
    public decimal Restriction_LONGITUDE { get; set; }
    public int Restriction_radius { get; set; }
}