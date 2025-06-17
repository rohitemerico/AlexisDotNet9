using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class ASecPayCheque : Base_Dashboard
    {
        private Guid sID;
        [XmlAttribute]
        public Guid SID
        {
            get { return sID; }
            set { sID = value; }
        }

        private string chequePayTo;
        [XmlAttribute]
        public string ChequePayTo
        {
            get { return chequePayTo; }
            set { chequePayTo = value; }
        }
    }
}
