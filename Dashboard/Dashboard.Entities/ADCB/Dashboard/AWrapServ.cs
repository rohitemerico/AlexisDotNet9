//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    public enum WrapServType
    {
        Wrap,
        Serv
    }

    [Serializable]
    public class AWrapServ : Base_Dashboard
    {
        private Guid wsID;
        [XmlAttribute]
        public Guid WSID
        {
            get { return wsID; }
            set { wsID = value; }
        }

        private string wsMainName;
        [XmlAttribute]
        public string WSMainName
        {
            get { return wsMainName; }
            set { wsMainName = value; }
        }

        private WrapServType wsType;
        [XmlAttribute]
        public WrapServType WSType
        {
            get { return wsType; }
            set { wsType = value; }
        }

        private WrapServDetails detail;
        [XmlAttribute]
        public WrapServDetails Detail
        {
            get
            {
                if (detail == null)
                    detail = new WrapServDetails();
                return detail;
            }
            set { detail = value; }
        }

        private List<WrapServDetails> detailList;
        [XmlAttribute]
        public List<WrapServDetails> DetailList
        {
            get
            {
                if (detailList == null)
                    detailList = new List<WrapServDetails>();

                return detailList;
            }
            set { detailList = value; }
        }

        [Serializable]
        public class WrapServDetails
        {
            private Guid ref_wsID;
            [XmlAttribute]
            public Guid Ref_wsID
            {
                get { return ref_wsID; }
                set { ref_wsID = value; }
            }

            private Guid wsID_Detail;
            [XmlAttribute]
            public Guid WSID_Detail
            {
                get { return wsID_Detail; }
                set { wsID_Detail = value; }
            }

            private string wsDetailName;
            [XmlAttribute]
            public string WSDetailName
            {
                get { return wsDetailName; }
                set { wsDetailName = value; }
            }
        }


    }

}