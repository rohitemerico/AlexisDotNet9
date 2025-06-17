using System.Xml.Serialization;

namespace MDM.iOS.Entities.Dashboard
{
    /// <summary>
    /// The entity class for the user roles. 
    /// </summary>

    [Serializable]
    public class UIRoleEn : Base_Dashboard
    {
        private Guid rID;
        [XmlAttribute]
        public Guid RoleID
        {
            get
            {
                if (rID == Guid.Empty)
                {
                    rID = Guid.NewGuid();
                }
                return rID;
            }
            set { rID = value; }
        }


        private string rDesc;
        [XmlAttribute]
        public string RoleDesc
        {
            get { return rDesc; }
            set { rDesc = value; }
        }


        private List<UIPermissionAccessEn> permissionAccessList;
        [XmlAttribute]
        public List<UIPermissionAccessEn> PermissionAccessList
        {
            get
            {
                if (permissionAccessList == null)
                {
                    permissionAccessList = new List<UIPermissionAccessEn>();
                }
                return permissionAccessList;
            }
            set { permissionAccessList = value; }
        }


    }

    public class UIPermissionAccessEn
    {
        private Guid mID;
        [XmlAttribute]
        public Guid MenuID
        {
            get { return mID; }
            set { mID = value; }
        }


        private string mDesc;
        [XmlAttribute]
        public string MenuDesc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }


        private Guid rID;
        [XmlAttribute]
        public Guid RoleID
        {
            get { return rID; }
            set { rID = value; }
        }


        private bool mView;
        [XmlAttribute]
        public bool View
        {
            get { return mView; }
            set { mView = value; }
        }


        private bool mMaker;
        [XmlAttribute]
        public bool Maker
        {
            get { return mMaker; }
            set { mMaker = value; }
        }


        private bool mChecker;
        [XmlAttribute]
        public bool Checker
        {
            get { return mChecker; }
            set { mChecker = value; }
        }
    }
}
