//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class Role : Base_Dashboard
    {
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

        private RolePermission permission;
        [XmlAttribute]
        public RolePermission Permission
        {
            get { return permission; }
            set { permission = value; }
        }

        private List<RolePermission> permissionList;
        [XmlAttribute]
        public List<RolePermission> PermissionList
        {
            get
            {
                if (permissionList == null)
                    permissionList = new List<RolePermission>();

                return permissionList;
            }
            set { permissionList = value; }
        }

        [Serializable]
        public class RolePermission
        {
            private Guid roleRefID;
            [XmlAttribute]
            public Guid RoleRefID
            {
                get { return roleRefID; }
                set { roleRefID = value; }
            }

            private Guid rID;
            [XmlAttribute]
            public Guid RoleID
            {
                get { return rID; }
                set { rID = value; }
            }

            private Guid menuID;
            [XmlAttribute]
            public Guid MenuID
            {
                get { return menuID; }
                set { menuID = value; }
            }

            private string mDesc;
            [XmlAttribute]
            public string MenuDesc
            {
                get { return mDesc; }
                set { mDesc = value; }
            }

            private bool mView;
            [XmlAttribute]
            public bool MView
            {
                get { return mView; }
                set { mView = value; }
            }

            private bool mCheck;
            [XmlAttribute]
            public bool MCheck
            {
                get { return mCheck; }
                set { mCheck = value; }
            }

            private bool mMake;
            [XmlAttribute]
            public bool MMake
            {
                get { return mMake; }
                set { mMake = value; }
            }
        }

    }
}