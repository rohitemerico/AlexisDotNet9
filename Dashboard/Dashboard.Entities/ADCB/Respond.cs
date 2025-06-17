using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class Respond
    {
        private List<Biller> billingItems;
        [XmlArray]
        public List<Biller> BillingItems
        {
            get { return billingItems; }
            set { billingItems = value; }
        }

        private int errorCode;
        [XmlAttribute]
        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        private string errorDesc;
        [XmlAttribute]
        public string ErrorDesc
        {
            get { return errorDesc; }
            set { errorDesc = value; }
        }

        private string[] accNO;
        [XmlAttribute]
        public string[] AccNO
        {
            get { return accNO; }
            set { accNO = value; }
        }

        private string nRIC;
        [XmlAttribute]
        public string NRIC
        {
            get { return nRIC; }
            set { nRIC = value; }
        }

        private string tranID;
        [XmlAttribute]
        public string TranID
        {
            get { return tranID; }
            set { tranID = value; }
        }

    }
}
