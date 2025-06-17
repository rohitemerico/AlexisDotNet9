//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class ADCBResp
    {
        [XmlElement]
        public string CLINE { get; set; }

        [XmlElement]
        public string DriverID { get; set; }

        [XmlElement]
        public string Owner { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement]
        public string Status { get; set; }

        [XmlElement]
        public string StartDateTime { get; set; }

        [XmlElement]
        public string EndDateTime { get; set; }

        [XmlElement]
        public string Activity { get; set; }

        [XmlElement]
        public string Type { get; set; }

        [XmlElement]
        public string CustType { get; set; }

        [XmlElement]
        public string CustCategory { get; set; }

        [XmlElement]
        public string CallTransTo { get; set; }

        [XmlElement]
        public string Authenticated { get; set; }

        [XmlElement]
        public WrapUp WrapUpInc { get; set; }

        [XmlElement]
        public WrapUp WrapUpFlag { get; set; }

        [XmlElement]
        public string IVRReq { get; set; }

        [XmlElement]
        public string CreatedBy { get; set; }

        [XmlElement]
        public string WrapUpCat { get; set; }

        [XmlElement]
        public string WrapUpSubCat { get; set; }

        [XmlElement]
        public AuthenticationFlag AuthFlag { get; set; }
    }

    public enum AuthenticationFlag
    {
        NA, Success, Failed
    }

    public enum WrapUp
    {
        Y, N
    }
}
