using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard;

[Serializable]
public class User : Base_Dashboard
{
    private Guid userID;
    [XmlAttribute]
    public Guid UserID
    {
        get { return userID; }
        set { userID = value; }
    }

    private string userName;
    [XmlAttribute]
    public string UserName
    {
        get { return userName.ToUpper(); }
        set { userName = value; }
    }

    private string userFullName;
    [XmlAttribute]
    public string UserFullName
    {
        get { return userFullName; }
        set { userFullName = value; }
    }

    private Guid roleID;
    [XmlAttribute]
    public Guid RoleID
    {
        get { return roleID; }
        set { roleID = value; }
    }

    private string roleDesc;
    [XmlAttribute]
    public string RoleDesc
    {
        get { return roleDesc; }
        set { roleDesc = value; }
    }

    private string userCMID;
    [XmlAttribute]
    public string UserCMID
    {
        get { return userCMID; }
        set { userCMID = value; }
    }

    private bool agentFlag;
    [XmlAttribute]
    public bool AgentFlag
    {
        get { return agentFlag; }
        set { agentFlag = value; }
    }

    private bool mCardReplenishment;
    [XmlAttribute]
    public bool CardReplenishment
    {
        get { return mCardReplenishment; }
        set { mCardReplenishment = value; }
    }
    private bool mChequeReplenishment;
    [XmlAttribute]
    public bool ChequeReplenishment
    {
        get { return mChequeReplenishment; }
        set { mChequeReplenishment = value; }
    }
    private bool mConsumableCollection;
    [XmlAttribute]
    public bool ConsumableCollection
    {
        get { return mConsumableCollection; }
        set { mConsumableCollection = value; }
    }
    private bool mSecurity;
    [XmlAttribute]
    public bool Security
    {
        get { return mSecurity; }
        set { mSecurity = value; }
    }
    private bool mTroubleshoot;
    [XmlAttribute]
    public bool Troubleshoot
    {
        get { return mTroubleshoot; }
        set { mTroubleshoot = value; }
    }

    private bool mLocalUser;
    [XmlAttribute]
    public bool LocalUser
    {
        get { return mLocalUser; }
        set { mLocalUser = value; }
    }

    private string mPassword;
    [XmlAttribute]
    public string Password
    {
        get { return mPassword; }
        set { mPassword = value; }
    }
    private string mEmail;
    [XmlAttribute]
    public string Email
    {
        get { return mEmail; }
        set { mEmail = value; }
    }

}

[Serializable]
public class LocalUser : Base_Dashboard
{
    private Guid userID;
    [XmlAttribute]
    public Guid UserID
    {
        get { return userID; }
        set { userID = value; }
    }

    private string _password;
    [XmlAttribute]
    public string Password
    {
        get { return _password; }
        set { _password = value; }
    }

    private string email;
    [XmlAttribute]
    public string Email
    {
        get { return email; }
        set { email = value; }
    }
}