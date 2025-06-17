using System.Xml.Serialization;

namespace MDM.iOS.Entities.Dashboard
{
    /// <summary>
    /// The entity class for user object. 
    /// </summary>

    [Serializable]
    public class UIUserEn : Base_Dashboard
    {
        private Guid uID;
        [XmlAttribute]
        public Guid UserID
        {
            get
            {
                if (uID == Guid.Empty)
                {
                    uID = Guid.NewGuid();
                }
                return uID;
            }
            set { uID = value; }
        }

        private string uName;
        [XmlAttribute]
        public string UserName
        {
            get { return uName; }
            set { uName = value; }
        }

        private string fullname;
        [XmlAttribute]
        public string FullName
        {
            get { return fullname; }
            set { fullname = value; }
        }

        private string email;
        [XmlAttribute]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string staffID;
        [XmlAttribute]
        public string StaffID
        {
            get { return staffID; }
            set { staffID = value; }
        }

        private string groupID;
        [XmlAttribute]
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        private string tempgid;
        [XmlAttribute]
        public string TempGID
        {
            get { return tempgid; }
            set { tempgid = value; }
        }

        private string uPass;
        [XmlAttribute]
        public string Password
        {
            get { return uPass; }
            set { uPass = value; }
        }


        private Guid rID;
        [XmlAttribute]
        public Guid RoleID
        {
            get { return rID; }
            set { rID = value; }
        }


        private Guid bID;
        [XmlAttribute]
        public Guid BranchID
        {
            get { return bID; }
            set { bID = value; }
        }


        private string uUserType;
        [XmlAttribute]
        public string UserType
        {
            get { return uUserType; }
            set { uUserType = value; }
        }


        private bool uIsLogin;
        [XmlAttribute]
        public bool IsLogin
        {
            get { return uIsLogin; }
            set { uIsLogin = value; }
        }


        private bool uIsReset;
        [XmlAttribute]
        public bool Reset
        {
            get { return uIsReset; }
            set { uIsReset = value; }
        }


        private bool uIsSuspend;
        [XmlAttribute]
        public bool IsSuspend
        {
            get { return uIsSuspend; }
            set { uIsSuspend = value; }
        }


        private int uLoginAttempt;
        [XmlAttribute]
        public int LoginAttempt
        {
            get { return uLoginAttempt; }
            set { uLoginAttempt = value; }
        }


        private DateTime uLastLoginDate;
        [XmlAttribute]
        public DateTime LastLoginDate
        {
            get { return uLastLoginDate; }
            set { uLastLoginDate = value; }
        }

        private string uSessionID;
        [XmlAttribute]
        public string SessionID
        {
            get { return uSessionID; }
            set { uSessionID = value; }
        }


        private DateTime currentDate;
        [XmlAttribute]
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; }
        }
    }
}
