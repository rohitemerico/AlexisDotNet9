//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MCard : Base_Dashboard
    {
        private Guid cardID;
        [XmlAttribute]
        public Guid CID
        {
            get { return cardID; }
            set { cardID = value; }
        }

        private string cardDesc;
        [XmlAttribute]
        public string CDesc
        {
            get { return cardDesc; }
            set { cardDesc = value; }
        }

        private bool cardContactless;
        [XmlAttribute]
        public bool cContactless
        {
            get { return cardContactless; }
            set { cardContactless = value; }
        }

        private string cardType;
        [XmlAttribute]
        public string CType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        private string cardBin;
        [XmlAttribute]
        public string CBin
        {
            get { return cardBin; }
            set { cardBin = value; }
        }

        private string cardMask;
        [XmlAttribute]
        public string CMask
        {
            get { return cardMask; }
            set { cardMask = value; }
        }

    }

}