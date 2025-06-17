using System.Xml.Serialization;

namespace MDM.iOS.Entities.Dashboard
{
    [Serializable]
    public class Base_Dashboard
    {
        private Object items;
        [XmlAttribute]
        public Object Items
        {
            get { return items; }
            set { items = value; }
        }

        private Object items2;
        [XmlAttribute]
        public Object Items2
        {
            get { return items2; }
            set { items2 = value; }
        }

        private DateTime createdDate;
        [XmlAttribute]
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private DateTime approvedDate;
        [XmlAttribute]
        public DateTime ApprovedDate
        {
            get { return approvedDate; }
            set { approvedDate = value; }
        }

        private DateTime declinedDate;
        [XmlAttribute]
        public DateTime DeclinedDate
        {
            get { return declinedDate; }
            set { declinedDate = value; }
        }

        private DateTime updatedDate;
        [XmlAttribute]
        public DateTime UpdatedDate
        {
            get { return updatedDate; }
            set { updatedDate = value; }
        }

        private Guid createdBy;
        [XmlAttribute]
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        private Guid approvedBy;
        [XmlAttribute]
        public Guid ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        private Guid declinedBy;
        [XmlAttribute]
        public Guid DeclinedBy
        {
            get { return declinedBy; }
            set { declinedBy = value; }
        }

        private Guid updatedBy;
        [XmlAttribute]
        public Guid UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        private int status;
        [XmlAttribute]
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private string remarks;
        [XmlAttribute]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        private int returnStatusCode;
        [XmlAttribute]
        public int ReturnStatusCode
        {
            get { return returnStatusCode; }
            set { returnStatusCode = value; }
        }

        private string returnStatus;
        [XmlAttribute]
        public string ReturnStatus
        {
            get { return returnStatus; }
            set { returnStatus = value; }
        }

    }
}
