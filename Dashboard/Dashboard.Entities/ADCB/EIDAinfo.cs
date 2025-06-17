//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class EIDAinfo
    {
        [XmlElement]
        public string reqTimeStamp { get; set; }
        [XmlElement]
        public string ReferenceNo { get; set; }
        [XmlElement]
        public string SenderID { get; set; }
        [XmlElement]
        public string RepTimeStamp { get; set; }

        [XmlElement]
        public string IDNo { get; set; }
        [XmlElement]
        public string CardNo { get; set; }
        [XmlElement]
        public object IDPhoto { get; set; }
        [XmlElement]
        public string Title { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public object Sex { get; set; }
        [XmlElement]
        public string Nationality { get; set; }

        [XmlElement]
        public string IssueDate { get; set; }
        [XmlElement]
        public string ExpiryDate { get; set; }
        [XmlElement]
        public string DOB { get; set; }
        [XmlElement]
        public Sex Gender { get; set; }

        [XmlElement]
        public string Marital { get; set; }
        [XmlElement]
        public string Occup { get; set; }
        [XmlElement]
        public string MotherName { get; set; }

        [XmlElement]
        public string HusbandID { get; set; }
        [XmlElement]
        public string ResidentType { get; set; }
        [XmlElement]
        public string ResidentNo { get; set; }
        [XmlElement]
        public string ResidentExpiry { get; set; }
        [XmlElement]
        public string PassportNo { get; set; }
        [XmlElement]
        public string PassportType { get; set; }
        [XmlElement]
        public string PassportCountry { get; set; }
        [XmlElement]
        public string PassportExpiryDate { get; set; }
        [XmlElement]
        public string PlaceOfBirth { get; set; }

        [XmlElement]
        public string errorcode { get; set; }
        [XmlElement]
        public string errorDecription { get; set; }
    }

    public enum Sex
    {
        M, F, X
    }

    public enum CardType
    {
        Credit, Debit
    }
}