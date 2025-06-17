using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class ReqHeader
    {
        //[XmlElement]
        //public string usecaseID { get; set; }

        [XmlElement]
        public string StandID { get; set; }

        [XmlElement]
        public string serviceName { get; set; }

        [XmlElement]
        public string versionNo { get; set; }

        [XmlElement]
        public string serviceAction { get; set; }

        [XmlElement]
        public string sysRefNumber { get; set; }

        [XmlElement]
        public string senderID { get; set; }

        [XmlElement]
        public string reqTimeStamp { get; set; }

        //[XmlElement]
        //public string Username { get; set; }

        [XmlElement]
        public string CardType { get; set; }

        [XmlElement]
        public string CardNo { get; set; }

        [XmlElement]
        public string Credentials { get; set; }
    }

    [Serializable]
    public class RespHeader
    {
        //[XmlElement]
        //public string usecaseID { get; set; }

        [XmlElement]
        public string ID { get; set; }


        [XmlElement]
        public string ReferenceNo { get; set; }

        [XmlElement]
        public string senderID { get; set; }

        [XmlElement]
        public string reqTimeStamp { get; set; }

        [XmlElement]
        public string repTimeStamp { get; set; }



        [XmlElement]
        public string returnCode { get; set; }

        [XmlElement]
        public CardType ECardType { get; set; }

        [XmlElement]
        public string CardNo { get; set; }

        [XmlElement]
        public string errorDescription { get; set; }

        [XmlElement]
        public string errorDetail { get; set; }
    }
}
