using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM;

[Serializable]
public class En_MDMAppInstallationSummary
{
    private Guid id;
    [XmlAnyAttribute]
    public Guid ID
    {
        get { return id; }
        set { id = value; }
    }

    private Guid appID;
    [XmlAnyAttribute]
    public Guid AppID
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

    private int status;
    [XmlAttribute]
    public int Status
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

    private int installType;
    [XmlAttribute]
    public int InstallType
    {
        get { return installType; }
        set { installType = value; }
    }
}
