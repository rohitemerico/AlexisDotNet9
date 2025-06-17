//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class AMasterSettings : Base_Dashboard
    {
        private int monthChequeMax;
        [XmlAttribute]
        public int MonthChequeMax
        {
            get { return monthChequeMax; }
            set { monthChequeMax = value; }
        }

        private string vdns;
        [XmlAttribute]
        public string VDNs
        {
            get { return vdns; }
            set { vdns = value; }
        }

        private string english;
        [XmlAttribute]
        public string English
        {
            get { return english; }
            set { english = value; }
        }

        private string arabic;
        [XmlAttribute]
        public string Arabic
        {
            get { return arabic; }
            set { arabic = value; }
        }

        private string pword;
        [XmlAttribute]
        public string PWord
        {
            get { return pword; }
            set { pword = value; }
        }
    }

}