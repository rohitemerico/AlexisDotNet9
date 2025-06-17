//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MGroup : Base_Dashboard
    {
        private Guid kID;
        [XmlAttribute]
        public Guid KID
        {
            get { return kID; }
            set { kID = value; }
        }

        private string kDesc;
        [XmlAttribute]
        public string KDesc
        {
            get { return kDesc; }
            set { kDesc = value; }
        }

        private Guid kHopperID;
        [XmlAttribute]
        public Guid KHopperID
        {
            get { return kHopperID; }
            set { kHopperID = value; }
        }

        private string kHopperDesc;
        [XmlAttribute]
        public string KHopperDesc
        {
            get { return kHopperDesc; }
            set { kHopperDesc = value; }
        }

        private Guid kDocumentID;
        [XmlAttribute]
        public Guid KDocumentID
        {
            get { return kDocumentID; }
            set { kDocumentID = value; }
        }

        private string kDocumentDesc;
        [XmlAttribute]
        public string KDocumentDesc
        {
            get { return kDocumentDesc; }
            set { kDocumentDesc = value; }
        }

        private Guid kAlertID;
        [XmlAttribute]
        public Guid KAlertID
        {
            get { return kAlertID; }
            set { kAlertID = value; }
        }

        private string kAlertDesc;
        [XmlAttribute]
        public string KAlertDesc
        {
            get { return kAlertDesc; }
            set { kAlertDesc = value; }
        }

        private Guid kBusinessHourID;
        [XmlAttribute]
        public Guid KBusinessHourID
        {
            get { return kBusinessHourID; }
            set { kBusinessHourID = value; }
        }

        private string kBusinessHourDesc;
        [XmlAttribute]
        public string KBusinessHourDesc
        {
            get { return kBusinessHourDesc; }
            set { kBusinessHourDesc = value; }
        }

        private MAdvertisement kScreenBackground;
        [XmlAttribute]
        public MAdvertisement KScreenBackground
        {
            get { return kScreenBackground; }
            set { kScreenBackground = value; }
        }


        public List<string> AdvIds { get; set; }
        //private ListItemCollection kScreenType;
        //[XmlAttribute]
        //public int KScreenType
        //{
        //    get { return kScreenType; }
        //    set { kScreenType = value; }
        //}

    }

}