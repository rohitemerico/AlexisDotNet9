using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    /// <summary>
    /// Entity class for managed installed applications by devices. 
    /// </summary>
    [Serializable]
    public class En_MDMApp
    {

        public Guid appId;
        [XmlAttribute]
        private Guid AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        public String name;
        [XmlAttribute]
        private String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String desc;
        [XmlAttribute]
        private String Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        public string version;
        [XmlAttribute]
        private string Version
        {
            get { return version; }
            set { version = value; }
        }

        public int status;
        [XmlAttribute]
        private int Status
        {
            get { return status; }
            set { status = value; }
        }

        public string path;
        [XmlAttribute]
        private string Path
        {
            get { return path; }
            set { path = value; }
        }


        public string identifier;
        [XmlAttribute]
        private string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }


        public string icon_FilePath;
        [XmlAttribute]
        private string Icon_FilePath
        {
            get { return icon_FilePath; }
            set { icon_FilePath = value; }
        }


        public DateTime createdDate;
        [XmlAttribute]
        private DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public DateTime approvedDate;
        [XmlAttribute]
        private DateTime ApprovedDate
        {
            get { return approvedDate; }
            set { approvedDate = value; }
        }

        public DateTime declineDate;
        [XmlAttribute]
        private DateTime DeclineDate
        {
            get { return declineDate; }
            set { declineDate = value; }
        }

        public DateTime updatedDate;
        [XmlAttribute]
        private DateTime UpdatedDate
        {
            get { return updatedDate; }
            set { updatedDate = value; }
        }


        public Guid createdBy;
        [XmlAttribute]
        private Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public Guid approvedBy;
        [XmlAttribute]
        private Guid ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        public Guid declineBy;
        [XmlAttribute]
        private Guid DeclineBy
        {
            get { return declineBy; }
            set { declineBy = value; }
        }

        public Guid updatedBy;
        [XmlAttribute]
        private Guid UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }


        public DateTime expDate;
        [XmlAttribute]
        private DateTime ExpDate
        {
            get { return expDate; }
            set { expDate = value; }
        }
    }
}
