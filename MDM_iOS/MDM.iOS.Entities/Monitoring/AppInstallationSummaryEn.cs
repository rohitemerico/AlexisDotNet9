using System.Xml.Serialization;

namespace MDM.iOS.Entities.Monitoring;
[Serializable]
public class AppInstallationSummaryEn
{
    private string id;
    [XmlAnyAttribute]
    public string ID
    {
        get { return id; }
        set { id = value; }
    }

    private string appID;
    [XmlAnyAttribute]
    public string AppID
    {
        get { return appID; }
        set { appID = value; }
    }

    private string machineID;
    [XmlAnyAttribute]
    public string MachineID
    {
        get { return machineID; }
        set { machineID = value; }
    }

    private string status;
    [XmlAttribute]
    public string Status
    {
        get { return status; }
        set { status = value; }
    }
    private DateTime createdDate;
    [XmlAttribute]
    public DateTime CreatedDate
    {
        get { return createdDate; }
        set { createdDate = value; }
    }

    private Guid createdBy;
    [XmlAttribute]
    public Guid CreatedBy
    {
        get { return createdBy; }
        set { createdBy = value; }
    }

    private string installType;
    [XmlAttribute]
    public string InstallType
    {
        get { return installType; }
        set { installType = value; }
    }
}
