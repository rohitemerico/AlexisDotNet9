//using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class Base_Dashboard
    {

        private object items;
        [XmlAttribute]
        public object Items
        {
            get { return items; }
            set { items = value; }
        }

        private object items2;
        [XmlAttribute]
        public object Items2
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

        private DateTime declineDate;
        [XmlAttribute]
        public DateTime DeclineDate
        {
            get { return declineDate; }
            set { declineDate = value; }
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

        private Guid declineBy;
        [XmlAttribute]
        public Guid DeclineBy
        {
            get { return declineBy; }
            set { declineBy = value; }
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

    }
}